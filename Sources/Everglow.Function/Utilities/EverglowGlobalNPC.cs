using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Steamworks;

namespace Everglow.Commons.Utilities;

public partial class EverglowGlobalNPC : GlobalNPC
{
	public List<ElementDebuff> elementDebuffs = [];

	public override bool InstancePerEntity => true;

	public override void SetDefaults(NPC entity)
	{
		foreach (var elementType in Enum.GetValues(typeof(ElementDebuff.ElementDebuffType)))
		{
			elementDebuffs.Add(new((ElementDebuff.ElementDebuffType)elementType));
		}
	}

	public override void UpdateLifeRegen(NPC npc, ref int damage)
	{
		foreach (var element in elementDebuffs)
		{
			element.ApplyEffect(npc);
		}
	}

	public override void AI(NPC npc)
	{
		foreach (var element in elementDebuffs)
		{
			element.UpdateBuildUp(npc);
		}
	}

	public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		SpriteBatchState sBS = GraphicsUtils.GetState(spriteBatch).Value;
		spriteBatch.End();
		spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, default, default, null, Main.GameViewMatrix.TransformationMatrix);

		var element = elementDebuffs[(int)ElementDebuff.ElementDebuffType.Necrosis];
		if (element.Proc)
		{
			ValueBarHelper.DrawValueBar(spriteBatch, npc.Bottom - Main.screenPosition, element.Duration / (float)element.DurationMax, Color.Gray, Color.Gray);
		}
		else if (element.BuildUp > 0)
		{
			ValueBarHelper.DrawValueBar(spriteBatch, npc.Bottom - Main.screenPosition, element.BuildUp / (float)element.BuildUpMax, Color.White, Color.White);
		}

		spriteBatch.End();
		spriteBatch.Begin(sBS);
	}
}