using Everglow.Commons.Weapons;

namespace Everglow.Example.Test;

public class TrailProjectile : TrailingProjectile
{
	public override void SetDef()
	{
		TrailTexture = Commons.ModAsset.Trail_2.Value;
		base.SetDef();
	}
}