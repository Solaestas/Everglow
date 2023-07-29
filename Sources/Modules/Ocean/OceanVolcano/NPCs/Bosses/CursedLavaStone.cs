using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs.VolCano
{
    [AutoloadBossHead]
    public class CursedLavaStone : ModNPC
	{
		private int num1 = 0;
		private int num2;
		public override void SetStaticDefaults()
		{
            // // base.DisplayName.SetDefault("诅咒熔岩巨石怪");
            // base.// DisplayName.AddTranslation(GameCulture.Chinese, "诅咒熔岩巨石怪");
		}
		public override void SetDefaults()
		{
			base.NPC.damage = 120;
			base.NPC.width = 100;
			base.NPC.height = 100;
			base.NPC.defense = 120;
			base.NPC.lifeMax = 30000;
			base.NPC.knockBackResist = 0;
			base.NPC.alpha = 0;
			base.NPC.lavaImmune = true;
			base.NPC.noGravity = true;
			base.NPC.noTileCollide = true;
            base.NPC.aiStyle = -1;
            NPC.dontTakeDamage = true;
            NPC.boss = false;
        }
        private int Xa = 0;
        public override void AI()
        {
            if(Xa == 0)
            {
                Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, 0, 0, ModContent.ProjectileType<Everglow.Ocean.Projectiles.MagicHalo>(), 0, 0, Main.myPlayer, 10, 0f);
            }
            Xa += 1;
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            Player player = Main.player[Main.myPlayer];
            Vector2 playerposition = Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2);
            Vector2 vector = new Vector2(base.NPC.position.X + (float)base.NPC.width * 0.5f, base.NPC.position.Y + (float)base.NPC.height * 0.5f);
            if (player.behindBackWall && Main.tile[(int)(player.Center.X / 16), (int)(player.Center.Y / 16)].WallType == Mod.Find<ModWall>("熔岩石墙").Type && mplayer.ZoneVolcano)
            {
                Vector2 v3 = new Vector2(300, 0).RotatedBy(Xa / 200f);
                float num4 = player.Center.X + v3.X - vector.X;
                float num5 = player.Center.Y + v3.Y - vector.Y;
                float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                NPC.velocity += new Vector2(num4, num5) / num6 * 0.04f * ((NPC.Center - player.Center - v3).Length() / 300 + 1);
                if (NPC.velocity.Length() < 4)
                {
                    NPC.velocity *= 1.01f;
                }
                if (NPC.velocity.Length() > 10)
                {
                    NPC.velocity *= 0.98f;
                }
                NPC.damage = 120;
                if(Xa < 1200 && Xa % 60 == 0)
                {
                    for(int o = 0;o < 15;o++)
                    {
                        Vector2 v = new Vector2(0, 8).RotatedBy(Math.PI * 2 / 15 * o);
                        Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, v.X, v.Y, 467, 100, 0, Main.myPlayer, 10, 0f);
                    }
                }
                if (Xa >= 1200 && Xa % 4 == 0 && Xa < 2400)
                {
                    Vector2 v = new Vector2(0, 8).RotatedBy(Math.PI * 2 / 60 * Xa);
                    Projectile.NewProjectile(NPC.Center.X, NPC.Center.Y, v.X, v.Y, 467, 100, 0, Main.myPlayer, 10, 0f);
                }
                if(Xa > 2400)
                {
                    Xa = 1;
                }
            }
            else
            {
                float num4 = player.position.X - vector.X;
                float num5 = player.position.Y - vector.Y;
                float num6 = (float)Math.Sqrt((double)(num4 * num4 + num5 * num5));
                NPC.velocity += new Vector2(num4, num5) / num6 * 0.1f;
                if (NPC.velocity.Length() < 15)
                {
                    NPC.velocity *= 1.05f;
                }
                if (NPC.velocity.Length() > 17)
                {
                    NPC.velocity *= 0.95f;
                }
                NPC.velocity.Y += 1f;
                NPC.damage = 120;
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
            vector2 -= new Vector2((float)base.Mod.GetTexture("NPCs/VolCano/熔岩巨石怪2Glow").Width, (float)(base.Mod.GetTexture("NPCs/VolCano/熔岩巨石怪2Glow").Height / Main.npcFrameCount[base.NPC.type])) * 1f / 2f;
            vector2 += vector * 1f + new Vector2(0f, 4f + base.NPC.gfxOffY);
            Color color = Utils.MultiplyRGBA(new Color(97 - base.NPC.alpha, 97 - base.NPC.alpha, 97 - base.NPC.alpha, 0), Color.White);
            Main.spriteBatch.Draw(base.Mod.GetTexture("NPCs/VolCano/熔岩巨石怪2Glow"), vector2, new Rectangle?(base.NPC.frame), color, base.NPC.rotation, vector, 1f, effects, 0f);
        }
		public override void HitEffect(NPC.HitInfo hit)
        {
			SoundEngine.PlaySound(SoundID.Item10, new Vector2(base.NPC.position.X, base.NPC.position.Y));
            for (int i = 0; i < 5; i++)
            {
                Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 54, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            for (int j = 0; j < 3; j++)
            {
                Dust.NewDust(base.NPC.position, base.NPC.width, base.NPC.height, 6, (float)hitDirection, -1f, 0, default(Color), 1f);
            }
            if (base.NPC.life <= 0)
            {
                for (int i = 0; i < 10; i++)
                {
                    float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                    Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor5, base.Mod.GetGoreSlot("Gores/火山浮石碎块" + (i % 5 + 1).ToString()), 1f);
                }
                for (int i = 0; i < 5; i++)
                {
                    float scaleFactor5 = (float)(Main.rand.Next(-20, 20) / 100f);
                    Gore.NewGore(base.NPC.position, base.NPC.velocity * scaleFactor5, base.Mod.GetGoreSlot("Gores/火山浮石碎块" + (i % 2 + 6).ToString()), 1f);
                    NPC.NewNPC((int)NPC.Center.X, (int)NPC.Center.Y, Mod.Find<ModNPC>("火山浮石").Type, 0, 0, Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.NextFloat(-0.15f, 0.15f), Main.rand.Next(0, 60), 255);
                }
            }
            if(NPC.CountNPCS(Mod.Find<ModNPC>("LavaStone2").Type) <= 1)
            {
                if (!MythWorld.downedVol)
                {
                    for (int i = (int)(Main.maxTilesX * 0.62f + 445); i < (int)(Main.maxTilesX * 0.62f + 455); i++)
                    {
                        for (int j = (int)(Main.maxTilesY * 0.67f - 80); j < (int)(Main.maxTilesY * 0.67f - 60); j++)
                        {
                            if (Main.tile[i, j].TileType == Mod.Find<ModTile>("熔岩石").Type)
                            {
                                WorldGen.KillTile(i, j, false, false, true);
                            }
                        }
                    }
                    MythWorld.downedVol = true;
                }
            }
        }
        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
		}
	}
}
