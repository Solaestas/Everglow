using Everglow.Sources.Commons.Function.Curves;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public class StarDancer : ClubProj
    {
        public override void SetDef()
        {
            Beta = 0.005f;
            MaxOmega = 0.45f;
            WarpValue = 0.5f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            if (Main.rand.NextBool(7))
            {
                Vector2 v0 = new Vector2(Main.rand.NextFloat(1f, 1.2f), 0).RotatedByRandom(6.283);
                for (int i = 0; i < 5; i++)
                {
                    Vector2 v1 = v0.RotatedBy(i / 2.5 * Math.PI);
                    Vector2 v2 = v0.RotatedBy((i + 0.5) / 2.5 * Math.PI) * 3;
                    Vector2 v3 = v0.RotatedBy((i + 1) / 2.5 * Math.PI);
                    for (int j = 0; j < 15; j++)
                    {
                        Vector2 v4 = (v1 * j + v2 * (14 - j)) / 14f;
                        Vector2 v5 = (v3 * j + v2 * (14 - j)) / 14f;
                        Vector2 v6 = v2 * (14 - j) / 14f;
                        Dust D = Dust.NewDustDirect(target.Center + v4 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.GoldCoin, 0, 0, 0, default, 1.5f);
                        D.noGravity = true;
                        D.velocity = v4;

                        Dust D1 = Dust.NewDustDirect(target.Center + v5 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.GoldCoin, 0, 0, 0, default, 1.5f);
                        D1.noGravity = true;
                        D1.velocity = v5;

                        Dust D2 = Dust.NewDustDirect(target.Center + v6 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, DustID.GoldCoin, 0, 0, 0, default, 1.3f);
                        D2.noGravity = true;
                        D2.velocity = v6;
                    }
                }
                Vector2 v7 = new Vector2(0, -Main.rand.NextFloat(1000f, 1200f)).RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f));
                Projectile.NewProjectile(Projectile.GetSource_FromAI(), target.Center + v7, -v7 / 40f, ProjectileID.FallingStar, (int)(Projectile.damage * 8.3f), Projectile.knockBack, Projectile.owner);
                SoundEngine.PlaySound(SoundID.DD2_CrystalCartImpact,target.Center);
            }
        }
        public override void AI()
        {
            base.AI();
            if (Omega > 0.1f)
            {
                GenerateDust();
            }
        }
        internal float ReflectStrength = 1.2f;
        private void GenerateDust()
        {
            Vector2 v0 = new Vector2(1, 1);
            v0 *= Main.rand.NextFloat(Main.rand.NextFloat(1, HitLength), HitLength);
            v0.X *= Projectile.spriteDirection;
            if (Main.rand.NextBool(2))
            {
                v0 *= -1;
            }
            v0 = v0.RotatedBy(Projectile.rotation);
            float Speed = Math.Min(Omega * 0.5f, 0.181f);
            int type = DustID.GoldCoin;
            for (float d = 0.1f; d < Omega; d += 0.04f)
            {
                Dust D = Dust.NewDustDirect(Projectile.Center + v0 - new Vector2(4)/*Dust的Size=8x8*/, 0, 0, type, -v0.Y * Speed, v0.X * Speed, 150, default, Main.rand.NextFloat(0.1f, 0.2f));
                D.noGravity = true;
                D.velocity = new Vector2(-v0.Y * Speed, v0.X * Speed);
            }
        }
        public override void PostDraw(Color lightColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (Projectile.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Texture2D texture = MythContent.QuickTexture("MiscItems/Weapons/Clubs/Projectiles/StarDancer_glow");
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, lightColor, Projectile.rotation, texture.Size() / 2f, Projectile.scale, effects, 0f);
            for (int i = 0; i < 5; i++)
            {
                float fade = Omega * 2f + 0.2f;
                fade *= (5 - i) / 5f;
                Color color2 = new Color(fade, fade, fade, 0);
                Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, color2, Projectile.rotation - i * 0.75f * Omega, texture.Size() / 2f, Projectile.scale, effects, 0f);
            }
        }
        public override void PostPreDraw()
        {
            List<Vector2> SmoothTrailX = CatmullRom.SmoothPath(trailVecs.ToList());//平滑
            List<Vector2> SmoothTrail = new List<Vector2>();
            for (int x = 0; x < SmoothTrailX.Count - 1; x++)
            {
                SmoothTrail.Add(SmoothTrailX[x]);
            }
            if (trailVecs.Count != 0)
            {
                SmoothTrail.Add(trailVecs.ToArray()[trailVecs.Count - 1]);
            }

            int length = SmoothTrail.Count;
            if (length <= 3)
            {
                return;
            }
            Vector2[] trail = SmoothTrail.ToArray();
            List<Vertex2D> bars = new List<Vertex2D>();

            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
                float w2 = MathF.Sqrt(TrailAlpha(factor));
                w *= w2;
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.1f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w * ReflectStrength)));
            }
            bars.Add(new Vertex2D(Projectile.Center, Color.Transparent, new Vector3(0, 0, 0)));
            bars.Add(new Vertex2D(Projectile.Center, Color.Transparent, new Vector3(0, 0, 0)));
            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
                float w2 = MathF.Sqrt(TrailAlpha(factor));
                w *= w2 * w;
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * 0.1f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * Projectile.scale, Color.White, new Vector3(factor, 0, w * ReflectStrength)));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            Effect MeleeTrail = MythContent.QuickEffect("MiscItems/Weapons/Clubs/Projectiles/CrystalClubTrail");
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            MeleeTrail.Parameters["tex1"].SetValue(MythContent.QuickTexture("MiscItems/Weapons/Clubs/Projectiles/CrystalClub_trail"));
            Vector4 lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
            lightColor.W = 0.7f * Omega;
            MeleeTrail.Parameters["Light"].SetValue(lightColor);
            MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
    }
}
