using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementDebuff;

public abstract class ElementDebuff
{
	public ElementDebuff(ElementDebuffType type, Asset<Texture2D> icon, Color color)
	{
		Type = type;
		Icon = icon;
		Color = color;

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
	/// The icon of element debuff
	/// </summary>
	public Asset<Texture2D> Icon { get; init; }

	/// <summary>
	/// The background color of element debuff in-game icon
	/// </summary>
	public Color Color { get; init; }

	/// <summary>
	/// The element debuff build-up, use <see cref="AddBuildUp"/> to edit this property.
	/// </summary>
	public int BuildUp { get; protected set; }

	/// <summary>
	/// The element debuff build-up limitation. Once the build-up is more than this max value, the element debuff will proc
	/// </summary>
	public int BuildUpMax { get; protected set; }

	/// <summary>
	/// If the element debuff has build-up
	/// </summary>
	public bool HasBuildUp => BuildUp > 0;

	/// <summary>
	/// Elemental Resistance can reduce the Elemental Debuff build-up they took
	/// </summary>
	public float ElementResistance { get; protected set; }

	/// <summary>
	/// Determine whether this element debuff is in proc state.
	/// <para/>This property should only be edit by <see cref="UpdateBuildUp(NPC)"/>.
	/// </summary>
	public bool Proc { get; protected set; }

	/// <summary>
	/// The timer of element debuff proc state
	/// </summary>
	public int Duration { get; protected set; }

	/// <summary>
	/// The duration of element debuff proc state
	/// </summary>
	public int DurationMax { get; protected set; }

	/// <summary>
	/// Damage per second when element debuff is proc
	/// </summary>
	public int DotDamage { get; protected set; }

	/// <summary>
	/// Damage on element debuff proc
	/// </summary>
	public int ProcDamage { get; protected set; }

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
	/// <para/> Should only be called in <see cref="GlobalNPC.AI(NPC)"/>
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
				OnProc(npc);
			}
		}
	}

	public virtual void OnProc(NPC npc)
	{
		Proc = true;
		BuildUp = 0;
		Duration = DurationMax;

		if (npc.dontTakeDamage || npc.life <= 0)
		{
			return;
		}

		npc.life -= ProcDamage;
		if (npc.life <= 0)
		{
			npc.life = 1;
		}
	}

	/// <summary>
	/// Apply proc state effect dot damage to npc, which can reduce npc's life regen.
	/// <para/> Should only be called in <see cref="GlobalNPC.UpdateLifeRegen(NPC, ref int)"/>
	/// </summary>
	/// <param name="npc"></param>
	public void ApplyDotDamage(NPC npc)
	{
		if (Proc)
		{
			if (npc.dontTakeDamage)
			{
				return;
			}

			npc.lifeRegen -= DotDamage;
		}
	}

	internal void SetBuildUpMax(int buildUpMax)
	{
		BuildUpMax = buildUpMax;
	}

	internal void SetDurationMax(int durationMax)
	{
		DurationMax = durationMax;
	}

	internal void SetDotDamage(int dotDamage)
	{
		DotDamage = dotDamage;
	}

	internal void SetProcDamage(int procDamage)
	{
		ProcDamage = procDamage;
	}

	internal void SetElementResistance(float elementResistance)
	{
		ElementResistance = elementResistance;
	}

	public abstract void Draw(SpriteBatch spriteBatch);
}