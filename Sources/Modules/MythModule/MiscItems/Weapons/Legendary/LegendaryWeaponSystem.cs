using Everglow.Sources.Commons.Core.ModuleSystem;
using Terraria.GameContent.Shaders;
using Terraria.Graphics.Shaders;
using Terraria.Graphics.Effects;

namespace Everglow.Sources.Modules.MythModule.MiscItems.Weapons.Legendary
{
	public class LegendaryWeaponSystem : ModSystem
	{
		public Effect Geer3D = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/GearReflect").Value;
		public Effect GeerWave = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/GearWave").Value;
		public Effect GeerWave2 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/GearWave2").Value;
		public Effect FadeCurseGreen = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeCurseGreen").Value;
		public Effect FadeLaserRed = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeLaserRed").Value;
		public Effect FadeRainBlue = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeRainBlue").Value;
		public Effect SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
		public Effect SleCra2 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value;
        public Effect SleCra3 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack3").Value;
        public Effect SleCra4 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack4").Value;
        public Effect SleCra5 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack5").Value;
        public Effect SleCra6 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack6").Value;
        public Effect SleCra7 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack7").Value;
        public Effect SleCra8 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack8").Value;
        public Effect SleCra9 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack9").Value;
		public Effect BlueF = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/BlueFlame").Value;
		public Effect BlueWave = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/BlueWave").Value;
		public override void SetStaticDefaults()
		{
			var Geer3D = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/GearReflect").Value;
			var GeerWave = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/GearWave").Value;
			var GeerWave2 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/GearWave2").Value;
			var FadeCurseGreen = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeCurseGreen").Value;
			var FadeLaserRed = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeLaserRed").Value;
			var FadeRainBlue = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeRainBlue").Value;
			var SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
			var SleCra2 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value;
			var SleCra3 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack3").Value;
			var SleCra4 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack4").Value;
			var SleCra5 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack5").Value;
			var SleCra6 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack6").Value;
			var SleCra7 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack7").Value;
			var SleCra8 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack8").Value;
			var SleCra9 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack9").Value;
			var BlueF = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/BlueFlame").Value;
			var BlueWave = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/BlueWave").Value;
			Filters.Scene["SLECR"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR"].Load();
			Filters.Scene["SLECR2"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra2), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR2"].Load();
			Filters.Scene["SLECR3"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra3), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR3"].Load();
			Filters.Scene["SLECR4"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra4), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR4"].Load();
			Filters.Scene["SLECR5"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra5), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR5"].Load();
			Filters.Scene["SLECR6"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra6), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR6"].Load();
			Filters.Scene["SLECR7"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra7), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR7"].Load();
			Filters.Scene["SLECR8"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra8), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR8"].Load();
			Filters.Scene["SLECR9"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra9), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR9"].Load();
		}
		public override void Load()
		{
			Filters.Scene["Everglow:SleCra"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra"].Load();
			SleCra2 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value;
			Filters.Scene["Everglow:SleCra2"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra2"].Load();
			SleCra3 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value;
			Filters.Scene["Everglow:SleCra3"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack3").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra3"].Load();
			SleCra4 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value;
			Filters.Scene["Everglow:SleCra4"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack4").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra4"].Load();
			SleCra5 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value;
			Filters.Scene["Everglow:SleCra5"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack5").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra5"].Load();
			SleCra6 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value;
			Filters.Scene["Everglow:SleCra6"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack6").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra6"].Load();
			SleCra7 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value;
			Filters.Scene["Everglow:SleCra7"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack7").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra7"].Load();
			SleCra8 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value;
			Filters.Scene["Everglow:SleCra8"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack8").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra8"].Load();
			SleCra9 = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack2").Value;
			Filters.Scene["Everglow:SleCra9"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack9").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra9"].Load();
			base.Load();
		}
	}
}