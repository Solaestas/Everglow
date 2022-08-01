using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.ZYModule.Commons.Function.MapIO;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.ZYModule.Items
{
    internal class SightOfTileProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.friendly = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.aiStyle = -1;
            Projectile.penetrate = -1;
            Projectile.timeLeft = 10000;
            Projectile.hostile = false;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
        }
        public override void AI()
        {
            Player player = Main.player[Projectile.owner];
            player.itemTime = 5;
            player.itemAnimation = 5;
            Projectile.position = player.MountedCenter - new Vector2(17);
            if (Projectile.timeLeft > 6)
            {
                StartPoint = Main.MouseWorld;
            }
            if (Main.mouseLeft)
            {
                Projectile.timeLeft = 5;
            }


        }
        private Vector2 StartPoint = Vector2.Zero;
        public override bool PreDraw(ref Color lightColor)
        {
            Player player = Main.player[Projectile.owner];

            Vector2 Vdr = Main.MouseWorld - Projectile.Center;
            
            Vdr = Vdr / Vdr.Length() * 7;

            player.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, (float)(Math.Atan2(Vdr.Y, Vdr.X) - Math.PI / 2d));
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/SightOfTileProj").Value;
            Color color = Lighting.GetColor((int)Projectile.Center.X / 16, (int)((double)Projectile.Center.Y / 16.0));
            SpriteEffects S = SpriteEffects.None;
            if (Math.Sign(Vdr.X) == -1)
            {
                player.direction = -1;
            }
            else
            {
                player.direction = 1;
            }

            Main.spriteBatch.Draw(t, player.MountedCenter - Main.screenPosition + Vdr * 5f, null, color, (float)(Math.Atan2(Vdr.Y, Vdr.X) + Math.PI / 4d), t.Size() / 2f, Projectile.scale, S, 0f);
            int X1 = (int)(StartPoint.X / 16f);
            int X2 = (int)(Main.MouseWorld.X / 16f);
            int Y1 = (int)(StartPoint.Y / 16f);
            int Y2 = (int)(Main.MouseWorld.Y / 16f);
            DrawNinePiecesForTiles(X1, X2, Y1, Y2);
            return false;
        }
        private void DrawNinePiecesForTiles(int LeftX, int RightX, int UpY, int DownY)
        {
            if (LeftX > RightX)
            {
                int exchange = RightX;
                RightX = LeftX;
                LeftX = exchange;
            }
            if (UpY > DownY)
            {
                int exchange = DownY;
                DownY = UpY;
                UpY = exchange;
            }

            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
            Texture2D t = ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZYModule/Items/Rectangle").Value;
            Color baseColor = new Color(0, 30, 120, 180);
            if (LeftX == RightX)
            {
                if (UpY == DownY)
                {
                    int ScPosX = (int)Main.screenPosition.X;
                    int ScPosY = (int)Main.screenPosition.Y;
                    Rectangle source = new Rectangle(0, 0, 8, 8);
                    Main.spriteBatch.Draw(t, new Rectangle(LeftX * 16 - ScPosX, UpY * 16 - ScPosY, 8, 8), source, GetTileColor(LeftX, UpY, baseColor));
                    source = new Rectangle(40, 0, 8, 8);
                    Main.spriteBatch.Draw(t, new Rectangle(LeftX * 16 - ScPosX + 8, UpY * 16 - ScPosY, 8, 8), source, GetTileColor(LeftX, UpY, baseColor));
                    source = new Rectangle(0, 40, 8, 8);
                    Main.spriteBatch.Draw(t, new Rectangle(LeftX * 16 - ScPosX, UpY * 16 - ScPosY + 8, 8, 8), source, GetTileColor(LeftX, UpY, baseColor));
                    source = new Rectangle(40, 40, 8, 8);
                    Main.spriteBatch.Draw(t, new Rectangle(LeftX * 16 - ScPosX + 8, UpY * 16 - ScPosY + 8, 8, 8), source, GetTileColor(LeftX, UpY, baseColor));
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                    return;
                }
                for (int y = UpY; y < DownY + 1; y++)
                {
                    int ScPosX = (int)Main.screenPosition.X;
                    int ScPosY = (int)Main.screenPosition.Y;
                    Rectangle source = new Rectangle(0, 16, 8, 16);
                    Rectangle source2 = new Rectangle(40, 16, 8, 16);
                    if (y == UpY)
                    {
                        source.Y = 0;
                        source2.Y = 0;
                    }
                    if (y == DownY)
                    {
                        source.Y = 32;
                        source2.Y = 32;
                    }
                    Main.spriteBatch.Draw(t, new Rectangle(LeftX * 16 - ScPosX, y * 16 - ScPosY, 8, 16), source, GetTileColor(LeftX, y, baseColor));
                    Main.spriteBatch.Draw(t, new Rectangle(LeftX * 16 - ScPosX + 8, y * 16 - ScPosY, 8, 16), source2, GetTileColor(LeftX, y, baseColor));
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                return;
            }
            if (UpY == DownY)
            {
                for(int x = LeftX; x < RightX + 1; x++)
                {
                    int ScPosX = (int)Main.screenPosition.X;
                    int ScPosY = (int)Main.screenPosition.Y;
                    Rectangle source = new Rectangle(16, 0, 16, 8);
                    Rectangle source2 = new Rectangle(16, 40, 16, 8);
                    if (x == LeftX)
                    {
                        source.X = 0;
                        source2.X = 0;
                    }
                    if (x == RightX)
                    {
                        source.X = 32;
                        source2.X = 32;
                    }
                    Main.spriteBatch.Draw(t, new Rectangle(x * 16 - ScPosX, UpY * 16 - ScPosY, 16, 8), source, GetTileColor(x, UpY, baseColor));
                    Main.spriteBatch.Draw(t, new Rectangle(x * 16 - ScPosX, UpY * 16 - ScPosY + 8, 16, 8), source2, GetTileColor(x, UpY, baseColor));
                }
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
                return;
            }


            for (int x = LeftX; x < RightX + 1; x++)
            {
                for (int y = UpY; y < DownY + 1; y++)
                {
                    int ScPosX = (int)Main.screenPosition.X;
                    int ScPosY = (int)Main.screenPosition.Y;
                    Rectangle source = new Rectangle(16, 16, 16, 16);
                    if (x == LeftX)
                    {
                        source.X = 0;
                    }
                    if (x == RightX)
                    {
                        source.X = 32;
                    }
                    if (y == UpY)
                    {
                        source.Y = 0;
                    }
                    if (y == DownY)
                    {
                        source.Y = 32;
                    }
                    Main.spriteBatch.Draw(t, new Rectangle(x * 16 - ScPosX, y * 16 - ScPosY, 16, 16), source, GetTileColor(x, y, baseColor));
                }
            }
            Main.spriteBatch.End();
            Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
        }
        private Color GetTileColor(int i, int j, Color baseColor)
        {
            if (Main.tile[i, j].HasTile)
            {
                if (Main.tile[i, j].WallType > 0)
                {
                    return new Color(255, 255, 0, 200);
                }
                return new Color(100, 100, 0, 10);
            }
            if (Main.tile[i, j].WallType > 0)
            {
                return new Color(255, 55, 0, 10);
            }

            return baseColor;
        }
        public override void Kill(int timeLeft)
        {
            int LeftX = (int)(StartPoint.X / 16f);
            int RightX = (int)(Main.MouseWorld.X / 16f);
            int UpY = (int)(StartPoint.Y / 16f);
            int DownY = (int)(Main.MouseWorld.Y / 16f);
            if (LeftX > RightX)
            {
                int exchange = RightX;
                RightX = LeftX;
                LeftX = exchange;
            }
            if (UpY > DownY)
            {
                int exchange = DownY;
                DownY = UpY;
                UpY = exchange;
            }
            MapIO mapIO = new MapIO(LeftX, UpY, RightX - LeftX + 1, DownY - UpY + 1);
            int Count = 0;
            while (File.Exists("MapTiles" + Count.ToString() + ".mapio"))
            {
                Count++;
            }
            mapIO.Write("MapTiles" + Count.ToString() + ".mapio");
        }
    }
}
