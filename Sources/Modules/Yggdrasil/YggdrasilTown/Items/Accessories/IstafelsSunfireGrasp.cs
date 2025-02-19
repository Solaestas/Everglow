using Everglow.Commons.Mechanics.ElementalDebuff;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Everglow.Yggdrasil.YggdrasilTown.Projectiles;
using Terraria.Audio;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public class IstafelsSunfireGrasp : ModItem
{
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
		player.GetDamage<MagicDamageClass>() += 0.2f;
		player.GetModPlayer<IstafelsSunfireGraspPlayer>().IstafelsSunfireGraspEnable = true;

		if (player.whoAmI == Main.myPlayer
			&& (Main.mouseMiddle && Main.mouseMiddleRelease)
			&& !player.HasBuff<IstafelsSunfireGraspSkillCooldown>())
		{
			player.GetModPlayer<IstafelsSunfireGraspPlayer>().SkillEnable = true;
		}
	}

	public class IstafelsSunfireGraspPlayer : ModPlayer
	{
		public const float ElementDebuffBuildUpRate = 2f;
		public const int FireBallBuildUpMax = 200;
		public const int FireBallCooldown = 600;

		public const int SkillCooldown = 3600;
		public const int SkillDuration = 1200;
		public const int SkillDistance = 1200;
		public const float SkillDamageBonus = 0.5f;

		public bool IstafelsSunfireGraspEnable { get; set; }

		public bool SkillEnable { get; set; } = false;

		public int FireBallBuildUp { get; set; }

		public IstafelsSunfireGrasp_FireBall FireBallProj { get; private set; }

		public override void ResetEffects()
		{
			if (!IstafelsSunfireGraspEnable)
			{
				FireBallBuildUp = 0;
			}

			IstafelsSunfireGraspEnable = false;
		}

		public override void PostUpdateMiscEffects()
		{
			if (Player.HasBuff<IstafelsSunfireGraspSkillCooldown>())
			{
				return;
			}

			if (!SkillEnable)
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
			Player.AddBuff(ModContent.BuffType<IstafelsSunfireGraspSkillCooldown>(), SkillCooldown);
			Player.AddBuff(ModContent.BuffType<IstafelsSunfireGraspSkillBuff>(), SkillDuration);

			// Play sound effect
			SoundEngine.PlaySound(SoundID.Item130, Player.Center);

			// Update symbol
			SkillEnable = false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (IstafelsSunfireGraspEnable
			   && hit.DamageType == DamageClass.Magic)
			{
				target.AddElementalDebuffBuildUp(Player, ElementalDebuffType.Burn, (int)(damageDone * ElementDebuffBuildUpRate));

				if (!Player.HasBuff<IstafelsSunfireGraspFireBallCooldown>())
				{
					if (FireBallProj == null || !FireBallProj.Projectile.active)
					{
						var proj = Projectile.NewProjectileDirect(Player.GetSource_FromAI(), Player.Center, Player.velocity, ModContent.ProjectileType<IstafelsSunfireGrasp_FireBall>(), 1, 1.2f, Player.whoAmI);
						Player.AddBuff(ModContent.BuffType<IstafelsSunfireGraspFireBallBuff>(), 2);
						FireBallProj = (IstafelsSunfireGrasp_FireBall)proj.ModProjectile;
					}

					FireBallBuildUp += damageDone;
					FireBallProj.BuildUpProgress = Math.Clamp(FireBallBuildUp / (float)FireBallBuildUpMax, 0f, 1f);
					if (FireBallBuildUp > FireBallBuildUpMax)
					{
						ShootFireBall(target);
					}
				}
			}
		}

		public void ShootFireBall(NPC target)
		{
			FireBallBuildUp = 0;
			Player.AddBuff(ModContent.BuffType<IstafelsSunfireGraspFireBallCooldown>(), FireBallCooldown);

			FireBallProj.Active(target);
			FireBallProj = null;
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