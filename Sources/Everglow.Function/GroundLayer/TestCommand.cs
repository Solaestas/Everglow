namespace Everglow.Commons.GroundLayer;

internal class TestCommand : ModCommand
{
	public override string Command => "Layer";

	public override CommandType Type => CommandType.Chat;

	public override void Action(CommandCaller caller, string input, string[] args)
	{
		if (args.Length == 0 || args[0] == "clear")
		{
			GroundLayerManager.Instance.Clear();
		}
		else if (args[0] == "new")
		{
			for (int i = 0; i < 100; i++)
			{
				GroundLayerManager.Instance.AddLayer("Test" + i, ModAsset.Noise_melting, new(Main.LocalPlayer.Center + Main.rand.NextFloat(MathHelper.TwoPi).ToRotationVector2() * Main.rand.NextFloat(3200), Main.rand.NextFloat(-1600, 1600)));
			}
		}
	}
}