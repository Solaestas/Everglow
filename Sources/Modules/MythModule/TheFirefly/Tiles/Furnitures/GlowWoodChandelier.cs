using Everglow.Sources.Commons.Core.Utils;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles.Furnitures
{
    public class GlowWoodChandelier : ModTile
    {
        private Asset<Texture2D> flameTexture;

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

            DustType = ModContent.DustType<BlueGlow>();
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
            AddMapEntry(new Color(0, 14, 175), name);

            if (!Main.dedServ)
            {
                flameTexture = ModContent.Request<Texture2D>("Everglow/Sources/Modules/MythModule/TheFirefly/Tiles/Furnitures/GlowWoodChandelier_Flame");
            }
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
                r = 0.1f;
                g = 0.9f;
                b = 1f;
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
            if (closer)
            {
                var tile = Main.tile[i, j];
                foreach (Player player in Main.player)
                {
                    if (player.Hitbox.Intersects(new Rectangle(i * 16 - 8, j * 16 - 8, 18, 18)))
                    {
                        if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)))
                        {
                            TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                        }
                        else
                        {
                            float rot;
                            float Omega;
                            Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)].X;
                            rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)].Y;
                            if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                            {
                                TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                            }
                            if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                            {
                                TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18));
                            }
                        }
                    }
                }
            }
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            var tile = Main.tile[i, j];
            if (tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0)
            {
                TileSpin tileSpin = new TileSpin();
                tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18);
                Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodChandelier");
                tileSpin.DrawRotatedChandelier(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18, tex, 8, -2);

                ulong randSeed = Main.TileFrameSeed ^ (ulong)((long)j << 32 | (uint)i); // Don't remove any casts.
                Color color = new Color(30, 30, 30, 0);
                if (tile.TileFrameX < 54)
                {
                    for (int k = 0; k < 7; k++)
                    {
                        float xx = Utils.RandomInt(ref randSeed, -10, 11) * 0.15f;
                        float yy = Utils.RandomInt(ref randSeed, -10, 1) * 0.35f;
                        tileSpin.DrawRotatedChandelier(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18, flameTexture.Value, xx + 8, yy * 0.6f - 2, true, color);
                    }
                }
            }
            return false;
        }

        public override void KillMultiTile(int x, int y, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 48, 32, ModContent.ItemType<Items.Furnitures.GlowWoodChandelier>());
        }
    }
}