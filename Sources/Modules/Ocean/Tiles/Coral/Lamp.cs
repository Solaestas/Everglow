using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MythMod.Tiles.Ocean
{
    public class Lamp : ModTile
    {
        public override void SetDefaults()
        {
            Main.tileLighted[(int)base.Type] = true;
            Main.tileFrameImportant[(int)base.Type] = true;
            Main.tileNoAttach[(int)base.Type] = true;
            Main.tileLavaDeath[(int)base.Type] = true;
            Main.tileWaterDeath[(int)base.Type] = false;
            TileObjectData.newTile.Width = 6;
            TileObjectData.newTile.Height = 5;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                16
            };
            TileObjectData.newTile.CoordinateWidth = 16;
            TileObjectData.newTile.CoordinatePadding = 2;
            TileObjectData.newTile.Origin = new Point16(1, 0);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.newTile.AnchorTop = new AnchorData((Terraria.Enums.AnchorType)1, 1, 1);
            TileObjectData.addTile((int)base.Type);
            base.AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("灯塔巨灯");
            base.AddMapEntry(new Color(191, 142, 111), modTranslation);
            this.adjTiles = new int[]
            {
                4
            };
            modTranslation.AddTranslation(GameCulture.Chinese, "灯塔巨灯");
        }
        private int D = 0;
        private int E = 0;
        private int F = 0;
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if(Main.dayTime && D == 0)
            {
                D = 1;
                for(int X = i - 8;X < i + 8;X ++)
                {
                    for (int Y = j - 6; Y < j + 6; Y++)
                    {
                        if(Main.tile[X,Y].type == mod.TileType("Lamp"))
                        {
                            Main.tile[X, Y].frameY -= (short)((((Main.tile[X, Y].frameY + 45) / 90) - 1) * 90);
                        }
                    }
                }
            }
            if (!Main.dayTime && D == 1)
            {
                D = 0;
                for (int X = i - 8; X < i + 8; X++)
                {
                    for (int Y = j - 6; Y < j + 6; Y++)
                    {
                        if (Main.tile[X, Y].type == mod.TileType("Lamp") && Main.tile[X, Y].frameY < 90)
                        {
                            Main.tile[X, Y].frameY += 90;
                        }
                    }
                }
            }
            if (!Main.dayTime && E == 10 && F == 0)
            {
                F = 1;
                for (int X = i - 8; X < i + 8; X++)
                {
                    for (int Y = j - 6; Y < j + 6; Y++)
                    {
                        if (Main.tile[X, Y].type == mod.TileType("Lamp") && Main.tile[X, Y].frameY < 180)
                        {
                            Main.tile[X, Y].frameY += 90;
                        }
                    }
                }
            }
            if (!Main.dayTime && E == 20 && F == 1)
            {
                F = 2;
                for (int X = i - 8; X < i + 8; X++)
                {
                    for (int Y = j - 6; Y < j + 6; Y++)
                    {
                        if (Main.tile[X, Y].type == mod.TileType("Lamp") && Main.tile[X, Y].frameY < 270)
                        {
                            Main.tile[X, Y].frameY += 90;
                        }
                    }
                }
            }
            if (!Main.dayTime && E == 30 && F == 2)
            {
                F = 3;
                for (int X = i - 8; X < i + 8; X++)
                {
                    for (int Y = j - 6; Y < j + 6; Y++)
                    {
                        if (Main.tile[X, Y].type == mod.TileType("Lamp") && Main.tile[X, Y].frameY < 360)
                        {
                            Main.tile[X, Y].frameY += 90;
                        }
                    }
                }
            }
            if (!Main.dayTime && E == 40 && F == 3)
            {
                F = 4;
                for (int X = i - 8; X < i + 8; X++)
                {
                    for (int Y = j - 6; Y < j + 6; Y++)
                    {
                        if (Main.tile[X, Y].type == mod.TileType("Lamp") && Main.tile[X, Y].frameY < 450)
                        {
                            Main.tile[X, Y].frameY += 90;
                        }
                    }
                }
            }
            if (!Main.dayTime && E == 50 && F == 4)
            {
                F = 5;
                for (int X = i - 8; X < i + 8; X++)
                {
                    for (int Y = j - 6; Y < j + 6; Y++)
                    {
                        if (Main.tile[X, Y].type == mod.TileType("Lamp") && Main.tile[X, Y].frameY < 540)
                        {
                            Main.tile[X, Y].frameY += 90;
                        }
                    }
                }
            }
            if (!Main.dayTime && E == 60 && F == 5)
            {
                F = 0;
                for (int X = i - 8; X < i + 8; X++)
                {
                    for (int Y = j - 6; Y < j + 6; Y++)
                    {
                        if (Main.tile[X, Y].type == mod.TileType("Lamp") && Main.tile[X, Y].frameY < 630)
                        {
                            Main.tile[X, Y].frameY -= 450;
                        }
                    }
                }
            }
            if (!Main.dayTime)
            {
                if (E < 60)
                {
                    E += 1;
                }
                else
                {
                    E = 0;
                }
            }
            else
            {
                E = 0;
            }
        }
        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 32, base.mod.ItemType("LighthouseLamp"));
        }
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            if (Main.tile[i, j].frameY >= 108)
            {
                r = 1f;
                g = 0.8f;
                b = 0.7f;
                return;
            }
            r = 0f;
            g = 0f;
            b = 0f;
        }
        public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Tile tile = Main.tile[i, j];
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            int height = 16;
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            if (false)
            {
                //Main.spriteBatch.Draw(mod.GetTexture("Tiles/Ocean/海洋封印台Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), new Color(mplayer.movieTime / 120f, mplayer.movieTime / 120f, mplayer.movieTime / 120f, 0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            if (false)
            {
                //Main.spriteBatch.Draw(mod.GetTexture("Tiles/Ocean/海洋封印台Glow"), new Vector2(i * 16 - (int)Main.screenPosition.X, j * 16 - (int)Main.screenPosition.Y) + zero, new Rectangle(tile.frameX, tile.frameY, 16, height), new Color(255, 255, 255, 0), 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
