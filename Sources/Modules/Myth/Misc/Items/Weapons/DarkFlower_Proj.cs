using System;
using System.Collections.Generic;
using System.Net.Http;
using Everglow.Common.VFX.CommonVFXDusts;
using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
namespace Everglow.Myth.Misc.Items.Weapons;

public class DarkFlower_Proj : ModProjectile
{
    public override string Texture => "Terraria/Images/Projectile_" + ProjectileID.Fireball;

    private Projectile projectile
    {
        get => Projectile;
    }

    public override void SetStaticDefaults()
    {
        ProjectileID.Sets.DrawScreenCheckFluff[projectile.type] += 500;
    }

    public override void SetDefaults()
    {
        projectile.width = 16;
        projectile.height = 16;
        projectile.aiStyle = -1;
        projectile.hostile = false;
        projectile.friendly = true;
        projectile.timeLeft = 800;
        projectile.ignoreWater = true;
        projectile.tileCollide = true;
        projectile.penetrate = 2;
        projectile.scale = 1.5f;
        projectile.usesLocalNPCImmunity = true;
        projectile.localNPCHitCooldown = 10;
        oldPos = new Vector2[25];
        projectile.DamageType = DamageClass.Magic;
        //projectile.hide = true;

    }
    private Player player => Main.player[projectile.owner];

    private Vector2[] oldPos;

    private void Pre_Kill()
    {
        if (projectile.timeLeft > 10)
        {
            projectile.timeLeft = 10;
            projectile.friendly = false;
            FogVFX fog = MEACVFX.Create<FogVFX>(projectile.Center + Main.rand.NextVector2Circular(20, 20), Main.rand.NextVector2Circular(2, 2) + projectile.velocity * 0.1f, 0);
            fog.substract = true;
            fog.drawColor = new Color(0.3f, 0.6f, 0.3f, 1f);
            fog.SetTimeleft(Main.rand.Next(50, 80));
            fog.scale = 0.7f * projectile.scale * Main.rand.NextFloat(1f, 2f);
            fog.ai0 = 1;
        }
    }
    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        Pre_Kill();
        for (int i = 0; i < 10; i++)
        {
            Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 29, 0, 0, 0, default, 2);
            d.velocity += Main.rand.NextVector2Circular(5, 5);
            d.noGravity = true;
            d.noLight = Main.rand.NextBool();
        }
        /*
        SoundStyle sound = SoundID.NPCDeath44;
        sound.Volume = 0.2f;
        sound.Pitch = -0.2f;
        SoundEngine.PlaySound(sound, projectile.Center);//命中音效*/

    }
    public override bool TileCollideStyle(ref int width, ref int height, ref bool fallThrough, ref Vector2 hitboxCenterFrac)
    {
        fallThrough = false;
        return true;
    }
    public override void OnKill(int timeLeft)
    {


    }
    public override bool OnTileCollide(Vector2 oldVelocity)
    {

        if (projectile.velocity.Y != oldVelocity.Y)
        {
            projectile.velocity.Y = -oldVelocity.Y * 0.7f;
            projectile.velocity.X = projectile.velocity.X * 0.9f;
        }

        else if (projectile.velocity.X != oldVelocity.X)
        {
            projectile.velocity.X = -oldVelocity.X * 0.8f;
        }
        if (++projectile.ai[1] > 12)
            Pre_Kill();
        return false;
    }
    NPC npcTarget = null;
    float alpha = 1f;
    public override void AI()
    {
        if (projectile.timeLeft < 10)
        {
            alpha -= 0.1f;
            projectile.velocity *= 0.6f;
        }
        projectile.rotation += 0.2f * (projectile.velocity.X > 0 ? 1 : -1);

        if (Main.rand.NextBool(2))
        {
            Dust d = Dust.NewDustDirect(projectile.position, projectile.width, projectile.height, 29, 0, 0,0,default,1.2f);
            d.noLight = true;
            d.velocity = projectile.velocity*0.5f;
        }
        if (projectile.timeLeft % 5 == 0 && projectile.ai[0] == 0)
        {
            float maxDis = 200;
            NPC target = null;
            foreach (NPC npc in Main.npc)
            {
                float dis = Vector2.Distance(npc.Center, projectile.Center);
                if (dis < maxDis && npc.CanBeChasedBy(null, true))
                {
                    maxDis = dis;
                    target = npc;
                }
            }
            if (target != null)
            {
                projectile.ai[0] = 1;
                npcTarget = target;
            }
        }
        if (projectile.ai[0] == 1)
        {
            if (npcTarget != null && npcTarget.CanBeChasedBy())
            {
                projectile.tileCollide = false;
                projectile.velocity = Vector2.Lerp(projectile.velocity, projectile.DirectionTo(npcTarget.Center) * 20, 0.06f);
            }
        }
        else
        {
            projectile.velocity.Y += 0.2f;
        }
        ProjectileUtils.TrackOldValue(oldPos, projectile.Center);
    }

    public override bool PreDraw(ref Color lightColor)
    {
        Texture2D tex = Terraria.GameContent.TextureAssets.Projectile[projectile.type].Value;
        Color c = new Color(1f, 0.5f, 0.2f) * alpha;
        List<Vertex2D> bars = new List<Vertex2D>
            {
                new Vertex2D(projectile.Center-Main.screenPosition, Color.Red,new Vector3(0, 0.5f, 1.3f))
            };
        float counts = oldPos.Length;

        for (int i = 0; i < oldPos.Length - 1; ++i)
        {
            if (oldPos[i + 1] == Vector2.Zero)
            {
                break;
            }
            float t = projectile.timeLeft * 0.04f;
            var normalDir = oldPos[i] - oldPos[i + 1];
            normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
            var factor = i / (float)counts;
            var w = MathHelper.Lerp(1f, 0.1f, factor);
            float width = MathHelper.Lerp(20, 0, factor);
            bars.Add(new Vertex2D(oldPos[i] - Main.screenPosition + normalDir * width, c * w, new Vector3((float)Math.Sqrt(factor) + t, 1, w)));
            bars.Add(new Vertex2D(oldPos[i] - Main.screenPosition + normalDir * -width, c * w, new Vector3((float)Math.Sqrt(factor) + t, 0, w)));
        }
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Immediate, CustomBlendStates.Subtract, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);

        Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>("Terraria/Images/Extra_189", AssetRequestMode.ImmediateLoad).Value;
        Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail.Value;
        if (bars.Count >= 3)
        {
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
        }
        Main.spriteBatch.End();
        Main.spriteBatch.Begin(SpriteSortMode.Deferred, CustomBlendStates.Subtract, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.Transform);

        DrawOnCenter(tex, projectile.Center - Main.screenPosition, new Color(0.5f, 1f, 0.8f) * alpha, projectile.rotation, projectile.scale);

        Main.spriteBatch.End();
        Main.spriteBatch.Begin(sBS);

        return false;
    }
    public void DrawOnCenter(Texture2D tex, Vector2 pos, Color color, float rotation, float scale)
    {
        Main.spriteBatch.Draw(tex, pos, null, color, rotation, tex.Size() / 2, scale, 0, 0);
    }
}
