using Terraria.DataStructures;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.TileEffect;

public class UnderwaterLightningMechanism_Lightning : ModProjectile
{
	public override string LocalizationCategory => LocalizationUtils.Categories.RangedProjectiles;

	public Vector2 StartPos;

	public Vector2 EndPos;

	public List<Vector4> JointsAndVels = new List<Vector4>();

	public List<Vector2> LightningTrail = new List<Vector2>();

	public bool Initialized = false;

	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.friendly = true;
		Projectile.hostile = true;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 120;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
	}

	public List<Point> ContinueAirTilesArea = new List<Point>();

	public Vector2 GetMarginalForce(Vector2 pos)
	{
		Vector2 marginalRejectionalForce = Vector2.zeroVector;
		float forceSize = 0.3f;
		Main.NewText(pos.ToTileCoordinates());
		for (int i = 0; i < 8; i++)
		{
			Vector2 v0 = new Vector2(0, -16).RotatedBy(i / 8f * MathHelper.TwoPi);
			if (ContinueAirTilesArea.Contains((v0 + pos).ToTileCoordinates()))
			{
				marginalRejectionalForce += v0 * forceSize;
			}
			else
			{
				marginalRejectionalForce -= v0 * forceSize;
			}
		}
		forceSize = 0.01f;
		for (int i = 0; i < 16; i++)
		{
			Vector2 v0 = new Vector2(0, -48).RotatedBy(i / 16f * MathHelper.TwoPi);
			if (ContinueAirTilesArea.Contains((v0 + pos).ToTileCoordinates()))
			{
				marginalRejectionalForce += v0 * forceSize;
			}
			else
			{
				marginalRejectionalForce -= v0 * forceSize;
			}
		}
		return marginalRejectionalForce;
	}

	public void BFSContinueEmpty()
	{
		int maxContinueCount = 4096;
		(int, int)[] directions =
		{
			(0, 1),
			(1, 0),
			(0, -1),
			(-1, 0),
		};
		Queue<Point> queueCheckedStart = new Queue<Point>();
		Queue<Point> queueCheckedEnd = new Queue<Point>();

		// 将起始点加入队列
		queueCheckedStart.Enqueue(StartPos.ToTileCoordinates());
		queueCheckedEnd.Enqueue(EndPos.ToTileCoordinates());
		List<Point> visitedStart = new List<Point>();
		List<Point> visitedEnd = new List<Point>();

		List<Point> visitedStartDir = new List<Point>();
		List<Point> visitedEndDir = new List<Point>();

		Point intersectPoint = default(Point);
		while (queueCheckedStart.Count > 0)
		{
			var tilePosStart = queueCheckedStart.Dequeue();
			foreach (var (dx, dy) in directions)
			{
				int checkX = tilePosStart.X + dx;
				int checkY = tilePosStart.Y + dy;
				Point point = new Point(checkX, checkY);

				// 检查边界和障碍物
				if (checkX >= 20 && checkX < Main.maxTilesX - 20 && checkY >= 20 && checkY < Main.maxTilesY - 20 &&
					!Collision.IsWorldPointSolid(point.ToWorldCoordinates()) && !visitedStart.Contains(point))
				{
					queueCheckedStart.Enqueue(point);
					visitedStart.Add(point);
					visitedStartDir.Add(new Point(-dx, -dy));
				}
			}
			if (queueCheckedStart.Count > maxContinueCount || visitedStart.Count > maxContinueCount)
			{
				break;
			}
			var tilePosEnd = queueCheckedEnd.Dequeue();
			foreach (var (dx, dy) in directions)
			{
				int checkX = tilePosEnd.X + dx;
				int checkY = tilePosEnd.Y + dy;
				Point point = new Point(checkX, checkY);

				// 检查边界和障碍物
				if (checkX >= 20 && checkX < Main.maxTilesX - 20 && checkY >= 20 && checkY < Main.maxTilesY - 20 &&
					!Collision.IsWorldPointSolid(point.ToWorldCoordinates()) && !visitedEnd.Contains(point))
				{
					queueCheckedEnd.Enqueue(point);
					visitedEnd.Add(point);
					visitedEndDir.Add(new Point(-dx, -dy));
				}
			}
			if (queueCheckedEnd.Count > maxContinueCount || visitedEnd.Count > maxContinueCount)
			{
				break;
			}
			if (visitedStart.Intersect(visitedEnd).ToList().Count > 0)
			{
				intersectPoint = visitedStart.Intersect(visitedEnd).ToArray()[0];
				break;
			}
		}
		ContinueAirTilesArea.AddRange(visitedEnd);
		ContinueAirTilesArea.AddRange(visitedStart);
		List<Point> trailToStart = new List<Point>();
		int intersectIndex = visitedStart.IndexOf(intersectPoint);
		if (intersectIndex > -1)
		{
			Point checkPoint = intersectPoint;
			for (int i = 0; i < 999; i++)
			{
				int checkIndex = visitedStart.IndexOf(checkPoint);
				trailToStart.Add(checkPoint);
				if (checkIndex <= -1)
				{
					break;
				}
				checkPoint += visitedStartDir[checkIndex];
				if (checkPoint == StartPos.ToTileCoordinates())
				{
					break;
				}
			}
		}

		trailToStart.Reverse();
		List<Point> trailToEnd = new List<Point>();
		intersectIndex = visitedEnd.IndexOf(intersectPoint);
		if (intersectIndex > -1)
		{
			Point checkPoint = intersectPoint;
			for (int i = 0; i < 999; i++)
			{
				int checkIndex = visitedEnd.IndexOf(checkPoint);
				if (checkIndex <= -1)
				{
					break;
				}
				checkPoint += visitedEndDir[checkIndex];
				trailToEnd.Add(checkPoint);
				if (checkPoint == EndPos.ToTileCoordinates())
				{
					break;
				}
			}
		}

		List<Point> lightningTrail = [.. trailToStart, .. trailToEnd];

		// The shortest way to connect StartPos and EndPos.
		LightningTrail = new List<Vector2>();
		for (int i = 0; i < lightningTrail.Count; i++)
		{
			if (i == 0 || i == lightningTrail.Count - 1)
			{
				LightningTrail.Add(lightningTrail[i].ToWorldCoordinates());
			}
			else
			{
				Vector2 back = lightningTrail[i].ToWorldCoordinates() - lightningTrail[i - 1].ToWorldCoordinates();
				Vector2 front = lightningTrail[i + 1].ToWorldCoordinates() - lightningTrail[i].ToWorldCoordinates();
				if ((front - back).Length() > 1)
				{
					LightningTrail.Add(lightningTrail[i].ToWorldCoordinates());
				}
			}
		}
		List<Vector2> LightningTrailOblique = new List<Vector2>();
		int lastJoint = 0;
		if (LightningTrail.Count > 0)
		{
			for (int i = 0; i < LightningTrail.Count; i++)
			{
				if (i == 0)
				{
					LightningTrailOblique.Add(LightningTrail[i]);
				}
				if (i > 0 && i <= LightningTrail.Count - 1)
				{
					if (!LineInSafeArea(LightningTrail[lastJoint], LightningTrail[i]))
					{
						LightningTrailOblique.Add(LightningTrail[i - 1]);
						lastJoint = i - 1;
					}
				}
				if (i == LightningTrail.Count - 1)
				{
					LightningTrailOblique.Add(LightningTrail[i]);
				}
			}
		}
		LightningTrail = GraphicsUtils.CatmullRom(LightningTrailOblique.ToList());
		for (int i = 0; i < LightningTrail.Count; i++)
		{
			LightningTrail[i] += GetMarginalForce(LightningTrail[i]);
		}
	}

	public bool LineInSafeArea(Vector2 pos0, Vector2 pos1)
	{
		Vector2 route = pos1 - pos0;
		Vector2 step = route.NormalizeSafe() * 16f;
		int times = (int)(route.Length() / 16f);
		Vector2 checkPoint = pos0;
		for (int k = 0; k < times; k++)
		{
			if (!ContinueAirTilesArea.Contains(checkPoint.ToTileCoordinates()))
			{
				return false;
			}
			checkPoint += step;
		}
		if (!ContinueAirTilesArea.Contains(pos1.ToTileCoordinates()))
		{
			return false;
		}
		return true;
	}

	public override void AI()
	{
		if (!Initialized)
		{
			if (Collision.IsWorldPointSolid(StartPos) || Collision.IsWorldPointSolid(EndPos))
			{
				Projectile.Kill();
				return;
			}
			BFSContinueEmpty();
			Initialized = true;
		}
		else if (LightningTrail.Count > 0)
		{
			for (int i = 1; i < LightningTrail.Count - 1; i++)
			{
				Vector2 dirWidth = (LightningTrail[i] - LightningTrail[i - 1]).NormalizeSafe().RotatedBy(MathHelper.PiOver2);
				LightningTrail[i] += dirWidth * GetRandomValueMove(i);
			}
		}
	}

	public float GetRandomValueMove(float index)
	{
		float value = 0;
		for (int i = 0; i < 8; i++)
		{
			value += MathF.Sin((i + index + (float)Main.time / 36f) * MathF.Pow(2, i)) / (2 ^ i);
		}
		return value;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox) => base.Colliding(projHitbox, targetHitbox);

	public override bool PreDraw(ref Color lightColor)
	{
		if (LightningTrail.Count <= 1)
		{
			return false;
		}
		Texture2D tex = Commons.ModAsset.Trail_10.Value;
		Color drawColor = new Color(1f, 1f, 1f, 0);
		for (int j = 0; j < 3; j++)
		{
			List<Vertex2D> bars = new List<Vertex2D>();
			for (int i = 0; i < LightningTrail.Count; i++)
			{
				Vector2 dir = new Vector2(0, -10).RotatedBy(j / 3f * MathHelper.TwoPi);
				bars.Add(LightningTrail[i] + dir - Main.screenPosition, drawColor, new Vector3(i / 15f, 0, 0));
				bars.Add(LightningTrail[i] - dir - Main.screenPosition, drawColor, new Vector3(i / 15f, 1, 0));
			}
			if (bars.Count > 3)
			{
				Main.graphics.graphicsDevice.Textures[0] = tex;
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
		}

		return false;
	}
}