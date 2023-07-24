using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace MythMod.Tiles.Ocean
{
	// Token: 0x02000DD8 RID: 3544
	public class 柳珊瑚 : ModTile
	{
		// Token: 0x06004899 RID: 18585 RVA: 0x003495E8 File Offset: 0x003477E8
		public override void SetDefaults()
		{
			Main.tileFrameImportant[(int)base.Type] = true;
			Main.tileNoAttach[(int)base.Type] = true;
			Main.tileSolidTop[(int)base.Type] = false;
			TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
			TileObjectData.newTile.AnchorBottom = new AnchorData(0, 0, 0);
			TileObjectData.newAlternate.CopyFrom(TileObjectData.newTile);
			TileObjectData.newAlternate.AnchorLeft = new AnchorData((Terraria.Enums.AnchorType)1, (int)2, (int)0);
			TileObjectData.addAlternate(1);
			TileObjectData.newTile.AnchorRight = new AnchorData((Terraria.Enums.AnchorType)1, 2, 0);
			TileObjectData.addTile((int)base.Type);
			this.dustType = 60;
            ModTranslation modTranslation = base.CreateMapEntryName(null);
            modTranslation.SetDefault("");
            modTranslation.AddTranslation(GameCulture.Chinese, "");
            base.AddMapEntry(new Color(255, 97, 35), modTranslation);
			this.mineResist = 3f;
			base.SetDefaults();
		}

		// Token: 0x0600489A RID: 18586 RVA: 0x000138D5 File Offset: 0x00011AD5
		public override void NumDust(int i, int j, bool fail, ref int num)
		{
			num = (fail ? 1 : 3);
		}
        public override void KillMultiTile(int i, int j, int frameX, int frameY)
        {
            Item.NewItem(i * 16, j * 16, 16, 32, base.mod.ItemType("Gorgonian"));
        }
    }
}
