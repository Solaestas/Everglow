using System.Reflection;

namespace Everglow.Commons.Modules;

public class ModuleManager : IDisposable
{
	private List<Assembly> _assemblies;

	private List<IModule> _modules;

	public ModuleManager()
	{
		var assemblies = AppDomain.CurrentDomain.GetAssemblies();

		var main = assemblies.Last(asm => asm.GetName().Name == "Everglow");

		// 重新加载时以前的程序集还在，取Last
		_assemblies = assemblies
			.Where(asm => asm.GetName().Name.StartsWith($"Everglow.", StringComparison.Ordinal))
			.Where(asm => asm.FullName != main.FullName)
			.GroupBy(asm => asm.FullName)
			.Select(asms => asms.Last())
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

	public IEnumerable<T> CreateInstances<T>() => CreateInstances<T>(type => true);

	public IEnumerable<T> CreateInstances<T>(Func<Type, bool> condition) => Types
		.Where(t => t.IsAssignableTo(typeof(T)))
		.Where(t => !t.IsAbstract && !t.IsInterface)
		.Where(condition)
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