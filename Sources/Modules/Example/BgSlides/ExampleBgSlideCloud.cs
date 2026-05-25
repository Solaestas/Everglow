using Everglow.Commons.Utilities.BackgroundHelper;

namespace Everglow.Example.BgSlides;

public class ExampleBgSlideCloud : BgSlide
{
	public int TimeLeft = 600;

	public override void SetDefaults()
	{
		base.SetDefaults();
		UniqueName = "ExampleBgSlideCloud" + GetHashCode();
		Texture = Terraria.GameContent.TextureAssets.Cloud[Main.rand.Next(Terraria.GameContent.TextureAssets.Cloud.Length)].Value;
		Distance = Main.rand.NextFloat(10f, 120f);
		Scale = 40f;
	}

	public override void FadeIn()
	{
		if (Alpha < 0.5f)
		{
			Alpha += 0.01f;
		}
		else
		{
			Alpha = 0.5f;
		}
	}

	public override void Update()
	{
		base.Update();
		TimeLeft--;
		WorldAnchor += new Vector2(50f / Distance, 0f);
	}

	public override void Draw()
	{
		DrawPreset_Piece(this);
	}

	public override bool CanActive()
	{
		return TimeLeft > 0;
	}
}