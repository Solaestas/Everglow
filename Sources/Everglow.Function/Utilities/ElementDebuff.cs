using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Terraria.DataStructures;

namespace Everglow.Commons.Utilities;

public class ElementDebuff
{
	public ElementDebuff(ElementDebuffType type)
	{
		Type = type;
		BuildUp = 0;
		BuildUpMax = 1000;
		ElementResistance = 0f;
		Proc = false;
		Duration = 0;
		DurationMax = 60;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public enum ElementDebuffType
	{
		NervousImpairment,
		Corrosion,
		Burn,
		Necrosis,
	}

	public ElementDebuffType Type { get; init; }

	public int BuildUp { get; private set; }

	public int BuildUpMax { get; private set; }

	public float ElementResistance { get; private set; }

	public bool Proc { get; private set; }

	public int Duration { get; private set; }

	public int DurationMax { get; private set; }

	/// <summary>
	/// Damage per second when element debuff is proc
	/// </summary>
	public int DotDamage { get; private set; }

	/// <summary>
	/// Damage on element debuff proc
	/// </summary>
	public int ProcDamage { get; private set; }

	public bool AddBuildUp(int buildUp)
	{
		if (Proc || buildUp <= 0)
		{
			return false;
		}
		else
		{
			BuildUp += (int)(buildUp * (1 - ElementResistance));
			return true;
		}
	}

	public void UpdateBuildUp(NPC npc)
	{
		if (Proc)
		{
			Duration--;

			if (Duration <= 0)
			{
				Duration = 0;
				Proc = false;
			}
		}
		else
		{
			if (BuildUp >= BuildUpMax)
			{
				Proc = true;
				BuildUp = 0;
				Duration = DurationMax;

				npc.life -= ProcDamage;
				if (npc.life <= 0)
				{
					npc.life = 1;
				}
			}
		}
	}

	public void ApplyEffect(NPC npc)
	{
		if (Proc)
		{
			npc.lifeRegen -= DotDamage;
		}
	}
}