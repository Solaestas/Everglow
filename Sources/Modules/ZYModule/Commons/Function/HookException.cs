using Everglow.Sources.Commons.Core.ModuleSystem;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function
{
    internal class HookException : Exception
    {
        private static readonly string[] safeMods = new string[]
        {
            "ModLoader",
            "Hero",
            "CheatSheet",
            "Everglow"
        };
        private static readonly HashSet<string> hasThrown = new HashSet<string>();
        private HookException(string message, Exception innerException = null) : base(message, innerException) { }
        public static void Throw(string HookName, Exception innerException = null)
        {
            if(!hasThrown.Contains(HookName))
            {
                hasThrown.Add(HookName);
            }
            if(innerException is HookException)
            {
                Quick.Throw(innerException);
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("已加载Mod :");
            foreach (var mod in ModLoader.Mods)
            {
                if(safeMods.Contains(mod.Name))
                {
                    continue;
                }
                builder.AppendLine($"Name : {mod.DisplayName}, Version : {mod.Version}");
            }
            builder.AppendLine($"异常Hook : {HookName}");
            Quick.Throw(new HookException(builder.ToString(), innerException));
        }
        public static void Silent(string HookName, Exception innerException = null)
        {
            if(hasThrown.Contains(HookName))
            {
                return;
            }
            hasThrown.Add(HookName);
            if(innerException is HookException)
            {
                Quick.Log(innerException);
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("已加载Mod :");
            foreach (var mod in ModLoader.Mods)
            {
                if (safeMods.Contains(mod.Name))
                {
                    continue;
                }
                builder.AppendLine($"Name : {mod.DisplayName}, Version : {mod.Version}");
            }
            builder.AppendLine($"异常Hook : {HookName}");
            Quick.Log(new HookException(builder.ToString(), innerException));
        }
        /// <summary>
        /// 检测未抛出过异常
        /// </summary>
        /// <param name="HookName"></param>
        /// <returns></returns>
        public static bool Check(string HookName) => !hasThrown.Contains(HookName);
    }

}
