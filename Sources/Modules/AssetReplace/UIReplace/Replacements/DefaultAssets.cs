using Everglow.AssetReplace.UIReplace.Core;
using ReLogic.Content;
using Terraria.GameContent;

namespace Everglow.AssetReplace.UIReplace.Replacements;

internal class DefaultAssets
{
	public Asset<Texture2D>[] InventoryBacks = new Asset<Texture2D>[17];
	public ClassicBar ClassicBar = new();
	public FancyBar FancyBar = new();
	public HorizontalBar HorizontalBar = new();

	public void LoadTextures()
	{
		for (int i = 1; i <= 17; i++)
		{
			if (i is 16)
			{
				continue;
			}

			InventoryBacks[i - 1] = UIReplaceModule.GetTexture($"UISkinDefault/Inventory/Inventory_Back{i}");
		}

		// ClassicBar.LoadTextures("UISkinMyth");
		// FancyBar.LoadTextures("UISkinMyth");
		// HorizontalBar.LoadTextures("UISkinMyth");
	}

	public void Apply()
	{
		TextureAssets.InventoryBack = InventoryBacks[0];
		TextureAssets.InventoryBack2 = InventoryBacks[1];
		TextureAssets.InventoryBack3 = InventoryBacks[2];
		TextureAssets.InventoryBack4 = InventoryBacks[3];
		TextureAssets.InventoryBack5 = InventoryBacks[4];
		TextureAssets.InventoryBack6 = InventoryBacks[5];
		TextureAssets.InventoryBack7 = InventoryBacks[6];
		TextureAssets.InventoryBack8 = InventoryBacks[7];
		TextureAssets.InventoryBack9 = InventoryBacks[8];
		TextureAssets.InventoryBack10 = InventoryBacks[9];
		TextureAssets.InventoryBack11 = InventoryBacks[10];
		TextureAssets.InventoryBack12 = InventoryBacks[11];
		TextureAssets.InventoryBack13 = InventoryBacks[12];
		TextureAssets.InventoryBack14 = InventoryBacks[13];
		TextureAssets.InventoryBack15 = InventoryBacks[14];
		TextureAssets.InventoryBack16 = InventoryBacks[0];
		TextureAssets.InventoryBack17 = InventoryBacks[16];
		TextureAssets.InventoryBack18 = InventoryBacks[0];

		UIReplaceModule.TerrariaAssets.ClassicBar.ReplaceTextures();
		UIReplaceModule.TerrariaAssets.FancyBar.ReplaceTextures();
		UIReplaceModule.TerrariaAssets.HorizontalBar.ReplaceTextures();

		// ClassicBar.ReplaceTextures();
		// FancyBar.ReplaceTextures();
		// HorizontalBar.ReplaceTextures();
	}
}