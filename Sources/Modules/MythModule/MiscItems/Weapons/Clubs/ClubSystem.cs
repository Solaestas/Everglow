using Everglow.Sources.Commons.Core.ModuleSystem;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Clubs
{
	public class ClubSystem : ModSystem
	{
		public Effect ClubVague = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague").Value;
		public Effect ClubVague2 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2").Value;
		public Effect ClubVagueF = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVagueF").Value;
		public Effect ClubVague2F = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2F").Value;
		public override void SetStaticDefaults()
		{
			var ClubVague = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague").Value;
			var ClubVague2 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2").Value;
			var ClubVagueF = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVagueF").Value;
			var ClubVague2F = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2F").Value;
			Filters.Scene["ClubVague"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["ClubVague"].Load();
			Filters.Scene["ClubVague2"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["ClubVague2"].Load();
			Filters.Scene["ClubVagueF"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVagueF").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["ClubVagueF"].Load();
			Filters.Scene["ClubVague2F"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2F").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["ClubVague2F"].Load();
		}
		public static void LoadClubVagues()
		{
			var ClubVague = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague").Value;
			var ClubVague2 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2").Value;
			Filters.Scene["ClubVague"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["ClubVague2"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2").Value), "Test"), EffectPriority.Medium);
		}
		public static void LoadClubVaguesF()
		{
			var ClubVague = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVagueF").Value;
			var ClubVague2 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2F").Value;
			Filters.Scene["ClubVagueF"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVagueF").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["ClubVague2F"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/ClubVague2F").Value), "Test"), EffectPriority.Medium);
		}
	}
}