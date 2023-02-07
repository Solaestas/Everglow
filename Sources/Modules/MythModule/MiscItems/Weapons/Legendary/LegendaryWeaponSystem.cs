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

		public Effect FadeRainBlue = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeRainBlue").Value;
		public Effect SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;

		public Effect BlueWave = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/BlueWave").Value;
		public override void SetStaticDefaults()
		{
			var Geer3D = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/GearReflect").Value;
			var GeerWave = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/GearWave").Value;
			var FadeRainBlue = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/FadeRainBlue").Value;
			var SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;

			var BlueWave = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/BlueWave").Value;
			Filters.Scene["SLECR"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR"].Load();
			Filters.Scene["SLECR2"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR2"].Load();
			Filters.Scene["SLECR3"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR3"].Load();
			Filters.Scene["SLECR4"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR4"].Load();
			Filters.Scene["SLECR5"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR5"].Load();
			Filters.Scene["SLECR6"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR6"].Load();
			Filters.Scene["SLECR7"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR7"].Load();
			Filters.Scene["SLECR8"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR8"].Load();
			Filters.Scene["SLECR9"] = new Filter(new ScreenShaderData(new Ref<Effect>(SleCra), "Test"), EffectPriority.Medium);
			Filters.Scene["SLECR9"].Load();
		}
		public override void Load()
		{
			Filters.Scene["Everglow:SleCra"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra"].Load();
			SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
			Filters.Scene["Everglow:SleCra2"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra2"].Load();
			SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
			Filters.Scene["Everglow:SleCra3"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra3"].Load();
			SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
			Filters.Scene["Everglow:SleCra4"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra4"].Load();
			SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
			Filters.Scene["Everglow:SleCra5"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra5"].Load();
			SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
			Filters.Scene["Everglow:SleCra6"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra6"].Load();
			SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
			Filters.Scene["Everglow:SleCra7"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra7"].Load();
			SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
			Filters.Scene["Everglow:SleCra8"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra8"].Load();
			SleCra = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value;
			Filters.Scene["Everglow:SleCra9"] = new Filter(new ScreenShaderData(new Ref<Effect>(ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/SilenceCrack").Value), "Test"), EffectPriority.Medium);
			Filters.Scene["Everglow:SleCra9"].Load();
			base.Load();
		}
	}
}