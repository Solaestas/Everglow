using Everglow.Commons.Weapons.StabbingSwords;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
	public class SwordfishBeak_Pro_Stab : StabbingProjectile_Stab
	{
		public override void SetDefaults()
		{
			Color = new Color(47, 72, 80);
			base.SetDefaults();
			TradeShade = 0.3f;
			Shade = 0.2f;
			FadeTradeShade = 0.64f;
			FadeScale = 1;
			TradeLightColorValue = 1f;
			FadeLightColorValue = 0.4f;
			MaxLength = 0.75f;
			DrawWidth = 0.4f;
		}
		public override void OnSpawn(IEntitySource source)
		{
			Player player = Main.player[Projectile.owner];
			if (player.wet)
			{
				MaxLength = 1.125f;
			}
			else
			{
				MaxLength = 0.75f;
			}
			base.OnSpawn(source);
		}
		public override void DrawEffect(Color lightColor)
		{
			base.DrawEffect(lightColor);
		}
		public override void AI()
		{
			if(Main.rand.NextBool(7))
			{
				Vector2 pos = Projectile.position + Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.4f, 8f);
				Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.4f, 0.4f)) * Main.rand.NextFloat(0.04f, 0.24f);
				if (Collision.CanHit(Projectile.Center - Projectile.velocity, 0, 0, pos + vel, 0, 0))
				{
					Dust dust = Dust.NewDustDirect(pos, Projectile.width, Projectile.height, ModContent.DustType<SeaWater>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.7f));
					dust.velocity = vel;
				}
			}
			base.AI();
		}
		public override void HitTile()
		{
			SoundEngine.PlaySound(SoundID.Dig.WithPitchOffset(Main.rand.NextFloat(0.6f, 1f)), Projectile.Center);
		}
	}
}