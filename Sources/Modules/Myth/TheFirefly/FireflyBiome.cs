using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Backgrounds;
using Everglow.Myth.TheFirefly.Water;

namespace Everglow.Myth.TheFirefly;

public class FireflyBiomeBG : ModSurfaceBackgroundStyle
{
	public override void ModifyFarFades(float[] fades, float transitionSpeed)
	{
	}
}

public class FireflyBiome : ModBiome
{
	public override int Music => MythContent.QuickMusic("MothBiome");
	public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
	public override string BestiaryIcon => "Everglow/Myth/TheFirefly/FireflyIcon";
	public override string BackgroundPath => base.BackgroundPath;
	public override string MapBackground => "Everglow/Myth/TheFirefly/Backgrounds/FireflyBiomeInside_Background";
	public override ModWaterStyle WaterStyle => ModContent.GetInstance<FireflyWaterStyle>();
	public override Color? BackgroundColor => base.BackgroundColor;
	public override ModSurfaceBackgroundStyle SurfaceBackgroundStyle => ModContent.GetInstance<FireflyBiomeBG>();
	public override ModUndergroundBackgroundStyle UndergroundBackgroundStyle => ModContent.Find<ModUndergroundBackgroundStyle>("Everglow/MothUndergroundBackground");

	public override bool IsBiomeActive(Player player)
	{
		return MothBackground.BiomeActive();
	}
	public override void OnInBiome(Player player)
	{
		base.OnInBiome(player);
	}
}