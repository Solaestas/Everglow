using Terraria.Localization;
namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Weapon.Legendary
{
    public class Gear7 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Gear");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "齿轮");
        }
        public override void SetDefaults()
        {
            Projectile.width = 230;
            Projectile.height = 230;
            Projectile.friendly = false;
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.timeLeft = 60;
            //Projectile.extraUpdates = 10;
            Projectile.tileCollide = false;
        }
        public override Color? GetAlpha(Color lightColor)
        {
            return new Color?(new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0));
        }
        float Ome = 0;
        float Ang = 0;
        Vector2 Inpos;
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Inpos = player.Center - new Vector2(170 * player.direction, 150) - new Vector2(115, 0) + new Vector2(32 * Projectile.ai[0], 0);
            Projectile.position = Projectile.position * 0.498f + Inpos * 0.502f;
            Projectile.rotation -= Ome * 0.27f;
            if (Ome < 0.1f && Projectile.timeLeft > 55)
            {
                Ome += 0.0006f;
            }
            if (Projectile.timeLeft <= 55)
            {
                Ome *= 0.98f;
            }
            if (fade < 1f && Projectile.timeLeft > 55)
            {
                fade += 0.02f;
            }
            if (Projectile.timeLeft <= 55)
            {
                fade *= 0.93f;
            }
            if (Main.mouseLeft && player.HeldItem.type == ModContent.ItemType<MiscItems.Weapons.Legendary.MachineSkeGun>())
            {
                Projectile.timeLeft = 60;
            }
        }
        float fade = 0;
        /*float R1 = 0;
        float R2 = 0;
        float R3 = 0;
        float AR1 = 50;
        float AR2 = 70;
        float AR3 = 80;
        int[] L = new int[9];
        float[] R = new float[9];*/
        public override void PostDraw(Color lightColor)
        {
            /* if(!Main.gamePaused)
             {
                 R1 = R1 * 0.95f + AR1 * 0.05f;
                 R2 = R2 * 0.95f + AR2 * 0.05f;
                 R3 = R3 * 0.95f + AR3 * 0.05f;
             }*/
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            /* CirR0 += 0.007f;
             CirPro0 += 0.1f;
             Main.spriteBatch.End();
             Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
             Color color3 = new Color(fade * fade, fade * fade, fade * fade, 0);
             Vector2 vf = Projectile.Center - Main.screenPosition;
             Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/Gear9").Value;
             for (int d = 0;d < 9;d++)
             {
                 L[d] = (int)(45 + Math.Sin(CirPro0 * Math.Sin((4 - d + CirPro0) * 37)) * (20 + d * d + Math.Cos(CirPro0 * 4 * d) * 14));
                 R[d] = (float)(Math.Pow(1.15, d + Math.Cos(CirPro0 * 4 * d) * 14));
                 List<VertexBase.CustomVertexInfo> Vx = new List<VertexBase.CustomVertexInfo>();        
                 for (int h = 0; h < L[d]; h++)
                 {
                     color3.A = 0;
                     color3.R = (byte)(color3.R * (255 - Projectile.alpha) / 255f);
                     color3.G = (byte)(color3.G * (255 - Projectile.alpha) / 255f);
                     color3.B = (byte)(color3.B * (255 - Projectile.alpha) / 255f);
                     Vector2 v0 = new Vector2(0, R1 * (d / 7f + 1)).RotatedBy(h / 45d * Math.PI + CirR0 + R[d]);
                     Vector2 v1 = new Vector2(0, R1 * (d / 7f + 1)).RotatedBy((h + 1) / 45d * Math.PI + CirR0 + R[d]);
                     Vx.Add(new VertexBase.CustomVertexInfo(vf + v0, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                     Vx.Add(new VertexBase.CustomVertexInfo(vf + v1, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                     Vx.Add(new VertexBase.CustomVertexInfo(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
                 }
                 Main.graphics.GraphicsDevice.Textures[0] = t;
                 Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx.ToArray(), 0, Vx.Count / 3);
             }

             List<VertexBase.CustomVertexInfo> Vx2 = new List<VertexBase.CustomVertexInfo>();
             for (int h = 0; h < 90; h++)
             {
                 color3.A = 0;
                 color3.R = (byte)(color3.R * (255 - Projectile.alpha) / 255f);
                 color3.G = (byte)(color3.G * (255 - Projectile.alpha) / 255f);
                 color3.B = (byte)(color3.B * (255 - Projectile.alpha) / 255f);
                 Vector2 v0 = new Vector2(0, R2).RotatedBy(h / 45d * Math.PI + CirR0);
                 Vector2 v1 = new Vector2(0, R2).RotatedBy((h + 1) / 45d * Math.PI + CirR0);
                 Vx2.Add(new VertexBase.CustomVertexInfo(vf + v0, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                 Vx2.Add(new VertexBase.CustomVertexInfo(vf + v1, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                 Vx2.Add(new VertexBase.CustomVertexInfo(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
             }
             t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/Gear10").Value;
             Main.graphics.GraphicsDevice.Textures[0] = t;
             Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx2.ToArray(), 0, Vx2.Count / 3);

             List<VertexBase.CustomVertexInfo> Vx3 = new List<VertexBase.CustomVertexInfo>();
             for (int h = 0; h < 90; h++)
             {
                 color3.A = 0;
                 color3.R = (byte)(color3.R * (255 - Projectile.alpha) / 255f);
                 color3.G = (byte)(color3.G * (255 - Projectile.alpha) / 255f);
                 color3.B = (byte)(color3.B * (255 - Projectile.alpha) / 255f);
                 Vector2 v0 = new Vector2(0, R3).RotatedBy(h / 45d * Math.PI + CirR0);
                 Vector2 v1 = new Vector2(0, R3).RotatedBy((h + 1) / 45d * Math.PI + CirR0);
                 if (h % 2 == 1)
                 {
                     Vx3.Add(new VertexBase.CustomVertexInfo(vf + v0, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                     Vx3.Add(new VertexBase.CustomVertexInfo(vf + v1, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                     Vx3.Add(new VertexBase.CustomVertexInfo(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
                 }
                 else
                 {
                     Vx3.Add(new VertexBase.CustomVertexInfo(vf + v0, color3, new Vector3(((0.999f + h) / 30f) % 1f, 0, 0)));
                     Vx3.Add(new VertexBase.CustomVertexInfo(vf + v1, color3, new Vector3(((h) / 30f) % 1f, 0, 0)));
                     Vx3.Add(new VertexBase.CustomVertexInfo(vf, color3, new Vector3(((0.5f + h) / 30f) % 1f, 1, 0)));
                 }
             }
             t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/Gearlogo").Value;
             Main.graphics.GraphicsDevice.Textures[0] = t;
             Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Vx3.ToArray(), 0, Vx3.Count / 3);*/
        }
        int frequency = 30;
        int Acount = 1;
        private Effect ef;
        float Rota = 0;
        float CirR0 = 0;
        float CirPro0 = 0;
        public override bool PreDraw(ref Color lightColor)
        {
            CirPro0 += 0.003f;
            Rota = (float)(-Main.time * 0.08f);
            ef = (Effect)ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/GearWave").Value;
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/MiscItems/Projectiles/Weapon/Legendary/Gear7").Value;

            //Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0), (float)(Math.Sin(Main.time * 0.04) * 1.3f), new Vector2(115), 1f, SpriteEffects.None, 0f);
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Additive, SamplerState.AnisotropicClamp, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
            Main.spriteBatch.Draw(t, Projectile.Center - Main.screenPosition, null, new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0), (float)(Math.Sin(Main.time * 0.04) * 1.3f), new Vector2(115), 1.3f, SpriteEffects.None, 0f);
            //Main.spriteBatch.Draw(t2, Projectile.Center - Main.screenPosition, null, new Color((float)(fade * fade), (float)(fade * fade), (float)(fade * fade), 0), (float)(Math.Sin(Main.time * 0.04) * 0.5f), new Vector2(115), 1f, SpriteEffects.None, 0f);
            ef.Parameters["radiu"].SetValue(Math.Abs(CirPro0 % 2f - 1f));
            ef.Parameters["Col"].SetValue(1);
            if (Projectile.timeLeft <= 55)
            {
                ef.Parameters["Col"].SetValue(fade);
            }
            ef.CurrentTechnique.Passes["Test"].Apply();
            return true;
        }
    }
}
