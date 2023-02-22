using System.Reflection;
using Everglow.Common.Enums;
using MonoMod.Cil;

namespace Everglow.Common.Interfaces;

public interface IHookManager : IDisposable
{
	public IHookHandler AddHook(CodeLayer layer, Delegate hook, string name = default);
	public IHookHandler AddHook(MethodInfo target, Delegate hook);
	public IHookHandler AddHook(MethodInfo target, ILContext.Manipulator hook);
	public void RemoveHook(IHookHandler handler);
}
