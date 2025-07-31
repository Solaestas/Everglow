namespace Everglow.Commons.Mechanics.Cooldown;

public static class CooldownExtensions
{
	public static void AddCooldown(this Player player, string id, int timeToAdd, bool overwrite = true)
	{
		if (!player.HasCooldown(id) || overwrite)
		{
			var modP = player.GetModPlayer<CooldownPlayer>();
			var instance = new CooldownInstance(player, CooldownRegistry.GetNet(id), timeToAdd);
			modP.cooldowns[id] = instance;
			modP.SyncCooldownAddition(Main.dedServ, instance);
		}
	}

	public static bool HasCooldown(this Player player, Predicate<CooldownBase> predicate) =>
		player.GetModPlayer<CooldownPlayer>().cooldowns.Any(cd => predicate(cd.Value.cooldown));

	public static bool HasCooldown(this Player player, string id) =>
		HasCooldown(player, cd => cd.TypeID == id);

	public static bool HasCooldown<TCooldownBase>(this Player player) =>
		HasCooldown(player, cd => cd is TCooldownBase);

	public static void ClearCooldown(this Player player, string id)
	{
		var mp = player.GetModPlayer<CooldownPlayer>();
		if (mp.cooldowns.Remove(id))
		{
			mp.SyncCooldownRemoval(Main.dedServ, [id]);
		}
	}
}