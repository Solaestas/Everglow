using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementalDebuff;

public abstract class ElementalDebuff
{
	public ElementalDebuff(ElementalDebuffType type, Asset<Texture2D> icon, Color color)
	{
		Type = type;
		Icon = icon;
		Color = color;

		BuildUp = 0;
		BuildUpMax = 1000;
		ElementalResistance = 0f;
		elementalResistanceStatModifier = StatModifier.Default;
		Proc = false;
		Duration = 0;
		DurationMax = 60;
		DotDamage = 2;
		ProcDamage = 200;
	}

	/// <summary>
	/// Elemental resistance state modifier
	/// </summary>
	private StatModifier elementalResistanceStatModifier;

	/// <summary>
	/// Use this to manage <see cref="ElementalResistanceCurrent"/>. Same to the usage of <see cref="Player.GetDamage{T}()"></see>
	/// </summary>
	/// <returns></returns>
	public ref StatModifier ElementalResistanceStatModifier => ref elementalResistanceStatModifier;

	/// <summary>
	/// The type of element debuff
	/// </summary>
	public ElementalDebuffType Type { get; init; }

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
	/// The element debuff build-up limitation. Once the build-up is more than this max value, the element debuff will proc.
	/// </summary>
	public int BuildUpMax { get; protected set; }

	/// <summary>
	/// If the element debuff has build-up.
	/// </summary>
	public bool HasBuildUp => BuildUp > 0;

	/// <summary>
	/// Elemental resistance can reduce the Elemental Debuff build-up they took.
	/// </summary>
	public float ElementalResistance { get; protected set; }

	/// <summary>
	/// Current elemental resistance value.
	/// </summary>
	public float ElementalResistanceCurrent => elementalResistanceStatModifier.ApplyTo(ElementalResistance);

	/// <summary>
	/// Determine whether this element debuff is in proc state.
	/// <para/>This property should only be edit by <see cref="Update(NPC)"/>.
	/// </summary>
	public bool Proc { get; protected set; }

	/// <summary>
	/// The timer of element debuff proc state.
	/// </summary>
	public int Duration { get; protected set; }

	/// <summary>
	/// The duration of element debuff proc state.
	/// </summary>
	public int DurationMax { get; protected set; }

	/// <summary>
	/// Damage per second when element debuff is proc.
	/// </summary>
	public int DotDamage { get; protected set; }

	/// <summary>
	/// Damage on element debuff proc.
	/// </summary>
	public int ProcDamage { get; protected set; }

	/// <summary>
	/// Add element debuff build-up, calculate resistance and penetration.
	/// </summary>
	/// <param name="buildUp">Build-up value</param>
	/// <param name="elementPenetration">Element penetration. If penetration is less than 0, it will be count as 0.</param>
	/// <returns></returns>
	public bool AddBuildUp(int buildUp, float elementPenetration = 0)
	{
		if (Proc || buildUp <= 0)
		{
			return false;
		}
		else
		{
			if (elementPenetration < 0)
			{
				elementPenetration = 0;
			}

			// Calculate element resistance and penetration
			float finElementResisitance = ElementalResistanceCurrent - elementPenetration;
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
	public void Update(NPC npc)
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

	/// <summary>
	/// Apply proc damage to npc, and set duration to max
	/// </summary>
	/// <param name="npc"></param>
	public virtual void OnProc(NPC npc)
	{
		Proc = true;
		BuildUp = 0;
		Duration = DurationMax;

		if (npc.dontTakeDamage)
		{
			return;
		}

		npc.lifeRegenCount -= ProcDamage * 120;

		PostProc(npc);
	}

	/// <summary>
	/// Custom effects on proc.
	/// </summary>
	/// <param name="npc"></param>
	public virtual void PostProc(NPC npc)
	{
	}

	/// <summary>
	/// Apply proc state effect dot damage to npc, which can reduce npc's life regen.
	/// <para/> Should only be called in <see cref="GlobalNPC.UpdateLifeRegen(NPC, ref int)"/>
	/// </summary>
	/// <param name="npc"></param>
	public void ApplyDot(NPC npc)
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

	/// <summary>
	/// Set elemental debuff custom value in the global npc, or the values will be default.
	/// <para/>Global npc instance is created after <see cref="ModNPC.SetDefaults"/>, so this method should not be called in <see cref="ModNPC.SetDefaults"/>
	/// </summary>
	/// <param name="npc"></param>
	/// <param name="type"></param>
	/// <param name="buildUpMax"></param>
	/// <param name="durationMax"></param>
	/// <param name="dotDamage"></param>
	/// <param name="procDamage"></param>
	/// <param name="elementResistance"></param>
	public ElementalDebuff SetInfo(ElementalDebuffInfo elementalDebuffInfo)
	{
		if (elementalDebuffInfo.BuildUpMax != null)
		{
			BuildUpMax = elementalDebuffInfo.BuildUpMax.Value;
		}

		if (elementalDebuffInfo.DurationMax != null)
		{
			DurationMax = elementalDebuffInfo.DurationMax.Value;
		}

		if (elementalDebuffInfo.DotDamage != null)
		{
			DotDamage = elementalDebuffInfo.DotDamage.Value;
		}

		if (elementalDebuffInfo.ProcDamage != null)
		{
			ProcDamage = elementalDebuffInfo.ProcDamage.Value;
		}

		if (elementalDebuffInfo.ElementalResistance != null)
		{
			ElementalResistance = elementalDebuffInfo.ElementalResistance.Value;
		}

		return this;
	}

	/// <summary>
	/// Reset all stat
	/// </summary>
	public void Reset()
	{
		Duration = 0;
		Proc = false;
		BuildUp = 0;
		elementalResistanceStatModifier = StatModifier.Default;
	}

	/// <summary>
	/// Be called in <see cref="GlobalNPC.ResetEffects(NPC)"/>
	/// </summary>
	public void ResetEffects()
	{
		elementalResistanceStatModifier = StatModifier.Default;
	}
}