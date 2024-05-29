using Everglow.Commons.DataStructures;
using Everglow.Commons.VFX.CommonVFXDusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.Myth.TheTusk.Projectiles;

// ai0和ai1用来存左,右两侧颌骨的张开度
public class Living_Jawbone_Huge_ground : ModProjectile
{
	public List<Vector2> RotationBaseUp = new List<Vector2>();
	public List<Vector2> RotationBaseDown = new List<Vector2>();

	public override string Texture => ModAsset.Living_Jawbone_Huge_Mod;

	public override void SetDefaults()
	{
		Projectile.width = 100;
		Projectile.height = 100;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 180;
		Projectile.alpha = 255;
		Projectile.penetrate = -1;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
		Vector2 checkDir = GetTopographicGradient(Projectile.Center);
		for (int y = 0; y < 100; y++)
		{
			if (Collision.SolidCollision(Projectile.Center, 0, 0))
			{
				break;
			}
			Projectile.position -= checkDir * 10;
			if (y == 99)
			{
				Projectile.active = false;
			}
		}
		Projectile.rotation = GetTopographicGradient(Projectile.Center).ToRotation();
		Projectile.position -= Projectile.rotation.ToRotationVector2() * 20;
		for (int t = 0; t < 100; t++)
		{
			Vector2 check_up = Projectile.Center + new Vector2(450, 0).RotatedBy(Projectile.rotation - Projectile.ai[0]);
			if (GetToCenterCollision20TimesIn450Range(check_up) > 18)
			{
				break;
			}
			else
			{
				Projectile.ai[0] += 0.05f;
			}
			if (t == 99)
			{
				Projectile.active = false;
			}
		}
		for (int t = 0; t < 100; t++)
		{
			Vector2 check_down = Projectile.Center + new Vector2(450, 0).RotatedBy(Projectile.rotation + Projectile.ai[1]);
			if (GetToCenterCollision20TimesIn450Range(check_down) > 18)
			{
				break;
			}
			else
			{
				Projectile.ai[1] += 0.05f;
			}
			if (t == 99)
			{
				Projectile.active = false;
			}
		}
		for (int i = 0; i < 20; i++)
		{
			RotationBaseUp.Add(new Vector2(18 * i, 0).RotatedBy(Projectile.rotation - Projectile.ai[0]) + Projectile.Center);
			RotationBaseDown.Add(new Vector2(18 * i, 0).RotatedBy(Projectile.rotation + Projectile.ai[1]) + Projectile.Center);
		}
		Projectile.damage = 100;
	}

	/// <summary>
	/// 查找沿Proj中心点到此点方向上640距离的20次碰撞次数
	/// </summary>
	/// <param name="position"></param>
	/// <returns></returns>
	public int GetToCenterCollision20TimesIn450Range(Vector2 position)
	{
		int times = 0;
		if (position == Projectile.Center)
		{
			return times;
		}
		Vector2 normal = Vector2.Normalize(position - Projectile.Center);
		for (int i = 0; i < 20; i++)
		{
			Vector2 checkPoint = normal * i * 32 + Projectile.Center;
			if (Collision.SolidCollision(checkPoint, 0, 0))
			{
				times++;
			}
		}
		return times;
	}

	public Vector2 GetTopographicGradient(Vector2 position)
	{
		Vector2 normal = Vector2.zeroVector;
		for (int i = -20; i < 21; i++)
		{
			for (int j = -20; j < 21; j++)
			{
				float length = new Vector2(i, j).Length();
				if (length is < 20 and > 0)
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
		normal = Utils.SafeNormalize(normal, Vector2.zeroVector);
		return normal;
	}

	public void UpdateRotationBase()
	{
		List<Vector2> newRotationBaseUp = new List<Vector2>();
		for (int i = 0; i < 20; i++)
		{
			if (Collision.SolidCollision(RotationBaseUp[i], 0, 0))
			{
				Vector2 boneUp = new Vector2(360, 0).RotatedBy(Projectile.rotation - Projectile.ai[0]) * i / 20f + Projectile.Center;
				if (Collision.SolidCollision(boneUp, 0, 0))
				{
					newRotationBaseUp.Add(boneUp);
				}
				else
				{
					newRotationBaseUp.Add(RotationBaseUp[i]);
				}
			}
			else
			{
				newRotationBaseUp.Add(RotationBaseUp[i]);
			}
		}
		RotationBaseUp = newRotationBaseUp;

		List<Vector2> newRotationBaseDown = new List<Vector2>();
		for (int i = 0; i < 20; i++)
		{
			if (Collision.SolidCollision(RotationBaseDown[i], 0, 0))
			{
				Vector2 boneDown = new Vector2(360, 0).RotatedBy(Projectile.rotation + Projectile.ai[1]) * i / 20f + Projectile.Center;
				if (Collision.SolidCollision(boneDown, 0, 0))
				{
					newRotationBaseDown.Add(boneDown);
				}
				else
				{
					newRotationBaseDown.Add(RotationBaseDown[i]);
				}
			}
			else
			{
				newRotationBaseDown.Add(RotationBaseDown[i]);
			}
		}
		RotationBaseDown = newRotationBaseDown;
	}

	public override void AI()
	{
		Projectile.velocity *= 0;
		if (Projectile.alpha > 0)
		{
			Projectile.alpha -= 8;
			Projectile.timeLeft = 180;
		}
		else
		{
			Projectile.alpha = 0;
		}
		if (Projectile.timeLeft is < 180 and > 160)
		{
			if (Projectile.ai[0] > 1.4f)
			{
				Projectile.ai[0] = (float)Utils.Lerp(Projectile.ai[0], 1.4f, 0.3f);
			}
			if (Projectile.ai[1] > 1.4f)
			{
				Projectile.ai[1] = (float)Utils.Lerp(Projectile.ai[1], 1.4f, 0.3f);
			}

			UpdateRotationBase();
		}
		if (Projectile.timeLeft is < 160 and > 130)
		{
			if (Projectile.ai[0] > 1.4f)
			{
				Projectile.ai[0] += 0.01f;
			}
			if (Projectile.ai[1] > 1.4f)
			{
				Projectile.ai[1] += 0.01f;
			}
		}
		if (Projectile.timeLeft == 170)
		{
			SoundEngine.PlaySound(new SoundStyle(ModAsset.TuskJawBoneBite_Mod).WithVolumeScale(Main.rand.NextFloat(0.8f, 1f)), Projectile.Center);
		}
		if (Projectile.timeLeft == 121)
		{
			for (int i = 0; i < 20; i++)
			{
				Vector2 pos0 = new Vector2(MathF.Sqrt(Main.rand.NextFloat(1f)) * 430 * Projectile.scale, 0).RotatedBy(Projectile.rotation - Projectile.ai[0] / 20f * i);
				var blood = new BloodDrop
				{
					velocity = pos0.RotatedBy(MathHelper.PiOver2) * 0.1f,
					Active = true,
					Visible = true,
					position = pos0 + Projectile.Center,
					maxTime = Main.rand.Next(54, 74),
					scale = Main.rand.NextFloat(6f, 25f),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
				};
				Ins.VFXManager.Add(blood);
				var blood2 = new BloodSplash
				{
					velocity = pos0.RotatedBy(MathHelper.PiOver2) * 0.1f,
					Active = true,
					Visible = true,
					position = pos0 + Projectile.Center,
					maxTime = Main.rand.Next(54, 74),
					scale = Main.rand.NextFloat(6f, 18f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0, Main.rand.NextFloat(20.0f, 40.0f) },
				};
				Ins.VFXManager.Add(blood2);
			}
			for (int i = 0; i < 20; i++)
			{
				Vector2 pos1 = new Vector2(MathF.Sqrt(Main.rand.NextFloat(1f)) * 430 * Projectile.scale, 0).RotatedBy(Projectile.rotation + Projectile.ai[1] / 20f * i);
				var blood = new BloodDrop
				{
					velocity = pos1.RotatedBy(-MathHelper.PiOver2) * 0.1f,
					Active = true,
					Visible = true,
					position = pos1 + Projectile.Center,
					maxTime = Main.rand.Next(54, 74),
					scale = Main.rand.NextFloat(6f, 25f),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { 0f, Main.rand.NextFloat(0.0f, 4.93f) },
				};
				Ins.VFXManager.Add(blood);
				var blood2 = new BloodSplash
				{
					velocity = pos1.RotatedBy(-MathHelper.PiOver2) * 0.1f,
					Active = true,
					Visible = true,
					position = pos1 + Projectile.Center,
					maxTime = Main.rand.Next(54, 74),
					scale = Main.rand.NextFloat(6f, 18f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0, Main.rand.NextFloat(20.0f, 40.0f) },
				};
				Ins.VFXManager.Add(blood2);
			}
		}
		if (Projectile.timeLeft == 120)
		{
			Projectile.ai[0] = 0;
			Projectile.ai[1] = 0;
			ShakerManager.AddShaker(Projectile.Center, Projectile.rotation.ToRotationVector2(), 200, 60, 120);
			Collision.HitTiles(Projectile.position - new Vector2(300), Projectile.rotation.ToRotationVector2() * 3, Projectile.width + 600, Projectile.height + 600);
			Vector2 vel = Projectile.rotation.ToRotationVector2() * 45f;
			foreach (Dust dust in Main.dust)
			{
				if (dust.active)
				{
					float distance = (dust.position - Projectile.Center).Length();
					if (distance < 100)
					{
						dust.velocity += vel * (100 - distance) / 100f;
					}
				}
			}
		}
		if (Projectile.timeLeft < 90)
		{
			Projectile.ai[0] = (float)Utils.Lerp(Projectile.ai[0], 0.4f, 0.05f);
			Projectile.ai[1] = (float)Utils.Lerp(Projectile.ai[0], 0.4f, 0.05f);
		}
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		float k = 0;
		if (Projectile.timeLeft == 121)
		{
			for (int i = 0; i < 20; i++)
			{
				bool b2 = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(430 * Projectile.scale, 0).RotatedBy(Projectile.rotation - Projectile.ai[0] / 20f * i), 60, ref k);
				bool b3 = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(430 * Projectile.scale, 0).RotatedBy(Projectile.rotation + Projectile.ai[1] / 20f * i), 60, ref k);
				if (b2 || b3)
				{
					return true;
				}
			}
		}
		bool b0 = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(430 * Projectile.scale, 0).RotatedBy(Projectile.rotation - Projectile.ai[0]), 60, ref k);
		bool b1 = Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, Projectile.Center + new Vector2(430 * Projectile.scale, 0).RotatedBy(Projectile.rotation + Projectile.ai[1]), 60, ref k);
		return b0 || b1;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		Projectile.hide = true;
		behindNPCsAndTiles.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		BeginDissolveShader(Math.Clamp(Projectile.timeLeft / 50f - 0.2f, -0.2f, 1), new Vector4(0.5f, 0f, 0f, 0.7f));
		Texture stickyBlood = ModAsset.Living_Jawbone_BloodLines.Value;

		List<Vertex2D> bars = new List<Vertex2D>();
		Vector2 bonePlaceUp = new Vector2(360, 0).RotatedBy(Projectile.rotation - Projectile.ai[0] - 0.14f);
		for (int i = 0; i < 20; i++)
		{
			AddVertex(bars, RotationBaseUp[i], new Vector3(i / 10f, 1, 0));
			AddVertex(bars, bonePlaceUp * i / 20f + Projectile.Center, new Vector3(i / 10f, 0, 0));
		}
		Main.graphics.graphicsDevice.Textures[0] = stickyBlood;
		Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		bars = new List<Vertex2D>();
		Vector2 bonePlaceDown = new Vector2(360, 0).RotatedBy(Projectile.rotation + Projectile.ai[1] + 0.14f);
		for (int i = 0; i < 20; i++)
		{
			AddVertex(bars, RotationBaseDown[i], new Vector3(i / 10f, 1, 0));
			AddVertex(bars, bonePlaceDown * i / 20f + Projectile.Center, new Vector3(i / 10f, 0, 0));
		}
		Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		BeginDissolveShader(Math.Clamp(Projectile.timeLeft / 20f - 0.2f, -0.2f, 1), new Vector4(0.5f, 0.4f, 0.4f, 0.7f));

		Texture2D upJaw = ModAsset.Living_Jawbone_Huge_up.Value;
		bonePlaceUp = new Vector2(upJaw.Width, 0).RotatedBy(Projectile.rotation - Projectile.ai[0]);
		bars = new List<Vertex2D>();
		for (int i = 0; i < 11; i++)
		{
			Vector2 width = Vector2.Normalize(bonePlaceUp.RotatedBy(MathHelper.PiOver2)) * upJaw.Height * 0.5f;
			AddVertex(bars, bonePlaceUp * i / 10f + width + Projectile.Center, new Vector3(i / 10f, 1, 0));
			AddVertex(bars, bonePlaceUp * i / 10f - width + Projectile.Center, new Vector3(i / 10f, 0, 0));
		}
		Main.graphics.graphicsDevice.Textures[0] = upJaw;
		Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		Texture2D downJaw = ModAsset.Living_Jawbone_Huge_down.Value;
		bonePlaceDown = new Vector2(downJaw.Width, 0).RotatedBy(Projectile.rotation + Projectile.ai[1]);
		bars = new List<Vertex2D>();
		for (int i = 0; i < 11; i++)
		{
			Vector2 width = Vector2.Normalize(bonePlaceDown.RotatedBy(MathHelper.PiOver2)) * downJaw.Height * 0.5f;
			AddVertex(bars, bonePlaceDown * i / 10f + width + Projectile.Center, new Vector3(i / 10f, 1, 0));
			AddVertex(bars, bonePlaceDown * i / 10f - width + Projectile.Center, new Vector3(i / 10f, 0, 0));
		}
		Main.graphics.graphicsDevice.Textures[0] = downJaw;
		Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
		return false;
	}

	public void AddVertex(List<Vertex2D> bars, Vector2 pos, Vector3 coords)
	{
		bars.Add(pos, Lighting.GetColor(pos.ToTileCoordinates()), coords);
	}

	public void BeginDissolveShader(float dissolveValue, Vector4 dissolveColor)
	{
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null);
		Effect dissolve = ModAsset.Living_Jawbone_Huge_dissolve.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		float dissolveDuration = dissolveValue;
		dissolve.Parameters["uTransform"].SetValue(model * projection);
		dissolve.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_spiderNet.Value);
		dissolve.Parameters["duration"].SetValue(dissolveDuration);
		dissolve.Parameters["uDissolveColor"].SetValue(dissolveColor);
		dissolve.Parameters["uNoiseSize"].SetValue(1f);
		dissolve.Parameters["uNoiseXY"].SetValue(new Vector2(Projectile.ai[1], Projectile.ai[2]));
		dissolve.CurrentTechnique.Passes[0].Apply();
	}
}