using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace Everglow.Ocean.Tiles.Ocean
{
	// Token: 0x02000DCE RID: 3534
	public class 小脑珊瑚 : ModTile
	{
		// Token: 0x06004868 RID: 18536 RVA: 0x0034883C File Offset: 0x00346A3C
		public override void SetStaticDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileNoAttach[(int)base.Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x2);
            TileObjectData.newTile.CoordinateHeights = new int[]
            {
                16,
                18
            };
            TileObjectData.newTile.CoordinateWidth = 36;
            TileObjectData.addTile((int)base.Type);
			this.DustType = 253;
            LocalizedText modTranslation = base.CreateMapEntryName();
            // modTranslation.SetDefault("");
            base.AddMapEntry(new Color(42, 91, 10), modTranslation);
			this.MineResist = 3f;
			base.SetStaticDefaults();
			// modTranslation.AddTranslation(GameCulture.Chinese, "");
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x000138D5 File Offset: 0x00011AD5
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
        // Token: 0x0600486A RID: 18538 RVA: 0x003488D0 File Offset: 0x00346AD0
        public override void NearbyEffects(int i, int j, bool closer)
		{
		}
	}
}
