namespace Everglow.Myth.LanternMoon.LanternCommon;

public class LanternMoonSceneEffect : ModSceneEffect
{
	public int MusicTimer = 0;

	public override int Music => ModContent.GetInstance<LanternMoonInvasionEvent>().SwitchMusic();

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