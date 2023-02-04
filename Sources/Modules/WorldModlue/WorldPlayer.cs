using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader.IO;

namespace Everglow.Sources.Modules.WorldModlue
{
    /// <summary>
    /// 用来实现某个我觉得非常坏的但是万象想要的效果
    /// </summary>
    internal class WorldPlayer : ModPlayer
    {
        public override void SaveData(TagCompound tag)
        {
            //TODO 储存各个WorldHistory
        }
        public override void LoadData(TagCompound tag)
        {
            //TODO 读取WorldHistory
        }
    }
}
