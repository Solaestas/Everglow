namespace Everglow.Myth.Common;

public sealed class MythManualMusicRegistration : ILoadable
{
	public void Load(Mod mod)
	{
		// Title Music
		MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/BaseMusic");
		MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MenuMusic");
		// Moth Musics
		MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothBiome");
		MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFighting");
		// Other Moth Music
		//MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFightingAlt");
		//MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFightingOld");
		//MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFightingOld2");
		// Acytaea
		MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/AcytaeaFighting");

		// Tusk Musics
		MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/TuskBiome");
		MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/TuskTension");
		MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/TuskFighting");
		// Lantern Moon Musics
		MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/DashCore");
	}
	public void Unload()
	{
	}
}
