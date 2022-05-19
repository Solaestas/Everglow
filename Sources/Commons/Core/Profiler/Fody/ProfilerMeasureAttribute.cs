using MethodBoundaryAspect.Fody.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Profiler.Fody
{
    /// <summary>
    /// 用于将一个函数标记为可以被分析，在Profiler模式下该函数的调用信息会被记录
    /// </summary>
    internal class ProfilerMeasureAttribute : OnMethodBoundaryAspect
    {
        private Stopwatch _stopwatch;
        public ProfilerMeasureAttribute()
        {
            _stopwatch = new Stopwatch();
        }
        public override void OnEntry(MethodExecutionArgs args)
        {
            _stopwatch.Restart();
        }

        public override void OnExit(MethodExecutionArgs args)
        {
            _stopwatch.Stop();
            var fullName = $"{args.Method.DeclaringType.FullName}:{args.Method.Name}";
            Everglow.ProfilerManager.AddEntry(fullName, _stopwatch.Elapsed.TotalMilliseconds);
        }

        public override void OnException(MethodExecutionArgs args)
        {
            _stopwatch.Stop();
            var fullName = $"{args.Method.DeclaringType.FullName}:{args.Method.Name}";
            Everglow.ProfilerManager.AddEntry(fullName, _stopwatch.Elapsed.TotalMilliseconds);
        }
    }

}
