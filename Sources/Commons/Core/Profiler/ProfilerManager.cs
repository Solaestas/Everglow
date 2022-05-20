using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Core.Profiler
{
    /// <summary>
    /// 用于管理性能分析数据的类
    /// </summary>
    internal class ProfilerManager
    {
        private Dictionary<string, UnitInfo> m_unitInfoTable;

        public ProfilerManager()
        {
            m_unitInfoTable = new Dictionary<string, UnitInfo>();
        }

        /// <summary>
        /// 向当前分析数据里添加一个条目
        /// </summary>
        /// <param name="name"></param>
        /// <param name="timeInMs"></param>
        public void AddEntry(string name, double timeInMs)
        {
            if (m_unitInfoTable.ContainsKey(name))
            {
                var info = m_unitInfoTable[name];
                info.TimeInMs += timeInMs;
                info.Count++;
            }
            else
            {
                m_unitInfoTable.Add(name, new UnitInfo(name, 1, timeInMs));
            }
        }

        public void Clear()
        {
            m_unitInfoTable.Clear();
        }

        public void PrintSummary()
        {
            List<UnitInfo> entries = new List<UnitInfo>();
            foreach (var unitInfo in m_unitInfoTable.Values)
            {
                entries.Add(unitInfo);
            }

            entries.Sort((a, b) =>
            {
                if (a.TimeInMs == b.TimeInMs)
                    return 0;
                return a.TimeInMs > b.TimeInMs ? -1 : 1;
            });

            foreach (var unitInfo in entries)
            {
                Console.WriteLine($"{unitInfo.Name}, Time = {unitInfo.TimeInMs:F2}ms, Count = {unitInfo.Count}");
            }
        }
    }
}
