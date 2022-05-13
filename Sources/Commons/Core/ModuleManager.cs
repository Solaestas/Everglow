using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons
{
    public class ModuleManager
    {
        private static ModuleManager instance;
        public static ModuleManager Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ModuleManager();
                }
                return instance;
            }
        }
        internal Dictionary<Type, IModule> modulesByType;
        internal Dictionary<string, IModule> modulesByName;
        internal List<IModule> modules;
        /// <summary>
        /// 为了方便，直接写成了static
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetModule<T>() where T : IModule
        {
            return (T)Instance.modulesByType[typeof(T)];
        }
        public static IModule GetModule(Type type) => Instance.modulesByType[type];
        public static IModule GetModule(string name) => Instance.modulesByName[name];
        /// <summary>
        /// 查找类型为或者继承自T的IModule
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IEnumerable<T> FindModule<T>() where T : IModule => from ins in Instance.modules where ins is T select (T)ins;
        public static IEnumerable<IModule> FindModule(Type type) => from ins in Instance.modules
                                                                    where ins.GetType().IsSubclassOf(type) 
                                                                       || ins.GetType() == type
                                                                    select ins;
        public static IEnumerable<IModule> FindModule(string name) => from ins in Instance.modules where ins.Name == name select ins;
        /// <summary>
        /// 虽然我觉得Module应该不会后期加载，但还是写上去了
        /// </summary>
        /// <param name="module"></param>
        public static void AddModule(IModule module)
        {
            var ins = Instance;
            ins.modules.Add(module);
            //应该不会重复加同一种module吧……，反正这样写是不会报错就是了
            ins.modulesByName[module.Name] = module;
            ins.modulesByType[module.GetType()] = module;
            module.Load();
        }
        /// <summary>
        /// 提前卸载一个Module
        /// </summary>
        /// <param name="module"></param>
        public static bool RemoveModule(IModule module)
        {
            var ins = Instance;
            if(!ins.modules.Remove(module))
            {
                return false;
            }
            ins.modulesByName.Remove(module.Name);
            ins.modulesByType.Remove(module.GetType());
            module.Unload();
            return true;
        }
        public void Load()
        {
            modules = new List<IModule>();
            modulesByType = new Dictionary<Type, IModule>();
            modulesByName = new Dictionary<string, IModule>();
            var assembly = Assembly.GetAssembly(typeof(ModuleManager));
            foreach (var type in assembly.GetTypes().Where(type => !type.IsAbstract &&
                type.GetInterfaces().Contains(typeof(IModule)) &&
                Attribute.IsDefined(type, typeof(DontAutoLoadAttribute))
                ))
            {
                IModule ins = Activator.CreateInstance(type) as IModule;
                modules.Add(ins);
                modulesByType[type] = ins;
                modulesByName[ins.Name] = ins;
            }
            
            for (int i = 0; i < modules.Count; i++)
            {
                modules[i].Load();
            }
        }
        public void Unload()
        {
            if (modules is not null)
            {
                modules.Reverse();
                foreach (var mod in modules)
                {
                    mod.Unload();
                }
                modules = null;
            }
            modulesByType = null;
            modulesByName = null;
        }
        
    }
}
