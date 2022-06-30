using ReLogic.Content;

namespace Everglow.Sources.Modules.AssetReplaceModule
{
	public class HorizontalBar
	{
		public Asset<Texture2D> HpFill;
		public Asset<Texture2D> HpFillGold;
		public Asset<Texture2D> HpPanelMiddle;
		public Asset<Texture2D> HpPanelRight;
		public Asset<Texture2D> HpPanelRightGold;
		public Asset<Texture2D> MpFill;
		public Asset<Texture2D> MpPanelMiddle;
		public Asset<Texture2D> MpPanelRight;
		public Asset<Texture2D> PanelLeft;

		/// <summary>
		/// 根据传入的路径读取Texture2D
		/// </summary>
		/// <param name="path">贴图组在Resources文件夹内的名字，比如UISkinMyth</param>
		public void LoadTextures(string path) {
			HpFill = AssetReplaceModule.GetTexture($"{path}/Bars/Horizontal/HP_Fill");
			HpFillGold = AssetReplaceModule.GetTexture($"{path}/Bars/Horizontal/HP_FillGold");
			HpPanelMiddle = AssetReplaceModule.GetTexture($"{path}/Bars/Horizontal/HP_Panel_Middle");
			HpPanelRight = AssetReplaceModule.GetTexture($"{path}/Bars/Horizontal/HP_Panel_Right");
			HpPanelRightGold = AssetReplaceModule.GetTexture($"{path}/Bars/Horizontal/HP_Panel_RightGold");
			MpFill = AssetReplaceModule.GetTexture($"{path}/Bars/Horizontal/MP_Fill");
			MpPanelMiddle = AssetReplaceModule.GetTexture($"{path}/Bars/Horizontal/MP_Panel_Middle");
			MpPanelRight = AssetReplaceModule.GetTexture($"{path}/Bars/Horizontal/MP_Panel_Right");
			PanelLeft = AssetReplaceModule.GetTexture($"{path}/Bars/Horizontal/Panel_Left");
		}

		public void ReplaceTextures() {
			if (AssetReplaceModule.PlayerResourceSets.TryGetValue("HorizontalBars", out var value)) {
				// 获取Fields
				var type = value.GetType();
				var Fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
				var field = new Dictionary<string, FieldInfo>();
				foreach (var f in Fields) {
					field[f.Name] = f;
				}
				// 设置贴图
				field["_hpFill"].SetValue(value, HpFill);
				field["_hpFillHoney"].SetValue(value, HpFillGold);
				field["_mpFill"].SetValue(value, MpFill);
				field["_panelLeft"].SetValue(value, PanelLeft);
				field["_panelMiddleHP"].SetValue(value, HpPanelMiddle);
				field["_panelRightHP"].SetValue(value, HpPanelRight);
				field["_panelMiddleMP"].SetValue(value, MpPanelMiddle);
				field["_panelRightMP"].SetValue(value, MpPanelRight);
				return;
			}
			Everglow.Instance.Logger.Warn("HorizontalBar sprites replacement loading failed, sprite replacement would not work.");
		}
	}
}
