﻿using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Core.VFX;
using Everglow.Sources.Commons.Core.VFX.Pipelines;
using Everglow.Sources.Commons.Core.VFX.Visuals;
using Everglow.Sources.Commons.Function.Vertex;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.CrystalStorm
{
    [Pipeline(typeof(WCSPipeline))]
    internal class CrystalParticle : Visual
    {
        public Vector2 position;
        public Vector2 velocity;
        public int timeLeft;
        public float size;
        public float omega;
        public float rotation;

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
            position += velocity;
            timeLeft -= 1;
            if (timeLeft <= 0)
            {
                Kill();
            }
            if (timeLeft <= 30)
            {
                velocity *= 0.98f;
            }
            velocity = velocity.RotatedBy(omega);
            omega += Main.rand.NextFloat(-0.05f, 0.05f);
            if (Math.Abs(omega) > 0.15)
            {
                omega *= 0.98f;
            }

            Theta += Ros;
            po1 = new Vector2(p1.X, p1.Y * (float)Math.Sin(Theta)).RotatedBy(rotation) * 90 * size;
            po2 = new Vector2(p2.X, p2.Y * (float)Math.Sin(Theta)).RotatedBy(rotation) * 90 * size;
            po3 = new Vector2(p3.X, p3.Y * (float)Math.Sin(Theta)).RotatedBy(rotation) * 90 * size;
            rotation -= omega * 0.66f;
            velocity *= 0.99f;
            size *= 0.95f;
            if (size < 0.05f)
            {
                base.Update();
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
            SpriteBatch sb = Main.spriteBatch;

            //var cur = VFXManager.Instance.CurrentRenderTarget;

            //gd.SetRenderTarget(Main.screenTargetSwap);
            //sb.Begin(SpriteSortMode.Immediate, BlendState.Opaque, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone);
            //sb.Draw(Main.screenTarget, Vector2.Zero, Color.White);

            //gd.BlendState = BlendState.AlphaBlend;
            //gd.DrawUserPrimitives(PrimitiveType.TriangleList, Vy.ToArray(), 0, Vy.Count - 2);
            //gd.SetRenderTarget(Main.screenTarget);

            //gd.BlendState = BlendState.Additive;
            //sb.Draw(Main.screenTargetSwap, Main.screenTargetSwap.Bounds, Color.White);
            //sb.End();

            Color Co0 = new Color(135, 0, 255);
            int DrawBase = (int)(122.5 + Math.Sin(RamdomC) * 122.5);
            List<Vertex2D> Vx = new List<Vertex2D>();
            colorD = new Color((DrawBase + Co0.R) / 8, (DrawBase + Co0.G) / 8, (DrawBase + Co0.B) / 8, 0);
            Vx.Add(new Vertex2D(po1 + position, colorD, new Vector3(0, 0, 0)));
            Vx.Add(new Vertex2D(po2 + position, colorD, new Vector3(0, 0, 0)));
            Vx.Add(new Vertex2D(po3 + position, colorD, new Vector3(0, 0, 0)));
            //gd.Textures[0] = TextureAssets.MagicPixel.Value;
            //gd.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count - 2);
            VFXManager.spriteBatch.Draw(TextureAssets.MagicPixel.Value,Vx,PrimitiveType.TriangleList);
        }

        public override CallOpportunity DrawLayer => CallOpportunity.PostDrawFilter;
    }
}