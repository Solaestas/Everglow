namespace Everglow.Yggdrasil.Common;

public sealed class YggdrasilManualMusicRegistration : ILoadable
{
	public void Load(Mod mod)
	{
		MusicLoader.AddMusic(mod, "Yggdrasil/Musics/KelpCurtainBGM");
		MusicLoader.AddMusic(mod, "Yggdrasil/Musics/YggdrasilTownBGM");
	}

	public void Unload()
	{
	}
}