using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs.OceanCrystal
{
    [AutoloadBossHead]
    public class OceanCrystalEye2 : ModNPC
	{
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("海洋封印之眼");
			Main.npcFrameCount[base.NPC.type] = 1;
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "海洋封印之眼");
		}
        private int a = 0;
        private bool down = true;
        private bool initialization = true;
        private bool flag2s = true;
        private bool flag1 = false;
        private bool flag2 = false;
        private bool flag3 = false;
        private bool flag4 = false;
        private bool flag5 = false;
        private bool canDespawn;
		public override void SetDefaults()
		{
			base.NPC.noGravity = true;
			base.NPC.damage = 75;
			base.NPC.width = 28;
			base.NPC.height = 28;
			base.NPC.defense = 50;
			base.NPC.lifeMax = (Main.expertMode ? 4500 : 3750);
            if(Main.masterMode)
            {
                base.NPC.lifeMax = 2750;
            }
			base.NPC.alpha = 0;
			base.NPC.aiStyle = -1;
			this.AIType = -1;
            base.NPC.knockBackResist = 0f;
            base.NPC.boss = false;
            base.NPC.noGravity = true;
            base.NPC.noTileCollide = true;
			base.NPC.value = (float)Item.buyPrice(0, 0, 0, 0);
			base.NPC.HitSound = SoundID.NPCHit25;
            base.NPC.scale =1;
        }
        private float z = 0;
        public override void AI()
        {
            if(z == 0)
            {
                z = Main.rand.NextFloat(0.0f, 10.0f);
            }
            z += 0.05f;
            if(!flag1)
            {
                NPC.localAI[0] = Main.rand.Next(0,80);
                flag1 = true;
            }
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            Player player = Main.player[base.NPC.target];
            bool zoneUnderworldHeight = player.ZoneUnderworldHeight;
            base.NPC.TargetClosest(true);
            Vector2 center = base.NPC.Center;
            float num = player.Center.X - center.X;
            float num2 = player.Center.Y - center.Y;
            float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
            NPC.localAI[0] += 1;
            int num5 = (int)Player.FindClosest(base.NPC.Center, 1, 1);
            num2 += 1;
            Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, base.NPC.velocity.X * 0.5f, base.NPC.velocity.Y * 0.5f, 0, default(Color), 1f);
            float num6 = (float)Math.Sqrt((Main.player[num5].Center.X - base.NPC.Center.X) * (Main.player[num5].Center.X - base.NPC.Center.X) + (Main.player[num5].Center.Y - base.NPC.Center.Y) * (Main.player[num5].Center.Y - base.NPC.Center.Y));
            base.NPC.rotation -= (float)Math.Sqrt((float)NPC.velocity.X * (float)NPC.velocity.X + (float)NPC.velocity.Y * (float)NPC.velocity.Y) * 0.003f;
            if (num2 % 200 > 100 && num6 < 600f)
            {
                if (Math.Abs(base.NPC.velocity.X) + Math.Abs(base.NPC.velocity.Y) < 5f)
                {
                    base.NPC.velocity *= 1.2f;
                }
                if (Math.Abs(base.NPC.velocity.X) + Math.Abs(base.NPC.velocity.Y) > 6f)
                {
                    base.NPC.velocity *= 0.96f;
                }
                if (num > 150)
                {
                    num -= Main.rand.Next(0, 5);
                }
                else if (num < -150)
                {
                    num += Main.rand.Next(0, 5);
                }
                else
                {
                    num += Main.rand.Next(-5, 5);
                }
                NPC.velocity = NPC.velocity.RotatedBy(Math.PI * num / 10000f);
            }
            else
            {
                if (Math.Abs(base.NPC.velocity.X) + Math.Abs(base.NPC.velocity.Y) < 5f)
                {
                    base.NPC.velocity *= 1.2f;
                }
                if (Math.Abs(base.NPC.velocity.X) + Math.Abs(base.NPC.velocity.Y) > 6f)
                {
                    base.NPC.velocity *= 0.96f;
                }
                NPC.velocity = NPC.velocity * 0.96f + (Main.player[num5].Center - base.NPC.Center) / num6 * 0.25f;
            }
            if(NPC.localAI[0] % 75 == 0)
            {
                Vector2 v = (player.Center - (NPC.Center)) * (13 / num3);
                Projectile.NewProjectile(NPC.GetSource_FromAI(), base.NPC.Center.X, base.NPC.Center.Y, v.X, v.Y,ModContent.ProjectileType<Everglow.Ocean.Projectiles.蔚蓝射线>(), (int)(base.NPC.damage * 0.2f), 0.2f, Main.myPlayer, 0f, 0f);
            }
            if (Main.rand.Next(300) == 1)
            {
                for(int u = 0; u < 40; u++)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, base.NPC.velocity.X * 0.5f, base.NPC.velocity.Y * 0.5f, 0, default(Color), 2f);
                }
                NPC.position += (player.Center - NPC.Center).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
                for (int u = 0; u < 40; u++)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, base.NPC.velocity.X * 0.5f, base.NPC.velocity.Y * 0.5f, 0, default(Color), 2f);
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
            vector2 -= new Vector2((float)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/OceanCrystal/海洋封印之眼2Glow").Width(), (float)(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/OceanCrystal/海洋封印之眼2Glow").Height() / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(297 - base.NPC.alpha, 297 - base.NPC.alpha, 297 - base.NPC.alpha, 0), Color.Blue);
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/OceanCrystal/海洋封印之眼2Glow"), vector2, new Rectangle?(base.NPC.frame), color, z, vector, 1f, effects, 0f);
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/OceanCrystal/海洋封印之眼2Glow"), vector2, new Rectangle?(base.NPC.frame), color, -z, vector, 1f, effects, 0f);
        }
        public override bool CheckActive()
		{
			return this.canDespawn;
		}
	}
}
