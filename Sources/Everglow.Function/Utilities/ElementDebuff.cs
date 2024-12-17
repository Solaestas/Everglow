namespace Everglow.Commons.Utilities;

public partial class ElementDebuff
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

	/// <summary>
	/// The type of element debuff
	/// </summary>
	public ElementDebuffType Type { get; init; }

	/// <summary>
	/// The element debuff build-up, use <see cref="ElementDebuff.AddBuildUp"/> to edit this property.
	/// </summary>
	public int BuildUp { get; private set; }

	/// <summary>
	/// The element debuff build-up limitation. Once the build-up is more than this max value, the element debuff will proc
	/// </summary>
	public int BuildUpMax { get; private set; }

	/// <summary>
	/// Elemental Resistance can reduce the Elemental Debuff build-up they took
	/// </summary>
	public float ElementResistance { get; private set; }

	/// <summary>
	/// Determine whether this element debuff is in proc state.
	/// <para/>This property should only be edit by <see cref="ElementDebuff.UpdateBuildUp(NPC)"/>.
	/// </summary>
	public bool Proc { get; private set; }

	/// <summary>
	/// The timer of element debuff proc state
	/// </summary>
	public int Duration { get; private set; }

	/// <summary>
	/// The duration of element debuff proc state
	/// </summary>
	public int DurationMax { get; private set; }

	/// <summary>
	/// Damage per second when element debuff is proc
	/// </summary>
	public int DotDamage { get; private set; }

	/// <summary>
	/// Damage on element debuff proc
	/// </summary>
	public int ProcDamage { get; private set; }

	/// <summary>
	/// Add element debuff build-up, calculate resistance and penetration.
	/// </summary>
	/// <param name="buildUp">Build-up value</param>
	/// <param name="elementPenetration">Element penetration</param>
	/// <returns></returns>
	public bool AddBuildUp(int buildUp, float elementPenetration = 0)
	{
		if (Proc || buildUp <= 0 || elementPenetration < 0)
		{
			return false;
		}
		else
		{
			// Calculate element resistance and penetration
			float finElementResisitance = ElementResistance - elementPenetration;
			if (finElementResisitance < 0)
			{
				finElementResisitance = 0;
			}
			else if (finElementResisitance > 1)
			{
				finElementResisitance = 1;
			}

			BuildUp += (int)(buildUp * (1 - finElementResisitance));
			return true;
		}
	}

	/// <summary>
	/// Update the debuff and apply proc effect to npc
	/// </summary>
	/// <param name="npc"></param>
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

	/// <summary>
	/// Apply proc state effect dot damage to npc, which can reduce npc's life regen
	/// </summary>
	/// <param name="npc"></param>
	public void ApplyEffect(NPC npc)
	{
		if (Proc)
		{
			npc.lifeRegen -= DotDamage;
		}
	}

	public void SetBuildUpMax(int buildUpMax)
	{
		BuildUpMax = buildUpMax;
	}

	public void SetDurationMax(int durationMax)
	{
		DurationMax = durationMax;
	}

	public void SetDotDamage(int dotDamage)
	{
		DotDamage = dotDamage;
	}

	public void SetProcDamage(int procDamage)
	{
		ProcDamage = procDamage;
	}
}