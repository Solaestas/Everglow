using Everglow.Commons.Mechanics;
using Everglow.Myth.LanternMoon.NPCs.LanternGhostKing;

namespace Everglow.Myth.LanternMoon.LanternCommon;

public class LanternMoonMusicManager : MusicHelper
{
	public int Wave15StartTimer = 360000;

	public override void PostUpdateEverything()
	{
		if (Wave15StartTimer == 0)
		{
			StartMusic(ModAsset.LanternMoonMusic_15_Accompaniment_Loop_Mod, 60, false);
			StartMusic(ModAsset.LanternMoonMusic_15_Percussion_Loop_Mod, 20, false);
			PlayMusic(ModAsset.LanternMoonMusic_Pre15_15_Transition_Mod, false);
		}
		if (Wave15StartTimer == 6 * 135)
		{
			PlayMusic(ModAsset.LanternMoonMusic_Pre15_15_Transition_Mod, false);
		}
		if (Wave15StartTimer == 60 * 14)
		{
			PlayMusic(ModAsset.LanternMoonMusic_15_Melody_Head_Mod, false);
		}
		if (Wave15StartTimer == 60 * 32 - 2)
		{
			StartMusic(ModAsset.LanternMoonMusic_15_Accompaniment_Loop_Mod, 3, true);
			StartMusic(ModAsset.LanternMoonMusic_15_Melody_Loop_Mod, 3, true);
			StartMusic(ModAsset.LanternMoonMusic_15_Percussion_Loop_Mod, 3, true);
		}
		if(Wave15StartTimer < 360000)
		{
			Wave15StartTimer++;
		}
		if(CustomMusicCues.Count > 0)
		{
			LanternMoonInvasionEvent LanternMoon = ModContent.GetInstance<LanternMoonInvasionEvent>();
			if (NPC.CountNPCS(ModContent.NPCType<LanternGhostKing>()) < 1)
			{
				FadeMusic(ModAsset.LanternMoonMusic_15_Accompaniment_Loop_Mod, 120);
				FadeMusic(ModAsset.LanternMoonMusic_15_Melody_Loop_Mod, 120);
				FadeMusic(ModAsset.LanternMoonMusic_15_Percussion_Loop_Mod, 120);
				FadeMusic(ModAsset.LanternMoonMusic_15_Melody_Head_Mod, 120);
				FadeMusic(ModAsset.LanternMoonMusic_Pre15_15_Transition_Mod, 120);
			}
		}
		base.PostUpdateEverything();
	}

	public override void EndMusicEffect(string path)
	{
		if(Wave15StartTimer >= 360000)
		{
			if (path == ModAsset.LanternMoonMusic_Pre15_Accompaniment_Head_Mod)
			{
				PlayMusic(ModAsset.LanternMoonMusic_Pre15_Accompaniment_Loop_Mod, true);
			}
			if (path == ModAsset.LanternMoonMusic_Pre15_Percussion_Head_Mod)
			{
				PlayMusic(ModAsset.LanternMoonMusic_Pre15_Percussion_Loop_Mod, true);
			}
		}
		else
		{

		}
	}
}