namespace Everglow.Commons.Mechanics.ElementDebuff.Debuffs;

public class NecrosisDebuff : ElementDebuff
{
	public NecrosisDebuff()
		: base(ElementDebuffType.Necrosis, ModAsset.StarSlash, Color.Gray)
	{
		BuildUpMax = 1000;
		DurationMax = 60;
		DotDamage = 2;
		ProcDamage = 200;
	}

	public override void Draw(SpriteBatch spriteBatch) => throw new NotImplementedException();
}