using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using System;
using Microsoft.Xna.Framework;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ID;
using Terraria.Localization;
using Ionic.Zlib;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Terraria.Chat;
using Terraria.GameContent.Events;
using Terraria.GameContent.NetModules;
using Terraria.GameContent.Tile_Entities;
using Terraria.IO;
using Terraria.Net;
using Terraria.Net.Sockets;
using Terraria.Social;


namespace Everglow.Ocean.Common
{
	public class OceanContentPlayer : ModPlayer
	{
		public int Waveshader = 0;
		public int Zonefloor = 0;
		public int Maxfloor = 1;
		public bool OceanCatch = false;
		public bool firstClick;

		public bool ZoneOcean;
		public bool ZoneOceanTown;
		public bool ZoneVolcano;
		public bool ZoneOceanDeep;
		public bool ZoneCoral;
		public string worldName = "";
		public bool create = false;
		public override void Initialize()
		{
			this.LavaCryst = 0;
			this.FinalLava = false;
		}
		public override void ResetEffects()
		{
			this.LargeTurquoise = false;
			this.LargeAquamarine = false;
			this.LargeOlivine = false;
		}
		public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
		{
			//Texture2D texture = base.mod.GetTexture("UIImages/Life");
			//Main.heartTexture = texture;
			//Main.heart2Texture = base.mod.GetTexture("UIImages/Life2");
		}
		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			if (Player.statLifeMax2 <= 200)
			{
				Player.statLife = 100;
			}
			else
			{
				Player.statLife = Player.statLifeMax2 / 2;
			}
			if (Player.name == "万象元素")
			{
				Player.active = true;
				Player.dead = false;
				Player.statLife = Player.statLifeMax2;
				return false;
			}
			else
			{
				return true;
			}
		}

		public bool FinalLava;
		public bool Ost = true;

		public int lavaI = 0;
		public bool BanTra = false;
		public bool BanTraBall = true;
		//123456

		public override void CatchFish(FishingAttempt attempt, ref int itemDrop, ref int npcSpawn, ref AdvancedPopupRequest sonar, ref Vector2 sonarPosition)
		{
			//OceanContentPlayer ocplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
			//if (ocplayer.ZoneVolcano)
			//{
			//	flag = true;
			//}
			//if (ocplayer.ZoneOcean)
			//{
			//	flag1 = true;
			//}
			//if (junk && flag1 && poolSize >= 150)
			//{
			//	if (power < 40)
			//	{
			//		switch (Main.rand.Next(3))
			//		{
			//			case 0:
			//				caughtType = ModContent.ItemType<Everglow.Ocean.Items.锈铁剑>();
			//				break;
			//			case 1:
			//				caughtType = ModContent.ItemType<Everglow.Ocean.Items.马尾藻>();
			//				break;
			//			case 2:
			//				caughtType = ModContent.ItemType<Everglow.Ocean.Items.空瓶>();
			//				break;
			//		}
			//	}
			//	return;
			//}
			//if (power >= 20 && power <= 40 && poolSize >= 150)
			//{
			//	if (Main.rand.Next(15) == 0 && power < 80)
			//	{
			//		switch (Main.rand.Next(3))
			//		{
			//			case 0:
			//				caughtType = ModContent.ItemType<Everglow.Ocean.Items.锈铁剑>();
			//				break;
			//			case 1:
			//				caughtType = ModContent.ItemType<Everglow.Ocean.Items.马尾藻>();
			//				break;
			//			case 2:
			//				caughtType = ModContent.ItemType<Everglow.Ocean.Items.空瓶>();
			//				break;
			//		}
			//	}
			//	if (power >= 60 && poolSize >= 150 && questFish == ModContent.ItemType<Everglow.Ocean.Items.鱿鱼>())
			//	{
			//		caughtType = ModContent.ItemType<Everglow.Ocean.Items.鱿鱼>();
			//		if (power >= 70 && Main.rand.Next(25) == 0 && power < 440)
			//		{
			//			caughtType = ModContent.ItemType<Everglow.Ocean.Items.发光磷虾>();
			//		}
			//		if (power >= 80)
			//		{
			//			if (power >= 70 && Main.rand.Next(25) == 0 && power < 240)
			//			{
			//				caughtType = ModContent.ItemType<Everglow.Ocean.Items.电鳐>();
			//			}
			//		}
			//	}
			//}
		}
		public override void OnRespawn()
		{
			if (ZoneOcean || ZoneVolcano || ZoneOceanDeep || ZoneCoral)
			{
				Player.SpawnX = 160;
				Player.SpawnY = (int)((Main.maxTilesY / 10f + 60) * 16f);
				Player.FindSpawn();
			}
		}
		public override void UpdateDead()
		{
			if (ZoneOcean || ZoneVolcano || ZoneOceanDeep || ZoneCoral)
			{
				Player.SpawnX = 160;
				Player.SpawnY = (int)((Main.maxTilesY / 10f + 60) * 16f);
				Player.FindSpawn();
			}
			OceanContentPlayer ocplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
			if (Player.name == "万象元素")
			{
				Player.Spawn(context: PlayerSpawnContext.ReviveFromDeath);
				Player.respawnTimer = 0;
				if (Player.statLifeMax2 <= 200)
				{
					Player.statLife = 100;
				}
				else
				{
					Player.statLife = Player.statLifeMax2 / 2;
				}
				Player.immuneTime = 120;
			}
		}
		public static float FeatherCount = 0;
		public override void PostUpdateMiscEffects()
		{
			if (ZoneVolcano && !FinalLava)
			{
				if (LavaCryst <= 60)
				{
					Player.statLifeMax2 += LavaCryst * 5;
				}
				else
				{
					Player.statLifeMax2 += 300;
				}
			}
			if (FinalLava)
			{
				if (LavaCryst <= 60)
				{
					Player.statLifeMax2 += LavaCryst * 5;
				}
				else
				{
					Player.statLifeMax2 += 300;
				}
			}
			//Texture2D texture2 = base.mod.GetTexture("UIImages/Mana");
			//Main.manaTexture = texture2;

			//Texture2D texture3 = base.mod.GetTexture("UIImages/MiniMapFrame");
			//Main.miniMapFrameTexture = texture3;
			//Texture2D texture4 = base.mod.GetTexture("UIImages/MiniMapFrame2");
			//Main.miniMapFrame2Texture = texture4;
			//Texture2D texture5 = base.mod.GetTexture("UIImages/Text_Back");
			//Main.textBackTexture = texture5;
			//Texture2D texture6 = base.mod.GetTexture("UIImages/Map");
			//Main.mapTexture = texture6;
			//Texture2D texture7 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back");
			//Main.inventoryBackTexture = texture7;
			//Texture2D texture8 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back2");
			// Main.inventoryBack2Texture = texture8;
			//Texture2D texture9 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back3");
			// Main.inventoryBack3Texture = texture9;
			//Texture2D texture10 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back4");
			//Main.inventoryBack4Texture = texture10;
			//Texture2D texture11 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back5");
			//Main.inventoryBack5Texture = texture11;
			//Texture2D texture12 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back6");
			//Main.inventoryBack6Texture = texture12;
			//Texture2D texture13 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back7");
			//Main.inventoryBack7Texture = texture13;
			//Texture2D texture14 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back8");
			//Main.inventoryBack8Texture = texture14;
			//Texture2D texture15 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back9");
			//Main.inventoryBack9Texture = texture15;
			// Texture2D texture16 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back10");
			//Main.inventoryBack10Texture = texture16;
			//Texture2D texture17 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back11");
			//Main.inventoryBack11Texture = texture17;
			//Texture2D texture18 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back12");
			//Main.inventoryBack12Texture = texture18;
			//Texture2D texture19 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back13");
			//Main.inventoryBack13Texture = texture19;
			//Texture2D texture20 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back14");
			//Main.inventoryBack14Texture = texture20;
			//Texture2D texture21 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back15");
			//Main.inventoryBack15Texture = texture21;
			//Texture2D texture22 = base.mod.GetTexture("UIImages/皮肤1/Inventory_Back16");
			//Main.inventoryBack16Texture = texture22;
		}
		public override void SaveData(TagCompound tag)
		{
			List<string> list = new List<string>();
			TagCompound tagCompound = new TagCompound();
			tagCompound.Add("boost", list);
			tagCompound.Add("Lav2", LavaCryst);
			tagCompound.Add("FLav", FinalLava);
		}
		public override void LoadData(TagCompound tag)
		{
			string[] S = new string[40];
			int[] I = new int[40];
			LavaCryst = tag.GetInt("Lav2");
			FinalLava = tag.GetBool("FLav");
			IList<string> list = tag.GetList<string>("boost");
		}
		//123456
		public int FlameShield = 0;
		private int FlameShieldCool = 0;
		public override bool CanBeHitByNPC(NPC npc, ref int cooldownSlot)
		{
			return base.CanBeHitByNPC(npc, ref cooldownSlot);
		}
		public override void Kill(double damage, int hitDirection, bool pvp, PlayerDeathReason damageSource)
		{
			if (this.LargeTurquoise)
			{
				int num = base.Player.FindItem(ModContent.ItemType<Everglow.Ocean.Items.LargeTurquoise>());
				if (num >= 0)
				{
					base.Player.inventory[num].stack--;
					Item.NewItem(null, (int)base.Player.position.X, (int)base.Player.position.Y, base.Player.width, base.Player.height, ModContent.ItemType<Everglow.Ocean.Items.LargeTurquoise>(), 1, false, 0, false, false);
					if (base.Player.inventory[num].stack <= 0)
					{
						base.Player.inventory[num] = new Item();
					}
				}
			}
			if (this.LargeAquamarine)
			{
				int num = base.Player.FindItem(ModContent.ItemType<Everglow.Ocean.Items.LargeAquamarine>());
				if (num >= 0)
				{
					base.Player.inventory[num].stack--;
					Item.NewItem((int)base.Player.position.X, (int)base.Player.position.Y, base.Player.width, base.Player.height, ModContent.ItemType<Everglow.Ocean.Items.LargeAquamarine>(), 1, false, 0, false, false);
					if (base.Player.inventory[num].stack <= 0)
					{
						base.Player.inventory[num] = new Item();
					}
				}
			}
			if (this.LargeOlivine)
			{
				int num = base.Player.FindItem(ModContent.ItemType<Everglow.Ocean.Items.LargeOlivine>());
				if (num >= 0)
				{
					base.Player.inventory[num].stack--;
					Item.NewItem((int)base.Player.position.X, (int)base.Player.position.Y, base.Player.width, base.Player.height, ModContent.ItemType<Everglow.Ocean.Items.LargeOlivine>(), 1, false, 0, false, false);
					if (base.Player.inventory[num].stack <= 0)
					{
						base.Player.inventory[num] = new Item();
					}
				}
			}
			if (Player.name == "万象元素")
			{
				Player.active = true;
				Player.dead = false;
				Player.statLife = Player.statLifeMax2;
			}
		}
		public bool LargeTurquoise;
		public bool LargeAquamarine;
		public bool LargeOlivine;
		public int LavaCryst;
		public int floor = 1;
		public int aimFloor = 1;

	}
}