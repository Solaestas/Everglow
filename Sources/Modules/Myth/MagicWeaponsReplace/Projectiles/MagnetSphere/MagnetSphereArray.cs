using Everglow.Myth.Common;
using static Everglow.Myth.Common.MythUtils;
namespace Everglow.Myth.MagicWeaponsReplace.Projectiles.MagnetSphere;

internal class MagnetSphereArray : ModProjectile, IWarpProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 28;
		Projectile.height = 28;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 10000;
		Projectile.DamageType = DamageClass.Summon;
		Projectile.tileCollide = false;
	}

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Projectile.Center = Projectile.Center * 0.7f + (player.Center + new Vector2(player.direction * 22, 12 * player.gravDir * (float)(0.2 + Math.Sin(Main.timeForVisualEffects / 18d) / 2d))) * 0.3f;
		Projectile.spriteDirection = player.direction;
		Projectile.velocity *= 0;
		if (player.itemTime > 0 && player.HeldItem.type == ItemID.MagnetSphere)
		{
			Projectile.timeLeft = player.itemTime + 60;
			if (Timer < 30)
				Timer++;
		}
		else
		{
			Timer--;
			if (Timer < 0)
				Projectile.Kill();
		}
		Player.CompositeArmStretchAmount PCAS = Player.CompositeArmStretchAmount.Full;

		player.SetCompositeArmFront(true, PCAS, (float)(-Math.Sin(Main.timeForVisualEffects / 18d) * 0.6 + 1.2) * -player.direction);
		Vector2 vTOMouse = Main.MouseWorld - player.Center;
		player.SetCompositeArmBack(true, PCAS, (float)(Math.Atan2(vTOMouse.Y, vTOMouse.X) - Math.PI / 2d));
		Projectile.rotation = player.fullRotation;

		RingPos = RingPos * 0.9f + new Vector2(-12 * player.direction, -24 * player.gravDir) * 0.1f;
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		behindNPCs.Add(index);
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Projectile.hide = false;
		DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLineBlackShade"), new Color(1f, 1f, 1f, 1f));
		DrawMagicArray(MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/Vague"), new Color(0, 255, 174, 0));
		return false;
	}

	internal int Timer = 0;
	internal Vector2 RingPos = Vector2.Zero;

	public void DrawMagicArray(Texture2D tex, Color c0)
	{
		Player player = Main.player[Projectile.owner];
		Texture2D Water = tex;
		var c1 = new Color(c0.R * 0.39f / 255f, c0.G * 0.39f / 255f, c0.B * 0.39f / 255f, c0.A * 0.39f / 255f);
		DrawTexCircle(Timer * 1.6f, 22, c0, player.Center + RingPos - Main.screenPosition, Water, Main.timeForVisualEffects / 17);
		DrawTexCircle(Timer * 1.3f, 32, c1, player.Center + RingPos - Main.screenPosition, Water, -Main.timeForVisualEffects / 17);

		float timeRot = (float)(Main.timeForVisualEffects / 57d);
		Vector2 Point0 = player.Center + RingPos - Main.screenPosition;
		Vector2 Point1 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 0 + timeRot);
		Vector2 Point2 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 2 / 3d + timeRot);
		Vector2 Point3 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 4 / 3d - timeRot);

		Vector2 Point4 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 1 / 3d + timeRot * 2.4f);
		Vector2 Point5 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 3 / 3d - timeRot * 0.8f);
		Vector2 Point6 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 5 / 3d - timeRot);

		Vector2 Point7 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 1 / 3d + timeRot * 2);
		Vector2 Point8 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 3 / 3d - timeRot * 1.6f);
		Vector2 Point9 = player.Center + RingPos - Main.screenPosition + new Vector2(0, Timer * 1.8f).RotatedBy(Math.PI * 5 / 3d - timeRot * 1.1f);


		float Light1 = (float)(Math.Sin(Main.timeForVisualEffects / 3f + Math.PI / 3d * 1) + 0.2) / 1.4f;
		float Light2 = (float)(Math.Sin(Main.timeForVisualEffects / 3f + Math.PI / 3d * 2) + 0.2) / 1.4f;
		float Light3 = (float)(Math.Sin(Main.timeForVisualEffects / 4f + Math.PI / 3d * 3) + 0.2) / 1.4f;
		float Light4 = (float)(Math.Sin(Main.timeForVisualEffects / 3f + Math.PI / 3d * 4) + 0.2) / 1.4f;
		float Light5 = (float)(Math.Sin(Main.timeForVisualEffects / 2.3f + Math.PI / 3d * 4) + 0.2) / 1.4f;
		float Light6 = (float)(Math.Sin(Main.timeForVisualEffects / 3f + Math.PI / 3d * 6) + 0.2) / 1.4f;
		float Light7 = (float)(Math.Sin(Main.timeForVisualEffects / 5f + Math.PI / 3d * 4) + 0.4) / 2.4f;
		float Light8 = (float)(Math.Sin(Main.timeForVisualEffects / 3.3f + Math.PI / 3d * 4) + 0.3) / 1.8f;
		float Light9 = (float)(Math.Sin(Main.timeForVisualEffects / 3f + Math.PI / 3d * 6) + 0.2) / 1.4f;


		DrawTexLine(Point0, Point2, c1 * Light1, c1 * Light2, Water);
		DrawTexLine(Point0, Point3, c1 * Light2, c1 * Light3, Water);
		DrawTexLine(Point0, Point1, c1 * Light3, c1 * Light4, Water);

		DrawTexLine(Point0, Point5, c1 * Light4, c1 * Light5, Water);
		DrawTexLine(Point0, Point6, c1 * Light5, c1 * Light6, Water);
		DrawTexLine(Point0, Point4, c1 * Light6, c1 * Light1, Water);

		DrawTexLine(Point0, Point7, c1 * Light4, c1 * Light7, Water);
		DrawTexLine(Point0, Point8, c1 * Light5, c1 * Light9, Water);
		DrawTexLine(Point0, Point9, c1 * Light6, c1 * Light8, Water);
	}




	public void DrawWarp(VFXBatch spriteBatch)
	{

		Player player = Main.player[Projectile.owner];
		DrawTexCircle(spriteBatch, Timer * 1.2f, 52, new Color(64, 70, 255, 0), player.Center + RingPos - Main.screenPosition, MythContent.QuickTexture("MagicWeaponsReplace/Projectiles/WaterLine"), Main.timeForVisualEffects / 17);
	}
}