using System;
using Everglow.Ocean.Common;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.NPCs
{
	public class GunShrime : ModNPC
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("枪虾");
			Main.npcFrameCount[base.NPC.type] = 4;
			// base.// DisplayName.AddTranslation(GameCulture.Chinese, "枪虾");
		}
		public override void SetDefaults()
		{
			base.NPC.damage = 0;
			base.NPC.width = 46;
			base.NPC.height = 18;
			base.NPC.defense = 0;
			base.NPC.lifeMax = 10;
			base.NPC.aiStyle = 3;
			this.AIType = 67;
			base.NPC.value = (float)Item.buyPrice(0, 1, 0, 0);
			base.NPC.HitSound = SoundID.NPCHit1;
			base.NPC.DeathSound = SoundID.NPCDeath1;
			base.NPC.buffImmune[189] = true;
			this.Banner = base.NPC.type;
			this.BannerItem = ModContent.ItemType<Everglow.Ocean.Items.BombshrimpBanner>();
		}
		public override void AI()
		{
            NPC.localAI[0] += 1;
			base.NPC.spriteDirection = ((base.NPC.direction > 0) ? -1 : 1);
			float num = (Main.player[base.NPC.target].Center - base.NPC.Center).Length();
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
			if (base.NPC.velocity.X < -num2 || base.NPC.velocity.X > num2)
			{
				if (base.NPC.velocity.Y == 0f)
				{
					base.NPC.velocity *= 0.8f;
					return;
				}
			}
			else if (base.NPC.velocity.X < num2 && base.NPC.direction == 1)
			{
				base.NPC.velocity.X = base.NPC.velocity.X + 1f;
				if (base.NPC.velocity.X > num2)
				{
					base.NPC.velocity.X = num2;
					return;
				}
			}
			else if (base.NPC.velocity.X > -num2 && base.NPC.direction == -1)
			{
				base.NPC.velocity.X = base.NPC.velocity.X - 1f;
				if (base.NPC.velocity.X < -num2)
				{
					base.NPC.velocity.X = -num2;
				}
			}
            int num5 = (int)Player.FindClosest(base.NPC.Center, 1, 1);
            if ((Main.player[num5].Center - base.NPC.Center).Length() < 200f && NPC.localAI[0] % 90 == 0)
            {
                SoundEngine.PlaySound(new SoundStyle("Everglow/Ocean/Sounds/烟花爆炸"), (int)base.NPC.Center.X + base.NPC.direction * 16, (int)base.NPC.Center.Y - 4);
                int num3 = Projectile.NewProjectile(NPC.GetSource_FromAI(), base.NPC.Center.X + base.NPC.direction * 16f, base.NPC.Center.Y - 4f, 0, 0,ModContent.ProjectileType<Everglow.Ocean.Projectiles.Weapons.Other.CrackSoundWave>(), 150, 1.2f, Main.myPlayer, 0, 0f);
                Main.projectile[num3].hostile = true;
                Main.projectile[num3].friendly = false;
            }
		}
		public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
		{
			if (Main.netMode != 1)
			{
				Projectile.NewProjectile(null, target.Center.X, target.Center.Y, 0f, 0f, 612, 0, 0f, Main.myPlayer, 0f, 0f);
			}
		}
		public override void FindFrame(int frameHeight)
		{
			base.NPC.frameCounter += 0.15000000596046448;
			base.NPC.frameCounter %= (double)Main.npcFrameCount[base.NPC.type];
			int num = (int)base.NPC.frameCounter;
			base.NPC.frame.Y = num * frameHeight;
		}
		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
            if (spawnInfo.PlayerSafe)
            {
                return 0f;
            }
            if (spawnInfo.Player.GetModPlayer<OceanContentPlayer>().ZoneOcean && spawnInfo.Water)
            {
                return 0.1f;
            }
            else
            {
                return 0f;
            }
        }
		public override void OnKill()
		{
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.OceanDeep.Items.VoidBubble>(), 1, false, 0, false, false);
            }
            if (Main.rand.Next(3) == 0)
            {
                Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.OceanDeep.Items.BladeScale>(), 1, false, 0, false, false);
            }
            if (Main.rand.Next(150) == 0 && Main.hardMode)
            {
                Item.NewItem(NPC.GetSource_Death(), (int)base.NPC.position.X, (int)base.NPC.position.Y, base.NPC.width, base.NPC.height, ModContent.ItemType<Everglow.Ocean.OceanDeep.Items.Weapons.PistolShrimpPlier>(), 1, false, 0, false, false);
			}
		}
	}
}
