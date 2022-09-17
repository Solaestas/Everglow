using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Everglow.Sources.Commons.Core.VFX;
using Terraria.GameContent;
using Terraria.Audio;
namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.DemonScythe
{
    internal class DemonScythePlus : ModProjectile, IWarpProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 60;
            Projectile.height = 60;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = 6;
            Projectile.timeLeft = 10000;
            Projectile.DamageType = DamageClass.MagicSummonHybrid;
            Projectile.tileCollide = false;

        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            if (Timer < 30)
            {
                Timer+= 2;
            }
            if(Projectile.velocity.Length() < 48f)
            {
                Projectile.velocity *= 1.05f;
            }
            float vL = Projectile.velocity.Length() * 0.1f;
            vL = Math.Min(vL, 4f);
            float kSize = Math.Min(vL , 1f);
            for(float x = -vL;x < vL + 1; x += 1)
            {
                float size = Main.rand.NextFloat(1.45f, 1.75f) * kSize;
                Vector2 lineVel = new Vector2(0, 24).RotatedBy(Math.PI * 0.5 - Main.time / 1.8 + x / 2d);
                lineVel = RotAndEclipse(lineVel);
                Dust d0 = Dust.NewDustDirect(Projectile.Center + lineVel - new Vector2(size * 4, size * 4.5f), 0, 0, ModContent.DustType<Dusts.DemoFlame>(), 0, 0, 0, default(Color), size);
                d0.fadeIn = 12f;
                Vector2 lineVel2 = new Vector2(0, 24).RotatedBy(Math.PI * 1 - Main.time / 1.8 + x / 2d);
                lineVel2 = RotAndEclipse(lineVel2);
                d0.velocity = Projectile.velocity + lineVel2 * 0.1f;
            }
            if(Collision.SolidCollision(Projectile.Center,0,0))
            {
                Projectile.Kill();
            }
            
        }
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCHit4, Projectile.Center);
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - Projectile.velocity * 2, Vector2.One, ModContent.ProjectileType<DemoSpark>(), Projectile.damage / 2, 0, Projectile.owner, 0.5f * Projectile.velocity.Length());
            base.Kill(timeLeft);
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            knockback = Projectile.knockBack * Projectile.velocity.Length() * 0.2f;
            Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center - Projectile.velocity * 2, Vector2.One, ModContent.ProjectileType<DemoSpark>(), Projectile.damage / 2, 0, Projectile.owner, 0.5f * Projectile.velocity.Length());
            float k = Math.Clamp(Projectile.velocity.Length() / 8f,1f, 5f);
            damage = (int)(damage * k);
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
           overWiresUI.Add(index);
        }
        public override bool PreDraw(ref Color lightColor)
        {
            Projectile.hide = false;
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineBlackShade"), new Color(1f, 1f, 1f, 1f));
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineBlackShade"), new Color(1f, 1f, 1f, 1f));
            DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine"), new Color(0.4f, 0.0f, 0.8f, 0));
            return false;
        }
        internal int Timer = 0;
        public void DrawMagicArray(Texture2D tex, Color c0)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D Water = tex;
            Color c1 = new Color(c0.R * 0.19f / 255f, c0.G * 0.19f / 255f, c0.B * 0.19f / 255f, c0.A * 0.19f / 255f);
            Color c2 = new Color(c0.R * 0.09f / 255f, c0.G * 0.09f / 255f, c0.B * 0.09f / 255f, c0.A * 0.09f / 255f); 
            float Size0 = (float)(Math.Sin(Main.time / 12) / 7d + 1);
            float Size1 = (float)(Math.Sin((Main.time + 40) / 24) / 7d + 1);
            float Size2 = Timer / 30f;
            DrawTexCircle(24, 25 * Size2, c0 * Size1, Projectile.Center - Main.screenPosition, Water, -Main.time / 7);
            DrawTexCircle(22, 12 * Size2, c1 * Size1, Projectile.Center - Main.screenPosition, Water, -Main.time / 27);
            DrawTexCircle(20, 12 * Size2, c2 * Size1, Projectile.Center - Main.screenPosition , Water, -Main.time / 127);
            DrawTexMoon(24, 25 * Size2, c0 * Size1, Projectile.Center - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/BloomLight"), -Main.time / 1.8);

        }

        private void DrawTexCircle(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious / 2; h++)
            {
                circle.Add(new Vertex2D(center + RotAndEclipse(new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot)), color, new Vector3(h * 2 / radious, 1, 0)));
                circle.Add(new Vertex2D(center + RotAndEclipse(new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 4 + addRot)), color, new Vector3(h * 2 / radious, 0, 0)));
            }
            circle.Add(new Vertex2D(center + RotAndEclipse(new Vector2(0, radious).RotatedBy(addRot)), color, new Vector3(0.5f, 1, 0)));
            circle.Add(new Vertex2D(center + RotAndEclipse(new Vector2(0, radious + width).RotatedBy(addRot)), color, new Vector3(0.5f, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        private void DrawTexMoon(float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
        {
            List<Vertex2D> circle = new List<Vertex2D>();
            for (int h = 0; h < radious * 5; h++)
            {
                Vector2 up = new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 0.27 + addRot);
                Vector2 down = new Vector2(0, radious + width).RotatedBy(h / radious * Math.PI * 0.27 + addRot);
                up = RotAndEclipse(up);
                down = RotAndEclipse(down);
                circle.Add(new Vertex2D(center + up, color, new Vector3(h * 0.2f / radious, 1, 0)));
                circle.Add(new Vertex2D(center + down, color, new Vector3(h * 0.2f / radious, 0, 0)));
            }
            //circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0.5f, 1, 0)));
            //circle.Add(new Vertex2D(center + new Vector2(0, radious + width).RotatedBy(addRot), color, new Vector3(0.5f, 0, 0)));
            if (circle.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, circle.ToArray(), 0, circle.Count - 2);
            }
        }
        private Vector2 RotAndEclipse(Vector2 orig)
        {
            return new Vector2(orig.X, orig.Y * 0.6f).RotatedBy(Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X));
        }
        public void DrawTexLine(Vector2 StartPos, Vector2 EndPos, Color color1, Color color2, Texture2D tex)
        {
            float Wid = 6f;
            Vector2 Width = Vector2.Normalize(StartPos - EndPos).RotatedBy(Math.PI / 2d) * Wid;

            List<Vertex2D> vertex2Ds = new List<Vertex2D>();

            for (int x = 0; x < 3; x++)
            {
                float Value0 = (float)(Main.time / 291d + 20) % 1f;
                float Value1 = (float)(Main.time / 291d + 20.03) % 1f;
                vertex2Ds.Add(new Vertex2D(StartPos + Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));

                vertex2Ds.Add(new Vertex2D(EndPos + Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 0, 0)));
                vertex2Ds.Add(new Vertex2D(EndPos - Width + new Vector2(x / 3f).RotatedBy(x), color2, new Vector3(Value1, 1, 0)));
                vertex2Ds.Add(new Vertex2D(StartPos - Width + new Vector2(x / 3f).RotatedBy(x), color1, new Vector3(Value0, 1, 0)));
            }


            Main.graphics.GraphicsDevice.Textures[0] = tex;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
        }
        public void DrawWarp()
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.AnisotropicWrap, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Effect KEx = ModContent.Request<Effect>("Everglow/Sources/Modules/MEACModule/Effects/DrawWarp", ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            KEx.CurrentTechnique.Passes[0].Apply();
            Player player = Main.player[Projectile.owner];
            //DrawTexCircle(Timer * 1.2f, 52, new Color(64, 70, 255, 0), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine"), Main.time / 17);
            DrawTexMoon(34, 35, new Color(64, 70, 255, 0), Projectile.Center - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/BloomLight"), Main.time / 3);
            DrawTexMoon(22, 35, new Color(64, 70, 255, 0), Projectile.Center - Main.screenPosition , MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/BloomLight"), -Main.time / 1.8);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
