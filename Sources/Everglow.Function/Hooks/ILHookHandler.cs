using Everglow.Commons.Interfaces;
using MonoMod.RuntimeDetour;

namespace Everglow.Commons.Hooks;

public class ILHookHandler : IHookHandler
{
	public bool Enable
	{
		get => Hook.IsApplied;
		set
		{
			if (value)
			{
				Hook.Apply();
			}
			else
			{
				Hook.Undo();
			}
		}
	}

	public ILHook Hook { get; init; }

	public string Name => Hook.Method.Name;
}
