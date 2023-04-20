namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.DreamWeaver;

internal class DreamWeaverBook : MagicBookProjectile
{
	public override void SetDef()
	{
		DustType = DustID.WaterCandle;
		ItemType = ModContent.ItemType<TheFirefly.Items.Weapons.DreamWeaver>();
		GlowPath = "MagicWeaponsReplace/Projectiles/DreamWeaver/DreamWeaverFrontGlow";
		BackGlowPath = "MagicWeaponsReplace/Projectiles/DreamWeaver/DreamWeaverBackGlow";
		BackTexPath = "MagicWeaponsReplace/Projectiles/DreamWeaver/DreamWeaverBack";
		PaperTexPath = "MagicWeaponsReplace/Projectiles/DreamWeaver/DreamWeaverPaper";
		FrontTexPath = "MagicWeaponsReplace/Projectiles/DreamWeaver/DreamWeaverFront";
		TexCoordTop = new Vector2(9, 0);
		TexCoordLeft = new Vector2(0, 34);
		TexCoordDown = new Vector2(21, 46);
		TexCoordRight = new Vector2(34, 9);
		effectColor = new Color(0, 0, 55, 175);
	}

	public override void SpecialAI()
	{
		Player player = Main.player[Projectile.owner];
		if (player.itemTime <= 0 || player.HeldItem.type != ModContent.ItemType<TheFirefly.Items.Weapons.DreamWeaver>())
		{
			if (Timer < 0)
				Projectile.Kill();
		}
		if (player.itemTime == 2 && player.HeldItem.type == ItemType)
		{
			if (Main.mouseRight)
			{
				player.statMana -= 7;
				for (int d = 0; d < 2; d++)
				{
					Vector2 velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero).RotatedBy(Main.rand.NextFloat(-0.1f, 0.1f)) * player.HeldItem.shootSpeed * 1.3f;
					var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2 + new Vector2(0, -10), velocity, ModContent.ProjectileType<DreamWeaverII>(), player.HeldItem.damage * 2, player.HeldItem.knockBack, player.whoAmI);
					p.CritChance = player.GetWeaponCrit(player.HeldItem);
				}
			}
			else
			{
				Vector2 velocity = (Main.MouseWorld - Projectile.Center).SafeNormalize(Vector2.Zero) * player.HeldItem.shootSpeed * 1.3f;
				var p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + velocity * -2 + new Vector2(0, -10), velocity, ModContent.ProjectileType<DreamWeaverII>(), player.HeldItem.damage * 2, player.HeldItem.knockBack, player.whoAmI);
				p.CritChance = player.GetWeaponCrit(player.HeldItem);
			}
		}
	}
}