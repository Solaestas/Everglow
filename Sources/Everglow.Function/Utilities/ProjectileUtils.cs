namespace Everglow.Commons.Utilities;

public static class ProjectileUtils
{
	public static void TrackOldValue<T>(T[] array, T curValue)
	{
		for (int i = array.Length - 1; i > 0; i--)
		{
			array[i] = array[i - 1];
		}
		array[0] = curValue;
	}

	public static bool IsSafeInTheWorld(Projectile projectile)
	{
		return IsSafeInTheWorld(projectile.TopLeft) && IsSafeInTheWorld(projectile.TopRight) && IsSafeInTheWorld(projectile.BottomLeft) && IsSafeInTheWorld(projectile.BottomRight);
	}

	public static bool IsSafeInTheWorld(Vector2 position)
	{
		if (position.X <= 320 || position.X >= Main.maxTilesX * 16 - 320)
		{
			return false;
		}
		if (position.Y <= 320 || position.Y >= Main.maxTilesY * 16 - 320)
		{
			return false;
		}
		return true;
	}

	/// <summary>
	/// Get a terrain normal toward the air.
	/// </summary>
	/// <param name="position"></param>
	/// <param name="maxRange"></param>
	/// <returns></returns>
	public static Vector2 GetTopographicGradient(Vector2 position, int maxRange)
	{
		if(maxRange <= 0)
		{
			maxRange = 1;
		}
		Vector2 normal = Vector2.zeroVector;
		for (int i = -maxRange; i <= maxRange; i++)
		{
			for (int j = -maxRange; j <= maxRange; j++)
			{
				float length = new Vector2(i, j).Length();
				if (length <= maxRange && length > 0)
				{
					if (Collision.SolidCollision(position + new Vector2(i, j) * 16, 0, 0))
					{
						normal -= Utils.SafeNormalize(new Vector2(i, j), Vector2.zeroVector) / (length + 5);
					}
					else
					{
						normal += Utils.SafeNormalize(new Vector2(i, j), Vector2.zeroVector) / (length + 5);
					}
				}
			}
		}
		if(normal.Length() < 0.01)
		{
			return Vector2.zeroVector;
		}
		normal = Utils.SafeNormalize(normal, Vector2.zeroVector);
		return normal;
	}

	public abstract class StickNPCProjectile : ModProjectile
	{
		/// <summary>
		/// 目标敌人,目标敌人死亡的时候会改成-2
		/// </summary>
		public int StuckNPC = -1;

		/// <summary>
		/// 相对角度
		/// </summary>
		public float RelativeAngle = 0;

		/// <summary>
		/// 击中时的角度
		/// </summary>
		public float HitTargetAngle = 0;

		/// <summary>
		/// 相对位置
		/// </summary>
		public Vector2 RelativePos = Vector2.zeroVector;

		/// <summary>
		/// 击中时怪的大小
		/// </summary>
		public float HitTargetScale = 1f;

		public override void AI()
		{
			UpdateSticking();
		}

		public virtual void UpdateSticking()
		{
			if (StuckNPC >= 0 && StuckNPC < Main.maxNPCs)
			{
				NPC target = Main.npc[StuckNPC];
				if (target == null || !target.active)
				{
					StuckNPC = -2;
					return;
				}
				else
				{
					Projectile.rotation = target.rotation + RelativeAngle;
					float scaleRate = target.scale / HitTargetScale;
					Projectile.Center = target.Center + RelativePos.RotatedBy(target.rotation + RelativeAngle - HitTargetAngle) * scaleRate;
					Projectile.velocity = target.velocity;
				}
			}
		}

		public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
		{
			RelativeAngle = Projectile.rotation - target.rotation;
			HitTargetAngle = Projectile.rotation;
			RelativePos = Projectile.Center - target.Center;
			HitTargetScale = target.scale;
			StuckNPC = target.whoAmI;
		}
	}
}