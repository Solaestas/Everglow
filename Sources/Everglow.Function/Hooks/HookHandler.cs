using Everglow.Commons.Enums;
using Everglow.Commons.Interfaces;

namespace Everglow.Commons.Hooks;

public record HookHandler(CodeLayer Layer, Delegate Hook, string Name) : IHookHandler
{
	public bool Enable { get; set; } = true;
}
