namespace Everglow.Sources.Modules.MythModule.Common
{

    public sealed class ManualMusicRegistrationExample : ILoadable
    {
        public void Load(Mod mod)
        {
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothBiome");
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothFighting");

        }
        public void Unload()
        {

        }
    }
}
