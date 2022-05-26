namespace Everglow.Sources.Modules.MythModule.Common
{

    public sealed class ManualMusicRegistrationExample : ILoadable
    {
        public void Load(Mod mod)
        {
            MusicLoader.AddMusic(mod, "Sources/Modules/MythModule/Musics/MothBiome");
        }
        public void Unload()
        {

        }
    }
}
