using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Function.FeatureFlags;
using MonoMod.Cil;

namespace Everglow.Sources.Modules.AssetReplaceModule
{
	public class ItemTooltipDrawModule : IModule
	{
		public string Name => "Tooltip Drawing Modify";

		public void Load() {
            IL.Terraria.Main.MouseText_DrawItemTooltip += PatchTooltipDrawing;
		}

        private void PatchTooltipDrawing(ILContext il) {
			var c = new ILCursor(il);
			if (!c.TryGotoNext(MoveType.After, i => i.MatchLdsfld<Main>(nameof(Main.SettingsEnabled_OpaqueBoxBehindTooltips))))
				return;
            c.EmitDelegate<Func<bool, bool>>((returnValue) => ModContent.GetInstance<EverglowClientConfig>().TextureReplace == TextureReplaceMode.Terraria && returnValue);
		}

		public void Unload() {

		}
	}
}
