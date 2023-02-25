using System.Reflection;

namespace Everglow.Commons.Modules;

public class ModuleManager : IDisposable
{
	private List<Assembly> _assemblies;
	private List<IModule> _modules;

	public ModuleManager()
	{
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();
		// 为什么重新加载时之前的程序集还在
		var main = assemblies.Last(asm => asm.GetName().Name == "Everglow");

		_assemblies = assemblies
			.Where(asm => asm.GetName().Name.StartsWith($"Everglow.", StringComparison.Ordinal))
			.Where(asm => asm != main)
			.ToList();

		_modules = _assemblies
			.SelectMany(asm => asm.GetTypes())
			.Where(type => !type.IsAbstract)
			.Where(type => !type.IsInterface)
			.Where(type => type.IsAssignableTo(typeof(IModule)))
			.Select(type => Activator.CreateInstance(type) as IModule)
			.Where(module => module.Condition)
			.ToList();
		foreach (var module in _modules)
		{
			module.Load();
		}
	}

	public IEnumerable<Type> Types => _assemblies.SelectMany(s => s.GetTypes())
		.Where(t => t.GetCustomAttribute(typeof(ModuleHideTypeAttribute)) is null);

	public IEnumerable<T> CreateInstances<T>() => Types
		.Where(t => t.IsAssignableTo(typeof(T)))
		.Where(t => !t.IsAbstract && !t.IsInterface)
		.Select(Activator.CreateInstance)
		.Cast<T>();

	public void Dispose()
	{
		foreach (var module in _modules)
		{
			module.Unload();
		}
		GC.SuppressFinalize(this);
	}

}