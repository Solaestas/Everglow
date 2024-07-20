using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.NPCs.NPCList
{
	#region EventNPCs
	public class BloodMoonNPCs : GlobalNPC
	{
		public static List<int> vanillaBloodMoonNPCs;
		public override void Unload()
		{
			vanillaBloodMoonNPCs = null;
		}

		public BloodMoonNPCs()
		{
			vanillaBloodMoonNPCs = new List<int>
			{
				47,52, 53, 57, 109,168,186, 187, 188, 189, 190, 191, 192,
				193, 194, 316, 317, 318, 319, 320,
				321, 322, 323, 324, 331, 332,378, 430, 431, 432,332,
				434, 435, 436, 464, 465,470, 489,
				490,587,590,591,618,619,620,621,622,623
			};
		}
	}
	public class EclipseNPCs : GlobalNPC
	{
		public static List<int> vanillaEclipseNPCs;
		public override void Unload()
		{
			vanillaEclipseNPCs = null;
		}

		public EclipseNPCs()
		{
			vanillaEclipseNPCs = new List<int>
			{
				251, 253, 162, 166, 159, 158, 460, 461, 462, 463,
				466, 467, 468, 469, 477, 478, 479
			};
		}
	}
	public class FrostMoonNPCs : GlobalNPC
	{
		public static List<int> vanillaFrostMoonNPCs;
		public override void Unload()
		{
			vanillaFrostMoonNPCs = null;
		}

		public FrostMoonNPCs()
		{
			vanillaFrostMoonNPCs = new List<int>
			{
				338, 339, 340, 341, 342, 343, 344, 345, 346, 347,
				348, 349, 350, 351, 352
			};
		}
	}
	public class PumpkinMoonNPCs : GlobalNPC
	{
		public static List<int> vanillaPumpkinMoonNPCs;
		public override void Unload()
		{
			vanillaPumpkinMoonNPCs = null;
		}

		public PumpkinMoonNPCs()
		{
			vanillaPumpkinMoonNPCs = new List<int>
			{
				305, 306, 307, 308, 309, 310, 311, 312, 313, 314,
				315, 325, 326, 327, 328, 329, 330
			};
		}
	}
	#endregion
}