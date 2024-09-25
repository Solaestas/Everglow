namespace Everglow.Myth.TheFirefly.Gores;

[Pipeline(typeof(BluishGiantGentianGorePipeline))]
public class BluishGiantGentian_gore3 : BluishGiantGentianGore
{
	public override void OnSpawn()
	{
		texture = ModAsset.BluishGiantGentian_gore3.Value;
		DissolveAnimationTexture = Commons.ModAsset.Noise_flame_0.Value;
		base.OnSpawn();
	}
}