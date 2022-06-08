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

}
internal static class DebugUtils
{
    [Conditional("DEBUG")]
    public static void DebugInvoke(Action action) => action();

    private static HashSet<HookFlag> flags = new HashSet<HookFlag>();
    [Conditional("DEBUG")]
    public static void TriggerFlag(HookFlag flag)
    {
        if (flags.Contains(flag))
        {
            return;
        }
        flags.Add(flag);
    }

    [Conditional("DEBUG")]
    public static void PrintFlags()
    {
        Console.WriteLine($"未触发Flag ： {(from flag in EnumUtils.GetEnums<HookFlag>() where !flags.Contains(flag) select flag.ToString() + " ").BuildString()}");
        Console.WriteLine($"已触发Flag ： {(from flag in EnumUtils.GetEnums<HookFlag>() where flags.Contains(flag) select flag.ToString() + " ").BuildString()}");
    }
    [Conditional("DEBUG"), ProfilerMeasure]
    public static void ProfilerInvoke(Action action) => action();


}
