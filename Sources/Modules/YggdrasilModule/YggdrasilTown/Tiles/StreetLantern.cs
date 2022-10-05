using Everglow.Sources.Commons.Core.Utils;
using Everglow.Sources.Modules.YggdrasilModule.Common.Utils;
using Everglow.Sources.Modules.YggdrasilModule.Common;
using Terraria.ObjectData;
using Terraria.Enums;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.YggdrasilModule.YggdrasilTown.Tiles
{
    public class StreetLantern : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 6;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                16,
                16,
                16,
                19
            };
            TileObjectData.newTile.CoordinateWidth = 48;
            TileObjectData.newTile.Direction = TileObjectDirection.PlaceLeft;
            TileObjectData.newTile.CoordinatePaddingFix = new Point16(0, -3);
            // The following 3 lines are needed if you decide to add more styles and stack them vertically
            TileObjectData.newTile.StyleWrapLimit = 2;
            TileObjectData.newTile.StyleMultiplier = 2;
            TileObjectData.newTile.StyleHorizontal = false;

            TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
            TileObjectData.newAlternate.Direction = TileObjectDirection.PlaceRight;
            TileObjectData.addAlternate(1); // Facing right will use the second texture style
            TileObjectData.addTile(Type);

            DustType = DustID.DynastyWood;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(151, 31, 32), modTranslation);
        }

        public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
        {
            var tile = Main.tile[i, j];
            if (tile.TileFrameX < 48)
            {
                r = 1f;
                g = 0.15f;
                b = 0f;
            }
            else
            {
                r = 0f;
                g = 0f;
                b = 0f;
            }
        }
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.StreetLantern>());
        }
        public override void HitWire(int i, int j)
        {
            FurnitureUtils.LightHitwire(i, j, Type, 1, 6, 48);
        }
        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                var tile = Main.tile[i, j];
                if (tile.TileFrameY % 108 <= 36)
                {
                    foreach (Player player in Main.player)
                    {
                        if (player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
                        {
                            if (!TileSpin.TileRotation.ContainsKey((i, j - (tile.TileFrameY % 108) / 18)))
                            {
                                TileSpin.TileRotation.Add((i, j - (tile.TileFrameY % 108) / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                            }
                            else
                            {
                                float rot;
                                float Omega;
                                Omega = TileSpin.TileRotation[(i, j - (tile.TileFrameY % 108) / 18)].X;
                                rot = TileSpin.TileRotation[(i, j - (tile.TileFrameY % 108) / 18)].Y;
                                float mass = 24f;
                                float MaxSpeed = Math.Abs(Math.Clamp(player.velocity.X / mass, -0.5f, 0.5f));
                                if (Math.Abs(Omega) < MaxSpeed && Math.Abs(rot) < MaxSpeed)
                                {
                                    TileSpin.TileRotation[(i, j - (tile.TileFrameY % 108) / 18)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                                }
                                if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                                {
                                    TileSpin.TileRotation.Remove((i, j - (tile.TileFrameY % 108) / 18));
                                }
                            }
                        }
                        if(Main.tile[i, j].WallType == 0)
                        {
                            if (!TileSpin.TileRotation.ContainsKey((i, j - (tile.TileFrameY % 108) / 18)))
                            {
                                TileSpin.TileRotation.Add((i, j - (tile.TileFrameY % 108) / 18), new Vector2(Main.windSpeedCurrent * 0.2f, 0));
                            }
                        }
                    }
                }
            }
        }
        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            Texture2D tPostTexture = YggdrasilContent.QuickTexture("YggdrasilTown/Tiles/StreetLantern_Post");
            Rectangle rt = new Rectangle(i * 16 - 16, j * 16, 48, 16);
            Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
            if (Main.drawToScreen)
            {
                zero = Vector2.Zero;
            }
            rt.X -= (int)(Main.screenPosition.X - zero.X);
            rt.Y -= (int)(Main.screenPosition.Y - zero.Y);
            Tile tile = Main.tile[i, j];

            spriteBatch.Draw(tPostTexture, rt, new Rectangle(tile.TileFrameX, tile.TileFrameY, 48, 16), Lighting.GetColor(i, j));

            if ((tile.TileFrameY % 108) == 0 && (tile.TileFrameX == 0 || tile.TileFrameX == 48))
            {
                TileSpin tileSpin = new TileSpin();
                tileSpin.Update(i, j);
                Texture2D tex = YggdrasilContent.QuickTexture("YggdrasilTown/Tiles/StreetLantern_Lantern");
                float OffsetX = 4;
                if (tile.TileFrameY != 0)
                {
                    OffsetX = 14;
                }
                tileSpin.DrawRotatedTile(i, j, tex, new Rectangle(tile.TileFrameX / 48 * 36, tile.TileFrameY / 108 * 32, 36, 32), new Vector2(18, 0), OffsetX, 8);
                if(tile.TileFrameX == 0)
                {
                    Lighting.AddLight(i, j, 1f, 0.45f, 0.15f);
                    Lighting.AddLight(i, j + 1, 1f, 0.45f, 0.15f);
                }
            }
            return false;
        }
    }
}
