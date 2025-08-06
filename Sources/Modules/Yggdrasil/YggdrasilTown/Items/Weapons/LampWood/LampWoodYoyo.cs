namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.LampWood;

public class LampWoodYoyo : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MeleeWeapons;

    public override void SetDefaults()
    {
        Item.width = 24;
        Item.height = 24;

        Item.useStyle = ItemUseStyleID.Shoot;
        Item.useTime = 25;
        Item.useAnimation = 25;
        Item.noMelee = true;
        Item.noUseGraphic = true;
        Item.UseSound = SoundID.Item1;

        Item.damage = 9;
        Item.DamageType = DamageClass.MeleeNoSpeed;
        Item.knockBack = 2.5f;
        Item.channel = true;
        Item.rare = ItemRarityID.White;
        Item.value = Item.buyPrice(silver: 5);

        Item.shoot = ModContent.ProjectileType<Projectiles.LampWoodYoyo>();
        Item.shootSpeed = 16f;
    }
}