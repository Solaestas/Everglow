using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Commons.Weapons.StabbingSwords;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class YoenLeZed_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			Color = new Color(100, 180, 255);
			base.SetDefaults();
			TradeShade = 0.2f;
			Shade = 0.2f;
			FadeTradeShade = 0.74f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.8f;
			MaxLength = 1.15f;
			DrawWidth = 0.4f;
		}
		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(Color.White);
		}
        public void GenerateVFX(int Frequency)
        {
            float mulVelocity = Main.rand.NextFloat(0.75f, 1.5f);
            for (int g = 0; g < Frequency; g++)
            {
                float size = Main.rand.NextFloat(8f, Main.rand.NextFloat(8f, 16f));
                Vector2 afterVelocity = Projectile.velocity;
                var electric = new ElectricCurrent
                {
                    velocity = afterVelocity * mulVelocity,
                    Active = true,
                    Visible = true,
                    position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283),
                    maxTime = size * size / 8f,
                    scale = size,
                    ai = new float[] { Main.rand.NextFloat(0.0f, 0.6f), size / 2, 0 }
                };
                Ins.VFXManager.Add(electric);
            }
        }
        public void SplitVFX(int Frequency)
        {
            float mulVelocity = 1f;
            for (int g = 0; g < Frequency; g++)
            {
                float size = Main.rand.NextFloat(8f, Main.rand.NextFloat(4f, 10f));
                Vector2 afterVelocity = Projectile.velocity*10;
                var electric = new ElectricCurrent
                {
                    velocity = afterVelocity * mulVelocity,
                    Active = true,
                    Visible = true,
                    position = Projectile.Center + new Vector2(Main.rand.NextFloat(-6f, 6f), 0).RotatedByRandom(6.283) + Projectile.velocity * MathF.Sqrt(Main.rand.NextFloat(1f)) * 10,
                    maxTime = size * size / 8f,
                    scale = size,
                    ai = new float[] { Main.rand.NextFloat(0.0f, 0.6f), size, Main.rand.NextFloat(0.2f, Main.rand.NextFloat(0.2f, 0.4f)) }
                };
                Ins.VFXManager.Add(electric);
            }
        }
        public override void AI()
		{
            //GenerateVFX(1);
            if(Main.rand.NextBool(2))
			SplitVFX(1);
			Lighting.AddLight(Projectile.Center,new Vector3(0.5f,0.8f,1f)*(ToKill/120f));
			base.AI();
		}
	}
}