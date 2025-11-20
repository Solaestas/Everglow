using Everglow.Commons.MEAC;
using Everglow.Commons.Templates.Weapons.StabbingSwords.VFX;
using Everglow.Commons.TileHelper;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;
using Terraria.Utilities;

namespace Everglow.Commons.Templates.Weapons.StabbingSwords;

/// <summary>
/// Principle : generate <see cref="DrawParameters_Structure"/> continuously as an attack unit, with random direction and scale in a certain range.<br/>
/// Attack unit will generate as <see cref="LightDraw"/> at first, then turn to <see cref="DarkDraw"/>.<br/>
/// Attack units in <see cref="DarkDraw"/> will fade per update, and kill after <see cref="MaxOldAttackUnitCount"/> times.
/// </summary>
public abstract class StabbingProjectile : ModProjectile, IWarpProjectile
{
	/// <summary>
	/// Default ExtraUpdates | 默认额外刷新次数
	/// </summary>
	public const int NormalExtraUpdates = 20;

	/// <summary>
	/// Main color | 主要颜色
	/// </summary>
	public Color AttackColor = Color.White;

	/// <summary>
	/// Shadow intensity of first attack unit(<see cref="LightDraw"/>) | 首个攻击单元阴影强度
	/// </summary>
	public float Shade = 0f;

	/// <summary>
	/// Shadow intensity of old attack units(<see cref="DarkDraw"/>) | 旧攻击单元阴影强度
	/// </summary>
	public float OldShade = 0f;

	/// <summary>
	/// Color(RGB) illumination coefficient of old attack units | 旧攻击单元RGB亮度系数
	/// </summary>
	public float OldLightColorValue = 0f;

	/// <summary>
	/// Amount of old attack units (Length of<see cref="DarkDraw"/>), default to 4; Warning : The projectile will keep active until old attack units run out | 最大旧攻击单元数，默认4; 警告：射弹会一直存在直到旧攻击单元耗尽
	/// </summary>
	public int MaxOldAttackUnitCount = 0;

	/// <summary>
	/// Scale of old attack units will multiply this per update, no more than 1.0f | 旧攻击单元大小每次更新倍率, 不大于1.0f
	/// </summary>
	public float ScaleMultiplicative_Modifier = 0f;

	/// <summary>
	/// Light effect width coefficient of an attack unit， default to 1.0f | 攻击单元刀光宽度系数, 默认1.0f
	/// </summary>
	public float AttackEffectWidth = 1f;

	/// <summary>
	/// Shadow intensity of old attack units will multiply this per update, no more than 1.0f | 旧攻击单元阴影强度每次更新倍率, 不大于1.0f
	/// </summary>
	public float ShadeMultiplicative_Modifier = 0f;

	/// <summary>
	/// Color(RGB) illumination of old attack units will multiply this per update, no more than 1.0f | 旧攻击单元RGB色彩每次更新倍率, 不大于1.0f
	/// </summary>
	public float LightColorValueMultiplicative_Modifier = 0f;

	/// <summary>
	/// Light effect length coefficient of an attack unit， default to 1.0f (1.0 * 72 = 72) | 攻击单元刀光长度系数, 默认1.0f (1.0 * 72 = 72)
	/// </summary>
	public float AttackLength = 1f;

	/// <summary>
	/// Glow color, no affected by environment, default to <see cref="Color.Transparent"/> | 荧光颜色，不受环境影响，默认无色
	/// </summary>
	public Color GlowColor = Color.Transparent;

	/// <summary>
	/// Glow color(RGB) of old attack units will multiply this per update, no more than 1.0f | 旧攻击单元荧光颜色每次更新倍率, 不大于1.0f
	/// </summary>
	public float FadeGlowColorValue = 0f;

	public Player Owner => Main.player[Projectile.owner];

	public override string Texture => ModAsset.StabbingProjectile_Mod;

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
		SetCustomDefaults();
	}

	public virtual void SetCustomDefaults()
	{
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
		Projectile.extraUpdates = (int)(NormalExtraUpdates * (Owner.meleeSpeed + 1));
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
			SoundEngine.PlaySound(ss.WithPitchOffset(Owner.meleeSpeed), Projectile.Center);
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
				Projectile.timeLeft = MaxOldAttackUnitCount * (NormalExtraUpdates + 1);
			}
		}

		if (!Owner.controlUseItem && Projectile.timeLeft > MaxOldAttackUnitCount * (NormalExtraUpdates + 1))
		{
			Projectile.timeLeft = MaxOldAttackUnitCount * (NormalExtraUpdates + 1);
		}

		if (Owner.HeldItem.ModItem is StabbingSwordItem modItem)
		{
			if (!Owner.GetModPlayer<StabbingSwordStaminaPlayer>().CheckStamina(modItem.StaminaCost / Projectile.extraUpdates))
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
				foreach (DrawParameters_Structure draw in DarkDraw)
				{
					Vector2 HitRange = new Vector2(1, 0).RotatedBy(draw.Rotation) * AttackLength * 72;
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
		Vector2 end = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * cutLength * Projectile.scale * AttackLength;
		while (!Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, end, 0, 0))
		{
			cutLength -= 8;
			if (cutLength < 0)
			{
				break;
			}
			end = Projectile.Center + Projectile.velocity.SafeNormalize(Vector2.UnitX) * cutLength * Projectile.scale * AttackLength;
		}
		Utils.PlotTileLine(Projectile.Center, end, 80f * Projectile.scale, DelegateMethods.CutTiles);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		return false;
	}

	public struct DrawParameters_Structure
	{
		public DrawParameters_Structure(Vector2 postion, Color color, float rotation, Vector2 size, SpriteEffects spriteEffect)
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

	public DrawParameters_Structure ItemDraw = default;

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

	public DrawParameters_Structure[] DarkDraw = new DrawParameters_Structure[200];

	public void UpdateDarkDraw()
	{
		UnifiedRandom rand = Main.rand;
		Vector2 Pos = Projectile.Center - Projectile.rotation.ToRotationVector2() * 2;
		float rndFloat = rand.NextFloat();
		float lerpedFloat = Utils.GetLerpValue(0f, 0.3f, rndFloat, clamped: true) * Utils.GetLerpValue(1f, 0.5f, rndFloat, clamped: true);
		float lerpedTwice = MathHelper.Lerp(0.6f, 1f, lerpedFloat);

		float rndRange = rand.NextFloat(AttackLength * 0.5f, AttackLength * 1.21f) * 2f;
		float rndDirction = rand.NextFloatDirection();
		float drawRotation = Projectile.rotation + rndDirction * (MathF.PI * 2f) * 0.03f;
		float additiveDrawPos = AttackLength * 15f + MathHelper.Lerp(0f, 50f, rndFloat) + rndRange * 16f;
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
		Vector2 drawSize = new Vector2(volumn, AttackEffectWidth) * lerpedTwice;
		for (int f = MaxOldAttackUnitCount - 1; f >= 0; f--)
		{
			DarkDraw[f].Color.A = (byte)(DarkDraw[f].Color.A * MathF.Pow(ShadeMultiplicative_Modifier, 1f / NormalExtraUpdates));
			DarkDraw[f].Size.Y *= MathF.Pow(ScaleMultiplicative_Modifier, 1f / NormalExtraUpdates);
		}

		if (UpdateTimer % NormalExtraUpdates == NormalExtraUpdates / 2)
		{
			if (MaxOldAttackUnitCount > 0)
			{
				for (int f = MaxOldAttackUnitCount - 1; f > 0; f--)
				{
					DarkDraw[f] = DarkDraw[f - 1];
					DarkDraw[f].Postion = DarkDraw[f - 1].Postion + Main.player[Projectile.owner].velocity;
					DarkDraw[f].Color.A = (byte)(DarkDraw[f - 1].Color.A * MathF.Pow(ShadeMultiplicative_Modifier, 1f / NormalExtraUpdates));
					DarkDraw[f].Size.Y = DarkDraw[f - 1].Size.Y * MathF.Pow(ScaleMultiplicative_Modifier, 1f / NormalExtraUpdates);
				}
			}
			if (Projectile.timeLeft >= (MaxOldAttackUnitCount - 1) * (NormalExtraUpdates + 1))
			{
				DarkDraw[0].Color.A = (byte)(OldShade * 255);
				DarkDraw[0].Postion = drawPos;
				DarkDraw[0].Size = drawSize;
				DarkDraw[0].Rotation = drawRotation;
			}
			else
			{
				DarkDraw[0].Color.A = (byte)(DarkDraw[0].Color.A * MathF.Pow(ShadeMultiplicative_Modifier, 1f / NormalExtraUpdates));
				DarkDraw[0].Postion = drawPos + Main.player[Projectile.owner].velocity;
				DarkDraw[0].Size.Y = drawSize.Y * MathF.Pow(ScaleMultiplicative_Modifier, 1f / NormalExtraUpdates);
				DarkDraw[0].Rotation = drawRotation;
			}
		}
	}

	public DrawParameters_Structure LightDraw = default;

	public void UpdateLightDraw()
	{
		if (UpdateTimer % NormalExtraUpdates == NormalExtraUpdates / 2)
		{
			UnifiedRandom rand = Main.rand;
			Vector2 Pos = Projectile.Center - Projectile.rotation.ToRotationVector2() * 2;
			float rndFloat = rand.NextFloat();
			float lerpedFloat = Utils.GetLerpValue(0f, 0.3f, rndFloat, clamped: true) * Utils.GetLerpValue(1f, 0.5f, rndFloat, clamped: true);
			float lerpedTwice = MathHelper.Lerp(0.6f, 1f, lerpedFloat);

			float rndRange = rand.NextFloat(AttackLength * 0.5f, AttackLength * 1.21f) * 2f;
			float rndDirction = rand.NextFloatDirection();
			float drawRotation = Projectile.rotation + rndDirction * (MathF.PI * 2f) * 0.03f;
			float additiveDrawPos = AttackLength * 15f + MathHelper.Lerp(0f, 50f, rndFloat) + rndRange * 16f;
			Vector2 drawPos = Pos + drawRotation.ToRotationVector2() * additiveDrawPos + rand.NextVector2Circular(20f, 20f);
			bool collided = true;
			float distanceCheck = 0f;
			while (!SolidTileButNotSolidTop(drawPos + new Vector2(1, 0).RotatedBy(drawRotation) * 36f * distanceCheck * lerpedTwice))
			{
				distanceCheck += 0.03f;
				if (distanceCheck >= rndRange)
				{
					collided = false;
					break;
				}
			}
			if (collided)
			{
				if (distanceCheck > 0)
				{
					HitTileEffect(drawPos + new Vector2(1, 0).RotatedBy(drawRotation) * 36f * distanceCheck * lerpedTwice, drawRotation, rndRange - distanceCheck);
				}
				rndRange = distanceCheck;
			}
			Vector2 drawSize = new Vector2(rndRange, AttackEffectWidth) * lerpedTwice;
			LightDraw.Postion = drawPos;
			LightDraw.Size = drawSize;
			LightDraw.Rotation = drawRotation;
		}
		else
		{
			LightDraw.Color.A = (byte)(LightDraw.Color.A * MathF.Pow(ShadeMultiplicative_Modifier, 1f / NormalExtraUpdates));
			LightDraw.Size.Y *= MathF.Pow(ScaleMultiplicative_Modifier * 0.2f, 1f / NormalExtraUpdates);
		}
	}

	public virtual void HitTileEffect(Vector2 hitPosition, float rotation, float power)
	{
		var hitSparkFixed = new StabbingProjectile_HitEffect()
		{
			Active = true,
			Visible = true,
			Position = hitPosition,
			MaxTime = 8,
			Scale = 0.12f * power,
			Rotation = rotation,
			Color = new Color(1f, 0.45f, 0.05f, 0),
		};
		Ins.VFXManager.Add(hitSparkFixed);
		Vector2 tilePos = hitPosition + new Vector2(1, 0).RotatedBy(rotation);
		Point tileCoord = tilePos.ToTileCoordinates();
		Tile tile = WorldGenMisc.SafeGetTile(tileCoord);
		if (TileClassification.StabbingSwordFragileTileType.Contains(tile.TileType))
		{
			WorldGenMisc.DamageTile(tileCoord, 40, Owner);
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
		if (OldShade > 0)
		{
			for (int f = MaxOldAttackUnitCount - 1; f > -1; f--)
			{
				Main.spriteBatch.Draw(Shadow, DarkDraw[f].Postion - Main.screenPosition, null, Color.White * (DarkDraw[f].Color.A / 255f), DarkDraw[f].Rotation, drawShadowOrigin, DarkDraw[f].Size, SpriteEffects.None, 0f);
				Color fadeLight = AttackColor * (DarkDraw[f].Color.A / 255f);
				fadeLight.A = 0;
				fadeLight = fadeLight * OldLightColorValue * MathF.Pow(LightColorValueMultiplicative_Modifier, f);
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
		Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, new Color(lightColor.R / 255f * AttackColor.R / 255f, lightColor.G / 255f * AttackColor.G / 255f, lightColor.B / 255f * AttackColor.B / 255f, 0), LightDraw.Rotation, drawOrigin, LightDraw.Size, SpriteEffects.None, 0f);
		if (GlowColor != Color.Transparent)
		{
			Main.spriteBatch.Draw(light, LightDraw.Postion - Main.screenPosition, null, GlowColor, LightDraw.Rotation, drawShadowOrigin, LightDraw.Size, SpriteEffects.None, 0f);
		}

		// Debug Codes.
		// Texture2D mark = ModAsset.White.Value;
		// Main.spriteBatch.Draw(mark, LightDraw.Postion + new Vector2(1, 0).RotatedBy(LightDraw.Rotation) * 36f * LightDraw.Size.X - Main.screenPosition, null, new Color(1f, 0f, 0, 1), 0, mark.Size() * 0.5f, 0.05f, SpriteEffects.None, 0f);
		// Main.spriteBatch.Draw(mark, LightDraw.Postion + new Vector2(-1, 0).RotatedBy(LightDraw.Rotation) * 36f * LightDraw.Size.X - Main.screenPosition, null, new Color(1f, 0f, 0, 1), 0, mark.Size() * 0.5f, 0.05f, SpriteEffects.None, 0f);
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
		if (OldShade > 0)
		{
			for (int f = MaxOldAttackUnitCount - 1; f > -1; f--)
			{
				Vector2 center = DarkDraw[f].Postion - Main.screenPosition;
				Vector2 normalX = new Vector2(0, 40).RotatedBy(LightDraw.Rotation).RotatedBy(-Math.PI / 2) * LightDraw.Size.X;
				Vector2 normalY = new Vector2(0, 15).RotatedBy(LightDraw.Rotation) * LightDraw.Size.Y;
				Vector2 start = center - normalX * 0.4f;
				Vector2 middle = center;
				Vector2 end = center + normalX;
				Color alphaColor = AttackColor;
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
		if (OldShade > 0)
		{
			Vector2 center = LightDraw.Postion - Main.screenPosition;
			Vector2 normalX = new Vector2(0, 45).RotatedBy(LightDraw.Rotation).RotatedBy(-Math.PI / 2) * LightDraw.Size.X;
			Vector2 normalY = new Vector2(0, 20).RotatedBy(LightDraw.Rotation) * LightDraw.Size.Y;
			Vector2 start = center - normalX * 0.4f;
			Vector2 middle = center;
			Vector2 end = center + normalX;
			Color alphaColor = AttackColor;
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

	public static bool SolidTileButNotSolidTop(Vector2 worldPosition)
	{
		Tile tile = WorldGenMisc.SafeGetTile_WorldCoord(worldPosition);
		bool solidTop = Main.tileSolidTop[tile.TileType];
		return Collision.IsWorldPointSolid(worldPosition) && !solidTop;
	}
}