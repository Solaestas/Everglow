using Everglow.Commons.Utilities.BackgroundHelper;
using Everglow.Yggdrasil.KelpCurtain.Biomes;

namespace Everglow.Yggdrasil.KelpCurtain.Background;

public class IsleOfBloom_Underground_sky : BackgroundSlideBase
{
	public override void SetDefaults()
	{
		base.SetDefaults();
		Texture = ModAsset.IsleOfBloom_Underground_sky.Value;
		Distance = 2.8f;
		UseColorStyle = 2;
		Shader = Effects.XWrap_YClamp_Shader;
	}

	public override void Update()
	{
		base.Update();
	}

	public override void Draw()
	{
		Main.spriteBatch.Draw(Texture, WorldAnchor - Main.screenPosition, null, Color.White, 0, Texture.Size() * 0.5f, new Vector2(4, 1), SpriteEffects.None, 0);
	}

	public override bool CanActive()
	{
		return Main.LocalPlayer.InModBiome<DeathJadeLakeBiome>();
	}
}