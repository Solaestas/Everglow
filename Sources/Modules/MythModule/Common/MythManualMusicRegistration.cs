namespace Everglow.Sources.Modules.MythModule.Common
{

    public sealed class ManualMusicRegistrationExample : ILoadable
    {
        public void Load(Mod mod)
        {
            // moth
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothBiome");
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFighting");
            // tusk
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/TuskBiome");
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/TuskTension");
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/TuskFighting");
        }
        public void Unload()
        {

        }
    }
}
