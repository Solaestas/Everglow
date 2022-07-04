using Everglow.Sources.Commons.Core.ModHooks;
using Terraria.Audio;
using Terraria.UI;

namespace Everglow.Sources.Modules.AssetReplaceModule
{
    public class ArmorPickSoundModify : GlobalItem, IModifyItemPickSound
    {
        public override bool AppliesToEntity(Item entity, bool lateInstantiation) =>
            lateInstantiation && entity.type == ItemID.Gel;

        public void ModifyItemPickSound(Item item, int context, bool putIn, ref SoundStyle? customSound, ref bool playOriginalSound) {
            if (context == ItemSlot.Context.InventoryItem) {
                playOriginalSound = false;
                // 左键点击
                if (Main.mouseLeft) {
                    if (putIn)
                        customSound = SoundID.NPCHit10;
                    else
                        customSound = SoundID.NPCHit2;
                }
                // 原版里右键优先级高于左键，所以右键应该放在后面
                // 右键点击（putIn必为false）
                if (Main.mouseRight) {
                    customSound = SoundID.NPCHit5;
                }
            }
        }
    }
}
