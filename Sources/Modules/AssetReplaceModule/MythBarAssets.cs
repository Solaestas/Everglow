using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.AssetReplaceModule
{
	public class MythBarAssets
	{
		public ClassicBar ClassicBar = new();
		public FancyBar FancyBar = new();
		public HorizontalBar HorizontalBar = new();

		public void LoadTextures() {
			ClassicBar.LoadTextures("UISkinMyth");
			FancyBar.LoadTextures("UISkinMyth");
			HorizontalBar.LoadTextures("UISkinMyth");
		}

		public void Apply() {
			ClassicBar.ReplaceTextures();
			FancyBar.ReplaceTextures();
			HorizontalBar.ReplaceTextures();
		}
	}
}
