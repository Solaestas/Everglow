using Everglow.Commons.DataStructures;
using Everglow.Myth.TheTusk.NPCs.BloodTusk;
using Terraria.DataStructures;

namespace Everglow.Myth.TheTusk.Projectiles;

public class BloodTusk_Tentacle : ModProjectile
{
	public List<Vector2> TentaclePoints = new List<Vector2>();
	public NPC BloodTuskOwner;
	public Vector2 LinePostions = Vector2.zeroVector;

	public override void SetDefaults()
	{
		Projectile.tileCollide = false;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.timeLeft = 3000;
		Projectile.penetrate = -1;
		Projectile.extraUpdates = 10;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.rotation = 0;
		Projectile.Center = BloodTuskOwner.Bottom + new Vector2(0, -36);
		int index = -1;
		if (BloodTuskOwner is null)
		{
			index = NPC.FindFirstNPC(ModContent.NPCType<BloodTusk>());
		}
		if (index is >= 0 and < 200)
		{
			BloodTuskOwner = Main.npc[index];
		}
		else
		{
			Projectile.active = false;
			return;
		}
		LinePostions = Vector2.zeroVector;
	}

	public override void AI()
	{
		Projectile.Center = BloodTuskOwner.Bottom + new Vector2(0, -36);
		if (Projectile.timeLeft > 1000)
		{
			if (Projectile.timeLeft % 15 == 0)
			{
				TentaclePoints.Add(LinePostions);
			}
			if (Projectile.timeLeft < 1300)
			{
				Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.direction * Projectile.ai[0]);
				Projectile.ai[0] *= 0.98f;
			}
		}
		LinePostions += Projectile.velocity;
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		float value = 0;
		if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, new Vector2(Projectile.ai[0] * 90, 0).RotatedBy(Projectile.rotation) + Projectile.Center, 10, ref value))
		{
			return true;
		}
		return false;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		Projectile.hide = true;
		behindNPCs.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		if (TentaclePoints.Count <= 1)
		{
			return false;
		}
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, default, RasterizerState.CullNone, null);
		Effect dissolve = ModAsset.BloodMembrane.Value;
		float duration = 1f;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		dissolve.Parameters["uTransform"].SetValue(model * projection);
		dissolve.Parameters["uNoise"].SetValue(Commons.ModAsset.Noise_spiderNet.Value);
		dissolve.Parameters["duration"].SetValue(duration);
		dissolve.Parameters["uDissolveColor"].SetValue(new Vector4(0.4f, 0, 0, 0.8f) * lightColor.ToVector4());
		dissolve.Parameters["uNoiseSize"].SetValue(0.7f);
		dissolve.Parameters["uNoiseXY"].SetValue(new Vector2(Projectile.ai[1], Projectile.ai[2]));
		dissolve.CurrentTechnique.Passes[0].Apply();

		Texture2D rope = ModAsset.BloodTusk_Tentacle.Value;
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 0; i < TentaclePoints.Count - 30; i++)
		{
			AddVertex(bars, GetPaintPos(TentaclePoints[i]), new Vector3(i / 20f, 0, 0));
			AddVertex(bars, GetPaintPos(TentaclePoints[i]), new Vector3(i / 20f, 0.5f, 0));
		}
		int endAt = Math.Max(TentaclePoints.Count - 30, 0);
		AddVertex(bars, GetPaintPos(TentaclePoints[endAt]), new Vector3(endAt / 20f, 0, 0));
		AddVertex(bars, GetPaintPos(TentaclePoints[endAt]), new Vector3(endAt / 20f, 0.5f, 0));
		AddVertex(bars, GetPaintPos(TentaclePoints[endAt]), new Vector3(0, 0, 0));
		AddVertex(bars, GetPaintPos(TentaclePoints[endAt]), new Vector3(0, 0.5f, 0));
		for (int i = TentaclePoints.Count - 30; i < TentaclePoints.Count - 1; i++)
		{
			AddVertex(bars, GetPaintPos(TentaclePoints[i]), new Vector3((i - endAt) / 40f, 0.5f, 0));
			AddVertex(bars, GetPaintPos(TentaclePoints[i]), new Vector3((i - endAt) / 40f, 1f, 0));
		}
		Main.graphics.graphicsDevice.Textures[0] = rope;
		Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, bars.ToArray(), 0, bars.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
		return false;
	}

	public Vector2 GetPaintPos(Vector2 pos)
	{
		return pos.RotatedBy(Projectile.rotation) + Projectile.Center;
	}

	public void AddVertex(List<Vertex2D> bars, Vector2 pos, Vector3 coords)
	{
		bars.Add(pos, Lighting.GetColor(pos.ToTileCoordinates()), coords);
	}
}