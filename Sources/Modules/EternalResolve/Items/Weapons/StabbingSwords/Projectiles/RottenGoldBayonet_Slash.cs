using Everglow.Commons.Vertex;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class RottenGoldBayonet_Slash : ModProjectile
    {
		public override string Texture => "Everglow/Commons/Weapons/StabbingSwords/StabbingProjectile";
		private Vector2 StartCenter;
		public override void OnSpawn(IEntitySource source)
		{
			StartCenter = Projectile.Center;
		}
		public override void SetDefaults()
        {
			Projectile.width = 30;
			Projectile.height = 30;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 60;
			Projectile.timeLeft = 120;
			Projectile.extraUpdates = 3;
		}
		public override void AI()
		{
			Projectile.velocity *= 0.93f;
			if(Main.rand.NextBool(7) && Projectile.timeLeft < 90)
			{
				Vector2 vel = Projectile.velocity.RotatedBy(Main.rand.NextFloat(-0.01f, 0.01f)) * Main.rand.NextFloat(14f, 28f);
				Dust dust = Dust.NewDustDirect(Projectile.position - Vector2.Normalize(Projectile.velocity) * Main.rand.NextFloat(0f, (120 - Projectile.timeLeft) * 3f), Projectile.width, Projectile.height, ModContent.DustType<CorruptShine_withoutPlayer>(), 0, 0, 0, default, Main.rand.NextFloat(0.95f, 1.7f));
				dust.velocity = vel;
			}
			if(Projectile.timeLeft == 114)
			{
				SoundEngine.PlaySound(new SoundStyle("Everglow/EternalResolve/Sounds/Slash").WithVolumeScale(0.5f), Projectile.Center);
			}
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 hitCenter = StartCenter + Vector2.Normalize(Projectile.velocity) * 120f;
			lightColor = Lighting.GetColor((int)(hitCenter.X / 16f), (int)(hitCenter.Y / 16f));
			float value0 = (120 - Projectile.timeLeft) / 120f;
			float value1 = MathF.Pow(value0, 0.5f);
			float width = (1 - MathF.Cos(value1 * 2f * MathF.PI)) * 10f;
			Vector2 normalizedVelocity = Utils.SafeNormalize(Projectile.velocity, Vector2.zeroVector);
			Vector2 normalize = normalizedVelocity.RotatedBy(Math.PI / 2d) * width;
			Color shadow = Color.White * 0.7f;
			List<Vertex2D> bars = new List<Vertex2D>
			{
				new Vertex2D(StartCenter + normalize - Main.screenPosition, shadow, new Vector3(0,0,0)),
				new Vertex2D(StartCenter - normalize - Main.screenPosition, shadow, new Vector3(0,1,0)),
				new Vertex2D(Projectile.Center + normalize - Main.screenPosition, shadow, new Vector3(1,0,0)),
				new Vertex2D(Projectile.Center - normalize - Main.screenPosition,shadow, new Vector3(1,1,0))
			};
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.StabbingProjectileShade.Value;
			if (bars.Count > 3)
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			Color light = new Color(0.45f * lightColor.R / 255f, 0.45f * lightColor.G / 255f, 1f * lightColor.B / 255f, 0);
			light *= width / 10f;
			bars = new List<Vertex2D>
			{
				new Vertex2D(StartCenter + normalize - Main.screenPosition, light, new Vector3(0,0,0)),
				new Vertex2D(StartCenter - normalize - Main.screenPosition, light, new Vector3(0,1,0)),
				new Vertex2D(Projectile.Center + normalize - Main.screenPosition, light, new Vector3(1,0,0)),
				new Vertex2D(Projectile.Center - normalize - Main.screenPosition, light, new Vector3(1,1,0))
			};
			Main.graphics.GraphicsDevice.Textures[0] = Commons.ModAsset.StabbingProjectile.Value;
			if (bars.Count > 3)
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			return false;
		}
	}
}