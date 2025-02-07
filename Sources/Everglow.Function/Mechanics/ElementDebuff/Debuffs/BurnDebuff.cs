namespace Everglow.Commons.Mechanics.ElementDebuff.Debuffs;

public class BurnDebuff : ElementDebuff
{
	public BurnDebuff()
		: base(ElementDebuffType.Burn, ModAsset.StarSlash, Color.Orange)
	{
		BuildUpMax = 1000;
		DurationMax = 60;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override void Draw(SpriteBatch spriteBatch) => throw new NotImplementedException();

	public override void PostProc(NPC npc)
	{
		npc.AddBuff(BuffID.OnFire3, 1200);
	}
}