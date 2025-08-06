using Everglow.Yggdrasil.YggdrasilTown.Projectiles.Magic;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons;

public class MagicOfLightAndShadow : ModItem
{
    public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MagicWeapons;

    public override void SetStaticDefaults()
    {
        Item.staff[Type] = true;
    }

    public override void SetDefaults()
    {
        Item.width = 48;
        Item.height = 48;

        Item.DamageType = DamageClass.Magic;
        Item.damage = 33;
        Item.knockBack = 0.5f;
        Item.mana = 9;

        Item.useStyle = ItemUseStyleID.Shoot;
        Item.UseSound = SoundID.Item20;
        Item.useTime = Item.useAnimation = 17;
        Item.noMelee = true;
        Item.autoReuse = false;
        Item.rare = ItemRarityID.Green;
        Item.value = 14400;

        Item.shoot = ModContent.ProjectileType<MagicOfLightAndShadow_Proj>();
        Item.shootSpeed = 8;
    }

    public override bool AltFunctionUse(Player player) => base.AltFunctionUse(player);

    public override bool? UseItem(Player player) => base.UseItem(player);

    public override bool CanUseItem(Player player)
    {
        if (player.ownedProjectileCounts[Item.shoot] >= 8)
        {
            return false;
        }
        return base.CanUseItem(player);
    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI);
        return false;
    }
}