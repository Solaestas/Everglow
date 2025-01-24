using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EvilMusicRemnant_Projectile : ModProjectile
{
	public const int SearchDistance = 200;

	public int TargetWhoAmI
	{
		get => (int)Projectile.ai[0];
		set => Projectile.ai[0] = value;
	}

	public override void SetDefaults()
	{
		Projectile.width = 18;
		Projectile.height = 24;

		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.timeLeft = 300;

		TargetWhoAmI = -1;
	}

	public override void OnSpawn(IEntitySource source)
	{
		// Select random texture
		var num = Main.rand.Next(3);
		switch (num)
		{
			case 1:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile2_Mod;
				Projectile.width = 18;
				Projectile.height = 24;
				break;
			case 2:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile3_Mod;
				Projectile.width = 10;
				Projectile.height = 22;
				break;
			default:
				ProjectileTexture = ModAsset.EvilMusicRemnant_Projectile1_Mod;
				Projectile.width = 22;
				Projectile.height = 24;
				break;
		}
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
		}
		else
		{
			if (!ProjectileUtils.MinionCheckTargetActive(TargetWhoAmI))
			{
				TargetWhoAmI = -1;
				return;
			}

			var target = Main.npc[TargetWhoAmI];
			var directionToTarget = (target.Center - Projectile.Center).NormalizeSafe();
			Projectile.velocity = MathUtils.Lerp(0.05f, Projectile.velocity, directionToTarget * 6);
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = ModContent.Request<Texture2D>(ProjectileTexture).Value;
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, Color.White, 0, texture.Size() / 2, 1, SpriteEffects.None, 0);
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