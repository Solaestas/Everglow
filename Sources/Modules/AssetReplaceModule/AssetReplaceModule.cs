using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Function.FeatureFlags;
using Everglow.Sources.Modules.MythModule.Common;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.GameContent.UI.ResourceSets;

namespace Everglow.Sources.Modules.AssetReplaceModule
{
    public class AssetReplaceModule : IModule
    {
        public string Name => "Assets Replacement";

        public static Dictionary<string, IPlayerResourcesDisplaySet> PlayerResourceSets => 
            (Dictionary<string, IPlayerResourcesDisplaySet>)typeof(PlayerResourceSetsManager).GetField("_sets", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(Main.ResourceSetsManager);

        internal static TerrariaAssets TerrariaAssets = new();
        internal static EternalAssets EternalAssets = new();
        internal static MythAssets MythAssets = new();

        public static bool IsLoaded = false;

        public static Asset<Texture2D> GetTexture(string path) =>
            ModContent.Request<Texture2D>("Everglow/Resources/" + path, AssetRequestMode.ImmediateLoad);
        public static Asset<ManualMusicRegistrationExample> GetAsset(string path) =>
            ModContent.Request<ManualMusicRegistrationExample>("Everglow/Sources/Modules/MythModule/" + path, AssetRequestMode.ImmediateLoad);

        public static Asset<T> LoadVanillaAsset<T>(string assetName) where T : class => Main.Assets.Request<T>(assetName, AssetRequestMode.ImmediateLoad);

        public void Load() {
            if (Main.netMode != NetmodeID.Server) {
                TerrariaAssets.LoadTextures();
                EternalAssets.LoadTextures();
                MythAssets.LoadTextures();
                ReplaceTextures(ModContent.GetInstance<EverglowClientConfig>().TextureReplace);
                IsLoaded = true;
            }
        }

        public static void ReplaceTextures(TextureReplaceMode mode) {
            switch (mode) {
                case TextureReplaceMode.Terraria:
                    TerrariaAssets.Apply();
                    break;
                case TextureReplaceMode.EternalResolve:
                    EternalAssets.Apply();
                    break;
                case TextureReplaceMode.Myth:
                    MythAssets.Apply();
                    break;
                // 有人直接改config配置文件？
                default:
                    TerrariaAssets.Apply();
                    break;
            }
        }

        public void Unload() {
            if (Main.netMode != NetmodeID.Server) {
                ReplaceTextures(TextureReplaceMode.Terraria);
                TerrariaAssets.Apply();

                TerrariaAssets = null;
                EternalAssets = null;
                MythAssets = null;

                IsLoaded = false;
            }
        }
    }
}
