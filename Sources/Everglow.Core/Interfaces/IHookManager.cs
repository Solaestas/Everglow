using System.Reflection;
using Everglow.Commons.Enums;
using MonoMod.Cil;

namespace Everglow.Commons.Interfaces;

public interface IHookManager : IDisposable
{
	public IHookHandler AddHook(CodeLayer layer, Delegate hook, string name = default);
	public IHookHandler AddHook(MethodInfo target, Delegate hook);
	public IHookHandler AddHook(MethodInfo target, ILContext.Manipulator hook);
	public void RemoveHook(IHookHandler handler);
	public void Disable(TerrariaFunction function);
	public void Enable(TerrariaFunction function);
}
