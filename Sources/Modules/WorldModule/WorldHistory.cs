using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.IO;

namespace Everglow.Sources.Modules.WorldModule
{
    internal class WorldHistory
    {
        public WorldHistory(WorldFileData root)
        {
            this.root = root;
        }
        /// <summary>
        /// 世界根源
        /// </summary>
        public readonly WorldFileData root;
        /// <summary>
        /// 用来缓冲当前尝试进入的世界,不立即添加到历史记录中
        /// </summary>
        internal World buffer;
        /// <summary>
        /// 历史记录,实际修改时才用.读取记录用属性History!
        /// </summary>
        internal Queue<World> history = new();
        /// <summary>
        /// 读取历史记录
        /// </summary>
        public IReadOnlyList<World> History => history.ToList();
        /// <summary>
        /// 读取访问过的世界
        /// </summary>
        public IReadOnlySet<World> Visited => history.ToHashSet();
    }
}
