namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts
{
    public class Vertebral : ModDust
	{
		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.position += Main.player[dust.color.R].velocity;
			dust.scale *= 0.9f;
			dust.rotation += 0.7f;
			if (dust.scale < 0.02f)
			{
				dust.active = false;
			}
			dust.velocity *= 0.95f;
			dust.rotation += 0.9f;
			return false;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			return lightColor;
		}
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, Main.rand.Next(3) * 10, 10, 10);
			base.OnSpawn(dust);
		}
	}
}