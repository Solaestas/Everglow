using Terraria.DataStructures;
using static Humanizer.In;
using static Terraria.GameContent.Animations.IL_Actions.NPCs;

namespace Everglow.Yggdrasil.YggdrasilTown.Projectiles;

public class Stonefragment : ModProjectile
{
	public override void SetDefaults()
	{
		Projectile.width = 16;
		Projectile.height = 16;
		Projectile.aiStyle = 0;
		Projectile.friendly = false;
		Projectile.hostile = true;
		Projectile.ignoreWater = false;
		Projectile.tileCollide = false;
		Projectile.timeLeft = 600;
	}

	public override void AI()
	{if(Projectile.velocity.Y<=12)
		Projectile.velocity.Y += 0.08f;
	if(Projectile.timeLeft <= 570)
			Projectile.tileCollide = true;

	}
	public override void OnSpawn(IEntitySource source)
	{

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
