namespace Everglow.Myth.LanternMoon.Gores;
[Pipeline(typeof(DissolveAndNoDissolvePipeline))]
public class LanternGhostKingGore6 : DissolveGore
{
	public override void OnSpawn()
	{
		texture = ModAsset.LanternGhostKingGore6S.Value;
		DissolveAnimationTexture = ModAsset.LanternGhostKingGore6G.Value;
		NoDissolvePartTexture = null;
		base.OnSpawn();
	}
	public override void Update()
	{
		base.Update();

		float alpha2 = (timer - 100) / (maxTime - 100f);
		alpha2 = Math.Clamp(alpha2, 0.0f, 1.0f);
		alpha2 = MathF.Sin(alpha2 * MathHelper.Pi);
		Lighting.AddLight(position, new Vector3(1f, 0.5f, 0) * alpha2 * width / 60f);
	}
}
