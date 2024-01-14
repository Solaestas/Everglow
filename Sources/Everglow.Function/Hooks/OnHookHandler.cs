using Everglow.Commons.Interfaces;
using MonoMod.RuntimeDetour;

namespace Everglow.Commons.Hooks;

public class OnHookHandler : IHookHandler
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

	public Hook Hook { get; init; }

	public string Name => Hook.Target.Name;
}
