namespace Everglow.Sources.Modules.MythModule.MiscItems.Accessories
{
    public class SilverWing : ModItem
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Badge of Silver Wings");
            //DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "银翼之章");
            //Tooltip.SetDefault("Increases crit damage by 8%\nIf you have not been using a weapon for 3s, increases damage by 40%\n'Concentrate the power of your soul'");
            //Tooltip.AddTranslation((int)GameCulture.CultureName.Chinese, "暴击伤害增加8%\n若3秒没有使用武器则伤害增加40%\n'聚集你灵魂的力量'");
        }
        public override void SetDefaults()
        {
            Item.width = 60;
            Item.height = 26;
            Item.value = 1000;
            Item.accessory = true;
            Item.rare = 0;
        }
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            //if (MythPlayer.damage40 <= 1)
            //{
                player.GetDamage(DamageClass.Generic) *= 1.4f;
            //}
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.SilverBar, 14)
                .AddIngredient(ItemID.Ruby, 8)
                .AddTile(TileID.Anvils)
                .Register();
            CreateRecipe()
                .AddIngredient(ItemID.TungstenBar, 14)
                .AddIngredient(ItemID.Ruby, 8)
                .AddTile(TileID.Anvils)
                .Register();
        }
    }
}
