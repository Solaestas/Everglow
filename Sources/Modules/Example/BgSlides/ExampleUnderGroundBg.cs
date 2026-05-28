using Everglow.Commons.Utilities.BackgroundHelper;
using Everglow.Commons.Vertex;

namespace Everglow.Example.BgSlides;

public class ExampleUnderGroundBg : BackgroundSlideBase
{
	public int TimeLeft = 600;

	public List<Point> BgTiles = new List<Point>();

	public override void SetDefaults()
	{
		base.SetDefaults();
		Texture = Commons.ModAsset.NoiseWave.Value;
		Distance = 5f;
		UseColorStyle = 1;
		Shader = Effects.XWrap_YWrap_Shader;
	}

	public override void Update()
	{
		base.Update();
		TimeLeft--;
	}

	public override void Draw()
	{
		List<Vertex2D> bars = new List<Vertex2D>();
		TileVertexRenderer.Add_TileBgVertice_UnsafeLambda(this, BgTiles, bars);
		DrawVertexBackground(this, PrimitiveType.TriangleStrip, bars);
	}

	public override bool CanActive()
	{
		return TimeLeft > 0;
	}
}