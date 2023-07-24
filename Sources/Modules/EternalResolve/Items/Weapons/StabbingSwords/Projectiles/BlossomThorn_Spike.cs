using Everglow.Commons.Vertex;
using Everglow.EternalResolve.Items.Weapons.StabbingSwords.Dusts;
using Terraria.Audio;
using Terraria.DataStructures;

namespace Everglow.EternalResolve.Items.Weapons.StabbingSwords.Projectiles
{
    public class BlossomThorn_Spike : ModProjectile
    {
		public override string Texture => "Everglow/Commons/Weapons/StabbingSwords/StabbingProjectile";
		private Vector2 StartCenter;
		public override void OnSpawn(IEntitySource source)
		{
			StartCenter = Projectile.Center;
		}
		public override void SetDefaults()
        {
			Projectile.width = 3;
			Projectile.height = 3;
			Projectile.aiStyle = -1;
			Projectile.friendly = true;
			Projectile.tileCollide = false;
			Projectile.DamageType = DamageClass.Melee;
			Projectile.penetrate = -1;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.timeLeft = 20;
			Projectile.extraUpdates = 2;
		}
		public override void AI()
		{
			Projectile.velocity *= 0.93f;
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Vector2 hitCenter = StartCenter + Vector2.Normalize(Projectile.velocity) * 20f;
			lightColor = Lighting.GetColor((int)(hitCenter.X / 16f), (int)(hitCenter.Y / 16f));
			float value0 = (20 - Projectile.timeLeft) / 20f;
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
			Color light = new Color(0.3f * lightColor.R / 255f, 0.3f * lightColor.G / 255f, 0.24f * lightColor.B / 255f, 0);
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