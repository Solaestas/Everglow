using Everglow.Common.NetUtils;

namespace Everglow.Common.PlayerUtils;

internal class VirtualKey : INetUpdate<bool>
{
	internal const int DoubleClickMaxInterval = 10;
	/// <summary>
	/// 确保第一次加载时链表不为空
	/// </summary>
	private int updateTime = -1;
	private ulong cache;
	public ulong Cache => cache;
	public bool Press => (cache & 1) == 1;
	public bool Release => (cache & 1) == 0;
	public bool JustPress => (cache & 2) == 2;
	public bool JustRelease => (cache & 2) == 0;
	public bool Click => Release && JustPress;
	public bool DoubleClick => Click && (cache & (1uL << DoubleClickMaxInterval) - 4uL) != 0;
	public void LocalUpdate(bool input)
	{
		cache <<= 1;
		cache |= input ? 1ul : 0;
	}
	public void NetUpdate(bool input)
	{
		if (updateTime == HookSystem.UITimer)
		{
			cache = cache & ~1ul | (input ? 1ul : 0);
		}
		else
		{
			cache <<= 1;
			cache |= input ? 1ul : 0;
			updateTime = HookSystem.UITimer;
		}
	}
	public void Forcast()
	{
		if (updateTime != HookSystem.UITimer)
		{
			cache = cache << 1 | cache & 1;
			updateTime = HookSystem.UITimer;
		}
	}
	public bool PreviousPress(int tickCount)
	{
		return (cache & (1uL << tickCount) - 1uL) != 0;
	}
	public static implicit operator bool(VirtualKey key) => key.Press;
}
