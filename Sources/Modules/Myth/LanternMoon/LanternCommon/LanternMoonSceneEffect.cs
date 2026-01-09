using Everglow.Commons.Mechanics.Events;

namespace Everglow.Myth.LanternMoon.LanternCommon;

public class LanternMoonSceneEffect : ModSceneEffect
{
	public override int Music => MusicLoader.GetMusicSlot(ModAsset.LanternMoonMusic_Mod);

	public override SceneEffectPriority Priority => SceneEffectPriority.Event;

	public override bool IsSceneEffectActive(Player player)
	{
		if (ModContent.GetInstance<LanternMoonInvasionEvent>().Active)
		{
			return true;
		}
		return false;
	}
}