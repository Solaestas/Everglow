using ReLogic.Content;
using Terraria.GameContent;

namespace Everglow.Commons.AssetReplace.UIReplace;

internal class EverglowAssets
{
	public Asset<Texture2D>[] InventoryBacks = new Asset<Texture2D>[20];

	public void LoadTextures()
	{
		for (int i = 0; i <= 18; i++)
		{
			if (i is 1)
				continue;
			InventoryBacks[i] = UIReplaceModule.GetTexture($"UISkinEverglow/Inventory/Inventory_Back{i}");
		}
	}

	public void Apply()
	{
		// 永恒意志还没有自己的HP, MP槽样式，所以这里用原版的把它覆盖了
		UIReplaceModule.TerrariaAssets.ClassicBar.ReplaceTextures();
		UIReplaceModule.TerrariaAssets.FancyBar.ReplaceTextures();
		UIReplaceModule.TerrariaAssets.HorizontalBar.ReplaceTextures();
		TextureAssets.InventoryBack = InventoryBacks[0];
		TextureAssets.InventoryBack2 = InventoryBacks[2];
		TextureAssets.InventoryBack3 = InventoryBacks[3];
		TextureAssets.InventoryBack4 = InventoryBacks[4];
		TextureAssets.InventoryBack5 = InventoryBacks[5];
		TextureAssets.InventoryBack6 = InventoryBacks[6];
		TextureAssets.InventoryBack7 = InventoryBacks[7];
		TextureAssets.InventoryBack8 = InventoryBacks[8];
		TextureAssets.InventoryBack9 = InventoryBacks[9];
		TextureAssets.InventoryBack10 = InventoryBacks[10];
		TextureAssets.InventoryBack11 = InventoryBacks[11];
		TextureAssets.InventoryBack12 = InventoryBacks[12];
		TextureAssets.InventoryBack13 = InventoryBacks[13];
		TextureAssets.InventoryBack14 = InventoryBacks[14];
		TextureAssets.InventoryBack15 = InventoryBacks[15];
		TextureAssets.InventoryBack16 = InventoryBacks[16];
		TextureAssets.InventoryBack17 = InventoryBacks[17];
		TextureAssets.InventoryBack18 = InventoryBacks[18];
		TextureAssets.InventoryBack19 = InventoryBacks[19];
	}
}
