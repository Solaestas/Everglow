using Everglow.Sources.Commons.Core.Utils;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class LampLotus : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[Type] = false;
            Main.tileNoAttach[Type] = true;
            Main.tileCut[Type] = true;

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 1;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateWidth = 28;
            TileObjectData.addTile(Type);
            DustType = 191;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(81, 110, 255), modTranslation);
            HitSound = SoundID.Grass;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
        }

        public override void RandomUpdate(int i, int j)
        {
            var tile = Main.tile[i, j];
            var tile2 = Main.tile[i, j - 1];

            if (tile2.TileType != tile.TileType)
            {
                int length = 0;
                while (Main.tile[i, j + length].TileType == tile.TileType && Main.tile[i, j + length].LiquidAmount == 0)
                {
                    length++;
                }
                if (length <= 9)
                {
                    tile2.TileType = (ushort)(ModContent.TileType<Tiles.LampLotus>());
                    tile2.HasTile = true;
                }
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            if (closer)
            {
                var tile = Main.tile[i, j];
                if (Main.tile[i, j + 1].TileType != tile.TileType)
                {
                    int length = 0;
                    while (Main.tile[i, j - length].TileType == tile.TileType)
                    {
                        length++;
                    }
                    foreach (Player player in Main.player)
                    {
                        if (player.Hitbox.Intersects(new Rectangle(i * 16, (j - length) * 16, 16, 16 * length)))
                        {
                            if (!TileSpin.TileRotation.ContainsKey((i, j)))
                            {
                                TileSpin.TileRotation.Add((i, j), new Vector2(Math.Clamp(player.velocity.X * 0.02f, -1, 1) * 0.2f));
                            }
                            else
                            {
                                float rot;
                                float Omega;
                                Omega = TileSpin.TileRotation[(i, j)].X;
                                rot = TileSpin.TileRotation[(i, j)].Y;
                                if (Math.Abs(Omega) < 0.4f && Math.Abs(rot) < 0.4f)
                                {
                                    TileSpin.TileRotation[(i, j)] = new Vector2(Omega + Math.Clamp(player.velocity.X * 0.02f, -1, 1) * 2f, rot + Omega + Math.Clamp(player.velocity.X * 0.02f, -1, 1) * 2f);
                                }
                                if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                                {
                                    TileSpin.TileRotation.Remove((i, j));
                                }
                            }
                        }
                        if (Main.tile[i, j - length].WallType == 0)
                        {
                            if (!TileSpin.TileRotation.ContainsKey((i, j)))
                            {
                                TileSpin.TileRotation.Add((i, j), new Vector2(Math.Clamp(Main.windSpeedCurrent, -1, 1) * (0.3f + MathUtils.Sin(i + (float)Main.time / 24f) * 0.2f)));
                            }
                            else
                            {
                                float rot;
                                float Omega;
                                Omega = TileSpin.TileRotation[(i, j)].X;
                                rot = TileSpin.TileRotation[(i, j)].Y;
                                if (Math.Abs(Omega) < 4f && Math.Abs(rot) < 4f)
                                {
                                    TileSpin.TileRotation[(i, j)] = new Vector2(Omega * 0.999f + (Math.Clamp(Main.windSpeedCurrent, -1, 1) * (0.3f + MathUtils.Sin(i + (float)Main.time / 24f) * 0.1f)) * 0.002f, rot * 0.999f + (Math.Clamp(Main.windSpeedCurrent, -1, 1) * (0.3f + MathUtils.Sin(i + (float)Main.time / 24f) * 0.1f)) * 0.002f);
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
            if (Main.tile[i, j + 1].TileType != tile.TileType)
            {
                int length = 0;
                while (Main.tile[i, j - length].TileType == tile.TileType)
                {
                    length++;
                }
                Texture2D texflower = MythContent.QuickTexture("TheFirefly/Tiles/LampLotus");
                Texture2D texflowerGlow = MythContent.QuickTexture("TheFirefly/Tiles/LampLotusGlow");
                TileSpin tsp = new TileSpin();
                tsp.Update(i, j);
                tsp.DrawReed(i, j, length, texflower, texflower, new Rectangle(tile.TileFrameX, 0, 28, 34), new Rectangle(tile.TileFrameX, 36, 28, 16), new Vector2(14, 34), new Vector2(14, 16), 8, 16);
                tsp.DrawReed(i, j, length, texflower, texflowerGlow, new Rectangle(tile.TileFrameX, 0, 28, 34), new Rectangle(tile.TileFrameX, 36, 28, 16), new Vector2(14, 34), new Vector2(14, 16), 8, 16, 1, true, new Color(0, 155, 255, 0));
            }
            return false;
        }
    }
}