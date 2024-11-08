using Everglow.Yggdrasil.WorldGeneration;
using Terraria;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class DeadBeetleEgg_beetle : ModProjectile
{
	private Player Owner => Main.player[Projectile.owner];

	public int EnemyTarget;

	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.tileCollide = true;
		Projectile.timeLeft = 300;
		Projectile.aiStyle = -1;
		Projectile.penetrate = -1;
		Projectile.DamageType = DamageClass.Summon;
	}

	public int ManaValue = 0;

	/// <summary>
	/// 0 : walk;1 : fly
	/// </summary>
	public int State = 0;

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.ai[0] = 0;
		Projectile.ai[1] = 0;
		EnemyTarget = -1;
		base.OnSpawn(source);
	}

	public override void AI()
	{
		Projectile.timeLeft = 300;
		if (Projectile.velocity.X > 1)
		{
			Projectile.spriteDirection = -1;
		}
		if (Projectile.velocity.X < -1)
		{
			Projectile.spriteDirection = 1;
		}
		FindFrame();
		EnemyTarget = FindEnemy();
		if (EnemyTarget != -1)
		{
			AttackNoMagic();
		}
		else
		{
			ApproachMyOwner();
		}
	}

	/// <summary>
	/// When there are no enemies, try go back to player.
	/// </summary>
	public void ApproachMyOwner()
	{
		Vector2 targetPos = FindTargetPosWhenNoEnemies();
		bool PlayerStand = Collision.SolidCollision(Owner.BottomLeft, Owner.width, 4);
		Vector2 toTarget = targetPos - Projectile.velocity - Projectile.Center;
		if (toTarget.Length() > 2400)
		{
			Projectile.Center = targetPos;
		}
		if (PlayerStand)
		{
			if (Main.rand.NextBool(16))
			{
				State = 0;
			}
		}
		else
		{
			if (Main.rand.NextBool(16))
			{
				State = 1;
			}
		}
		if (State == 0)
		{
			if (toTarget.Length() > 8)
			{
				toTarget = Vector2.Normalize(toTarget);
				if (Collision.SolidCollision(Projectile.Bottom, 2, 8))
				{
					Projectile.velocity.X += toTarget.X * 0.5f;
				}
				else
				{
					Projectile.velocity.X += toTarget.X * 0.1f;
				}
				if (Projectile.velocity.Length() > 8f)
				{
					Projectile.velocity = Vector2.Normalize(Projectile.velocity) * 8;
				}
				Projectile.velocity.Y += 0.3f;
			}
			else
			{
				Projectile.velocity *= 0f;
			}
		}
		if (State == 1)
		{
			if (toTarget.Length() > 8)
			{
				toTarget = Vector2.Normalize(toTarget);
				Projectile.velocity += toTarget * 0.5f;
				Projectile.velocity *= 0.95f;
			}
			else
			{
				Projectile.velocity *= 0f;
			}
		}
	}

	public int FindEnemy()
	{
		if (Owner.MinionAttackTargetNPC != -1)
		{
			return Owner.MinionAttackTargetNPC;
		}
		float minDetectionRange = 1200;
		int targeWhoAmI = -1;
		foreach (NPC npc in Main.npc)
		{
			if (npc != null && npc.active && npc.life > 0 && !npc.friendly)
			{
				if((npc.Center - Owner.Center).Length() < 1200)
				{
					float distance = (npc.Center - Projectile.Center).Length();
					if (distance < minDetectionRange)
					{
						targeWhoAmI = npc.whoAmI;
						minDetectionRange = distance;
					}
				}
			}
		}
		return targeWhoAmI;
	}

	public void AttackNoMagic()
	{
		State = 1;
		NPC target = Main.npc[EnemyTarget];
		if(target == null || !target.active || target.life <= 0 || target.friendly)
		{
			EnemyTarget = -1;
			return;
		}
		Vector2 relativePos = new Vector2(0, -45).RotatedBy(Projectile.ai[1]);
		Vector2 targetPos = target.Center + relativePos;
		Vector2 toTarget = targetPos - Projectile.velocity - Projectile.Center;
		if (Projectile.ai[0] <= 0)
		{
			if (toTarget.Length() > 8)
			{
				toTarget = Vector2.Normalize(toTarget);
				Projectile.velocity += toTarget * 0.5f;
				Projectile.velocity *= 0.95f;
			}
			else
			{
				Projectile.ai[0] = 20f;
			}
		}
		else
		{
			if(Projectile.ai[0] == 20)
			{
				Projectile.friendly = true;
				Projectile.velocity = -relativePos * 0.7f;
				Projectile.ai[1] = Main.rand.NextFloat(-1.3f, 1.3f);
			}
			if(Projectile.ai[0] == 1)
			{
				Projectile.friendly = false;
			}
			Projectile.velocity *= 0.9f;
			Projectile.ai[0]--;
		}
	}

	public Vector2 FindTargetPosWhenNoEnemies()
	{
		// when state is walk, line up by whoAmI.
		if (State == 0)
		{
			// A serrated function.
			float deltaX = MathF.Acos(MathF.Cos(GetOrderFromOwner() * 0.1f));
			Vector2 targetPos = Owner.Center + new Vector2((-48 - deltaX * 240) * Owner.direction, 24);
			if (!Collision.SolidCollision(targetPos - new Vector2(15), 30, 32))
			{
				int count = 0;
				while (!Collision.SolidCollision(targetPos - new Vector2(15), 30, 32))
				{
					count++;
					targetPos.Y += 16;
					if (count > 40)
					{
						break;
					}
				}
			}
			if (Collision.SolidCollision(targetPos - new Vector2(30), 30, 30))
			{
				int count = 0;
				while (Collision.SolidCollision(targetPos - new Vector2(30), 30, 30))
				{
					count++;
					targetPos.Y -= 16;
					if (count > 40)
					{
						break;
					}
				}
			}
			return targetPos;
		}

		// when state is fly, swirl behide player.
		if (State == 1)
		{
			float timeValue = (float)Main.time * 0.09f;
			Vector2 targetPos = Owner.Center + new Vector2(-120, -72) + new Vector2(180 * MathF.Sin(timeValue + Projectile.whoAmI), 90 * MathF.Sin(timeValue * 0.33f + Projectile.whoAmI));
			return targetPos;
		}
		return Owner.Center;
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		if (State == 0)
		{
			Vector2 targetPos = FindTargetPosWhenNoEnemies();
			Vector2 toTarget = targetPos - Projectile.velocity - Projectile.Center;
			if (toTarget.Length() > 60)
			{
				Projectile.velocity.Y -= 3f;
			}
		}
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
	}

	/// <summary>
	/// Get a unique order by Projectile.whoAmI.
	/// </summary>
	/// <returns></returns>
	public int GetOrderFromOwner()
	{
		int count = 0;
		for (int i = 0; i < Main.projectile.Length; i++)
		{
			Projectile projectile = Main.projectile[i];
			if (projectile.active && projectile.owner == Owner.whoAmI)
			{
				if (projectile == Projectile)
				{
					return count;
				}
				if (projectile.type == Type)
				{
					count++;
				}
			}
		}
		return 0;
	}

	/// <summary>
	/// Walking or flying will behaved different.
	/// </summary>
	public void FindFrame()
	{
		if (State == 0)
		{
			Projectile.frameCounter += (int)Projectile.velocity.Length() * 10;
			if (Projectile.frameCounter > 24)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame >= 3)
			{
				Projectile.frame = 0;
			}
			Point myPos = Projectile.Center.ToTileCoordinates();
			float rot = YggdrasilWorldGeneration.TerrianSurfaceAngle(myPos.X, myPos.Y, 4);
			if (rot != -1)
			{
				Projectile.rotation = rot - MathHelper.PiOver2;
			}
			else
			{
				Projectile.rotation = 0;
			}
		}
		if (State == 1)
		{
			Projectile.frameCounter += 1;
			if (Projectile.frameCounter > 1)
			{
				Projectile.frame++;
				Projectile.frameCounter = 0;
			}
			if (Projectile.frame >= 4)
			{
				Projectile.frame = 0;
			}
			Projectile.rotation = 0;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = ModAsset.DeadBeetleEgg_beetle.Value;
		Rectangle frame = new Rectangle(0, Projectile.frame * 30, 60, 30);
		if (State == 1)
		{
			texture = ModAsset.DeadBeetleEgg_beetle_fly.Value;
			frame = new Rectangle(0, ((Projectile.frame + Projectile.whoAmI) % 4) * 60, 60, 60);
		}
		Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, frame, lightColor, Projectile.rotation, frame.Size() * 0.5f, Projectile.scale, Projectile.spriteDirection == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
		return false;
	}
}