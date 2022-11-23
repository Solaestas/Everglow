namespace Everglow.Sources.Modules.YggdrasilModule.Common
{
    public sealed class YggdrasilManualMusicRegistration : ILoadable
    {
        public void Load(Mod mod)
        {
            MusicLoader.AddMusic(mod, "Sources/Modules/YggdrasilModule/Musics/KelpCurtainBGM");
            MusicLoader.AddMusic(mod, "Sources/Modules/YggdrasilModule/Musics/YggdrasilTownBGM");
        }

        public void Unload()
        {
        }
    }
}