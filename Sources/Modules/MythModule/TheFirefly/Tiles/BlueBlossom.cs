using Everglow.Sources.Modules.MythModule.Common;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class BlueBlossom : ModTile
    {
        public override void PostSetDefaults()
        {
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.Height = 3;
            TileObjectData.newTile.Width = 1;
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                16,
                18
            };
            TileObjectData.newTile.CoordinateWidth = 60;
            TileObjectData.addTile(Type);
            DustType = 191;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(81, 110, 255), modTranslation);
            HitSound = SoundID.Grass;
        }

        public override void NumDust(int i, int j, bool fail, ref int num)
        {
            num = (fail ? 1 : 3);
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            for (int x = 0; x < 5; x++)
            {
                Item.NewItem(null, i * 16, j * 16, 16, 32, ModContent.ItemType<Items.GlowingPedal>());
            }
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
            short num = (short)(Main.rand.Next(0, 6));
            Main.tile[i, j].TileFrameX = (short)(num * 48);
            Main.tile[i, j + 1].TileFrameX = (short)(num * 48);
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
            Lighting.AddLight(i, j, 0.1f, 0.5f, 1.2f);
            if (closer)
            {
                var tile = Main.tile[i, j];
                foreach (Player player in Main.player)
                {
                    if (player.Hitbox.Intersects(new Rectangle(i * 16, j * 16, 16, 16)))
                    {
                        if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 16 + 1)))
                        {
                            TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 16 + 1), new Vector2(Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                        }
                        else
                        {
                            float rot;
                            float Omega;
                            Omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 1)].X;
                            rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 1)].Y;
                            if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                            {
                                TileSpin.TileRotation[(i, j - tile.TileFrameY / 16 + 1)] = new Vector2(Omega + Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega + Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                            }
                            if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                            {
                                TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 16 + 1));
                            }
                        }

                        if (!TileSpin.TileRotation.ContainsKey((i, j - tile.TileFrameY / 16)))
                        {
                            TileSpin.TileRotation.Add((i, j - tile.TileFrameY / 16), new Vector2(Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                        }
                        else
                        {
                            float rot;
                            float Omega;
                            Omega = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16)].X;
                            rot = TileSpin.TileRotation[(i, j - tile.TileFrameY / 16)].Y;
                            if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                            {
                                TileSpin.TileRotation[(i, j - tile.TileFrameY / 16)] = new Vector2(Omega + Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega + Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                            }
                            if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                            {
                                TileSpin.TileRotation.Remove((i, j - tile.TileFrameY / 16));
                            }
                        }
                    }
                }
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];
            if (tile.TileFrameY == 16)
            {
                TileSpin tileSpin = new TileSpin();
                tileSpin.UpdateBlackShrub(i, j - tile.TileFrameY / 16, 0.25f, 0.13f, new Vector2(0, -20), 0, 24);
                tileSpin.Update(i, j - tile.TileFrameY / 16 + 1);
                Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/BlueBlossomDraw");
                tileSpin.DrawRotatedTile(i, j - tile.TileFrameY / 16, tex, new Rectangle(tile.TileFrameX, 0, 120, 108), new Vector2(60, 108), 16, 54, false);
                tex = MythContent.QuickTexture("TheFirefly/Tiles/BlueBlossomGlow");
                tileSpin.DrawRotatedTile(i, j - tile.TileFrameY / 16, tex, new Rectangle(tile.TileFrameX, 0, 120, 108), new Vector2(60, 108), 16, 54, true, new Color(5, 5, 5, 0));
            }
            return false;
        }
    }
}