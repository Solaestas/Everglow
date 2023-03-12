﻿using Everglow.Sources.Commons.Function.Vertex;

namespace Everglow.Sources.Modules.MythModule.LanternMoon.Projectiles.DashCore
{
    class ImmuneCircle : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 34;
            Projectile.height = 34;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 180000;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color(0, 0, 0, 0);
        }
        float ka = 1;
        public override void AI()
        {
            if (Projectile.timeLeft < 60f)
            {
                ka *= 0.97f;
            }
            Lighting.AddLight(Projectile.Center, (byte)(color0.R * ka) / 300f, (byte)(color0.G * ka) / 300f, (byte)(color0.B * ka) / 300f);
            int AimPlayer = Projectile.owner;
            if (Main.player[AimPlayer].active)
            {
                Projectile.Center = Main.player[AimPlayer].Center + new Vector2(0, -24);
                Aimcolor = new Color(0, 0, 0);
                if (Main.player[AimPlayer].HasBuff(ModContent.BuffType<Buffs.WhiteImmune>()))
                {
                    Aimcolor = new Color(255, 255, 255);
                }
                if (Main.player[AimPlayer].HasBuff(ModContent.BuffType<Buffs.RedImmune>()))
                {
                    Aimcolor = new Color(255, 0, 0);
                }
                if (Main.player[AimPlayer].HasBuff(ModContent.BuffType<Buffs.GreenImmune>()))
                {
                    Aimcolor = new Color(0, 255, 17);
                }
                if (Main.player[AimPlayer].HasBuff(ModContent.BuffType<Buffs.BlueImmune>()))
                {
                    Aimcolor = new Color(0, 131, 255);
                }
                if (Main.player[AimPlayer].HasBuff(ModContent.BuffType<Buffs.BrownImmune>()))
                {
                    Aimcolor = new Color(107, 53, 0);
                }
                if (Main.player[AimPlayer].HasBuff(ModContent.BuffType<Buffs.PurpleImmune>()))
                {
                    Aimcolor = new Color(129, 4, 224);
                }
                if (Main.player[AimPlayer].HasBuff(ModContent.BuffType<Buffs.PinkImmune>()))
                {
                    Aimcolor = new Color(255, 0, 191);
                }
                if (Main.player[AimPlayer].HasBuff(ModContent.BuffType<Buffs.YellowImmune>()))
                {
                    Aimcolor = new Color(255, 204, 0);
                }
            }
            else
            {
                if (Projectile.timeLeft > 65)
                {
                    Projectile.timeLeft = 60;
                }
            }
            if (Aimcolor == new Color(0, 0, 0) && Projectile.timeLeft > 65)
            {
                Projectile.timeLeft = 60;
            }
            color0.R = (byte)(color0.R * 0.94f + Aimcolor.R * 0.06f);
            color0.G = (byte)(color0.G * 0.94f + Aimcolor.G * 0.06f);
            color0.B = (byte)(color0.B * 0.94f + Aimcolor.B * 0.06f);
            color0.A = (byte)(color0.A * 0.94f + Aimcolor.A * 0.06f);
            kb *= 0.97f;
        }
        Color color0 = new Color(0, 0, 0);
        Color Aimcolor = new Color(0, 0, 0);
        float kb = 1;
        public override bool PreDraw(ref Color lightColor)
        {
            return false;
        }
        int TrueL = 1;
        float CirR0 = 0;
        float CirPro0 = 0;
        public override void PostDraw(Color lightColor)
        {
            CirR0 += 0.007f;
            CirPro0 += 0.1f;
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            List<Vertex2D> Vx = new List<Vertex2D>();

            Vector2 vf = Projectile.Center - Main.screenPosition;
            Color color3 = new Color(color0.R, color0.G, color0.B, 0);
            for (int h = 0; h < 90; h++)
            {
                color3.A = 0;
                color3.R = (byte)(color3.R * (255 - Projectile.alpha) / 255f);
                color3.G = (byte)(color3.G * (255 - Projectile.alpha) / 255f);
                color3.B = (byte)(color3.B * (255 - Projectile.alpha) / 255f);
                Vector2 v0 = new Vector2(0, 50).RotatedBy(h / 45d * Math.PI + CirR0);
                Vector2 v1 = new Vector2(0, 50).RotatedBy((h + 1) / 45d * Math.PI + CirR0);
                Vx.Add(new Vertex2D(vf + v0, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                Vx.Add(new Vertex2D(vf + v1, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                Vx.Add(new Vertex2D(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
            }
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/DashCore/ImmuneCircle").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);


            List<Vertex2D> Vx2 = new List<Vertex2D>();
            for (int h = 0; h < 90; h++)
            {
                color3.A = 0;
                color3.R = (byte)(color3.R * (255 - Projectile.alpha) / 255f);
                color3.G = (byte)(color3.G * (255 - Projectile.alpha) / 255f);
                color3.B = (byte)(color3.B * (255 - Projectile.alpha) / 255f);
                Vector2 v0 = new Vector2(0, 50).RotatedBy(h / 45d * Math.PI + CirR0);
                Vector2 v1 = new Vector2(0, 50).RotatedBy((h + 1) / 45d * Math.PI + CirR0);
                Vx2.Add(new Vertex2D(vf + v0, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                Vx2.Add(new Vertex2D(vf + v1, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                Vx2.Add(new Vertex2D(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
            }
            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/DashCore/ImmuneCircle3").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);

            List<Vertex2D> Vx3 = new List<Vertex2D>();
            for (int h = 0; h < 90; h++)
            {
                color3.A = 0;
                color3.R = (byte)(color3.R * (255 - Projectile.alpha) / 255f);
                color3.G = (byte)(color3.G * (255 - Projectile.alpha) / 255f);
                color3.B = (byte)(color3.B * (255 - Projectile.alpha) / 255f);
                Vector2 v0 = new Vector2(0, 50).RotatedBy(h / 45d * Math.PI + CirR0);
                Vector2 v1 = new Vector2(0, 50).RotatedBy((h + 1) / 45d * Math.PI + CirR0);
                if (h % 2 == 1)
                {
                    Vx3.Add(new Vertex2D(vf + v0, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                    Vx3.Add(new Vertex2D(vf + v1, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                    Vx3.Add(new Vertex2D(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
                }
                else
                {
                    Vx3.Add(new Vertex2D(vf + v0, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                    Vx3.Add(new Vertex2D(vf + v1, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                    Vx3.Add(new Vertex2D(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
                }
            }
            t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/LanternMoon/Projectiles/DashCore/ImmuneCircle2").Value;
            Main.graphics.GraphicsDevice.Textures[0] = t;
            Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx3.ToArray(), 0, Vx3.Count / 3);
        }
    }
}
