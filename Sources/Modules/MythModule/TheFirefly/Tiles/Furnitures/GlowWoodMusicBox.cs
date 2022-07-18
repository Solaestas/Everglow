using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles.Furnitures
{
	// See ExampleMod/Common/Systems/MusicLoadingSystem for an explanation on music.
	public class GlowWoodMusicBox : ModTile
	{
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[Type] = true;
			Main.tileObsidianKill[Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.Origin = new Point16(0, 1);
			TileObjectData.newTile.LavaDeath = false;
			TileObjectData.newTile.DrawYOffset = 2;
			TileObjectData.addTile(Type);

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("Music Box");
			AddMapEntry(new Color(200, 200, 200), name);
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY)
		{
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 48, ModContent.ItemType<Items.Furnitures.GlowWoodMusicBox>());
		}

		public override void MouseOver(int i, int j)
		{
			Player player = Main.LocalPlayer;
			player.noThrow = 2;
			player.cursorItemIconEnabled = true;
			player.cursorItemIconID = ModContent.ItemType<Items.Furnitures.GlowWoodMusicBox>();
		}
        public override void HitWire(int i, int j)
        {
            var tile = Main.tile[i, j];
            int width = 36;
            int DeltaX = tile.TileFrameX % width / 18;
            int DeltaY = tile.TileFrameY / 18;
            int AddX = 0;



            while (Main.tile[i - DeltaX + AddX, j - DeltaY].TileFrameX % width == AddX * 18 && Main.tile[i - DeltaX + AddX, j - DeltaY].HasTile && Main.tile[i - DeltaX + AddX, j - DeltaY].TileType == Type)
            {
                int AddY = 0;
                while (Main.tile[i - DeltaX + AddX, j - DeltaY + AddY].TileFrameY == AddY * 18 && Main.tile[i - DeltaX + AddX, j - DeltaY + AddY].HasTile && Main.tile[i - DeltaX + AddX, j - DeltaY + AddY].TileType == Type)
                {
                    ;
                    if (Main.tile[i - DeltaX + AddX, j - DeltaY + AddY].TileFrameX < width)
                    {
                        Main.tile[i - DeltaX + AddX, j - DeltaY + AddY].TileFrameX += (short)width;
                    }
                    else
                    {
                        Main.tile[i - DeltaX + AddX, j - DeltaY + AddY].TileFrameX -= (short)width;
                    }
                    if (Wiring.running)
                    {
                        Wiring.SkipWire(i - DeltaX + AddX, j - DeltaY + AddY);
                    }
                    AddY++;
                }
                AddX++;
            }
        }
    }
}
