using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs.OceanCrystal
{
    [AutoloadBossHead]
    public class OceanCrystalEye2 : ModNPC
	{
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("海洋封印之眼");
			Main.npcFrameCount[base.npc.type] = 1;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "海洋封印之眼");
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
			base.npc.noGravity = true;
			base.npc.damage = 75;
			base.npc.width = 28;
			base.npc.height = 28;
			base.npc.defense = 50;
			base.npc.lifeMax = (Main.expertMode ? 4500 : 3750);
            if(MythWorld.Myth)
            {
                base.npc.lifeMax = 2750;
            }
			base.npc.alpha = 0;
			base.npc.aiStyle = -1;
			this.aiType = -1;
            base.npc.knockBackResist = 0f;
            base.npc.boss = false;
            base.npc.noGravity = true;
            base.npc.noTileCollide = true;
			base.npc.value = (float)Item.buyPrice(0, 0, 0, 0);
			base.npc.HitSound = SoundID.NPCHit25;
            base.npc.scale =1;
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
                npc.localAI[0] = Main.rand.Next(0,80);
                flag1 = true;
            }
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            Player player = Main.player[base.npc.target];
            bool zoneUnderworldHeight = player.ZoneUnderworldHeight;
            base.npc.TargetClosest(true);
            Vector2 center = base.npc.Center;
            float num = player.Center.X - center.X;
            float num2 = player.Center.Y - center.Y;
            float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
            npc.localAI[0] += 1;
            int num5 = (int)Player.FindClosest(base.npc.Center, 1, 1);
            num2 += 1;
            Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 59, base.npc.velocity.X * 0.5f, base.npc.velocity.Y * 0.5f, 0, default(Color), 1f);
            float num6 = (float)Math.Sqrt((Main.player[num5].Center.X - base.npc.Center.X) * (Main.player[num5].Center.X - base.npc.Center.X) + (Main.player[num5].Center.Y - base.npc.Center.Y) * (Main.player[num5].Center.Y - base.npc.Center.Y));
            base.npc.rotation -= (float)Math.Sqrt((float)npc.velocity.X * (float)npc.velocity.X + (float)npc.velocity.Y * (float)npc.velocity.Y) * 0.003f;
            if (num2 % 200 > 100 && num6 < 600f)
            {
                if (Math.Abs(base.npc.velocity.X) + Math.Abs(base.npc.velocity.Y) < 5f)
                {
                    base.npc.velocity *= 1.2f;
                }
                if (Math.Abs(base.npc.velocity.X) + Math.Abs(base.npc.velocity.Y) > 6f)
                {
                    base.npc.velocity *= 0.96f;
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
                npc.velocity = npc.velocity.RotatedBy(Math.PI * num / 10000f);
            }
            else
            {
                if (Math.Abs(base.npc.velocity.X) + Math.Abs(base.npc.velocity.Y) < 5f)
                {
                    base.npc.velocity *= 1.2f;
                }
                if (Math.Abs(base.npc.velocity.X) + Math.Abs(base.npc.velocity.Y) > 6f)
                {
                    base.npc.velocity *= 0.96f;
                }
                npc.velocity = npc.velocity * 0.96f + (Main.player[num5].Center - base.npc.Center) / num6 * 0.25f;
            }
            if(npc.localAI[0] % 75 == 0)
            {
                Vector2 v = (player.Center - (npc.Center)) * (13 / num3);
                Projectile.NewProjectile(base.npc.Center.X, base.npc.Center.Y, v.X, v.Y, base.mod.ProjectileType("蔚蓝射线"), (int)(base.npc.damage * 0.2f), 0.2f, Main.myPlayer, 0f, 0f);
            }
            if (Main.rand.Next(300) == 1)
            {
                for(int u = 0; u < 40; u++)
                {
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 59, base.npc.velocity.X * 0.5f, base.npc.velocity.Y * 0.5f, 0, default(Color), 2f);
                }
                npc.position += (player.Center - npc.Center).RotatedBy(Main.rand.NextFloat(-0.5f, 0.5f));
                for (int u = 0; u < 40; u++)
                {
                    Dust.NewDust(base.npc.position, base.npc.width, base.npc.height, 59, base.npc.velocity.X * 0.5f, base.npc.velocity.Y * 0.5f, 0, default(Color), 2f);
                }
            }
        }
        public override void PostDraw(SpriteBatch spriteBatch, Color drawColor)
        {
            SpriteEffects effects = SpriteEffects.None;
            if (base.npc.spriteDirection == 1)
            {
                effects = SpriteEffects.FlipHorizontally;
            }
            Vector2 value = new Vector2(base.npc.Center.X, base.npc.Center.Y);
            Vector2 vector = new Vector2((float)(Main.npcTexture[base.npc.type].Width / 2), (float)(Main.npcTexture[base.npc.type].Height / Main.npcFrameCount[base.npc.type] / 2));
            Vector2 vector2 = value - Main.screenPosition;
            vector2 -= new Vector2((float)base.mod.GetTexture("NPCs/OceanCrystal/海洋封印之眼2Glow").Width, (float)(base.mod.GetTexture("NPCs/OceanCrystal/海洋封印之眼2Glow").Height / Main.npcFrameCount[base.npc.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.npc.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(297 - base.npc.alpha, 297 - base.npc.alpha, 297 - base.npc.alpha, 0), Color.Blue);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/OceanCrystal/海洋封印之眼2Glow"), vector2, new Rectangle?(base.npc.frame), color, z, vector, 1f, effects, 0f);
            Main.spriteBatch.Draw(base.mod.GetTexture("NPCs/OceanCrystal/海洋封印之眼2Glow"), vector2, new Rectangle?(base.npc.frame), color, -z, vector, 1f, effects, 0f);
        }
        public override bool CheckActive()
		{
			return this.canDespawn;
		}
	}
}
