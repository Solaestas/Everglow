using System.Xml;

namespace Everglow.Scripts;

public static class Module
{
	static Module()
	{
		Everglow = FindEverglow();
	}

	private static string FindEverglow()
	{
		var target = "Everglow.sln";
		var dir = Environment.CurrentDirectory;
		while (true)
		{
			foreach (var file in Directory.EnumerateFiles(dir))
			{
				if (Path.GetFileName(file) == target)
				{
					return dir;
				}
			}
			dir = Path.GetFullPath(Path.Combine(dir, ".."));
			if (dir == Path.GetPathRoot(dir))
			{
				throw new Exception("Unable to find Everglow.sln");
			}
		}
	}

	public static string[] GetEnabledModule()
	{
		var path = Path.Combine(Everglow, "Sources", "Directory.Build.props");

		// Read XML
		var doc = new XmlDocument();
		doc.Load(path);

		// Find Property Modules
		var modules = doc.SelectSingleNode("/Project/PropertyGroup/Modules")!.InnerText;

		// Split
		return modules.Split(';');
	}

	public static void SetEnabledModule(string[] modules)
	{
		var path = Path.Combine(Everglow, "Sources", "Directory.Build.props");

		// Read XML
		var doc = new XmlDocument();
		doc.Load(path);

		// Find Property Modules
		doc.SelectSingleNode("/Project/PropertyGroup/Modules")!.InnerText = string.Join(";", modules);

		// Save
		doc.Save(path);
	}

	public static string Everglow { get; set; }

	public static string[] Modules => Directory.EnumerateDirectories(Path.Combine(Everglow, "Sources", "Modules")).Select(s => Path.GetFileName(s)).ToArray();
}