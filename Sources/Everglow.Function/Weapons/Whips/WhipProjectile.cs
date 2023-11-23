using Terraria.Audio;
using Terraria.Enums;
using Terraria.GameContent;

namespace Everglow.Commons.Weapons.Whips;

public abstract class WhipProjectile : ModProjectile
{
	public List<Vector2> WhipPointsForCollision = new List<Vector2>();

	public override void SetDefaults()
	{
		DefaultToWhip();
		SetDef();
	}
	public virtual void SetDef()
	{

	}
	public void DefaultToWhip()
	{
		Projectile.width = 18;
		Projectile.height = 18;
		Projectile.aiStyle = -1;//165
		Projectile.friendly = true;
		Projectile.penetrate = -1;
		Projectile.tileCollide = false;
		Projectile.scale = 1f;
		Projectile.ownerHitCheck = true;
		Projectile.extraUpdates = 1;
		Projectile.usesLocalNPCImmunity = true;
		Projectile.localNPCHitCooldown = -1;
		Projectile.DamageType = DamageClass.Summon;
	}
	public override void CutTiles()
	{
		var value = new Vector2(Projectile.width * Projectile.scale / 2f, 0f);
		for (int i = 0; i < WhipPointsForCollision.Count; i++)
		{
			DelegateMethods.tilecut_0 = TileCuttingContext.AttackProjectile;
			Utils.PlotTileLine(WhipPointsForCollision[i] - value, WhipPointsForCollision[i] + value, Projectile.height * Projectile.scale, new Utils.TileActionAttempt(DelegateMethods.CutTiles));
		}
		base.CutTiles();
	}
	public override bool? CanCutTiles()
	{
		return true;
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		for (int n = 0; n < WhipPointsForCollision.Count; n++)
		{
			var point = WhipPointsForCollision[n].ToPoint();
			projHitbox.Location = new Point(point.X - projHitbox.Width / 2, point.Y - projHitbox.Height / 2);
			if (projHitbox.Intersects(targetHitbox))
			{
				return true;
			}
		}
		return false;
	}
	public override void AI()
	{
		AI_165_Whip();
		WhipPointsForCollision.Clear();
		FillWhipControlPoints(Projectile, WhipPointsForCollision);
		return;
	}
	private void AI_165_Whip()
	{
		Player player = Main.player[Projectile.owner];
		Projectile.rotation = Projectile.velocity.ToRotation() + 1.5707964f;
		Projectile.ai[0] += 1f;
		float num;
		int num2;
		float num3;
		GetWhipSettings(Projectile, out num, out num2, out num3);
		Projectile.tileCollide = false;
		Projectile.Center = Main.GetPlayerArmPosition(Projectile) + Projectile.velocity * (Projectile.ai[0] - 1f);
		Projectile.spriteDirection = Vector2.Dot(Projectile.velocity, Vector2.UnitX) < 0f ? -1 : 1;
		if (Projectile.ai[0] >= num || player.itemAnimation == 0)
		{
			Projectile.Kill();
			return;
		}
		player.heldProj = Projectile.whoAmI;
		player.itemAnimation = player.itemAnimationMax - (int)(Projectile.ai[0] / Projectile.MaxUpdates);
		player.itemTime = player.itemAnimation;
		if (Projectile.ai[0] == (int)(num / 2f))
		{
			WhipPointsForCollision.Clear();
			FillWhipControlPoints(Projectile, WhipPointsForCollision);
			Vector2 position = WhipPointsForCollision[WhipPointsForCollision.Count - 1];
			SoundEngine.PlaySound(SoundID.Item153, position);
		}
		float t = Projectile.ai[0] / num;
		if (Utils.GetLerpValue(0.1f, 0.7f, t, true) * Utils.GetLerpValue(0.9f, 0.7f, t, true) > 0.5f && Main.rand.Next(3) != 0)
		{
			WhipPointsForCollision.Clear();
			FillWhipControlPoints(Projectile, WhipPointsForCollision);
			int num5 = Main.rand.Next(WhipPointsForCollision.Count - 10, WhipPointsForCollision.Count);
			Rectangle rectangle = Utils.CenteredRectangle(WhipPointsForCollision[num5], new Vector2(30f, 30f));
			var dust = Dust.NewDustDirect(rectangle.TopLeft(), rectangle.Width, rectangle.Height, DustID.Bone, 0f, 0f, 100, Color.White, 1f);
			dust.position = WhipPointsForCollision[num5];
			dust.fadeIn = 0.3f;
			Vector2 spinningpoint = WhipPointsForCollision[num5] - WhipPointsForCollision[num5 - 1];
			dust.noGravity = true;
			dust.velocity *= 0.5f;
			dust.velocity += spinningpoint.RotatedBy((double)(player.direction * 1.5707964f), default);
			dust.velocity *= 0.5f;
		}
	}
	public static void GetWhipSettings(Projectile proj, out float timeToFlyOut, out int segments, out float rangeMultiplier)
	{
		timeToFlyOut = Main.player[proj.owner].itemAnimationMax * proj.MaxUpdates;
		rangeMultiplier = 1f;
		segments = 20;
		rangeMultiplier *= 1.2f;//鞭长
	}
	public static void FillWhipControlPoints(Projectile proj, List<Vector2> controlPoints)
	{
		float timeToFlyOut;
		int segments;
		float rangeMultiplier;
		GetWhipSettings(proj, out timeToFlyOut, out segments, out rangeMultiplier);
		float duration = proj.ai[0] / timeToFlyOut;
		float duration15 = 1.5f;
		float maxRotation = MathHelper.Pi * 10f * (1f - duration * duration15) * -proj.spriteDirection / segments;
		float durationSquare15 = duration * duration15;
		float factor = 0f;
		if (durationSquare15 > 1f)
		{
			factor = (durationSquare15 - 1f) / 0.5f;
			durationSquare15 = MathHelper.Lerp(1f, 0f, factor);
		}
		Player player = Main.player[proj.owner];
		Item heldItem = player.HeldItem;
		float durationValue = heldItem.useAnimation * 2 * duration * player.whipRangeMultiplier;
		durationValue = proj.velocity.Length() * durationValue * durationSquare15 * rangeMultiplier / segments;
		Vector2 startArmPosition = Main.GetPlayerArmPosition(proj);
		Vector2 vector = startArmPosition;
		float num14 = -MathHelper.PiOver2;
		Vector2 value = vector;
		float num15 = MathHelper.PiOver2 + MathHelper.PiOver2 * proj.spriteDirection;
		Vector2 value2 = vector;
		float num16 = MathHelper.PiOver2;
		controlPoints.Add(startArmPosition);
		for (int i = 0; i < segments; i++)
		{
			float num17 = i / (float)segments;
			float num18 = maxRotation * num17;
			Vector2 vector2 = vector + num14.ToRotationVector2() * durationValue;
			Vector2 vector3 = value2 + num16.ToRotationVector2() * (durationValue * 2f);
			Vector2 vector4 = value + num15.ToRotationVector2() * (durationValue * 2f);
			float num19 = 1f - durationSquare15;
			float num20 = 1f - num19 * num19;
			var value3 = Vector2.Lerp(vector3, vector2, num20 * 0.9f + 0.1f);
			var value4 = Vector2.Lerp(vector4, value3, num20 * 0.7f + 0.3f);
			Vector2 spinningpoint = startArmPosition + (value4 - startArmPosition) * new Vector2(1f, duration15);
			float num21 = factor;
			num21 *= num21;
			Vector2 item = spinningpoint.RotatedBy((double)(proj.rotation + 4.712389f * num21 * proj.spriteDirection), startArmPosition);
			controlPoints.Add(item);
			num14 += num18;
			num16 += num18;
			num15 += num18;
			vector = vector2;
			value2 = vector3;
			value = vector4;
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawWhip(Projectile);
		return false;
	}
	public virtual void DrawWhip(Projectile proj)
	{
		var list = new List<Vector2>();
		FillWhipControlPoints(proj, list);
		Texture2D mainTexture = TextureAssets.Projectile[proj.type].Value;
		for (int i = 0; i < list.Count - 1; i++)
		{
			int frame = (int)(i / (float)(list.Count - 1) * 4);
			if (frame == 0 && i > 0)
			{
				frame = 1;
			}
			Rectangle rectangle = new Rectangle(0, 28 * frame, 18, 28);
			var origin = new Vector2(rectangle.Width / 2, 2f);
			Vector2 positionNow = list[i];
			Vector2 positionAdd = list[i + 1] - positionNow;
			float rotation = positionAdd.ToRotation() - MathHelper.PiOver2;
			Color color = Lighting.GetColor(positionNow.ToTileCoordinates());
			var scale = new Vector2(1f, (positionAdd.Length() + 2f) / rectangle.Height * 2f);
			Main.spriteBatch.Draw(mainTexture, list[i] - Main.screenPosition, new Rectangle?(rectangle), color, rotation, origin, scale, SpriteEffects.None, 0f);
		}
	}
}
