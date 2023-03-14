using Everglow.Myth.Common;
using Everglow.Myth.TheFirefly.Dusts;

namespace Everglow.Myth.TheFirefly.NPCs;

public class LargeFireBulb : ModNPC
{
	public override void SetDefaults()
	{
		NPC.damage = 0;
		NPC.width = 128;
		NPC.height = 128;
		NPC.defense = 0;
		NPC.lifeMax = 1;
		NPC.knockBackResist = 1f;
		NPC.value = Item.buyPrice(0, 0, 0, 0);
		NPC.color = new Color(0, 0, 0, 0);
		NPC.alpha = 0;
		NPC.boss = false;
		NPC.lavaImmune = true;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.behindTiles = true;

		NPC.dontTakeDamage = true;
		NPC.aiStyle = -1;
	}
	private bool HitT = false;
	private bool Ini = false;
	private float MaxL = 0;
	private Vector2 StaCen = Vector2.Zero;

	public override void AI()
	{
		if (!Ini)
		{
			int MaxxL = 400;
			for (int Dy = 5; Dy < 400; Dy++)
			{
				if (Collision.SolidCollision(NPC.Center + new Vector2(0 - 5, 20 + Dy - 5), 10, 10))
				{
					MaxxL = Dy;
					break;
				}
			}
			NPC.velocity = new Vector2(0, 0.3f);
			MaxL = Main.rand.Next(4, MaxxL);
			Ini = true;
			StaCen = NPC.Center;
		}
		Vector2 TOCen = StaCen - NPC.Center;
		if (!HitT)
		{
			if (NPC.Center.Y - StaCen.Y < MaxL)
			{
				if (Collision.SolidCollision(NPC.position - Vector2.One * 5f + NPC.velocity * 10, 10, 10))
				{
					NPC.velocity *= 0.96f;
					if (NPC.velocity.Length() < 0.05f)
					{
						NPC.velocity *= 0;
						MaxL = NPC.Center.Y - StaCen.Y;
						HitT = true;
					}
				}
			}
			else
			{
				NPC.velocity *= 0.99f;
				if (NPC.velocity.Length() < 0.05f)
				{
					NPC.velocity *= 0;
					MaxL = NPC.Center.Y - StaCen.Y;
					HitT = true;
				}
			}
		}
		else
		{
			NPC.noTileCollide = false;
			NPC.dontTakeDamage = false;
			float Leng = NPC.velocity.Length() * NPC.velocity.Length() / MaxL;

			NPC.velocity += TOCen / TOCen.Length() * Leng;
			NPC.velocity += new Vector2(0, 0.15f);
			NPC.velocity += TOCen / TOCen.Length() * (TOCen.Length() - MaxL) * 0.001f;
			if (NPC.velocity.Length() > 1f)
				NPC.velocity -= NPC.velocity * 0.003f;
		}
		NPC.rotation = (float)(Math.Atan2(TOCen.Y, TOCen.X) + Math.PI / 2d);
		Lighting.AddLight((int)(NPC.Center.X / 16), (int)(NPC.Center.Y / 16 - 1), 0, 0.1f, 0.8f);
	}

	int HitCount = 0;
	public override void HitEffect(int hitDirection, double damage)
	{
		NPC.velocity *= 0.2f;
		if (NPC.life <= 0)
		{
			NPC.life = 1;
			NPC.active = true;
			HitCount++;
			if (HitCount >= 8 + Main.rand.Next(3, 5)) //Attempted random hit count criteria. ~Setnour6
			{
				for (int h = 0; h < 60; h += 3)
				{
					Vector2 v3 = new Vector2(0, (float)Math.Sin(h * Math.PI / 4d) + 2).RotatedBy(h * Math.PI / 10d) * Main.rand.NextFloat(0.2f, 1.1f);
					int r = Dust.NewDust(NPC.Center - new Vector2(4, 4), 0, 0, ModContent.DustType<PureBlue>(), 0, 0, 0, default, 4f * Main.rand.NextFloat(0.7f, 5.9f));
					Main.dust[r].noGravity = true;
					Main.dust[r].velocity = v3 * 4;
				}
				for (int y = 0; y < 30; y += 3)
				{
					int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.9f, 4.2f));
					Main.dust[index].noGravity = true;
					Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(1.8f, 8.5f)).RotatedByRandom(Math.PI * 2d);
				}
				for (int y = 0; y < 30; y += 3)
				{
					int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(2f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.9f, 4.2f));
					Main.dust[index].noGravity = true;
					Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(3.0f, 10f)).RotatedByRandom(Math.PI * 2d);
				}
				for (int y = 0; y < 18; y++)
				{
					int index = Dust.NewDust(NPC.Center + new Vector2(0, Main.rand.NextFloat(48f)).RotatedByRandom(3.1415926 * 2), 0, 0, ModContent.DustType<BlueGlow>(), 0f, 0f, 100, default, Main.rand.NextFloat(0.8f, 3.2f));
					Main.dust[index].noGravity = true;
					Main.dust[index].velocity = new Vector2(0, Main.rand.NextFloat(1.8f, 4.5f)).RotatedByRandom(Math.PI * 2d);
				}
				NPC.active = false;
				return;
			}
		}
	}

	private Vector2[] vPos = new Vector2[200];

	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		SpriteEffects effects = SpriteEffects.None;
		if (NPC.spriteDirection == 1)
			effects = SpriteEffects.FlipHorizontally;
		Texture2D tx = MythContent.QuickTexture("TheFirefly/NPCs/LargeFireBulb");
		Texture2D tg = MythContent.QuickTexture("TheFirefly/NPCs/LargeFireBulb_Glow");
		var vector = new Vector2(tx.Width / 2f, tx.Height / (float)Main.npcFrameCount[NPC.type] / 2f);

		Color color0 = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Center.Y / 16d));
		Main.spriteBatch.Draw(tx, NPC.Center - Main.screenPosition, new Rectangle(0, 128, 128, 128), color0, NPC.rotation, vector, 1f, effects, 0f);

		Main.spriteBatch.Draw(tx, StaCen - Main.screenPosition + new Vector2(0, 24), new Rectangle(0, 0, 128, 64), color0, 0, vector, 1f, effects, 0f);
		var color = new Color(255, 255, 255, 0);
		Main.spriteBatch.Draw(tg, NPC.Center - Main.screenPosition, new Rectangle(0, 128, 128, 128), color, NPC.rotation, vector, 1f, effects, 0f);
		vPos[0] = NPC.Center + new Vector2(0, -34);
		for (int f = 1; f < 200; f++)
		{
			if ((StaCen - vPos[f - 1]).Length() < 24)
				break;
			vPos[f] = vPos[f - 1] + (StaCen - vPos[f - 1]) / (StaCen - vPos[f - 1]).Length() * 6;
			Color color2 = Lighting.GetColor((int)(vPos[f].X / 16d), (int)(vPos[f].Y / 16d));
			Main.spriteBatch.Draw(tx, vPos[f] - Main.screenPosition + new Vector2(0, 40), new Rectangle(0, 64, 128, 6), color2, NPC.rotation, vector, 1f, effects, 0f);
		}
		return false;
	}
}