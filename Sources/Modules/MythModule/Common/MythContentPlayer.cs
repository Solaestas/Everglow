using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.Common
{
	public class MythContentPlayer : ModPlayer
	{
		public int GoldLiquidPupil = 0;
		public int WhitePedal = 0;
		public int ThreeColorCrown = 0;
		public int ThreeColorCrownCool = 0;
		public int ThreeColorCrownBuff1 = 0;
		public int ThreeColorCrownBuff2 = 0;
		public int ThreeColorCrownBuff3 = 0;
		public int OrangeStickCool = 0;
		public int OrangeStick = 0;
		public int PurpleBallFlower = 0;
		public int PurpleBallFlowerCool = 0;
		public int CyanPedal = 0;
		public int CyanBranch = 0;
		public int CyanBranchCool = 0;
		public int BlueTorchFlower = 0;
		public int BlueTorchFlowerTime = 0;
		public bool create1sand = false;
		public bool Turn = true;
		public int ImmuCurseF = 0;
		public int Dashcool = 0;
		public int RainCritAdd = 0;
		public float RainDamageAdd = 0;
		public float StackDamageAdd = 0;
		public int RainLifeAdd = 0;
		public float RainManaAdd = 0;
		public float RainManaAdd2 = 0;
		public float RainSpeedAdd = 0;
		public int RainSpeedAddTime = 0;
		public int RainDefenseAdd = 0;
		public float RainMissAdd = 0;
		public float Miss = 0;
		public int damage40 = 0;
		public float CritDamage = 1f;
		public int SilverBuff = 0;
		public float AddCritDamage = 0;
		public bool DefMoth;
		public bool DefTusk;
		public float AddDamagePoint = 0;

		public override bool PreKill(double damage, int hitDirection, bool pvp, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource)
		{
			return true;
		}
		internal void UpdateDecrease(ref int value)
		{
            if (value > 0)
            {
                value--;
            }
            else
            {
                value = 0;
            }
        }
		public override void PreUpdate()
		{
			UpdateDecrease(ref GoldLiquidPupil);
            UpdateDecrease(ref WhitePedal);
            UpdateDecrease(ref ThreeColorCrownBuff3);
            UpdateDecrease(ref ThreeColorCrownBuff2);
            UpdateDecrease(ref ThreeColorCrownBuff1);
            UpdateDecrease(ref ThreeColorCrownCool);
            UpdateDecrease(ref ThreeColorCrown);
            UpdateDecrease(ref OrangeStickCool);
            UpdateDecrease(ref OrangeStick);
            UpdateDecrease(ref PurpleBallFlowerCool);
            UpdateDecrease(ref PurpleBallFlower);
            UpdateDecrease(ref CyanBranchCool);
            UpdateDecrease(ref CyanBranch);
            UpdateDecrease(ref CyanPedal);
            UpdateDecrease(ref BlueTorchFlower);
            UpdateDecrease(ref BlueTorchFlowerTime);
            UpdateDecrease(ref CyanBranch);
            UpdateDecrease(ref damage40);
            UpdateDecrease(ref SilverBuff);
            UpdateDecrease(ref ImmuCurseF);
            UpdateDecrease(ref Dashcool);
            UpdateDecrease(ref IMMUNE);
            UpdateDecrease(ref RainSpeedAddTime);
            UpdateDecrease(ref SilverBuff);
            AddCritDamage = 0;	
			if (IMMUNE > 0)
			{
				IMMUNE--;
			}
			base.PreUpdate();
		}
		public override void PostUpdateMiscEffects()
		{
			int MaxS = -1;
			for (int p = 0; p < 58; p++)
			{
				if (Player.inventory[p].type == Player.HeldItem.type)
				{
					MaxS += 1;
				}
			}
			if (MaxS > 5)
			{
				MaxS = 5;
			}
			StackDamageAdd = MaxS * 0.05f;
			Miss = 0;
			CritDamage = 1f;
			Player.GetCritChance(DamageClass.Generic) += RainCritAdd;
			Player.GetDamage(DamageClass.Generic) *= RainDamageAdd + 1;
			Player.GetDamage(DamageClass.Generic) *= StackDamageAdd + 1;
			Miss += RainMissAdd;
			if (CyanPedal > 0)
			{
				Miss += 4;
			}
			if (WhitePedal > 0)
			{
				Miss += 2;
			}
			if (CyanBranchCool > 0)
			{
				Miss += 8;
			}
			Player.lifeRegen += RainLifeAdd;
			CritDamage += AddCritDamage;
			RainManaAdd2 = RainManaAdd;
			if (RainManaAdd2 > 10)
			{
				Player.statMana += 1;
				RainManaAdd2 -= 10;
				if (RainManaAdd2 > 10)
				{
					Player.statMana += 1;
					RainManaAdd2 -= 10;
					if (RainManaAdd2 > 10)
					{
						Player.statMana += 1;
						RainManaAdd2 -= 10;
						if (RainManaAdd2 > 10)
						{
							Player.statMana += 1;
							RainManaAdd2 -= 10;
							if (RainManaAdd2 > 10)
							{
								Player.statMana += 1;
								RainManaAdd2 -= 10;
								if (RainManaAdd2 > 10)
								{
									Player.statMana += 1;
									RainManaAdd2 -= 10;
									if (RainManaAdd2 > 10)
									{
										Player.statMana += 1;
										RainManaAdd2 -= 10;
									}
								}
							}
						}
					}
				}
			}
			if (Main.rand.Next(10) < RainManaAdd2)
			{
				Player.statMana += 1;
			}
			Player.statDefense += RainDefenseAdd;
			if (RainSpeedAddTime > 0)
			{
				Player.maxRunSpeed += RainSpeedAdd / 6f;
				Player.maxFallSpeed += RainSpeedAdd;
				Player.wingAccRunSpeed += RainSpeedAdd;
			}
			if (SilverBuff > 0)
			{
				Player.maxRunSpeed += 0.05f;
				Player.maxFallSpeed += 1.5f;
				Player.wingAccRunSpeed += 0.05f;
				Player.GetDamage(DamageClass.Generic) *= 1.05f;
			}
			if (Player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Melee.Hepuyuan.Hepuyuan>()] + Player.ownedProjectileCounts[ModContent.ProjectileType<MiscItems.Projectiles.Weapon.Melee.Hepuyuan.HepuyuanDown>()] > 0)
			{
				Player.maxFallSpeed += 10000f;
			}
		}
		public int IMMUNE = 0;
		public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
		{
			if (damage < 3)
			{
				return false;
			}
			if (OrangeStick > 0)
			{
				OrangeStickCool = 300;
			}
			if (CyanBranch > 0)
			{
				CyanBranchCool = 600;
			}
			float vl = Player.velocity.Length();
			if (vl > 30)
			{
				vl = 30;
			}
			if (vl < 5)
			{
				vl = 3 * vl - 10;
			}
			float Misp = (Miss + vl * Miss * 0.1f) / 100f;
			Misp = (float)(-1.1 / (Misp + 1) + 1.1);
			if (Main.rand.NextFloat(0, 1f) <= Misp && IMMUNE == 0)
			{
				CombatText.NewText(new Rectangle((int)Player.Center.X - 10, (int)Player.Center.Y - 10, 20, 20), Color.Cyan, "Miss");
				IMMUNE = 30;
				if (Player.longInvince)
				{
					IMMUNE = 45;
				}

				return false;
			}
			if (IMMUNE > 0)
			{
				return false;
			}

			return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
		}
		public override void Load()
		{

		}
	}
}
