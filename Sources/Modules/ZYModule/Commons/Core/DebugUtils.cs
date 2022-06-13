using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Commons.Core.Profiler.Fody;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Enumerator;

namespace Everglow.Sources.Modules.ZYModule.Commons.Core;

enum HookFlag
{
    WallSlideException
}
internal static class DebugUtils
{
    [Conditional("DEBUG")]
    public static void DebugInvoke(Action action) => action();

    private static Dictionary<HookFlag, int> flags = new Dictionary<HookFlag, int>();
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
        Console.WriteLine($"未触发Flag ： {(from flag in EnumUtils.GetEnums<HookFlag>() where !flags.ContainsKey(flag) select flag.ToString() + " ").BuildString()}");
        Console.WriteLine($"已触发Flag ： {(from flag in EnumUtils.GetEnums<HookFlag>() where flags.ContainsKey(flag) select $"{flag} : {flags[flag]}次 ").BuildString()}");
    }
    [Conditional("DEBUG"), ProfilerMeasure]
    public static void ProfilerInvoke(Action action) => action();


}
