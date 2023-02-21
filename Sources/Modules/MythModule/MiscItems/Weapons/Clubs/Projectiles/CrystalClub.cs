using Everglow.Sources.Commons.Function.Curves;
using Everglow.Sources.Commons.Function.Vertex;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs.Projectiles
{
    public class CrystalClub : ClubProj
    {
        public override void SetDef()
        {
            Beta = 0.005f;
            MaxOmega = 0.45f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void AI()
        {
            base.AI();

            if (Omega > 0.1f)
            {
                for (float d = 0.1f; d < Omega; d += 0.1f)
                {
                }
                if (FlyClubCooling > 0)
                {
                    FlyClubCooling--;
                }
                if (FlyClubCooling <= 0 && Omega > 0.2f)
                {
                    FlyClubCooling = (int)(44);
                }
            }
        }
        private int FlyClubCooling = 0;
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

            float fade = Omega * 2f + 0.2f;
            Color color2 = new Color(fade, Math.Min(fade * 0.4f, 0.6f), 0, 0);

            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
                float w2 = MathF.Sqrt(TrailAlpha(factor));
                w *= w2 * w;
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * 0.5f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center + trail[i] * 1.0f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 0, w)));
            }
            bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, Color.Transparent, new Vector3(0, 0, 0)));
            bars.Add(new Vertex2D(Projectile.Center - Main.screenPosition, Color.Transparent, new Vector3(0, 0, 0)));
            for (int i = 0; i < length; i++)
            {
                float factor = i / (length - 1f);
                float w = 1 - Math.Abs((trail[i].X * 0.5f + trail[i].Y * 0.5f) / trail[i].Length());
                float w2 = MathF.Sqrt(TrailAlpha(factor));
                w *= w2 * w;
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * 0.5f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 1, 0f)));
                bars.Add(new Vertex2D(Projectile.Center - trail[i] * 1.0f * Projectile.scale - Main.screenPosition, color2, new Vector3(factor, 0, w)));
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, TrailBlendState(), SamplerState.AnisotropicWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
            var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.ZoomMatrix;

            //Effect MeleeTrail = MythContent.QuickEffect("MiscItems/Weapons/Clubs/Projectiles/MetalClubTrail");
            //MeleeTrail.Parameters["uTransform"].SetValue(model * projection);
            Main.graphics.GraphicsDevice.Textures[0] = ModContent.Request<Texture2D>(TrailShapeTex(), ReLogic.Content.AssetRequestMode.ImmediateLoad).Value;

            //MeleeTrail.Parameters["tex1"].SetValue((Texture2D)ModContent.Request<Texture2D>(Texture));
            //if (ReflectTexturePath != "")
            //{
            //    try
            //    {
            //        MeleeTrail.Parameters["tex1"].SetValue(ModContent.Request<Texture2D>(ReflectTexturePath).Value);
            //    }
            //    catch
            //    {
            //        MeleeTrail.Parameters["tex1"].SetValue((Texture2D)ModContent.Request<Texture2D>(Texture));
            //    }
            //}
            Vector4 lightColor = Lighting.GetColor((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16)).ToVector4();
            lightColor.W = 0.7f * Omega;
            //MeleeTrail.Parameters["Light"].SetValue(lightColor);
            //MeleeTrail.CurrentTechnique.Passes["TrailByOrigTex"].Apply();

            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);        
        }
    }
}