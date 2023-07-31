namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts
{
    public class NightDust : ModDust
	{
		public override bool Update(Dust dust)
		{
			return true;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			Color c0 = Color.White * 0.5f;
			c0.A = 150;
			if (dust.frame.Y == 0)
			{
				c0.A = 40;
			}
			return c0;
		}
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, Main.rand.Next(3) * 10, 10, 10);
			base.OnSpawn(dust);
		}
	}
}