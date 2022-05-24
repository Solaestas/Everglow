using Everglow.Sources.Modules.MythModule.TheFirefly.Backgrounds;
namespace Everglow.Sources.Modules.MythModule.TheFirefly
{
    public class FireflyBiome : ModBiome
    {
		public override int Music => Common.MythContent.QuickMusic("MothBiome");

		public override SceneEffectPriority Priority => SceneEffectPriority.Environment;
		public override string BestiaryIcon => "Everglow/Sources/Modules/MythModule/TheFirefly/FireflyIcon";
		public override string BackgroundPath => base.BackgroundPath;
		public override Color? BackgroundColor => base.BackgroundColor;
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Glowing Cocoon");
		}
		public override bool IsBiomeActive(Player player)
		{
			MothBackground mothBackground = ModContent.GetInstance<MothBackground>();
			return mothBackground.BiomeActive();
		}
	}
}

