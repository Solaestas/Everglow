using IL.Terraria.DataStructures;
using IL.Terraria.GameContent.Metadata;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles
{
    public class BlackVine : ModTile//TODO:Need to copy some code from vanilla.This is a kind of vine,it will swaying in the wind or dragged by moving-players.
    {
        public override void PostSetDefaults()
        {
            Main.tileCut[Type] = true;
            Main.tileBlockLight[Type] = true;
            Main.tileLavaDeath[Type] = true;
            Main.tileNoFail[Type] = true;
            TileID.Sets.IsVine[Type] = true;
            TileID.Sets.ReplaceTileBreakDown[Type] = true;
            TileID.Sets.VineThreads[Type] = true;
            TileID.Sets.DrawFlipMode[Type] = 1;
            Main.tileFrameImportant[Type] = true;
            DustType = 191;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            AddMapEntry(new Color(11, 11, 11), modTranslation);
            HitSound = SoundID.Grass;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            if (WorldGen.genRand.NextBool(2) && Main.player[Player.FindClosest(new Vector2(i * 16, j * 16), 16, 16)].cordage)
            {
                Item.NewItem(null, new Vector2((float)(i * 16) + 8f, (float)(j * 16) + 8f), 2996, 1, false, 0, false, false);
            }
        }

        public override void PlaceInWorld(int i, int j, Item item)
        {
        }

        public override void RandomUpdate(int i, int j)
        {
            Tile below = Main.tile[i, j + 1];
            if (below.HasTile || below.LiquidType != 0 )
            {
                return;
            }
            bool growVine = false;
            int vineOriginYPos = j;
            Tile val;
            while (vineOriginYPos > j - 10)
            {
                Tile consideredVineOrigin = Main.tile[i, vineOriginYPos];
                if (consideredVineOrigin.BottomSlope)
                {
                    growVine = false;
                    break;
                }
                val = Main.tile[i, vineOriginYPos];
                if (val.HasTile)
                {
                    val = Main.tile[i, vineOriginYPos];
                    if (!val.BottomSlope)
                    {
                        growVine = true;
                        break;
                    }
                }
                j--;
            }
            if (growVine)
            {
                int y = j + 1;
                val = Main.tile[i, y];
                val.TileType = (ushort)ModContent.TileType<BlackVine>();
                val = Main.tile[i, y];
                val.TileFrameX = (short)(WorldGen.genRand.Next(8) * 18);
                val = Main.tile[i, y];
                val.TileFrameY = 72;
                val = Main.tile[i, y];
                val.Get<TileWallWireStateData>().HasTile=true;
                val = Main.tile[i, j];
                val.TileFrameX = (short)(WorldGen.genRand.Next(12) * 18);
                val = Main.tile[i, j];
                val.TileFrameY = (short)(WorldGen.genRand.Next(4) * 18);
                WorldGen.SquareTileFrame(i, y, true);
                WorldGen.SquareTileFrame(i, j, true);
                if (Main.netMode == 2)
                {
                    NetMessage.SendTileSquare(-1, i, y, 3, 0);
                }
            }
        }

        public override void NearbyEffects(int i, int j, bool closer)
        {
        }

        public override bool PreDraw(int i, int j, SpriteBatch spriteBatch)
        {
            return true;
        }
    }
}