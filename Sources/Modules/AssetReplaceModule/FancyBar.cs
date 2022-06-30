using ReLogic.Content;

namespace Everglow.Sources.Modules.AssetReplaceModule
{
    public class FancyBar
    {
        public Asset<Texture2D> HeartFillRed;
        public Asset<Texture2D> HeartFillGold;
        public Asset<Texture2D> HeartLeft;
        public Asset<Texture2D> HeartMiddle;
        public Asset<Texture2D> HeartRight;
        public Asset<Texture2D> HeartSingleFancy;
        public Asset<Texture2D> HeartRightFancy;
        public Asset<Texture2D> StarA;
        public Asset<Texture2D> StarB;
        public Asset<Texture2D> StarC;
        public Asset<Texture2D> StarFill;
        public Asset<Texture2D> StarSingle;

        /// <summary>
        /// 根据传入的路径读取Texture2D
        /// </summary>
        /// <param name="path">贴图组在Resources文件夹内的名字，比如UISkinMyth</param>
        public void LoadTextures(string path) {
            HeartFillRed = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Fill");
            HeartFillGold = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Fill_B");
            HeartLeft = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Left");
            HeartMiddle = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Middle");
            HeartRight = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Right");
            HeartSingleFancy = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Single_Fancy");
            HeartRightFancy = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Heart_Right_Fancy");
            StarA = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Star_A");
            StarB = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Star_B");
            StarC = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Star_C");
            StarFill = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Star_Fill");
            StarSingle = AssetReplaceModule.GetTexture($"{path}/Bars/Fancy/Star_Single");
        }

        public void ReplaceTextures() {
            if (AssetReplaceModule.PlayerResourceSets.TryGetValue("New", out var value)) {
                // 获取Fields
                var type = value.GetType();
                var Fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                var field = new Dictionary<string, FieldInfo>();
                foreach (var f in Fields) {
                    field[f.Name] = f;
                }
                // 设置贴图
                field["_heartLeft"].SetValue(value, HeartLeft);
                field["_heartMiddle"].SetValue(value, HeartMiddle);
                field["_heartRight"].SetValue(value, HeartRight);
                field["_heartRightFancy"].SetValue(value, HeartRightFancy);
                field["_heartFill"].SetValue(value, HeartFillRed);
                field["_heartFillHoney"].SetValue(value, HeartFillGold);
                field["_starTop"].SetValue(value, StarA);
                field["_starMiddle"].SetValue(value, StarB);
                field["_starBottom"].SetValue(value, StarC);
                field["_starSingle"].SetValue(value, StarSingle);
                field["_starFill"].SetValue(value, StarFill);
                return;
            }
            Everglow.Instance.Logger.Warn("FancyBar sprites replacement loading failed, sprite replacement would not work.");
        }
    }
}
