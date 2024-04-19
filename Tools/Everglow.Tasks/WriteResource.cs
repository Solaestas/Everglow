using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;

namespace Everglow.Tasks;

public class WriteResource : Task
{
	[Required]
	public ITaskItem[] Resources { get; set; } = default!;

	[Required]
	public string OutputPath { get; set; } = string.Empty;

	[Required]
	public string Prefix { get; set; } = string.Empty;

	public override bool Execute()
	{
		List<Entry> list = [];
		foreach(var item in Resources)
		{
			if(bool.TryParse(item.GetMetadata("Pack"), out var pack) && pack)
			{
				string path = Path.Combine(Prefix, item.GetMetadata("ModPath"));
				if(Prefix == "Commons")
				{
					var location = Path.Combine("..", "Everglow.Function", item.ItemSpec);
					list.Add((location, $"{Prefix}\\{path}"));
				}
				else
				{
					var location = Path.Combine("..", "Modules", Prefix, item.ItemSpec);
					list.Add((location, $"{Prefix}\\{path}"));
				}
			}
		}
		File.WriteAllText(OutputPath, JsonConvert.SerializeObject(list));
		return true;
	}
}
