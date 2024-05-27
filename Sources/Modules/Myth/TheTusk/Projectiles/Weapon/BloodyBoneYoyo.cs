using Everglow.Commons.Weapons.Yoyos;
using Humanizer;
using Terraria;
using Terraria.DataStructures;
using static Everglow.Myth.TheTusk.Projectiles.Weapon.BloodyBoneYoyo;

namespace Everglow.Myth.TheTusk.Projectiles.Weapon;

public class BloodyBoneYoyo : YoyoProjectile
{
	public override void SetDefaults()
	{

		base.SetDefaults();
		MaxStaySeconds = 60;
		Acceleration = 7;
		MaxRopeLength = 320;
	}
	public struct Tentacle
	{
		public List<Vector2> oldPos;
		public List<Vector2> jointVel;
		public Vector2 velocity;
		public Vector2 position;
		public float time;
		public float ai0;
		public float ai1;
		public float ai2;
		public float ai3;
	}
	public List<Tentacle> tentacles;
	public Tentacle UpdateTentacle(Tentacle tentacle)
	{
		if (tentacle.time < 75 && Projectile.ai[0] > 0)
		{
			tentacle.oldPos.Add(tentacle.position);
			float mulRot = (75 - tentacle.time) / 75f;
			mulRot = MathF.Max(mulRot, 0);
			tentacle.jointVel.Add(tentacle.velocity.RotatedBy(MathHelper.PiOver2 ) * 0.1f * mulRot * MathF.Sin(tentacle.time * 0.4f));
		}
		if (tentacle.time > 75)
		{
			if(tentacle.oldPos.Count > 0)
			{
				tentacle.oldPos.RemoveAt(0);
				tentacle.jointVel.RemoveAt(0);
				float coilValue = 20;
				if (tentacle.time > 100)
				{
					coilValue = (tentacle.time - 95) * 4;
				}
				for (int x = tentacle.oldPos.Count - 1; x >= 0; x--)
				{
					float value = Math.Max(0, ((coilValue - 0.01f) - x) / coilValue);
					tentacle.oldPos[x] -= tentacle.oldPos[0] * value;
				}
			}		
		}
		else if(tentacle.oldPos.Count > Main.rand.Next(150 - (int)tentacle.time) * 2f)
		{
			tentacle.oldPos.RemoveAt(0);
			tentacle.jointVel.RemoveAt(0);
			float coilValue = 20;
			if (tentacle.time > 100)
			{
				coilValue = (tentacle.time - 95) * 4;
			}
			for (int x = tentacle.oldPos.Count - 1; x >= 0; x--)
			{
				float value = Math.Max(0, ((coilValue - 0.01f) - x) / coilValue);
				tentacle.oldPos[x] -= tentacle.oldPos[0] * value;
			}
		}
		//收球的时候剧烈收回
		if (Projectile.ai[0] <= 0)
		{
			for (int a = 0;a < 2;a++)
			{
				if(tentacle.oldPos.Count > 0)
				{
					tentacle.oldPos.RemoveAt(0);
					tentacle.jointVel.RemoveAt(0);
					for (int x = tentacle.oldPos.Count - 1; x >= 0; x--)
					{
						float value = Math.Max(0, (19.99f - x) / 20f);
						tentacle.oldPos[x] -= tentacle.oldPos[0] * value;
					}
				}
				else
				{
					break;
				}
			}
		}
		for (int x = tentacle.oldPos.Count - 1; x >= 0; x--)
		{
			if (tentacle.jointVel.Count > x)
			{
				tentacle.oldPos[x] += tentacle.jointVel[x];
				tentacle.jointVel[x] *= 0.98f;
			}
		}
		////碰撞
		//for (int x = tentacle.oldPos.Count - 1; x >= 0; x--)
		//{
		//	if(tentacle.jointVel.Count > x)
		//	{
		//		if(Collision.SolidCollision(tentacle.oldPos[x] + tentacle.jointVel[x] + Projectile.Center, 1, 1))
		//		{
		//			tentacle.jointVel[x] -= Vector2.Normalize(tentacle.oldPos[x]);
		//			tentacle.oldPos[x] += tentacle.jointVel[x];

		//		}
		//		else if(Collision.SolidCollision(tentacle.oldPos[x] + Projectile.velocity + Projectile.Center, 1, 1))
		//		{
		//			tentacle.jointVel[x] -= Vector2.Normalize(Projectile.velocity) * 0.2f;
		//			tentacle.oldPos[x] -= Projectile.velocity;
		//		}
		//		else
		//		{
		//			tentacle.oldPos[x] += tentacle.jointVel[x];
		//			tentacle.jointVel[x] *= 0.95f;
		//		}
		//	}
		//}
		////牵拉
		//for (int x = tentacle.oldPos.Count - 1; x >= 1; x--)
		//{
		//	if (tentacle.jointVel.Count > x)
		//	{
		//		Vector2 v0 = tentacle.oldPos[x - 1] + tentacle.jointVel[x - 1] - tentacle.oldPos[x] - tentacle.jointVel[x];
		//		if (v0.Length() > 15)
		//		{
		//			Vector2 dragForce = Vector2.Normalize(v0) * (v0.Length() - 15) * 0.01f;
		//			for (int y = x; y < tentacle.oldPos.Count - 1; y++)
		//			{
		//				tentacle.oldPos[x] += dragForce * Math.Max(0, (19.99f - x) / 20f);
		//			}
		//		}
		//	}
		//}
		if (Collision.SolidCollision(tentacle.position + tentacle.velocity + Projectile.Center, 1, 1))
		{
			if(tentacle.time < 75)
			{
				tentacle.time = 150 - tentacle.time;
			}
		}
		tentacle.position += tentacle.velocity;
		float maxRotVel = tentacle.time / 30f;
		if(maxRotVel > 1)
		{
			maxRotVel = 1;
		}
		tentacle.velocity = tentacle.velocity.RotatedBy(tentacle.ai0 * maxRotVel);

		tentacle.ai0 *= 0.98f;
		tentacle.ai0 += tentacle.ai1;
		tentacle.ai1 *= 0.98f;
		if (tentacle.time > 30)
		{
			if(tentacle.velocity.Length() > 0.1f)
			{
				tentacle.velocity *= 0.94f;
			}
		}
		if (tentacle.time > 60)
		{
			//tentacle.velocity += Vector2.Normalize(-tentacle.velocity - tentacle.position);
		}
		tentacle.time++;
		return tentacle;
	}
	public override void OnSpawn(IEntitySource source)
	{
		tentacles = new List<Tentacle>();
		base.OnSpawn(source);
	}
	public override void AI()
	{
		if (Projectile.oldPosition != Vector2.Zero)
		{
			for (int g = 0; g < Projectile.velocity.Length() * 2.5f; g++)
			{
				Vector2 a0 = new Vector2(Projectile.width, Projectile.height) / 2f;
				Vector2 v3 = Projectile.oldPosition + a0;
				Vector2 v4 = Vector2.Normalize(Projectile.velocity) * 0.6f;
				Dust dust = Dust.NewDustDirect(v3 + v4 * g - new Vector2(4, 4), 4, 4, DustID.Blood, 0f, 0f, 100, default, Main.rand.NextFloat(1.3f, 2f) * 0.4f);
				dust.noGravity = true;
				dust.velocity = new Vector2(0, Main.rand.NextFloat(0.4f, 2.5f)).RotatedByRandom(Math.PI * 2d) * 0.5f;
			}
		}
		if(Main.rand.NextBool(15) && tentacles.Count < 8)
		{
			Tentacle tentacle = new Tentacle();
			tentacle.oldPos = new List<Vector2>();
			tentacle.jointVel = new List<Vector2>();
			tentacle.time = 0;
			tentacle.position = Vector2.zeroVector;
			tentacle.velocity = new Vector2(Main.rand.NextFloat(3, 6f), 0).RotatedByRandom(MathHelper.TwoPi);
			tentacle.ai0 = Main.rand.NextFloat(-0.3f, 0.3f);
			tentacle.ai1 = Main.rand.NextFloat(-0.001f, 0.027f) * -MathF.Sign(tentacle.ai0);
			tentacles.Add(tentacle);
		}
		if (tentacles.Count > 0)
		{
			for (int x = tentacles.Count - 1; x >= 0; x--)
			{
				tentacles[x] = UpdateTentacle(tentacles[x]);
				if (tentacles[x].time > 150)
				{
					tentacles.RemoveAt(x);
				}
			}
		}

		base.AI();
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{

	}
	public override void DrawString(Vector2 to = default)
	{
		Player player = Main.player[Projectile.owner];
		Vector2 mountedCenter = player.MountedCenter;
		Vector2 vector = mountedCenter;
		vector.Y += player.gfxOffY;
		if (to != default)
			vector = to;
		float num = Projectile.Center.X - vector.X;
		float num2 = Projectile.Center.Y - vector.Y;
		Math.Sqrt((double)(num * num + num2 * num2));
		float rotation;
		if (!Projectile.counterweight)
		{
			int num3 = -1;
			if (Projectile.position.X + Projectile.width / 2 < player.position.X + player.width / 2)
				num3 = 1;
			num3 *= -1;
			player.itemRotation = (float)Math.Atan2((double)(num2 * num3), (double)(num * num3));
		}
		bool checkSelf = true;
		if (num == 0f && num2 == 0f)
			checkSelf = false;
		else
		{
			float num4 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			num4 = 12f / num4;
			num *= num4;
			num2 *= num4;
			vector.X -= num * 0.1f;
			vector.Y -= num2 * 0.1f;
			num = Projectile.position.X + Projectile.width * 0.5f - vector.X;
			num2 = Projectile.position.Y + Projectile.height * 0.5f - vector.Y;
		}
		int count = 0;
		while (checkSelf)
		{
			count++;
			float num6 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			float num7 = num6;
			if (float.IsNaN(num6) || float.IsNaN(num7))
				checkSelf = false;
			else
			{
				if (num6 < 4f)
				{
					checkSelf = false;
				}
				num6 = 1f / num6;
				num *= num6;
				num2 *= num6;
				vector.X += num;
				vector.Y += num2;
				num = Projectile.position.X + Projectile.width * 0.5f - vector.X;
				num2 = Projectile.position.Y + Projectile.height * 0.1f - vector.Y;
				if (num7 > 12f)
				{
					float num8 = 0.3f;
					float num9 = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
					if (num9 > 16f)
						num9 = 16f;
					num9 = 1f - num9 / 16f;
					num8 *= num9;
					num9 = num7 / 80f;
					if (num9 > 1f)
						num9 = 1f;
					num8 *= num9;
					if (num8 < 0f)
						num8 = 0f;
					num8 *= num9;
					num8 *= 0.5f;
					if (num2 > 0f)
					{
						num2 *= 1f + num8;
						num *= 1f - num8;
					}
					else
					{
						num9 = Math.Abs(Projectile.velocity.X) / 3f;
						if (num9 > 1f)
							num9 = 1f;
						num9 -= 0.5f;
						num8 *= num9;
						if (num8 > 0f)
							num8 *= 2f;
						num2 *= 1f + num8;
						num *= 1f - num8;
					}
				}
				rotation = (float)Math.Atan2((double)num2, (double)num) - 1.57f;
				int stringColor = player.stringColor;
				Color color = WorldGen.paintColor(stringColor);
				if (color.R < 75)
					color.R = 75;
				if (color.G < 75)
					color.G = 75;
				if (color.B < 75)
					color.B = 75;
				if (stringColor == 13)
					color = new Color(20, 20, 20);
				else if (stringColor == 14 || stringColor == 0)
				{
					color = new Color(200, 200, 200);
				}
				else if (stringColor == 28)
				{
					color = new Color(163, 116, 91);
				}
				else if (stringColor == 27)
				{
					color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
				}
				color.A = (byte)(color.A * 0.4f);
				float num10 = 0.5f;
				color = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f), color);
				color = new Color((byte)(color.R * num10), (byte)(color.G * num10), (byte)(color.B * num10), (byte)(color.A * num10));
				Texture2D tex = ModAsset.BloodyBoneYoyo_line.Value;
				if (count % 12 == 0)
				{
					Main.spriteBatch.Draw(tex, vector + new Vector2(0, 4) - Main.screenPosition, new Rectangle(3, 0, 2, 6), color, rotation + MathHelper.PiOver2, Vector2.One, new Vector2(1f, 1f), SpriteEffects.None, 0f);
				}
				else if (count % 12 == 6)
				{
					Main.spriteBatch.Draw(tex, vector + new Vector2(0, 4) - Main.screenPosition, new Rectangle(6, 0, 2, 6), color, rotation + MathHelper.PiOver2, Vector2.One, new Vector2(1f, 1f), SpriteEffects.None, 0f);
				}
				else
				{
					Main.spriteBatch.Draw(tex, vector + new Vector2(0, 4) - Main.screenPosition, new Rectangle(0, 0, 2, 6), color, rotation + MathHelper.PiOver2, Vector2.One, new Vector2(1f, 1f), SpriteEffects.None, 0f);
				}
			}
		}
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		foreach (Tentacle tentacle in tentacles)
		{
			foreach(Vector2 v0 in tentacle.oldPos)
			{
				Rectangle rectangle = new Rectangle((int)(Projectile.Center + v0).X - 4, (int)(Projectile.Center + v0).Y - 4, 8, 8);
				if (targetHitbox.Intersects(rectangle))
				{
					return true;
				}
			}
		}
		return base.Colliding(projHitbox, targetHitbox);
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawString();
		List<Vertex2D> bars = new List<Vertex2D>();
		foreach(Tentacle tentacle in tentacles)
		{
			for (int x = 0;x < tentacle.oldPos.Count;x++)
			{
				Vector2 drawPos = tentacle.oldPos[x] + Projectile.Center;
				Vector2 posLeft = Vector2.Normalize(tentacle.oldPos[x]).RotatedBy(MathHelper.PiOver2) * 5f;
				if(x > 0)
				{
					posLeft = Vector2.Normalize(tentacle.oldPos[x] - tentacle.oldPos[x - 1]).RotatedBy(MathHelper.PiOver2) * 5f;
				}
				float factor = (x + 120 - tentacle.oldPos.Count) / 120f;
				float width = 1f;
				if(factor > 0.8f)
				{
					width = MathF.Sin((1 - factor) * 2.5f * MathF.PI);
				}
				if (x == 0)
				{
					bars.Add(Projectile.Center + posLeft, Color.Transparent, new Vector3(factor, 0, width));
					bars.Add(Projectile.Center - posLeft, Color.Transparent, new Vector3(factor, 1, width));
					bars.Add(Projectile.Center + posLeft, lightColor, new Vector3(factor, 0, width));
					bars.Add(Projectile.Center - posLeft, lightColor, new Vector3(factor, 1, width));
				}
				Color newLightColor = Lighting.GetColor(drawPos.ToTileCoordinates());
				bars.Add(drawPos + posLeft, newLightColor, new Vector3(factor, 0, width));
				bars.Add(drawPos - posLeft, newLightColor, new Vector3(factor, 1, width));
				if(x == tentacle.oldPos.Count - 1)
				{
					bars.Add(drawPos + posLeft, Color.Transparent, new Vector3(factor, 0, width));
					bars.Add(drawPos - posLeft, Color.Transparent, new Vector3(factor, 1, width));
				}
			}
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Effect effect = Commons.ModAsset.Trailing.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition.X, -Main.screenPosition.Y, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.BloodyBoneYoyo_tentacle.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		return true;
	}
}
