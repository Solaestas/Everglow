using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Newtonsoft.Json;

namespace Everglow.Tasks;
public class ReadResource : Task
{
	[Required]
	public ITaskItem[] Lists { get; set; } = default!;

	[Output]
	public TaskItem[] Resources { get; set; } = [];
	public override bool Execute()
	{
		List<TaskItem> result = [];
		foreach(var json in Lists)
		{
			string text = File.ReadAllText(json.ItemSpec);
			var list = JsonConvert.DeserializeObject<List<Entry>>(text) ?? [];
			foreach (var (ItemSpec, ModPath) in list)
			{
				var item = new TaskItem(ItemSpec);
				item.SetMetadata("ModPath", ModPath);
				item.SetMetadata("Pack", true.ToString());
				result.Add(item);
			}
		}
		Resources = [.. result];
		return true;
	}
}
