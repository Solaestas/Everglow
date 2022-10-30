namespace Everglow.Sources.Modules.MythModule.Common
{
    public sealed class MythManualMusicRegistration : ILoadable
    {
        public void Load(Mod mod)
        {
            // Moth Musics
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothBiome");
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFighting");
            // Acytaea
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/AcytaeaFighting");
        }
            // Other Moth Music
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFightingAlt");
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFightingOld");
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFightingOld2");
        }

        public void Unload()
        {
        }
    }
}