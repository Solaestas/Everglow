namespace Everglow.Commons.Mechanics.Cooldown;

public class CooldownPlayer : ModPlayer
{
	public Dictionary<string, CooldownInstance> cooldowns = [];

	public override void OnEnterWorld()
	{
		// Player.AddCooldown(TestCooldown.ID, 60 * 50000);
		// Player.AddCooldown(TestCooldown2.ID, 60 * 5000);
		// Player.AddCooldown(TestCooldown3.ID, 60 * 500);
		// Player.AddCooldown(TestCooldown4.ID, 60 * 50);
	}

	public override void PostUpdateMiscEffects()
	{
		var expiredCooldowns = new List<string>();
		foreach (var (key, cdInstance) in cooldowns)
		{
			var cdBase = cdInstance.cooldown;
			if (cdBase.CanTickDown)
			{
				cdInstance.timeLeft--;
			}

			cdBase.Update();
			if (cdInstance.timeLeft <= 0)
			{
				cdBase.OnCompleted();
				expiredCooldowns.Add(key);
			}
		}

		foreach (var key in expiredCooldowns)
		{
			cooldowns.Remove(key);
		}

		// TODO: Sync cooldown removal here
	}

	internal void SyncCooldown(bool server, CooldownInstance cd)
	{
		if (Main.netMode == NetmodeID.SinglePlayer)
		{
			return;
		}

		// TODO: Implement server sync logic
	}
}