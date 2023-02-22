using System.Reflection;

namespace Everglow.Common.ModuleSystem;

public class ModuleManager
{
	private Assembly[] _modules;

	public IEnumerable<Type> Types => _modules.SelectMany(m => m.GetTypes());

	public IEnumerable<T> CreateInstances<T>() => Types
		.Where(t => t.IsAssignableTo(typeof(T)))
		.Where(t => !t.IsAbstract && !t.IsInterface)
		.Select(Activator.CreateInstance)
		.Cast<T>();
	public void LoadModule(Assembly main)
	{
		var mainName = main.GetName().Name;
		var core = Assembly.GetExecutingAssembly();
		_modules = AppDomain.CurrentDomain.GetAssemblies()
			.Where(asm => asm != core)
			.Where(asm => asm.GetName().Name.StartsWith($"{mainName}.", StringComparison.Ordinal))
			.ToArray();
		foreach (var module in _modules)
		{

		}
	}
}