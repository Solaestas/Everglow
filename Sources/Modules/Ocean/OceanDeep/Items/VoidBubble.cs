using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace MythMod.Items.UnderSea
{
    public class VoidBubble : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("空灵泡");
            Tooltip.SetDefault("");
            Main.RegisterItemAnimation(item.type, new DrawAnimationVertical(4, 4));
            ItemID.Sets.AnimatesAsSoul[item.type] = true;
            ItemID.Sets.ItemIconPulse[item.type] = true;
        }
        public override void SetDefaults()
        {
            Item refItem = new Item();
            item.width = refItem.width;
            item.height = refItem.height;
            item.maxStack = 999;
            item.value = 2000;
            item.rare = 8;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            maxFallSpeed *= -0.75f;
            Lighting.AddLight((int)((base.item.position.X + (float)(base.item.width / 2)) / 16f), (int)((base.item.position.Y + (float)(base.item.height / 2)) / 16f), 113*0.005f, 31*0.005f, 158*0.005f);
        }
    }
}