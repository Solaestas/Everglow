using Everglow.Sources.Commons.Core;
using Everglow.Sources.Commons.Function.NetUtils;

namespace Everglow.Sources.Commons.Function.PlayerUtils;

internal class MouseTrail : INetUpdate<Vector2>
{
    public const int Capacity = 30;
    /// <summary>
    /// 确保第一次加载时链表不为空
    /// </summary>
    private int updateTime = -1;
    public LinkedList<Vector2> position = new LinkedList<Vector2>();
    public Vector2 Current => position.Last?.Value ?? Vector2.Zero;
    public static implicit operator Vector2(MouseTrail mouse) => mouse.Current;
    public void Forcast()
    {
        if (updateTime != HookSystem.UITimer)
        {
            if (position.Count < 2)
            {
                return;
            }
            var last = position.Last;
            var lastlast = last.Previous;
            if (position.Count == Capacity)
            {
                position.RemoveFirst();
            }
            position.AddLast(2 * last.Value - lastlast.Value);
            updateTime = HookSystem.UITimer;
        }
    }

    public void LocalUpdate(Vector2 input)
    {
        if (position.Count == Capacity)
        {
            position.RemoveFirst();
        }
        position.AddLast(input);
    }

    public void NetUpdate(Vector2 input)
    {
        if (updateTime == HookSystem.UITimer)
        {
            position.Last.ValueRef = input;
        }
        else
        {
            if (position.Count == Capacity)
            {
                position.RemoveFirst();
            }
            position.AddLast(input);
            updateTime = HookSystem.UITimer;
        }
    }
}
