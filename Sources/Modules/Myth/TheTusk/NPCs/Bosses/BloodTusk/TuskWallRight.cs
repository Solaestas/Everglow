using Terraria;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Everglow.Myth.TheTusk.NPCs.Bosses.BloodTusk;

public class TuskWallRight : ModNPC
{
	public override void SetStaticDefaults()
	{
		// DisplayName.SetDefault("Tusk Wall");
			}
	private Vector2[] V = new Vector2[10];
	private Vector2[] VMax = new Vector2[10];
	public override void SetDefaults()
	{
		NPC.behindTiles = true;
		NPC.damage = 0;
		NPC.width = 418;
		NPC.height = 880;
		NPC.defense = 0;
		NPC.lifeMax = 5;
		NPC.knockBackResist = 0f;
		NPC.value = Item.buyPrice(0, 0, 0, 0);
		NPC.aiStyle = -1;
		NPC.alpha = 255;
		NPC.lavaImmune = true;
		NPC.noGravity = false;
		NPC.noTileCollide = false;
		NPC.dontTakeDamage = true;
	}
	private bool squ = false;
	private bool Down = true;
	private bool canDespawn;
	public NPC BloodTuskTarget;
	public override void OnSpawn(IEntitySource source)
	{
		foreach (NPC npc in Main.npc)
		{
			if (npc.active)
			{
				if (npc.type == ModContent.NPCType<BloodTusk>())
				{
					if ((npc.Center - NPC.Center).Length() < 3000)
					{
						BloodTuskTarget = npc;
						break;
					}
				}
			}
		}
		base.OnSpawn(source);
	}
	public override void AI()
	{
		if (BloodTuskTarget == null)
		{
			foreach (NPC npc in Main.npc)
			{
				if (npc.active)
				{
					if (npc.type == ModContent.NPCType<BloodTusk>())
					{
						if ((npc.Center - NPC.Center).Length() < 3000)
						{
							BloodTuskTarget = npc;
							break;
						}
					}
				}
			}
		}
		if (BloodTuskTarget == null)
		{
			NPC.active = false;
		}
		BloodTusk bTusk = BloodTuskTarget.ModNPC as BloodTusk;
		/*联机情况下错误排查*/
		if (NPC.Bottom.Y > bTusk.DarkCenter.Y && Collision.SolidCollision(NPC.Bottom + new Vector2(0, -10), 1, 1))
			NPC.position.Y -= 5f;
		NPC.TargetClosest(false);
		VMax[0] = new Vector2(0, 840);
		Player player = Main.player[NPC.target];
		if (NPC.collideX && Down)
			NPC.active = false;
		NPC.velocity.X *= 0;

		if (BloodTuskTarget.active && BloodTuskTarget.life < BloodTuskTarget.lifeMax * 0.5)
		{
			if (NPC.Center.X - BloodTuskTarget.Center.X > 150)
				NPC.position.X -= 0.05f;
			if (NPC.Center.X - BloodTuskTarget.Center.X < -150)
				NPC.position.X += 0.05f;
		}
		if (NPC.collideY && NPC.alpha > 0 && !squ)
		{
			if (Main.tile[(int)(NPC.Bottom.X / 16d), (int)(NPC.Bottom.Y / 16d)].IsHalfBlock && Down)
			{
				Down = false;
				NPC.position.Y += 16;
			}
			startFight = true;
			V[0] = VMax[0];
			NPC.alpha -= 25;
		}
		if (NPC.alpha <= 0)
			NPC.alpha = 0;
		if (NPC.CountNPCS(ModContent.NPCType<BloodTusk>()) > 0 && !squ)
		{
			if (V[0].Y > 5)
			{
				TuskModPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<TuskModPlayer>();
				mplayer.MinaShake = 6;
				V[0].Y -= 5f;
				for (int h = 0; h < 20; h++)
				{
					int k = Dust.NewDust(NPC.Bottom + new Vector2(Main.rand.NextFloat(-200f, 200f), Main.rand.NextFloat(-8f, 8f)), 0, 0, DustID.Blood, 0, 0, 0, default, Main.rand.NextFloat(1.3f, 4.3f));
					Main.dust[k].noGravity = true;
				}
			}
		}

		if (NPC.CountNPCS(ModContent.NPCType<BloodTusk>()) <= 0)
			squ = true;
		if (bTusk.Killing > 0)
			squ = true;
		if (squ)
		{
			TuskModPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<TuskModPlayer>();
			mplayer.Shake = 6;
			V[0].Y += 5f;
			for (int h = 0; h < 20; h++)
			{
				int k = Dust.NewDust(NPC.Bottom + new Vector2(Main.rand.NextFloat(-200f, 200f), Main.rand.NextFloat(-8f, 8f)), 0, 0, DustID.Blood, 0, 0, 0, default, Main.rand.NextFloat(1.3f, 4.3f));
				Main.dust[k].noGravity = true;
			}
			if (V[0].Y > 860)
			{
				NPC.alpha += 15;
				if (NPC.alpha > 240)
					NPC.active = false;
			}
		}
		if (player.Center.Y < NPC.Bottom.Y - 200)
		{
			if (player.Center.X > NPC.Left.X + 150)
			{
				if (player.velocity.X > 0)
				{
					player.velocity.X = -1;
					player.position = player.oldPosition;
				}
			}
		}
		else
		{
			if (player.Center.X > NPC.Left.X + 40)
			{
				if (player.velocity.X > 0)
				{
					player.velocity.X = -1;
					player.position = player.oldPosition;
				}
			}
		}

		if (!player.active || player.dead)
			canDespawn = true;
		else
		{
			if (NPC.CountNPCS(ModContent.NPCType<BloodTusk>()) <= 0)
				canDespawn = true;
			else
			{
				if (bTusk.Killing > 0)
					canDespawn = true;
				else
				{
					canDespawn = false;
				}
			}
		}
	}
	public override bool CheckActive()
	{
		return canDespawn;
	}
	public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
	{
	}
	private bool startFight = false;
	public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
	{
		if (!startFight)
			return false;
		Color color = Lighting.GetColor((int)(NPC.Center.X / 16d), (int)(NPC.Bottom.Y / 16d));
		color = NPC.GetAlpha(color) * ((255 - NPC.alpha) / 255f);
		Texture2D t = ModAsset.TuskWallRight.Value;
		Main.spriteBatch.Draw(t, NPC.Center - Main.screenPosition + V[0] + new Vector2(0, 60), new Rectangle(0, 0, t.Width, t.Height - (int)V[0].Y), color, NPC.rotation, new Vector2(t.Width / 2f, t.Height / 2f), 1f, SpriteEffects.None, 0f);
		return false;
	}
}
