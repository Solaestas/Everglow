using Terraria.ModLoader.IO;

namespace Everglow.Commons.MissionSystem.Abstracts;

public interface ITagCompoundEntity
{
	// Summary:
	//     Allows you to save custom data for this player.
	//
	//     NOTE: The provided tag is always empty by default, and is provided as an argument
	//     only for the sake of convenience and optimization.
	//     NOTE: Try to only save data that isn't default values.
	public void SaveData(TagCompound tag);

	// Summary:
	//     Allows you to load custom data that you have saved for this player.
	//     Try to write defensive loading code that won't crash if something's missing.
	public void LoadData(TagCompound tag);
}