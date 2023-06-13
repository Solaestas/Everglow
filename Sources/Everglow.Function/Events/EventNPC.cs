namespace Everglow.Commons.Events
{
	internal class EventNPC : GlobalNPC
	{
		public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
		{
			foreach (ModEvent e in EventManager.activeEvents)
			{
				if (!e.EditSpawnPool(pool, spawnInfo))
				{
					break;
				}
			}
		}
		public override void EditSpawnRange(Player player, ref int spawnRangeX, ref int spawnRangeY, ref int safeRangeX, ref int safeRangeY)
		{
			foreach (ModEvent e in EventManager.activeEvents)
			{
				if (!e.EditSpawnRange(player, ref spawnRangeX, ref spawnRangeY, ref safeRangeX, ref safeRangeY))
				{
					break;
				}
			}
		}
		public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
		{
			foreach (ModEvent e in EventManager.activeEvents)
			{
				if (!e.EditSpawnRate(player, ref spawnRate, ref maxSpawns))
				{
					break;
				}
			}
		}
	}
}
