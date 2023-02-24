using System.Reflection;

namespace Everglow.Common.Modules;

public class ModuleManager
{
	private List<IModule> _modules = new();

	public IEnumerable<Type> Types => Assembly.GetExecutingAssembly().GetTypes()
		.Where(t => t.GetCustomAttribute(typeof(ModuleHideTypeAttribute)) is null);

	public IEnumerable<T> CreateInstances<T>() => Types
		.Where(t => t.IsAssignableTo(typeof(T)))
		.Where(t => !t.IsAbstract && !t.IsInterface)
		.Select(Activator.CreateInstance)
		.Cast<T>();

	public void LoadModule()
	{
		//var mainName = main.GetName().Name;
		//var core = Assembly.GetExecutingAssembly();
		//var codes = AppDomain.CurrentDomain.GetAssemblies()
		//	.Where(asm => asm != core)
		//	.Where(asm => asm.GetName().Name.StartsWith($"{mainName}.", StringComparison.Ordinal))
		//	.ToArray();
		//foreach (var code in codes)
		//{
		//	var module = code.GetTypes().Single(type => type.IsAssignableTo(typeof(IModule)));
		//	_modules.Add(Activator.CreateInstance(module) as IModule);
		//}
	}
}