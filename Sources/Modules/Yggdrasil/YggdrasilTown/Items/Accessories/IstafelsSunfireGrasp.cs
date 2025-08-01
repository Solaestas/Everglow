using Everglow.Commons.Mechanics.Cooldown;
using Everglow.Commons.Mechanics.ElementalDebuff;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class IstafelsSunfireGrasp : ModItem
{
	public const float MagicDamageBonus = 0.11f;
	public const float MagicCritBonus = 5f;

	public override void SetStaticDefaults()
	{
		Item.SetNameOverride("Istafel's Sunfire Grasp");
	}

	public override void SetDefaults()
	{
		Item.width = 34;
		Item.height = 38;

		Item.accessory = true;

		Item.rare = ItemRarityID.Expert;
		Item.value = Item.buyPrice(gold: 10);
	}

	public override void ModifyTooltips(List<TooltipLine> tooltips)
	{
		tooltips.Add(new TooltipLine(Mod, "Expert", $"Legendary"));
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		// Test Code
		// if(Main.mouseLeft && Main.mouseLeftRelease)
		// {
		// // Shoot explosion
		// Projectile.NewProjectile(Item.GetSource_FromAI(), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<IstafelsSunfireGrasp_Explosion>(), 200, 1f, player.whoAmI);

		// // Shoot scoria
		// var scoriaProjCount = Main.rand.Next(8, 10);
		// for (int i = 0; i < scoriaProjCount; i++)
		// {
		// var velocity = player.velocity * 0.6f + Vector2.UnitX.RotatedBy(Main.rand.NextFloat(MathHelper.TwoPi)) * 4f;
		// Projectile.NewProjectile(Item.GetSource_FromAI(), Main.MouseWorld, velocity, ModContent.ProjectileType<IstafelsSunfireGrasp_Scoria>(), 1, 1.1f, player.whoAmI);
		// }
		// }
		player.GetDamage<MagicDamageClass>() += MagicDamageBonus;
		player.GetCritChance<MagicDamageClass>() += MagicCritBonus;
		player.GetModPlayer<IstafelsSunfireGraspPlayer>().IstafelsSunfireGraspEnable = true;

		if (player.whoAmI == Main.myPlayer
			&& (Main.mouseMiddle && Main.mouseMiddleRelease)
			&& !player.HasCooldown<IstafelsSunfireGraspSkillCooldown>())
		{
			player.GetModPlayer<IstafelsSunfireGraspPlayer>().SkillEnable = true;
		}
	}

	public class IstafelsSunfireGraspPlayer : ModPlayer
	{
		public const float ElementDebuffBuildUpRate = 2f;
		public const int FireBallCooldown = 600;

		public const int SkillCooldown = 3600;
		public const int SkillDuration = 1200;
		public const int SkillDistance = 1200;
		public const float SkillDamageBonus = 0.5f;

		public bool IstafelsSunfireGraspEnable { get; set; }

		public bool SkillEnable { get; set; } = false;

		public IstafelsSunfireGrasp_FireBall FireBallProj { get; private set; }

		public override void ResetEffects()
		{
			if (!IstafelsSunfireGraspEnable)
			{
				if (FireBallProj != null)
				{
					FireBallProj.BuildUp = 0;
				}
			}

			IstafelsSunfireGraspEnable = false;
		}

		public override void PostUpdateMiscEffects()
		{
			// Handle skill trigger
			if (Player.HasCooldown<IstafelsSunfireGraspSkillCooldown>()
				|| !SkillEnable)
			{
				return;
			}

			Skill();
		}

		/// <summary>
		/// The skill of <see cref="IstafelsSunfireGrasp"/>
		/// </summary>
		private void Skill()
		{
			// If the target is currently receiving flame debuffs, all debuffs currently placed on that enemy
			// immediately produce DMG equal to 100% of their original DMG
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.friendly || npc.townNPC || npc.dontTakeDamage || npc.Center.Distance(Player.Center) > SkillDistance)
				{
					continue;
				}

				if (HasFlameDebuff(npc))
				{
					ApplyFlameDebuffDamage(npc);

					// Generate skill VFX
					Projectile.NewProjectile(Player.GetSource_FromAI(), npc.Center, Vector2.Zero, ModContent.ProjectileType<IstafelsSunfireGrasp_SkillVFX>(), 0, 0, Player.whoAmI, npc.whoAmI, Math.Max(npc.width, npc.height));
				}
			}

			// Add skill buff and cooldown debuff
			Player.AddCooldown(IstafelsSunfireGraspSkillCooldown.ID, SkillCooldown);
			Player.AddBuff(ModContent.BuffType<IstafelsSunfireGraspSkillBuff>(), SkillDuration);

			// Play sound effect
			SoundEngine.PlaySound(SoundID.Item130, Player.Center);

			// Update symbol
			SkillEnable = false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (IstafelsSunfireGraspEnable && hit.DamageType == DamageClass.Magic)
			{
				target.AddElementalDebuffBuildUp(Player, ElementalDebuffType.Burn, (int)(damageDone * ElementDebuffBuildUpRate));

				ManageFireBall(target, damageDone);
			}
		}

		private void ManageFireBall(NPC target, int damageDone)
		{
			if (!Player.HasCooldown<IstafelsSunfireGraspFireBallCooldown>())
			{
				// If don't have fire ball, create one
				if (FireBallProj == null || !FireBallProj.Projectile.active)
				{
					var proj = Projectile.NewProjectileDirect(Player.GetSource_FromAI(), Player.Center, Player.velocity, ModContent.ProjectileType<IstafelsSunfireGrasp_FireBall>(), 1, 1.2f, Player.whoAmI);
					Player.AddBuff(ModContent.BuffType<IstafelsSunfireGraspFireBallBuff>(), 2);
					FireBallProj = (IstafelsSunfireGrasp_FireBall)proj.ModProjectile;
				}

				// Add build-up
				FireBallProj.BuildUp += damageDone / 3;

				// If build-up is max, shoot fire ball
				if (FireBallProj.BuildUp > IstafelsSunfireGrasp_FireBall.BuildUpMax)
				{
					Player.AddCooldown(IstafelsSunfireGraspFireBallCooldown.ID, FireBallCooldown);

					FireBallProj.Active(target);
					FireBallProj = null;
				}
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			if (Player.HasBuff<IstafelsSunfireGraspSkillBuff>())
			{
				if (HasFlameDebuff(target))
				{
					modifiers.FinalDamage.Additive += SkillDamageBonus;
				}
			}
		}

		private static bool HasFlameDebuff(NPC target)
		{
			return BuffUtils.VanillaFlameDebuffs.Any(target.HasBuff)
				|| target.HasBuff(ModContent.BuffType<Charred>());
		}

		private static void ApplyFlameDebuffDamage(NPC target)
		{
			var vanillaDotDamage = target.GetVanillaDotDamage(BuffUtils.VanillaFlameDebuffs);

			var charredIndex = target.FindBuffIndex(ModContent.BuffType<Charred>());
			var charredDamage = charredIndex >= 0 ? target.buffTime[charredIndex] * Charred.LifeRegenReductionFromDot : 0;

			var totalDamage = vanillaDotDamage + charredDamage;

			target.lifeRegenCount -= totalDamage;
			if (target.life < 1)
			{
				target.life = 1;
			}
		}
	}
}