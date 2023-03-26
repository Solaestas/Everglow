using Everglow.Myth.MiscItems.Buffs;
using Terraria;

namespace Everglow.Myth.Common;

public class MythContentGlobalNPC : GlobalNPC
{
	public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
	{
		return true;
	}
	public override void UpdateLifeRegen(NPC npc, ref int damage)
	{
		if (npc.HasBuff(ModContent.BuffType<Freeze2>()) && !npc.HasBuff(ModContent.BuffType<Freeze>()))
		{
			if (4f * npc.width * npc.height / 10300f * npc.scale > 1.5f)
			{
				for (int i = 0; i < 20; i++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Water, 0, 0, 0, default, 3f);
				}
			}
			else
			{
				for (int i = 0; i < 20; i++)
				{
					Dust.NewDust(npc.position, npc.width, npc.height, DustID.Water, 0, 0, 0, default, 2f);
				}
			}
		}
	}
	public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{

	}
	public override bool PreAI(NPC npc)
	{
		if (npc.HasBuff(ModContent.BuffType<Freeze>()))
			return false;
		return true;
	}
}