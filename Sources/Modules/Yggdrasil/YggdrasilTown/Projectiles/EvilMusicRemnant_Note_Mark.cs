using Everglow.Commons.DataStructures;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EvilMusicRemnant_Note_Mark : ModProjectile
{
	public int MaxTime = 600;

	public float mouseDistanceValue = 0;

	public override void SetDefaults()
	{
		Projectile.width = 26;
		Projectile.height = 26;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.timeLeft = MaxTime;
	}

	public override void OnSpawn(IEntitySource source)
	{
		Projectile.scale = 0;
		mouseDistanceValue = (Main.MouseWorld - Main.player[Projectile.owner].Center).Length() / 6f + 30;
	}

	private string ProjectileTexture { get; set; } = ModAsset.EvilMusicRemnant_Projectile1_Mod;

	public override string Texture => ProjectileTexture;

	public override void AI()
	{
		if (Projectile.timeLeft > MaxTime - 32f)
		{
			float value = (MaxTime - Projectile.timeLeft) / 30f;
			Projectile.scale = MathF.Pow(value, 0.3f);
		}
		else
		{
			Projectile.scale = 1;
		}
		Projectile.velocity *= 0;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		var texture = Commons.ModAsset.StarSlash.Value;
		var textureBlack = Commons.ModAsset.StarSlash_black.Value;
		float timeValue = (float)Main.timeForVisualEffects * 0.01f;
		var drawPos = Projectile.Center - Main.screenPosition;
		float fade = 1f;
		if(Projectile.timeLeft < 60f)
		{
			fade = Projectile.timeLeft / 60f;
		}
		float staveFade = 1f;
		if (Projectile.timeLeft > MaxTime - mouseDistanceValue)
		{
			if (Projectile.timeLeft > MaxTime - mouseDistanceValue + 60)
			{
				staveFade = 0;
			}
			else
			{
				staveFade = (MaxTime - Projectile.timeLeft - mouseDistanceValue + 60) / 60f;
			}
		}
		float staveSize = MathF.Pow(3 - staveFade * 2, 0.5f);
		List<Vertex2D> bars_front_dark = new List<Vertex2D>();
		List<Vertex2D> bars_back_dark = new List<Vertex2D>();

		List<Vertex2D> bars_front = new List<Vertex2D>();
		List<Vertex2D> bars_back = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			var ringWidth = 60f * Projectile.scale * staveSize;
			var phase = i / 100f * MathHelper.TwoPi;
			var x0 = MathF.Cos(timeValue * 2f + phase + Projectile.whoAmI) * ringWidth;
			var y0 = MathF.Sin(timeValue * 2f + phase * 3 + Projectile.whoAmI) * 10 * MathF.Sin(timeValue * 2f) * Projectile.scale * staveSize;
			var z0 = MathF.Sin(timeValue * 2f + phase + Projectile.whoAmI) * ringWidth;
			var oldPhase = (i - 1) / 100f * MathHelper.TwoPi;
			var oldZ0 = MathF.Sin(timeValue * 2f + oldPhase + Projectile.whoAmI) * ringWidth;
			var nextPhase = (i + 1) / 100f * MathHelper.TwoPi;
			var nextZ0 = MathF.Sin(timeValue * 2f + nextPhase + Projectile.whoAmI) * ringWidth;
			var staveCurve = new Vector2(x0, y0);
			float scale = ((ringWidth - z0) + 4 * ringWidth) / (5 * ringWidth);
			float z0Value = 1 - (z0 + ringWidth) / (2 * ringWidth);
			//float scale = (z0 + 4 * ringWidth) / (5 * ringWidth);
			//float z0Value = (z0 + ringWidth) / (2 * ringWidth);
			var drawColor = Color.Lerp(new Color(128, 45, 229, 0), new Color(24, 2, 150, 0), z0Value) * fade * staveFade;
			var darkColor = Color.Lerp(new Color(255, 255, 255, 255), new Color(127, 127, 127, 127), z0Value) * fade * staveFade;
			var height = 24f * Projectile.scale;
			if (z0 > 0)
			{
				bars_front.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, drawColor, new Vector3(phase, 0, 0));
				bars_front.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, drawColor, new Vector3(phase, 1, 0));

				bars_front_dark.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, darkColor, new Vector3(phase, 0, 0));
				bars_front_dark.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, darkColor, new Vector3(phase, 1, 0));
				if (oldZ0 <= 0)
				{
					bars_back.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 0, 0));
					bars_back.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 1, 0));

					bars_back_dark.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 0, 0));
					bars_back_dark.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 1, 0));
				}
				if (nextZ0 <= 0)
				{
					bars_back.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 0, 0));
					bars_back.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 1, 0));
					bars_back.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, drawColor, new Vector3(phase, 0, 0));
					bars_back.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, drawColor, new Vector3(phase, 1, 0));

					bars_back_dark.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 0, 0));
					bars_back_dark.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 1, 0));
					bars_back_dark.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, darkColor, new Vector3(phase, 0, 0));
					bars_back_dark.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, darkColor, new Vector3(phase, 1, 0));
				}
			}
			else
			{
				bars_back.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, drawColor, new Vector3(phase, 0, 0));
				bars_back.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, drawColor, new Vector3(phase, 1, 0));

				bars_back_dark.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, darkColor, new Vector3(phase, 0, 0));
				bars_back_dark.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, darkColor, new Vector3(phase, 1, 0));
				if (oldZ0 > 0)
				{
					bars_front.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 0, 0));
					bars_front.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 1, 0));

					bars_front_dark.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 0, 0));
					bars_front_dark.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 1, 0));
				}
				if (nextZ0 > 0)
				{
					bars_front.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 0, 0));
					bars_front.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 1, 0));
					bars_front.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, drawColor, new Vector3(phase, 0, 0));
					bars_front.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, drawColor, new Vector3(phase, 1, 0));

					bars_front_dark.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 0, 0));
					bars_front_dark.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, new Color(0, 0, 0, 0), new Vector3(phase, 1, 0));
					bars_front_dark.Add(drawPos + staveCurve + new Vector2(0, -height) / scale, darkColor, new Vector3(phase, 0, 0));
					bars_front_dark.Add(drawPos + staveCurve * 0.8f + new Vector2(0, height) / scale, darkColor, new Vector3(phase, 1, 0));
				}
			}
		}
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.EvilMusicRemnant_Stave_black.Value;
		if (bars_back_dark.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars_back_dark.ToArray(), 0, bars_back_dark.Count - 2);
		}
		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.EvilMusicRemnant_Stave.Value;
		if (bars_back.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars_back.ToArray(), 0, bars_back.Count - 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		//Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition, null, new Color(50, 232, 123, 0), 0, texture.Size() / 2, new Vector2(1f * Projectile.scale * fade, 0.5f * Projectile.scale), SpriteEffects.None, 0);
		Main.spriteBatch.Draw(textureBlack, Projectile.Center - Main.screenPosition, null, Color.White, 0, textureBlack.Size() / 2, new Vector2(1f * Projectile.scale * fade, 0.5f * Projectile.scale), SpriteEffects.None, 0);
		for (int i = 0; i < 3; i++)
		{
			Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(MathF.Sin(timeValue * 3 + i / 3f * MathHelper.TwoPi) * 16, 0), null, new Color(50, 232, 123, 0) * 0.2f * fade, 0, texture.Size() / 2, 0.4f * Projectile.scale, SpriteEffects.None, 0);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.EvilMusicRemnant_Stave_black.Value;
		if (bars_front_dark.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars_front_dark.ToArray(), 0, bars_front_dark.Count - 2);
		}
		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.EvilMusicRemnant_Stave.Value;
		if (bars_front.Count > 3)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars_front.ToArray(), 0, bars_front.Count - 2);
		}
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);
		return false;
	}

	public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
	{
		if (!target.active && !target.friendly && !target.townNPC)
		{
			SummonMinion();
		}
	}

	public void SummonMinion()
	{
		var owner = Main.player[Projectile.owner];
		var minionProjType = ModContent.ProjectileType<EvilMusicRemnant_Minion>();

		if (owner.maxMinions <= owner.slotsMinions)
		{
			if (owner.ownedProjectileCounts[minionProjType] <= 0)
			{
				return;
			}
			else
			{
				var queryMinions = Main.projectile.Where(x => x.type == minionProjType && x.active);
				if (queryMinions.Any())
				{
					queryMinions.Last().Kill();
				}
			}
		}

		var index = Projectile.NewProjectile(Projectile.GetSource_FromAI(), Projectile.Center, Vector2.Zero, minionProjType, Projectile.damage, Projectile.knockBack, Projectile.owner, owner.ownedProjectileCounts[minionProjType] + 1);
		owner.AddBuff(ModContent.BuffType<Buffs.EvilMusicRemnant>(), 30);
	}
}