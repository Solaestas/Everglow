using Everglow.Commons.Mechanics.Mission.Core;
using Everglow.Commons.Mechanics.Mission.PlayerSide.Enums;
using Everglow.Commons.Mechanics.Mission.Presentation;
using Everglow.Commons.UI.UIElements;
using Everglow.Commons.Utilities;
using Everglow.Commons.Vertex;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements;

public class UIMissionBackground : UIBlock
{
	private static readonly Color InitialColor = new Color(1f, 1f, 1f, 0f) * 0.8f;

	private PlayerMissionState? poolType = null;

	private MissionType? missionType = null;

	private float chainMovement = 0;

	private Color PoolTypeColor => ColorDefinition.GetMissionStateColor(poolType);

	private Color MissionTypeColor => ColorDefinition.GetMissionTypeColor(missionType);

	public void SetSpectrumColor(PlayerMissionState? poolType, MissionType? missionType)
	{
		this.poolType = poolType;
		this.missionType = missionType;
	}

	public void SetChainValue(float value)
	{
		chainMovement = value;
	}

	public override void Draw(SpriteBatch sb)
	{
		base.Draw(sb);
		Rectangle sourceRectangle = new Rectangle(0, 0, HitBox.Width, HitBox.Height);
		sb.Draw(ModAsset.Marble_Texture.Value, HitBox, sourceRectangle, new Color(1f, 1f, 1f, 1));
	}

	private void DrawChain(SpriteBatch sb)
	{
		var sBS = GraphicsUtils.GetState(Main.spriteBatch).Value;
		Main.spriteBatch.End();
		Main.spriteBatch.Begin(sBS);

		Rectangle listHitBox = MissionContainer.List.HitBox;
		Texture2D texture = ModAsset.MirrorChain.Value;
		float scale = MissionContainer.Scale;
		float width = texture.Width * scale;
		float height = texture.Height * scale;

		float globalTexCoordOffset = chainMovement / height - 0.26f; // + 0.07f;

		// Draw mirrior chains (Left. Move with mission items synchronously)
		var vertices = new List<Vertex2D>();
		{
			float startX = listHitBox.X + 5 * scale;
			float endX = startX + width;

			float startY = listHitBox.Y - 2 * scale - 120 * scale;
			float endY = startY + height * 7f;

			float startTexCoordY = 0 - 0.66f;
			float endTexCoordY = startTexCoordY + (endY - startY) / height;

			float resourceOffset = 0.23f;
			startTexCoordY -= resourceOffset;
			endTexCoordY -= resourceOffset;

			startTexCoordY -= globalTexCoordOffset;
			endTexCoordY -= globalTexCoordOffset;

			vertices.Add(new Vector2(startX, startY), Color.White, new(0, startTexCoordY, 0));
			vertices.Add(new Vector2(endX, startY), Color.White, new(1, startTexCoordY, 0));
			vertices.Add(new Vector2(startX, endY), Color.White, new(0, endTexCoordY, 0));
			vertices.Add(new Vector2(endX, endY), Color.White, new(1, endTexCoordY, 0));
		}

		Main.graphics.GraphicsDevice.Textures[0] = texture;
		Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

		// Draw mirrior chains (Right. Used as scrollbar)
		vertices = [];
		{
			float startX = listHitBox.X + listHitBox.Width - 18 * scale;
			float endX = startX + width;

			float startY = listHitBox.Y - 272 * scale;
			float endY = startY + height * 9f;

			float startTexCoordY = 0 - 0.42f;
			float endTexCoordY = startTexCoordY + (endY - startY) / height;

			startTexCoordY += globalTexCoordOffset;
			endTexCoordY += globalTexCoordOffset;

			vertices.Add(new Vector2(startX, startY), Color.White, new(0, startTexCoordY, 0));
			vertices.Add(new Vector2(endX, startY), Color.White, new(1, startTexCoordY, 0));
			vertices.Add(new Vector2(startX, endY), Color.White, new(0, endTexCoordY, 0));
			vertices.Add(new Vector2(endX, endY), Color.White, new(1, endTexCoordY, 0));
		}
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

		// Draw mirror chains (Above. Used to reflect the spectrum）
		vertices = [];
		{
			var baseCoord = new Vector2(HitBox.X, HitBox.Y);
			var vLeft = baseCoord + new Vector2(86, 284f) * scale;
			var vRight = baseCoord + new Vector2(448, 136f) * scale;
			var xOffset = width * MathF.Sin(MathHelper.PiOver4 / 2);
			var yOffset = width * MathF.Cos(MathHelper.PiOver4 / 2);

			var distance = Vector2.Distance(vLeft, vRight);
			var texCoordXDiff = distance / height;
			var startTexCoordY = globalTexCoordOffset;
			var endTexCoordY = texCoordXDiff + globalTexCoordOffset;

			vertices.Add(new Vector2(vLeft.X, vLeft.Y), Color.White, new(0, startTexCoordY, 0));
			vertices.Add(new Vector2(vLeft.X + xOffset, vLeft.Y + yOffset), Color.White, new(1, startTexCoordY, 0));
			vertices.Add(new Vector2(vRight.X, vRight.Y), Color.White, new(0, endTexCoordY, 0));
			vertices.Add(new Vector2(vRight.X + xOffset, vRight.Y + yOffset), Color.White, new(1, endTexCoordY, 0));
		}
		Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
	}
}
