namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles.Magic;

public class EyeOfAnabiosis_MagicCircleText
{
	public const float GenerateRange = 26f;

	public EyeOfAnabiosis_MagicCircleText()
	{
		RelativePosition = new Vector2(Main.rand.NextFloat(-GenerateRange, GenerateRange), 0);
		VelocityY = new Vector2(0, Main.rand.NextFloat(-0.4f, -0.45f));

		var CutHeight = Main.rand.Next(1, 4); // [2, 4]
		var CutOffsetX = Main.rand.Next(0, 19); // [0, 18]
		var CutOffsetY = Main.rand.Next(0, 9 - CutHeight); // [0, 8 - h]
		RandomCutOff = (CutHeight, CutOffsetX, CutOffsetY);
	}

	public Vector2 RelativePosition { get; private set; }

	public Vector2 VelocityY { get; init; }

	private (int CutHeight, int CutOffsetX, int CutOffsetY) RandomCutOff { get; init; }

	public Rectangle SourceRectangle
	{
		get
		{
			var texture = Commons.ModAsset.AlienWriting.Value;
			var frameWidth = texture.Width / 19;
			var frameHeight = texture.Height / 8;
			return new Rectangle(RandomCutOff.CutOffsetX * frameWidth, RandomCutOff.CutOffsetY * frameHeight, frameWidth, RandomCutOff.CutHeight * frameHeight);
		}
	}

	public Color Color => new Color(11, 200, 230, 0);

	public Vector2 Scale => new Vector2(MathF.Sqrt(1 - MathF.Pow(RelativePosition.X / GenerateRange, 2)), 1) * 0.08f;

	public Vector2 Origin => new Vector2(SourceRectangle.Width / 2, SourceRectangle.Height);

	public void Update()
	{
		RelativePosition = RelativePosition + VelocityY;
	}
}