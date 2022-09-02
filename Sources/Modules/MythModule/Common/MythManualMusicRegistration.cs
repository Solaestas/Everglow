namespace Everglow.Sources.Modules.MythModule.Common
{

    public sealed class ManualMusicRegistrationExample : ILoadable
    {
        public void Load(Mod mod)
        {
            // Moth Musics
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothBiome");
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFighting");
            // Acytaea
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/AcytaeaFighting");
        }
        public void Unload()
        {

        }
    }
}
