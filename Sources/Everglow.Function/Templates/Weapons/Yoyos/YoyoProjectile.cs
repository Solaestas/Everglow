using System.Reflection;
using Everglow.Commons.CustomTiles;
using Everglow.Commons.Utilities;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace Everglow.Commons.Templates.Weapons.Yoyos;

public abstract class YoyoProjectile : ModProjectile
{
	public override void SetStaticDefaults()
	{
		// The following sets are only applicable to yoyo that use aiStyle 99.

		// YoyosLifeTimeMultiplier is how long in seconds the yoyo will stay out before automatically returning to the player.
		// Vanilla values range from 3f (Wood) to 16f (Chik), and defaults to -1f. Leaving as -1 will make the time infinite.
		ProjectileID.Sets.YoyosLifeTimeMultiplier[Projectile.type] = 3.5f;

		// YoyosMaximumRange is the maximum distance the yoyo sleep away from the player.
		// Vanilla values range from 130f (Wood) to 400f (Terrarian), and defaults to 200f.
		ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 300f;

		// YoyosTopSpeed is top speed of the yoyo Projectile.
		// Vanilla values range from 9f (Wood) to 17.5f (Terrarian), and defaults to 10f.
		ProjectileID.Sets.YoyosTopSpeed[Projectile.type] = 13f;
	}

	public override void SetDefaults()
	{
		// Projectile.CloneDefaults(549);
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.scale = 1f;

		// Projectile.aiStyle = ProjAIStyleID.Yoyo;
		Projectile.aiStyle = -1;
		Projectile.DamageType = DamageClass.MeleeNoSpeed;

		ProjectileID.Sets.YoyosMaximumRange[Projectile.type] = 200f;
		MaxStaySeconds = 3;
		MaxRopeLength = 200;
		Acceleration = 14f;
		RotatedSpeed = 0.45f;
		Weight = 4f;
		SetCustomDefaults();
	}

	public virtual void SetCustomDefaults()
	{
	}

	public override void Load()
	{
		Ins.HookManager.AddHook(typeof(Projectile).GetMethod(nameof(Projectile.AI_099_1), BindingFlags.NonPublic | BindingFlags.Instance), ILHook_Projectile_Counterweight);
	}

	/// <summary>
	/// Modify the counterweight checking code inside Projectile.AI_099_1();
	/// </summary>
	/// <param name="il"></param>
	private void ILHook_Projectile_Counterweight(ILContext il)
	{
		ILCursor cursor = new ILCursor(il);

		var isNotModYoyoProjLabel = cursor.DefineLabel();
		if (cursor.TryGotoNext(
			MoveType.Before,
			x => x.MatchLdsfld(typeof(Main).GetField("projectile")),
			x => x.MatchLdloc(10),
			x => x.MatchLdelemRef(),
			x => x.MatchLdfld<Projectile>("aiStyle"),
			x => x.MatchLdcI4(99)))
		{
			cursor.MarkLabel(isNotModYoyoProjLabel);
		}

		var isYoyoProjLabel = cursor.DefineLabel();
		if (cursor.TryGotoNext(
			MoveType.After,
			x => x.MatchLdfld<Projectile>("type"),
			x => x.MatchLdcI4(561),
			x => x.MatchBle(out _)))
		{
			cursor.MarkLabel(isYoyoProjLabel);
		}

		if (cursor.TryGotoPrev(
			MoveType.After,
			x => x.MatchLdfld<Projectile>("owner"),
			x => x.MatchLdarg0(),
			x => x.MatchLdfld<Projectile>("owner"),
			x => x.MatchBneUn(out _))) // find the following branch instruction
		{
			cursor.EmitLdsfld(typeof(Main).GetField("projectile"));
			cursor.EmitLdloc(10);
			cursor.EmitLdelemRef();
			cursor.EmitCallvirt(typeof(Projectile).GetMethod("get_ModProjectile"));
			cursor.EmitBrfalse(isNotModYoyoProjLabel);

			cursor.EmitLdsfld(typeof(Main).GetField("projectile"));
			cursor.EmitLdloc(10);
			cursor.EmitLdelemRef();
			cursor.Emit(OpCodes.Callvirt, typeof(Projectile).GetMethod("get_ModProjectile"));
			cursor.EmitIsinst(typeof(YoyoProjectile));
			cursor.EmitBrtrue(isYoyoProjLabel);
		}
	}

	public override bool PreAI()
	{
		if (Projectile.ModProjectile is not null)
		{
		}
		return base.PreAI();
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

	/// <summary>
	/// The yoyo string, a rope connect yoyo projectile with player's hand.
	/// </summary>
	public List<Vector2> YoyoStringPos = new List<Vector2>();

	public override void AI()
	{
		YoyoAI();
		ExtraAI();
	}

	/// <summary>
	/// Reading base._ before overriding.
	/// </summary>
	/// <param name="target"></param>
	/// <param name="hit"></param>
	/// <param name="damageDone"></param>
	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		ModifyOnHitBounce(target, hit, damageDone);
		ModifyCounterWeightAndYoyoGloveEffect(target.Center, Projectile.damage, Projectile.knockBack);
		base.OnHitNPC(target, hit, damageDone);
	}

	public virtual void ModifyOnHitBounce(NPC target, NPC.HitInfo hit, int damageDone)
	{
		Projectile.velocity += Vector2.Normalize(Projectile.Center - target.Center) / (Weight + 0.01f) * 100f;
	}

	/// <summary>
	/// Vanilla function, spawn counterweight and the other yoyo on hit.
	/// </summary>
	/// <param name="hitPos"></param>
	/// <param name="damage"></param>
	/// <param name="knockBack"></param>
	public virtual void ModifyCounterWeightAndYoyoGloveEffect(Vector2 hitPos, int damage, float knockBack)
	{
		if (Projectile.owner < 0 || Projectile.owner >= Main.player.Length)
		{
			return;
		}
		Player player = Main.player[Projectile.owner];
		if (!player.yoyoGlove && player.counterWeight <= 0)
		{
			return;
		}

		int lastYoyoWhoAmI = -1;
		int ownYoyoCount = 0;
		int ownCounterWeightCount = 0;
		for (int i = 0; i < 1000; i++)
		{
			Projectile proj = Main.projectile[i];
			if (proj.active && proj.owner == player.whoAmI)
			{
				if (proj.counterweight)
				{
					ownCounterWeightCount++;
				}
				else if (proj.aiStyle == ProjAIStyleID.Yoyo || proj.ModProjectile is YoyoProjectile)
				{
					ownYoyoCount++;
					lastYoyoWhoAmI = i;
				}
			}
		}

		if (lastYoyoWhoAmI < 0 || lastYoyoWhoAmI >= Main.projectile.Length)
		{
			return;
		}

		Projectile parentProj = Main.projectile[lastYoyoWhoAmI];
		if (player.yoyoGlove && ownYoyoCount < 2)
		{
			Vector2 vector = hitPos - player.Center;
			vector.Normalize();
			vector *= 16f;
			Projectile.NewProjectile(Projectile.InheritSource(parentProj), player.Center, vector, parentProj.type, parentProj.damage, parentProj.knockBack, player.whoAmI, 1f);
		}
		else if (ownCounterWeightCount < ownYoyoCount)
		{
			Vector2 vector2 = hitPos - player.Center;
			vector2.Normalize();
			vector2 *= 16f;
			knockBack = (knockBack + 6f) / 2f;
			IEntitySource spawnSource = Projectile.InheritSource(parentProj);
			if (ownCounterWeightCount > 0)
			{
				Projectile.NewProjectile(spawnSource, player.Center, vector2, player.counterWeight, (int)(damage * 0.8f), knockBack, player.whoAmI, 1f);
			}
			else
			{
				Projectile.NewProjectile(spawnSource, player.Center, vector2, player.counterWeight, (int)(damage * 0.8f), knockBack, player.whoAmI);
			}
		}
	}

	public override bool OnTileCollide(Vector2 oldVelocity)
	{
		return false;
	}

	/// <summary>
	/// Yoyo AI from vanilla.
	/// </summary>
	/// <param name="index"></param>
	/// <param name="seconds"></param>
	/// <param name="MaxRopeLength"></param>
	/// <param name="acceleration"></param>
	/// <param name="rotationSpeed"></param>
	public virtual void YoyoAI()
	{
		Player player = Main.player[Projectile.owner];
		player.statDefense.AdditiveBonus += 1;
		bool checkSelf = false;
		float maxRopeDistance = MaxRopeLength;
		for (int i = 0; i < Projectile.whoAmI; i++)
		{
			if (Main.projectile[i].active && Main.projectile[i].owner == Projectile.owner && Main.projectile[i].type == Projectile.type)
			{
				checkSelf = true;
			}
		}
		if (Projectile.owner == Main.myPlayer)
		{
			Projectile.localAI[0] += 1f;
			if (checkSelf)
			{
				Projectile.localAI[0] += Main.rand.NextFloat(1.0f, 3.1f);
			}

			float timerOfSecond = Projectile.localAI[0] / 60f;
			timerOfSecond /= (1f + player.GetAttackSpeed(DamageClass.Generic)) / 2f;
			if (timerOfSecond > MaxStaySeconds && MaxStaySeconds > 0)
			{
				Projectile.ai[0] = -1f;
			}
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
		{
			Projectile.Kill();
		}
		Projectile.timeLeft = 6;
		if (player.yoyoString)
		{
			maxRopeDistance = MaxRopeLength * 1.25f + 30f;
		}
		maxRopeDistance /= (1f + player.GetAttackSpeed(DamageClass.Generic) * 3f) / 4f;
		float num3 = Acceleration / ((1f + player.GetAttackSpeed(DamageClass.Generic) * 3f) / 4f);
		float num4 = 14f - num3 / 2f;
		float num5 = 5f + num3 / 2f;
		if (checkSelf)
		{
			num5 += 20f;
		}
		if (Projectile.ai[0] >= 0f)
		{
			if (Projectile.velocity.Length() > num3)
			{
				Projectile.velocity *= 0.98f;
			}

			bool flag3 = false;
			bool flag4 = false;
			Vector2 vector = player.Center - Projectile.Center;
			if (vector.Length() > maxRopeDistance)
			{
				flag3 = true;
				if ((double)vector.Length() > maxRopeDistance * 1.3)
				{
					flag4 = true;
				}
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
					if (vector3.Length() > maxRopeDistance)
					{
						vector3.Normalize();
						vector3 *= maxRopeDistance;
						vector3 = player.Center + vector3;
						x = vector3.X;
						y = vector3.Y;
					}
					if (Projectile.ai[0] != x || Projectile.ai[1] != y)
					{
						var value = new Vector2(x, y);
						Vector2 vector4 = value - player.Center;
						if (vector4.Length() > maxRopeDistance - 1f)
						{
							vector4.Normalize();
							vector4 *= maxRopeDistance - 1f;
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
					{
						Projectile.velocity.X = Projectile.velocity.X * 0.5f;
					}

					if (Projectile.Center.Y > player.Center.Y && Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y = Projectile.velocity.Y * 0.5f;
					}

					if (Projectile.Center.X < player.Center.X && Projectile.velocity.X > 0f)
					{
						Projectile.velocity.X = Projectile.velocity.X * 0.5f;
					}

					if (Projectile.Center.Y < player.Center.Y && Projectile.velocity.Y > 0f)
					{
						Projectile.velocity.Y = Projectile.velocity.Y * 0.5f;
					}
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
			{
				Projectile.Kill();
			}
			else
			{
				vector6.Normalize();
				vector6 *= num3;
				Projectile.velocity = (Projectile.velocity * (num4 - 1f) + vector6) / num4;
			}
		}
		Projectile.rotation += RotatedSpeed;
	}

	public virtual void ExtraAI()
	{
	}

	public override bool PreDraw(ref Color lightColor)
	{
		DrawYoyo_String();
		return base.PreDraw(ref lightColor);
	}

	/// <summary>
	/// playerHeldPos, keep default to the holding position of player's hand, or set to a world position manually.
	/// </summary>
	/// <param name="playerHeldPos"></param>
	public virtual void DrawYoyo_String(Vector2 playerHeldPos = default)
	{
		Player player = Main.player[Projectile.owner];
		Vector2 mountedCenter = player.MountedCenter;
		Vector2 stringUnitPos = mountedCenter;
		stringUnitPos.Y += player.gfxOffY;
		if (playerHeldPos != default)
		{
			stringUnitPos = playerHeldPos;
		}
		Vector2 unitToYoyoCenter = Projectile.Center - stringUnitPos;
		if (!Projectile.counterweight)
		{
			int stringDirection = -1;
			if (Projectile.position.X + Projectile.width / 2 < player.position.X + player.width / 2)
			{
				stringDirection = 1;
			}
			stringDirection *= -1;
			Vector2 rotVec = unitToYoyoCenter * stringDirection;
			player.itemRotation = rotVec.ToRotationSafe();
		}

		float unitLength = 12f;
		float finalLength;
		float finalRot;
		YoyoStringPos.Clear();
		GenerateYoyo_String(stringUnitPos, YoyoStringPos, out finalLength, out finalRot);
		DrawYoyo_String_Pieces(player, unitLength, finalLength, finalRot);
	}

	public virtual void DrawYoyo_String_Pieces(Player player, float unitLength, float finalLength, float finalRot)
	{
		Texture2D tex = TextureAssets.FishingLine.Value;
		int stringColor = player.stringColor;
		for (int i = 0; i < YoyoStringPos.Count; i++)
		{
			float rotation = finalRot;
			if (i < YoyoStringPos.Count - 1)
			{
				rotation = (YoyoStringPos[i + 1] - YoyoStringPos[i]).ToRotation() - MathHelper.PiOver2;
			}
			else
			{
				unitLength = finalLength;
			}
			Vector2 drawPos = YoyoStringPos[i] + tex.Size() * 0.5f - new Vector2(6, 0);
			Color color = ModifyYoyoStringColor_VanillaRender(stringColor, drawPos, i, YoyoStringPos.Count);
			drawPos -= Main.screenPosition;
			Main.spriteBatch.Draw(tex, drawPos, new Rectangle(0, 0, tex.Width, (int)unitLength), color, rotation, new Vector2(tex.Width * 0.5f, 0f), 1f, SpriteEffects.None, 0f);
			DrawYoyo_String_Attachments(drawPos, rotation, color, unitLength, i, YoyoStringPos.Count);
		}
	}

	public virtual void DrawYoyo_String_Attachments(Vector2 drawPos, float rotation, Color color, float length, float index, float stringUnitCount)
	{
	}

	/// <summary>
	/// Generate a yoyo-string that consist of a List<Vector2>().
	/// </summary>
	/// <param name="stringUnitPos"></param>
	/// <param name="yoyo_string_pos"></param>
	/// <param name="finalLength"></param>
	/// <param name="finalRot"></param>
	public virtual void GenerateYoyo_String(Vector2 stringUnitPos, List<Vector2> yoyo_string_pos, out float finalLength, out float finalRot)
	{
		finalLength = 12f;
		finalRot = 0f;
		Vector2 unitToYoyoCenter = Projectile.Center - stringUnitPos;
		bool checkSelf = true;
		if (unitToYoyoCenter == Vector2.zeroVector)
		{
			checkSelf = false;
		}
		else
		{
			float stringLength = unitToYoyoCenter.Length();
			stringLength = 12f / stringLength;
			unitToYoyoCenter *= stringLength;
			stringUnitPos -= unitToYoyoCenter * 0.1f;
			unitToYoyoCenter = Projectile.position + Projectile.Size * 0.5f - stringUnitPos;
		}
		while (checkSelf)
		{
			float restStringLength = unitToYoyoCenter.Length();
			float restStringLength_Copy = restStringLength;
			if (float.IsNaN(restStringLength) && float.IsNaN(restStringLength_Copy))
			{
				checkSelf = false;
			}
			else
			{
				if (restStringLength < 20f)
				{
					finalLength = restStringLength - 8f;
					checkSelf = false;
				}
				restStringLength = 12f / restStringLength;
				unitToYoyoCenter *= restStringLength;
				stringUnitPos += unitToYoyoCenter;
				unitToYoyoCenter = Projectile.position + Projectile.Size * new Vector2(0.5f, 0.1f) - stringUnitPos;
				if (restStringLength_Copy > 12f)
				{
					float speedChangeRate = 0.3f;
					float manhattanSpeed = Math.Abs(Projectile.velocity.X) + Math.Abs(Projectile.velocity.Y);
					if (manhattanSpeed > 16f)
					{
						manhattanSpeed = 16f;
					}
					manhattanSpeed = 1f - manhattanSpeed / 16f;
					speedChangeRate *= manhattanSpeed;
					manhattanSpeed = restStringLength_Copy / 80f;
					if (manhattanSpeed > 1f)
					{
						manhattanSpeed = 1f;
					}
					speedChangeRate *= manhattanSpeed;
					if (speedChangeRate < 0f)
					{
						speedChangeRate = 0f;
					}
					speedChangeRate *= manhattanSpeed;
					speedChangeRate *= 0.5f;
					if (unitToYoyoCenter.Y > 0f)
					{
						unitToYoyoCenter.Y *= 1f + speedChangeRate;
						unitToYoyoCenter.X *= 1f - speedChangeRate;
					}
					else
					{
						manhattanSpeed = Math.Abs(Projectile.velocity.X) / 3f;
						if (manhattanSpeed > 1f)
						{
							manhattanSpeed = 1f;
						}
						manhattanSpeed -= 0.5f;
						speedChangeRate *= manhattanSpeed;
						if (speedChangeRate > 0f)
						{
							speedChangeRate *= 2f;
						}
						unitToYoyoCenter.Y *= 1f + speedChangeRate;
						unitToYoyoCenter.X *= 1f - speedChangeRate;
					}
				}
				yoyo_string_pos.Add(stringUnitPos);
				finalRot = unitToYoyoCenter.ToRotation() - MathHelper.PiOver2;
			}
		}
	}

	public virtual Color ModifyYoyoStringColor_VanillaRender(int playerStringColor, Vector2 worldPos, float index, float stringCount)
	{
		Color color = WorldGen.paintColor(playerStringColor);
		if (color.R < 75)
		{
			color.R = 75;
		}
		if (color.G < 75)
		{
			color.G = 75;
		}
		if (color.B < 75)
		{
			color.B = 75;
		}
		if (playerStringColor == 13)
		{
			color = new Color(20, 20, 20);
		}
		else if (playerStringColor == 14 || playerStringColor == 0)
		{
			color = new Color(200, 200, 200);
		}
		else if (playerStringColor == 28)
		{
			color = new Color(163, 116, 91);
		}
		else if (playerStringColor == 27)
		{
			color = new Color(Main.DiscoR, Main.DiscoG, Main.DiscoB);
		}
		color.A = (byte)(color.A * 0.4f);
		color = Lighting.GetColor(worldPos.ToTileCoordinates(), color);
		color *= 0.5f;
		return ModifyYoyoStringColor_PostVanillaRender(color, worldPos, index, stringCount);
	}

	public virtual Color ModifyYoyoStringColor_PostVanillaRender(Color vanillaColor, Vector2 worldPos, float index, float stringCount)
	{
		return vanillaColor;
	}
}