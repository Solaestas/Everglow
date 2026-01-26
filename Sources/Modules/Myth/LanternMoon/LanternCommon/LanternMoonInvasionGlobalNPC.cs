

using Spine;

namespace Everglow.Myth.LanternMoon.LanternCommon;

public class LanternMoonInvasionGlobalNPC : GlobalNPC
{
	public LanternMoonInvasionEvent LanternMoon = ModContent.GetInstance<LanternMoonInvasionEvent>();

	public override bool InstancePerEntity => true;

	public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
	{
		if (LanternMoon is not null && LanternMoon.Active)
		{
			foreach (int key in pool.Keys.ToList())
			{
				pool[key] = 0f;
			}
			if (LanternMoon.Active && LanternMoon.Wave > 0 && LanternMoon.WaveEnemiesType[LanternMoon.Wave - 1] is not null)
			{
				if(LanternMoon.Wave != 15)
				{
					foreach (var type in LanternMoon.WaveEnemiesType[LanternMoon.Wave - 1])
					{
						pool.Add(type, 1);
					}
				}
			}
		}
		base.EditSpawnPool(pool, spawnInfo);
	}

	public override void EditSpawnRate(Player player, ref int spawnRate, ref int maxSpawns)
	{
		if (LanternMoon is not null && LanternMoon.Active)
		{
			if(spawnRate > 0)
			{
				spawnRate /= 15;
				maxSpawns = 60;
			}
		}
		base.EditSpawnRate(player, ref spawnRate, ref maxSpawns);
	}
}