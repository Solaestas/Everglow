using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Common;
public static class ModIns
{
	public static Mod Mod { get; private set; }
	public static void SetInstance(Mod mod)
	{
		Mod = mod;
	}

	public static void DisposeAll()
	{
		Mod = null;
	}
}
