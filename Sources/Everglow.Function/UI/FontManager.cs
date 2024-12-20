using FontStashSharp;

namespace Everglow.Commons.UI;

public class FontManager : ILoadable
{
	public static FontManager Instance { get; private set; }

	public static FontSystem FusionPixel12;
	public Dictionary<string, FontSystem> Fonts = [];

	public FontManager()
	{
		Instance = this;
	}

	public void Load(Mod mod)
	{
		mod.GetFileNames().ForEach(n =>
		{
			if (Path.GetExtension(n) == ".ttf")
			{
				var fs = new FontSystem();
				fs.AddFont(mod.GetFileBytes(n));
				Fonts.Add(Path.GetFileNameWithoutExtension(n), fs);
			}
		});
		FusionPixel12 = Fonts["FusionPixel12"];
	}

	public void Unload()
	{
	}
}