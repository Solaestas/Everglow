using Everglow.Commons.Utilities;
using Everglow.Commons.VFX.CommonVFXDusts;
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
		npc.SetLifeRegenExpectedLossPerSecond(7);
		if (npc.wet)
		{
			npc.SetLifeRegenExpectedLossPerSecond(7 * 3);
		}
		if (Main.rand.NextBool(3))
		{
			Dust d = Dust.NewDustDirect(npc.Center, 0, 0, ModContent.DustType<ElectricMiddleDust>(), 0, 0);
			d.scale = Main.rand.NextFloat(0.85f, 1.15f) * 0.04f;
		}

		float mulVelocity = 1f;
		float size = Main.rand.NextFloat(8f, Main.rand.NextFloat(4f, 10f));
		Vector2 afterVelocity = new Vector2(0, size * 1.3f).RotatedByRandom(MathHelper.TwoPi);
		var electric = new ElectricCurrent
		{
			velocity = afterVelocity * mulVelocity,
			Active = true,
			Visible = true,
			position = npc.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) - afterVelocity * mulVelocity * 2,
			maxTime = size * size / 8f,
			scale = size,
			ai = new float[] { Main.rand.NextFloat(0.0f, 0.6f), size, Main.rand.NextFloat(0.2f, Main.rand.NextFloat(0.2f, 0.4f)) }
		};
		Ins.VFXManager.Add(electric);

		base.Update(npc, ref buffIndex);
	}
}
