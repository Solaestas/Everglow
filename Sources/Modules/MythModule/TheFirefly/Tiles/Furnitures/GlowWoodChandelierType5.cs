using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;
using Everglow.Sources.Commons.Core.Utils;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles.Furnitures
{
    public class GlowWoodChandelierType5 : ModTile
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
            if(closer)
            {
                var tile = Main.tile[i, j];
                foreach(Player player in Main.player)
                {
                    if(player.Hitbox.Intersects(new Rectangle(i * 16, j * 16 - 8, 18, 18)))
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



                    if (player.Hitbox.Intersects(new Rectangle(i * 16 + 11, j * 16 - 8, 18, 18)))
                    {
                        if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18)))
                        {
                            TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                        }
                        else
                        {
                            float rot;
                            float Omega;
                            Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18)].X;
                            rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18)].Y;
                            if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                            {
                                TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                            }
                            if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                            {
                                TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18));
                            }
                        }
                    }

                    if (player.Hitbox.Intersects(new Rectangle(i * 16 - 3, j * 16 - 8, 18, 18)))
                    {
                        if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1)))
                        {
                            TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                        }
                        else
                        {
                            float rot;
                            float Omega;
                            Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1)].X;
                            rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1)].Y;
                            if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                            {
                                TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                            }
                            if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                            {
                                TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1));
                            }
                        }
                    }


                    if (player.Hitbox.Intersects(new Rectangle(i * 16 - 10, j * 16 - 8, 18, 18)))
                    {
                        if (!TileSpin.TileRotation.ContainsKey((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18)))
                        {
                            TileSpin.TileRotation.Add((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18), new Vector2(-Math.Clamp(player.velocity.X, -1, 1) * 0.2f));
                        }
                        else
                        {
                            float rot;
                            float Omega;
                            Omega = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18)].X;
                            rot = TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18)].Y;
                            if (Math.Abs(Omega) < 0.04f && Math.Abs(rot) < 0.04f)
                            {
                                TileSpin.TileRotation[(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18)] = new Vector2(Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f, rot + Omega - Math.Clamp(player.velocity.X, -1, 1) * 0.2f);
                            }
                            if (Math.Abs(Omega) < 0.001f && Math.Abs(rot) < 0.001f)
                            {
                                TileSpin.TileRotation.Remove((i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18));
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
                Adx = 54;
            }
            if (tile.TileFrameX % 54 == 18 && tile.TileFrameY == 0)
            {
                TileSpin tileSpin = new TileSpin();
                tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18);
                Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodChandelierType5");
                tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18, tex,new Rectangle(30 + Adx, 0, 10, 48), new Vector2(5, 0), 16, -2, 1);



                tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18);
                tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18 + 1, j - tile.TileFrameY / 18, tex, new Rectangle(40 + Adx, 0, 10, 48), new Vector2(5, 0), 10, -2, 1);



                tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1);
                tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18, j - tile.TileFrameY / 18 + 1, tex, new Rectangle(18 + Adx, 0, 10, 48), new Vector2(5, 0), 4, -18, 1);



                tileSpin.Update(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18);
                tileSpin.DrawRotatedTile(i - (tile.TileFrameX % 54 - 18) / 18 - 1, j - tile.TileFrameY / 18, tex, new Rectangle(6 + Adx, 0, 10, 48), new Vector2(5, 0), 8, -2, 1);

            }
            return false;
        }
        public override void KillMultiTile(int x, int y, int frameX, int frameY)
        {
            Item.NewItem(new EntitySource_TileBreak(x, y), x * 16, y * 16, 48, 32, ModContent.ItemType<Items.Furnitures.GlowWoodChandelierType5>());
        }
    }
}
