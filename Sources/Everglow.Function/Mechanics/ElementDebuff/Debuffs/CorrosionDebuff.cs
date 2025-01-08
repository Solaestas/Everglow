namespace Everglow.Commons.Mechanics.ElementDebuff.Debuffs;

public class CorrosionDebuff : ElementDebuff
{
	public CorrosionDebuff()
		: base(ElementDebuffType.Corrosion, ModAsset.StarSlash, Color.Purple)
	{
		BuildUpMax = 1000;
		DurationMax = 60;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override void Draw(SpriteBatch spriteBatch) => throw new NotImplementedException();
}