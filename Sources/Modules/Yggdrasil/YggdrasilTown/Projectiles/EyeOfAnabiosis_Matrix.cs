using Everglow.Commons.DataStructures;
using Everglow.Yggdrasil.YggdrasilTown.Items.Weapons.SquamousShell;
using Terraria.DataStructures;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class EyeOfAnabiosis_Matrix : ModProjectile
{
	public override string Texture => Commons.ModAsset.EmptyBuff_Mod;

	private Player Owner => Main.player[Projectile.owner];

	public int WinkTimer = 0;

	public override void SetDefaults()
	{
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.aiStyle = -1;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.timeLeft = 60;
	}

	public override void OnSpawn(IEntitySource source)
	{
		WinkTimer = 0;
	}

	public override void AI()
	{
		if (CanExist())
		{
			Projectile.timeLeft = 60;
			Projectile.Center = Owner.Center + new Vector2(0, -24);
		}
		WinkTimer++;
	}

	public bool CanExist()
	{
		if (Owner.HeldItem.type == ModContent.ItemType<EyeOfAnabiosis>())
		{
			if (Owner.heldProj >= 0 && Main.projectile[Owner.heldProj].type == ModContent.ProjectileType<EyeOfAnabiosis_Weapon>())
			{
				EyeOfAnabiosis_Weapon eOAW = Main.projectile[Owner.heldProj].ModProjectile as EyeOfAnabiosis_Weapon;
				if (eOAW.ChargeTimer >= EyeOfAnabiosis_Weapon.MaxChargeTime - 10)
				{
					return true;
				}
			}
		}
		return false;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		base.DrawBehind(index, behindNPCsAndTiles, behindNPCs, behindProjectiles, overPlayers, overWiresUI);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		// eyelash top
		float wink = Math.Clamp(Math.Min(WinkTimer, Projectile.timeLeft) / 60f, 0, 1);
		wink = MathF.Pow(wink, 2);
		float timeValue = (float)Main.time * 0.03f;
		Vector2 drawCenter = Projectile.Center - Main.screenPosition;
		float frameHeightOut = 0.750f;
		float frameHeightIn = 0.875f;

		// dark peripheral ring
		List<Vertex2D> bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			Color drawColor = Color.White * 0.8f * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * 0.1f;
			bars.Add(drawCenter + new Vector2(0, 100).RotatedBy(rotValue), drawColor, new Vector3(i / 50f, frameHeightOut, 0));
			bars.Add(drawCenter + new Vector2(0, 80).RotatedBy(rotValue), drawColor, new Vector3(i / 50f, frameHeightIn, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.AlienWriting_black.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// colorful peripheral ring
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			float lerpValue = (MathF.Sin(i / 25f * MathHelper.TwoPi + timeValue) + 1) * 0.5f;
			Color drawColor = Color.Lerp(new Color(0f, 0.7f, 1f, 0f), new Color(0f, 0f, 0.3f, 0f), lerpValue) * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * 0.1f;
			bars.Add(drawCenter + new Vector2(0, 100).RotatedBy(rotValue), drawColor, new Vector3(i / 50f, frameHeightOut, 0));
			bars.Add(drawCenter + new Vector2(0, 80).RotatedBy(rotValue), drawColor, new Vector3(i / 50f, frameHeightIn, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.AlienWriting.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// dark inner ring0
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			Color drawColor = Color.White * 0.3f * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * -0.03f;
			bars.Add(drawCenter + new Vector2(0, 74).RotatedBy(rotValue), drawColor, new Vector3(i / 50f, frameHeightOut, 0));
			bars.Add(drawCenter + new Vector2(0, 62).RotatedBy(rotValue), drawColor, new Vector3(i / 50f, frameHeightIn, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.AlienWriting_black.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// colorful inner ring0
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			float lerpValue = (MathF.Sin(i / 6.25f * MathHelper.TwoPi + timeValue) + 1) * 0.5f;
			Color drawColor = Color.Lerp(new Color(0f, 0.7f, 1f, 0f), new Color(0.4f, 1f, 1f, 0f), lerpValue) * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * -0.03f;
			bars.Add(drawCenter + new Vector2(0, 74).RotatedBy(rotValue), drawColor, new Vector3(i / 50f, frameHeightOut, 0));
			bars.Add(drawCenter + new Vector2(0, 62).RotatedBy(rotValue), drawColor, new Vector3(i / 50f, frameHeightIn, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.AlienWriting.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// dark inner ring1
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			Color drawColor = Color.White * 0.8f * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * 0.03f;
			bars.Add(drawCenter + new Vector2(0, 88).RotatedBy(rotValue), drawColor, new Vector3(i / 25f, 0.2f, 0));
			bars.Add(drawCenter + new Vector2(0, 76).RotatedBy(rotValue), drawColor, new Vector3(i / 25f, 0.5f, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_1_black.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// colorful inner ring1
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			float lerpValue = (MathF.Sin(i / 25f * MathHelper.TwoPi + timeValue) + 1) * 0.5f;
			Color drawColor = Color.Lerp(new Color(0f, 0.7f, 1f, 0f), new Color(0f, 0.2f, 0.8f, 0f), lerpValue) * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * 0.03f;
			bars.Add(drawCenter + new Vector2(0, 88).RotatedBy(rotValue), drawColor, new Vector3(i / 25f, 0.2f, 0));
			bars.Add(drawCenter + new Vector2(0, 76).RotatedBy(rotValue), drawColor, new Vector3(i / 25f, 0.5f, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_1.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// dark inner ring2
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			Color drawColor = Color.White * 0.3f * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * 0.03f;
			bars.Add(drawCenter + new Vector2(0, 80).RotatedBy(rotValue), drawColor, new Vector3(i / 25f, 0.2f, 0));
			bars.Add(drawCenter + new Vector2(0, 68).RotatedBy(rotValue), drawColor, new Vector3(i / 25f, 0.5f, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Star_black.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// colorful inner ring2
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			float lerpValue = (MathF.Sin(i / 6.25f * MathHelper.TwoPi + timeValue) + 1) * 0.5f;
			Color drawColor = Color.Lerp(new Color(0f, 0.7f, 1f, 0f), new Color(0f, 0f, 0.3f, 0f), lerpValue) * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * 0.03f;
			bars.Add(drawCenter + new Vector2(0, 80).RotatedBy(rotValue), drawColor, new Vector3(i / 25f, 0.2f, 0));
			bars.Add(drawCenter + new Vector2(0, 68).RotatedBy(rotValue), drawColor, new Vector3(i / 25f, 0.5f, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Textures_Star.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// eyelash top
		bars = new List<Vertex2D>();
		List<Vertex2D> barsBlack = new List<Vertex2D>();
		List<Vertex2D> eyelash = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			float lerpValue = (MathF.Sin(i / 33f * MathHelper.TwoPi + timeValue) + 1) * 0.5f;
			Color drawColor = Color.Lerp(new Color(0f, 0.7f, 1f, 0f), new Color(0f, 1f, 2f, 0f), lerpValue) * wink;
			float height = (MathF.Cos((i - 50) / 50f * MathHelper.Pi) + 1) * wink * 17;
			float deltaX = (i - 50) * 1.4f;
			bars.Add(drawCenter + new Vector2(deltaX, -height - 10), drawColor, new Vector3(i / 25f, 0f, 0));
			bars.Add(drawCenter + new Vector2(deltaX, -height), drawColor, new Vector3(i / 25f, 0.5f, 0));

			barsBlack.Add(drawCenter + new Vector2(deltaX, -height - 10), Color.White, new Vector3(i / 25f, 0f, 0));
			barsBlack.Add(drawCenter + new Vector2(deltaX, -height), Color.White, new Vector3(i / 25f, 0.5f, 0));
			if (i % 10 == 0)
			{
				Vector2 eyelashStart = new Vector2(deltaX, -height);
				Vector2 eyelashEnd = Vector2.Normalize(eyelashStart) * 76;
				Vector2 eyelashWidth = Vector2.Normalize(eyelashEnd - eyelashStart).RotatedBy(MathHelper.PiOver2) * 0.5f;
				eyelash.Add(drawCenter + eyelashStart + eyelashWidth * 2, drawColor, new Vector3(i / 25f, 0f + timeValue * 0.1f, 0));
				eyelash.Add(drawCenter + eyelashStart - eyelashWidth * 2, drawColor, new Vector3(i / 25f + 0.25f, 0f + timeValue * 0.1f, 0));
				eyelash.Add(drawCenter + eyelashEnd - eyelashWidth, drawColor, new Vector3(i / 25f + 0.25f, 0.2f + timeValue * 0.1f, 0));

				eyelash.Add(drawCenter + eyelashEnd - eyelashWidth, drawColor, new Vector3(i / 25f + 0.25f, 0.2f + timeValue * 0.1f, 0));
				eyelash.Add(drawCenter + eyelashEnd + eyelashWidth, drawColor, new Vector3(i / 25f, 0.2f + timeValue * 0.1f, 0));
				eyelash.Add(drawCenter + eyelashStart + eyelashWidth * 2, drawColor, new Vector3(i / 25f, 0f + timeValue * 0.1f, 0));
			}
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_1_black.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, barsBlack.ToArray(), 0, barsBlack.Count - 2);

		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_1.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_perlin.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, eyelash.ToArray(), 0, eyelash.Count / 3);

		// eyelash bottom
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			float lerpValue = (MathF.Sin(i / 33f * MathHelper.TwoPi + timeValue) + 1) * 0.5f;
			Color drawColor = Color.Lerp(new Color(0f, 0.7f, 1f, 0f), new Color(0f, 0f, 0.3f, 0f), lerpValue) * wink;
			float height = (MathF.Cos((i - 50) / 50f * MathHelper.Pi) + 1) * wink * 17;
			float deltaX = (i - 50) * 1.4f;
			bars.Add(drawCenter + new Vector2(deltaX, height + 10), drawColor, new Vector3(i / 25f, 0f, 0));
			bars.Add(drawCenter + new Vector2(deltaX, height), drawColor, new Vector3(i / 25f, 0.5f, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_1.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// pupil black
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			Color drawColor = Color.Black * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * 0.03f;
			bars.Add(drawCenter + new Vector2(0, 24 * wink).RotatedBy(rotValue), drawColor, new Vector3(i / 20f, 0.2f + timeValue * 0.05f, 0));
			bars.Add(drawCenter + new Vector2(0, 10 * wink).RotatedBy(rotValue), new Color(0f, 0f, 0.3f, 0f), new Vector3(i / 20f, 0.4f + timeValue * 0.05f, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_melting.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// pupil
		Vector2 addPos = (Main.MouseScreen - drawCenter) * 0.06f;
		if (addPos.Length() > 10)
		{
			addPos = Vector2.Normalize(addPos) * 10;
		}
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			float lerpValue = (MathF.Sin(i / 50f * MathHelper.TwoPi + timeValue) + 1) * 0.5f;
			Color drawColor = Color.Lerp(new Color(0f, 0.7f, 1f, 0f), new Color(0f, 0.4f, 0.7f, 0f), lerpValue) * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * 0.03f;
			bars.Add(drawCenter + new Vector2(0, 24 * wink).RotatedBy(rotValue), drawColor, new Vector3(i / 20f, 0.2f + timeValue * 0.05f, 0));
			bars.Add(drawCenter + addPos + new Vector2(0, 10 * wink).RotatedBy(rotValue), new Color(0f, 0f, 0.3f, 0f), new Vector3(i / 20f, 0.4f + timeValue * 0.05f, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Noise_melting.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// pupil edge
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 100; i++)
		{
			float lerpValue = (MathF.Sin(i / 50f * MathHelper.TwoPi + timeValue) + 1) * 0.5f;
			Color drawColor = Color.Lerp(new Color(0f, 0.7f, 1f, 0f), new Color(0f, 0.4f, 0.7f, 0f), lerpValue) * wink;
			float rotValue = i / 100f * MathHelper.TwoPi + timeValue * 0.03f;
			bars.Add(drawCenter + new Vector2(0, 24 * wink).RotatedBy(rotValue), drawColor, new Vector3(i / 25f, 0.5f, 0));
			bars.Add(drawCenter + new Vector2(0, 20 * wink).RotatedBy(rotValue), new Color(0f, 0f, 0.3f, 0f), new Vector3(i / 25f, 0.7f, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_6.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		// pupil center
		bars = new List<Vertex2D>();
		for (int i = 0; i <= 10; i++)
		{
			float lerpValue = (MathF.Sin(i / 5f * MathHelper.TwoPi + timeValue) + 1) * 0.5f;
			Color drawColor = Color.Lerp(new Color(0f, 0.7f, 1f, 0f), new Color(0f, 0.4f, 0.7f, 0f), lerpValue) * wink;
			float rotValue = i / 10f * MathHelper.TwoPi + timeValue * 0.03f;
			bars.Add(drawCenter + addPos + new Vector2(0, 6 * wink).RotatedBy(rotValue), drawColor, new Vector3(i / 25f, 0.5f, 0));
			bars.Add(drawCenter + addPos + new Vector2(0, 3 * wink).RotatedBy(rotValue), new Color(0f, 0f, 0.3f, 0f), new Vector3(i / 25f, 0.7f, 0));
		}
		Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.Trail_6.Value;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		if (WinkTimer < 60f)
		{
			Texture2D light = Commons.ModAsset.StarSlash.Value;
			Texture2D light_black = Commons.ModAsset.StarSlash_black.Value;
			float dark = (60 - WinkTimer) / 60f;
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0f, 0.7f, 1f, 0f), MathHelper.PiOver2, light.Size() / 2f, new Vector2(dark * dark * 0.8f, 1.4f), SpriteEffects.None, 0);
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0f, 0.7f, 1f, 0f), 0, light.Size() / 2f, new Vector2(dark * dark * 0.4f, 0.5f), SpriteEffects.None, 0);
		}
		return false;
	}
}