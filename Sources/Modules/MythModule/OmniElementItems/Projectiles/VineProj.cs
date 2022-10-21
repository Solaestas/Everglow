﻿using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.OmniElementItems.Projectiles
{
    public class VineProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Projectile.extraUpdates = 1;
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.hostile = false;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 300;
            ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 60;
        }
        internal Vector2 StartPos = Vector2.Zero; 

        public override void AI()
        {
            Projectile.hide = true;
            Player player = Main.player[Projectile.owner];
            if (StartPos == Vector2.Zero)
            {
                StartPos = player.Center;
            }
            float colorLight = Math.Min(Projectile.timeLeft / 100f, 1f);
            if (Projectile.timeLeft < 75)
            {
                if (Projectile.ai[0] > 50)//0~100
                {
                    Projectile.velocity = Projectile.velocity.RotatedBy(Math.PI / -20f);
                    Projectile.velocity *= 0.975f;
                    Lighting.AddLight(Projectile.Center, colorLight * 0.0f , colorLight * 0.9f , colorLight * 0.0f );
                }
                else
                {
                    Projectile.velocity = Projectile.velocity.RotatedBy(Math.PI / 20f);
                    Projectile.velocity *= 0.975f;
                    Lighting.AddLight(Projectile.Center, colorLight * 0.0f , colorLight * 0.9f , colorLight * 0.0f );
                }
            }
            else
            {
                if((Projectile.Center - StartPos).Length() >= 100)
                {
                    Projectile.timeLeft -= 5;
                }
                Projectile.ai[1] += 1 / 30f;//0.0~2.0
                Projectile.velocity = Projectile.velocity.RotatedBy(Math.PI / 60f * (float)Math.Sin(Projectile.ai[1] * Math.PI));
                Lighting.AddLight(Projectile.Center, 0, colorLight * 0.9f , 0);
                if (Main.rand.NextBool(40))
                {
                    int VineP = Projectile.NewProjectile(null, Projectile.Center + player.Center - StartPos, Projectile.velocity, ModContent.ProjectileType<VineProj2>(), 50, 2f, Main.myPlayer, Main.rand.Next(100), Main.rand.NextFloat(0, 2f));
                    Main.projectile[VineP].timeLeft = Main.rand.Next(70, 140);
                }
            }
        }
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            float colorLight = Math.Min(Projectile.timeLeft / 100f, 1f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> bars = new List<Vertex2D>();
            float width = 6;
            if (Projectile.timeLeft < 60)
            {
                width = Projectile.timeLeft / 10f;
            }
            int TrueL = 0;
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                TrueL++;
            }
            for (int i = 1; i < Projectile.oldPos.Length; ++i)
            {
                if (Projectile.oldPos[i] == Vector2.Zero)
                    break;
                var normalDir = Projectile.oldPos[i - 1] - Projectile.oldPos[i];
                normalDir = Vector2.Normalize(new Vector2(-normalDir.Y, normalDir.X));
                var factor = 1f;
                if (Projectile.oldPos.Length > 0)
                {
                    factor = i / (float)Projectile.oldPos.Length;
                }
                var w = MathHelper.Lerp(1f, 0.05f, factor);
                Lighting.AddLight(Projectile.oldPos[i], colorLight * 1.2f * (1 - factor), colorLight * 0.7f * (1 - factor), 0);
                Vector2 DrawPos = player.Center + Projectile.oldPos[i] - StartPos + new Vector2(4) - Main.screenPosition;
                bars.Add(new Vertex2D(DrawPos + normalDir * width, new Color(0.01f, 0.1f, 0.05f, 0), new Vector3(factor + 0.008f, 1, w)));
                bars.Add(new Vertex2D(DrawPos - normalDir * width, new Color(0.01f, 0.1f, 0.05f, 0), new Vector3(factor + 0.008f, 0, w)));
            }
            List<Vertex2D> Vx = new List<Vertex2D>();
            if (bars.Count > 2)
            {
                Vx.Add(bars[0]);
                var vertex = new Vertex2D((bars[0].position + bars[1].position) * 0.5f + (bars[0].position - bars[1].position).RotatedBy(-Math.PI / 2) * 1f, new Color(254, 254, 254, 0), new Vector3(1f, 0.5f, 1));
                Vx.Add(bars[1]);
                Vx.Add(vertex);
                for (int i = 0; i < bars.Count - 2; i += 2)
                {
                    Vx.Add(bars[i]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 1]);

                    Vx.Add(bars[i + 1]);
                    Vx.Add(bars[i + 2]);
                    Vx.Add(bars[i + 3]);
                }
            }
            if (Vx.Count > 2)
            {
                Texture2D t = MythContent.QuickTexture("OmniElementItems/Projectiles/VineLine");
                Main.graphics.GraphicsDevice.Textures[0] = t;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
            }

            //Rectangle DestR = Projectile.Hitbox;
            //DestR.X -= (int)Main.screenPosition.X;
            //DestR.Y -= (int)Main.screenPosition.Y;
            //Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, DestR, new Color(200, 50, 0, 0));
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            behindProjectiles.Add(index);
            base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
        }
    }
}
