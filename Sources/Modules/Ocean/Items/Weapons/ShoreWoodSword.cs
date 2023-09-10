using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
namespace Everglow.Ocean.Items.Weapons
{
    public class ShoreWoodSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "滨岸木剑");
        }
        public override void SetDefaults()
        {
            Item.damage = 40;
            Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 18;
            Item.rare = 3;
            Item.useAnimation = 12;
            Item.useStyle = 1;
            Item.knockBack = 4;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 2;
            Item.value = 1200;
            Item.scale = 1f;

        }
    }
}
