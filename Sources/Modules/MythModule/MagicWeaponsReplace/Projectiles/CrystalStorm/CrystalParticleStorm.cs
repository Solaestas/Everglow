using Terraria.GameContent;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Commons.Function.ObjectPool;
using Everglow.Sources.Commons.Core.VFX.Visuals;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CrystalStorm
{
    [Pipeline(typeof(WCSPipeline))]

    internal class CrystalParticleStorm : Visual
    {
        public Vector2 position;
        public Vector2 velocity;

        Vector2 AimCenter;
        Vector2 OldAimCenter;

        public int timeLeft;
        public float size;
        public float omega;
        public float rotation;
        public float AI0;
        public float AI1;
        public float AI2;

        private float Theta;

        private Vector2 VS1;
        private Vector2 VS2;
        private Vector2 VS3;

        private Vector2 p1;
        private Vector2 p2;
        private Vector2 p3;

        private Vector2 po1;
        private Vector2 po2;
        private Vector2 po3;
        private float RamdomC;

        private float Ros;
        public override void OnSpawn()
        {
            AimCenter = Main.MouseWorld;
            OldAimCenter = Main.MouseWorld;
            timeLeft = Main.rand.Next(387, 399);
            RamdomC = Main.rand.NextFloat(0f, 1500f);
            Theta += Main.rand.NextFloat(-3.14f, 3.14f);
            Ros = Main.rand.NextFloat(-0.15f, 0.15f);
            p1 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
            p2 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
            p3 = new Vector2(Main.rand.NextFloat(-0.5f, 0.5f), Main.rand.NextFloat(-0.5f, 0.5f));
            base.OnSpawn();
        }
        public override void Update()
        {

            if ((OldAimCenter - Main.MouseWorld).Length() > 200 && OldAimCenter != Vector2.Zero)
            {
                if (timeLeft > 20)
                {
                    timeLeft -= 5;
                }
            }
            AimCenter = Main.MouseWorld;
            OldAimCenter = Main.MouseWorld;
            for (int x = -80; x < 808; x += 8)
            {
                if (Collision.SolidCollision(AimCenter + new Vector2(0, x), 1, 1))
                {
                    AimCenter += new Vector2(0, x);
                    break;
                }
            }
            float Dy = AimCenter.Y - position.Y;
            float xCoefficient = Dy * Dy / 600f - 0.4f * Dy + 50;
            Vector2 TrueAim = AimCenter + new Vector2(xCoefficient * (float)(Math.Sin(Main.timeForVisualEffects * 0.1 + AI0)), 0) - position;



            AI2 = (byte)(AI2 * 0.95 + xCoefficient * 0.05);

            if (!Main.mouseRight)
            {
                velocity = velocity * 0.75f + new Vector2(Utils.SafeNormalize(TrueAim, new Vector2(0, 0.05f)).X, -AI1 * 0.3f) * 0.25f / (float)(AI2) * 500f;
                velocity *= Main.rand.NextFloat(0.85f, 1.15f);
            }
            else
            {
                velocity = velocity * 0.75f + new Vector2(Utils.SafeNormalize(TrueAim, new Vector2(0, 0.05f)).X, -AI1 * 0.3f) * 0.25f / (float)(AI2) * 500f;
                velocity *= Main.rand.NextFloat(0.85f, 1.15f);
            }



            position += velocity;
            timeLeft -= 1;
            if(timeLeft <= 0)
            {
                Kill();
            }


            Theta += Ros;
            po1 = new Vector2(p1.X, p1.Y * (float)Math.Sin(Theta)).RotatedBy(rotation) * 90 * size;
            po2 = new Vector2(p2.X, p2.Y * (float)Math.Sin(Theta)).RotatedBy(rotation) * 90 * size;
            po3 = new Vector2(p3.X, p3.Y * (float)Math.Sin(Theta)).RotatedBy(rotation) * 90 * size;
            rotation -= omega * 0.66f;
            
            if(timeLeft < 20)
            {
                size *= 0.9f;
            }
            if (timeLeft > 100)
            {
                size *= 1.1f;
            }
        }
        public override void Draw()
        {

            List<Vertex2D> Vy = new List<Vertex2D>();
            Color colorD = Color.White;
            Vector2 v1 = po1 + position;
            Vector2 v2 = po2 + position;
            Vector2 v3 = po3 + position;
            if (VS1 == Vector2.Zero)
            {
                VS1 = v1 - Main.screenPosition;
            }
            if (VS2 == Vector2.Zero)
            {
                VS2 = v2 - Main.screenPosition;
            }
            if (VS3 == Vector2.Zero)
            {
                VS3 = v3 - Main.screenPosition;
            }
            Vy.Add(new Vertex2D(v1, colorD, new Vector3(VS1.X / Main.screenTarget.Width, VS1.Y / Main.screenTarget.Height, 0)));
            Vy.Add(new Vertex2D(v2, colorD, new Vector3(VS2.X / Main.screenTarget.Width, VS2.Y / Main.screenTarget.Height, 0)));
            Vy.Add(new Vertex2D(v3, colorD, new Vector3(VS3.X / Main.screenTarget.Width, VS3.Y / Main.screenTarget.Height, 0)));

            GraphicsDevice gd = Main.graphics.GraphicsDevice;
            //SpriteBatch sb = Main.spriteBatch;

            //var cur = VFXManager.Instance.CurrentRenderTarget;

            //gd.SetRenderTarget(VFXManager.Instance.CurrentRenderTarget);
            //sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone);
            //sb.Draw(cur, Vector2.Zero, Color.White);

            //gd.BlendState = BlendState.AlphaBlend;
            //gd.DrawUserPrimitives(PrimitiveType.TriangleList, Vy.ToArray(), 0, Vy.Count - 2);
            //gd.SetRenderTarget(VFXManager.Instance.CurrentRenderTarget);

            //gd.BlendState = BlendState.Additive;
            //sb.Draw(Main.screenTargetSwap, cur.Bounds, Color.White);
            //sb.End();

            Color Co0 = new Color(135, 0, 255);
            int DrawBase = (int)(122.5 + Math.Sin(RamdomC) * 122.5);
            List<Vertex2D> Vx = new List<Vertex2D>();
            colorD = new Color((DrawBase + Co0.R) / 8, (DrawBase + Co0.G) / 8, (DrawBase + Co0.B) / 8, 0);
            Vx.Add(new Vertex2D(po1 + position, colorD, new Vector3(0, 0, 0)));
            Vx.Add(new Vertex2D(po2 + position, colorD, new Vector3(0, 0, 0)));
            Vx.Add(new Vertex2D(po3 + position, colorD, new Vector3(0, 0, 0)));
            gd.Textures[0] = TextureAssets.MagicPixel.Value;
            gd.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count - 2);
        }
        public override CallOpportunity DrawLayer => CallOpportunity.PostDrawNPCs;
    }
}
