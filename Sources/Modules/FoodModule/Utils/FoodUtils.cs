using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.FoodModule.Utils
{
    internal static class FoodUtils
    {
        public static int GetFrames(int hours, int minutes, int seconds, int frames)
        {
            return (((hours * 60 + minutes) * 60 + seconds) * 60) + frames;
        } 
    }
}
