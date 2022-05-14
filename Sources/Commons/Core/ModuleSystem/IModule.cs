using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.ModuleSystem
{
    public interface IModule
    {
        public string Name { get; }
        public string Description { get; }
        public void Load();
        public void Unload();
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class DontAutoLoadAttribute : Attribute
    {

    }

    /// <summary>
    /// 用于指定一个IModule类型的加载前置依赖类型
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ModuleDependencyAttribute : Attribute
    {
        private List<Type> m_dependTypes;
        public ModuleDependencyAttribute(params Type[] types)
        {
            m_dependTypes = types.ToList();
            Debug.Assert(types.All(t => typeof(IModule).IsAssignableFrom(t)));
        }
        public List<Type> DependTypes { get { return m_dependTypes; } }
    }
}
