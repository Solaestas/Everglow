using Everglow.Myth.LanternMoon.Gores;
using Everglow.Myth.LanternMoon.LanternCommon;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.NPCs;

public class FloatLantern : ModNPC
{

	public LanternMoonProgress LanternMoonProgress = ModContent.GetInstance<LanternMoonProgress>();
	public override void SetStaticDefaults()
	{
		Main.npcFrameCount[NPC.type] = 3;
	}
	public override void SetDefaults()
	{
		NPC.damage = 100;
		NPC.lifeMax = 500;
		NPC.npcSlots = 14f;
		NPC.width = 62;
		NPC.height = 74;
		NPC.defense = 0;
		NPC.value = 0;
		NPC.aiStyle = -1;
		NPC.knockBackResist = 0.6f;
		NPC.dontTakeDamage = false;
		NPC.noGravity = true;
		NPC.noTileCollide = true;
		NPC.HitSound = SoundID.NPCHit3;
	}
	public override void OnSpawn(IEntitySource source)
	{
		NPC.ai[1] = Main.rand.Next(-120, 0);
		NPC.ai[2] = Main.rand.NextFloat(0.3f, 1800f);
	}
	public override void AI()
	{
		NPC.ai[1] += 1;
		NPC.ai[2] += 0.01f;
		NPC.TargetClosest(false);
		Player player = Main.player[NPC.target];
		NPC.rotation = NPC.velocity.X / 30f;
		NPC.ai[0] += 1;
		if (NPC.ai[0] % 45 < 15)
			NPC.frame.Y = 0;
		if (NPC.ai[0] % 45 >= 15 && NPC.ai[0] % 45 < 30)
			NPC.frame.Y = 74;
		if (NPC.ai[0] % 45 >= 30 && NPC.ai[0] % 45 < 45)
			NPC.frame.Y = 148;
		Vector2 v = player.Center + new Vector2((float)Math.Sin(NPC.ai[0] / 40f) * 500f, (float)Math.Sin((NPC.ai[0] + 200) / 40f) * 50f - 150) - NPC.Center;
		if (NPC.velocity.Length() < 9f)
			NPC.velocity += v / v.Length() * 0.35f;
		NPC.velocity *= 0.96f;

		if (Main.dayTime)
			NPC.velocity.Y += 1;
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}
	public override void HitEffect(NPC.HitInfo hit)
	{
		if (NPC.life <= 0)
		{
			var gore0 = new FloatLanternGore1
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
				noGravity = false,
				position = NPC.Center
			};
			Ins.VFXManager.Add(gore0);
			var gore1 = new FloatLanternGore2
			{
				Active = true,
				Visible = true,
				velocity = new Vector2(Main.rand.NextFloat(0, 6), 0).RotatedByRandom(6.283),
				noGravity = false,
				position = NPC.Center
			};
			Ins.VFXManager.Add(gore1);
			for (int f = 0; f < 2; f++)
			{
				var gore2 = new FloatLanternGore3
				{
					Active = true,
					Visible = true,
					velocity = new Vector2(Main.rand.NextFloat(0, 21), 0).RotatedByRandom(6.283),
					noGravity = false,
					position = NPC.Center
				};
				Ins.VFXManager.Add(gore2);
				var gore3 = new FloatLanternGore4
				{
					Active = true,
					Visible = true,
					velocity = new Vector2(Main.rand.NextFloat(0, 21), 0).RotatedByRandom(6.283),
					noGravity = false,
					position = NPC.Center
				};
				Ins.VFXManager.Add(gore3);
				var gore4 = new FloatLanternGore5
				{
					Active = true,
					Visible = true,
					velocity = new Vector2(Main.rand.NextFloat(0, 21), 0).RotatedByRandom(6.283),
					noGravity = false,
					position = NPC.Center
				};
				Ins.VFXManager.Add(gore4);
				var gore5 = new FloatLanternGore6
				{
					Active = true,
					Visible = true,
					velocity = new Vector2(Main.rand.NextFloat(0, 21), 0).RotatedByRandom(6.283),
					noGravity = false,
					position = NPC.Center
				};
				Ins.VFXManager.Add(gore5);
			}
			LanternMoonProgress.AddPoint(15);

			for (int f = 0; f < 22; f++)
			{
				Vector2 v3 = new Vector2(0, Main.rand.NextFloat(0, 12f)).RotatedByRandom(MathHelper.TwoPi);
				int r = Dust.NewDust(NPC.Center - new Vector2(4, 4) - new Vector2(4, 4), 8, 8, ModContent.DustType<Dusts.Flame4>(), v3.X, v3.Y, 0, default, Main.rand.NextFloat(0.6f, 1.8f));
				Main.dust[r].noGravity = true;
				Main.dust[r].velocity = v3;
			}
		}
	}
	public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		var texture = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Texture2D tg = ModAsset.FloatLanternGlow.Value;
		Texture2D tg2 = ModAsset.FloatLanternGlow2.Value;
		SpriteEffects effects = SpriteEffects.None;
		if (NPC.spriteDirection == 1)
			effects = SpriteEffects.FlipHorizontally;
		var value = new Vector2(NPC.Center.X, NPC.Center.Y);
		var vector = new Vector2(texture.Width / 2, texture.Height / Main.npcFrameCount[NPC.type] / 2);
		Vector2 vector2 = value - Main.screenPosition;
		vector2 -= new Vector2(tg.Width, tg.Height / Main.npcFrameCount[NPC.type]) * 1f / 2f;
		vector2 += vector * 1f + new Vector2(0f, 4f + NPC.gfxOffY);
		spriteBatch.Draw(tg, vector2, new Rectangle(0, NPC.frame.Y, 62, 74), new Color(200, 200, 200, 0), NPC.rotation, vector, 1f, effects, 0f);

		spriteBatch.Draw(tg2, vector2, new Rectangle(0, NPC.frame.Y, 62, 74), new Color(200, 200, 200, 0), NPC.rotation, vector, 1f, effects, 0f);
	}
}
