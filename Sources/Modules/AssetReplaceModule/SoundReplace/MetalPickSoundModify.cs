using Everglow.Sources.Commons.Core.ModHooks;
using Terraria.Audio;
using Terraria.UI;

namespace Everglow.Sources.Modules.AssetReplaceModule.SoundReplace
{
    public class MetalPickSoundModify : GlobalItem, IModifyItemPickSound
    {
        public static readonly SoundStyle MetalPick = new($"Everglow/Resources/Sounds/Metal", SoundType.Sound) {
            Volume = 1f,
            PitchRange = (-0.3f, 0.5f),
            MaxInstances = 0
        };

        public static readonly int[] Bars = new int[] {
            57, 117, 175, 381, 382, 391, 1006, 1184, 1191, 1198, 1225, 1257, 1552, 1706, 1715, 3261, 3467
        };

        public static readonly int[] Ores = new int[] {
            56, 880, 947, 3460
        };

        public static readonly int[] Other = new int[] {
            ItemID.LeadAnvil, ItemID.IronAnvil, ItemID.MythrilAnvil, ItemID.OrichalcumAnvil
        };

        public override bool AppliesToEntity(Item entity, bool lateInstantiation) => lateInstantiation &&
        (entity.type >= ItemID.IronOre && entity.type <= ItemID.SilverOre) ||
        (entity.type >= ItemID.GoldBar && entity.type <= ItemID.IronBar) ||
        (entity.type >= ItemID.TinBar && entity.type <= ItemID.PlatinumBar) ||
        (entity.type >= ItemID.TinOre && entity.type <= ItemID.PlatinumOre) ||
        (entity.type >= ItemID.CobaltOre && entity.type <= ItemID.AdamantiteOre) ||
        (entity.type >= ItemID.PalladiumOre && entity.type <= ItemID.TitaniumOre) ||
        Bars.Contains(entity.type) || Ores.Contains(entity.type) || Other.Contains(entity.type);

        public void ModifyItemPickSound(Item item, int context, bool putIn, ref SoundStyle? customSound, ref bool playOriginalSound) {
            if (context == ItemSlot.Context.InventoryItem) {
                playOriginalSound = false;
                SoundEngine.PlaySound(MetalPick);
            }
        }
    }
}
