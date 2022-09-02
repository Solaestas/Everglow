using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.ZYModule.Visuals.ScreenShaders
{
    [Flags]
    internal enum ScreenParameter
    {
        None = 0,
        uResolution = 1,
        uInvZoom = 2,
        uZoomMatrix = 4,
        uInvResolution = 8,
        uOpacity = 16,
        uTime = 32,
        uNoise = 64
    }
}
