namespace Everglow.Commons.Mechanics.Cooldown;

public static class CooldownExtensions
{
	public static void AddCooldown(this Player player, string id, int timeToAdd, bool overwrite = true)
	{
		if (!player.HasCooldown(id) || overwrite)
		{
			var modP = player.GetModPlayer<CooldownPlayer>();

			Type cdBaseT = CooldownRegistry.nameToType[id];
			var cdBase = Activator.CreateInstance(cdBaseT) as CooldownBase;
			var instance = new CooldownInstance(player, cdBase, timeToAdd);
			cdBase.Instance = instance;

			modP.cooldowns[id] = instance;
			modP.SyncCooldown(Main.dedServ, instance);
		}
	}

	public static bool HasCooldown(this Player player, Predicate<CooldownBase> predicate) =>
		player.GetModPlayer<CooldownPlayer>().cooldowns.Any(cd => predicate(cd.Value.cooldown));

	public static bool HasCooldown(this Player player, string id) =>
		HasCooldown(player, cd => cd.TypeID == id);

	public static bool HasCooldown<TCooldownBase>(this Player player) =>
		HasCooldown(player, cd => cd is TCooldownBase);
}