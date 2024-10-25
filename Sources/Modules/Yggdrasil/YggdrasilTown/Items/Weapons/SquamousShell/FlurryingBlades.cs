using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Creative;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.SquamousShell;

public class FlurryingBlades : ModItem
{
	public override void SetStaticDefaults()
	{
		CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
	}

	public override void SetDefaults()
	{
		Item.damage = 21;
		Item.DamageType = DamageClass.Magic;
		Item.width = 40;
		Item.height = 46;
		Item.useTime = 11;
		Item.useAnimation = 11;
		Item.useStyle = ItemUseStyleID.Shoot;
		Item.noMelee = true;
		Item.knockBack = 0.8f;
		Item.value = 12400;
		Item.rare = ItemRarityID.Green;
		Item.autoReuse = true;
		Item.shoot = ModContent.ProjectileType<Projectiles.YggdrasilSlashBall>();
		Item.shootSpeed = 7;
		Item.crit = 16;
		Item.mana = 12;
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		if (player.ownedProjectileCounts[type] < 1)
		{
			float ai1 = Main.rand.NextFloat(0.7f, 1.4f);
			Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, 0.9f, ai1);
			SoundEngine.PlaySound(new SoundStyle("Everglow/Yggdrasil/YggdrasilTown/Sounds/Knife").WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)).WithVolume(0.8f), player.Center);
		}
		return false;
	}

	public override void HoldItem(Player player)
	{
	}
}