using Terraria.GameContent;
namespace Everglow.Commons.Weapons.Yoyos;

public abstract class YoyoProjectile : ModProjectile
{
	public override void SetDefaults()
	{
		//Projectile.CloneDefaults(549);
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.scale = 1f;
		ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 200f;
		Projectile.DamageType = DamageClass.Melee;
		MaxStaySeconds = 3;
		MaxRopeLength = 200;
		Acceleration = 14f;
		RotatedSpeed = 0.45f;
		Weight = 4f;
		SetDef();
	}
	public virtual void SetDef()
	{

	}
	/// <summary>
	/// When hit npc, the speed of rebound inversely proportional to the value.default to 10.
	/// </summary>
	public float Weight;
	/// <summary>
	/// Yoyo exists after [this] seconds will be reuse.less than 0 to make yoyo exists enternal.default to 3.
	/// </summary>
	public float MaxStaySeconds;
	/// <summary>
	/// default to 200.
	/// </summary>
	public float MaxRopeLength;
	/// <summary>
	/// default to 14.
	/// </summary>
	public float Acceleration;
	/// <summary>
	/// default to 0.45f.
	/// </summary>
	public float RotatedSpeed;
	public override void AI()
	{
		YoyoAI();
	}
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		Player player = Main.player[Projectile.owner];
		Projectile.velocity += Vector2.Normalize(Projectile.Center - target.Center) / (Weight + 0.01f) * 100f;
		if(player.yoyoGlove && Projectile.ai[2] != 1 && player.ownedProjectileCounts[Projectile.type] <= 1)
		{
			Projectile.NewProjectile(Projectile.GetSource_FromAI(),player.Center, Vector2.Normalize(Projectile.Center - player.MountedCenter) * player.HeldItem.shootSpeed, Projectile.type, Projectile.damage, Projectile.knockBack, player.whoAmI, 0, 0, 1);
		}
		base.OnHitNPC(target, hit, damageDone);
	}
	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		return false;
	}
	/// <summary>
	/// Yoyo
	/// </summary>
	/// <param name="index"></param>
	/// <param name="seconds"></param>
	/// <param name="MaxRopeLength"></param>
	/// <param name="acceleration"></param>
	/// <param name="rotationSpeed"></param>
	public virtual void YoyoAI()
	{
		Player player = Main.player[Projectile.owner];

		bool checkSelf = false;
		for (int i = 0; i < Projectile.whoAmI; i++)
		{
			if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type)
				checkSelf = true;
		}
		if (Projectile.owner == Main.myPlayer)
		{
			Projectile.localAI[0] += 1f;
			if (checkSelf)
				Projectile.localAI[0] += Main.rand.NextFloat(1.0f, 3.1f);
			float timerOfSecond = Projectile.localAI[0] / 60f;
			timerOfSecond /= (1f + player.GetAttackSpeed(DamageClass.Generic)) / 2f;
			if (timerOfSecond > MaxStaySeconds && MaxStaySeconds > 0)
				Projectile.ai[0] = -1f;
		}
		if (player.dead)
		{
			Projectile.Kill();
			return;
		}
		if (!checkSelf)
		{
			player.heldProj = Projectile.whoAmI;
			player.itemAnimation = 2;
			player.itemTime = 2;
			if (Projectile.Center.X > player.Center.X)
			{
				player.ChangeDir(1);
				Projectile.direction = 1;
			}
			else
			{
				player.ChangeDir(-1);
				Projectile.direction = -1;
			}
		}
		if (Projectile.velocity.HasNaNs())
			Projectile.Kill();
		Projectile.timeLeft = 6;

		if (player.yoyoString)
			MaxRopeLength = MaxRopeLength * 1.25f + 30f;
		MaxRopeLength /= (1f + player.GetAttackSpeed(DamageClass.Generic) * 3f) / 4f;
		float num3 = Acceleration / ((1f + player.GetAttackSpeed(DamageClass.Generic) * 3f) / 4f);
		float num4 = 14f - num3 / 2f;
		float num5 = 5f + num3 / 2f;
		if (checkSelf)
			num5 += 20f;
		if (Projectile.ai[0] >= 0f)
		{
			if (Projectile.velocity.Length() > num3)
				Projectile.velocity *= 0.98f;
			bool flag3 = false;
			bool flag4 = false;
			Vector2 vector = player.Center - Projectile.Center;
			if (vector.Length() > MaxRopeLength)
			{
				flag3 = true;
				if ((double)vector.Length() > (double)MaxRopeLength * 1.3)
					flag4 = true;
			}
			if (Projectile.owner == Main.myPlayer)
			{
				if (!player.channel || player.stoned || player.frozen)
				{
					Projectile.ai[0] = -1f;
					Projectile.ai[1] = 0f;
					Projectile.netUpdate = true;
				}
				else
				{
					Vector2 vector2 = Main.ReverseGravitySupport(Main.MouseScreen, 0f) + Main.screenPosition;
					float x = vector2.X;
					float y = vector2.Y;
					Vector2 vector3 = new Vector2(x, y) - player.Center;
					if (vector3.Length() > MaxRopeLength)
					{
						vector3.Normalize();
						vector3 *= MaxRopeLength;
						vector3 = player.Center + vector3;
						x = vector3.X;
						y = vector3.Y;
					}
					if (Projectile.ai[0] != x || Projectile.ai[1] != y)
					{
						var value = new Vector2(x, y);
						Vector2 vector4 = value - player.Center;
						if (vector4.Length() > MaxRopeLength - 1f)
						{
							vector4.Normalize();
							vector4 *= MaxRopeLength - 1f;
							value = player.Center + vector4;
							x = value.X;
							y = value.Y;
						}
						Projectile.ai[0] = x;
						Projectile.ai[1] = y;
						Projectile.netUpdate = true;
					}
				}
			}
			if (flag4 && Projectile.owner == Main.myPlayer)
			{
				Projectile.ai[0] = -1f;
				Projectile.netUpdate = true;
			}
			if (Projectile.ai[0] >= 0f)
			{
				if (flag3)
				{
					num4 /= 2f;
					num3 *= 2f;
					if (Projectile.Center.X > player.Center.X && Projectile.velocity.X > 0f)
						Projectile.velocity.X = Projectile.velocity.X * 0.5f;
					if (Projectile.Center.Y > player.Center.Y && Projectile.velocity.Y > 0f)
						Projectile.velocity.Y = Projectile.velocity.Y * 0.5f;
					if (Projectile.Center.X < player.Center.X && Projectile.velocity.X > 0f)
						Projectile.velocity.X = Projectile.velocity.X * 0.5f;
					if (Projectile.Center.Y < player.Center.Y && Projectile.velocity.Y > 0f)
						Projectile.velocity.Y = Projectile.velocity.Y * 0.5f;
				}
				var value2 = new Vector2(Projectile.ai[0], Projectile.ai[1]);
				Vector2 vector5 = value2 - Projectile.Center;
				Projectile.velocity.Length();
				if (vector5.Length() > num5)
				{
					vector5.Normalize();
					vector5 *= num3;
					Projectile.velocity = (Projectile.velocity * (num4 - 1f) + vector5) / num4;
				}
				else if (checkSelf)
				{
					if ((double)Projectile.velocity.Length() < (double)num3 * 0.6)
					{
						vector5 = Projectile.velocity;
						vector5.Normalize();
						vector5 *= num3 * 0.6f;
						Projectile.velocity = (Projectile.velocity * (num4 - 1f) + vector5) / num4;
					}
				}
				else
				{
					Projectile.velocity *= 0.8f;
				}
				if (checkSelf && !flag3 && (double)Projectile.velocity.Length() < (double)num3 * 0.6)
				{
					Projectile.velocity.Normalize();
					Projectile.velocity *= num3 * 0.6f;
				}
			}
		}
		else
		{
			num4 *= 0.8f;
			num3 *= 1.5f;
			Projectile.tileCollide = false;
			Vector2 vector6 = player.position - Projectile.Center;
			float num6 = vector6.Length();
			if (num6 < num3 + 10f || num6 == 0f)
				Projectile.Kill();
			else
			{
				vector6.Normalize();
				vector6 *= num3;
				Projectile.velocity = (Projectile.velocity * (num4 - 1f) + vector6) / num4;
			}
		}
		Projectile.rotation += RotatedSpeed;
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawString();
		return base.PreDraw(ref lightColor);
	}
	public virtual void DrawString(Vector2 to = default)
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
		while (checkSelf)
		{
			float num5 = 12f;
			float num6 = (float)Math.Sqrt((double)(num * num + num2 * num2));
			float num7 = num6;
			if (float.IsNaN(num6) || float.IsNaN(num7))
				checkSelf = false;
			else
			{
				if (num6 < 20f)
				{
					num5 = num6 - 8f;
					checkSelf = false;
				}
				num6 = 12f / num6;
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
				Texture2D tex = TextureAssets.FishingLine.Value;
				Main.spriteBatch.Draw(tex, new Vector2(vector.X - Main.screenPosition.X + tex.Width * 0.5f, vector.Y - Main.screenPosition.Y + tex.Height * 0.5f) - new Vector2(6f, 0f), new Rectangle?(new Rectangle(0, 0, tex.Width, (int)num5)), color, rotation, new Vector2(tex.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
			}
		}
	}
}
