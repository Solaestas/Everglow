﻿using Everglow.Sources.Commons.Core.ModHooks;
using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Function.FeatureFlags;
using Microsoft.Xna.Framework.Audio;
using Terraria.Audio;

namespace Everglow.Sources.Modules.AssetReplaceModule.SoundReplace
{
    internal class PickItemSoundReplaceModule : IModule
    {
        public string Name => "Pick Item Sound Modify";

        public void Load() {
            _playOriginalSound = true;
            On.Terraria.UI.ItemSlot.LeftClick_ItemArray_int_int += PatchLeftClick;
            On.Terraria.UI.ItemSlot.RightClick_ItemArray_int_int += PatchRightClick;
            On.Terraria.Audio.SoundEngine.PlaySound_int_int_int_int_float_float += PatchLegacyIDPlaySound;
        }

        private void PatchRightClick(On.Terraria.UI.ItemSlot.orig_RightClick_ItemArray_int_int orig, Item[] inv, int context, int slot) {
            _playOriginalSound = true;
            if (ModContent.GetInstance<EverglowClientConfig>().ItemPickSoundReplace && Main.mouseRight && Main.stackSplit <= 1) {
                SoundStyle? customSoundStyle = null;

                IModifyItemPickSound.Invoke(inv[slot], context, false, ref customSoundStyle, ref _playOriginalSound);

                if (customSoundStyle.HasValue) {
                    SoundEngine.PlaySound(customSoundStyle.Value);
                }

                orig.Invoke(inv, context, slot);
                return;
            }
            orig.Invoke(inv, context, slot);
        }

        private void PatchLeftClick(On.Terraria.UI.ItemSlot.orig_LeftClick_ItemArray_int_int orig, Item[] inv, int context, int slot) {
            _playOriginalSound = true;
            if (ModContent.GetInstance<EverglowClientConfig>().ItemPickSoundReplace && Main.mouseLeft && Main.mouseLeftRelease) {
                SoundStyle? customSoundStyle = null;

                if (!Main.mouseItem.IsAir)
                    IModifyItemPickSound.Invoke(Main.mouseItem, context, true, ref customSoundStyle, ref _playOriginalSound);
                else if (!inv[slot].IsAir)
                    IModifyItemPickSound.Invoke(inv[slot], context, false, ref customSoundStyle, ref _playOriginalSound);

                if (customSoundStyle.HasValue) {
                    SoundEngine.PlaySound(customSoundStyle.Value);
                }

                orig.Invoke(inv, context, slot);
                return;
            }
            orig.Invoke(inv, context, slot);
        }

        private SoundEffectInstance PatchLegacyIDPlaySound(On.Terraria.Audio.SoundEngine.orig_PlaySound_int_int_int_int_float_float orig, int type, int x, int y, int Style, float volumeScale, float pitchOffset) {
            if (_playOriginalSound) {
                return orig.Invoke(type, x, y, Style, volumeScale, pitchOffset);
            }
            return null;
        }

        private static bool _playOriginalSound = false;

        public void Unload() {
        }
    }
}
