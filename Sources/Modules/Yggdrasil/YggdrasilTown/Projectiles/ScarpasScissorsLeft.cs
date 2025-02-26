using Terraria.Audio;
using Terraria.DataStructures;
using static Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.ScarpasScissors;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class ScarpasScissorsLeft : ScarpasScissorsProjBase
{
	protected virtual bool Left => true;

	private ref float Timer => ref Projectile.ai[1];

	private float CutTime => 64f / Owner.GetTotalAttackSpeed(Projectile.DamageType);

	public override void SetDefaults()
	{
		Projectile.width = 44;
		Projectile.height = 44;

		Projectile.timeLeft = 10000;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;

		Projectile.DamageType = DamageClass.Melee;
		Projectile.friendly = true;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 10;
		Projectile.ownerHitCheck = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.spriteDirection = Main.MouseWorld.X > Owner.MountedCenter.X ? 1 : -1;

		Projectile.rotation = (Main.MouseWorld - Owner.Center).ToRotation();
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		Vector2 start = Owner.MountedCenter;
		Vector2 end = start + Projectile.rotation.ToRotationVector2() * (Projectile.Size.Length() * Projectile.scale);
		float collisionPoint = 0f;
		return Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), start, end, 15f * Projectile.scale, ref collisionPoint);
	}

	public override void AI()
	{
		if (!HeldProjAI())
		{
			return;
		}

		AttackAI();

		Timer++;
	}

	private bool HeldProjAI()
	{
		Owner.itemAnimation = 2;
		Owner.itemTime = 2;
		Owner.heldProj = Projectile.whoAmI;

		if (!Owner.active || Owner.dead || Owner.noItems || Owner.CCed)
		{
			Projectile.Kill();
			return false;
		}

		return true;
	}

	private void AttackAI()
	{
		if (Timer > CutTime)
		{
			Projectile.Kill();
		}

		SetWeaponPosition_Cut();

		if (CutRotation() < 0f && Left)
		{
			Dust.NewDust(Projectile.Center + Projectile.rotation.ToRotationVector2() * 40f, 1, 1, DustID.Iron, Scale: 0.4f);
			SoundEngine.PlaySound(SoundID.Item1, Projectile.Center);
		}
	}

	public void SetWeaponPosition_Cut()
	{
		var baseRotation = Projectile.rotation;
		float rotationOffset = Projectile.spriteDirection > 0 ? MathHelper.ToRadians(45f) : MathHelper.ToRadians(135f);
		baseRotation -= rotationOffset;
		baseRotation -= 0.6f * Owner.direction;
		Owner.SetCompositeArmFront(true, Player.CompositeArmStretchAmount.Full, baseRotation + CutRotation() + 0.4f);
		Owner.SetCompositeArmBack(true, Player.CompositeArmStretchAmount.Full, baseRotation - CutRotation() - 0.8f);

		Vector2 armPosition = Owner.GetFrontHandPosition(Player.CompositeArmStretchAmount.Full, Projectile.rotation - (float)Math.PI / 2);
		armPosition = Owner.Center;
		armPosition.Y += Owner.gfxOffY;

		Projectile.Center = armPosition; // Set projectile to arm position
		Projectile.scale = Owner.GetAdjustedItemScale(Owner.HeldItem); // Slightly scale up the projectile and also take into account melee size modifiers
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var drawColor = lightColor * Projectile.Opacity;
		var distanceToArmCenter = 18f;
		var position = Projectile.Center - Main.screenPosition + Projectile.rotation.ToRotationVector2() * distanceToArmCenter;

		var leftTexture = ModAsset.ScarpasScissorsLeft.Value;
		var leftOrigin = new Vector2(22.5f, 32.5f);
		var rightTexture = ModAsset.ScarpasScissorsRight.Value;
		var rightOrigin = new Vector2(10.5f, 32.5f);

		float rotationOffset = MathHelper.ToRadians(45f);
		SpriteEffects effects = SpriteEffects.None;

		if (Projectile.spriteDirection <= 0)
		{
			leftOrigin.X = leftTexture.Width - leftOrigin.X;
			rightOrigin.X = rightTexture.Width - rightOrigin.X;
			rotationOffset = MathHelper.ToRadians(135f);
			effects = SpriteEffects.FlipHorizontally;
		}

		var leftRotation = rotationOffset + Projectile.spriteDirection * CutRotation();
		var rightRotation = rotationOffset - Projectile.spriteDirection * CutRotation();

		if (Left)
		{
			Main.spriteBatch.Draw(leftTexture, position, default, drawColor, Projectile.rotation + leftRotation, leftOrigin, Projectile.scale, effects, 0);
		}
		else
		{
			Main.spriteBatch.Draw(rightTexture, position, default, drawColor, Projectile.rotation + rightRotation, rightOrigin, Projectile.scale, effects, 0);
		}

		return false;
	}

	public override bool? CanHitNPC(NPC target) => CutRotation() < 0.1f;

	public float CutRotation()
	{
		var progress = Timer / (float)CutTime;
		var timeValue = MathF.Pow(progress, 2) * 24;
		var waveValue = MathF.Pow(MathF.Sin(timeValue) + 1f, 2) - 0.05f;
		var angle = 0.2f;
		return waveValue * angle;
	}

	public override void SendExtraAI(BinaryWriter writer)
	{
		writer.Write((sbyte)Projectile.spriteDirection);
	}

	public override void ReceiveExtraAI(BinaryReader reader)
	{
		Projectile.spriteDirection = reader.ReadSByte();
	}
}