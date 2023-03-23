namespace Everglow.TwilightForest.Common;

public sealed class TwilightForestManualMusicRegistration : ILoadable
{
	public void Load(Mod mod)
	{
		MusicLoader.AddMusic(mod, "TwilightForest/Musics/PlaceholderTwilightForestBGM");
		MusicLoader.AddMusic(mod, "TwilightForest/Musics/PlaceholderTwilightForestUndergroundBGM");
	}
	public void Unload()
	{
	}
}
