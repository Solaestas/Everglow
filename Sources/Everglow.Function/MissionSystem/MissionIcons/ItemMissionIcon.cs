using Terraria.GameContent;

namespace Everglow.Commons.MissionSystem.MissionIcons;

public class ItemMissionIcon : IMissionIcon
{
	private ItemMissionIcon()
	{
	}

	private int itemType;
	private string tooltip;

	public string Tooltip => tooltip;

	public void Draw(SpriteBatch spriteBatch, Rectangle destinationRectangle)
	{
		var drawCenter = new Vector2(
			destinationRectangle.X + destinationRectangle.Width / 2,
			destinationRectangle.Y + destinationRectangle.Height / 2);

		if (itemType >= TextureAssets.Item.Length)
		{
			throw new InvalidDataException();
		}

		var texture = TextureAssets.Item[itemType].Value;

		Rectangle? frameRect = null;
		var origin = texture.Size() / 2;
		if (Main.itemAnimationsRegistered.Contains(itemType))
		{
			frameRect = Main.itemAnimations[itemType].GetFrame(texture);
			origin = new Vector2(frameRect.Value.Width, frameRect.Value.Height) / 2;
		}

		spriteBatch.Draw(texture, drawCenter, frameRect, Color.White, 0, origin, 1f, SpriteEffects.None, 0);
	}

	public static ItemMissionIcon Create(int itemType, string tooltip = null)
	{
		if (itemType >= TextureAssets.Item.Length)
		{
			throw new InvalidDataException();
		}

		return new ItemMissionIcon()
		{
			itemType = itemType,
			tooltip = tooltip ?? string.Empty,
		};
	}
}