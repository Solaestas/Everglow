using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Everglow.Commons.Utilities;
using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.ElementalDebuff;

public partial class ElementalDebuffGlobalNPC : GlobalNPC
{
	private Dictionary<string, ElementalDebuffInstance> elementalDebuffs = [];

	public IReadOnlyDictionary<string, ElementalDebuffInstance> ElementalDebuffs => elementalDebuffs.AsReadOnly();

	public override bool InstancePerEntity => true;

	public override void SetDefaults(NPC entity)
	{
		foreach (var elementType in ElementalDebuffRegistry.GetTypes())
		{
			elementalDebuffs.Add(elementType, ElementalDebuffRegistry.GetInstance(elementType, entity));
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

			var handler = element.Handler;

			var buildUpColor = Color.Lerp(Color.White * 0.85f, lightColor, 0.3f);
			var durationColor = Color.Lerp(new Color(0.3f, 0.3f, 0.3f, 1f), lightColor, 0.3f);
			var backgroundColor = handler.Color;
			float modifiedX = ((activeElementDebuffCount - 1) / 2f - drawedElementDebuffCount) * scale * 20;
			var drawPosition = drawCenter + new Vector2(modifiedX, -10 * scale);
			var buffBarScale = scale * 0.06f;

			// Draw element debuff icons and VFXs
			if (element.Proc)
			{
				ValueBarHelper.DrawCircleValueBar(spriteBatch, drawPosition, 1 - element.TimeProgress, durationColor, backgroundColor, buffBarScale, handler.Texture.Value);
				var breakingTime = (handler.Duration - element.TimeLeft) / MathF.Min(30, handler.Duration);
				if (breakingTime < 1f)
				{
					ValueBarHelper.DrawBreakOutEffect(spriteBatch, drawPosition, breakingTime, buffBarScale);
				}
				drawedElementDebuffCount++;
			}
			else if (element.HasBuildUp)
			{
				ValueBarHelper.DrawCircleValueBar(spriteBatch, drawPosition, 1 - element.BuildUpProgress, buildUpColor, backgroundColor, buffBarScale, handler.Texture.Value);
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

	public override void SendExtraAI(NPC npc, BitWriter bitWriter, BinaryWriter binaryWriter)
	{
		var updates = elementalDebuffs.Where(e => e.Value.HasValue());
		binaryWriter.Write((byte)updates.Count());
		foreach (var (_, element) in updates)
		{
			binaryWriter.Write(element.NetID);
			element.NetSend(binaryWriter);
		}
	}

	public override void ReceiveExtraAI(NPC npc, BitReader bitReader, BinaryReader binaryReader)
	{
		var count = binaryReader.ReadByte();
		var writes = new List<string>();
		for (var i = 0; i < count; i++)
		{
			var netID = binaryReader.ReadUInt16();
			var id = ElementalDebuffRegistry.Registry[netID].ID;
			writes.Add(id);
			if (elementalDebuffs.TryGetValue(id, out var element))
			{
				element.NetReceive(binaryReader);
			}
			else
			{
				// Avoid binaryReader exception.
				ElementalDebuffInstance.EmptyNetReceive(binaryReader);
			}
		}

		foreach (var (_, element) in elementalDebuffs.Where(e => !writes.Contains(e.Key)))
		{
			element.Reset();
		}
	}
}