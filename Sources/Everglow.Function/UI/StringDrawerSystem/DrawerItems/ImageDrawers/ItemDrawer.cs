using FontStashSharp;
using Terraria.GameContent;

namespace Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;

internal class ItemDrawer : DrawerItem
{
	public int ItemType;

	public Vector2 ItemBlockSize;

	public string ItemStack;
	public Color ItemStackColor;
	public int ItemStackFontSize;

	public override string ToString()
	{
		return $"{Line} Item:{ItemType}";
	}

	public override void Draw(SpriteBatch sb)
	{
		//sb.Draw(
		//	TextureAssets.MagicPixel.Value,
		//	new Rectangle((int)Position.X, (int)Position.Y,
		//	(int)ItemBlockSize.X, (int)ItemBlockSize.Y), Color.White);

		var tex = TextureAssets.Item[ItemType].Value;
		float scale = Math.Min(ItemBlockSize.X / tex.Width, ItemBlockSize.Y / tex.Height);
		sb.Draw(tex, Position + ItemBlockSize / 2f - tex.Size() * scale / 2f, null,
			Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);

		var font = FontManager.FusionPixel12.GetFont(ItemStackFontSize);
		var stackSize = new Vector2(font.MeasureString(ItemStack).X, font.LineHeight);
		sb.DrawString(font, ItemStack, Position + ItemBlockSize * 0.8f - stackSize / 2f, ItemStackColor,
			null, 0, default, 0, 0, 0, TextStyle.None, FontSystemEffect.Stroked, 1);
	}

	public override DrawerItem GetInstance(StringDrawer stringDrawer, string originalText, string name, StringParameters stringParameters)
	{
		ItemDrawer drawerItem = (ItemDrawer)Activator.CreateInstance(GetType());
		drawerItem.Init(stringDrawer, originalText, name, stringParameters);
		return drawerItem;
	}

	public override Vector2 GetSize()
	{
		var font = FontManager.FusionPixel12.GetFont(ItemStackFontSize);
		var stackSize = new Vector2(font.MeasureString(ItemStack).X, font.LineHeight);
		var fs = ItemBlockSize * 0.8f + stackSize / 2f;
		return new Vector2(Math.Max(ItemBlockSize.X, fs.X), Math.Max(ItemBlockSize.Y, fs.Y));
	}

	public override void Init(StringDrawer stringDrawer, string originalText, string name, StringParameters stringParameters)
	{
		base.Init(stringDrawer, originalText, name, stringParameters);
		ItemType = stringParameters.GetInt("Type",
			stringDrawer.DefaultParameters.GetInt("ItemType", 0));
		ItemBlockSize = stringParameters.GetVector2("BlockSize",
			stringDrawer.DefaultParameters.GetVector2("ItemBlockSize", new Vector2(26f)));
		ItemStack = stringParameters.GetString("Stack",
			stringDrawer.DefaultParameters.GetString("ItemStack", string.Empty));
		ItemStackColor = stringParameters.GetColor("StackColor",
			stringDrawer.DefaultParameters.GetColor("ItemStackColor", Color.White));
		ItemStackFontSize = stringParameters.GetInt("StackFontSize",
			stringDrawer.DefaultParameters.GetInt("ItemStackFontSize", 16));
	}

	public override float WordWrap(ref int index, List<DrawerItem> drawerItems, ref int line, float width, float originWidth)
	{
		Line++;
		line++;
		return originWidth - GetSize().X;
	}
}