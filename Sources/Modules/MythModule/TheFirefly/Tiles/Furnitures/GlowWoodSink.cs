using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;
using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Tiles.Furnitures
{
	public class GlowWoodSink : ModTile
	{
		public override void SetStaticDefaults() {

			TileID.Sets.CountsAsWaterSource[Type] = true;



			Main.tileSolid[Type] = false;
			Main.tileLavaDeath[Type] = false;
			Main.tileFrameImportant[Type] = true;

			TileObjectData.newTile.CopyFrom(TileObjectData.Style2x2);
			TileObjectData.newTile.CoordinateHeights = new[] { 16, 18 };
			TileObjectData.addTile(Type);

			DustType = ModContent.DustType<BlueGlow>();
			AdjTiles = new int[] { Type };

			ModTranslation name = CreateMapEntryName();
			name.SetDefault("GlowWood Sink");
			AddMapEntry(new Color(0, 14, 175), name);
		}

		public override void NumDust(int i, int j, bool fail, ref int num) {
			num = fail ? 1 : 3;
		}

		public override void KillMultiTile(int i, int j, int frameX, int frameY) {
			Item.NewItem(new EntitySource_TileBreak(i, j), i * 16, j * 16, 16, 16, ModContent.ItemType<Items.Furnitures.GlowWoodSink>());
		}
	}
}