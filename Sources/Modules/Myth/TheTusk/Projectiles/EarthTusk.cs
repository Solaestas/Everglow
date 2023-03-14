using Everglow.Myth.Common;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Shaders;

namespace Everglow.Myth.TheTusk.Projectiles;

public class EarthTusk : ModProjectile
{
	public override string Texture => "Everglow/Myth/TheTusk/Projectiles/Textures/Tuskplus0";
	public override void SetDefaults()
	{
		Projectile.tileCollide = false;
		Projectile.friendly = false;
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.timeLeft = 60;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 12;
	}
	internal Vector2 SummonCenter = Vector2.Zero;
	internal int TextureType = 0;
	private void GetRotation()
	{
		Player player = Main.player[Projectile.owner];
		Vector2 CheckPoint = Projectile.Center;
		for (int y = 0; y < 60; y++)
		{
			if (Collision.SolidCollision(CheckPoint, 1, 1))
				break;
			else
			{
				CheckPoint += new Vector2(0, 10) * player.gravDir;
			}
		}
		if (!Collision.SolidCollision(CheckPoint, 1, 1))
			Projectile.Kill();

		Projectile.rotation = ToTileOutside(CheckPoint) - MathF.PI * 1.5f;
	}
	private void ToCollisionTileEdge()
	{
		int times = 0;
		Vector2 CheckCenter = Projectile.Center;
		while (!Collision.SolidCollision(CheckCenter, 1, 1))
		{
			times++;
			CheckCenter = Projectile.Center + new Vector2(times * Projectile.ai[1], 0).RotatedBy(times / 6f * Projectile.ai[1]);
			if (times > 256)
				Projectile.Kill();

		}
		Projectile.velocity *= 0;
		Projectile.Center = CheckCenter;
	}
	public override void OnSpawn(IEntitySource source)
	{
		ToCollisionTileEdge();
		GetRotation();
		Projectile.position -= new Vector2(0, -80).RotatedBy(Projectile.rotation) * Projectile.scale;
		SummonCenter = Projectile.Center;
		TextureType = Main.rand.Next(6);
		Projectile.friendly = true;
		Collision.HitTiles(Projectile.Center + new Vector2(0, -80).RotatedBy(Projectile.rotation) * Projectile.scale - new Vector2(8), new Vector2(0, -0.2f).RotatedBy(Projectile.rotation), 16, 16);
	}
	private float ToTileOutside(Vector2 Point)
	{
		Vector2 CheckPoint = Point;
		Vector2 TotalVector = Vector2.Zero;//合向量
		int TCount = 0;
		for (int a = 0; a < 12; a++)
		{
			Vector2 v0 = new Vector2(10, 0).RotatedBy(a / 6d * Math.PI + Main.rand.NextFloat(0.01f));
			if (Collision.SolidCollision(CheckPoint + v0, 1, 1))
			{
				TotalVector -= v0;
				TCount++;
			}
			else
			{
				TotalVector += v0;
			}
		}
		for (int a = 0; a < 24; a++)
		{
			Vector2 v0 = new Vector2(20, 0).RotatedBy(a / 12d * Math.PI + Main.rand.NextFloat(0.01f));
			if (Collision.SolidCollision(CheckPoint + v0, 1, 1))
			{
				TotalVector -= v0 * 0.5f;
				TCount++;
			}
			else
			{
				TotalVector += v0 * 0.5f;
			}
		}
		if (TotalVector == Vector2.Zero || TCount > 35)
			return 0;
		float Angle = (float)Math.Atan2(TotalVector.Y, TotalVector.X);
		return Angle;
	}
	private void Reproduct()
	{
		if (Projectile.ai[0] > 0)
		{
			Vector2 Center = SummonCenter + new Vector2(0, -80).RotatedBy(Projectile.rotation) * Projectile.scale;
			float newRotation;
			Vector2 abstractNextPoint = Center + new Vector2(8 * Projectile.ai[1], 0).RotatedBy(Projectile.rotation);
			for (int f = 0; f < 12; f++)
			{
				newRotation = ToTileOutside(abstractNextPoint) + MathF.PI * 1.5f;
				abstractNextPoint += new Vector2(8 * Projectile.ai[1], 0).RotatedBy(newRotation);
			}
			Vector2 SummonPoint;
			if (!Collision.SolidCollision(abstractNextPoint, 1, 1))
			{
				Vector2 SummonNext = abstractNextPoint;
				int times = 0;
				while (!Collision.SolidCollision(SummonNext, 1, 1))
				{
					times++;
					SummonNext = abstractNextPoint + new Vector2(times * Projectile.ai[1], 0).RotatedBy(times / 6f * Projectile.ai[1]);
					if (times > 256f)
					{
						SummonNext = Center;
						Projectile.ai[1] *= -1;
						break;
					}
				}
				SummonPoint = SummonNext;
			}
			else
			{
				Vector2 SummonNext = abstractNextPoint;
				int times = 0;
				while (Collision.SolidCollision(SummonNext, 1, 1))
				{
					times++;
					SummonNext = abstractNextPoint + new Vector2(times * Projectile.ai[1], 0).RotatedBy(times / 6f * Projectile.ai[1]);
					if (times > 256f)
					{
						SummonNext = Center;
						Projectile.ai[1] *= -1;
						break;
					}
					else
					{
						while ((SummonNext - Center).Length() < 25f)
						{
							times++;
							SummonNext = abstractNextPoint + new Vector2(times * Projectile.ai[1], 0).RotatedBy(times / 6f * Projectile.ai[1]);
						}
					}
				}
				if (times <= 256f)
					SummonNext = abstractNextPoint + new Vector2(times - 1, 0).RotatedBy((times - 1) / 6f);
				SummonPoint = SummonNext;
			}

			Projectile.NewProjectile(Projectile.GetSource_FromAI(), SummonPoint, Vector2.Zero, ModContent.ProjectileType<EarthTusk>(), Projectile.damage, Projectile.knockBack * 0.8f, Projectile.owner, Projectile.ai[0] - 1, Projectile.ai[1]);
		}
	}
	public override void AI()
	{
		if (Projectile.timeLeft > 50)
			Projectile.Center = (SummonCenter + new Vector2(0, -80).RotatedBy(Projectile.rotation) * Projectile.scale) * 0.24f + Projectile.Center * 0.76f;

		if (Projectile.timeLeft < 30)
			Projectile.Center = SummonCenter * 0.08f + Projectile.Center * 0.92f;
		if (Projectile.timeLeft == 55 && Projectile.ai[0] > 0)
			Reproduct();
		Projectile.hide = true;
		ProduceWaterRipples(new Vector2(0, -80).RotatedBy(Projectile.rotation) * Projectile.scale);
	}
	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCsAndTiles.Add(index);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		Vector2 TenNormalize = new Vector2(10, 0).RotatedBy(Projectile.rotation) * Projectile.scale;
		Vector2 HitLength = new Vector2(0, -80).RotatedBy(Projectile.rotation) * Projectile.scale;
		Vector2 SummonCenterII = HitLength * 0.9f + SummonCenter;
		Vector2 ProjCenterII = HitLength + Projectile.Center;
		Color StartColor = Lighting.GetColor((int)(SummonCenterII.X / 16f), (int)(SummonCenterII.Y / 16f));
		Color EndColor = Lighting.GetColor((int)(ProjCenterII.X / 16f), (int)(ProjCenterII.Y / 16f));
		float value = (Projectile.Center - SummonCenter).Length() / 80f;

		var bars = new List<Vertex2D>
		{
			new Vertex2D(SummonCenterII + TenNormalize - Main.screenPosition, StartColor, new Vector3(0, value, 0)),
			new Vertex2D(SummonCenterII - TenNormalize - Main.screenPosition, StartColor, new Vector3(1, value, 0)),

			new Vertex2D(ProjCenterII + TenNormalize - Main.screenPosition, EndColor, new Vector3(0, 0, 0)),
			new Vertex2D(ProjCenterII - TenNormalize - Main.screenPosition, EndColor, new Vector3(1, 0, 0))
		};

		Main.graphics.GraphicsDevice.Textures[0] = MythContent.QuickTexture("TheTusk/Projectiles/Textures/Tuskplus" + TextureType.ToString());
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);


		SummonCenterII = HitLength * 0.95f + SummonCenter;
		Tile tilebelow = Main.tile[(int)(SummonCenterII.X / 16f), (int)(SummonCenterII.Y / 16f)];
		Texture2D TileBelowTexture = TextureAssets.Tile[tilebelow.TileType].Value;
		float leftX = tilebelow.TileFrameX / (float)TileBelowTexture.Width;
		float rightX = (tilebelow.TileFrameX + 16f) / TileBelowTexture.Width;
		float topY = tilebelow.TileFrameY / (float)TileBelowTexture.Height;
		float bottomY = (tilebelow.TileFrameY + 16f) / TileBelowTexture.Height;
		Vector2 fragilesLeft = new Vector2(-20, 0).RotatedBy(Projectile.rotation);
		Vector2 fragilesRight = new Vector2(20, 0).RotatedBy(Projectile.rotation);

		float rotateValue = (Projectile.Center - SummonCenter).Length() / 110f;
		Vector2 fragilesLeftToCenter = fragilesLeft + new Vector2(16, 0).RotatedBy(-rotateValue + Projectile.rotation);
		Vector2 fragilesRightToCenter = fragilesRight + new Vector2(-16, 0).RotatedBy(rotateValue + Projectile.rotation);
		Vector2 fragilesLeftToCenterBottom = fragilesLeft + new Vector2(16, 16).RotatedBy(-rotateValue + Projectile.rotation);
		Vector2 fragilesRightToCenterBottom = fragilesRight + new Vector2(-16, 16).RotatedBy(rotateValue + Projectile.rotation);

		float squzze = 1;
		while (!Collision.SolidCollision(SummonCenterII + fragilesLeft, 0, 0))
		{
			squzze -= 0.1f;
			fragilesLeft = new Vector2(-20, 0).RotatedBy(Projectile.rotation) * squzze;
			fragilesLeftToCenter = fragilesLeft + new Vector2(16, 0).RotatedBy(-rotateValue + Projectile.rotation) * squzze;
			fragilesLeftToCenterBottom = fragilesLeft + new Vector2(16, 16).RotatedBy(-rotateValue + Projectile.rotation) * squzze;
			if (squzze < 0.1f)
				break;
		}
		while (!Collision.SolidCollision(SummonCenterII + fragilesRight, 0, 0))
		{
			squzze -= 0.1f;
			fragilesRight = new Vector2(20, 0).RotatedBy(Projectile.rotation) * squzze;
			fragilesRightToCenter = fragilesRight + new Vector2(-16, 0).RotatedBy(rotateValue + Projectile.rotation) * squzze;
			fragilesRightToCenterBottom = fragilesRight + new Vector2(-16, 16).RotatedBy(rotateValue + Projectile.rotation) * squzze;
			if (squzze < 0.1f)
				break;
		}
		Vector2 largeFragilesTop = new Vector2(0, -rotateValue * 12 - 4).RotatedBy(Projectile.rotation);

		var Fragiles = new List<Vertex2D>
		{
			new Vertex2D(SummonCenterII + fragilesLeft - Main.screenPosition, EndColor, new Vector3(leftX, topY, 0)),
			new Vertex2D(SummonCenterII + fragilesLeftToCenter - Main.screenPosition, EndColor, new Vector3(rightX, topY, 0)),
			new Vertex2D(SummonCenterII + fragilesLeftToCenterBottom - Main.screenPosition, EndColor, new Vector3(rightX, bottomY, 0)),


			new Vertex2D(SummonCenterII + fragilesRightToCenter - Main.screenPosition, EndColor, new Vector3(rightX, topY, 0)),
			new Vertex2D(SummonCenterII + fragilesRight - Main.screenPosition, EndColor, new Vector3(leftX, topY, 0)),
			new Vertex2D(SummonCenterII + fragilesRightToCenterBottom - Main.screenPosition, EndColor, new Vector3(rightX, bottomY, 0))
		};

		Main.graphics.GraphicsDevice.Textures[0] = TileBelowTexture;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, Fragiles.ToArray(), 0, Fragiles.Count / 3);

		return false;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		float point = 0;
		if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), SummonCenter, Projectile.Center, 20, ref point))
			return true;
		return false;
	}
	private void ProduceWaterRipples(Vector2 beamDims)
	{
		var shaderData = (WaterShaderData)Terraria.Graphics.Effects.Filters.Scene["WaterDistortion"].GetShader();
		float waveSine = 1f * (float)Math.Sin(Main.GlobalTimeWrappedHourly * 20f);
		Vector2 HitRange = new Vector2(0, -80).RotatedBy(Projectile.rotation) * Projectile.scale;
		Vector2 ripplePos = Projectile.Center + HitRange;
		Color waveData = new Color(0.5f, 0.1f * Math.Sign(waveSine) + 0.5f, 0f, 1f) * Math.Abs(waveSine);
		shaderData.QueueRipple(ripplePos, waveData, beamDims, RippleShape.Square, Projectile.rotation + MathHelper.Pi / 2f);
	}
}
