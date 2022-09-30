using Everglow.Sources.Modules.YggdrasilModule.Common.Utils;
using Everglow.Sources.Modules.YggdrasilModule.Common;
using Terraria.DataStructures;
using Everglow.Sources.Commons.Core.Utils;
using Terraria.Enums;
using Terraria.ObjectData;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles
{
    public class Chinalamp3 : ModTile
    {
        public override void SetStaticDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileLighted[Type] = true;
            Main.tileSolid[Type] = false;
            Main.tileNoFail[Type] = true;
            TileID.Sets.HasOutlines[Type] = true;
            TileID.Sets.CanBeSleptIn[Type] = true; // Facilitates calling ModifySleepingTargetInfo
            TileID.Sets.InteractibleByNPCs[Type] = true; // Town NPCs will palm their hand at this tile
            TileID.Sets.IsValidSpawnPoint[Type] = true;

            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTorch);

            DustType = DustID.Pearlwood;
            AdjTiles = new int[] { TileID.Chandeliers };

            // Placement
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x3); 
            TileObjectData.newTile.AnchorTop = new AnchorData(AnchorType.SolidTile, TileObjectData.newTile.Width, 0);
            TileObjectData.newTile.AnchorBottom = default(AnchorData);
            TileObjectData.newTile.CoordinateHeights = new[] { 16, 16, 16 };
            TileObjectData.addTile(Type);

            // Etc
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Chandelier");
            AddMapEntry(new Color(216, 172, 125), name);
        }
        public override void HitWire(int i, int j)
        {
            FurnitureUtils.LightHitwire(i, j, Type, 3, 3);
        }
    
        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            var tile = Main.tile[i, j];
            if (tile.TileFrameX < 54)
            {
                r = 0.8f;
                g = 0.75f;
                b = 0.4f;
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if(closer)
            {
                var tile = Main.tile[i, j];
                if (tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0)
                {
                    foreach (Player player in Main.player)
                    {
                        if (player.Hitbox.Intersects(new Rectangle(i * 16 - 14, j * 16 + 18, 12, 30)))
                        {
                            if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j)))
                            {
                                TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                            }
                            else
                            {
                                float rot;
                                float Omega;
                                Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j)].X;
                                rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j)].Y;
                                if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                                {
                                    TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                                }
                                if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                                {
                                    TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j));
                                }
                            }
                        }



                        if (player.Hitbox.Intersects(new Rectangle(i * 16 + 2, j * 16 + 16, 12, 28)))
                        {
                            if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j)))
                            {
                                TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                            }
                            else
                            {
                                float rot;
                                float Omega;
                                Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j)].X;
                                rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j)].Y;
                                if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                                {
                                    TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                                }
                                if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                                {
                                    TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j));
                                }
                            }
                        }

                        if (player.Hitbox.Intersects(new Rectangle(i * 16 - 4, j * 16 + 10, 12, 32)))
                        {
                            if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j + 1)))
                            {
                                TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j + 1), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                            }
                            else
                            {
                                float rot;
                                float Omega;
                                Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 1)].X;
                                rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 1)].Y;
                                if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                                {
                                    TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 1)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                                }
                                if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                                {
                                    TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j + 1));
                                }
                            }
                        }


                        if (player.Hitbox.Intersects(new Rectangle(i * 16 + 8, j * 16 + 28, 12, 32)))
                        {
                            if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j)))
                            {
                                TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                            }
                            else
                            {
                                float rot;
                                float Omega;
                                Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j)].X;
                                rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j)].Y;
                                if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                                {
                                    TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                                }
                                if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                                {
                                    TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j));
                                }
                            }
                        }

                        if (player.Hitbox.Intersects(new Rectangle(i * 16 + 8, j * 16 + 28, 12, 32)))
                        {
                            if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j + 2)))
                            {
                                TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j + 2), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                            }
                            else
                            {
                                float rot;
                                float Omega;
                                Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 2)].X;
                                rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 2)].Y;
                                if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                                {
                                    TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j + 2)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                                }
                                if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                                {
                                    TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j + 2));
                                }
                            }
                        }
                    }
                }
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];
            int Adx = 0;
            if(tile.TileFrameX > 54)
            {
                Adx = 180;
            }
            if (tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0)
            {
                TileSpin tileSpin = new TileSpin();
                Texture2D tex = YggdrasilContent.QuickTexture("YggdrasilTown/Tiles/Chinalamp3_Depart");

                tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18, j + 2);
                tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18, j + 2, tex, new Rectangle(144 + Adx, 0, 36, 64), new Vector2(18, 0), 0, -34, 1);

                tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j);
                tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j, tex, new Rectangle(108 + Adx, 0, 36, 64), new Vector2(18, 0), 16, -2, 1);

                tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18, j + 1);
                tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18, j + 1, tex, new Rectangle(72 + Adx, 0, 36, 64), new Vector2(18, 0), 0, -18, 1);

                tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j);
                tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j, tex, new Rectangle(36 + Adx, 0, 36, 64), new Vector2(18, 0), -16, -2, 1);

                tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18, j);
                tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18, j, tex, new Rectangle(0 + Adx, 0, 36, 64), new Vector2(18, 0), 0, -2, 1);
      
            }
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }

            //Rectangle rc = Main.LocalPlayer.Hitbox;
            //rc.X -= (int)(Main.screenPosition.X - zero.X);
            //rc.Y -= (int)(Main.screenPosition.Y - zero.Y);
            //if (tile.TileFrameY == 0)
            //{
            //    if (tile.TileFrameX == 18)
            //    {
            //        Rectangle rc2 = new Rectangle(i * 16 - 14, j * 16 + 18, 12, 30);
            //        rc2.X -= (int)(Main.screenPosition.X - zero.X);
            //        rc2.Y -= (int)(Main.screenPosition.Y - zero.Y);
            //        spriteBatch.Draw(TextureAssets.MagicPixel.Value, rc2, new Color(0.5f, 0, 0, 0));
            //        rc2 = new Rectangle(i * 16 + 2, j * 16 + 16, 12, 28);
            //        rc2.X -= (int)(Main.screenPosition.X - zero.X);
            //        rc2.Y -= (int)(Main.screenPosition.Y - zero.Y);
            //        spriteBatch.Draw(TextureAssets.MagicPixel.Value, rc2, new Color(0.5f, 0, 0, 0));
            //        rc2 = new Rectangle(i * 16 - 4, j * 16 + 10, 12, 32);
            //        rc2.X -= (int)(Main.screenPosition.X - zero.X);
            //        rc2.Y -= (int)(Main.screenPosition.Y - zero.Y);
            //        spriteBatch.Draw(TextureAssets.MagicPixel.Value, rc2, new Color(0.5f, 0, 0, 0));
            //        rc2 = new Rectangle(i * 16 - 8, j * 16 + 28, 12, 30);
            //        rc2.X -= (int)(Main.screenPosition.X - zero.X);
            //        rc2.Y -= (int)(Main.screenPosition.Y - zero.Y);
            //        spriteBatch.Draw(TextureAssets.MagicPixel.Value, rc2, new Color(0.5f, 0, 0, 0));
            //        rc2 = new Rectangle(i * 16 - 10, j * 16 + 14, 12, 32);
            //        rc2.X -= (int)(Main.screenPosition.X - zero.X);
            //        rc2.Y -= (int)(Main.screenPosition.Y - zero.Y);
            //        spriteBatch.Draw(TextureAssets.MagicPixel.Value, rc2, new Color(0.5f, 0, 0, 0));
            //    }
            //}
            //spriteBatch.Draw(TextureAssets.MagicPixel.Value,rc,new Color(0.5f,0,0,0));


            return false;
        }
        public override void KillMultiTile(int x, int y, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 48, 32, ModContent.ItemType<Items.Chinalamp3>());
        }
    }
}
