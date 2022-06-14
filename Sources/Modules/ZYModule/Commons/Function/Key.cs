namespace Everglow.Sources.Modules.ZYModule.Commons.Function;

internal class VirtualKey
{
    internal const int DoubleClickMaxInterval = 10;
    internal ulong cache;
    public ulong Cache => cache;
    public bool Press => (cache & 1) == 1;
    public bool Release => (cache & 1) == 0;
    public bool JustPress => (cache & 2) == 2;
    public bool JustRelease => (cache & 2) == 0;
    public bool Click => Release && JustPress;
    public bool DoubleClick => Click && (cache & ((1uL << DoubleClickMaxInterval) - 4uL)) != 0;
    public void Update(bool IsPress)
    {
        cache <<= 1;
        cache |= IsPress ? 1ul : 0;
    }
    public bool PreviousPress(int tickCount)
    {
        return (cache & ((1uL << tickCount) - 1uL)) != 0;
    }
    public static implicit operator bool(VirtualKey key) => key.Press;
}
