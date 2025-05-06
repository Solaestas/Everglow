namespace Everglow.Yggdrasil.Common;

public sealed class YggdrasilManualMusicRegistration : ILoadable
{
	public void Load(Mod mod)
	{
		MusicLoader.AddMusic(mod, ModAsset.KelpCurtainBGM_Path);
		MusicLoader.AddMusic(mod, ModAsset.OldKelpCurtainBGM_Path);
		MusicLoader.AddMusic(mod, ModAsset.NewYggdrasilTownBGM_Path);
		MusicLoader.AddMusic(mod, ModAsset.YggdrasilTownBGM_Path);
		MusicLoader.AddMusic(mod, ModAsset.KingJellyBallBGM_Path);
		MusicLoader.AddMusic(mod, ModAsset.SquamousShellBGM_Path);
		MusicLoader.AddMusic(mod, ModAsset.OriginPylonBGM_Path);
		MusicLoader.AddMusic(mod, ModAsset.Arena_BGM_Path);
	}

	public void Unload()
	{
	}
}