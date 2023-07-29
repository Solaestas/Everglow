using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;


namespace Everglow.Ocean.Tiles.Ocean
{
	// Token: 0x02000EBF RID: 3775
	public class 泥岩 : ModTile
	{
		// Token: 0x06004756 RID: 18262 RVA: 0x0028BA28 File Offset: 0x00289C28
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileNoAttach[(int)base.Type] = true;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.CoordinateHeights = new int[]
			{
				16,
				16
			};
			TileObjectData.addTile((int)base.Type);
			LocalizedText modTranslation = base.CreateMapEntryName(null);
			// modTranslation.SetDefault("");
			modTranslation.AddTranslation(GameCulture.English, "");
			AddMapEntry(new Color(148, 107, 74), modTranslation);
			this.DustType = 37;
			this.HitSound = 0;
			this.soundStyle/* tModPorter Note: Removed. Integrate into HitSound */ = 0;
			this.disableSmartCursor/* tModPorter Note: Removed. Use TileID.Sets.DisableSmartCursor instead */ = true;
		}


		// Token: 0x06004758 RID: 18264 RVA: 0x00013686 File Offset: 0x00011886
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 10 : 3);
		}

		// Token: 0x0400064E RID: 1614
		public int timer;
	}
}
