using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Core.Profiler.Fody;

namespace Everglow.Sources.Commons.Core.ModuleSystem
{
    [ProfilerMeasure]
    /// <summary>
    /// 用于管理模块加载、卸载的类
    /// </summary>
    public class ModuleManager
    {
        private Dictionary<Type, IModule> modulesByType = new Dictionary<Type, IModule>();
        private Dictionary<string, IModule> modulesByName = new Dictionary<string, IModule>();
        private List<IModule> modules = new List<IModule>();

        /// <summary>
        /// 从程序集的类型中加载所有Module，并且按照其加载依赖关系排序
        /// </summary>
        public void LoadAllModules()
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

            //这里先把List和Dictionary设置好，在执行Load，可以避免一些基本的因为调用其他Module产生的依赖关系
            foreach (var type in dependencyGraph.TopologicalSort())
            {
                IModule module = Activator.CreateInstance(type) as IModule;
                modules.Add(module);
                modulesByName.Add(module.Name, module);
                modulesByType.Add(module.GetType(), module);
            }
            foreach (var module in modules)
            {
                module.Load();
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
                                                               where ins.Name == name
                                                               select ins;
        public IEnumerable<IModule> FindModule(Func<IModule, bool> predicate) => modules.Where(predicate);
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
                foreach (var type in attr.DependTypes)
                {
                    if (!modulesByType.ContainsKey(type))
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
            if (!modules.Remove(module))
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
