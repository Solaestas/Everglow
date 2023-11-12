using Everglow.Commons.Weapons.CrossBow;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;
public class CyanVineCrossBow_Proj : CrossBowProjectile
{
	public override void SetDef()
	{
		CrossBowTexture = ModAsset.CyanVineCrossBow_Proj.Value;
		ChordTexture = ModAsset.CyanVineCrossBow_Proj_Chord.Value;
		HeldProjectileOffset = new Vector2(-6, -6);
	}
	public override void DrawChord(SpriteBatch spriteBatch, Color lightColor)
	{
		if (ChordTexture == null)
		{
			return;
		}
		Player player = Main.player[Projectile.owner];
		float sliderPosValue = (player.itemAnimationMax - player.itemAnimation) / (float)player.itemAnimationMax;
		sliderPosValue = MathF.Pow(sliderPosValue, 0.6f);
		Vector2 upChordHead = new Vector2(5, -13);
		Vector2 downChordHead = new Vector2(7, 9);
		Vector2 upChordTail = new Vector2(24 - sliderPosValue * 32, -6);
		Vector2 downChordTail = new Vector2(24 - sliderPosValue * 32, -3);
		Vector2 sliderHead = new Vector2(28 - sliderPosValue * 32, -6);
		Vector2 sliderTail = new Vector2(10 - sliderPosValue * 32, -6);
		if (player.direction * player.gravDir < 0)
		{
			upChordHead.Y *= -1;
			downChordHead.Y *= -1;
			upChordTail.Y *= -1;
			downChordTail.Y *= -1;
			sliderHead.Y *= -1;
			sliderTail.Y *= -1;
		}
		upChordHead = upChordHead.RotatedBy(Projectile.rotation);
		downChordHead = downChordHead.RotatedBy(Projectile.rotation);
		upChordTail = upChordTail.RotatedBy(Projectile.rotation);
		downChordTail = downChordTail.RotatedBy(Projectile.rotation);
		sliderHead = sliderHead.RotatedBy(Projectile.rotation);
		sliderTail = sliderTail.RotatedBy(Projectile.rotation);


		Vector2 upChord = upChordHead - upChordTail;
		Vector2 slider = sliderHead - sliderTail;
		Vector2 downChord = downChordHead - downChordTail;
		Vector2 basePoint = Projectile.Center - Main.screenPosition + HeldPoint;
		float totalDir = player.direction * player.gravDir;
		var vertices = new List<Vertex2D>();

		vertices.Add(basePoint + upChordHead + upChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, lightColor, new Vector3(0 / (float)ChordTexture.Width, 0 / (float)ChordTexture.Height, 0));
		vertices.Add(basePoint + upChordHead - upChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, lightColor, new Vector3(0 / (float)ChordTexture.Width, 3 / (float)ChordTexture.Height, 0));

		vertices.Add(basePoint + upChordTail + upChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, lightColor, new Vector3(8 / (float)ChordTexture.Width, 0 / (float)ChordTexture.Height, 0));
		vertices.Add(basePoint + upChordTail - upChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, lightColor, new Vector3(8 / (float)ChordTexture.Width, 3 / (float)ChordTexture.Height, 0));

		vertices.Add(basePoint + upChordTail + upChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, Color.Transparent, new Vector3(8 / (float)ChordTexture.Width, 0 / (float)ChordTexture.Height, 0));
		vertices.Add(basePoint + upChordTail - upChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, Color.Transparent, new Vector3(8 / (float)ChordTexture.Width, 3 / (float)ChordTexture.Height, 0));

		vertices.Add(basePoint + sliderHead + slider.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 3 * totalDir, Color.Transparent, new Vector3(18 / (float)ChordTexture.Width, 6 / (float)ChordTexture.Height, 0));
		vertices.Add(basePoint + sliderHead - slider.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 3 * totalDir, Color.Transparent, new Vector3(18 / (float)ChordTexture.Width, 3 / (float)ChordTexture.Height, 0));

		vertices.Add(basePoint + sliderHead + slider.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 3 * totalDir, lightColor, new Vector3(18 / (float)ChordTexture.Width, 6 / (float)ChordTexture.Height, 0));
		vertices.Add(basePoint + sliderHead - slider.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 3 * totalDir, lightColor, new Vector3(18 / (float)ChordTexture.Width, 3 / (float)ChordTexture.Height, 0));

		vertices.Add(basePoint + sliderTail + slider.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 3 * totalDir, lightColor, new Vector3(10 / (float)ChordTexture.Width, 6 / (float)ChordTexture.Height, 0));
		vertices.Add(basePoint + sliderTail - slider.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 3 * totalDir, lightColor, new Vector3(10 / (float)ChordTexture.Width, 3 / (float)ChordTexture.Height, 0));

		vertices.Add(basePoint + sliderTail + slider.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 3 * totalDir, Color.Transparent, new Vector3(10 / (float)ChordTexture.Width, 6 / (float)ChordTexture.Height, 0));
		vertices.Add(basePoint + sliderTail - slider.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 3 * totalDir, Color.Transparent, new Vector3(10 / (float)ChordTexture.Width, 3 / (float)ChordTexture.Height, 0));

		vertices.Add(basePoint + downChordHead + downChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, Color.Transparent, new Vector3(0 / (float)ChordTexture.Width, 5 / (float)ChordTexture.Height, 0));
		vertices.Add(basePoint + downChordHead - downChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, Color.Transparent, new Vector3(0 / (float)ChordTexture.Width, 9 / (float)ChordTexture.Height, 0));

		vertices.Add(basePoint + downChordHead + downChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, lightColor, new Vector3(0 / (float)ChordTexture.Width, 5 / (float)ChordTexture.Height, 0));
		vertices.Add(basePoint + downChordHead - downChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, lightColor, new Vector3(0 / (float)ChordTexture.Width, 9 / (float)ChordTexture.Height, 0));

		vertices.Add(basePoint + downChordTail + downChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, lightColor, new Vector3(8 / (float)ChordTexture.Width, 5 / (float)ChordTexture.Height, 0));
		vertices.Add(basePoint + downChordTail - downChord.NormalizeSafe().RotatedBy(MathHelper.PiOver2) * 1.2f * totalDir, lightColor, new Vector3(8 / (float)ChordTexture.Width, 9 / (float)ChordTexture.Height, 0));

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		Main.graphics.GraphicsDevice.Textures[0] = ChordTexture;
		if (vertices.Count > 2)
		{
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
		}
	}
}
