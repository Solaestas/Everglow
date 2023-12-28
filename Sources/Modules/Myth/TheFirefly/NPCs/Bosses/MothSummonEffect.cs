using Everglow.Commons.Weapons;
using Terraria.DataStructures;

namespace Everglow.Myth.TheFirefly.NPCs.Bosses;

public class MothSummonEffect : TrailingProjectile
{
	public override string Texture => "Everglow/Commons/Textures/LightPoint";
	public NPC Moth;
	public override void SetDef()
	{
		Projectile.timeLeft = 600;
		Projectile.tileCollide = false;
		base.SetDef();
	}
	public override void OnSpawn(IEntitySource source)
	{
		bool hasMoth = false;
		foreach (var npc in Main.npc)
		{
			if (npc.active && npc.type == ModContent.NPCType<CorruptMoth>())
			{
				if ((npc.Center - Projectile.Center).Length() < 2500)
				{
					hasMoth = true;
					Moth = npc;
					break;
				}
			}
		}
		if (!hasMoth)
		{
			Projectile.active = false;
		}
		Projectile.timeLeft = Main.rand.Next(510, 560);
		base.OnSpawn(source);
	}
	public override void AI()
	{
		if (!Moth.active || Moth.type != ModContent.NPCType<CorruptMoth>())
		{
			Projectile.active = false;
		}
		Timer++;
		Projectile.rotation = MathF.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
		if (TimeTokill >= 0 && TimeTokill <= 2)
			Projectile.Kill();
		TimeTokill--;
		if (TimeTokill < 0)
		{
			if (Projectile.timeLeft < 500)
			{
				Vector2 toTarget = Moth.Center - Projectile.Center - Projectile.velocity;
				if (toTarget.Length() < 35f)
				{
					KillMainStructure();
					CorruptMoth cMoth = Moth.ModNPC as CorruptMoth;
					if (cMoth != null)
					{
						cMoth.Timer += 10;
					}
				}
				Vector2 targetVel = Vector2.Normalize(toTarget) * 15f;
				Projectile.velocity = Vector2.Lerp(Projectile.velocity, targetVel, 0.05f);

			}
			else
			{
				Projectile.velocity = Projectile.velocity.RotatedBy(Projectile.ai[0]);
				if (Projectile.velocity.Length() > 3f)
				{
					Projectile.velocity *= 0.9f;
				}
			}
		}
		else
		{
			Projectile.velocity *= 0f;
			return;
		}
	}
	public override bool PreDraw(ref Color lightColor)
	{
		DrawTrail();
		if (TimeTokill <= 0)
		{
			var texMain = (Texture2D)ModContent.Request<Texture2D>(Texture);
			Main.spriteBatch.Draw(texMain, Projectile.Center - Main.screenPosition, null, TrailColor, Projectile.rotation, texMain.Size() / 2f, Projectile.scale, SpriteEffects.None, 0);
		}
		return false;
	}
}
