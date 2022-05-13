using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons
{
    public class ModuleManager
    {
        private Dictionary<string, IModule> m_modules;

        public ModuleManager()
        {
            m_modules = new Dictionary<string, IModule>();
        }

        public void AddModule(IModule module)
        {
            m_modules.Add(module.Name, module);
        }

        public IModule GetModule(string name)
        {
            return m_modules[name];
        }

        public void LoadAll()
        {
            foreach(var module in m_modules.Values)
            {
                module.Load();
            }
        }

        public void UnloadAll()
        {
            foreach (var module in m_modules.Values)
            {
                module.Unload();
            }
        }
    }
}
