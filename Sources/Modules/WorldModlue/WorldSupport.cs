using Everglow.Sources.Commons.Core.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.WorldModlue
{
    internal class WorldSupport : IModule
    {
        public string Name => nameof(WorldModlue);
        public void Load()
        {
            Hooks.Load();
        }
        public void Unload()
        {
            Hooks.Unload();
        }
        class Hooks
        {
            internal static void Unload()
            {
                ILHooks.Unload();
                OnHooks.Unload();
            }
            internal static void Load()
            {
                ILHooks.Load();
                OnHooks.Load();
            }
            class ILHooks
            {
                internal static void Unload()
                {
                }
                internal static void Load()
                {
                    //TODO 支持需要的钩子
                }
            }
            class OnHooks
            {
                internal static void Unload()
                {
                }
                internal static void Load()
                {
                    //TODO 支持需要的钩子
                }
            }
        }
    }
}
