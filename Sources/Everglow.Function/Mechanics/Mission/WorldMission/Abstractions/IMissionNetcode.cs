namespace Everglow.Commons.Mechanics.Mission.WorldMission.Abstractions;

public interface IMissionNetcode
{
	public void NetSend(BinaryWriter writer);

	public void NetReceive(BinaryReader reader);

	public void OnMPSync();
}