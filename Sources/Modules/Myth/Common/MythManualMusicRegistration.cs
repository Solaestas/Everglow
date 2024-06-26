namespace Everglow.Myth.Common;

public sealed class MythManualMusicRegistration : ILoadable
{
	public void Load(Mod mod)
	{
		// Title Music
		MusicLoader.AddMusic(mod, "Myth/Musics/BaseMusic");

		// MusicLoader.AddMusic(mod, "Myth/Musics/MenuMusic");
		// Moth Musics
		MusicLoader.AddMusic(mod, "Myth/Musics/MothBiome");
		MusicLoader.AddMusic(mod, "Myth/Musics/MothBiomeOld");

		// Other Moth Music
		MusicLoader.AddMusic(mod, "Myth/Musics/MothFightingAlt");


		// Acytaea
		MusicLoader.AddMusic(mod, "Myth/Musics/AcytaeaFighting");

		// Tusk Musics
		MusicLoader.AddMusic(mod, "Myth/Musics/TuskBiome");
		MusicLoader.AddMusic(mod, "Myth/Musics/TuskTension");
		MusicLoader.AddMusic(mod, "Myth/Musics/TuskFighting");

		// Lantern Moon Musics
		MusicLoader.AddMusic(mod, "Myth/Musics/DashCore");
	}

	public void Unload()
	{
	}
}