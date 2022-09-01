using Everglow.Sources.Modules.MythModule.Common;
using ReLogic.Content;

namespace Everglow.Sources.Modules.AssetReplaceModule
{
    public class MothFightingMusic
    {
        public Asset<ManualMusicRegistrationExample> MothFighting;
        public Asset<ManualMusicRegistrationExample> AltMothFighting;
        public Asset<ManualMusicRegistrationExample> OldMothFighting;

        /// <summary>
        /// 根据传入的路径读取Texture2D
        /// </summary>
        /// <param name="path">Sources, Modules, MythModule</param>
        public void LoadAudios(string path) {
            MothFighting = AssetReplaceModule.GetAsset($"{path}/Musics/MothFighting");
            AltMothFighting = AssetReplaceModule.GetAsset($"{path}/Musics/MothFightingAlt");
            OldMothFighting = AssetReplaceModule.GetAsset($"{path}/Musics/MothFightingOld2");
        }

        public void ReplaceAudios() {
            if (AssetReplaceModule.PlayerResourceSets.TryGetValue("New", out var value)) {
                // 获取Fields
                var type = value.GetType();
                var Fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                var field = new Dictionary<string, FieldInfo>();
                foreach (var f in Fields) {
                    field[f.Name] = f;
                }
                // 设置贴图
                field["MothFighting"].SetValue(value, MothFighting);
                field["MothFightingAlt"].SetValue(value, AltMothFighting);
                field["MothFightingOld2"].SetValue(value, OldMothFighting);
                return;
            }
            Everglow.Instance.Logger.Warn("Everglow music toggle loading failed, music toggle would not work.");
        }
    }
}
