using System.Text;

namespace Everglow.Scripts;

public abstract class Script
{
	public virtual string Name => GetType().Name;

	public abstract string Description { get; }

	public abstract void Run(string[] args);
}

public class AddModule : Script
{
	public override string Description => "Add a module to enable it";

	public override void Run(string[] args)
	{
		if (args.Length == 0)
		{
			Console.WriteLine("Please write module name");
		}
		var modules = Module.GetEnabledModule();
		modules = modules.Concat(args).ToHashSet().Intersect(Module.Modules).ToArray();
		Module.SetEnabledModule(modules);
	}
}

public class RemoveModule : Script
{
	public override string Description => "Remove a module to disable it";

	public override void Run(string[] args)
	{
		if (args.Length == 0)
		{
			Console.WriteLine("Please write module name");
		}
		var modules = Module.GetEnabledModule();
		modules = modules.Where(s => !args.Contains(s)).ToArray();
		Module.SetEnabledModule(modules);
	}
}

public class ListModule : Script
{
	public override string Description => "List all modules";

	public override void Run(string[] args)
	{
		var modules = Module.Modules;
		var enabled = Module.GetEnabledModule();
		int maxName = modules.Max(s => s.Length);
		StringBuilder sb = new StringBuilder();

		sb.Append('┌').Append('─', maxName + 2).Append('┬').Append('─', 5).Append('┐');
		Console.WriteLine(sb);
		sb.Clear();

		foreach (var module in modules)
		{
			bool enable = enabled.Contains(module);
			var color = $"\u001b[{(enable ? 92 : 37)}m";
			var white = $"\u001b[37m";
			sb.Append('│').Append(' ').Append(color).Append(module.PadRight(maxName)).Append(white).Append(' ').Append('│')
				.Append(' ').Append((enable ? "On " : "Off")).Append(' ').Append('│');
			Console.WriteLine(sb);
			sb.Clear();
		}

		sb.Append('└').Append('─', maxName + 2).Append('┴').Append('─', 5).Append('┘');
		Console.WriteLine(sb);
	}
}

public class ClearModule : Script
{
	public override string Description => "Disable all module";

	public override void Run(string[] args)
	{
		Module.SetEnabledModule([]);
		Console.WriteLine("All modules are disabled");
	}
}


public class CreateModule : Script
{
	public override string Description => "Create a new module";

	public override void Run(string[] args)
	{
		var name = args.FirstOrDefault();
		if(name == null)
		{
			Console.WriteLine("Must provide module name");
			return;
		}

		var dir = new DirectoryInfo(Path.Combine(Module.Everglow, "Sources", name));
		dir.Create();
		File.WriteAllText(Path.Combine(dir.FullName, $"Everglow.{name}.csproj"), """
			<Project Sdk="Microsoft.NET.Sdk">
			</Project>
			""");
	}
}
public class ResetModule : Script
{
	public override string Description => "Enable all module";

	public override void Run(string[] args)
	{
		Module.SetEnabledModule(Module.Modules);
		Console.Write("All modules are enabled");
	}
}

public class Help : Script
{
	public override string Description => "Show help";

	public override void Run(string[] args)
	{
		var scripts = ScriptCompleter.Scripts;

		int maxNameLength = scripts.Max(s => s.Name.Length);
		int maxDescriptionLength = scripts.Max(s => s.Description.Length);

		StringBuilder sb = new StringBuilder();
		sb.Append('┌').Append('─', maxNameLength + 2).Append('┬').Append('─', maxDescriptionLength + 2).Append('┐');
		Console.WriteLine(sb);
		sb.Clear();
		sb.Append("│ ")
			.Append("Script Name".PadRight(maxNameLength + 1))
			.Append("│ ")
			.Append("Description".PadRight(maxDescriptionLength + 1))
			.Append('│');
		Console.WriteLine(sb);
		sb.Clear();
		foreach (var script in scripts)
		{
			sb.Append('├').Append('─', maxNameLength + 2).Append('┼').Append('─', maxDescriptionLength + 2).Append('┤');
			Console.WriteLine(sb);
			sb.Clear();
			sb.Append($"│ \u001b[93m")
				.Append(script.Name.PadRight(maxNameLength + 1))
				.Append("\u001b[37m│ ")
				.Append(script.Description.PadRight(maxDescriptionLength + 1))
				.Append('│');
			Console.WriteLine(sb);
			sb.Clear();
		}
		sb.Append('└').Append('─', maxNameLength + 2).Append('┴').Append('─', maxDescriptionLength + 2).Append('┘');
		Console.WriteLine(sb);
	}
}

public class Exit : Script
{
	public override string Description => "Exit the script";

	public override void Run(string[] args)
	{
	}
}