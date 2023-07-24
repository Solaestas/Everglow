using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
namespace MythMod.Items.Shore
{
    public class ShoreWoodSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.AddTranslation(GameCulture.Chinese, "滨岸木剑");
        }
        public override void SetDefaults()
        {
            item.damage = 40;
            item.melee = true;
            item.width = 50;
            item.height = 50;
            item.useTime = 18;
            item.rare = 3;
            item.useAnimation = 12;
            item.useStyle = 1;
            item.knockBack = 4;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 2;
            item.value = 1200;
            item.scale = 1f;

        }
    }
}
