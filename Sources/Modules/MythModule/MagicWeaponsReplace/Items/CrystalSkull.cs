using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Items
{
    public class CrystalSkull : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 24;
            Item.height = 26;
            Item.value = 2000;
            Item.accessory = true;
            Item.rare = 4;
            //Item.vanity = true;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<GlobalItems.MagicBookPlayer>().MagicBookLevel = 1;
            base.UpdateAccessory(player, hideVisual);
        }
    }
}
