using Everglow.Commons.Mechanics;

namespace Everglow.Myth.LanternMoon.LanternCommon;

public class LanternMoonMusicManager : MusicHelper
{
	public override void EndMusicEffect(string path)
	{
		if (path == ModAsset.LanternMoonMusic_Pre15_Accompaniment_Head_Mod)
		{
			PlayMusic(ModAsset.LanternMoonMusic_Pre15_Accompaniment_Loop_Mod, true);
		}
		if (path == ModAsset.LanternMoonMusic_Pre15_Percussion_Head_Mod)
		{
			PlayMusic(ModAsset.LanternMoonMusic_Pre15_Percussion_Loop_Mod, true);
		}
		if (path == ModAsset.LanternMoonMusic_15_Melody_Head_Mod)
		{
			PlayMusic(ModAsset.LanternMoonMusic_15_Melody_Loop_Mod, true);
		}
	}
}