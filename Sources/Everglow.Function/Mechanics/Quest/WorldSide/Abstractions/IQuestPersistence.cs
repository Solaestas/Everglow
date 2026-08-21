using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Quest.WorldSide.Abstractions;

public interface IQuestPersistence
{
	public void SaveData(TagCompound tag);

	public void LoadData(TagCompound tag);
}
