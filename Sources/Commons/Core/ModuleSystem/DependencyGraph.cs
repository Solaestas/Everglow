using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.ModuleSystem
{
    public class DependencyGraph
    {
        private Dictionary<Type, int> m_typeToIdMapping;
        private List<Type> m_types;
        private Dictionary<int, List<int>> m_dependencyGraph;
        private Dictionary<int, int> m_dependencyFanin;
        public DependencyGraph()
        {
            m_typeToIdMapping = new Dictionary<Type, int>();
            m_types = new List<Type>();
            m_dependencyGraph = new Dictionary<int, List<int>>();
            m_dependencyFanin = new Dictionary<int, int>();
        }
        /// <summary>
        /// 添加一个没有依赖的<paramref name="type"/>
        /// </summary>
        /// <param name="type"></param>
        public void AddType(Type type)
        {
            int t = GetInternalID(type);
        }

        /// <summary>
        /// 添加一个依赖于<paramref name="depend"/>的<paramref name="type"/>
        /// </summary>
        /// <param name="depend"></param>
        /// <param name="type"></param>
        public void AddDependency(Type depend, Type type)
        {
            int u = GetInternalID(depend);
            int v = GetInternalID(type);

            if (m_dependencyGraph.ContainsKey(u))
            {
                m_dependencyGraph[u].Add(v);
            }
            else
            {
                m_dependencyGraph[u] = new List<int> { v };
            }

            AddFanin(v);
        }
        private void AddFanin(int v)
        {
            if (m_dependencyFanin.ContainsKey(v))
            {
                m_dependencyFanin[v]++;
            }
            else
            {
                m_dependencyFanin[v] = 1;
            }
        }

        public List<Type> TopologicalSort()
        {
            List<Type> result = new List<Type>();
            Queue<int> queue = new Queue<int>();

            for (int i = 0; i < m_types.Count; i++)
            {
                if (GetFanin(i) == 0)
                {
                    queue.Enqueue(i);
                }
            }

            while(queue.Count > 0)
            {
                int u = queue.Dequeue();

                if (!m_dependencyGraph.ContainsKey(u))
                {
                    result.Add(m_types[u]);
                    continue;
                }
                result.Add(m_types[u]);
                foreach (var v in m_dependencyGraph[u])
                {
                    m_dependencyFanin[v]--;
                    if (m_dependencyFanin[v] == 0)
                    {
                        queue.Enqueue(v);
                    }
                }
            }

            // 如果依赖图出现环就直接报错加载失败
            if(result.Count < m_types.Count)
            {
                throw new ArgumentException("Circular dependency detected, please remove the circle");
            }
            return result;
        }

        private int GetInternalID(Type t)
        {
            if (!m_typeToIdMapping.ContainsKey(t))
            {
                m_typeToIdMapping.Add(t, m_types.Count);//是漏了吗？
                m_types.Add(t);
                return m_types.Count - 1;
            }
            return m_typeToIdMapping[t];
        }

        private int GetFanin(int v)
        {
            if (!m_dependencyFanin.ContainsKey(v))
            {
                return 0;
            }
            else
            {
                return m_dependencyFanin[v];
            }
        }
    }
}
