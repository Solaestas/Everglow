using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MEACModule;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.DemonScythe
{
    internal class DemonScytheBook : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 28;
            Projectile.height = 28;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.tileCollide = false;
            Projectile.alpha = 255;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 22, 12 * player.gravDir * (float)(0.2 + Math.Sin(Main.time / 18d) / 2d))) * 0.3f;
            Projectile.spriteDirection = player.direction;
            Projectile.velocity *= 0;
            if (player.itemTime > 0 && player.HeldItem.type == ItemID.DemonScythe)
            {
                Projectile.timeLeft = player.itemTime + 60;
                if (Timer < 30)
                {
                    Timer++;
                }
            }
            else
            {
                Timer--;
                if (Timer < 0)
                {
                    Projectile.Kill();
                }
            }
            Player.CompositeArmStretchAmount PCAS = Player.CompositeArmStretchAmount.Full;

            player.SetCompositeArmFront(true, PCAS, (float)(-Math.Sin(Main.time / 18d) * 0.6 + 1.2) * -player.direction);
            Vector2 vTOMouse = Main.MouseWorld - player.Center;
            player.SetCompositeArmBack(true, PCAS, (float)(Math.Atan2(vTOMouse.Y, vTOMouse.X) - Math.PI / 2d));
            Projectile.rotation = player.fullRotation;
            if (player.itemTime == 2)
            {
                Vector2 velocity = Utils.SafeNormalize(Main.MouseWorld - Projectile.Center, Vector2.Zero) * player.HeldItem.shootSpeed;
                Projectile.NewProjectile(Projectile.GetSource_FromAI(),Projectile.Center + Utils.SafeNormalize(velocity, Vector2.Zero) * 25, velocity, ProjectileID.DemonScythe, player.HeldItem.damage, player.HeldItem.knockBack, player.whoAmI);
            }
        }
        public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
        {
            overPlayers.Add(index);
        }
        internal int Timer = 0;
        internal float BookScale = 12f;
        public override void PostDraw(Color lightColor)
        {
            Player player = Main.player[Projectile.owner];
            Texture2D Book = TextureAssets.Item[ItemID.DemonScythe].Value;
            Texture2D BookGlow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Item_"+ ItemID.DemonScythe +"_Glow");
            Texture2D Paper = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/DemonScythe/DemonScythePaper");
            Color c0 = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));

            Projectile.hide = false;
            DrawBack(Book);
            DrawBack(BookGlow, true);
            DrawPaper(Paper);
            DrawFront(Book);
            DrawFront(BookGlow, true);

        }
        public void DrawPaper(Texture2D tex)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 X0 = new Vector2(BookScale * player.direction, BookScale * player.gravDir) * 0.45f;
            Vector2 Y0 = new Vector2(BookScale * player.direction, -BookScale * player.gravDir) * 0.64f;
            Color c0 = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));

            for (int x = 0; x < 8; x++)
            {
                List<Vertex2D> bars = new List<Vertex2D>();
                for (int i = 0; i < 10; ++i)
                {
                    double rot = Timer / 270d + i * Timer / 400d * (1 + Math.Sin(Main.time / 7d) * 0.4);
                    rot -= x / 18d / 30d * (Timer);
                    rot += Projectile.rotation;
                    Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 4.5f;

                    float UpX = MathHelper.Lerp(16f / 28f, 27f / 28f, i / 9f);
                    float UpY = MathHelper.Lerp(0 / 32f, 11f / 32f, i / 9f);
                    Vector2 Up = new Vector2(UpX, UpY);
                    Vector2 Down = Up + new Vector2(-15f / 28f, 15f / 32f);

                    if (Math.Abs(rot) > Math.PI / 2d)
                    {
                        if (player.direction * player.gravDir == 1)
                        {
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        }
                        else
                        {
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        }
                    }
                    else
                    {
                        if (player.direction * player.gravDir == 1)
                        {
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        }
                        else
                        {
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        }
                    }
                }
                if (bars.Count > 0)
                {
                    Main.graphics.GraphicsDevice.Textures[0] = tex;
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
                }
            }
            List<Vertex2D> barsII = new List<Vertex2D>();
            for (int i = 0; i < 10; ++i)
            {
                double rotII = -Timer / 270d - i * Timer / 400d * (1 + Math.Sin(Main.time / 7d + 1) * 0.4);
                rotII += 8 / 18d / 30d * (Timer);

                double rotIII = Timer / 270d + i * Timer / 400d * (1 + Math.Sin(Main.time / 7d) * 0.4);
                rotIII -= 8 / 18d / 30d * (Timer);

                double rotIV = MathHelper.Lerp((float)rotII, (float)rotIII, (float)(Main.time / 15d + Math.Sin(Main.time / 62d) * 9) % 1f);
                rotIV += Projectile.rotation;
                Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rotIV) * i / 4.5f - Y0 * 0.05f - X0 * 0.02f;

                float UpX = MathHelper.Lerp(16f / 28f, 27f / 28f, i / 9f);
                float UpY = MathHelper.Lerp(0 / 32f, 11f / 32f, i / 9f);
                Vector2 Up = new Vector2(UpX, UpY);
                Vector2 Down = Up + new Vector2(-15f / 28f, 15f / 32f);

                if (Math.Abs(rotIV) > Math.PI / 2d)
                {
                    if(player.direction == 1)
                    {
                        barsII.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        barsII.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                    }
                    else
                    {
                        barsII.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        barsII.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                    }
                }
                else
                {
                    if (player.direction * player.gravDir == 1)
                    {
                        barsII.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        barsII.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                    }
                    else
                    {
                        barsII.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        barsII.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                    }
                }
            }
            if (barsII.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, barsII.ToArray(), 0, barsII.Count - 2);
            }

            for (int x = 0; x < 8; x++)
            {
                List<Vertex2D> bars = new List<Vertex2D>();
                for (int i = 0; i < 10; ++i)
                {
                    double rot = -Timer / 270d - i * Timer / 400d * (1 + Math.Sin(Main.time / 7d + 1) * 0.4);
                    rot += x / 18d / 30d * (Timer);
                    rot += Projectile.rotation;
                    Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 4.5f - Y0 * 0.05f - X0 * 0.02f;

                    float UpX = MathHelper.Lerp(16f / 28f, 27f / 28f, i / 9f);
                    float UpY = MathHelper.Lerp(0 / 32f, 11f / 32f, i / 9f);
                    Vector2 Up = new Vector2(UpX, UpY);
                    Vector2 Down = Up + new Vector2(-15f / 28f, 15f / 32f);

                    if (Math.Abs(rot) > Math.PI / 2d)
                    {
                        if (player.direction * player.gravDir == 1)
                        {
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        }
                        else
                        {
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        }
                    }
                    else
                    {
                        if (player.direction * player.gravDir == 1)
                        {
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        }
                        else
                        {
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        }
                    }
                }
                if (bars.Count > 0)
                {
                    Main.graphics.GraphicsDevice.Textures[0] = tex;
                    Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
                }
            }
        }
        public void DrawBack(Texture2D tex, bool Glowing = false)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 X0 = new Vector2(BookScale * player.direction, BookScale * player.gravDir) * 0.5f;
            Vector2 Y0 = new Vector2(BookScale * player.direction, -BookScale * player.gravDir) * 0.707f;
            Color c0 = new Color(255, 255, 255, 0);
            if (!Glowing)
            {
                c0 = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
            }

            List<Vertex2D> bars = new List<Vertex2D>();
            for (int i = 0; i < 10; ++i)
            {
                double rot = Timer / 270d + i * Timer / 400d * (1 + Math.Sin(Main.time / 7d) * 0.4);
                Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 4.5f;

                float UpX = MathHelper.Lerp(16f / 28f, 27f / 28f, i / 9f);
                float UpY = MathHelper.Lerp(0 / 32f, 11f / 32f, i / 9f);
                Vector2 Up = new Vector2(UpX, UpY);
                Vector2 Down = Up + new Vector2(-15f / 28f, 15f / 32f);

                rot += Projectile.rotation;
                if (Math.Abs(rot) > Math.PI / 2d)
                {
                        if (player.direction * player.gravDir == 1)
                        {
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        }
                        else
                        {
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        }
                    }
                    else
                    {
                        if (player.direction * player.gravDir == 1)
                        {
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        }
                        else
                        {
                            bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                            bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        }
                }

            }
            if (bars.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }
        public void DrawFront(Texture2D tex, bool Glowing = false)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 X0 = new Vector2(BookScale * player.direction, BookScale * player.gravDir) * 0.5f;
            Vector2 Y0 = new Vector2(BookScale * player.direction, -BookScale * player.gravDir) * 0.707f;
            Color c0 = new Color(255, 255, 255, 0);
            if (!Glowing)
            {
                c0 = Lighting.GetColor((int)(Projectile.Center.X / 16f), (int)(Projectile.Center.Y / 16f));
            }

            List<Vertex2D> bars = new List<Vertex2D>();
            for (int i = 0; i < 10; ++i)
            {
                double rot = -Timer / 270d - i * Timer / 400d * (1+ Math.Sin(Main.time / 7d + 1) * 0.4);
                rot += Projectile.rotation;
                Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 4.5f - Y0 * 0.05f - X0 *0.02f;

                float UpX = MathHelper.Lerp(16f / 28f, 27f / 28f, i / 9f);
                float UpY = MathHelper.Lerp(0 / 32f, 11f / 32f, i / 9f);
                Vector2 Up = new Vector2(UpX, UpY);
                Vector2 Down = Up + new Vector2(-15f / 28f, 15f / 32f);

                if (Math.Abs(rot) > Math.PI / 2d)
                {
                    if (player.direction * player.gravDir == 1)
                    {
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                    }
                    else
                    {
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                    }
                }
                else
                {
                    if (player.direction * player.gravDir == 1)
                    {
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                    }
                    else
                    {
                        bars.Add(new Vertex2D(BasePos - Y0 - Main.screenPosition, c0, new Vector3(Down, 0)));
                        bars.Add(new Vertex2D(BasePos + Y0 - Main.screenPosition, c0, new Vector3(Up, 0)));
                    }
                }
            }
            if (bars.Count > 0)
            {
                Main.graphics.GraphicsDevice.Textures[0] = tex;
                Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
            }
        }
        public override void Kill(int timeLeft)
        {
            Player player = Main.player[Projectile.owner];
            Vector2 X0 = new Vector2(BookScale * player.direction, BookScale * player.gravDir) * 0.5f;
            Vector2 Y0 = new Vector2(BookScale * player.direction, -BookScale * player.gravDir) * 0.707f;
            for (int i = 0; i < 10; ++i)
            {
                double rot = 0;
                rot += Projectile.rotation;
                Vector2 BasePos = Projectile.Center + X0 - X0.RotatedBy(rot) * i / 4.5f;
                Dust d0 = Dust.NewDustDirect(BasePos - Y0, 0, 0, DustID.DemonTorch);
                d0.noGravity = true;
                Dust d1 = Dust.NewDustDirect(BasePos + Y0, 0, 0, DustID.DemonTorch);
                d1.noGravity = true;
            }
            for (int i = 0; i < 14; ++i)
            {
                double rot = 0;
                rot += Projectile.rotation;
                Vector2 BasePos = Projectile.Center + Y0 - Y0.RotatedBy(rot) * i / 4.5f;
                Dust d0 = Dust.NewDustDirect(BasePos - X0, 0, 0, DustID.DemonTorch);
                d0.noGravity = true;
                Dust d1 = Dust.NewDustDirect(BasePos + X0, 0, 0, DustID.DemonTorch);
                d1.noGravity = true;
            }
        }
    }
}
