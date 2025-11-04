using Everglow.Commons.DataStructures;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;
using Everglow.Commons.VFX;
using ReLogic.Graphics;
using Terraria.GameContent;

namespace Everglow.Commons.MEAC;

public abstract partial class MeleeProj_3D : ModProjectile, IWarpProjectile_warpStyle2, IBloomProjectile
{
	public float CurrentTrailFade = 1f;

	public Queue<float> OldTrailFade = new Queue<float>();

	public bool EnableSphereCoordDraw = false;

	public Matrix ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(
			MathHelper.PiOver4,
			Main.screenWidth / (float)Main.screenHeight,
			0.1f,
			2000f);

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		overPlayers.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		SpriteBatchState sBS = Main.spriteBatch.GetState().Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointWrap, DepthStencilState.None, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		if (EnableSphereCoordDraw)
		{
			DrawReferenceSphere(RadialDistance);
		}
		DrawWeapon(WeaponAxis, MainAxis, new Vector3(0, 0, CenterZ));
		DrawTrail();
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		// Texture2D tex = ModAsset.SwirlPoint.Value;
		// Main.EntitySpriteDraw(tex,Projectile.Center - Main.screenPosition,null,Color.White,0,tex.Size() * 0.5f,1,SpriteEffects.None,0);
		return false;
	}

	public void DrawReferenceSphere(float radius = 120f)
	{
		Color drawColor = new Color(0.1f, 0.3f, 0.4f, 0);

		List<Vector2> basicRing = new List<Vector2>();
		for (int i = 0; i < 30; i++)
		{
			Vector2 ringPos = new Vector2(radius, 0).RotatedBy(i / 30f * MathHelper.TwoPi);
			basicRing.Add(ringPos);
		}

		Main.graphics.graphicsDevice.Textures[0] = ModAsset.StarSlashGray.Value;

		// Latitude Rings
		for (int l = 0; l < 9; l++)
		{
			List<Vector3> latitudeRing = ToLatitudeRing(basicRing, (l + 0.5f) / 9f * MathHelper.Pi - MathHelper.PiOver2, radius);
			Draw3DCurve(latitudeRing, drawColor, ScreenPositionOffset);
		}

		// LongitudeRing Rings
		for (int l = 0; l < 9; l++)
		{
			List<Vector3> longitudeRing = ToLongitudeRing(basicRing, l / 9f * MathHelper.Pi + (float)Main.time * 0.003f);
			Draw3DCurve(longitudeRing, drawColor, ScreenPositionOffset);
		}

		// Shear Surface
		List<Vector2> shearRing = new List<Vector2>();
		for (int i = 0; i < basicRing.Count; i++)
		{
			Vector2 ringPos = Project(new Vector3(basicRing[i], CenterZ), ProjectionMatrix);
			shearRing.Add(ringPos);
		}
		List<Vertex2D> shearSurface = new List<Vertex2D>();
		for (int i = 0; i <= shearRing.Count; i++)
		{
			Vector2 ringPos = shearRing[i % shearRing.Count];
			shearSurface.Add(ringPos + ScreenPositionOffset, drawColor, new Vector3(0.5f, 0.5f, 0));
			shearSurface.Add(ringPos * 0.9f + ScreenPositionOffset, drawColor, new Vector3(0.7f, 0.5f, 0));
		}
		if (shearSurface.Count >= 4)
		{
			Main.graphics.graphicsDevice.Textures[0] = ModAsset.StarSlashGray.Value;
			Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, shearSurface.ToArray(), 0, shearSurface.Count - 2);
		}

		// Current
		DrawPositionArrow(drawColor);
		Draw3DLine(RotatedAxis, Vector3.zero, Color.Red * 0.5f, new Vector3(0, 0, CenterZ));
		Draw3DLine(WeaponAxis, MainAxis, Color.Purple * 0.5f, new Vector3(0, 0, CenterZ));
		Color currentColor = new Color(1f, 0.05f, 0.1f, 0);
		List<Vector3> latitudeRing_current = ToLatitudeRing(basicRing, MathHelper.PiOver2 - PolarAngle, RadialDistance);
		DrawCurrentCoordRing(latitudeRing_current, currentColor, ScreenPositionOffset, 15);

		List<Vector3> longitudeRing_current = ToLongitudeRing(basicRing, AzimuthalAngle + MathHelper.PiOver2);
		DrawCurrentCoordRing(longitudeRing_current, currentColor, ScreenPositionOffset, 15);
	}

	public void DrawPositionText(Vector2 drawPos)
	{
		var coordText = MainAxis.ToString();
		coordText += "\n[R:" + SphericalCoordPos.X + " ,Theta:" + SphericalCoordPos.Y / MathHelper.TwoPi * 360 + " ,Phi:" + SphericalCoordPos.Z / MathHelper.TwoPi * 360 + "]";
		Main.spriteBatch.DrawString(FontAssets.MouseText.Value, coordText, drawPos + ScreenPositionOffset, Color.White, 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
	}

	public void DrawPositionArrow(Color drawColor)
	{
		Draw3DLine(MainAxis, Vector3.zero, drawColor, new Vector3(0, 0, CenterZ));
	}

	public void Draw3DLine(Vector3 start, Vector3 end, Color drawColor, Vector3 offset)
	{
		List<Vertex2D> arrow = new List<Vertex2D>();
		Vector2 start2 = Project(start + offset, ProjectionMatrix);
		Vector2 end2 = Project(end + offset, ProjectionMatrix);
		Vector2 arrowWidth = (start2 - end2).NormalizeSafe().RotatedBy(MathHelper.PiOver2);
		float startScale = GetSizeZ((start + offset).Z);
		float endScale = GetSizeZ((end + offset).Z);

		arrow.Add(start2 + arrowWidth * startScale + ScreenPositionOffset, drawColor, new Vector3(0.5f, 0.5f, 0));
		arrow.Add(start2 - arrowWidth * startScale + ScreenPositionOffset, drawColor, new Vector3(0.5f, 0.5f, 0));

		arrow.Add(end2 + arrowWidth * endScale + ScreenPositionOffset, drawColor, new Vector3(0.5f, 0.5f, 0));
		arrow.Add(end2 - arrowWidth * endScale + ScreenPositionOffset, drawColor, new Vector3(0.5f, 0.5f, 0));
		if (arrow.Count >= 4)
		{
			Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, arrow.ToArray(), 0, arrow.Count - 2);
		}
	}

	public virtual void DrawTrail()
	{
		if (OldArrowTips_Smoothed is null)
		{
			return;
		}
		Color origColor = new Color(1f, 1f, 1f, 0);

		// Draw Line
		// for (int k = 0; k < 3; k++)
		// {
		// Vector2 width = new Vector2(0, 2).RotatedBy(k / 3f * MathHelper.TwoPi);
		// List<Vertex2D> trails = new List<Vertex2D>();
		// for (int i = 0; i < OldArrowTips_Smoothed.Count; i++)
		// {
		// Vector3 currentPos3D = OldArrowTips_Smoothed[i];
		// Vector2 currentPos = Project(currentPos3D, ProjectionMatrix);
		// float size = GetSizeZ(currentPos3D.Z);
		// Color drawColor = origColor * (i / (float)OldArrowTips_Smoothed.Count);
		// trails.Add(currentPos + ScreenPositionOffset + width * size, drawColor, new Vector3(0.5f, 0.5f, 0));
		// trails.Add(currentPos + ScreenPositionOffset, drawColor, new Vector3(0.5f, 0.5f, 0));
		// }
		// if (trails.Count >= 4)
		// {
		// Main.graphics.graphicsDevice.Textures[0] = ModAsset.StarSlashGray.Value;
		// Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, trails.ToArray(), 0, trails.Count - 2);
		// }
		// }

		// Draw Slash
		List<Vertex2D> trails = new List<Vertex2D>();
		List<Vertex2D> trails_tip = new List<Vertex2D>();
		for (int i = 0; i < OldArrowTips_Smoothed.Count; i++)
		{
			Vector3 currentPos3D = OldArrowTips_Smoothed[i] + new Vector3(0, 0, CenterZ);
			Vector2 currentPos = Project(currentPos3D, ProjectionMatrix);

			Vector3 currentPos3D_Edge = OldArrowTips_Smoothed[i] * 0.6f + new Vector3(0, 0, CenterZ);
			Vector2 currentPos_Edge = Project(currentPos3D_Edge, ProjectionMatrix);

			Vector3 currentPos3D_Inner = OldArrowTips_Smoothed[i] * 0.2f + new Vector3(0, 0, CenterZ);
			Vector2 currentPos_Inner = Project(currentPos3D_Inner, ProjectionMatrix);

			Color drawColor = origColor * (i / (float)OldArrowTips_Smoothed.Count);
			float value = 1 - i / (float)OldArrowTips_Smoothed.Count;
			float timeValue = -(float)Main.time * 0.03f;
			float fade = GetFade(i);

			trails.Add(currentPos + ScreenPositionOffset, drawColor * value * fade, new Vector3(1f, value + timeValue, 0));
			trails.Add(currentPos_Inner + ScreenPositionOffset, drawColor * value * fade * 0, new Vector3(0f, value + timeValue, 0));

			trails_tip.Add(currentPos + ScreenPositionOffset, drawColor * fade, new Vector3(0.5f, 0.5f + value * 0.5f, 0));
			trails_tip.Add(currentPos_Edge + ScreenPositionOffset, drawColor * fade, new Vector3(0.3f, 0.5f + value * 0.5f, 0));
		}
		if (trails.Count >= 4)
		{
			Main.graphics.graphicsDevice.Textures[0] = ModAsset.Noise_flame_3.Value;
			Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, trails.ToArray(), 0, trails.Count - 2);
		}
		if (trails_tip.Count >= 4)
		{
			Main.graphics.graphicsDevice.Textures[0] = ModAsset.StarSlash.Value;
			Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, trails_tip.ToArray(), 0, trails_tip.Count - 2);
		}
	}

	public virtual void DrawWeapon(Vector3 start, Vector3 end, Vector3 offset)
	{
		Texture2D tex = ModContent.Request<Texture2D>(Texture).Value;
		Vector2 arrowTip = Project(start + offset, ProjectionMatrix);
		Vector2 center = Project(end + offset, ProjectionMatrix);
		Vector2 middle = (arrowTip + center) / 2f;
		float width = tex.Size().Length() / 2f;
		Vector2 dirWidth = (arrowTip - center).NormalizeSafe().RotatedBy(MathHelper.PiOver2) * width;

		List<Vertex2D> weapon = new List<Vertex2D>();
		AddVertex(weapon, center + ScreenPositionOffset, new Vector3(0, 1, 0));
		AddVertex(weapon, middle + dirWidth + ScreenPositionOffset, new Vector3(0, 0, 0));

		AddVertex(weapon, middle - dirWidth + ScreenPositionOffset, new Vector3(1, 1, 0));
		AddVertex(weapon, arrowTip + ScreenPositionOffset, new Vector3(1, 0, 0));

		if (weapon.Count >= 4)
		{
			Main.graphics.graphicsDevice.Textures[0] = tex;
			Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, weapon.ToArray(), 0, weapon.Count - 2);
		}
	}

	public void AddVertex(List<Vertex2D> bars, Vector2 screenPos, Vector3 coord)
	{
		bars.Add(screenPos, Lighting.GetColor((screenPos + Main.screenPosition).ToTileCoordinates()), coord);
	}

	public void Draw3DCurve(List<Vector3> curve, Color drawColor, Vector2 ScreenPositionOffset, float lineWidth = 3f)
	{
		for (int r = 0; r < 2; r++)
		{
			Vector2 normal = new Vector2(0, lineWidth).RotatedBy(r / 2f + MathHelper.Pi);
			List<Vertex2D> bars = new List<Vertex2D>();
			for (int i = 0; i <= curve.Count; i++)
			{
				Vector3 ringPos3D = curve[i % curve.Count];
				Vector2 ringPos2D = Project(ringPos3D, ProjectionMatrix);
				var drawColor2 = drawColor;
				if (ringPos3D.Z > CenterZ)
				{
					drawColor2 *= 0.3f;
				}
				float size = GetSizeZ(ringPos3D.Z);
				bars.Add(ringPos2D + ScreenPositionOffset, drawColor2, new Vector3(0.5f, 0.5f, 0));
				bars.Add(ringPos2D - normal * size + ScreenPositionOffset, drawColor2, new Vector3(0.7f, 0.5f, 0));
			}
			if (bars.Count >= 4)
			{
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
		}
	}

	public void DrawCurrentCoordRing(List<Vector3> curve, Color drawColor, Vector2 ScreenPositionOffset, float lineWidth = 3f)
	{
		for (int r = 0; r < 2; r++)
		{
			Vector2 normal = new Vector2(0, lineWidth).RotatedBy(r / 2f + MathHelper.Pi);
			List<Vertex2D> bars = new List<Vertex2D>();
			for (int i = 0; i <= curve.Count; i++)
			{
				Vector3 ringPos3D = curve[i % curve.Count];
				Vector2 ringPos2D = Project(ringPos3D, ProjectionMatrix);
				var drawColor2 = drawColor;
				if ((ringPos3D.Z - CenterZ) * MainAxis.Z < 0)
				{
					drawColor2 *= 0.1f;
				}
				float size = GetSizeZ(ringPos3D.Z);
				bars.Add(ringPos2D + ScreenPositionOffset, drawColor2, new Vector3(0.5f, 0.5f, 0));
				bars.Add(ringPos2D - normal * size + ScreenPositionOffset, drawColor2, new Vector3(0.7f, 0.5f, 0));
			}
			if (bars.Count >= 4)
			{
				Main.graphics.graphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
		}
	}

	public float GetFade(int indexInSmootheTrail)
	{
		float value = indexInSmootheTrail / (float)OldArrowTips_Smoothed.Count * OldTrailFade.Count;
		int index = (int)Math.Clamp(value, 0, OldTrailFade.Count - 1);
		return OldTrailFade.ToArray()[index];
	}

	public void DrawBloom()
	{
	}

	public void DrawWarp(VFXBatch spriteBatch)
	{
		if (OldArrowTips_Smoothed is null)
		{
			return;
		}
		List<Vertex2D> trails = new List<Vertex2D>();

		for (int i = 0; i < OldArrowTips_Smoothed.Count; i++)
		{
			Vector3 currentPos3D = OldArrowTips_Smoothed[i] * 0.95f + new Vector3(0, 0, CenterZ);
			Vector2 currentPos = Project(currentPos3D, ProjectionMatrix);

			Vector3 currentPos3D_Inner = OldArrowTips_Smoothed[i] * 0.2f + new Vector3(0, 0, CenterZ);
			Vector2 currentPos_Inner = Project(currentPos3D_Inner, ProjectionMatrix);

			Vector2 warpDir = currentPos.RotatedBy(MathHelper.PiOver2).NormalizeSafe() * 10f;
			if (i >= 1)
			{
				Vector3 currentPos3D_Old = OldArrowTips_Smoothed[i - 1] + new Vector3(0, 0, CenterZ);
				Vector2 currentPos_Old = Project(currentPos3D_Old, ProjectionMatrix);
				warpDir = currentPos - currentPos_Old;
			}
			warpDir *= 0.05f;
			float value = 1 - i / (float)OldArrowTips_Smoothed.Count;
			float timeValue = -(float)Main.time * 0.03f;
			Color drawColor = new Color(0.5f + warpDir.X, 0.5f + warpDir.Y, (1 - value) * 0.1f * GetFade(i), 0);
			Color drawColorInner = new Color(0.5f + warpDir.X, 0.5f + warpDir.Y, 0, 0);

			trails.Add(currentPos + ScreenPositionOffset, drawColor, new Vector3(1f, value + timeValue, 0));
			trails.Add(currentPos_Inner + ScreenPositionOffset, drawColorInner, new Vector3(0f, value + timeValue, 0));
		}
		spriteBatch.Draw(ModAsset.Noise_flame_3.Value, trails, PrimitiveType.TriangleStrip);
	}
}