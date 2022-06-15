using Everglow.Sources.Commons.Core.ModuleSystem;
using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Commons.Function
{
    internal class ILException : Exception
    {
        private ILException(string message, Exception innerException = null) : base(message, innerException) { }
        public static void Throw(string ILName, Exception innerException = null)
        {
            if(innerException is ILException)
            {
                Quick.Throw(innerException);
            }
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("已加载Mod :");
            foreach (var mod in ModLoader.Mods)
            {
                builder.AppendLine($"Name : {mod.DisplayName}, Version : {mod.Version}");
            }
            builder.AppendLine($"异常IL : {ILName}");
            Quick.Throw(new ILException(builder.ToString(), innerException));
        }
    }

}
