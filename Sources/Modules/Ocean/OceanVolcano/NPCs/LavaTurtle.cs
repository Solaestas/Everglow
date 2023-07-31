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
    public class LavaTurtle : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("lava tortoise");
			Main.npcFrameCount[base.NPC.type] = 8;
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "熔岩陆龟");
		}
		public override void SetDefaults()
		{
			base.NPC.aiStyle = 39;
            base.NPC.damage = 219;
			base.NPC.width = 72;
			base.NPC.height = 48;
			base.NPC.defense = 140;
			base.NPC.lifeMax = 2800;
			base.NPC.knockBackResist = 0f;
			this.AnimationType = 39;
			base.NPC.value = (float)Item.buyPrice(0, 0, 3, 0);
			base.NPC.color = new Color(0, 0, 0, 0);
			base.NPC.lavaImmune = true;
			base.NPC.noGravity = true;
			base.NPC.noTileCollide = false;
			base.NPC.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath1;
			//this.banner = base.npc.type;
			//this.bannerItem = ModContent.ItemType<Everglow.Ocean.Items.熔岩陆龟Banner>();
		}
        public override void AI()
        {
			if(base.NPC.velocity.Y >= 5f)
			{
			    base.NPC.velocity.Y = 5f;
			}
			else
			{
				base.NPC.velocity.Y += 0.15f;
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
            vector2 -= new Vector2((float)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/熔岩陆龟Glow").Width(), (float)(ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/熔岩陆龟Glow").Height() / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = new Color(0.4f, 0.4f, 0.4f, 0);
            Main.spriteBatch.Draw((Texture2D)ModContent.Request<Texture2D>("Everglow/Ocean/NPCs/熔岩陆龟Glow"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
        }
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (spawnInfo.PlayerSafe)
			{
				return 0f;
			}
			if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneVolcano && !spawnInfo.Water)
			{
				return 5f;
			}
			else
            {
                return 0f;
            }
		}
        public override void HitEffect(NPC.HitInfo hit)
        {
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            if (base.NPC.life <= 0)
            {
                for (int j = 0; j < 20; j++)
                {
                    Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 59, (float)hitDirection, -1f, 0, default(Color), 1f);
                }
            }
            if (base.NPC.life <= 0)
            {
                float scaleFactor = (float)(Main.rand.Next(-8, 8) / 100f);
                Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor, base.Mod.GetGoreSlot("Gores/熔岩陆龟碎块"), 1f);
            }
        }
        public override void OnKill()
        {
            if (Main.rand.Next(100) == 1)
            {
                Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.VolcanoBlade>(), 1, false, 0, false, false);
            }
            Item.NewItem((int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.Items.LavaStone>(), Main.rand.Next(2, 5), false, 0, false, false);
        }
    }
}
