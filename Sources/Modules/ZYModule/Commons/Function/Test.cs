using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function
{
    internal class Test
    {
        public static bool TestFlag { get; set; }
        public static StringBuilder Log { get; set; } = new StringBuilder();
        public static void TestLog(object obj)
        {
            if(TestFlag)
            {
                Log.AppendLine(obj.ToString());
            }
        }
        public static void TestInvoke(Action action)
        {
            Log.Clear();
            TestFlag = true;
            action.Invoke();
            TestFlag = false;
            Quick.Log(Log.ToString());
        }
    }
}
