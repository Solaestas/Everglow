namespace Everglow.Sources.Modules.MythModule.MiscItems.Projectiles.Typeless.FixCoins
{
	public class FixCoinDefense5 : FixCoinProjectile
	{
		public override string HeatMapTexture()
		{
			return "heatmapGoldYellow";
		}
		public override int PrefixID()
		{
			return 0;
		}
		public override int Level()
		{
			return 5;
		}
	}
}