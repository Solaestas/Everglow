using Everglow.Commons.VFX.CommonVFXDusts;
using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Everglow.EternalResolve.VFXs;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class EnchantedBayonet_Pro_Stab : StabbingProjectile_Stab
	{
        public override void SetDefaults()
        {
			base.SetDefaults();
			Color = new Color(76, 126, 255);
			TradeShade = 0.8f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.5f;
			MaxLength = 0.88f;
			DrawWidth = 0.4f;
		}
		public override void HitTile()
		{
			SoundStyle ss = SoundID.NPCHit4;
			SoundEngine.PlaySound(ss.WithPitchOffset(Main.rand.NextFloat(-0.4f, 0.4f)), Projectile.Center);
			for (int g = 0; g < 20; g++)
			{
				Vector2 newVelocity = new Vector2(0, Main.rand.NextFloat(2f, 6f)).RotatedByRandom(MathHelper.TwoPi);
				var spark = new Spark_EnchantedStabDust
				{
					velocity = newVelocity,
					Active = true,
					Visible = true,
					position = EndPos,
					maxTime = Main.rand.Next(1, 25),
					scale = Main.rand.NextFloat(0.1f, Main.rand.NextFloat(0.1f, 17.0f)),
					rotation = Main.rand.NextFloat(6.283f),
					ai = new float[] { Main.rand.NextFloat(0.0f, 0.93f), Main.rand.NextFloat(-0.13f, 0.13f) }
				};
				Ins.VFXManager.Add(spark);
			}
		}
		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(lightColor);
		}
		public override void AI()
		{
			if (Main.rand.NextBool(6))
			{
				Vector2 end = Projectile.Center + Projectile.velocity * 80 * MaxLength;
				if (EndPos != Vector2.zeroVector)
				{
					end = EndPos;
				}
				int type = ModContent.DustType<EnchantedDustMoveWithPlayer>();
				Dust dust = Dust.NewDustDirect(Vector2.Lerp(StartCenter, end, Main.rand.NextFloat(0.3f, 1f)) - new Vector2(4), 0, 0, type, 0, 0, 0, default, Main.rand.NextFloat(0.45f, 1.1f));
				dust.noGravity= true;
				dust.velocity = new Vector2(0, Main.rand.NextFloat(6f)).RotateRandom(6.283);
			}
			if(ToKill == 40)
			{
				Projectile p0 = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(),
	Projectile.Center
	, Projectile.velocity * 9, ModContent.ProjectileType<EnchantedBayonet_beam>()
	, Projectile.damage, Projectile.knockBack * 0.6f, Projectile.owner, -1);
				Main.NewText(p0.velocity);
			}
			base.AI();
		}
		public override void OnSpawn(IEntitySource source)
		{
			
			base.OnSpawn(source);
		}
	}
}