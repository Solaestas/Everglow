using Everglow.EternalResolve.VFXs;
using SteelSeries.GameSense;

namespace Everglow.EternalResolve.Buffs;

public class OnElectric : ModBuff
{
	public override void SetStaticDefaults()
	{
		Main.buffNoSave[Type] = true;
		Main.buffNoTimeDisplay[Type] = true;
	}

	public override void Update(NPC npc, ref int buffIndex)
	{
		int buffDamage = 35;
		if (npc.wet)
		{
			buffDamage *= 3;
		}
		if (npc.buffTime[buffIndex] <= 20)
		{
			buffDamage = 0;
		}
		npc.lifeRegen = -buffDamage;
		npc.lifeRegenExpectedLossPerSecond = 7;
		if (npc.wet)
		{
			npc.lifeRegenExpectedLossPerSecond *= 3;
		}
		if (Main.rand.NextBool(3))
		{
			Dust dust = Dust.NewDustDirect(npc.position, npc.width, npc.height, DustID.Electric);
			dust.velocity = new Vector2(0, Main.rand.NextFloat(5f)).RotatedByRandom(6.283);
			dust.scale = Main.rand.NextFloat(0.55f, 0.85f);
			dust.noGravity = true;
		}

		float mulWidth = 1f;
		Vector2 newVelocity = npc.velocity * 0.3f + new Vector2(Main.rand.NextFloat(6f, 8f), 0).RotatedByRandom(6.283);
		var yoenLeZedElecticFlowDust = new YoenLeZedElecticFlowDust_split_withoutPlayer
		{
			velocity = newVelocity,
			Active = true,
			Visible = true,
			position = npc.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + npc.velocity * Main.rand.NextFloat(-2.0f, -1.2f) - newVelocity * 2f,
			maxTime = Main.rand.Next(6, 11),
			ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.08f, 0.08f), Main.rand.NextFloat(26.6f, 28f) * mulWidth}
		};
		Ins.VFXManager.Add(yoenLeZedElecticFlowDust);
		base.Update(npc, ref buffIndex);
	}
}
