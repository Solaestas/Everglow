using Everglow.Commons.Utilities.BackgroundHelper;
using Everglow.Commons.Vertex;

namespace Everglow.Example.BgSlides;

public class ExampleUnderGroundBg : BgSlide
{
	public int TimeLeft = 600;

	public List<Point> BgTiles = new List<Point>();

	public override void SetDefaults()
	{
		base.SetDefaults();
		UniqueName = "ExampleUnderGroundBg" + GetHashCode();
		Texture = Terraria.GameContent.TextureAssets.Background[114].Value;
		Distance = 5f;
		UseColorStyle = 1;
		Shader = XWrap_YWrap_Shader;
	}

	public override void Update()
	{
		base.Update();
		TimeLeft--;
	}

	public override void Draw()
	{
		List<Vertex2D> bars = new List<Vertex2D>();
		Add_TileBgVertice(this, BgTiles, bars);
		DrawVertexBackground(this, PrimitiveType.TriangleList, bars);
	}

	public override bool CanActive()
	{
		return TimeLeft > 0;
	}
}