namespace Everglow.Yggdrasil.YggdrasilTown.Dusts;

public class JellyBallGel : ModDust
{
	public override bool Update(Dust dust)
	{
		return base.Update(dust);
	}
	public override Color? GetAlpha(Dust dust, Color lightColor)
	{
		return lightColor * 0.5f;
	}
}