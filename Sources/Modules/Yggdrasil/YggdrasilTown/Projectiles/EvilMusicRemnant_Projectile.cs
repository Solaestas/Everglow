using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EvilMusicRemnant_Projectile : ModProjectile
{
	public const int SearchDistance = 200;
	public Vector2 ClickCenter;
	public int MaxTime = 600;

	public int TargetWhoAmI
	{
		get => (int)Projectile.ai[0];
		set => Projectile.ai[0] = value;
	}

	public override void SetDefaults()
	{
		Projectile.width = 26;
		Projectile.height = 26;

		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.timeLeft = MaxTime;
	}

	public override void OnSpawn(IEntitySource source)
	{
		// Select random texture
		Projectile.timeLeft += Main.rand.Next(-30, 30);
		ClickCenter = Main.MouseWorld;
		var num = Main.rand.Next(6);
		switch (num)
		{
			case 1:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile2_Mod;
				break;
			case 2:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile3_Mod;
				break;
			case 3:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile4_Mod;
				break;
			case 4:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile5_Mod;
				break;
			case 5:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile6_Mod;
				break;
			default:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile1_Mod;
				break;
		}

		TargetWhoAmI = -1;
	}

	private string ProjectileTexture { get; set; } = ModAsset.EvilMusicRemnant_Projectile1_Mod;

	public override string Texture => ProjectileTexture;

	public override void AI()
	{
		if (TargetWhoAmI < 0)
		{
			TargetWhoAmI = ProjectileUtils.FindTarget(Projectile.Center, SearchDistance);
			if (TargetWhoAmI >= 0)
			{
				Projectile.netUpdate = true;
			}
			else if(Projectile.timeLeft < MaxTime - 30)
			{
				var ellipses = new Vector2(0, 80).RotatedBy(Projectile.whoAmI * MathHelper.TwoPi * 3f / 7f + Main.time * 0.04f);
				ellipses.Y *= 0.15f;
				var targetPos = ClickCenter + ellipses;
				var directionToTarget = (targetPos - Projectile.Center - Projectile.velocity).NormalizeSafe();
				Projectile.velocity = MathUtils.Lerp(0.05f, Projectile.velocity, directionToTarget * 6);
			}
		}
		else
		{
			if (!ProjectileUtils.MinionCheckTargetActive(TargetWhoAmI))
			{
				TargetWhoAmI = -1;
				return;
			}

			var target = Main.npc[TargetWhoAmI];
			var directionToTarget = (target.Center - Projectile.Center - Projectile.velocity).NormalizeSafe();
			Projectile.velocity = MathUtils.Lerp(0.05f, Projectile.velocity, directionToTarget * 6);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		float fade = 1f;
		if (Projectile.timeLeft < 60f)
		{
			fade = Projectile.timeLeft / 60f;
		}
		var texture = ModContent.Request<Texture2D>(ProjectileTexture).Value;
		float tremorValue = MathF.Sin(Projectile.timeLeft * 0.1f) * 0.3f + 1f;
		for (int i = 0; i < 6; i++)
		{
			var termorPos = new Vector2(2, 0).RotatedBy(i / 6f * MathHelper.TwoPi);
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + termorPos, null, new Color(153, 110, 255, 255) * 0.45f * fade, 0, texture.Size() / 2, tremorValue, SpriteEffects.None, 0);
		}
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(74, 23, 124, 255) * fade, 0, texture.Size() / 2, tremorValue, SpriteEffects.None, 0);
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (!target.active && !target.friendly && !target.townNPC)
		{
			SummonMinion();
		}
	}

	public void SummonMinion()
	{
		var owner = Main.player[Projectile.owner];
		var minionProjType = ModContent.ProjectileType<EvilMusicRemnant_Minion>();

		if (owner.maxMinions <= owner.slotsMinions)
		{
			if (owner.ownedProjectileCounts[minionProjType] <= 0)
			{
				return;
			}
			else
			{
				var queryMinions = Main.projectile.Where(x => x.type == minionProjType && x.active);
				if (queryMinions.Any())
				{
					queryMinions.Last().Kill();
				}
			}
		}

		var index = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, minionProjType, Projectile.damage, Projectile.knockBack, Projectile.owner, owner.ownedProjectileCounts[minionProjType] + 1);
		owner.AddBuff(ModContent.BuffType<Buffs.EvilMusicRemnant>(), 30);
	}
}