using Everglow.Commons.MEAC;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.Utilities;

namespace Everglow.Commons.Templates.Weapons.StabbingSwords;

public abstract class StabbingProjectile : ModProjectile, IWarpProjectile
{
	/// <summary>
	/// Default ExtraUpdates | 默认ExtraUpdates
	/// </summary>
	public const int NormalExtraUpdates = 20;

	/// <summary>
	/// Main color | 主要颜色
	/// </summary>
	public Color Color = Color.White;

	/// <summary>
	/// Shadow intensity | 阴影强度
	/// </summary>
	public float Shade = 0f;

	/// <summary>
	/// 重影深度
	/// </summary>
	public float TradeShade = 0f;

	/// <summary>
	/// 重影彩色部分亮度
	/// </summary>
	public float TradeLightColorValue = 0f;

	/// <summary>
	/// 重影数量 (小于200)
	/// </summary>
	public int TradeLength = 0;

	/// <summary>
	/// 重影大小缩变,小于1
	/// </summary>
	public float FadeScale = 0f;

	/// <summary>
	/// 刀光宽度1
	/// </summary>
	public float DrawWidth = 1f;

	/// <summary>
	/// 重影深度缩变,小于1
	/// </summary>
	public float FadeShade = 0f;

	/// <summary>
	/// 重影彩色部分亮度缩变,小于1
	/// </summary>
	public float FadeLightColorValue = 0f;

	/// <summary>
	/// 表示刺剑攻击长度,标准长度1
	/// </summary>
	public float MaxLength = 1f;

	/// <summary>
	/// 荧光颜色,默认不会发光
	/// </summary>
	public Color GlowColor = Color.Transparent;

	/// <summary>
	/// 荧光颜色缩变,小于1
	/// </summary>
	public float FadeGlowColorValue = 0f;

	public Player Owner => Main.player[Projectile.owner];

	public override string Texture => ModAsset.StabbingProjectile_Mod;

	/// <summary>
	/// 宽度width会影响伤害判定(斜矩形)的宽度,高度会影响判定的长度
	/// </summary>
	public override void SetDefaults()
	{
		Projectile.width = 30;
		Projectile.height = 30;
		Projectile.aiStyle = -1;
		Projectile.friendly = true;
		Projectile.tileCollide = false;
		Projectile.DamageType = DamageClass.Melee;
		Projectile.penetrate = -1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 4;
		Projectile.extraUpdates = 20;
	}

	public virtual int SoundTimer { get; private set; } = 6;

	public int UpdateTimer = 0;

	public override void SendExtraAI(BinaryWriter writer)
	{
		writer.Write(SoundTimer);
		base.SendExtraAI(writer);
	}

	public override void ReceiveExtraAI(BinaryReader reader)
	{
		SoundTimer = reader.ReadInt32();
		base.ReceiveExtraAI(reader);
	}

	public override void AI()
	{
		UpdateTimer++;
		Projectile.extraUpdates = (int)(NormalExtraUpdates * Owner.meleeSpeed);
		int animation = 9;
		float rotationRange = Main.rand.NextFloatDirection() * (MathF.PI * 2f) * 0.05f;
		Projectile.ai[0] += 1f / 20f;
		if (Projectile.ai[0] >= 8f)
		{
			Projectile.ai[0] = 0f;
		}

		Projectile.soundDelay--;
		if (Projectile.soundDelay <= 0)
		{
			Projectile.soundDelay = SoundTimer * (1 + NormalExtraUpdates);
			SoundStyle ss = SoundID.Item1;
			SoundEngine.PlaySound(ss.WithPitchOffset(Owner.meleeSpeed - 1), Projectile.Center);
		}
		if (Main.myPlayer == Projectile.owner)
		{
			if (Owner.channel && !Owner.noItems && !Owner.CCed)
			{
				float hitSize = Owner.inventory[Owner.selectedItem].shootSpeed * Projectile.scale;
				Vector2 toMouse = Main.MouseWorld - Owner.RotatedRelativePoint(Owner.MountedCenter);
				toMouse.Normalize();
				if (toMouse.HasNaNs())
				{
					toMouse = Vector2.UnitX * Owner.direction;
				}
				toMouse *= hitSize;
				if (toMouse.X != Projectile.velocity.X || toMouse.Y != Projectile.velocity.Y)
				{
					Projectile.netUpdate = true;
				}
				Projectile.velocity = toMouse;
				Projectile.timeLeft = TradeLength * (NormalExtraUpdates + 1);
			}
		}

		if (!Owner.controlUseItem && Projectile.timeLeft > TradeLength * (NormalExtraUpdates + 1))
		{
			Projectile.timeLeft = TradeLength * (NormalExtraUpdates + 1);
		}

		if (Owner.HeldItem.ModItem is StabbingSwordItem modItem)
		{
			if (!Owner.GetModPlayer<PlayerStamina>().CheckStamina(modItem.staminaCost / Projectile.extraUpdates))
			{
				OnStaminaDepleted(Owner);
				Projectile.Kill(); // Return一堆怪问题，杀了就好了
			}
		}

		Projectile.position = Owner.RotatedRelativePoint(Owner.MountedCenter, reverseRotation: false, addGfxOffY: false) - Projectile.Size / 2f;
		Projectile.rotation = Projectile.velocity.ToRotation();
		Projectile.spriteDirection = Projectile.direction;
		Owner.ChangeDir(Projectile.direction);
		Owner.heldProj = Projectile.whoAmI;
		Owner.SetDummyItemTime(animation);
		Owner.itemRotation = MathHelper.WrapAngle((float)Math.Atan2(Projectile.velocity.Y * Projectile.direction, Projectile.velocity.X * Projectile.direction) + rotationRange);
		Owner.itemAnimation = animation - (int)Projectile.ai[0];
		UpdateItemDraw();
		UpdateDarkDraw();
		UpdateLightDraw();
		if (Main.rand.NextBool(NormalExtraUpdates))
		{
			VisualParticle();
		}
		CustomBehavior();
	}

	public virtual void CustomBehavior()
	{
	}

	/// <summary>
	/// Handle what happens before the projectile is killed by stamina depletion.
	/// <para/>Called in <see cref="ModProjectile.AI()"/> when the player runs out of stamina, right before <see cref="ModProjectile.Kill(int)"/> is called.
	/// </summary>
	public virtual void OnStaminaDepleted(Player player)
	{
	}

	public virtual void VisualParticle()
	{
	}

	public virtual void HitTileSound(float scale)
	{
		SoundStyle sS = SoundID.NPCHit4; // new SoundStyle("Everglow/Commons/Weapons/StabbingSwords/StabCollide");
		SoundEngine.PlaySound(sS.WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)).WithVolume(0.3f), Projectile.Center);
		Projectile.soundDelay = SoundTimer * (1 + Projectile.extraUpdates);
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		if (UpdateTimer % (NormalExtraUpdates * Projectile.localNPCHitCooldown) == NormalExtraUpdates / 2)
		{
			if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, new Vector2(targetHitbox.Left + targetHitbox.Width / 2f, targetHitbox.Top + targetHitbox.Height / 2f), 0, 0))
			{
				foreach (DrawParameters draw in DarkDraw)
				{
					Vector2 HitRange = new Vector2(1, 0).RotatedBy(draw.Rotation) * MaxLength * 72;
					if (CollisionUtils.Intersect(targetHitbox.Left(), targetHitbox.Right(), targetHitbox.Height, draw.Postion, draw.Postion + HitRange, draw.Size.Y * 10))
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public override void CutTiles()
	{
		DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
		float cutLength = 164f;
		Vector2 end = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * cutLength * Projectile.scale * MaxLength;
		while (!Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, end, 0, 0))
		{
			cutLength -= 8;
			if (cutLength < 0)
			{
				break;
			}
			end = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * cutLength * Projectile.scale * MaxLength;
		}
		Utils.PlotTileLine(Projectile.Center, end, 80f * Projectile.scale, DelegateMethods.CutTiles);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public struct DrawParameters
	{
		public DrawParameters(Vector2 postion, Color color, float rotation, Vector2 size, SpriteEffects spriteEffect)
		{
			Postion = postion;
			Color = color;
			Rotation = rotation;
			Size = size;
			SpriteEffect = spriteEffect;
		}

		public Vector2 Postion;
		public Color Color;
		public float Rotation;
		public Vector2 Size;
		public SpriteEffects SpriteEffect;
	}

	public DrawParameters ItemDraw = default;

	public void UpdateItemDraw()
	{
		UnifiedRandom rand = Main.rand;
		Vector2 Pos = Projectile.Center - Projectile.rotation.ToRotationVector2() * 2;
		float itemDrawRotation = 8f + MathHelper.Lerp(0f, 20f, rand.NextFloat()) + rand.NextFloat() * 6f;
		float itemAddRotation = Projectile.rotation + rand.NextFloatDirection() * MathF.PI * 2f * 0.04f;
		float itemFinalRotation = itemAddRotation + MathF.PI / 4f;
		Vector2 itemPos = Pos + itemAddRotation.ToRotationVector2() * itemDrawRotation + rand.NextVector2Circular(8f, 8f);
		SpriteEffects itemSpriteEffect = SpriteEffects.None;
		if (Projectile.rotation < -MathF.PI / 2f || Projectile.rotation > MathF.PI / 2f)
		{
			itemFinalRotation += MathF.PI / 2f;
			itemSpriteEffect = itemSpriteEffect | SpriteEffects.FlipHorizontally;
		}
		ItemDraw.Postion = itemPos;
		ItemDraw.Rotation = itemFinalRotation;
		ItemDraw.Size = new Vector2(1f);
		ItemDraw.SpriteEffect = itemSpriteEffect;
	}

	public DrawParameters[] DarkDraw = new DrawParameters[200];

	public void UpdateDarkDraw()
	{
		UnifiedRandom rand = Main.rand;
		Vector2 Pos = Projectile.Center - Projectile.rotation.ToRotationVector2() * 2;
		float rndFloat = rand.NextFloat();
		float lerpedFloat = Utils.GetLerpValue(0f, 0.3f, rndFloat, clamped: true) * Utils.GetLerpValue(1f, 0.5f, rndFloat, clamped: true);
		float lerpedTwice = MathHelper.Lerp(0.6f, 1f, lerpedFloat);

		float rndRange = rand.NextFloat(MaxLength * 0.5f, MaxLength * 1.21f) * 2f;
		float rndDirction = rand.NextFloatDirection();
		float drawRotation = Projectile.rotation + rndDirction * (MathF.PI * 2f) * 0.03f;
		float additiveDrawPos = MaxLength * 15f + MathHelper.Lerp(0f, 50f, rndFloat) + rndRange * 16f;
		Vector2 drawPos = Pos + drawRotation.ToRotationVector2() * additiveDrawPos + rand.NextVector2Circular(20f, 20f);
		bool canHit = Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, drawPos + Vector2.Normalize(Projectile.velocity) * 36f * rndRange * lerpedTwice, 0, 0);

		float volumn = rndRange;
		if (!Main.gamePaused && !canHit)
		{
			while (!canHit)
			{
				volumn *= 0.9f;
				drawPos -= Projectile.velocity * 0.2f;
				if (volumn < 0.3f)
				{
					break;
				}
			}
			HitTileSound(volumn);
		}
		Vector2 drawSize = new Vector2(volumn, DrawWidth) * lerpedTwice;
		for (int f = TradeLength - 1; f >= 0; f--)
		{
			DarkDraw[f].Color.A = (byte)(DarkDraw[f].Color.A * MathF.Pow(FadeShade, 1f / NormalExtraUpdates));
			DarkDraw[f].Size.Y *= MathF.Pow(FadeScale, 1f / NormalExtraUpdates);
		}

		if (UpdateTimer % NormalExtraUpdates == NormalExtraUpdates / 2)
		{
			if (TradeLength > 0)
			{
				for (int f = TradeLength - 1; f > 0; f--)
				{
					DarkDraw[f] = DarkDraw[f - 1];
					DarkDraw[f].Postion = DarkDraw[f - 1].Postion + Main.player[Projectile.owner].velocity;
					DarkDraw[f].Color.A = (byte)(DarkDraw[f - 1].Color.A * MathF.Pow(FadeShade, 1f / NormalExtraUpdates));
					DarkDraw[f].Size.Y = DarkDraw[f - 1].Size.Y * MathF.Pow(FadeScale, 1f / NormalExtraUpdates);
				}
			}
			if (Projectile.timeLeft >= (TradeLength - 1) * (NormalExtraUpdates + 1))
			{
				DarkDraw[0].Color.A = (byte)(TradeShade * 255);
				DarkDraw[0].Postion = drawPos;
				DarkDraw[0].Size = drawSize;
				DarkDraw[0].Rotation = drawRotation;
			}
			else
			{
				DarkDraw[0].Color.A = (byte)(DarkDraw[0].Color.A * MathF.Pow(FadeShade, 1f / NormalExtraUpdates));
				DarkDraw[0].Postion = drawPos + Main.player[Projectile.owner].velocity;
				DarkDraw[0].Size.Y = drawSize.Y * MathF.Pow(FadeScale, 1f / NormalExtraUpdates);
				DarkDraw[0].Rotation = drawRotation;
			}
		}
	}

	public DrawParameters LightDraw = default;

	public void UpdateLightDraw()
	{
		if (UpdateTimer % NormalExtraUpdates == NormalExtraUpdates / 2)
		{
			UnifiedRandom rand = Main.rand;
			Vector2 Pos = Projectile.Center - Projectile.rotation.ToRotationVector2() * 2;
			float rndFloat = rand.NextFloat();
			float lerpedFloat = Utils.GetLerpValue(0f, 0.3f, rndFloat, clamped: true) * Utils.GetLerpValue(1f, 0.5f, rndFloat, clamped: true);
			float lerpedTwice = MathHelper.Lerp(0.6f, 1f, lerpedFloat);

			float rndRange = rand.NextFloat(MaxLength * 0.5f, MaxLength * 1.21f) * 2f;
			float rndDirction = rand.NextFloatDirection();
			float drawRotation = Projectile.rotation + rndDirction * (MathF.PI * 2f) * 0.03f;
			float additiveDrawPos = MaxLength * 15f + MathHelper.Lerp(0f, 50f, rndFloat) + rndRange * 16f;
			Vector2 drawPos = Pos + drawRotation.ToRotationVector2() * additiveDrawPos + rand.NextVector2Circular(20f, 20f);
			while (!Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, drawPos + Vector2.Normalize(Projectile.velocity) * 36f * rndRange * lerpedTwice, 0, 0))
			{
				rndRange *= 0.9f;
				drawPos -= Projectile.velocity * 0.2f;
				if (rndRange < 0.3f)
				{
					break;
				}
			}
			Vector2 drawSize = new Vector2(rndRange, DrawWidth) * lerpedTwice;
			LightDraw.Postion = drawPos;
			LightDraw.Size = drawSize;
			LightDraw.Rotation = drawRotation;
		}
		else
		{
			LightDraw.Color.A = (byte)(LightDraw.Color.A * MathF.Pow(FadeShade, 1f / NormalExtraUpdates));
			LightDraw.Size.Y *= MathF.Pow(FadeScale * 0.2f, 1f / NormalExtraUpdates);
		}
	}

	public virtual void DrawBeforeItem()
	{
	}

	/// <summary>
	/// mulVelocity 决定了旗帜下方两个角收到弹幕速度的影响大小
	/// </summary>
	/// <param name="lightColor"></param>
	/// <param name="offset"></param>
	/// <param name="flagTexture"></param>
	/// <param name="mulVelocityLeft"></param>
	/// <param name="mulVelocityRight"></param>
	public void DrawFlags(Color lightColor, float flagLeftX, float flagTopY, Texture2D flagTexture, float mulVelocityLeft = 1f, float mulVelocityRight = 1f)
	{
		Player player = Main.player[Projectile.owner];
		float flagRightX = flagLeftX + flagTexture.Width;
		Vector2 flagTopLeft = ItemDraw.Postion + new Vector2(flagLeftX, flagTopY).RotatedBy(ItemDraw.Rotation) - Main.screenPosition;
		Vector2 flagTopRight = ItemDraw.Postion + new Vector2(flagRightX, flagTopY).RotatedBy(ItemDraw.Rotation) - Main.screenPosition;
		if (ItemDraw.SpriteEffect == SpriteEffects.FlipHorizontally)
		{
			flagTopLeft = ItemDraw.Postion + new Vector2(-flagRightX, flagTopY).RotatedBy(ItemDraw.Rotation) - Main.screenPosition;
			flagTopRight = ItemDraw.Postion + new Vector2(-flagLeftX, flagTopY).RotatedBy(ItemDraw.Rotation) - Main.screenPosition;
		}
		Vector2 flagBottomLeft = flagTopLeft + new Vector2(0, flagTexture.Height) - Projectile.velocity * mulVelocityLeft - player.velocity;
		Vector2 flagBottomRight = flagTopRight + new Vector2(0, flagTexture.Height) - Projectile.velocity * mulVelocityRight - player.velocity;
		Vector2 deltaBottom = flagBottomRight - flagBottomLeft;
		float velBottomLeft = (deltaBottom.X - flagTexture.Width) * 1f;
		float velBottomRight = -(deltaBottom.X - flagTexture.Width) * 1f;
		flagBottomLeft.X += velBottomLeft;
		flagBottomRight.X += velBottomRight;
		var bars = new List<Vertex2D>();
		bars.Add(new Vertex2D(flagTopLeft, lightColor, new Vector3(0, 0, 0)));
		bars.Add(new Vertex2D(flagTopRight, lightColor, new Vector3(1, 0, 0)));
		bars.Add(new Vertex2D(flagBottomLeft, lightColor, new Vector3(0, 1, 0)));
		bars.Add(new Vertex2D(flagBottomRight, lightColor, new Vector3(1, 1, 0)));
		Main.graphics.GraphicsDevice.Textures[0] = flagTexture;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		}
	}

	public virtual void DrawItem(Color lightColor)
	{
		Player player = Main.player[Projectile.owner];
		Texture2D itemTexture = TextureAssets.Item[player.HeldItem.type].Value;
		Main.spriteBatch.Draw(itemTexture, ItemDraw.Postion - Main.screenPosition, null, lightColor, ItemDraw.Rotation, itemTexture.Size() / 2f, ItemDraw.Size, ItemDraw.SpriteEffect, 0f);
	}

	public virtual void DrawAfterItem()
	{
	}

	public virtual void DrawEffect(Color lightColor)
	{
		Texture2D Shadow = ModAsset.Star2_black.Value;
		Texture2D light = ModAsset.StabbingProjectile.Value;
		Vector2 drawOrigin = light.Size() / 2f;
		Vector2 drawShadowOrigin = Shadow.Size() / 2f;
		if (TradeShade > 0)
		{
			for (int f = TradeLength - 1; f > -1; f--)
			{
				Main.spriteBatch.Draw(Shadow, DarkDraw[f].Postion - Main.screenPosition, null, Color.White * (DarkDraw[f].Color.A / 255f), DarkDraw[f].Rotation, drawShadowOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
				Color fadeLight = Color * (DarkDraw[f].Color.A / 255f);
				fadeLight.A = 0;
				fadeLight = fadeLight * TradeLightColorValue * MathF.Pow(FadeLightColorValue, f);
				fadeLight = new Color(lightColor.R / 255f * fadeLight.R / 255f, lightColor.G / 255f * fadeLight.G / 255f, lightColor.B / 255f * fadeLight.B / 255f, 0);
				Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, fadeLight, DarkDraw[f].Rotation, drawOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
				if (GlowColor != Color.Transparent)
				{
					Main.spriteBatch.Draw(light, DarkDraw[f].Postion - Main.screenPosition, null, GlowColor * MathF.Pow(FadeGlowColorValue, f), DarkDraw[f].Rotation, drawShadowOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
				}
			}
		}
		if (Shade > 0)
		{
			Main.spriteBatch.Draw(Shadow, LightDraw.Postion - Main.screenPosition, null, Color.White * Shade, LightDraw.Rotation, drawShadowOrigin, LightDraw.Size, SpriteEffects.None, 0f);
		}
		Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, new Color(lightColor.R / 255f * Color.R / 255f, lightColor.G / 255f * Color.G / 255f, lightColor.B / 255f * Color.B / 255f, 0), LightDraw.Rotation, drawOrigin, LightDraw.Size, SpriteEffects.None, 0f);
		if (GlowColor != Color.Transparent)
		{
			Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, GlowColor, LightDraw.Rotation, drawShadowOrigin, LightDraw.Size, SpriteEffects.None, 0f);
		}
	}

	public override void PostDraw(Color lightColor)
	{
		DrawBeforeItem();
		DrawItem(lightColor);
		DrawAfterItem();
		DrawEffect(lightColor);
	}

	public void DrawWarp(VFXBatch sb)
	{
		float time = (float)(Main.time * 0.03);
		if (TradeShade > 0)
		{
			for (int f = TradeLength - 1; f > -1; f--)
			{
				Vector2 center = DarkDraw[f].Postion - Main.screenPosition;
				Vector2 normalX = new Vector2(0, 40).RotatedBy(LightDraw.Rotation).RotatedBy(-Math.PI / 2) * LightDraw.Size.X;
				Vector2 normalY = new Vector2(0, 15).RotatedBy(LightDraw.Rotation) * LightDraw.Size.Y;
				Vector2 start = center - normalX * 0.4f;
				Vector2 middle = center;
				Vector2 end = center + normalX;
				Color alphaColor = Color;
				alphaColor.A = 0;
				alphaColor.R = (byte)((DarkDraw[f].Rotation + 6.283 + Math.PI) % 6.283 / 6.283 * 255);
				alphaColor.G = (byte)(DarkDraw[f].Color.A * 0.1f);
				var bars = new List<Vertex2D>
				{
					new Vertex2D(start - normalY, new Color(alphaColor.R, alphaColor.G / 9, 0, 0), new Vector3(1 + time, 0, 0)),
					new Vertex2D(start + normalY, new Color(alphaColor.R, alphaColor.G / 9, 0, 0), new Vector3(1 + time, 1, 0)),
					new Vertex2D(middle - normalY, new Color(alphaColor.R, alphaColor.G / 3, 0, 0), new Vector3(0.5f + time, 0, 0.5f)),
					new Vertex2D(middle + normalY, new Color(alphaColor.R, alphaColor.G / 3, 0, 0), new Vector3(0.5f + time, 1, 0.5f)),
					new Vertex2D(end, alphaColor, new Vector3(0f + time, 0.5f, 1)),
					new Vertex2D(end, alphaColor, new Vector3(0f + time, 0.5f, 1)),
				};
				sb.Draw(ModAsset.Trail_1.Value, bars, PrimitiveType.TriangleStrip);
			}
		}
		if (TradeShade > 0)
		{
			Vector2 center = LightDraw.Postion - Main.screenPosition;
			Vector2 normalX = new Vector2(0, 45).RotatedBy(LightDraw.Rotation).RotatedBy(-Math.PI / 2) * LightDraw.Size.X;
			Vector2 normalY = new Vector2(0, 20).RotatedBy(LightDraw.Rotation) * LightDraw.Size.Y;
			Vector2 start = center - normalX * 0.4f;
			Vector2 middle = center;
			Vector2 end = center + normalX;
			Color alphaColor = Color;
			alphaColor.A = 0;
			alphaColor.R = (byte)((LightDraw.Rotation + 6.283 + Math.PI) % 6.283 / 6.283 * 255);
			alphaColor.G = 20;
			var bars = new List<Vertex2D>
			{
				new Vertex2D(start - normalY, new Color(alphaColor.R, alphaColor.G / 9, 0, 0), new Vector3(1 + time, 0, 0)),
				new Vertex2D(start + normalY, new Color(alphaColor.R, alphaColor.G / 9, 0, 0), new Vector3(1 + time, 1, 0)),
				new Vertex2D(middle - normalY, new Color(alphaColor.R, alphaColor.G / 3, 0, 0), new Vector3(0.5f + time, 0, 0.5f)),
				new Vertex2D(middle + normalY, new Color(alphaColor.R, alphaColor.G / 3, 0, 0), new Vector3(0.5f + time, 1, 0.5f)),
				new Vertex2D(end, alphaColor, new Vector3(0f + time, 0.5f, 1)),
				new Vertex2D(end, alphaColor, new Vector3(0f + time, 0.5f, 1)),
			};
			sb.Draw(ModAsset.Trail_1.Value, bars, PrimitiveType.TriangleStrip);
		}
	}
}