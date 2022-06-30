using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Function.FeatureFlags;
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

        public static Asset<Texture2D>[] TerrariaInventoryBacks = new Asset<Texture2D>[18];
        public static TerrariaBarAssets TerrariaBarAssets = new();

        public static Asset<Texture2D>[] EternalInventoryBacks = new Asset<Texture2D>[5];

        public static Asset<Texture2D>[] MythInventoryBacks = new Asset<Texture2D>[18];
        public static MythBarAssets MythBarAssets = new();

        public static bool IsLoaded = false;

        public void Load() => IsLoaded = false;

        public static Asset<Texture2D> GetTexture(string path) =>
            ModContent.Request<Texture2D>("Everglow/Resources/" + path, AssetRequestMode.ImmediateLoad);

        public static Asset<T> LoadVanillaAsset<T>(string assetName) where T : class => Main.Assets.Request<T>(assetName, AssetRequestMode.ImmediateLoad);

        public void SetupContent() {
            if (Main.netMode != NetmodeID.Server) {
                SetupTerrariaContents();
                SetupEternalContents();
                SetupMythContents();
                ReplaceTextures(ModContent.GetInstance<EverglowClientConfig>().TextureReplace);
                IsLoaded = true;
            }
        }

        public static void SetupTerrariaContents() {
            TerrariaInventoryBacks[0] = TextureAssets.InventoryBack;
            TerrariaInventoryBacks[1] = TextureAssets.InventoryBack2;
            TerrariaInventoryBacks[2] = TextureAssets.InventoryBack3;
            TerrariaInventoryBacks[3] = TextureAssets.InventoryBack4;
            TerrariaInventoryBacks[4] = TextureAssets.InventoryBack5;
            TerrariaInventoryBacks[5] = TextureAssets.InventoryBack6;
            TerrariaInventoryBacks[6] = TextureAssets.InventoryBack7;
            TerrariaInventoryBacks[7] = TextureAssets.InventoryBack8;
            TerrariaInventoryBacks[8] = TextureAssets.InventoryBack9;
            TerrariaInventoryBacks[9] = TextureAssets.InventoryBack10;
            TerrariaInventoryBacks[10] = TextureAssets.InventoryBack11;
            TerrariaInventoryBacks[11] = TextureAssets.InventoryBack12;
            TerrariaInventoryBacks[12] = TextureAssets.InventoryBack13;
            TerrariaInventoryBacks[13] = TextureAssets.InventoryBack14;
            TerrariaInventoryBacks[14] = TextureAssets.InventoryBack15;
            TerrariaInventoryBacks[15] = TextureAssets.InventoryBack16;
            TerrariaInventoryBacks[16] = TextureAssets.InventoryBack17;
            TerrariaInventoryBacks[17] = TextureAssets.InventoryBack18;
            TerrariaBarAssets.LoadTextures();
        }

        public static void SetupEternalContents() {
            for (int i = 0; i <= 4; i++)
                EternalInventoryBacks[i] = GetTexture($"UISkinEternal/Inventory/ItemSlot_{i}");
        }

        public static void SetupMythContents() {
            for (int i = 2; i <= 18; i++)
                MythInventoryBacks[i - 1] = GetTexture($"UISkinMyth/Inventory/Inventory_Back{i}");
            MythInventoryBacks[0] = GetTexture($"UISkinMyth/Inventory/Inventory_BackX");
            MythBarAssets.LoadTextures();
        }

        public static void ReplaceTextures(TextureReplaceMode mode) {
            switch (mode) {
                case TextureReplaceMode.Terraria: {
                        TextureAssets.InventoryBack = TerrariaInventoryBacks[0];
                        TextureAssets.InventoryBack2 = TerrariaInventoryBacks[1];
                        TextureAssets.InventoryBack3 = TerrariaInventoryBacks[2];
                        TextureAssets.InventoryBack4 = TerrariaInventoryBacks[3];
                        TextureAssets.InventoryBack5 = TerrariaInventoryBacks[4];
                        TextureAssets.InventoryBack6 = TerrariaInventoryBacks[5];
                        TextureAssets.InventoryBack7 = TerrariaInventoryBacks[6];
                        TextureAssets.InventoryBack8 = TerrariaInventoryBacks[7];
                        TextureAssets.InventoryBack9 = TerrariaInventoryBacks[8];
                        TextureAssets.InventoryBack10 = TerrariaInventoryBacks[9];
                        TextureAssets.InventoryBack11 = TerrariaInventoryBacks[10];
                        TextureAssets.InventoryBack12 = TerrariaInventoryBacks[11];
                        TextureAssets.InventoryBack13 = TerrariaInventoryBacks[12];
                        TextureAssets.InventoryBack14 = TerrariaInventoryBacks[13];
                        TextureAssets.InventoryBack15 = TerrariaInventoryBacks[14];
                        TextureAssets.InventoryBack16 = TerrariaInventoryBacks[15];
                        TextureAssets.InventoryBack17 = TerrariaInventoryBacks[16];
                        TextureAssets.InventoryBack18 = TerrariaInventoryBacks[17];
                        TerrariaBarAssets.Apply();
                        break;
                    }
                case TextureReplaceMode.EternalReslove: {
                        TextureAssets.InventoryBack = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack2 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack3 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack4 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack5 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack6 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack7 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack8 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack9 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack10 = EternalInventoryBacks[2];
                        TextureAssets.InventoryBack11 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack12 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack13 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack14 = EternalInventoryBacks[3];
                        TextureAssets.InventoryBack15 = EternalInventoryBacks[1];
                        TextureAssets.InventoryBack16 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack17 = EternalInventoryBacks[0];
                        TextureAssets.InventoryBack18 = EternalInventoryBacks[0];
                        break;
                    }
                case TextureReplaceMode.MythMod: {
                        TextureAssets.InventoryBack = MythInventoryBacks[0];
                        TextureAssets.InventoryBack2 = MythInventoryBacks[1];
                        TextureAssets.InventoryBack3 = MythInventoryBacks[2];
                        TextureAssets.InventoryBack4 = MythInventoryBacks[3];
                        TextureAssets.InventoryBack5 = MythInventoryBacks[4];
                        TextureAssets.InventoryBack6 = MythInventoryBacks[5];
                        TextureAssets.InventoryBack7 = MythInventoryBacks[6];
                        TextureAssets.InventoryBack8 = MythInventoryBacks[7];
                        TextureAssets.InventoryBack9 = MythInventoryBacks[8];
                        TextureAssets.InventoryBack10 = MythInventoryBacks[9];
                        TextureAssets.InventoryBack11 = MythInventoryBacks[10];
                        TextureAssets.InventoryBack12 = MythInventoryBacks[11];
                        TextureAssets.InventoryBack13 = MythInventoryBacks[12];
                        TextureAssets.InventoryBack14 = MythInventoryBacks[13];
                        TextureAssets.InventoryBack15 = MythInventoryBacks[14];
                        TextureAssets.InventoryBack16 = MythInventoryBacks[15];
                        TextureAssets.InventoryBack17 = MythInventoryBacks[16];
                        TextureAssets.InventoryBack18 = MythInventoryBacks[17];
                        MythBarAssets.Apply();
                        break;
                    }
            }
		}

        public void Unload() {
            if (Main.netMode != NetmodeID.Server) {
                ReplaceTextures(TextureReplaceMode.Terraria);
                TerrariaBarAssets.Apply();

                TerrariaInventoryBacks = null;
                TerrariaBarAssets = null;
                EternalInventoryBacks = null;
                MythInventoryBacks = null;
                MythBarAssets = null;

                IsLoaded = false;
            }
        }
    }
}
