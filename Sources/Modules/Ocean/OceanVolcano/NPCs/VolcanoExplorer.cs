using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Events;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.NPCs.TownNPCs
{
	[AutoloadHead]
	public class VolcanoExplorer : ModNPC
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("VolcanoExplorer");
			Main.npcFrameCount[base.npc.type] = 23;
			NPCID.Sets.ExtraFramesCount[base.npc.type] = 9;
			NPCID.Sets.AttackFrameCount[base.npc.type] = 4;
			NPCID.Sets.DangerDetectRange[base.npc.type] = 400;
			NPCID.Sets.AttackType[base.npc.type] = 0;
			NPCID.Sets.AttackTime[base.npc.type] = 60;
			NPCID.Sets.AttackAverageChance[base.npc.type] = 15;
			base.DisplayName.AddTranslation(GameCulture.Chinese, "火山探险家");
		}
		public override void SetDefaults()
		{
			base.npc.townNPC = true;
			base.npc.friendly = true;
			base.npc.width = 18;
			base.npc.height = 40;
			base.npc.aiStyle = 7;
			base.npc.damage = 10;
			base.npc.defense = 15;
			base.npc.lifeMax = 250;
			base.npc.HitSound = SoundID.NPCHit1;
			base.npc.DeathSound = SoundID.NPCDeath6;
			base.npc.knockBackResist = 0.5f;
			this.animationType = 22;
		}

		public override bool CanTownNPCSpawn(int numTownNPCs, int money)
		{
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            if (mplayer.ZoneVolcano)
            {
                return true;
            }
			return false;
		}

		public override string TownNPCName()
		{
            string [] npcName = new string[12];
            npcName[1] = "Brown";
            npcName[2] = "Clark";
            npcName[3] = "Davis";
            npcName[4] = "Harris";
            npcName[5] = "Jackson";
            npcName[6] = "Lewis";
            npcName[7] = "Miller";
            npcName[8] = "Smith";
            npcName[9] = "Thomas";
            npcName[10] = "Wilson";
            npcName[11] = "Adams";
            npcName[12] = "Alexander";
            return npcName[Main.rand.Next(1, 13)];
		}

		public override string GetChat()
		{
			if (npc.homeless)
			{
				if (Main.rand.Next(2) == 0)
				{
					return "这年头火山还挺平静";
				}
				return "看这到处都有硫磺,好东西";
			}
			else if (Main.bloodMoon)
			{
				int num = Main.rand.Next(4);
				if (num == 0)
				{
					return "按照海盗的说法,血红的月亮,又将有一场浩劫";
				}
				if (num == 1)
				{
					return "科学的解释是因为地影投影到月亮上,加上大气透镜,月亮就变红了";
				}
				if (num == 2)
				{
					return "周围的气氛有点紧张";
				}
				Main.player[Main.myPlayer].Hurt(PlayerDeathReason.ByOther(4), Main.player[Main.myPlayer].statLife / 2, -Main.player[Main.myPlayer].direction, false, false, false, -1);
				return "赶紧搭好帐篷吧";
			}
			else
			{
				IList<string> list = new List<string>();
				if (Main.dayTime)
				{
					list.Add("山腰上的那个矿脉看上去不错");
					list.Add("我在这发现了很多神奇的东西");
					list.Add("火山很危险,时刻小心");
					list.Add("地下可以找到橄榄石");
					list.Add("我还不敢下去火山口深处,据说那是另外一个世界");
					list.Add("天气好热,你有冰棍吗，" + Main.player[Main.myPlayer].name);
				}
				else
				{
                    list.Add("这里物资很充足,可以度过很多个晚上了");
                    list.Add("海岛的星空真美,不是吗");
                }
				return list[Main.rand.Next(list.Count)];
			}
		}
		public override void SetChatButtons(ref string button, ref string button2)
		{
			button = Language.GetTextValue("商店");
			button2 = Language.GetTextValue("关闭");
		}
		public override void OnChatButtonClicked(bool firstButton, ref bool shop)
		{
			if (firstButton)
			{
				shop = true;
				return;
			}
			shop = false;
		}
		public override void SetupShop(Chest shop, ref int nextSlot)
		{
			shop.item[nextSlot].SetDefaults(base.mod.ItemType("StarMark"), false);
			shop.item[nextSlot].shopCustomPrice = new int?(Item.buyPrice(0, 70, 0, 0));
			nextSlot++;
		}
		public override void TownNPCAttackStrength(ref int damage, ref float knockback)
		{
			damage = 15;
			knockback = 2f;
		}
		public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
		{
			cooldown = 180;
			randExtraCooldown = 60;
		}
		public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
		{
			//projType = base.mod.ProjectileType("Alpenstock");
			//attackDelay = 1;
		}
		public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
		{
			multiplier = 2f;
		}
	}
}
