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

	public override void AI(NPC npc)
	{
		foreach (var element in elementalDebuffs)
		{
			element.Value.Update(npc);
		}
	}

	public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var lightColor = Lighting.GetColor((int)npc.position.X, (int)npc.position.Y);

		SpriteBatchState sBS = spriteBatch.GetState().Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

		// Draw element debuff icons and VFXs
		int activeElementDebuffCount = elementalDebuffs.Where(x => x.Value.Proc || x.Value.HasBuildUp).Count();
		int drawedElementDebuffCount = 0;
		foreach (var (elementType, element) in elementalDebuffs)
		{
			if (!element.Proc && !element.HasBuildUp)
			{
				continue;
			}

			var buildUpColor = Color.Lerp(Color.White, lightColor, 0.3f);
			var durationColor = Color.Lerp(Color.Gray, lightColor, 0.3f);
			var backgroundColor = Color.Lerp(element.Color, lightColor, 0.5f);

			var drawPosition = npc.Center - Main.screenPosition;
			drawPosition += new Vector2(0, -npc.height / 2 * 1.4f).RotatedBy(MathHelper.TwoPi * ((activeElementDebuffCount - 1) / 2f - drawedElementDebuffCount) / Enum.GetValues<ElementalDebuffType>().Length);

			var scale = 0.1f * npc.height / 100;
			if (scale < 0.05f)
			{
				scale = 0.05f;
			}

			// Draw element debuff icons and VFXs
			if (element.Proc)
			{
				ValueBarHelper.DrawCircleValueBar(spriteBatch, drawPosition, element.Duration / (float)element.DurationMax, durationColor, backgroundColor, scale, element.Texture.Value);
				drawedElementDebuffCount++;
			}
			else if (element.HasBuildUp)
			{
				ValueBarHelper.DrawCircleValueBar(spriteBatch, drawPosition, element.BuildUp / (float)element.BuildUpMax, buildUpColor, backgroundColor, scale, element.Texture.Value);
				drawedElementDebuffCount++;
			}
		}

		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}
}