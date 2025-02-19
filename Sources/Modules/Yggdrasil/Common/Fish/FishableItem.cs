using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Yggdrasil.Common.Fish;

public class FishableItem(int item, int liquid, float chance)
{
	public int Item = item;
	public int Liquid = liquid;
	public float chance = chance;
}