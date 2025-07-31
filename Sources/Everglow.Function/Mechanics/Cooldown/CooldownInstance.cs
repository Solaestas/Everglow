namespace Everglow.Commons.Mechanics.Cooldown;

public sealed class CooldownInstance
{
	public CooldownInstance(Player player, string id, int timeMax, int timeLeft)
	{
		Type cdBaseT = CooldownRegistry.nameToType[id];
		var cooldown = Activator.CreateInstance(cdBaseT) as CooldownBase;
		cooldown.Instance = this;

		this.player = player;
		this.cooldown = cooldown;
		this.timeMax = timeMax;
		this.timeLeft = timeLeft;
	}

	public CooldownInstance(Player player, string id, int timeMax)
		: this(player, id, timeMax, timeMax)
	{
	}

	internal ushort netId;

	public Player player;

	public int timeMax;

	public int timeLeft;

	public float Completion => timeMax != 0 ? timeLeft / (float)timeMax : 0;

	public CooldownBase cooldown;
}