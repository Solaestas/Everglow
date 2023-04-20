using System.Reflection;

namespace Everglow.Commons.Modules;

public interface IModule
{
	public Assembly Code { get; }

	public string Name { get; }

	public bool Condition { get; }

	public void Load();

	public void Unload();
}