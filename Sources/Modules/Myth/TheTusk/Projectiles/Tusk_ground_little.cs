using Everglow.Commons.DataStructures;
using Terraria.DataStructures;

namespace Everglow.Myth.TheTusk.Projectiles;

/// <summary>
/// Projectile.ai[0] is the length of tusk over gum.
/// </summary>
public class Tusk_ground_little : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.tileCollide = false;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.width = 10;
		Projectile.height = 10;
		Projectile.timeLeft = 120;
		Projectile.penetrate = -1;
		ProjectileID.Sets.PlayerHurtDamageIgnoresDifficultyScaling[Type] = true;
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.ai[0] = 0;
		Vector2 rot = ProjectileUtils.GetTopographicGradient(Projectile.Center, 3);
		if (rot != Vector2.zeroVector)
		{
			Projectile.rotation = ProjectileUtils.GetTopographicGradient(Projectile.Center, 3).ToRotation();
		}
		else
		{
			Projectile.active = false;
			return;
		}
		Projectile.frame = Main.rand.Next(8);
		for (int i = 0; i < 15; i++)
		{
			if(!Collision.SolidCollision(Projectile.Center, 0, 0))
			{
				Projectile.position -= Projectile.rotation.ToRotationVector2() * 4;
			}
			else
			{
				break;
			}
		}
	}

	public override void AI()
	{
		if (Projectile.timeLeft > 107)
		{
			Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 0.1f, 0.8f);
		}
		if (Projectile.timeLeft is > 94 and <= 107)
		{
			Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 0f, 0.7f);
		}
		if (Projectile.timeLeft is > 90 and <= 94)
		{
			Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 0.16f, 0.8f);
		}
		if (Projectile.timeLeft is > 75 and <= 90)
		{
			Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 0f, 0.7f);
		}
		if (Projectile.timeLeft is > 60 and <= 75)
		{
			Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 1f, 0.7f);
		}
		if (Projectile.timeLeft is < 60)
		{
			Projectile.ai[0] = MathHelper.Lerp(Projectile.ai[0], 0f, 0.07f);
		}
	}

	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		float value = 0;
		if (Collision.CheckAABBvLineCollision(targetHitbox.TopLeft(), targetHitbox.Size(), Projectile.Center, new Vector2(Projectile.ai[0] * 60, 0).RotatedBy(Projectile.rotation) + Projectile.Center, 10, ref value))
		{
			return true;
		}
		return false;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		Projectile.hide = true;
		behindNPCsAndTiles.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Vector2 width = new Vector2(0, 10).RotatedBy(Projectile.rotation);
		Vector2 gumWidth = new Vector2(0, 10 + (1 - Projectile.ai[0]) * 20).RotatedBy(Projectile.rotation);
		Vector2 direction = new Vector2(1, 0).RotatedBy(Projectile.rotation);
		float height = 60 * Projectile.ai[0];

		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null);

		Effect effect = Commons.ModAsset.Shader2D.Value;
		var projection = Matrix.CreateOrthographicOffCenter(0, Main.screenWidth, Main.screenHeight, 0, 0, 1);
		var model = Matrix.CreateTranslation(new Vector3(-Main.screenPosition, 0)) * Main.GameViewMatrix.TransformationMatrix;
		effect.Parameters["uTransform"].SetValue(model * projection);
		effect.CurrentTechnique.Passes[0].Apply();
		List<Vertex2D> bars = new List<Vertex2D>();
		AddVertex(bars, Projectile.Center - width, new Vector3(Projectile.frame / 8f, Projectile.ai[0], 0));
		AddVertex(bars, Projectile.Center + width, new Vector3((Projectile.frame + 1) / 8f, Projectile.ai[0], 0));

		AddVertex(bars, Projectile.Center - width + direction * height, new Vector3(Projectile.frame / 8f, 0, 0));
		AddVertex(bars, Projectile.Center + width + direction * height, new Vector3((Projectile.frame + 1) / 8f, 0, 0));

		Texture2D tusk = ModAsset.Tusk_ground_little.Value;
		Main.graphics.graphicsDevice.Textures[0] = tusk;
		Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		bars = new List<Vertex2D>();
		AddVertex(bars, Projectile.Center - width - direction * 12, new Vector3(0, 1, 0));
		AddVertex(bars, Projectile.Center + width - direction * 12, new Vector3(1, 1, 0));

		AddVertex(bars, Projectile.Center - width + direction * MathF.Pow(height, 0.5f), new Vector3(0, 0, 0));
		AddVertex(bars, Projectile.Center + width + direction * MathF.Pow(height, 0.5f), new Vector3(1, 0, 0));

		Texture2D gum = ModAsset.TuskGum.Value;
		Main.graphics.graphicsDevice.Textures[0] = gum;
		Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
		return false;
	}

	public void AddVertex(List<Vertex2D> bars, Vector2 pos, Vector3 coords)
	{
		bars.Add(pos, Lighting.GetColor(pos.ToTileCoordinates()), coords);
	}
}