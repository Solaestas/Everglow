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


        /// <summary>
        /// 从程序集的类型中加载所有Module，并且按照其加载依赖关系排序
        /// </summary>
        private void LoadAllModules()
        {
            var dependencyGraph = new DependencyGraph();
            var assembly = Assembly.GetExecutingAssembly();
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
                IModule module = Activator.CreateInstance(type) as IModule;
                module.Load();
                modules.Add(module);
                modulesByName.Add(module.Name, module);
                modulesByType.Add(module.GetType(), module);
            }
        }

        /// <summary>
        /// 按照类型获取模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetModule<T>() where T : IModule
        {
            return (T)modulesByType[typeof(T)];
        }
        /// <summary>
        /// 按照类型获取模块
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IModule GetModule(Type type) => modulesByType[type];

        /// <summary>
        /// 按照模块的名字获取模块
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IModule GetModule(string name) => modulesByName[name];

        /// <summary>
        /// 查找类型为或者继承自<typeparamref name="T"/>的IModule
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> FindModule<T>() where T : IModule => from ins in modules where ins is T select (T)ins;

        /// <summary>
        /// 查找类型为或者继承自<paramref name="type"/>的IModule
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
            if (modulesByName.ContainsKey(module.Name) || modulesByType.ContainsKey(module.GetType()))
            {
                throw new InvalidOperationException("Module already registered");
            }
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
            modulesByName.Add(module.Name, module);
            modulesByType.Add(module.GetType(), module);
            module.Load();
        }
        /// <summary>
        /// 【不建议使用】提前卸载一个Module
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

        /// <summary>
        /// 按照依赖逆序卸载所有模块
        /// </summary>
        public void UnloadAllModules()
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
