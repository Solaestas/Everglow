using FontStashSharp;

namespace Everglow.Commons.UI.StringDrawerSystem.DrawerItems.TextDrawers;

public class TextDrawer : DrawerItem
{
	private static readonly Dictionary<string, float> _fontOffset = new()
	{
		{ nameof(FontManager.FusionPixel12), -0.1f },
	};

	public string Text = string.Empty;
	public Color Color = Color.White;
	public Vector2 Offset = Vector2.Zero;
	public Vector2 Scale = Vector2.One;
	public Vector2 Origin = Vector2.Zero;
	public float Rotation = 0f;
	public TextStyle TextStyle = TextStyle.None;
	public FontSystemEffect FontSystemEffect = FontSystemEffect.None;
	public float CharacterSpacing = 0f;
	public int EffectAmount = 0;
	public float FontSize = 30f;
	public float LayerDepth = 0f;
	public FontSystem FontSystem;

	public SpriteFontBase Font => FontSystem.GetFont(FontSize);

	public static TextDrawer Create(StringDrawer stringDrawer, string text, int line)
	{
		TextDrawer drawerItem = new TextDrawer();
		StringParameters stringParameters = new StringParameters();
		stringParameters["Text"] = text;
		drawerItem.Init(stringDrawer, text, nameof(TextDrawer), stringParameters);
		drawerItem.Line = line;
		return drawerItem;
	}

	public override string ToString() => $"{Line} {Text}";

	protected virtual Vector2 GetTextSize(string text) =>
		new Vector2(
			Font.MeasureString(text, Scale, CharacterSpacing, 0, FontSystemEffect, EffectAmount).X,
			Font.LineHeight * text.ReplaceLineEndings().Split(Environment.NewLine).Length);

	/// <summary>
	/// Wrap text width specific width.
	/// </summary>
	/// <param name="index"></param>
	/// <param name="drawerItems"></param>
	/// <param name="line"></param>
	/// <param name="width"></param>
	/// <param name="originWidth"></param>
	/// <returns></returns>
	public override float WordWrap(ref int index, List<DrawerItem> drawerItems, ref int line, float width, float originWidth)
	{
		if (string.IsNullOrWhiteSpace(Text))
		{
			return 0f;
		}

		if (width < GetTextSize(Text[0].ToString()).X)
		{
			width = originWidth;
			Line++;
			line++;
		}

		string text = Text;
		int endIndex = text.Length - 1;
		int middle;
		int startIndex = 0, oIndex = 0;

		while (true)
		{
			middle = (startIndex + endIndex) / 2;
			Vector2 size = GetTextSize(text[oIndex..(middle + 1)]);

			if (size.X == width)
			{
				if (IsEnglishWordBoundary(text, middle))
				{
					if (oIndex == 0)
					{
						Text = text[oIndex..(middle + 1)];
						width = originWidth;
					}
					else
					{
						var item = Decomposition(text[oIndex..(middle + 1)], drawerItems, index);
						item.Line = ++line;
						drawerItems.Insert(++index, item);
					}
					oIndex = middle + 1;
					startIndex = oIndex;
					endIndex = text.Length - 1;
				}
				else
				{
					middle = FindPreviousEnglishWordBoundary(text, middle);
					if (middle < oIndex)
					{
						middle = oIndex;
					}

					if (oIndex == 0)
					{
						Text = text[oIndex..(middle + 1)];
						width = originWidth;
					}
					else
					{
						var item = Decomposition(text[oIndex..(middle + 1)], drawerItems, index);
						item.Line = ++line;
						drawerItems.Insert(++index, item);
					}
					oIndex = middle + 1;
					startIndex = oIndex;
					endIndex = text.Length - 1;
				}
			}
			else if (size.X < width)
			{
				startIndex = middle + 1;
			}
			else
			{
				endIndex = middle - 1;
			}

			if (startIndex > endIndex)
			{
				if (IsEnglishWordBoundary(text, startIndex))
				{
					if (oIndex == 0)
					{
						Text = text[oIndex..startIndex];
						width = originWidth;
					}
					else
					{
						var item = Decomposition(text[oIndex..startIndex], drawerItems, index);
						item.Line = ++line;
						drawerItems.Insert(++index, item);
					}
					oIndex = startIndex;
					endIndex = text.Length - 1;

					if (startIndex >= text.Length)
					{
						return originWidth - drawerItems[index].GetSize().X;
					}
				}
				else
				{
					startIndex = FindPreviousEnglishWordBoundary(text, startIndex);
					if (startIndex < oIndex)
					{
						startIndex = oIndex;
					}

					if (oIndex == 0)
					{
						Text = text[oIndex..startIndex];
						width = originWidth;
					}
					else
					{
						var item = Decomposition(text[oIndex..startIndex], drawerItems, index);
						item.Line = ++line;
						drawerItems.Insert(++index, item);
					}
					oIndex = startIndex;
					endIndex = text.Length - 1;

					if (startIndex >= text.Length)
					{
						return originWidth - drawerItems[index].GetSize().X;
					}
				}
			}
		}
	}

	/// <summary>
	/// Helper method to check if a position is an English word boundary
	/// </summary>
	/// <param name="text"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	private bool IsEnglishWordBoundary(string text, int index)
	{
		if (index <= 0 || index >= text.Length)
			return true;

		char prevChar = text[index - 1];
		char currentChar = text[index];

		// Check if the current position is a space or punctuation
		return char.IsWhiteSpace(currentChar) || char.IsPunctuation(currentChar) ||
			   (IsEnglishCharacter(prevChar) && !IsEnglishCharacter(currentChar)) ||
			   (!IsEnglishCharacter(prevChar) && IsEnglishCharacter(currentChar));
	}

	/// <summary>
	/// Helper method to find the previous English word boundary
	/// </summary>
	/// <param name="text"></param>
	/// <param name="index"></param>
	/// <returns></returns>
	private int FindPreviousEnglishWordBoundary(string text, int index)
	{
		for (int i = index; i >= 0; i--)
		{
			if (IsEnglishWordBoundary(text, i))
			{
				return i;
			}
		}
		return 0;
	}

	/// <summary>
	/// Helper method to check if a character is an English letter
	/// </summary>
	/// <param name="c"></param>
	/// <returns></returns>
	private bool IsEnglishCharacter(char c)
	{
		return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
	}

	public virtual TextDrawer Decomposition(string text, List<DrawerItem> drawerItems, int index)
	{
		TextDrawer lastDrawer = (TextDrawer)drawerItems[index];
		TextDrawer drawerItem = (TextDrawer)Activator.CreateInstance(GetType());
		drawerItem.Text = text;
		drawerItem.Color = Color;
		drawerItem.Offset = Offset;
		drawerItem.Scale = Scale;
		drawerItem.Origin = Origin;
		drawerItem.Rotation = Rotation;
		drawerItem.TextStyle = TextStyle;
		drawerItem.FontSystemEffect = FontSystemEffect;
		drawerItem.CharacterSpacing = CharacterSpacing;
		drawerItem.EffectAmount = EffectAmount;
		drawerItem.FontSize = FontSize;
		drawerItem.LayerDepth = LayerDepth;
		drawerItem.FontSystem = FontSystem;
		drawerItem.TailLinkItems = new List<DrawerItem>(lastDrawer.TailLinkItems);
		lastDrawer.TailLinkItems.Clear();
		lastDrawer.TailLink(drawerItem);
		drawerItem.HeadLink(lastDrawer);
		return drawerItem;
	}

	public override Vector2 GetSize()
	{
		return GetTextSize(Text);
	}

	public override void Init(StringDrawer stringDrawer, string originalText, string name, StringParameters stringParameters)
	{
		base.Init(stringDrawer, originalText, name, stringParameters);
		if (Permission(stringDrawer, originalText, name, stringParameters) && stringParameters != null)
		{
			Text = stringParameters["Text"];

			Color = stringParameters.GetColor(
				"Color",
				stringDrawer.DefaultParameters.GetColor("Color", Color.White));

			Scale = stringParameters.GetVector2(
				"Scale",
				stringDrawer.DefaultParameters.GetVector2("Scale", Vector2.One));

			Origin = stringParameters.GetVector2(
				"Origin",
				stringDrawer.DefaultParameters.GetVector2("Origin"));

			Rotation = stringParameters.GetFloat(
				"Rotation",
				stringDrawer.DefaultParameters.GetFloat("Rotation"));

			TextStyle = (TextStyle)stringParameters.GetInt(
				"TextStyle",
				stringDrawer.DefaultParameters.GetInt("TextStyle"));

			FontSystemEffect = (FontSystemEffect)stringParameters.GetInt(
				"FontSystemEffect",
				stringDrawer.DefaultParameters.GetInt("FontSystemEffect"));

			CharacterSpacing = stringParameters.GetFloat(
				"CharacterSpacing",
				stringDrawer.DefaultParameters.GetFloat("CharacterSpacing"));

			EffectAmount = stringParameters.GetInt(
				"EffectAmount",
				stringDrawer.DefaultParameters.GetInt("EffectAmount"));

			FontSize = stringParameters.GetFloat(
				"FontSize",
				stringDrawer.DefaultParameters.GetFloat("FontSize", 30f));

			LayerDepth = stringParameters.GetFloat(
				"LayerDepth",
				stringDrawer.DefaultParameters.GetFloat("LayerDepth", 1f));

			var fontName = stringParameters.GetString(
				"Font",
				stringDrawer.DefaultParameters.GetString("Font", nameof(FontManager.FusionPixel12)));

			FontSystem = FontManager.Instance.Fonts[fontName];

			var fontOffset = _fontOffset.TryGetValue(fontName, out float value)
				? new Vector2(0f, GetSize().Y * value)
				: Vector2.Zero;
			Offset = stringParameters.GetVector2(
				"Offset",
				stringDrawer.DefaultParameters.GetVector2("Offset", fontOffset));
		}
		else
		{
			Text = originalText;
			FontSystem = FontManager.FusionPixel12;
		}
	}

	public override TextDrawer GetInstance(StringDrawer stringDrawer, string originalText, string name, StringParameters stringParameters)
	{
		TextDrawer drawerItem = (TextDrawer)Activator.CreateInstance(GetType());
		drawerItem.Init(stringDrawer, originalText, name, stringParameters);
		return drawerItem;
	}

	public override void Draw(SpriteBatch sb)
	{
		sb.DrawString(Font, Text, Position + Offset, Color, Scale, Rotation, Origin, LayerDepth, CharacterSpacing, 0, TextStyle, FontSystemEffect, EffectAmount);
	}
}