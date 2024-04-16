using Terraria.DataStructures;
using static Humanizer.In;
using static Terraria.GameContent.Animations.IL_Actions.NPCs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class HighSpeedStone : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 32;
		Projectile.height = 32;
		Projectile.aiStyle = 0;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 1000;
	}
	Player target;
	NPC owner;
	public override void AI()
	{
		Projectile.ai[1]++;
		Projectile.rotation += 0.03f;
		if (Projectile.ai[0] < 120)
		{
			Projectile.ai[0]++;
		}
		if (Projectile.ai[1] < 180)
		{
			Projectile.Center = Vector2.Lerp(Projectile.Center, owner.Center-new Vector2(0, Projectile.ai[1]/2), 0.05f);
		}
		else if (Projectile.ai[1] == 180)
		{

			Projectile.velocity = Vector2.Normalize(target.Center - Projectile.Center) * 14;
		}
		else
		{
			if(Projectile.velocity.Length()<=14)
			Projectile.velocity += new Vector2(0, 0.163f);
			Projectile.tileCollide = true;
		}
	}
	public override void OnSpawn(IEntitySource source)
	{
		target = Main.player[(int)Projectile.ai[0]];
		Projectile.ai[0] = 0;
		owner = Main.npc[(int)Projectile.ai[1]];
		Projectile.ai[1] = 0;
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{

	}
	public override void OnKill(int timeLeft)
	{

	}
	public override bool PreDraw(ref Color lightColor)
	{
		List<Vertex2D> vertex2Ds = new List<Vertex2D>();
		Texture2D texture = ModAsset.HighSpeedStone.Value;

		Vector2 DrawCenter = Projectile.Center - Main.screenPosition;
		for (int i = 1; i <= 10; i++)
		{
			Vector2 DrawPoint1 = new Vector2(0, -Projectile.ai[0] / 10).RotatedBy(i / 5d * Math.PI);
			Vector2 DrawPoint2 = new Vector2(0, -Projectile.ai[0] / 10).RotatedBy((i + 1) / 5d * Math.PI);
			Vector2 dp1 = DrawPoint1.RotatedBy(Projectile.rotation);
			Vector2 dp2 = DrawPoint2.RotatedBy(Projectile.rotation);
			vertex2Ds.Add(new Vertex2D(DrawCenter + dp1, Color.White, new Vector3(0.5f + DrawPoint1.X / texture.Width, 0.5f + DrawPoint1.Y / texture.Height, 0)));
			vertex2Ds.Add(new Vertex2D(DrawCenter + dp2, Color.White, new Vector3(0.5f + DrawPoint2.X / texture.Width, 0.5f + DrawPoint2.Y / texture.Height, 0)));
			vertex2Ds.Add(new Vertex2D(DrawCenter, Color.White, new Vector3(0.5f, 0.5f, 0)));

		}
		if (vertex2Ds.Count > 0)
		{
			Main.graphics.GraphicsDevice.Textures[0] = texture;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleList, vertex2Ds.ToArray(), 0, vertex2Ds.Count / 3);
		}
		return false;
	}
}
