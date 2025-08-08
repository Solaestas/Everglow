namespace Everglow.Commons.Mechanics.Cooldown.Tests;

public class TestCooldown : CooldownBase
{
	public static new string ID => "TestCooldown";

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.Wave.Value;
}

public class TestCooldown2 : CooldownBase
{
	public static new string ID => "TestCooldown2";

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.Noise_cell_rgb.Value;
}

public class TestCooldown3 : CooldownBase
{
	public static new string ID => "TestCooldown3";

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.Noise_burn.Value;
}

public class TestCooldown4 : CooldownBase
{
	public static new string ID => "TestCooldown4";

	public override string TypeID => ID;

	public override Texture2D Texture => ModAsset.AlienWriting.Value;
}