using Everglow.Commons.DataStructures;
using Everglow.Commons.Graphics;
using Everglow.Commons.Mechanics.ElementalDebuff;
using Everglow.Yggdrasil.KelpCurtain.Items.Weapons;

namespace Everglow.Yggdrasil.KelpCurtain.Projectiles.Magic;

public class GreenSungloShield : ModProjectile
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MagicProjectiles;

	public override string Texture => ModAsset.GreenSungloThorns_Mod;

	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 48;
		Projectile.aiStyle = -1;
		Projectile.friendly = false;
		Projectile.hostile = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1200;
		Projectile.penetrate = -1;
		Projectile.hide = true;

	}

	int timer = 0;

	public override void AI()
	{
		Player player = Main.player[Projectile.owner];
		Projectile.Center = Main.player[Projectile.owner].Center;
		Projectile.timeLeft++;
		timer++;
		if (player.HeldItem.type != ModContent.ItemType<GreenSungloStaff>())
		{
			Projectile.Kill();
		}
	}

	public override void DrawBehind(int index, List<int> behindNPCsAndTiles, List<int> behindNPCs, List<int> behindProjectiles, List<int> overPlayers, List<int> overWiresUI)
	{
		//overPlayers.Add(index);
	}


	public override bool PreDraw(ref Color lightColor)
	{
		int a = 3;
		int b = 2;
		int d;
		int Width = 20;
		int count = Math.Min(628, timer*2);
		var thorns = new List<Vertex2D>();
		var thornsbehind = new List<Vertex2D>();

		for (float i = 0; i <= count; i++)
		{
			Vector2 pos0 = new Vector2(MathF.Cos(a * (i + 1) * 0.01f + (float)Main.time * 0.02f), MathF.Sin(b * (i + 1) * 0.01f + (float)Main.time * 0.02f)) * 30;
			Vector2 pos = new Vector2(MathF.Cos(a * i*0.01f + (float)Main.time * 0.02f), MathF.Sin(b * i * 0.01f + (float)Main.time * 0.02f)) * 30;
			Vector2 pos2 = new Vector2(MathF.Cos(a * (i - 1) * 0.01f + (float)Main.time * 0.02f), MathF.Sin(b * (i - 1) * 0.01f + (float)Main.time * 0.02f)) * 30;
			Vector2 normal = (pos2 - pos0).NormalizeSafe().RotatedBy(MathF.PI + 0.5f);
			normal = MathUtils.Lerp(0.5f,normal,Vector2.UnitY);
			thorns.Add(new Vertex2D(Projectile.Center + pos + normal * Width - Main.screenPosition, Color.White, new Vector3(0, (float)(i / 80f), 0)));
			thorns.Add(new Vertex2D(Projectile.Center + pos - normal * Width - Main.screenPosition, Color.White, new Vector3(1, (float)(i / 80f), 0)));
			thornsbehind.Add(new Vertex2D(Projectile.Center + pos - normal * Width - Main.screenPosition, Color.White, new Vector3(0, (float)(i / 100f), 0)));
			thornsbehind.Add(new Vertex2D(Projectile.Center + pos + normal * Width - Main.screenPosition, Color.White, new Vector3(1, (float)(i / 100f), 0)));
		}

		Main.graphics.GraphicsDevice.Textures[0] = ModAsset.GreenSungloThorns.Value;
		if (thorns.Count > 3)
		{

			Main.NewText(count);
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, thorns.ToArray(), 0, thorns.Count - 2);
			//Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, thornsbehind.ToArray(), 0, thorns.Count - 2);
		}


		return false;
	}

	public override void PostDraw(Color lightColor) 
	{ 
	}
	public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
	{
		return (targetHitbox.Center() - projHitbox.Center()).Length() < 110f;
	}


	public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
	{
		target.AddElementalDebuffBuildUp(Main.player[Projectile.owner], ElementalDebuffType.Necrosis, Projectile.damage * 3);
		target.AddBuff(BuffID.Confused, 180);
		base.ModifyHitNPC(target, ref modifiers);
	}
}