namespace Everglow.Yggdrasil;

public class YggdrasilGlobalNPC : GlobalNPC
{
	public override bool InstancePerEntity => true;

	/// <summary>
	/// Woodland Wraith's spore zone.
	/// </summary>
	public bool InSporeZone { get; set; } = false;

	public override void ResetEffects(NPC npc)
	{
		InSporeZone = false;
	}
}