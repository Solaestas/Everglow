using Everglow.Myth.Common;
using Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.MagnetSphere;
using Terraria.DataStructures;
using static Everglow.Myth.Common.MythUtils;

namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.MagnetSphere
{
	public class MagnetSphereHit : ModProjectile, IWarpProjectile
	{
		protected override bool CloneNewInstances => false;
		public override bool IsCloneable => false;

		public override void SetDefaults()
		{
			Projectile.width = 240;
			Projectile.height = 240;
			Projectile.friendly = true;
			Projectile.hostile = false;
			Projectile.aiStyle = -1;
			Projectile.penetrate = -1;
			Projectile.timeLeft = 200;
			Projectile.tileCollide = false;
			Projectile.extraUpdates = 4;
			Projectile.usesLocalNPCImmunity = true;
			Projectile.localNPCHitCooldown = 6;
			Projectile.DamageType = DamageClass.Magic;
		}
		public void GenerateVFXExpolode(int Frequency, float mulVelocity = 1f)
		{
			for (int g = 0; g < Frequency * 3; g++)
			{
				Vector2 vel = new Vector2(0, Main.rand.NextFloat(4.65f, 5.5f)).RotatedByRandom(6.283) * mulVelocity;
				var me = new MagneticElectricity
				{
					velocity = vel,
					Active = true,
					Visible = true,
					maxTime = Main.rand.Next(54, 180),
					ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.01f, 0.01f), Main.rand.NextFloat(1.6f, 2f) * mulVelocity },
					position = Projectile.Center - vel * 3
				};
				VFXManager.Add(me);
			}
			for (int g = 0; g < Frequency; g++)
			{
				Vector2 vel = new Vector2(0, Main.rand.NextFloat(6.65f, 10.5f)).RotatedByRandom(6.283) * mulVelocity;
				var me = new MagneticElectricity
				{
					velocity = vel,
					Active = true,
					Visible = true,
					maxTime = Main.rand.Next(54, 180),
					ai = new float[] { Main.rand.NextFloat(0.1f, 1f), Main.rand.NextFloat(-0.01f, 0.01f), Main.rand.NextFloat(1.6f, 2f) * mulVelocity },
					position = Projectile.Center - vel * 3
				};
				VFXManager.Add(me);
			}
		}
		public override void OnSpawn(IEntitySource source)
		{
			GenerateVFXExpolode(4, 0.6f);
		}
		public override void AI()
		{
			Projectile.velocity *= 0.95f;

			if (Projectile.timeLeft <= 198)
				Projectile.friendly = false;
			float LightS = Projectile.timeLeft / 2f - 95f;
			if (LightS > 0)
				Lighting.AddLight((int)(Projectile.Center.X / 16), (int)(Projectile.Center.Y / 16), 0, LightS * 0.83f, LightS * 0.8f);

			Projectile.velocity *= 0;
		}
		public override void PostDraw(Color lightColor)
		{
			Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitLight");
			float Dark = Math.Max((Projectile.timeLeft - 150) / 50f, 0);
			Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, new Color(0, 199, 129, 0) * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f * Dark, SpriteEffects.None, 0);
		}
		public override bool PreDraw(ref Color lightColor)
		{
			Texture2D Shadow = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHit");
			float Dark = Math.Max((Projectile.timeLeft - 150) / 50f, 0);
			Main.spriteBatch.Draw(Shadow, Projectile.Center - Main.screenPosition, null, Color.White * Dark, 0, Shadow.Size() / 2f, 2.2f * Projectile.ai[0] / 15f, SpriteEffects.None, 0);
			Texture2D light = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/CursedFlames/CursedHitStar");
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 199, 129, 0), 0 + Projectile.ai[1], light.Size() / 2f, new Vector2(1f, Dark * Dark) * Projectile.ai[0] / 20f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 199, 129, 0), 1.57f + Projectile.ai[1], light.Size() / 2f, new Vector2(0.8f, Dark * Projectile.ai[0] / 20f), SpriteEffects.None, 0);

			float value = (480 - Projectile.timeLeft * 2.4f) / Projectile.timeLeft * 1.4f;
			if (value < 0)
				value = 0;
			float colorV = 0.9f * (1 - value);
			if (Projectile.ai[0] >= 10)
				colorV *= Projectile.ai[0] / 10f;
			Texture2D t = MythContent.QuickTexture("OmniElementItems/Projectiles/Wave");
			DrawTexCircle(value * 7 * Projectile.ai[0], 10 * value * value, new Color(0, colorV, colorV * 0.7f, 0f), Projectile.Center - Main.screenPosition, t);

			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 199, 129, 0), (float)(Math.PI / 4d) + Projectile.ai[1], light.Size() / 2f, new Vector2(0.6f, Dark * Projectile.ai[0] / 20f), SpriteEffects.None, 0);
			Main.spriteBatch.Draw(light, Projectile.Center - Main.screenPosition, null, new Color(0, 199, 129, 0), (float)(Math.PI / 4d * 3) + Projectile.ai[1], light.Size() / 2f, new Vector2(0.6f, Dark * Projectile.ai[0] / 20f), SpriteEffects.None, 0);
			return false;
		}

		private static void DrawTexCircle_VFXBatch(VFXBatch spriteBatch, float radious, float width, Color color, Vector2 center, Texture2D tex, double addRot = 0)
		{
			var circle = new List<Vertex2D>();

			for (int h = 0; h < radious / 2; h += 1)
			{
				circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 1, 0)));
				circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(h / radious * Math.PI * 4 + addRot), color, new Vector3(h * 2 / radious, 0, 0)));
			}
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(1, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(1, 0, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, Math.Max(radious - width, 0)).RotatedBy(addRot), color, new Vector3(0, 1, 0)));
			circle.Add(new Vertex2D(center + new Vector2(0, radious).RotatedBy(addRot), color, new Vector3(0, 0, 0)));
			if (circle.Count > 2)
				spriteBatch.Draw(tex, circle, PrimitiveType.TriangleStrip);
		}
		public void DrawWarp(VFXBatch spriteBatch)
		{
			float value = (200 - Projectile.timeLeft) / 200f;
			value = MathF.Sqrt(value);
			float colorV = 0.9f * (1 - value);
			if (Projectile.ai[0] >= 10)
				colorV *= Projectile.ai[0] / 10f;
			Texture2D t = MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Vague");
			float width = 60;
			if (Projectile.timeLeft < 60)
				width = Projectile.timeLeft;

			DrawTexCircle_VFXBatch(spriteBatch, value * 12 * Projectile.ai[0], width * 2, new Color(colorV, colorV * 0.7f, colorV, 0f), Projectile.Center - Main.screenPosition, t);
		}
		public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
		{
		}
		public override void OnHitPvp(Player target, int damage, bool crit)
		{
		}
	}
}