using Everglow.Commons.VFX.CommonVFXDusts;
using ReLogic.Content;

namespace Everglow.Commons.Mechanics.ElementalDebuff.Debuffs;

public class NecrosisDebuff : ElementalDebuff
{
	public NecrosisDebuff()
		: base(ElementalDebuffType.Necrosis)
	{
		BuildUpMax = 1000;
		DurationMax = 1200;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override void OnProc(NPC npc)
	{
		for (int i = 0; i < 20; i++)
		{
			Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 8f)).RotatedByRandom(MathHelper.TwoPi);
			var somg = new NecrosisSmogDust
			{
				velocity = newVelocity,
				Active = true,
				Visible = true,
				position = npc.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
				maxTime = Main.rand.Next(30, 45),
				scale = Main.rand.NextFloat(70f, 120f),
				rotation = Main.rand.NextFloat(6.283f),
				ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), 0 },
			};
			Ins.VFXManager.Add(somg);
		}
		base.OnProc(npc);
	}

	public override void UpdateProc(NPC npc)
	{
		base.UpdateProc(npc);
	}

	public override Asset<Texture2D> Texture => ModAsset.Necrosis;

	public override Color Color => new Color(0.1f, 0.1f, 0.1f, 1f);
}