using Everglow.Commons.Templates.Furniture.Elevator;
using Terraria.Enums;
using Terraria.ObjectData;

namespace Everglow.Yggdrasil.Common.Elevator.Tiles;

public class YggdrasilTownElevator_Indicator_Lamp : FloorIndicatorTile
{
	public override void SetCustomDefaults() => base.SetCustomDefaults();

	public override void ModifyLight(int i, int j, ref float r, ref float g, ref float b)
	{
		var tile = Main.tile[i, j];
		if (tile.TileFrameY is >= 54 and <= 72)
		{
			Lighting.AddLight(new Vector2(i * 16, j * 16), new Vector3(1f, 0.8f, 0.3f));
		}
	}

	public override void PostDraw(int i, int j, SpriteBatch spriteBatch)
	{
		Texture2D glow = ModAsset.YggdrasilTownElevator_Indicator_Lamp_glow.Value;
		var zero = new Vector2(Main.offScreenRange);
		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}
		var tile = Main.tile[i, j];
		Rectangle frame = new Rectangle(tile.TileFrameX, tile.TileFrameY, 32, 16);
		spriteBatch.Draw(glow, new Vector2(i, j) * 16 - Main.screenPosition + zero + new Vector2(-8, 0), frame, new Color(1f, 1f, 1f, 0), 0, Vector2.zeroVector, 1f, SpriteEffects.None, 0);
	}
}