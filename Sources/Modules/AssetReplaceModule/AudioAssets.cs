using Everglow.Sources.Modules.MythModule.Common;
using Microsoft.Xna.Framework.Audio;
using ReLogic.Content;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.Audio;
using Terraria.Initializers;

namespace Everglow.Sources.Modules.AssetReplaceModule
{
    internal class AudioAssets
    {
        public Asset<ManualMusicRegistrationExample>[] MusicSwapBacks = new Asset<ManualMusicRegistrationExample>[3];
    //    public ClassicBar ClassicBar = new();
    //    public FancyBar FancyBar = new();
    //    public HorizontalBar HorizontalBar = new();

        public void LoadMusic() {
            MusicSwapBacks[0] = AssetReplaceModule.GetAsset($"Musics/MothFighting");
            MusicSwapBacks[1] = AssetReplaceModule.GetAsset($"Musics/MothFightingAlt");
            MusicSwapBacks[2] = AssetReplaceModule.GetAsset($"Musics/MothFightingOld2");

            //        ClassicBar.LoadTextures("UISkinMyth");
            //        FancyBar.LoadTextures("UISkinMyth");
            //        HorizontalBar.LoadTextures("UISkinMyth");
        }

        public void Apply() {
            AssetReplaceModule.GetAsset(MusicSwapBacks[0]);

            MothFightingMusic.ReplaceAudios();
            FancyBar.ReplaceTextures();
        //    HorizontalBar.ReplaceTextures();
        }
    }
}
