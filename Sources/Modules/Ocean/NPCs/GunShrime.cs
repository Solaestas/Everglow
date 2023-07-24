using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs
{
	public class GunShrime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("枪虾");
			Main.npcFrameCount[base.npc.type] = 4;
			base.DisplayName.AddTranslation(GameCulture.Chinese, "枪虾");
		}
		public override void SetDefaults()
		{
			base.npc.damage = 0;
			base.npc.width = 46;
			base.npc.height = 18;
			base.npc.defense = 0;
			base.npc.lifeMax = 10;
			base.npc.aiStyle = 3;
			this.aiType = 67;
			base.npc.value = (float)Item.buyPrice(0, 1, 0, 0);
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath1;
			base.npc.buffImmune[189] = true;
			this.banner = base.npc.type;
			this.bannerItem = base.mod.ItemType("BombshrimpBanner");
		}
		public override void AI()
		{
            npc.localAI[0] += 1;
			base.npc.spriteDirection = ((base.npc.direction > 0) ? -1 : 1);
			float num = (Main.player[base.npc.target].Center - base.npc.Center).Length();
			num *= 0.0025f;
			if ((double)num > 1.5)
			{
				num = 1.5f;
			}
			float num2;
			if (Main.expertMode)
			{
				num2 = 3f - num;
			}
			else
			{
				num2 = 2.6f - num;
			}
			if (base.npc.velocity.X < -num2 || base.npc.velocity.X > num2)
			{
				if (base.npc.velocity.Y == 0f)
				{
					base.npc.velocity *= 0.8f;
					return;
				}
			}
			else if (base.npc.velocity.X < num2 && base.npc.direction == 1)
			{
				base.npc.velocity.X = base.npc.velocity.X + 1f;
				if (base.npc.velocity.X > num2)
				{
					base.npc.velocity.X = num2;
					return;
				}
			}
			else if (base.npc.velocity.X > -num2 && base.npc.direction == -1)
			{
				base.npc.velocity.X = base.npc.velocity.X - 1f;
				if (base.npc.velocity.X < -num2)
				{
					base.npc.velocity.X = -num2;
				}
			}
            int num5 = (int)Player.FindClosest(base.npc.Center, 1, 1);
            if ((Main.player[num5].Center - base.npc.Center).Length() < 200f && npc.localAI[0] % 90 == 0)
            {
                Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/烟花爆炸"), (int)base.npc.Center.X + base.npc.direction * 16, (int)base.npc.Center.Y - 4);
                int num3 = Projectile.NewProjectile(base.npc.Center.X + base.npc.direction * 16f, base.npc.Center.Y - 4f, 0, 0, base.mod.ProjectileType("CrackSoundWave"), 150, 1.2f, Main.myPlayer, 0, 0f);
                Main.projectile[num3].hostile = true;
                Main.projectile[num3].friendly = false;
            }
		}
		public override void OnHitPlayer(Player player, int damage, bool crit)
		{
			if (Main.netMode != 1)
			{
				Projectile.NewProjectile(player.Center.X, player.Center.Y, 0f, 0f, 612, 0, 0f, Main.myPlayer, 0f, 0f);
			}
		}
		public override void FindFrame(int frameHeight)
		{
			base.npc.frameCounter += 0.15000000596046448;
			base.npc.frameCounter %= (double)Main.npcFrameCount[base.npc.type];
			int num = (int)base.npc.frameCounter;
			base.npc.frame.Y = num * frameHeight;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            if (spawnInfo.playerSafe)
            {
                return 0f;
            }
            if (spawnInfo.player.GetModPlayer<MythPlayer>().ZoneOcean && spawnInfo.water)
            {
                return 0.1f;
            }
            else
            {
                return 0f;
            }
        }
		public override void NPCLoot()
		{
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("VoidBubble"), 1, false, 0, false, false);
            }
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("BladeScale"), 1, false, 0, false, false);
            }
            if (Main.rand.Next(150) == 0 && Main.hardMode)
            {
                Item.NewItem((int)base.npc.position.X, (int)base.npc.position.Y, base.npc.width, base.npc.height, base.mod.ItemType("PistolShrimpPlier"), 1, false, 0, false, false);
			}
		}
	}
}
