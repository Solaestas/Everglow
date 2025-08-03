namespace Everglow.Commons.Mechanics;

public class EverglowGlobalItem : GlobalItem
{
	public override bool CanConsumeAmmo(Item weapon, Item ammo, Player player) => Main.rand.NextFloat() < player.GetModPlayer<EverglowPlayer>().ammoCost;
}