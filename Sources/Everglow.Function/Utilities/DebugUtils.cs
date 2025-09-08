namespace Everglow.Commons.Utilities;

[Obsolete("This class belongs to @Solaestas, and is not used currently, so it's marked as obsoleted.")]
internal enum HookFlag
{
	WallSlideException,
}

[Obsolete("This class belongs to @Solaestas, and is not used currently, so it's marked as obsoleted.")]
internal static class DebugUtils
{
	[Conditional("DEBUG")]
	public static void DebugInvoke(Action action) => action();

	private static Dictionary<HookFlag, int> flags = new();

	[Conditional("DEBUG")]
	public static void TriggerFlag(HookFlag flag)
	{
		if (flags.ContainsKey(flag))
		{
			flags[flag]++;
			return;
		}
		flags.Add(flag, 0);
	}

	[Conditional("DEBUG")]
	public static void PrintFlags()
	{
		Console.WriteLine($"未触发Flag ： {string.Join(string.Empty, from flag in Enum.GetValues<HookFlag>() where !flags.ContainsKey(flag) select flag.ToString() + " ")}");
		Console.WriteLine($"已触发Flag ： {string.Join(string.Empty, from flag in Enum.GetValues<HookFlag>() where flags.ContainsKey(flag) select $"{flag} : {flags[flag]}次 ")}");
	}

	public static void ProfilerInvoke(Action action) => action();
}