using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.Gores
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