namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons
{
    public class CyanFrost : ModItem
    {
        /*public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Cyfrost Blade");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Chinese, "青霜剑");
            DisplayName.AddTranslation((int)GameCulture.CultureName.Russian, "Морозно-голубой клинок");
        }*/
        private int o = 0;
        public override void SetDefaults()
        {
            Item.damage = 30;//伤害 原75→现37
            Item.DamageType = DamageClass.Melee; // Makes the damage register as magic. If your item does not have any damage type, it becomes true damage (which means that damage scalars will not affect it). Be sure to have a damage type.
            Item.width = 104;
            Item.height = 104;
            Item.useTime = 17;
            Item.useAnimation = 17;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.knockBack = 2;
            Item.value = 10000;
            Item.rare = 1;
            Item.UseSound = SoundID.Item1;
            Item.autoReuse = true;
            Item.crit = 16;
        }
    }
}
