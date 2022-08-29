using Everglow.Sources.Modules.MythModule.Common;
using Terraria.ID;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items
{
    public class EvilCocoon : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Magical Cocoon");
            Tooltip.SetDefault("Summons Corrupted Moth");
            GetGlowMask = MythContent.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.width = 20;
            Item.height = 32;
            Item.useAnimation = 45;
            Item.useTime = 60;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.maxStack = 999;
            Item.consumable = true;
        }
        public override void ModifyTooltips(List<TooltipLine> list)
        {
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
        }
        public override bool CanUseItem(Player player)
        {
            if (NPC.CountNPCS(ModContent.NPCType<Bosses.CorruptMoth.NPCs.CorruptMoth>()) < 1)
            {
                NPC.NewNPC(null, (int)Main.MouseWorld.X, (int)Main.MouseWorld.Y + 50, ModContent.NPCType<Bosses.CorruptMoth.NPCs.EvilPack>(), 0, 0f, 0f, 0f, 0f, 255);
                Item.stack--;
                return true;
            }
            return false;
        }
    }
}
