using Everglow.Commons.VFX.Scene;
using SubworldLibrary;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.DeathJadeLake;

[Pipeline(typeof(WCSPipeline))]
public class GiantGhostClawBarnacle_Background : BackgroundVFX
{
	public override void Update()
	{
		if (!SubworldSystem.IsActive<YggdrasilWorld>())
		{
			Active = false;
		}
		base.Update();
	}

	public override void OnSpawn()
	{
		texture = ModAsset.GiantGhostClawBarnacle_Background.Value;
	}

	public override void Draw()
	{
		var frame = new Rectangle(0, 0, 304, 368);
		int widthSteps = frame.Width / 16; // 304 / 16 = 19.
		int heightSteps = frame.Height / 16; // 368 / 16 = 23;

		var bars = new List<Vertex2D>();
		for (int i = 0; i < widthSteps; i++)
		{
			for (int j = 0; j < heightSteps; j++)
			{
				AddBars(i, j, bars);
				AddBars(i + 1, j, bars);
				AddBars(i, j + 1, bars);

				AddBars(i, j + 1, bars);
				AddBars(i + 1, j, bars);
				AddBars(i + 1, j + 1, bars);
			}
		}
		if(bars.Count > 0)
		{
			Ins.Batch.Draw(texture, bars, PrimitiveType.TriangleList);
		}
	}

	public void AddBars(int i, int j, List<Vertex2D> bars)
	{
		var frame = new Rectangle(0, 0, 304, 368);
		bars.Add(position + new Vector2(i, j) * 16, Lighting.GetColor(originTile + new Point(i, j)), new Vector3(i * 16 / (float)frame.Width, j * 16 / (float)frame.Height, 0));
	}
}