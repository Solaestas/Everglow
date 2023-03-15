using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.Enums;
[Flags]
public enum TerrariaFunction
{
	None = 0,
	DrawBackground = 1,
	DrawNPCs = 2,
	DrawSkyAndHell = 4,
}
