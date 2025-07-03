using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.Yggdrasil.KelpCurtain.Projectiles.Weapons;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons.DevilHeart;

public class DevilHeartBayonet : StabbingSwordItem
{
	public override void SetDefaults()
	{
		Item.damage = 22;
		Item.knockBack = 1.5f;
		Item.rare = ItemRarityID.Green;
		Item.value = Item.sellPrice(0, 2, 0, 0);
		Item.shoot = ModContent.ProjectileType<DevilHeartBayonet_proj>();
		StabMulDamage = 4f;
		PowerfulStabProj = ModContent.ProjectileType<DevilHeartBayonet_proj_stab>();
		base.SetDefaults();
	}

	public override bool AltFunctionUse(Player player)
	{
		if (stabCD > 0)
		{
			return false;
		}

		if (!player.GetModPlayer<PlayerStamina>().CheckStamina(staminaCost * 45))
		{
			return false;
		}

		foreach (Projectile proj in Main.projectile)
		{
			if (proj.owner == player.whoAmI && proj.timeLeft > 1 && proj.type == PowerfulStabProj)
			{
				return false;
			}
		}
		if (player.statLife <= 15)
		{
			return false;
		}
		return true;
	}

	public override void Update(ref float gravity, ref float maxFallSpeed) => base.Update(ref gravity, ref maxFallSpeed);

	public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
	{
		return base.PreDrawInWorld(spriteBatch, lightColor, alphaColor, ref rotation, ref scale, whoAmI);
	}
}