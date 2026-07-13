using Everglow.Commons.Utilities.BackgroundHelper;
using Everglow.Yggdrasil.KelpCurtain.Biomes;
using Everglow.Yggdrasil.YggdrasilTown.Biomes;

namespace Everglow.Yggdrasil.YggdrasilTown.Background;

public class YggdrasilTown_Construct : BackgroundSlideBase
{
	public List<Point> BgTiles = new List<Point>();

	public override void SetDefaults()
	{
		base.SetDefaults();
		Texture = ModAsset.YggdrasilTown_Construct.Value;
		Distance = 1;
		UseColorStyle = 1;
		LayerPriority = 0;
		Shader = Effects.XWrap_YWrap_Shader;
	}

	public override void Update()
	{
		base.Update();
	}

	public override void Draw()
	{
		var bars = new List<Vertex2D>();
		BackgroundHigherPerformanceHelper.Add_TileBgVertice(this, BgTiles, bars);
		DrawVertexBackground(this, PrimitiveType.TriangleStrip, bars);
	}

	public override bool CanActive()
	{
		return Main.LocalPlayer.InModBiome<YggdrasilTownBiome>();
	}
}