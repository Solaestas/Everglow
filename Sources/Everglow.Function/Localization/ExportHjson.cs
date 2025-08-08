using System.Text;
using Hjson;
using Microsoft.CodeAnalysis;
using Terraria.ModLoader.Core;
using JsonObject = Hjson.JsonObject;

namespace Everglow.Commons.Localization;

public class ExportHjson
{
	private const string ModPrefix = "Mods.Everglow.";
	private const string NamePrefix = "Everglow/";

	public static string SavePath => Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

	public static void ExportCategoryFiles(Dictionary<string, List<string>> keyToName, string cultureKey)
	{
		UpdateLocalizationFiles(keyToName, ModCompile.ModSourcePath + $"\\Everglow\\Sources\\Everglow\\Localization\\{cultureKey}");
	}

	private static void UpdateLocalizationFiles(
		 Dictionary<string, List<string>> categoryItems,
		 string localizationPath)
	{
		foreach (var category in categoryItems)
		{
			string fileName = $"{ModPrefix}{category.Key}.hjson";
			string filePath = Path.Combine(localizationPath, fileName);

			// 1. 读取或创建HJSON数据
			JsonObject existingData = LoadOrCreateHjson(filePath);

			// 2. 添加新物品（仅当键不存在时）
			int addedCount = 0;
			foreach (var itemFullName in category.Value)
			{
				string key = ProcessObjectName(itemFullName);

				if (!existingData.ContainsKey(key))
				{
					JsonValue defaultEntry;
					if (category.Key.Contains("Items"))
					{
						defaultEntry = new JsonObject
						{
							{ "DisplayName", SplitCamelCase(key) },
							{ "Tooltip", string.Empty },
						};
					}
					else if (category.Key.Contains("Buffs")
						|| category.Key.Contains("Cooldowns"))
					{
						defaultEntry = new JsonObject
						{
							{ "DisplayName", SplitCamelCase(key) },
							{ "Description", string.Empty },
						};
					}
					else if (category.Key.Contains("Biomes"))
					{
						defaultEntry = new JsonObject
						{
							{ "DisplayName", SplitCamelCase(key) },
							{ "TownNPCDialogueName", string.Empty },
						};
					}
					else
					{
						defaultEntry = new JsonObject
						{
							{ "DisplayName", SplitCamelCase(key) },
						};
					}

					existingData.Add(key, defaultEntry);
					addedCount++;
				}
			}

			// 3. 如果有新增才保存
			if (addedCount > 0)
			{
				SaveHjsonFile(existingData, filePath);
				Console.WriteLine($"更新 {fileName}：新增 {addedCount} 个物品");
			}
		}
	}

	private static JsonObject LoadOrCreateHjson(string filePath)
	{
		if (!File.Exists(filePath))
		{
			return new JsonObject();
		}

		try
		{
			return HjsonValue.Load(filePath).Qo() as JsonObject ?? new JsonObject();
		}
		catch
		{
			Console.WriteLine($"警告：{filePath} 解析失败，将创建新文件");
			return new JsonObject();
		}
	}

	private static string SplitCamelCase(string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return input;
		}

		var result = new StringBuilder();
		result.Append(input[0]);

		for (int i = 1; i < input.Length; i++)
		{
			if (char.IsUpper(input[i]) &&
				!char.IsUpper(input[i - 1]) &&
				(i == input.Length - 1 || !char.IsUpper(input[i + 1])))
			{
				result.Append(' ');
			}
			result.Append(input[i]);
		}

		return result.ToString();
	}

	private static string ProcessObjectName(string fullName)
	{
		// 移除前缀并处理特殊字符
		string name = fullName.StartsWith(NamePrefix)
			? fullName.Substring(NamePrefix.Length)
			: fullName;

		return name.Replace("/", "_"); // 处理路径分隔符
	}

	private static void SaveHjsonFile(JsonObject data, string filePath)
	{
		try
		{
			Directory.CreateDirectory(Path.GetDirectoryName(filePath));
			var output = data.ToFancyHjsonString();
			output = PostProcess(output);
			File.WriteAllText(filePath, output);
		}
		catch (Exception ex)
		{
			Console.WriteLine($"保存失败: {filePath}\n错误: {ex.Message}");
		}
	}

	private static string PostProcess(string originalHjson)
	{
		// 1. 分割为行并移除首尾空行
		var lines = originalHjson.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
			.SkipWhile(string.IsNullOrWhiteSpace)
			.Reverse()
			.SkipWhile(string.IsNullOrWhiteSpace)
			.Reverse()
			.ToList();

		// 2. 移除最外层花括号行（仅当它们是首行和末行时）
		if (lines.Count > 0 && lines.First().Trim() == "{")
		{
			lines.RemoveAt(0);
		}
		if (lines.Count > 0 && lines.Last().Trim() == "}")
		{
			lines.RemoveAt(lines.Count - 1);
		}

		// 3. 统一移除每行的第一个tab（如果存在）
		var formattedLines = lines.Select(line =>
		{
			var trimmed = line.TrimStart();
			return line.Length - trimmed.Length > 0 ? line.Substring(1) : line;
		});

		// 4. 重新组合并确保格式正确
		var result = new StringBuilder();
		foreach (var line in formattedLines)
		{
			// 处理对象闭合花括号的缩进
			if (line.Trim() == "}")
			{
				var tabCount = line.TakeWhile(c => c == '\t').Count();
				result.AppendLine(new string('\t', tabCount) + "}");
			}
			else
			{
				result.AppendLine(line);
			}
		}

		return result.ToString().TrimEnd();
	}
}