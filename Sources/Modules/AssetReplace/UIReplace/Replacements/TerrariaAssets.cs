using Everglow.AssetReplace.UIReplace.Core;
using ReLogic.Content;
using Terraria.GameContent;

namespace Everglow.AssetReplace.UIReplace.Replacements;

public class TerrariaAssets
{
	public Asset<Texture2D>[] InventoryBacks = new Asset<Texture2D>[18];
	public ClassicBar ClassicBar = new();
	public FancyBar FancyBar = new();
	public HorizontalBar HorizontalBar = new();

	// 这里自己再写一遍了，毕竟除了这里也没其他地方会读取原版贴图
	public void LoadTextures()
	{
		InventoryBacks[0] = TextureAssets.InventoryBack;
		InventoryBacks[1] = TextureAssets.InventoryBack2;
		InventoryBacks[2] = TextureAssets.InventoryBack3;
		InventoryBacks[3] = TextureAssets.InventoryBack4;
		InventoryBacks[4] = TextureAssets.InventoryBack5;
		InventoryBacks[5] = TextureAssets.InventoryBack6;
		InventoryBacks[6] = TextureAssets.InventoryBack7;
		InventoryBacks[7] = TextureAssets.InventoryBack8;
		InventoryBacks[8] = TextureAssets.InventoryBack9;
		InventoryBacks[9] = TextureAssets.InventoryBack10;
		InventoryBacks[10] = TextureAssets.InventoryBack11;
		InventoryBacks[11] = TextureAssets.InventoryBack12;
		InventoryBacks[12] = TextureAssets.InventoryBack13;
		InventoryBacks[13] = TextureAssets.InventoryBack14;
		InventoryBacks[14] = TextureAssets.InventoryBack15;
		InventoryBacks[15] = TextureAssets.InventoryBack16;
		InventoryBacks[16] = TextureAssets.InventoryBack17;
		InventoryBacks[17] = TextureAssets.InventoryBack18;

		ClassicBar.BlueStar = UIReplaceModule.LoadVanillaAsset<Texture2D>($"Images/Mana");
		ClassicBar.RedHeart = UIReplaceModule.LoadVanillaAsset<Texture2D>($"Images/Heart");
		ClassicBar.GoldHeart = UIReplaceModule.LoadVanillaAsset<Texture2D>($"Images/Heart2");

		var mode = AssetRequestMode.ImmediateLoad;

		string str = "Images/UI/PlayerResourceSets/FancyClassic/";
		FancyBar.HeartLeft = Main.Assets.Request<Texture2D>(str + "Heart_Left", mode);
		FancyBar.HeartMiddle = Main.Assets.Request<Texture2D>(str + "Heart_Middle", mode);
		FancyBar.HeartRight = Main.Assets.Request<Texture2D>(str + "Heart_Right", mode);
		FancyBar.HeartRightFancy = Main.Assets.Request<Texture2D>(str + "Heart_Right_Fancy", mode);
		FancyBar.HeartFillRed = Main.Assets.Request<Texture2D>(str + "Heart_Fill", mode);
		FancyBar.HeartFillGold = Main.Assets.Request<Texture2D>(str + "Heart_Fill_B", mode);
		FancyBar.HeartSingleFancy = Main.Assets.Request<Texture2D>(str + "Heart_Single_Fancy", mode);
		FancyBar.StarA = Main.Assets.Request<Texture2D>(str + "Star_A", mode);
		FancyBar.StarB = Main.Assets.Request<Texture2D>(str + "Star_B", mode);
		FancyBar.StarC = Main.Assets.Request<Texture2D>(str + "Star_C", mode);
		FancyBar.StarSingle = Main.Assets.Request<Texture2D>(str + "Star_Single", mode);
		FancyBar.StarFill = Main.Assets.Request<Texture2D>(str + "Star_Fill", mode);

		str = "Images/UI/PlayerResourceSets/HorizontalBars/";
		HorizontalBar.HpFill = Main.Assets.Request<Texture2D>(str + "HP_Fill", mode);
		HorizontalBar.HpFillGold = Main.Assets.Request<Texture2D>(str + "HP_Fill_Honey", mode);
		HorizontalBar.MpFill = Main.Assets.Request<Texture2D>(str + "MP_Fill", mode);
		HorizontalBar.PanelLeft = Main.Assets.Request<Texture2D>(str + "Panel_Left", mode);
		HorizontalBar.HpPanelMiddle = Main.Assets.Request<Texture2D>(str + "HP_Panel_Middle", mode);
		HorizontalBar.HpPanelRight = Main.Assets.Request<Texture2D>(str + "HP_Panel_Right", mode);
		HorizontalBar.MpPanelMiddle = Main.Assets.Request<Texture2D>(str + "MP_Panel_Middle", mode);
		HorizontalBar.MpPanelRight = Main.Assets.Request<Texture2D>(str + "MP_Panel_Right", mode);
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
		TextureAssets.InventoryBack16 = InventoryBacks[15];
		TextureAssets.InventoryBack17 = InventoryBacks[16];
		TextureAssets.InventoryBack18 = InventoryBacks[17];

		ClassicBar.ReplaceTextures();
		FancyBar.ReplaceTextures();
		HorizontalBar.ReplaceTextures();
	}
}