using Everglow.Sources.Commons.Core.ModHooks;
using Terraria.Audio;
using Terraria.UI;
using Terraria.ID;

namespace Everglow.Sources.Modules.AssetReplaceModule.SoundReplace
{
    public class PotionPickSoundModify : GlobalItem, IModifyItemPickSound
    {
        public static readonly SoundStyle PotionPick = new($"Everglow/Resources/Sounds/Potion", SoundType.Sound) {
            Volume = 0.8f,
            PitchRange = (-0.3f, 0.3f),
            MaxInstances = 0
        };

        public static readonly ContentSamples.CreativeHelper.ItemGroup[] PotionGroups = new ContentSamples.CreativeHelper.ItemGroup[] {
            ContentSamples.CreativeHelper.ItemGroup.Flask,
            ContentSamples.CreativeHelper.ItemGroup.BuffPotion,
            ContentSamples.CreativeHelper.ItemGroup.LifePotions,
            ContentSamples.CreativeHelper.ItemGroup.ManaPotions
        };

        public void ModifyItemPickSound(Item item, int context, bool putIn, ref SoundStyle? customSound, ref bool playOriginalSound) {
            var group = ContentSamples.CreativeHelper.GetItemGroup(item, out _);
            if (context == ItemSlot.Context.InventoryItem && PotionGroups.Contains(group)) {
                playOriginalSound = false;
                SoundEngine.PlaySound(PotionPick);
            }
        }
    }
}
