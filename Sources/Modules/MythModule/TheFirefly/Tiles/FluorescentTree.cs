using Everglow.Sources.Modules.MythModule.Common;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class FluorescentTree : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileSolid[Type] = false;
            Main.tileLavaDeath[Type] = false;
            Main.tileFrameImportant[Type] = true;
            Main.tileBlockLight[Type] = false;
            Main.tileLighted[Type] = true;
            Main.tileAxe[Type] = true;

            TileObjectData.addTile(Type);

            ModTranslation modTranslation = LocalizationLoader.GetOrCreateTranslation("Mods.Everglow.MapEntry.FireflyTree");
            AddMapEntry(new Color(51, 26, 58), modTranslation);
            DustType = ModContent.DustType<TheFirefly.Dusts.MothBlue2>();
            ItemDrop = ModContent.ItemType<Items.GlowWood>();
            AdjTiles = new int[] { Type };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)//被砍爆的时候更新
        {

        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
            {
                int Dy = -1;
                while (Main.tile[i, j + Dy].TileType == Type && Dy > -100)
                {
                    Tile tileLeft = Main.tile[i - 1, j + Dy];
                    Tile tileRight = Main.tile[i + 1, j + Dy];
                    WorldGen.KillTile(i, j + Dy);
                    if (tileLeft.TileType == Type)
                    {
                        WorldGen.KillTile(i - 1, j + Dy);
                    }
                    if (tileRight.TileType == Type)
                    {
                        WorldGen.KillTile(i + 1, j + Dy);
                    }
                    if (Dy == -1)
                    {
                        tileLeft = Main.tile[i - 1, j];
                        if (tileLeft.TileType == Type)
                        {
                            WorldGen.KillTile(i - 1, j);
                        }
                        tileRight = Main.tile[i + 1, j];
                        if (tileRight.TileType == Type)
                        {
                            WorldGen.KillTile(i + 1, j);
                        }
                    }
                    Dy -= 1;
                }
            }
        }
        //public override void NearbyEffects(int i, int j, bool closer)
        //{
        //    Tile tile = Main.tile[i, j];
        //    if (tile.TileFrameY <= 3)
        //    {
        //        if(!Main.tile[i, j + 1].HasTile)
        //        {
        //            WorldGen.KillTile(i, j);
        //        }
        //    }
        //    if (tile.TileFrameY == 4)
        //    {
        //        if (!Main.tile[i + 1, j].HasTile || Main.tile[i + 1, j].TileType != Type)
        //        {
        //            WorldGen.KillTile(i, j);
        //        }
        //    }
        //    if (tile.TileFrameY == 5)
        //    {
        //        if (!Main.tile[i - 1, j].HasTile || Main.tile[i + 1, j].TileType != Type)
        //        {
        //            WorldGen.KillTile(i, j);
        //        }
        //    }
        //}
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D treeTexture = MythContent.QuickTexture("TheFirefly/Tiles/FluorescentTree");

            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            Tile tile = Main.tile[i, j];

            int Width;
            int Height = 16;
            int TexCoordY;
            int OffsetY = 20;
            int OffsetX = 0;
            float Rot = 0;
            Color color = Lighting.GetColor(i, j);
            switch (tile.TileFrameY)
            {
                default:
                    return false;
                case 0:  //树桩
                    Width = 74;
                    Height = 24;
                    TexCoordY = 180;
                    break;
                case 1:  //树干
                    Width = 26;
                    TexCoordY = 2;
                    break;
                case 2:  //树冠
                    Width = 150;
                    Height = 132;
                    TexCoordY = 46;
                    float Wind = Main.windSpeedCurrent / 5f;
                    Rot = Wind + (float)(Math.Sin(j + Main.timeForVisualEffects / 30f)) * Wind * 0.3f;
                    OffsetY = 24;
                    break;
                case 3:  //粗树干
                    Width = 50;
                    Height = 24;
                    TexCoordY = 20;
                    OffsetY = 28;
                    break;
                case 4:  //左树枝
                    Width = 34;
                    Height = 32;
                    OffsetX = -8;
                    TexCoordY = 240;
                    break;
                case 5:  //右树枝
                    Width = 34;
                    Height = 32;
                    OffsetX = 8;
                    TexCoordY = 206;
                    break;
            }
            Vector2 origin = new Vector2(Width / 2f, Height);
            spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY, Width, Height), color, Rot, origin,1,SpriteEffects.None,0);
            spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY + 274, Width, Height), new Color(1f, 1f, 1f, 0), Rot, origin, 1, SpriteEffects.None, 0);
            return false;
        }
    }
    public class UpdateMouseRightClickSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            if(Main.mouseRight && Main.mouseRightRelease)
            {
                int i = (int)(Main.MouseWorld.X / 16);
                int j = (int)(Main.MouseWorld.Y / 16);
                int Height = Main.rand.Next(6, 20);
                for(int g = 0;g < Height; g++)
                {
                    Tile tile = Main.tile[i, j - g];
                    if(g > 3)
                    {
                        if (Main.rand.NextBool(5))
                        {
                            Tile tileLeft = Main.tile[i - 1, j - g];
                            tileLeft.TileType = (ushort)ModContent.TileType<Tiles.FluorescentTree>();
                            tileLeft.TileFrameY = 4;
                            tileLeft.TileFrameX = (short)Main.rand.Next(4);
                            tileLeft.HasTile = true;
                        }
                        if (Main.rand.NextBool(5))
                        {
                            Tile tileRight = Main.tile[i + 1, j - g];
                            tileRight.TileType = (ushort)ModContent.TileType<Tiles.FluorescentTree>();
                            tileRight.TileFrameY = 5;
                            tileRight.TileFrameX = (short)Main.rand.Next(4);
                            tileRight.HasTile = true;
                        }
                    }
                    if (g == 0)
                    {
                        tile.TileType = (ushort)ModContent.TileType<Tiles.FluorescentTree>();
                        tile.TileFrameY = 0;
                        tile.HasTile = true;
                        continue;
                    }
                    if (g == 1)
                    {
                        tile.TileType = (ushort)ModContent.TileType<Tiles.FluorescentTree>();
                        tile.TileFrameY = -1;
                        tile.HasTile = true;
                        continue;
                    }
                    if (g == 2)
                    {
                        tile.TileType = (ushort)ModContent.TileType<Tiles.FluorescentTree>();
                        tile.TileFrameY = 3;
                        tile.TileFrameX = (short)Main.rand.Next(4);
                        tile.HasTile = true;
                        continue;
                    }
                    if (g == Height - 1)
                    {
                        tile.TileType = (ushort)ModContent.TileType<Tiles.FluorescentTree>();
                        tile.TileFrameY = 2;
                        tile.HasTile = true;
                        continue;
                    }
                    tile.TileType = (ushort)ModContent.TileType<Tiles.FluorescentTree>();
                    tile.TileFrameY = 1;
                    tile.TileFrameX = (short)Main.rand.Next(12);
                    tile.HasTile = true;
                }
            }
        }
    }
}