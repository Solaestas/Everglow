using System.Reflection;

namespace Everglow.Common.Modules;

public interface IModule
{
	public Assembly Code { get; }

	public string Name { get; }

	public bool Condition { get; }

	public void Load();

	public void Unload();
}