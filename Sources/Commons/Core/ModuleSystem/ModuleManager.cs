using Everglow.Sources.Commons.Core.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.ModuleSystem
{
    public class ModuleManager
    {
        private Dictionary<Type, IModule> modulesByType;
        private Dictionary<string, IModule> modulesByName;
        private List<IModule> modules;

        public ModuleManager()
        {
            modulesByType = new Dictionary<Type, IModule>();
            modulesByName = new Dictionary<string, IModule>();
            modules = new List<IModule>();

            LoadAllModules();
        }


        private void LoadAllModules()
        {
            var dependencyGraph = new DependencyGraph();
            var assembly = Assembly.GetAssembly(typeof(ModuleManager));
            foreach (var type in assembly.GetTypes()
                .Where(type => 
                !type.IsAbstract &&
                type.GetInterfaces().Contains(typeof(IModule)) &&
                !Attribute.IsDefined(type, typeof(DontAutoLoadAttribute))
                ))
            {
                var dependency = type.GetCustomAttribute<ModuleDependencyAttribute>(true);
                if (dependency is null)
                {
                    dependencyGraph.AddType(type);
                }
                else
                { 
                    foreach (var dependType in dependency.DependTypes)
                    {
                        dependencyGraph.AddDependency(dependType, type);
                    }
                }
            }
            

            foreach(var type in dependencyGraph.TopologicalSort())
            {
                IModule ins = Activator.CreateInstance(type) as IModule;
                ins.Load();
                modules.Add(ins);
                modulesByType[type] = ins;
                modulesByName[ins.Name] = ins;
            }
        }

        /// <summary>
        /// 为了方便，直接写成了static
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModule<T>() where T : IModule
        {
            return (T)modulesByType[typeof(T)];
        }
        public IModule GetModule(Type type) => modulesByType[type];
        public IModule GetModule(string name) => modulesByName[name];
        /// <summary>
        /// 查找类型为或者继承自T的IModule
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> FindModule<T>() where T : IModule => from ins in modules where ins is T select (T)ins;
        public IEnumerable<IModule> FindModule(Type type) => from ins in modules
                                                                    where ins.GetType().IsSubclassOf(type) 
                                                                       || ins.GetType() == type
                                                                    select ins;
        public IEnumerable<IModule> FindModule(string name) => from ins in modules 
                                                               where ins.Name == name select ins;
        /// <summary>
        /// 缺乏依赖会抛出异常
        /// </summary>
        /// <param name="module"></param>
        public void AddModule(IModule module)
        {
            if (Attribute.IsDefined(module.GetType(), typeof(ModuleDependencyAttribute)))
            {
                var attr = module.GetType().GetCustomAttribute<ModuleDependencyAttribute>();
                foreach(var type in attr.DependTypes)
                {
                    if(!modulesByType.ContainsKey(type))
                    {
                        throw new InvalidOperationException($"当前加载Module的依赖Module  {type.Name}  并未加载");
                    }
                }
            }
            modules.Add(module);
            //应该不会重复加同一种module吧……，反正这样写是不会报错就是了
            modulesByName[module.Name] = module;
            modulesByType[module.GetType()] = module;
            module.Load();
        }
        /// <summary>
        /// 提前卸载一个Module
        /// 依赖项不能自动卸载
        /// </summary>
        /// <param name="module"></param>
        public bool RemoveModule(IModule module)
        {
            if(!modules.Remove(module))
            {
                return false;
            }
            modulesByName.Remove(module.Name);
            modulesByType.Remove(module.GetType());
            module.Unload();
            return true;
        }

        public void Unload()
        {
            if (modules is not null)
            {
                foreach (var mod in modules.Reverse<IModule>())
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
