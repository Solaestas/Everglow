using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace Everglow.Ocean.OceanDeep.Items
{
    public class VoidBubble : ModItem
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("空灵泡");
            // Tooltip.SetDefault("");
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(4, 4));
            ItemID.Sets.AnimatesAsSoul[Item.type] = true;
            ItemID.Sets.ItemIconPulse[Item.type] = true;
        }
        public override void SetDefaults()
        {
            Item refItem = new Item();
            Item.width = refItem.width;
            Item.height = refItem.height;
            Item.maxStack = 999;
            Item.value = 2000;
            Item.rare = 8;
        }
        public override void Update(ref float gravity, ref float maxFallSpeed)
        {
            maxFallSpeed *= -0.75f;
            Lighting.AddLight((int)((base.Item.position.X + (float)(base.Item.width / 2)) / 16f), (int)((base.Item.position.Y + (float)(base.Item.height / 2)) / 16f), 113*0.005f, 31*0.005f, 158*0.005f);
        }
    }
}