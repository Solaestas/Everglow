using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.UI.UIElements;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

public class UIMissionBackground : UIBlock
{
	private static readonly Color InitialColor = new Color(1f, 1f, 1f, 0f) * 0.8f;

	private PoolType? poolType = null;

	private MissionType? missionType = null;

	private Color PoolTypeColor => poolType switch
	{
		PoolType.Accepted => new Color(0f, 1f, 0f, 0f),
		PoolType.Available => new Color(1f, 1f, 0f, 0f),
		PoolType.Failed => new Color(1f, 0f, 0f, 0f),
		PoolType.Overdue => new Color(0.5f, 0.2f, 0.2f, 0.1f),
		PoolType.Completed => new Color(0f, 0f, 1f, 0f),
		null => InitialColor,
		_ => InitialColor,
	};

	private Color MissionTypeColor => missionType switch
	{
		MissionType.None => new Color(0f, 0f, 0f, 0.1f),
		MissionType.MainStory => new Color(1f, 1f, 0f, 0f),
		MissionType.SideStory => new Color(1f, 0f, 1f, 0f),
		MissionType.Legendary => new Color(
			MathF.Sin((float)Main.timeForVisualEffects * 0.04f),
			MathF.Cos((float)Main.timeForVisualEffects * 0.03f),
			MathF.Sin((float)Main.timeForVisualEffects * 0.03f),
			0f),
		MissionType.Achievement => new Color(0f, 1f, 0f, 0f),
		MissionType.Daily => new Color(0f, 0f, 1f, 0f),
		MissionType.Challenge => new Color(1f, 0f, 0f, 0f),
		null => InitialColor,
		_ => InitialColor,
	};

	public void SetSpectrumColor(PoolType? poolType, MissionType? missionType)
	{
		this.poolType = poolType;
		this.missionType = missionType;
	}

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);

		sb.Draw(ModAsset.Mission_MarbleBoard.Value, HitBox, Color.White);
		sb.Draw(ModAsset.Mission_MarbleBoard_background.Value, HitBox, Color.White);

		var scale = MissionContainer.Scale;
		var basePos = HitBox.TopLeft();

		// Laser Prism
		var laserPrism = ModAsset.LaserPrism.Value;
		var laserPrismPos = basePos + scale * (new Vector2(45, 203) + laserPrism.Size() / 2);

		// Crystal
		var crystal = ModAsset.TetragonalCrystal.Value;
		var crystalPos1 = basePos + scale * new Vector2(239, 413);
		var crystalPos2 = basePos + scale * new Vector2(618, 52);

		// Glass Brick
		var glassBrick = ModAsset.GlassBrick.Value;
		var glassBrickPos = basePos + scale * (new Vector2(233, 203) + glassBrick.Size() / 2);

		var glassBrick2 = ModAsset.GlassBrick225.Value;
		var glassBrickPos2 = basePos + scale * (new Vector2(420, 47) + glassBrick.Size() / 2);

		// Reflect Chain
		var chain = ModAsset.MirrorChain.Value;
		var chainCenter = basePos + scale * new Vector2(274, 210);
		var chainRotation = MathHelper.PiOver4 * 3 / 2;

		#region Spectrum

		var sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin();

		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.Trail_1.Value;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;

		var texCoordOffset = (float)Main.timeForVisualEffects * -0.02f;
		{
			var spectrumColor = InitialColor;
			var spectrumPos = laserPrismPos;
			var radius = 8;
			var length = 60;

			var texCoordX = length / 100f;

			var vertices = new List<Vertex2D>();
			vertices.Add(spectrumPos + new Vector2(0, radius) * scale, spectrumColor, new(0 + texCoordOffset, 0, 0));
			vertices.Add(spectrumPos + new Vector2(0, -radius) * scale, spectrumColor, new(0 + texCoordOffset, 1, 0));
			vertices.Add(spectrumPos + new Vector2(length, radius) * scale, spectrumColor, new(texCoordX + texCoordOffset, 0, 0));
			vertices.Add(spectrumPos + new Vector2(length, -radius) * scale, spectrumColor, new(texCoordX + texCoordOffset, 1, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
		}

		{
			var spectrumColor = MissionTypeColor;
			var spectrumPos = laserPrismPos + new Vector2(60, 0) * scale;
			var radius = 8;
			var length = 60;

			var texCoordX = length / 100f;

			var vertices = new List<Vertex2D>();
			vertices.Add(spectrumPos + new Vector2(0, radius) * scale, MissionTypeColor, new(0 + texCoordOffset, 0, 0));
			vertices.Add(spectrumPos + new Vector2(0, -0) * scale, MissionTypeColor, new(0 + texCoordOffset, 0.5f, 0));
			vertices.Add(spectrumPos + new Vector2(length, radius) * scale, MissionTypeColor, new(texCoordX + texCoordOffset, 0, 0));
			vertices.Add(spectrumPos + new Vector2(length, -0) * scale, MissionTypeColor, new(texCoordX + texCoordOffset, 0.5f, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

			vertices = new List<Vertex2D>();
			vertices.Add(spectrumPos + new Vector2(0, 0) * scale, InitialColor, new(0 + texCoordOffset, 0.5f, 0));
			vertices.Add(spectrumPos + new Vector2(0, -radius) * scale, InitialColor, new(0 + texCoordOffset, 1, 0));
			vertices.Add(spectrumPos + new Vector2(length, 0) * scale, InitialColor, new(texCoordX + texCoordOffset, 0.5f, 0));
			vertices.Add(spectrumPos + new Vector2(length, -radius) * scale, InitialColor, new(texCoordX + texCoordOffset, 1, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
		}

		{
			var spectrumPos = laserPrismPos + new Vector2(120, 0) * scale;
			var radius = 8;
			var length = 100;

			var texCoordX = length / 100f;

			var vertices = new List<Vertex2D>();
			vertices.Add(spectrumPos + new Vector2(0, radius) * scale, MissionTypeColor, new(0 + texCoordOffset, 0, 0));
			vertices.Add(spectrumPos + new Vector2(0, -0) * scale, MissionTypeColor, new(0 + texCoordOffset, 0.5f, 0));
			vertices.Add(spectrumPos + new Vector2(length, radius) * scale, MissionTypeColor, new(texCoordX + texCoordOffset, 0, 0));
			vertices.Add(spectrumPos + new Vector2(length, -0) * scale, MissionTypeColor, new(texCoordX + texCoordOffset, 0.5f, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

			vertices = new List<Vertex2D>();
			vertices.Add(spectrumPos + new Vector2(0, 0) * scale, PoolTypeColor, new(0 + texCoordOffset, 0.5f, 0));
			vertices.Add(spectrumPos + new Vector2(0, -radius) * scale, PoolTypeColor, new(0 + texCoordOffset, 1, 0));
			vertices.Add(spectrumPos + new Vector2(length, 0) * scale, PoolTypeColor, new(texCoordX + texCoordOffset, 0.5f, 0));
			vertices.Add(spectrumPos + new Vector2(length, -radius) * scale, PoolTypeColor, new(texCoordX + texCoordOffset, 1, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
		}

		{
			var spectrumPos = chainCenter + new Vector2(-5, 0) * scale;
			var radius = 8;
			var length = 224;

			var texCoordX = length / 100f;

			var vertices = new List<Vertex2D>()
				.Add(spectrumPos + new Vector2(0, radius).RotatedBy(-MathHelper.PiOver4) * scale, MissionTypeColor, new(0 + texCoordOffset, 0, 0))
				.Add(spectrumPos + new Vector2(0, -0).RotatedBy(-MathHelper.PiOver4) * scale, MissionTypeColor, new(0 + texCoordOffset, 0.5f, 0))
				.Add(spectrumPos + new Vector2(length, radius).RotatedBy(-MathHelper.PiOver4) * scale, MissionTypeColor, new(texCoordX + texCoordOffset, 0, 0))
				.Add(spectrumPos + new Vector2(length, -0).RotatedBy(-MathHelper.PiOver4) * scale, MissionTypeColor, new(texCoordX + texCoordOffset, 0.5f, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

			vertices = new List<Vertex2D>()
				.Add(spectrumPos + new Vector2(0, 0).RotatedBy(-MathHelper.PiOver4) * scale, PoolTypeColor, new(0 + texCoordOffset, 0.5f, 0))
				.Add(spectrumPos + new Vector2(0, -radius).RotatedBy(-MathHelper.PiOver4) * scale, PoolTypeColor, new(0 + texCoordOffset, 1, 0))
				.Add(spectrumPos + new Vector2(length, 0).RotatedBy(-MathHelper.PiOver4) * scale, PoolTypeColor, new(texCoordX + texCoordOffset, 0.5f, 0))
				.Add(spectrumPos + new Vector2(length, -radius).RotatedBy(-MathHelper.PiOver4) * scale, PoolTypeColor, new(texCoordX + texCoordOffset, 1, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
		}

		{
			var spectrumPos = chainCenter + new Vector2(-6, 0) * scale + new Vector2(224, 0).RotatedBy(-MathHelper.PiOver4) * scale;
			var radius = 8;
			var length = 188;

			var texCoordX = length / 100f;

			var vertices = new List<Vertex2D>()
				.Add(spectrumPos + new Vector2(0, radius) * scale, MissionTypeColor, new(0 + texCoordOffset, 0, 0))
				.Add(spectrumPos + new Vector2(0, -0) * scale, MissionTypeColor, new(0 + texCoordOffset, 0.5f, 0))
				.Add(spectrumPos + new Vector2(length, radius) * scale, MissionTypeColor, new(texCoordX + texCoordOffset, 0, 0))
				.Add(spectrumPos + new Vector2(length, -0) * scale, MissionTypeColor, new(texCoordX + texCoordOffset, 0.5f, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

			vertices = new List<Vertex2D>()
				.Add(spectrumPos + new Vector2(0, 0) * scale, PoolTypeColor, new(0 + texCoordOffset, 0.5f, 0))
				.Add(spectrumPos + new Vector2(0, -radius) * scale, PoolTypeColor, new(0 + texCoordOffset, 1, 0))
				.Add(spectrumPos + new Vector2(length, 0) * scale, PoolTypeColor, new(texCoordX + texCoordOffset, 0.5f, 0))
				.Add(spectrumPos + new Vector2(length, -radius) * scale, PoolTypeColor, new(texCoordX + texCoordOffset, 1, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
		}

		{
			var spectrumPos = glassBrickPos + new Vector2(-2, -1) * scale;
			var radius = 8;
			var length = 198;

			var texCoordX = length / 100f;

			var vertices = new List<Vertex2D>()
				.Add(spectrumPos + new Vector2(0, 0) * scale, MissionTypeColor, new(0 + texCoordOffset, 0.5f, 0))
				.Add(spectrumPos + new Vector2(-radius, 0) * scale, MissionTypeColor, new(0 + texCoordOffset, 1, 0))
				.Add(spectrumPos + new Vector2(0, length) * scale, MissionTypeColor, new(texCoordX + texCoordOffset, 0.5f, 0))
				.Add(spectrumPos + new Vector2(-radius, length) * scale, MissionTypeColor, new(texCoordX + texCoordOffset, 1, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

			vertices = new List<Vertex2D>()
				.Add(spectrumPos + new Vector2(radius, 0) * scale, PoolTypeColor, new(0 + texCoordOffset, 0, 0))
				.Add(spectrumPos + new Vector2(-0, 0) * scale, PoolTypeColor, new(0 + texCoordOffset, 0.5f, 0))
				.Add(spectrumPos + new Vector2(radius, length) * scale, PoolTypeColor, new(texCoordX + texCoordOffset, 0, 0))
				.Add(spectrumPos + new Vector2(-0, length) * scale, PoolTypeColor, new(texCoordX + texCoordOffset, 0.5f, 0));
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
		}

		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		#endregion

		sb.Draw(laserPrism, laserPrismPos, null, Color.White, 0, laserPrism.Size() / 2, scale, SpriteEffects.None, 0);
		sb.Draw(crystal, crystalPos1, null, Color.White, 0, crystal.Size() / 2, scale, SpriteEffects.None, 0);
		sb.Draw(crystal, crystalPos2, null, Color.White, 0, crystal.Size() / 2, scale, SpriteEffects.None, 0);
		sb.Draw(glassBrick, glassBrickPos, null, Color.White, 0, glassBrick.Size() / 2, scale, SpriteEffects.None, 0);
		sb.Draw(glassBrick2, glassBrickPos2, null, Color.White, 0, glassBrick.Size() / 2, scale * 1.18f, SpriteEffects.None, 0);

		for (int i = -2; i <= 2; i++)
		{
			var chainPos = chainCenter + i * new Vector2(chain.Height * scale, 0).RotatedBy(MathHelper.PiOver2 + chainRotation);
			sb.Draw(chain, chainPos, null, Color.White, chainRotation, chain.Size() / 2, scale, SpriteEffects.None, 0);
		}
	}
}