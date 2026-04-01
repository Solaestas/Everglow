using Everglow.Commons.Netcode.Abstracts;

namespace Everglow.Commons.Mechanics.Mission.WorldMission.Packets;

/// <summary>
/// Used to sync 3 types of state change for a mission from server to clients:
/// <list type="number">
/// <item>Locked -> Active (Unlock)</item>
/// <item>Active -> Completed (Complete)</item>
/// <item>Active -> Failed (Fail)</item>
/// </list>
/// </summary>
public class MissionStateSyncPacket : IPacket
{
	public void Receive(BinaryReader reader, int whoAmI) => throw new NotImplementedException();

	public void Send(BinaryWriter writer) => throw new NotImplementedException();

	[HandlePacket(typeof(MissionStateSyncPacket))]
	public class MissionStateChangePacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			// All logic already executed in Receive()
			// Keep empty to satisfy the interface
		}
	}
}

/// <summary>
/// Used to sync 2 types of request for a mission from server to clients:
/// <list type="number">
/// <item>Retry</item>
/// <item>Reward</item>
/// </list>
/// </summary>
public class MissionRequestPacket : IPacket
{
	public void Receive(BinaryReader reader, int whoAmI) => throw new NotImplementedException();

	public void Send(BinaryWriter writer) => throw new NotImplementedException();

	[HandlePacket(typeof(MissionRequestPacket))]
	public class MissionRequestPacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			// All logic already executed in Receive()
			// Keep empty to satisfy the interface
		}
	}
}

/// <summary>
/// Used to sync objective completion for a mission from server to clients.
/// </summary>
public class MissionObjectiveCompletePacket : IPacket
{
	public void Receive(BinaryReader reader, int whoAmI) => throw new NotImplementedException();

	public void Send(BinaryWriter writer) => throw new NotImplementedException();

	[HandlePacket(typeof(MissionObjectiveCompletePacket))]
	public class MissionObjectiveCompletePacketHandler : IPacketHandler
	{
		public void Handle(IPacket packet, int whoAmI)
		{
			// All logic already executed in Receive()
			// Keep empty to satisfy the interface
		}
	}
}