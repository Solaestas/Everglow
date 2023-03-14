using Everglow.Sources.Modules.MythModule.MiscItems.Buffs;

namespace Everglow.Sources.Modules.MythModule.Common
{
	public class MythContentGlobalNPC : GlobalNPC
	{
		public override bool AppliesToEntity(NPC npc, bool lateInstatiation)
		{
			return true;
		}
		public override bool StrikeNPC(NPC npc, ref double damage, int defense, ref float knockback, int hitDirection, ref bool crit)
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
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Water, 0, 0, 0, default(Color), 3f);
					}
				}
				else
				{
					for (int i = 0; i < 20; i++)
					{
						Dust.NewDust(npc.position, npc.width, npc.height, DustID.Water, 0, 0, 0, default(Color), 2f);
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
			{
				return false;
			}
			return true;
		}
	}
}
