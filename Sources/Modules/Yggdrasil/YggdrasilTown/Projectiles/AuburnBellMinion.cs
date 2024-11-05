using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.YggdrasilTown.Buffs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class AuburnBellMinion : ModProjectile
{
	private const float NotMovingVelocity = 1E-05f;
	private const int NoTarget = -1;
	private const float MAXDistanceToOwner = 1000f;
	private const int TeleportCooldownValue = 60;
	private const int SearchDistance = 1000;

	private int MinionNumber => (int)Projectile.ai[0];

	private Player Owner => Main.player[Projectile.owner];

	private int targetWhoAmI = NoTarget;

	private int TargetWhoAmI
	{
		get => targetWhoAmI;
		set => targetWhoAmI = value;
	}

	private int TeleportCooldown { get; set; } = 0;

	public override void SetStaticDefaults()
	{
		Main.projPet[Projectile.type] = true;
		Main.projFrames[Projectile.type] = 8;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 2;
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5;
		ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
	}

	public override void SetDefaults()
	{
		Projectile.tileCollide = false;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = 30;
		Projectile.timeLeft = 18000;

		Projectile.penetrate = -1;
		Projectile.aiStyle = -1;
		Projectile.width = 70;
		Projectile.height = 10;

		Projectile.DamageType = DamageClass.Summon;
		Projectile.minion = true;
		Projectile.minionSlots = 1;

		Projectile.netImportant = true;
	}

	public override bool? CanCutTiles() => false;

	public override bool MinionContactDamage() => true;

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI) => behindNPCsAndTiles.Add(index);

	public override void AI()
	{
		Projectile.frame = ++Projectile.frame % Main.projFrames[Projectile.type];

		if (!CheckOwnerActive())
		{
			return;
		}

		if (TeleportCooldown > 0)
		{
			TeleportCooldown--;
		}
		else if (CheckDistanceToOwner())
		{
			TargetWhoAmI = NoTarget;
			TelePortTo(Owner.MountedCenter + new Vector2((10 - MinionNumber * 30) * Owner.direction, -40 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f - MinionNumber) * 35f));
			return;
		}

		if (TargetWhoAmI == NoTarget || !CheckTargetActive())
		{
			GeneralBehavior();
			SearchTarget();
		}
		else
		{
			Attack();
		}
	}

	private bool CheckOwnerActive()
	{
		if (Owner.dead || Owner.active is false)
		{
			Owner.ClearBuff(ModContent.BuffType<AuburnBell>());
			return false;
		}

		if (Owner.HasBuff(ModContent.BuffType<AuburnBell>()))
		{
			Owner.AddBuff(ModContent.BuffType<AuburnBell>(), 3600);
			Projectile.timeLeft = 2;
		}

		return true;
	}

	private bool CheckDistanceToOwner() => Projectile.Center.Distance(Owner.Center) > MAXDistanceToOwner;

	private void TelePortTo(Vector2 aim)
	{
		TeleportCooldown = TeleportCooldownValue;
		Projectile.Center = aim;
	}

	private void MoveTo(Vector2 aim)
	{
		float timeValue = (float)(Main.time * 0.014f);

		Projectile.velocity *= 0.97f;

		var newRotation = MathF.Log(MathF.Abs(Projectile.velocity.X) + 1) * 0.2f * Projectile.direction;
		Projectile.rotation = Projectile.rotation * 0.95f + newRotation * 0.05f;

		int dirY = Projectile.velocity.Y >= 0 ? 1 : -1;
		Vector2 aimPosition =
			aim +
			new Vector2(
				210f * MathF.Sin(timeValue * 2 + Projectile.whoAmI) * Projectile.direction,
				(-50 + 30f * MathF.Sin(timeValue * 0.15f + Projectile.whoAmI)) * dirY)
			* Projectile.scale;
		Vector2 toAim = aimPosition - Projectile.Center - Projectile.velocity;
		if (toAim.Length() > 50)
		{
			Projectile.velocity += Vector2.Normalize(toAim) * 0.15f * Projectile.scale;
		}
	}

	private void SearchTarget()
	{
		Projectile.Minion_FindTargetInRange(SearchDistance, ref targetWhoAmI, false);
	}

	private bool CheckTargetActive()
	{
		if (TargetWhoAmI == NoTarget)
		{
			return false;
		}

		NPC target = Main.npc[TargetWhoAmI];
		if (!target.active || target.dontTakeDamage)
		{
			TargetWhoAmI = NoTarget;
			return false;
		}

		return true;
	}

	private void Attack()
	{
		NPC target = Main.npc[TargetWhoAmI];

		MoveTo(target.Center);
	}

	private void GeneralBehavior()
	{
		Vector2 aim;

		if (Owner.velocity.Length() > NotMovingVelocity) // Player is moving
		{
			aim = Owner.MountedCenter
				+ new Vector2(
					x: (10 - MinionNumber * 30) * Owner.direction,
					y: -40 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f - MinionNumber) * 35f);
		}
		else
		{
			aim = Owner.MountedCenter
				+ new Vector2(
					x: Owner.direction * (MathF.Sin((float)Main.timeForVisualEffects * 0.02f) * 40f - MinionNumber * 30),
					y: -50 + MathF.Sin((float)Main.timeForVisualEffects * 0.04f) * 20f);
		}

		MoveTo(aim);
		Projectile.rotation = Projectile.rotation * 0.95f + Projectile.velocity.X * 0.002f;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		int BodyLength = 12;

		SpriteBatch spriteBatch = Main.spriteBatch;

		Texture2D texture = (Texture2D)ModContent.Request<Texture2D>(Texture);

		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(
			SpriteSortMode.Deferred,
			BlendState.NonPremultiplied,
			Main.DefaultSamplerState,
			DepthStencilState.None,
			RasterizerState.CullNone,
			null,
			Main.GameViewMatrix.TransformationMatrix);

		List<Vertex2D> bars = new List<Vertex2D>();
		Vector2 drawCenter = Projectile.Center - Main.screenPosition + new Vector2(BodyLength * 5f * 0.75f, 0).RotatedBy(Projectile.rotation) * Projectile.scale;
		for (int i = 0; i < BodyLength; i++)
		{
			Color drawColor = Lighting.GetColor(Projectile.Center.ToTileCoordinates());
			float jointIndex = i / (float)BodyLength;
			int frameY = (int)(Projectile.frame + i) % Main.projFrames[Projectile.type];
			float frameYValue = frameY / (float)Main.projFrames[Projectile.type];
			float deltaYValue = 1 / (float)Main.projFrames[Projectile.type];
			float jointScale = 0.5f + 0.5f * MathF.Sin(jointIndex * MathHelper.Pi);

			bars.Add(
				drawCenter + new Vector2(-20, -20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(0, frameYValue, 0));
			bars.Add(
				drawCenter + new Vector2(-20, 20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(0, frameYValue + deltaYValue, 0));
			bars.Add(
				drawCenter + new Vector2(20, -20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(1, frameYValue, 0));
			bars.Add(
				drawCenter + new Vector2(20, -20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(1, frameYValue, 0));
			bars.Add(
				drawCenter + new Vector2(-20, 20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(0, frameYValue + deltaYValue, 0));
			bars.Add(
				drawCenter + new Vector2(20, 20).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale,
				drawColor,
				new Vector3(1, frameYValue + deltaYValue, 0));

			drawCenter -= new Vector2(10, 0).RotatedBy(Projectile.rotation) * jointScale * Projectile.scale;
		}

		Main.graphics.GraphicsDevice.RasterizerState = RasterizerState.CullNone;
		Main.graphics.GraphicsDevice.Textures[0] = texture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.PointWrap;
		if (bars.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count / 3);
		}

		spriteBatch.End();
		spriteBatch.Begin(sBS);

		return false;
	}
}