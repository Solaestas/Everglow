using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.MiscProjectiles.Weapon.Melee.Hepuyuan
{
    class XiaoBlackWave : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 68;
            Projectile.height = 68;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 120;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.extraUpdates = 6;
        }
        public override void AI()
        {
            Projectile.velocity *= 0;
            Energy += Projectile.ai[0];
            addi++;
        }
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        private Effect ef;
        float radious = 0;
        Vector3[] CirclePoint = new Vector3[120];
        float Rad = 0;
        Vector2[] Circle2D = new Vector2[120];
        float Cy2 = -37.5f;
        float cirpro = 0;
        Vector2 DrawAdd = Vector2.Zero;
        double HaloRot = 0;
        Vector2 Halo1 = Vector2.Zero;
        bool HasForst = false;
        float Energy = 0;
        int addi = 0;
        Vector2 v0 = Vector2.Zero;
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Color drawColor = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));

            if (v0 == Vector2.Zero)
            {
                v0 = new Vector2(Projectile.ai[0], Projectile.ai[1]);
                Projectile.velocity *= 0;
            }
            Vector2 v1 = Vector2.Normalize(v0) * 40f * (float)(1 + Math.Sin(addi / 20d) / 6d);
            if (!Main.gamePaused)
            {
                Halo1 = v1;
            }
            if (!Main.gamePaused)
            {
                DrawAdd = Vector2.Normalize(v0) * 26f;
            }

            Cy2 = 0;//三维Y向校正量
            Rad = (float)Energy * 0.75f;//半径
            cirpro += 0.5f;
            for (int d = 0; d < 120; d++)
            {
                Circle2D[d] = new Vector2(30, 0).RotatedBy(d * Math.PI / 60d);//2D平面圆
                CirclePoint[d] = new Vector3(Circle2D[d].X, -15, 50 + Circle2D[d].Y);//向3维投影
            }
            for (int d = 0; d < 120; d++)
            {
                Circle2D[d] = new Vector2(CirclePoint[d].X / (float)CirclePoint[d].Z, CirclePoint[d].Y / (float)CirclePoint[d].Z + 0.3f/*二维Y向校正量*/) * Rad * (float)(1 + Math.Sin(addi / 31d + 5) / 7d);//落回2D
            }
            //背景层
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Vector2 Vbase = Projectile.Center - Main.screenPosition;

            List<Vertex2D> Vx4 = new List<Vertex2D>();
            if (!Main.gamePaused)
            {
                HaloRot = Math.Atan2(v0.Y, v0.X) + Math.PI / 2d;
            }
            float kt = Projectile.timeLeft / 120f;
            kt = (float)Math.Sqrt(kt);
            Color cr = new Color(kt, kt, kt, kt);
            for (int h = 0; h < 120; h++)
            {
                Vx4.Add(new Vertex2D(Vbase + Circle2D[(h) % 120].RotatedBy(Projectile.rotation), cr, new Vector3(((h + cirpro) / 30f) % 1f, 0, 0)));
                Vx4.Add(new Vertex2D(Vbase + Circle2D[(h + 1) % 120].RotatedBy(Projectile.rotation), cr, new Vector3(((0.999f + h + cirpro) / 30f) % 1f, 0, 0)));
                Vx4.Add(new Vertex2D(Vbase + new Vector2(0, -0.3f * Rad * (float)(1 + Math.Sin(addi / 31d + 5) / 7d)).RotatedBy(Projectile.rotation), cr, new Vector3(((0.5f + h + cirpro) / 30f) % 1f, 1, 0)));
            }


            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/UIimages/VisualTextures/ShadeRing").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx4.ToArray(), 0, Vx4.Count / 3);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

        }
    }
}
