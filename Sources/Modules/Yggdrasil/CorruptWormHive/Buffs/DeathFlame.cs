using Everglow.Yggdrasil.CorruptWormHive.VFXs;

namespace Everglow.Yggdrasil.CorruptWormHive.Buffs;

public class DeathFlame : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.debuff[Type] = true;
		Main.buffNoSave[Type] = true;
	}
	public override void Update(NPC npc, ref int buffIndex)
	{
		int freq = (int)(npc.velocity.Length() / 2f + 1);
		for (int d = 0; d < freq; d++)
		{
			if (Main.rand.NextBool(6))
			{
				var df = new DevilFlameDust
				{
					velocity = new Vector2(0, Main.rand.NextFloat(1.5f, 4f)).RotatedByRandom(6.283) * 1 + npc.velocity * 0.2f,
					Active = true,
					Visible = true,
					position = npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)),
					maxTime = Main.rand.Next(9, 30),
					ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.02f, 0.02f), Main.rand.NextFloat(8f, 12f) }
				};
				Ins.VFXManager.Add(df);
			}
		}
		int BuffDamage = (int)((1 - npc.life / (float)npc.lifeMax) * 400 + npc.defense + 40);
		npc.lifeRegenExpectedLossPerSecond = BuffDamage / 24;
		npc.lifeRegen = -BuffDamage;
		if (npc.life + npc.lifeRegen / 40 <= 0 && npc.active)
		{
			for (int d = 0; d < 40; d++)
			{
				var df = new DevilFlameDust
				{
					velocity = new Vector2(0, Main.rand.NextFloat(4.5f, 9f)).RotatedByRandom(6.283) * 1 + npc.velocity * 0.02f,
					Active = true,
					Visible = true,
					position = npc.position + new Vector2(Main.rand.Next(npc.width), Main.rand.Next(npc.height)),
					maxTime = Main.rand.Next(9, 48),
					ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.02f, 0.02f), Main.rand.NextFloat(8f, 24f) }
				};
				Ins.VFXManager.Add(df);
			}
		}
	}
}
