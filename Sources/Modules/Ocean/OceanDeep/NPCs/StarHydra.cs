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
	// Token: 0x02000420 RID: 1056
    public class StarHydra : ModNPC
	{
		private int num1 = 0;
		private bool flag1 = true;
		// Token: 0x06001475 RID: 5237 RVA: 0x000082F6 File Offset: 0x000064F6
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("babyjellyfish");
			Main.npcFrameCount[base.NPC.type] = 4;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "星渊水螅");
		}

		// Token: 0x06001476 RID: 5238 RVA: 0x000B399C File Offset: 0x000B1B9C
		public override void SetDefaults()
		{
			base.NPC.noGravity = true;
			base.NPC.damage = 30;
			base.NPC.width = 26;
			base.NPC.height = 26;
			base.NPC.defense = 5;
			base.NPC.lifeMax = 650;
			base.NPC.alpha = 90;
			base.NPC.aiStyle = -1;
			this.AIType = -1;
			base.NPC.buffImmune[70] = true;
			base.NPC.value = (float)Item.buyPrice(0, 0, 0, 0);
			base.NPC.HitSound = SoundID.NPCHit25;
			base.NPC.DeathSound = SoundID.NPCDeath28;
		}

		// Token: 0x06001477 RID: 5239 RVA: 0x000B3A6C File Offset: 0x000B1C6C
		public override void AI()
		{
			base.NPC.TargetClosest(true);
			num1 += 1;
			base.NPC.rotation = (float)Math.Atan2((double)base.NPC.velocity.Y, (double)base.NPC.velocity.X) + 1.57f;
            base.NPC.velocity *= 0.94f;
            float num2 = 0.5f;
            if (base.NPC.velocity.X > -num2 && base.NPC.velocity.X < num2 && base.NPC.velocity.Y > -num2 && base.NPC.velocity.Y < num2)
            {
                float num3 = 8f;
				float num7 = Main.rand.Next(1000, 5000) / 1000f;
                Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
                float num4 = Main.player[base.NPC.target].position.X + (float)(Main.player[base.NPC.target].width / 2) - vector.X;
                float num5 = Main.player[base.NPC.target].position.Y + (float)(Main.player[base.NPC.target].height / 2) - vector.Y;
                float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                num6 = num3 / num6;
                num4 *= num6;
                num5 *= num6;
                base.NPC.velocity.X = num4 * num7;
                base.NPC.velocity.Y = num5 * num7;
                return;
            }
		}
        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position)
		{
			scale = 0.7f;
			return null;
		}
		// Token: 0x06001478 RID: 5240 RVA: 0x000A9970 File Offset: 0x000A7B70
		public override void FindFrame(int frameHeight)
		{
			base.NPC.frameCounter += 0.15000000596046448;
			base.NPC.frameCounter %= (double)Main.npcFrameCount[base.NPC.type];
			int num = (int)base.NPC.frameCounter;
			base.NPC.frame.Y = num * frameHeight;
		}

		// Token: 0x06001479 RID: 5241 RVA: 0x000B4268 File Offset: 0x000B2468

		// Token: 0x0600147A RID: 5242 RVA: 0x0000801E File Offset: 0x0000621E
		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			target.AddBuff(70, 240, true);
			target.AddBuff(base.Mod.Find<ModBuff>("极剧毒").Type, 5, true);
		}
        // Token: 0x02000413 RID: 1043

		// Token: 0x0600147B RID: 5243 RVA: 0x000A99DC File Offset: 0x000A7BDC
		public override void HitEffect(NPC.HitInfo hit)
		{
			for (int i = 0; i < 3; i++)
			{
				Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, hit.HitDirection, -1f, 0, default(Color), 1f);
			}
			if (base.NPC.life <= 0)
			{
				for (int j = 0; j < 15; j++)
				{
					Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 5, hit.HitDirection, -1f, 0, default(Color), 1f);
				}
			}
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
            vector2 -= new Vector2((float)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/星渊水螅光辉").Width(), (float)(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/星渊水螅光辉").Height() / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(297 - base.NPC.alpha, 297 - base.NPC.alpha, 297 - base.NPC.alpha, 0), Color.White);
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/星渊水螅光辉"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
        }
        // Token: 0x02000413 RID: 1043
        public override void OnKill()
        {
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.空灵泡>(), 1, false, 0, false, false);
            }
        }
	}
}
