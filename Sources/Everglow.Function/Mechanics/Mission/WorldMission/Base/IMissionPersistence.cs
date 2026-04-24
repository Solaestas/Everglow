using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Base;

public interface IMissionPersistence
{
	public void SaveData(TagCompound tag);

	public void LoadData(TagCompound tag);
}