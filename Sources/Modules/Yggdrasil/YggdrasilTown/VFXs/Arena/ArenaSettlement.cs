namespace Everglow.Yggdrasil.YggdrasilTown.VFXs.Arena;

[Pipeline(typeof(ArenaSettlementPipeline))]
public class ArenaSettlement : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PreDrawFilter;

	public int Timer;

	public Texture2D Texture = ModAsset.SettlementIconsAtlas.Value;

	/// <summary>
	/// 0: Success;1: Fail;2: Tie
	/// </summary>
	public int State;

	public override void Update()
	{
		if(!YggdrasilTownCentralSystem.InArena_YggdrasilTown())
		{
			Active = false;
		}
		Timer++;
		base.Update();
	}

	public override void Draw()
	{
		float pocession = Timer / 120f;
		var screenCenter = new Vector2(Main.screenWidth, Main.screenHeight) * 0.5f;
		var drawColor = new Color(0.4f, 0f, 0, 0.5f);
		Vector2 successSize = new Vector2(612, 322);
		Vector2 failSize = new Vector2(301, 169) * 2;
		List<Vertex2D> bars = new List<Vertex2D>();
		bars.Add(screenCenter - successSize * 0.5f, drawColor, new Vector3(0, 28 / Texture.Height, pocession));
		bars.Add(screenCenter + new Vector2(successSize.X, -successSize.Y) * 0.5f, drawColor, new Vector3(successSize.X / Texture.Width, 0, pocession));
		bars.Add(screenCenter + new Vector2(-successSize.X, successSize.Y) * 0.5f, drawColor, new Vector3(0, (successSize.Y + 28) / Texture.Height, pocession));
		bars.Add(screenCenter + successSize * 0.5f, drawColor, new Vector3((successSize + new Vector2(0, 28)) / Texture.Size(), pocession));
		Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
		int score = YggdrasilTownCentralSystem.ArenaScore;
		Vector2 numberSize = new Vector2(52, 52);
		int offSetX = -6;
		if (score.ToString().Length == 1)
		{
			Vector2 numberTopLeft = new Vector2(2 + 54 * (score % 10), 792);
			int numberOffsetY = -40;
			Vector2 drawPos = screenCenter + new Vector2(0 + offSetX, numberOffsetY);
			bars = new List<Vertex2D>();
			bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
			bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
			Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
		}
		if (score.ToString().Length == 2)
		{
			int firstNumber = score % 10;
			int secondNumber = ((score - firstNumber) % 100) / 10;
			Vector2 numberTopLeft = new Vector2(2 + 54 * firstNumber, 792);
			int numberOffsetY = -40;
			Vector2 drawPos = screenCenter + new Vector2(27 + offSetX, numberOffsetY);
			bars = new List<Vertex2D>();
			bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
			bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
			Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);

			numberTopLeft = new Vector2(2 + 54 * secondNumber, 792);
			drawPos = screenCenter + new Vector2(-27 + offSetX, numberOffsetY);
			bars = new List<Vertex2D>();
			bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
			bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
			Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
		}
		if (score.ToString().Length == 3)
		{
			int firstNumber = score % 10;
			int secondNumber = ((score - firstNumber) % 100) / 10;
			int thirdNumber = (score - secondNumber * 10 - firstNumber) / 100;
			Vector2 numberTopLeft = new Vector2(2 + 54 * firstNumber, 792);
			int numberOffsetY = -40;
			Vector2 drawPos = screenCenter + new Vector2(54 + offSetX, numberOffsetY);
			bars = new List<Vertex2D>();
			bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
			bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
			Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);

			numberTopLeft = new Vector2(2 + 54 * secondNumber, 792);
			drawPos = screenCenter + new Vector2(0 + offSetX, numberOffsetY);
			bars = new List<Vertex2D>();
			bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
			bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
			Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);

			numberTopLeft = new Vector2(2 + 54 * thirdNumber, 792);
			drawPos = screenCenter + new Vector2(-54 + offSetX, numberOffsetY);
			bars = new List<Vertex2D>();
			bars.Add(drawPos - numberSize * 0.5f, drawColor, new Vector3(numberTopLeft / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(numberSize.X, -numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(numberSize.X, 0)) / Texture.Size(), pocession));
			bars.Add(drawPos + new Vector2(-numberSize.X, numberSize.Y) * 0.5f, drawColor, new Vector3((numberTopLeft + new Vector2(0, numberSize.Y)) / Texture.Size(), pocession));
			bars.Add(drawPos + numberSize * 0.5f, drawColor, new Vector3((numberTopLeft + numberSize) / Texture.Size(), pocession));
			Ins.Batch.Draw(bars, PrimitiveType.TriangleStrip);
		}
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