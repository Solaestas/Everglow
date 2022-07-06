using Everglow.Sources.Commons.Core.ModHooks;
using Terraria.Audio;
using Terraria.UI;
using Terraria.ID;

namespace Everglow.Sources.Modules.AssetReplaceModule.SoundReplace
{
    public class GlassPickSoundModify : GlobalItem, IModifyItemPickSound
    {
        public static readonly SoundStyle GlassPick = new($"Everglow/Resources/Sounds/Glass", SoundType.Sound) {
            Volume = 1f,
            PitchRange = (-0.1f, 0.3f),
            MaxInstances = 0
        };

        public static readonly int[] FragileGlass = new int[] {
            ItemID.Glass, ItemID.GlassPlatform, ItemID.GlassBowl, ItemID.Bottle, ItemID.Mug, ItemID.WineGlass, ItemID.GlassWall
        };

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => lateInstantiation &&
        FragileGlass.Contains(entity.type);

        public void ModifyItemPickSound(Item item, int context, bool putIn, ref SoundStyle? customSound, ref bool playOriginalSound) {
            if (context == ItemSlot.Context.InventoryItem) {
                playOriginalSound = false;
                SoundEngine.PlaySound(GlassPick);
            }
        }
    }
}
