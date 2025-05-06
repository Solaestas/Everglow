using System;

namespace Everglow.Yggdrasil.YggdrasilTown.VFXs.Arena;

[Pipeline(typeof(ArenaMessageBackgroundPipeline))]
public class SuccessIconBackground : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PreDrawFilter;

	public int Timer;

	public override void Update()
	{
		Timer++;
		if (!YggdrasilTownCentralSystem.InArena_YggdrasilTown())
		{
			Active = false;
		}
		base.Update();
	}

	public override void Draw()
	{
		float pocession = Timer / 120f;
		if (pocession > 1)
		{
			pocession = 1;
		}
		var drawColor = new Color(0f, 0.25f, 0, 0f);
		float timeValue = (float)(Main.time * 0.002);
		List<Vertex2D> bars = new List<Vertex2D>();
		bars.Add(new Vector2(0), drawColor, new Vector3(0, 0, pocession));
		bars.Add(new Vector2(Main.screenWidth, 0), drawColor, new Vector3(1, 0, pocession));
		bars.Add(new Vector2(0, Main.screenHeight), drawColor, new Vector3(0, 1, pocession));
		bars.Add(new Vector2(Main.screenWidth, Main.screenHeight), drawColor, new Vector3(1, 1, pocession));
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
	}

	public bool MouseInArea(Vector2 drawPos, Vector2 size)
	{
		Vector2 mousePos = Main.MouseWorld;
		if (mousePos.X >= drawPos.X && mousePos.X <= drawPos.X + size.X)
		{
			if (mousePos.Y >= drawPos.Y && mousePos.Y <= drawPos.Y + size.Y)
			{
				return true;
			}
		}
		return false;
	}
}