using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.Mission.WorldMission;

public class WorldMissionSystem : ModSystem
{
	private const string MissionManagerKey = "MissionManagerData";

	public WorldMissionManager Manager;

	public override void Load()
	{
		Manager = new();
		Manager.Load();
	}

	public override void Unload()
	{
		Manager.Unload();
		Manager = null;
	}

	public override void SetStaticDefaults()
	{
		Manager.Initialize();
	}

	public override void NetSend(BinaryWriter writer)
	{
		Manager.NetSend(writer);
	}

	public override void NetReceive(BinaryReader reader)
	{
		Manager.NetReceive(reader);
	}

	public override void LoadWorldData(TagCompound tag)
	{
		if (tag.TryGet<TagCompound>(MissionManagerKey, out var data))
		{
			Manager.LoadData(data);
		}
	}

	public override void SaveWorldData(TagCompound tag)
	{
		var data = new TagCompound();
		Manager.SaveData(data);
		tag.Add(MissionManagerKey, data);
	}

	public override void OnWorldLoad()
	{
		// Load world data to mission manager here.
		// All necessary world data have been synced at this point.

		// TODO: In single player, ModSystem.LoadWorldData() is called after this hook.
		// Maybe we can skip loading data, and just sync it in Netcode?
	}

	public override void OnWorldUnload()
	{
		// Clean up mission manager data here.
	}
}