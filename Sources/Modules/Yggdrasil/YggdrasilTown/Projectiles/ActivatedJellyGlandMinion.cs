using Everglow.Yggdrasil.YggdrasilTown.Dusts;
using Everglow.Yggdrasil.YggdrasilTown.VFXs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class ActivatedJellyGlandMinion : ModProjectile
{
	private enum MinionState
	{
		Dash,
		Rest,
	}

	private const int NoTarget = -1;

	private MinionState State { get; set; } = MinionState.Rest;

	private int TargetWhoAmI { get; set; } = NoTarget;

	public override void SetStaticDefaults()
	{
		Main.projFrames[Projectile.type] = 10;
	}

	public override void SetDefaults()
	{
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 20;
		Projectile.timeLeft = 600;

		Projectile.penetrate = -1;
		Projectile.aiStyle = -1;
		Projectile.width = 10;
		Projectile.height = 10;

		Projectile.DamageType = DamageClass.Summon;
		Projectile.minion = true;
		Projectile.minionSlots = 0;

		ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;

		Main.projPet[Projectile.type] = true;
	}

	public override bool? CanCutTiles() => false;

	public override bool MinionContactDamage() => false;

	public override void DrawBehind(
		int index,
		List<int> behindNPCsAndTiles,
		List<int> behindNPCs,
		List<int> behindProjectiles,
		List<int> overPlayers,
		List<int> overWiresUI) => behindNPCsAndTiles.Add(index);

	public override void AI()
	{
		if (Main.time % 10 == 0)
		{
			Projectile.frame = (Projectile.frame + 1) % Main.projFrames[Projectile.type];
		}

		Player owner = Main.player[Projectile.owner];
		if (CheckPlayerNotActive(owner))
		{
			Projectile.Kill();
			return;
		}

		if (TargetWhoAmI == NoTarget)
		{
			GeneralBehavior();
			SearchTarget(owner);
		}
		else if (CheckTargetNotActive())
		{
			TargetWhoAmI = NoTarget;
			SearchTarget(owner);
		}
		else
		{
			Attack();
		}
	}

	private bool CheckPlayerNotActive(Player owner) => owner.dead || !owner.active ? true : false;

	private bool CheckTargetNotActive()
	{
		if (TargetWhoAmI == NoTarget)
		{
			return true;
		}

		NPC target = Main.npc[TargetWhoAmI];
		if (!target.active || target.dontTakeDamage)
		{
			return true;
		}

		return false;
	}

	private void SearchTarget(Player owner)
	{
		float minDistance = 800f;

		Vector2 detectCenter = owner.Center;

		bool foundTarget = false;
		if (owner.HasMinionAttackTargetNPC)
		{
			NPC npc = Main.npc[owner.MinionAttackTargetNPC];

			float between = Vector2.Distance(owner.Center, detectCenter);

			if (between < minDistance)
			{
				TargetWhoAmI = npc.whoAmI;
				foundTarget = true;
			}
		}

		if (!foundTarget)
		{
			foreach (var npc in Main.ActiveNPCs)
			{
				if (npc.dontTakeDamage || npc.friendly || !npc.CanBeChasedBy() || !Collision.CanHit(Projectile, npc))
				{
					continue;
				}

				float distance = (npc.Center - detectCenter).Length();
				if (distance < minDistance)
				{
					TargetWhoAmI = npc.whoAmI;
					minDistance = distance;
					foundTarget = true;
				}
			}
		}

		Projectile.friendly = foundTarget;
	}

	private void Attack()
	{
		// TODO: Complete code
		Projectile.velocity = Vector2.Zero;
	}

	public void GeneralBehavior()
	{
		// TODO: Complete code
		Projectile.velocity = Vector2.Zero;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		float glowStrength = State == MinionState.Rest ? 0.4f : 1f;

		Texture2D texture = ModAsset.JellyBall.Value;
		Texture2D textureG = ModAsset.JellyBall_glow.Value;
		Texture2D textureB = ModAsset.JellyBall_bloom.Value;

		Color drawColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
		int frameWidth = texture.Width / 2;
		int frameHeight = texture.Height / 5;
		int frameX = frameWidth * (Projectile.frame / 5);
		int frameY = frameHeight * (Projectile.frame % 5);

		Rectangle sourceRect = new Rectangle(frameX, frameY, frameWidth, frameHeight);

		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, sourceRect, Color.Lerp(drawColor * 0.7f, new Color(0.6f, 1f, 1f, 1f), 0.4f), Projectile.rotation, origin: new Vector2(texture.Width / 4f, texture.Height / 10f), Projectile.scale, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(textureB, Projectile.Center - Main.screenPosition, sourceRect, new Color(1f, 1f, 1f, 0f) * glowStrength, Projectile.rotation, origin: new Vector2(80), Projectile.scale, SpriteEffects.None, 0);
		Main.spriteBatch.Draw(textureG, Projectile.Center - Main.screenPosition, sourceRect, new Color(1f, 1f, 1f, 0f) * glowStrength, Projectile.rotation, origin: new Vector2(texture.Width / 4f, texture.Height / 10f), Projectile.scale, SpriteEffects.None, 0);

		return false;
	}

	public override void OnKill(int timeLeft)
	{
		if (Projectile.timeLeft < 200)
		{
			for (int i = 0; i < 12; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<JellyBallGel>());
				d.scale *= Main.rand.NextFloat(0.7f, 1.4f);
				d.velocity = new Vector2(Main.rand.NextFloat(2, 6f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
			for (int i = 0; i < 4; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<JellyBallSpark>());
				d.velocity = new Vector2(Main.rand.NextFloat(2, 6f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
			for (int g = 0; g < 32; g++)
			{
				Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(2, 24)).RotatedByRandom(MathHelper.TwoPi);
				float mulScale = Main.rand.NextFloat(6f, 12f);
				var blood = new JellyBallGelDrop
				{
					velocity = afterVelocity / mulScale,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
					maxTime = Main.rand.Next(42, 84),
					scale = mulScale,
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
				};
				Ins.VFXManager.Add(blood);
			}
			for (int g = 0; g < 8; g++)
			{
				Vector2 afterVelocity = new Vector2(0, Main.rand.NextFloat(2, 4)).RotatedByRandom(MathHelper.TwoPi);
				var blood = new JellyBallGelSplash
				{
					velocity = afterVelocity,
					Active = true,
					Visible = true,
					position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) - afterVelocity,
					maxTime = Main.rand.Next(32, 94),
					scale = Main.rand.NextFloat(6f, 24f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.4f), 0 },
				};
				Ins.VFXManager.Add(blood);
			}
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<JellyBallGel>());
				d.scale *= Main.rand.NextFloat(0.6f, 1.3f);
				d.velocity = new Vector2(Main.rand.NextFloat(1, 3f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
			for (int i = 0; i < 1; i++)
			{
				Dust d = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, ModContent.DustType<JellyBallSpark>());
				d.velocity = new Vector2(Main.rand.NextFloat(2, 4f), 0).RotatedByRandom(MathHelper.TwoPi);
			}
		}
	}
}