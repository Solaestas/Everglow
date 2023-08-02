using ReLogic.Content;
using Terraria.GameContent;

namespace Everglow.Commons.AssetReplace.UIReplace;

internal class EternalAssets
{
	public Asset<Texture2D>[] InventoryBacks = new Asset<Texture2D>[5];

	public void LoadTextures()
	{
		for (int i = 0; i <= 4; i++)
			InventoryBacks[i] = UIReplaceModule.GetTexture($"UISkinEternal/Inventory/ItemSlot_{i}");
	}

	public void Apply()
	{
		// 永恒意志还没有自己的HP, MP槽样式，所以这里用原版的把它覆盖了
		UIReplaceModule.TerrariaAssets.ClassicBar.ReplaceTextures();
		UIReplaceModule.TerrariaAssets.FancyBar.ReplaceTextures();
		UIReplaceModule.TerrariaAssets.HorizontalBar.ReplaceTextures();
		TextureAssets.InventoryBack = InventoryBacks[0];
		TextureAssets.InventoryBack2 = InventoryBacks[0];
		TextureAssets.InventoryBack3 = InventoryBacks[0];
		TextureAssets.InventoryBack4 = InventoryBacks[0];
		TextureAssets.InventoryBack5 = InventoryBacks[0];
		TextureAssets.InventoryBack6 = InventoryBacks[0];
		TextureAssets.InventoryBack7 = InventoryBacks[0];
		TextureAssets.InventoryBack8 = InventoryBacks[0];
		TextureAssets.InventoryBack9 = InventoryBacks[0];
		TextureAssets.InventoryBack10 = InventoryBacks[2];
		TextureAssets.InventoryBack11 = InventoryBacks[0];
		TextureAssets.InventoryBack12 = InventoryBacks[0];
		TextureAssets.InventoryBack13 = InventoryBacks[0];
		TextureAssets.InventoryBack14 = InventoryBacks[3];
		TextureAssets.InventoryBack15 = InventoryBacks[1];
		TextureAssets.InventoryBack16 = InventoryBacks[0];
		TextureAssets.InventoryBack17 = InventoryBacks[0];
		TextureAssets.InventoryBack18 = InventoryBacks[0];
	}
}
