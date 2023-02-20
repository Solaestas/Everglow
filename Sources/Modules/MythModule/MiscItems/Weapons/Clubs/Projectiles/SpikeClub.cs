using Everglow.Sources.Commons.Function.Curves;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Common;
using System.Collections.Generic;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public class SpikeClub : ClubProj_metal
    {
        public override void SetDef()
        {
            HitLength = 45f;
            ReflectStrength = 6f;
        }
        internal List<Vector2> MoonBladeI = new List<Vector2>();
        internal List<Vector2> MoonBladeII = new List<Vector2>();
        internal List<Vector2> MoonBladeIII = new List<Vector2>();
        internal List<Vector2> MoonBladeIV = new List<Vector2>();
        internal int timeI = 60;
        internal int timeII = 60;
        internal int timeIII = 60;
        internal int timeIV = 60;
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
            int k = (int)(Omega * 10);
            for (int x = 0; x < Main.rand.Next(k); x++)
            {
                target.StrikeNPC((int)(Projectile.damage * 0.5f * Omega), 0, 1);
            }
            knockback *= 0.4f;

        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(BuffID.Bleeding, 600);
        }
        private void DrawMoon(List<Vector2> listVec, float timeLeft)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            List<Vector2> SmoothTrailX = CatmullRom.SmoothPath(listVec.ToList());//平滑
            List<Vector2> SmoothTrail = new List<Vector2>();
            for (int x = 0; x < SmoothTrailX.Count - 1; x++)
            {
                SmoothTrail.Add(SmoothTrailX[x]);
            }
            if (listVec.Count != 0)
            {
                SmoothTrail.Add(listVec.ToArray()[listVec.Count - 1]);
            }

            int length = SmoothTrail.Count;
            if (length <= 3)
            {
                return;
            }

            List<Vertex2D> bars = new List<Vertex2D>();
            Color light = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
            light *= 2;
            light.A /= 4;
            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float delta = (1 - timeLeft / 60f * 0.45f) * Omega / MaxOmega + 1 - Omega / MaxOmega;
                float maxValue = 20f;
                if(length < maxValue)
                {
                    delta = delta * length / maxValue + (maxValue - length) / maxValue;
                }
                bars.Add(new Vertex2D(Projectile.Center + SmoothTrail[i] * delta * Projectile.scale - Main.screenPosition, light, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center + SmoothTrail[i] * Projectile.scale - Main.screenPosition, light, new Vector3(factor, 0, 0f)));
            }
            Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/BloomLight");
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
        }
        private void UpdateMoon(ref List<Vector2> listVec)
        {
            if (listVec.Count > 0)
            {
                float value = 1;
                if(listVec.Count > 6)
                {
                    value = 6 / (float)listVec.Count;
                }
                listVec.Add(listVec[listVec.Count - 1].RotatedBy(Omega * value));
            }
        }
        private void ActivateMoon(ref List<Vector2> listVec)
        {
            if (listVec.Count > 0)
            {
                return;
            }
            listVec = new List<Vector2>();
            if(Main.rand.NextBool(2))
            {
                listVec.Add(trailVecs.ToList()[1] * Main.rand.NextFloat(0.75f, Math.Min(1.45f, 1 + Omega * 1.1f)));
            }
            else
            {
                listVec.Add(trailVecs.ToList()[1] * -Main.rand.NextFloat(0.75f, Math.Min(1.45f, 1 + Omega * 1.1f)));
            }
        }
        private void DeactivateMoon(ref List<Vector2> listVec)
        {
            listVec.Clear();
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
                w *= w2 * w;
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.7f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
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
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * 0.5f * Projectile.scale, Color.White, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * 0.8f * Projectile.scale, Color.White, new Vector3(factor, 0, w * ReflectStrength)));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            Effect MeleeTrail = MythContent.QuickEffect("MiscItems/Weapons/Clubs/Projectiles/MetalClubTrail");
            MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            

            MeleeTrail.Parameters["tex1"].SetValue((Texture2D)ModContent.Request<Texture2D>(Texture));
            if (ReflectTexturePath != "")
            {
                try
                {
                    MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(ReflectTexturePath).Value);
                }
                catch
                {
                    MeleeTrail.Parameters["tex1"].SetValue((Texture2D)ModContent.Request<Texture2D>(Texture));
                }
            }
            Vector4 lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
            lightColor.W = 0.7f * Omega;
            MeleeTrail.Parameters["Light"].SetValue(lightColor);
            MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

            if(!Main.gamePaused)
            {
                if (Main.rand.NextBool(10) && Omega > 0.1f)
                {
                    ActivateMoon(ref MoonBladeI);
                }
                if (MoonBladeI.Count > 20 && timeI == 60)
                {
                    if (Main.rand.Next(MoonBladeI.Count, 50) > 44)
                    {
                        timeI--;
                    }
                }
                if (timeI < 60)
                {
                    timeI--;
                    if (timeI <= 0)
                    {
                        timeI = 60;
                        DeactivateMoon(ref MoonBladeI);
                    }
                }

                if (Main.rand.NextBool(10) && Omega > 0.1f)
                {
                    ActivateMoon(ref MoonBladeII);
                }
                if (MoonBladeII.Count > 20 && timeII == 60)
                {
                    if (Main.rand.Next(MoonBladeII.Count, 50) > 44)
                    {
                        timeII--;
                    }
                }
                if (timeII < 60)
                {
                    timeII--;
                    if (timeII <= 0)
                    {
                        timeII = 60;
                        DeactivateMoon(ref MoonBladeII);
                    }
                }

                if (Main.rand.NextBool(10) && Omega > 0.1f)
                {
                    ActivateMoon(ref MoonBladeIII);
                }
                if (MoonBladeIII.Count > 20 && timeIII == 60)
                {
                    if (Main.rand.Next(MoonBladeIII.Count, 50) > 44)
                    {
                        timeIII--;
                    }
                }
                if (timeIII < 60)
                {
                    timeIII--;
                    if (timeIII <= 0)
                    {
                        timeIII = 60;
                        DeactivateMoon(ref MoonBladeIII);
                    }
                }
                if (Main.rand.NextBool(10) && Omega > 0.1f)
                {
                    ActivateMoon(ref MoonBladeIV);
                }
                if (MoonBladeIV.Count > 20 && timeIV == 60)
                {
                    if (Main.rand.Next(MoonBladeIV.Count, 50) > 44)
                    {
                        timeIV--;
                    }
                }
                if (timeIV < 60)
                {
                    timeIV--;
                    if (timeIV <= 0)
                    {
                        timeIV = 60;
                        DeactivateMoon(ref MoonBladeIV);
                    }
                }

                UpdateMoon(ref MoonBladeI);
                UpdateMoon(ref MoonBladeIV);
                UpdateMoon(ref MoonBladeIII);
                UpdateMoon(ref MoonBladeIV);
            }

            DrawMoon(MoonBladeI, timeI);
            DrawMoon(MoonBladeII, timeII);
            DrawMoon(MoonBladeIII, timeIII);
            DrawMoon(MoonBladeIV, timeIV);
        }
    }
}
