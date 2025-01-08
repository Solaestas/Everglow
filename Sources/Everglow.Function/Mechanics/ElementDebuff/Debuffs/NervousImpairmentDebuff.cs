namespace Everglow.Commons.Mechanics.ElementDebuff.Debuffs;

public class NervousImpairmentDebuff : ElementDebuff
{
	public NervousImpairmentDebuff()
		: base(ElementDebuffType.NervousImpairment, ModAsset.StarSlash, Color.LightSeaGreen)
	{
		BuildUpMax = 1000;
		DurationMax = 60;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override void Draw(SpriteBatch spriteBatch) => throw new NotImplementedException();
}