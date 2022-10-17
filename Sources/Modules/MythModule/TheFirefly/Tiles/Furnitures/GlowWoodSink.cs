using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;
using Terraria.ObjectData;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;

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
		public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
		{
			var tile = Main.tile[i, j];
			Vector2 zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
			if (Main.drawToScreen)
			{
				zero = Vector2.Zero;
			}
			Texture2D tex = MythContent.QuickTexture("TheFirefly/Tiles/Furnitures/GlowWoodSinkGlow");
			spriteBatch.Draw(tex, new Vector2(i * 16, j * 16) - Main.screenPosition + zero, new Rectangle(tile.TileFrameX, tile.TileFrameY, 16, 16), new Color(0.8f, 0.8f, 0.8f, 0), 0, new Vector2(0), 1, SpriteEffects.None, 0);

			base.PostDraw(i, j, spriteBatch);
		}
	}
}