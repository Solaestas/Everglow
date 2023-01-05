using Everglow.Sources.Modules.MythModule.TheTusk;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles
{
    class TuskTranF : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180000;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 0, 0);
        }
        public override void AI()
        {
            //Lighting.AddLight(Projectile.Center,(byte)(color0.R * ka) / 300f, (byte)(color0.G * ka) / 300f, (byte)(color0.B * ka) / 300f);
            int AimPlayer = Projectile.owner;
            if (Main.player[AimPlayer].active)
            {
                Projectile.Center = Main.player[AimPlayer].Center + new Vector2(0, -24);
                if (Projectile.timeLeft > 60)
                {
                    Aimcolor = Color.White;
                }
            }
            else
            {
                if (Projectile.timeLeft > 65)
                {
                    Projectile.timeLeft = 60;
                }
            }
            if (Projectile.ai[1] == 7 && Projectile.timeLeft > 65)
            {
                Projectile.timeLeft = 60;
                //Projectile.extraUpdates = 2;
                Aimcolor = new Color(0, 0, 0, 0);
            }
            if (Projectile.timeLeft % 10 == 0)
            {
                int Ct = 0;
                for (int x = -5; x < 6; x++)
                {
                    for (int y = -5; y < 6; y++)
                    {
                        if (Main.tile[(int)Projectile.Center.X / 16 + x, (int)Projectile.Center.Y / 16 + y].TileType == ModContent.TileType<TheTusk.Tiles.BloodyMossWheelFinished>())
                        {
                            Ct += 1;
                        }
                    }
                }
                if (Ct == 0 && Projectile.timeLeft > 65)
                {
                    Projectile.timeLeft = 60;
                    Aimcolor = new Color(0, 0, 0, 0);
                }
            }
            color0.R = (byte)(color0.R * 0.84f + Aimcolor.R * 0.16f);
            color0.G = (byte)(color0.G * 0.84f + Aimcolor.G * 0.16f);
            color0.B = (byte)(color0.B * 0.84f + Aimcolor.B * 0.16f);
            color0.A = (byte)(color0.A * 0.84f + Aimcolor.A * 0.16f);
        }
        Color color0 = new Color(0, 0, 0, 0);
        Color Aimcolor = new Color(0, 0, 0, 0);
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        public override void PostDraw(Color lightColor)
        {
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<VertexBase.CustomVertexInfo> Vx = new List<VertexBase.CustomVertexInfo>();

            Vector2 vf = Projectile.Center - Main.screenPosition;
            Color color3 = color0;
            for (int h = 0; h < 120; h++)
            {
                Vector2 v0 = new Vector2(0, -70).RotatedBy(h / 60d * Math.PI);
                Vector2 v1 = new Vector2(0, -70).RotatedBy((h + 1) / 60d * Math.PI);
                Vx.Add(new VertexBase.CustomVertexInfo(vf + v0, color3, new Vector3(((h) / 20f) % 1f, 0, 0)));
                Vx.Add(new VertexBase.CustomVertexInfo(vf + v1, color3, new Vector3(((0.999f + h) / 20f) % 1f, 0, 0)));
                Vx.Add(new VertexBase.CustomVertexInfo(vf, color3, new Vector3(((0.5f + h) / 20f) % 1f, 1, 0)));
            }
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/TuskTranF").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);


            List<VertexBase.CustomVertexInfo> Vx2 = new List<VertexBase.CustomVertexInfo>();
            for (int h = 0; h < Projectile.ai[0]; h++)
            {
                Vector2 v0 = new Vector2(0, -70).RotatedBy(h / 60d * Math.PI);
                Vector2 v1 = new Vector2(0, -70).RotatedBy((h + 1) / 60d * Math.PI);
                Vx2.Add(new VertexBase.CustomVertexInfo(vf + v0, color3, new Vector3(((h) / 20f) % 1f, 0, 0)));
                Vx2.Add(new VertexBase.CustomVertexInfo(vf + v1, color3, new Vector3(((0.999f + h) / 20f) % 1f, 0, 0)));
                Vx2.Add(new VertexBase.CustomVertexInfo(vf, color3, new Vector3(((0.5f + h) / 20f) % 1f, 1, 0)));
            }
            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscProjectiles/TuskTranF2").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);
        }
    }
}
