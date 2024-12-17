using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;

namespace Everglow.Commons.Utilities;

public partial class EverglowGlobalNPC : GlobalNPC
{
	private Dictionary<ElementDebuffType, ElementDebuff> elementDebuffs = [];

	public IReadOnlyDictionary<ElementDebuffType, ElementDebuff> ElementDebuffs => elementDebuffs.AsReadOnly();

	public override bool InstancePerEntity => true;

	public override void SetDefaults(NPC entity)
	{
		foreach (var elementType in Enum.GetValues(typeof(ElementDebuffType)))
		{
			elementDebuffs.Add((ElementDebuffType)elementType, new((ElementDebuffType)elementType));
		}
	}

	public override void UpdateLifeRegen(NPC npc, ref int damage)
	{
		foreach (var element in elementDebuffs)
		{
			element.Value.ApplyEffect(npc);
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

		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

		var element = elementDebuffs[ElementDebuffType.Necrosis];
		var buildUpColor = Color.White;
		//buildUpColor = buildUpColor.MultiplyRGBA(lightColor);
		var durationColor = Color.Gray;
		//durationColor = durationColor.MultiplyRGBA(lightColor);
		if (element.Proc)
		{
			ValueBarHelper.DrawValueBar(spriteBatch, npc.Bottom - Main.screenPosition, element.Duration / (float)element.DurationMax, durationColor, durationColor);
		}
		else if (element.BuildUp > 0)
		{
			ValueBarHelper.DrawValueBar(spriteBatch, npc.Bottom - Main.screenPosition, element.BuildUp / (float)element.BuildUpMax, buildUpColor, buildUpColor);
		}

		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}
}