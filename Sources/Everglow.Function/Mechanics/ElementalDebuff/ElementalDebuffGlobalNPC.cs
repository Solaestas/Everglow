using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.ElementalDebuff;

public partial class ElementalDebuffGlobalNPC : GlobalNPC
{
	private Dictionary<ElementalDebuffType, ElementalDebuff> elementalDebuffs = [];

	public IReadOnlyDictionary<ElementalDebuffType, ElementalDebuff> ElementalDebuffs => elementalDebuffs.AsReadOnly();

	public override bool InstancePerEntity => true;

	public override void SetDefaults(NPC entity)
	{
		foreach (var elementType in ElementalDebuffRegistry.GetAllTypes())
		{
			elementalDebuffs.Add(elementType, ElementalDebuffRegistry.CreateDebuff(elementType)
				.SetInfo(ElementalDebuffInfoRegistry.GetInfo(entity.type, elementType)));
		}
	}

	public override void UpdateLifeRegen(NPC npc, ref int damage)
	{
		foreach (var element in elementalDebuffs)
		{
			element.Value.ApplyDot(npc);
		}
	}

	public override void ResetEffects(NPC npc)
	{
		foreach (var element in elementalDebuffs)
		{
			element.Value.ResetEffects();
		}
	}

	public override bool? DrawHealthBar(NPC npc, byte hbPosition, ref float scale, ref Vector2 position)
	{
		var lightColor = Lighting.GetColor((int)npc.Center.X, (int)npc.Center.Y);

		SpriteBatch spriteBatch = Main.spriteBatch;
		SpriteBatchState sBS = spriteBatch.GetState().Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

		// Draw element debuff icons and VFXs
		int activeElementDebuffCount = elementalDebuffs.Where(x => x.Value.Proc || x.Value.HasBuildUp).Count();
		int drawedElementDebuffCount = 0;
		var drawCenter = position - Main.screenPosition;

		foreach (var (elementType, element) in elementalDebuffs)
		{
			if (!element.Proc && !element.HasBuildUp)
			{
				continue;
			}

			var buildUpColor = Color.Lerp(Color.White * 0.85f, lightColor, 0.3f);
			var durationColor = Color.Lerp(Color.Gray, lightColor, 0.3f);
			var backgroundColor = Color.Lerp(element.Color, lightColor, 0.5f);
			float modifiedX = ((activeElementDebuffCount - 1) / 2f - drawedElementDebuffCount) * scale * 20;
			var drawPosition = drawCenter + new Vector2(modifiedX, -10 * scale);
			var buffBarScale = scale * 0.06f;

			// Draw element debuff icons and VFXs
			if (element.Proc)
			{
				float breakingTime = (element.DurationMax - element.Duration) / MathF.Min(30, element.DurationMax);
				if (breakingTime < 1f)
				{
					ValueBarHelper.DrawBreakOutEffect(spriteBatch, drawPosition, breakingTime, buffBarScale);
				}
				ValueBarHelper.DrawCircleValueBar(spriteBatch, drawPosition, element.Duration / (float)element.DurationMax, durationColor, backgroundColor, buffBarScale, element.Texture.Value);
				drawedElementDebuffCount++;
			}
			else if (element.HasBuildUp)
			{
				ValueBarHelper.DrawCircleValueBar(spriteBatch, drawPosition, 1 - element.BuildUp / (float)element.BuildUpMax, buildUpColor, backgroundColor, buffBarScale, element.Texture.Value);
				drawedElementDebuffCount++;
			}
		}

		spriteBatch.End();
		spriteBatch.Begin(sBS);
		return base.DrawHealthBar(npc, hbPosition, ref scale, ref position);
	}

	public override void AI(NPC npc)
	{
		foreach (var element in elementalDebuffs)
		{
			element.Value.Update(npc);
		}
	}

	// public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	// {

	// }
}