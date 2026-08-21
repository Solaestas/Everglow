namespace Everglow.Commons.Mechanics.Quest.WorldSide.Abstractions;

public interface IQuestNetcode
{
	public void NetSend(BinaryWriter writer);

	public void NetReceive(BinaryReader reader);

	public void OnMPSync();
}
