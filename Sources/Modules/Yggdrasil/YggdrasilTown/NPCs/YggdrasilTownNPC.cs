namespace Everglow.Yggdrasil.YggdrasilTown.NPCs;

public class YggdrasilTownNPC : GlobalNPC
{
	private static HashSet<int> yggdrasilTownNPCTypes = new HashSet<int>();
	public static void RegisterYggdrasilTownNPC(int type) => yggdrasilTownNPCTypes.Add(type);
}
