using MethodBoundaryAspect.Fody.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Profiler.Fody
{
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
