using Everglow.Sources.Commons.Core.ModHooks;
using Terraria.Audio;
using Terraria.UI;
using Terraria.ID;

namespace Everglow.Sources.Modules.AssetReplaceModule.SoundReplace
{
    public class WaterBucketPickSoundModify : GlobalItem, IModifyItemPickSound
    {
        public static readonly SoundStyle WaterBucketPick = new($"Everglow/Resources/Sounds/WaterBucket", SoundType.Sound) {
            Volume = 0.8f,
            PitchRange = (-0.2f, 0.1f),
            MaxInstances = 2
        };

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => lateInstantiation &&
        (entity.type == ItemID.WaterBucket || entity.type == ItemID.BottomlessBucket);

        public void ModifyItemPickSound(Item item, int context, bool putIn, ref SoundStyle? customSound, ref bool playOriginalSound) {
            if (context == ItemSlot.Context.InventoryItem) {
                playOriginalSound = false;
                SoundEngine.PlaySound(WaterBucketPick);
            }
        }
    }
}
