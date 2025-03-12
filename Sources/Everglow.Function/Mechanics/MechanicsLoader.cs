using Everglow.Commons.Mechanics.MissionSystem.Core;

namespace Everglow.Commons.Mechanics;

public class MechanicsLoader
{
	public static void Load(Mod mod)
	{
		if (!Main.dedServ)
		{
			MissionManager.Load();
		}
	}

	public static void Unload()
	{
		if (!Main.dedServ)
		{
			MissionManager.UnLoad();
		}
	}
}