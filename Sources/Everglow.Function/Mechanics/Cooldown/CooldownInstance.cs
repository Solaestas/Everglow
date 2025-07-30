namespace Everglow.Commons.Mechanics.Cooldown;

public sealed class CooldownInstance
{
	public CooldownInstance(Player player, CooldownBase cd, int time)
	{
		this.player = player;
		this.cooldown = cd;
		this.timeMax = time;
		this.timeLeft = time;
	}

	internal ushort netId;

	public Player player;

	public int timeMax;

	public int timeLeft;

	public float Completion => timeMax != 0 ? timeLeft / (float)timeMax : 0;

	public CooldownBase cooldown;
}