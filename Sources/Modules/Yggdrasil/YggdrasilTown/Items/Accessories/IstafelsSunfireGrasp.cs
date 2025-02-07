using Everglow.Commons.Mechanics.ElementDebuff;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;
using Terraria.DataStructures;

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
		public const int FireBallCooldown = 9000;

		public const int SkillCooldown = 3600;
		public const int SkillDuration = 1200;
		public const int SkillDistance = 1200;
		public const float SkillDamageBonus = 0.5f;

		public bool IstafelsSunfireGraspEnable { get; set; }

		public bool SkillEnable { get; set; } = false;

		public int FireBallBuildUp { get; set; }

		public IstafelsSunfireGrasp_Projectile FireBallProj { get; private set; }

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
				}
			}

			Player.AddBuff(ModContent.BuffType<IstafelsSunfireGraspSkillCooldown>(), SkillCooldown);
			Player.AddBuff(ModContent.BuffType<IstafelsSunfireGraspSkillBuff>(), SkillDuration);

			SkillEnable = false;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (IstafelsSunfireGraspEnable
			   && hit.DamageType == DamageClass.Magic)
			{
				target.GetGlobalNPC<ElementDebuffGlobalNPC>().ElementDebuffs[ElementDebuffType.Burn].AddBuildUp((int)(damageDone * ElementDebuffBuildUpRate));

				if (!Player.HasBuff<IstafelsSunfireGraspFireBallCooldown>())
				{
					if (FireBallProj == null)
					{
						var proj = Projectile.NewProjectileDirect(Player.GetSource_FromAI(), Player.Center, Player.velocity, ModContent.ProjectileType<IstafelsSunfireGrasp_Projectile>(), 0, 0.4f, Player.whoAmI);
						FireBallProj = (IstafelsSunfireGrasp_Projectile)proj.ModProjectile;
					}

					FireBallBuildUp += damageDone;
					if (FireBallBuildUp > FireBallBuildUpMax)
					{
						ShootProjectile(target);
					}
				}
			}
		}

		public void ShootProjectile(NPC target)
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
					modifiers.FinalDamage.Multiplicative += SkillDamageBonus;
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
			var charredDamage = charredIndex >= 0 ? target.buffTime[charredIndex] * Charred.DotDamage : 0;

			var totalDamage = vanillaDotDamage + charredDamage;

			target.lifeRegenCount -= totalDamage;
			if (target.life < 1)
			{
				target.life = 1;
			}
		}
	}

	public class IstafelsSunfireGrasp_Projectile : ModProjectile
	{
		public const int ProjectileVelocity = 16;

		public override void SetDefaults()
		{
			Projectile.friendly = true;
			Projectile.hostile = true;

			Projectile.timeLeft = 300;

			Projectile.tileCollide = true;
			Projectile.penetrate = -1;
			Projectile.ignoreWater = true;

			Projectile.localNPCHitCooldown = 10;
			Projectile.DamageType = DamageClass.Magic;
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			if (Projectile.velocity.X != oldVelocity.X)
			{
				Projectile.velocity.X = -oldVelocity.X;
			}

			if (Projectile.velocity.Y != oldVelocity.Y)
			{
				Projectile.velocity.Y = -oldVelocity.Y;
			}

			Projectile.velocity *= 0.69f;
			return false;
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			modifiers.FinalDamage += 25;
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			const int BuffDuration = 960;
			target.AddBuff(ModContent.BuffType<Charred>(), BuffDuration);
		}

		public override void AI()
		{
			Projectile.velocity.Y += 0.2f;
			Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
			if (Projectile.velocity.Y > 16f)
			{
				Projectile.velocity.Y = 16f;
			}
		}

		public void Active(NPC target)
		{
			Projectile.velocity = (target.Center - Main.player[Projectile.owner].Center).NormalizeSafe() * ProjectileVelocity;
		}
	}
}