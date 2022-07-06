using Everglow.Sources.Commons.Core.ModHooks;
using Terraria.Audio;
using Terraria.UI;

namespace Everglow.Sources.Modules.AssetReplaceModule.SoundReplace
{
    public class WoodPickSoundModify : GlobalItem, IModifyItemPickSound
    {
        public static readonly SoundStyle WoodPick = new($"Everglow/Resources/Sounds/Wood", SoundType.Sound) {
            Volume = 0.8f,
            PitchVariance = 0.4f,
            MaxInstances = 0
        };

        public static readonly int[] Woods = new int[] {
            ItemID.Wood,
            ItemID.RichMahogany,
            ItemID.Ebonwood,
            ItemID.Shadewood,
            ItemID.Pearlwood,
            ItemID.BorealWood,
            ItemID.PalmWood,
            ItemID.DynastyWood,
            ItemID.SpookyWood
        };

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) =>
            lateInstantiation && Woods.Contains(entity.type);

        public void ModifyItemPickSound(Item item, int context, bool putIn, ref SoundStyle? customSound, ref bool playOriginalSound) {
            if (context == ItemSlot.Context.InventoryItem) {
                playOriginalSound = false;
                SoundEngine.PlaySound(WoodPick);
            }
        }
    }
}
