using Everglow.Sources.Modules.MythModule.Common;
using Terraria.ObjectData;
using Terraria.GameContent;

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
            DustType = ModContent.DustType<Dusts.FluorescentTreeDust>();
            ItemDrop = ModContent.ItemType<Items.GlowWood>();
            AdjTiles = new int[] { Type };
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)//被砍爆的时候更新
        {

        }
        public override bool CreateDust(int i, int j, ref int type)
        {
            
            return false;
        }
        public override bool Drop(int i, int j)
        {
            Item.NewItem(null,new Rectangle(i * 16 - 16, j * 16 ,48,16), ItemDrop,1,false,0,false,true);
            for (int x = 0; x < 6; x++)
            {
                Dust.NewDust(new Vector2(i * 16, j * 16), 16, 16, DustType,0,0,0,default,Main.rand.NextFloat(0.5f,1f));
            }
            var tile = Main.tile[i, j];
            if (tile.TileFrameY > 3)
            {
                Gore.NewGore(null, new Vector2(i * 16, j * 16) + new Vector2(Main.rand.Next(16), Main.rand.Next(16)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<FireflyTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
            }
            if (tile.TileFrameY == 2)
            {
                Main.NewText(j);
                for (int x = 0; x < 12; x++)
                {
                    Gore.NewGore(null, new Vector2(i * 16 - 32, j * 16 - 120) + new Vector2(Main.rand.Next(0), Main.rand.Next(0)), new Vector2(0, Main.rand.NextFloat(0f, 1f)).RotatedByRandom(6.283), ModContent.GoreType<FireflyTree_Leaf>(), Main.rand.NextFloat(0.65f, 1.45f));
                }
            }
            return false;
        }
        private void newDrop(int i, int j, int frameY)
        {

        }
        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem)
        {
            if (!fail)
            {
                int Dy = -1;//向上破坏的自变化Y坐标
                if (Main.tile[i, j].TileFrameY < 4)
                {
                    Tile tileLeft;
                    Tile tileRight;

                    while (Main.tile[i, j + Dy].TileType == Type && Dy > -100)
                    {
                        Drop(i, j + Dy);
                        tileLeft = Main.tile[i - 1, j + Dy];
                        tileRight = Main.tile[i + 1, j + Dy];
                        if (tileLeft.TileType == Type)
                        {
                            Drop(i - 1, j + Dy);
                        }
                        if (tileRight.TileType == Type)
                        {
                            Drop(i + 1, j + Dy);
                        }
                        Dy -= 1;
                    }


                    Dy = -1;//向上破坏的自变化Y坐标
                    tileLeft = Main.tile[i - 1, j];
                    if (tileLeft.TileType == Type)
                    {
                        tileLeft.HasTile = false;
                    }
                    tileRight = Main.tile[i + 1, j];
                    if (tileRight.TileType == Type)
                    {
                        tileRight.HasTile = false;
                    }
                    while (Main.tile[i, j + Dy].TileType == Type && Dy > -100)
                    {
                        Tile baseTile = Main.tile[i, j + Dy];

                        baseTile.HasTile = false;

                        tileLeft = Main.tile[i - 1, j + Dy];
                        tileRight = Main.tile[i + 1, j + Dy];
                        if (tileLeft.TileType == Type)
                        {

                            tileLeft.HasTile = false;
                        }
                        if (tileRight.TileType == Type)
                        {

                            tileRight.HasTile = false;
                        }
                        Dy -= 1;
                    }
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
            spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX + 8, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY, Width, Height), color, Rot, origin,1,SpriteEffects.None,0);
            spriteBatch.Draw(treeTexture, new Vector2(i * 16 + OffsetX + 8, j * 16 + OffsetY) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX * Width, TexCoordY + 274, Width, Height), new Color(1f, 1f, 1f, 0), Rot, origin, 1, SpriteEffects.None, 0);


            //int i0 = (int)(Main.MouseWorld.X / 16d);
            //int j0 = (int)(Main.MouseWorld.Y / 16d);
            //if ((i0, j0) == (i, j))
            //{
            //    Main.NewText(tile.TileFrameY, new Color(MathF.Sin((float)(Main.timeForVisualEffects / 10f)) * 0.5f + 0.5f, 0.5f, 0.5f));
            //    Main.NewText(tile.TileFrameX, new Color(0.2f, 0.5f, MathF.Sin((float)(Main.timeForVisualEffects / 10f)) * 0.5f + 0.5f));
            //    spriteBatch.Draw(TextureAssets.MagicPixel.Value, new Vector2(i * 16 + 8, j * 16 + 8) - Main.screenPosition + zero, new Rectangle(0, 0, 16, 16), new Color(1f, 0f, 0f, 0), 0, new Vector2(8), 1, SpriteEffects.None, 0);
            //}
            return false;
        }
    }
    public class UpdateMouseRightClickSystem : ModSystem
    {
        public override void PostUpdateEverything()
        {
            //Main.NewText(tile0.TileFrameY);
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
                        tile.TileFrameX = 0;
                        tile.HasTile = true;
                        continue;
                    }
                    if (g == 1)
                    {
                        tile.TileType = (ushort)ModContent.TileType<Tiles.FluorescentTree>();
                        tile.TileFrameY = -1;
                        tile.TileFrameX = 0;
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
                        tile.TileFrameX = 0;
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