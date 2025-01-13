namespace Everglow.Commons.Menu.Music;

public sealed class MenuManualMusicRegistration : ILoadable
{
	public void Load(Mod mod)
	{
		MusicLoader.AddMusic(mod, ModAsset.MenuMusic_Path);
	}

	public void Unload()
	{
	}
}