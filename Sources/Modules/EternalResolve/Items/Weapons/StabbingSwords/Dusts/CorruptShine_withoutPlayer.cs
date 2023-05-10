namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts
{
    public class CorruptShine_withoutPlayer : ModDust
	{
		public override string Texture => "Everglow/EternalResolve/Items/Weapons/StabbingSwords/Dusts/CorruptShine";
		public override bool Update(Dust dust)
		{
			dust.position += dust.velocity;
			dust.scale *= 0.9f;
			if(dust.scale < 0.02f)
			{
				dust.active = false;
			}
			dust.velocity *= 0.95f;
			dust.rotation += 0.4f;
			return false;
		}
		public override Color? GetAlpha(Dust dust, Color lightColor)
		{
			Color c0 = lightColor;
			c0.A = dust.color.A;
			return c0;
		}
		public override void OnSpawn(Dust dust)
		{
			dust.frame = new Rectangle(0, Main.rand.Next(3) * 9, 8, 9);
			dust.color.A = (byte)Main.rand.Next(70, 140);
			base.OnSpawn(dust);
		}
	}
}