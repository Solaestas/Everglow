using ReLogic.Content;

namespace Everglow.Sources.Modules.AssetReplaceModule
{
	public class TerrariaBarAssets
	{
		public ClassicBar ClassicBar = new();
		public FancyBar FancyBar = new();
		public HorizontalBar HorizontalBar = new();

		// 这里自己再写一遍了，毕竟除了这里也没其他地方会读取原版贴图
		public void LoadTextures() {
			ClassicBar.BlueStar = AssetReplaceModule.LoadVanillaAsset<Texture2D>($"Images/Mana");
			ClassicBar.RedHeart = AssetReplaceModule.LoadVanillaAsset<Texture2D>($"Images/Heart");
			ClassicBar.GoldHeart = AssetReplaceModule.LoadVanillaAsset<Texture2D>($"Images/Heart2");

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

		public void Apply() {
			ClassicBar.ReplaceTextures();
			FancyBar.ReplaceTextures();
			HorizontalBar.ReplaceTextures();
		}
	}
}
