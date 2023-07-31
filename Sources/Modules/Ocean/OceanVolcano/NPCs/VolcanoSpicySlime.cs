 using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
    public class VolcanoSpicySlime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("Lava Slime");
			Main.npcFrameCount[base.NPC.type] = 2;
			// base.DisplayName.AddTranslation(GameCulture.Chinese, "尖刺火山史莱姆");
		}
		public override void SetDefaults()
		{
			base.NPC.aiStyle = 1;
			base.NPC.damage = 168;
			base.NPC.width = 40;
			base.NPC.height = 30;
			base.NPC.defense = 10;
			base.NPC.lifeMax = 3500;
			base.NPC.knockBackResist = 0f;
			this.AnimationType = 81;
			base.NPC.value = (float)Item.buyPrice(0, 1, 0, 0);
			base.NPC.alpha = 25;
			base.NPC.lavaImmune = false;
			base.NPC.noGravity = false;
			base.NPC.noTileCollide = false;
			base.NPC.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath1;
			base.NPC.buffImmune[24] = true;
		}
        public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (base.NPC.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(base.NPC.Center.X, base.NPC.Center.Y);
            Vector2 vector = new Vector2((float)(TextureAssets.Npc[base.NPC.type].Value.Width / 2), (float)(TextureAssets.Npc[base.NPC.type].Value.Height / Main.npcFrameCount[base.NPC.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/尖刺史莱姆1Glow").Width(), (float)(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/尖刺史莱姆1Glow").Height() / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = new Color(0.49f, 0.49f, 0.49f, 0);
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/尖刺史莱姆1Glow"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
        }
        public override void AI()
		{
			if (this.spikeTimer > 0f)
			{
				this.spikeTimer -= 1f;
			}
			if (!base.NPC.wet)
			{
				Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
				float num = Main.player[base.NPC.target].position.X + (float)Main.player[base.NPC.target].width * 0.5f - vector.X;
				float num2 = Main.player[base.NPC.target].position.Y - vector.Y;
				float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
				if (Main.expertMode && num3 < 120f && Collision.CanHit(base.NPC.position, base.NPC.width, base.NPC.height, Main.player[base.NPC.target].position, Main.player[base.NPC.target].width, Main.player[base.NPC.target].height) && base.NPC.velocity.Y == 0f)
				{
					base.NPC.ai[0] = -40f;
					if (base.NPC.velocity.Y == 0f)
					{
						base.NPC.velocity.X = base.NPC.velocity.X * 0.9f;
					}
					if (Main.netMode != 1 && this.spikeTimer == 0f)
					{
						for (int i = 0; i < 5; i++)
						{
							Vector2 value = new Vector2((float)(i - 2), -4f);
							value.X *= 1f + (float)Main.rand.Next(-50, 51) * 0.005f;
							value.Y *= 1f + (float)Main.rand.Next(-50, 51) * 0.005f;
							value.Normalize();
							value *= 4f + (float)Main.rand.Next(-50, 51) * 0.01f;
							Projectile.NewProjectile(vector.X, vector.Y, value.X, value.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.烈火球>(), 192, 0f, Main.myPlayer, 0f, 0f);
							this.spikeTimer = 30f;
						}
						return;
					}
				}
				else if (num3 < 360f && Collision.CanHit(base.NPC.position, base.NPC.width, base.NPC.height, Main.player[base.NPC.target].position, Main.player[base.NPC.target].width, Main.player[base.NPC.target].height) && base.NPC.velocity.Y == 0f)
				{
					base.NPC.ai[0] = -40f;
					if (base.NPC.velocity.Y == 0f)
					{
						base.NPC.velocity.X = base.NPC.velocity.X * 0.9f;
					}
					if (Main.netMode != 1 && this.spikeTimer == 0f)
					{
						num2 = Main.player[base.NPC.target].position.Y - vector.Y - (float)Main.rand.Next(0, 200);
						num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
						num3 = 6.5f / num3;
						num *= num3;
						num2 *= num3;
						this.spikeTimer = 50f;
                        Projectile.NewProjectile(vector.X, vector.Y, num, num2,ModContent.ProjectileType<Everglow.Ocean.Projectiles.烈火球>(), 155, 0f, Main.myPlayer, 0f, 0f);
					}
				}
			}
		}
		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 5; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 4, (float)hitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.NPC.life <= 0)
			{
				for (int j = 0; j < 20; j++)
				{
					Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 4, (float)hitDirection, -1f, 0, default(Color), 1f);
				}
			}
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneVolcano)
			{
				return 5f;
			}
			return 0f;
		}
		public override void OnKill()
		{
            if (Main.rand.Next(100) == 1)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.LavaCupStaff>(), 1, false, 0, false, false);
            }
            Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.LavaStone>(), Main.rand.Next(1,4), false, 0, false, false);
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			if (Main.expertMode)
			{
				player.AddBuff(24, 540, true);
			}
		}
		public float spikeTimer = 60f;
	}
}
