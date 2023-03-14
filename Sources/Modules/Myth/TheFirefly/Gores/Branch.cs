using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Gores
{
	public class Branch : ModGore
	{
		public override void OnSpawn(Gore gore, IEntitySource source)
		{
			base.OnSpawn(gore, source);
		}

		public override Color? GetAlpha(Gore gore, Color lightColor)
		{
			return base.GetAlpha(gore, lightColor);
		}
	}
}