using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.ElementDebuff;

public partial class ElementDebuffGlobalNPC : GlobalNPC
{
	private Dictionary<ElementDebuffType, ElementDebuff> elementDebuffs = [];

	public IReadOnlyDictionary<ElementDebuffType, ElementDebuff> ElementDebuffs => elementDebuffs.AsReadOnly();

	public override bool InstancePerEntity => true;

	public override void SetDefaults(NPC entity)
	{
		foreach (var elementType in ElementDebuffRegistry.GetAllTypes())
		{
			elementDebuffs.Add(elementType, ElementDebuffRegistry.CreateDebuff(elementType));
		}
	}

	public override void UpdateLifeRegen(NPC npc, ref int damage)
	{
		foreach (var element in elementDebuffs)
		{
			element.Value.ApplyDotDamage(npc);
		}
	}

	public override void AI(NPC npc)
	{
		foreach (var element in elementDebuffs)
		{
			element.Value.UpdateBuildUp(npc);
		}
	}

	public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var lightColor = Lighting.GetColor((int)npc.position.X, (int)npc.position.Y);

		SpriteBatchState sBS = spriteBatch.GetState().Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

		// Draw element debuff icons and VFXs
		int activeElementDebuffCount = elementDebuffs.Where(x => x.Value.Proc || x.Value.HasBuildUp).Count();
		int drawedElementDebuffCount = 0;
		foreach (var (elementType, element) in elementDebuffs)
		{
			if (!element.Proc && !element.HasBuildUp)
			{
				continue;
			}

			var buildUpColor = Color.Lerp(Color.White, lightColor, 0.5f);
			var durationColor = Color.Lerp(Color.Gray, lightColor, 0.5f);
			var backgroundColor = Color.Lerp(element.Color, lightColor, 0.5f);

			var drawPosition = npc.Center - Main.screenPosition;
			drawPosition += new Vector2(0, -npc.height / 2 * 1.4f).RotatedBy(MathHelper.TwoPi * ((activeElementDebuffCount - 1) / 2f - drawedElementDebuffCount) / Enum.GetValues<ElementDebuffType>().Length);

			var scale = 0.1f * npc.height / 100;
			if (scale < 0.05f)
			{
				scale = 0.05f;
			}

			// Draw element debuff icons and VFXs
			if (element.Proc)
			{
				ValueBarHelper.DrawCircleValueBar(spriteBatch, drawPosition, element.Duration / (float)element.DurationMax, durationColor, backgroundColor, scale, element.Icon.Value);
				drawedElementDebuffCount++;
			}
			else if (element.HasBuildUp)
			{
				ValueBarHelper.DrawCircleValueBar(spriteBatch, drawPosition, element.BuildUp / (float)element.BuildUpMax, buildUpColor, backgroundColor, scale, element.Icon.Value);
				drawedElementDebuffCount++;
			}
		}

		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}
}