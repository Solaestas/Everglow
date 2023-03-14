using Everglow.Myth.MiscItems.Projectiles.Weapon.Melee.Hepuyuan;
using Terraria.DataStructures;

namespace Everglow.Myth.Common;

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
	public int Dashcool = 0;

	public float Miss = 0;
	public float CriticalDamage = 1f;
	public int SilverBuff = 0;
	public override void ResetEffects()//这个是更新帧刷的函数,在UpdateAccessory之前
	{
		CriticalDamage = 0f;
	}
	public override void ModifyHitNPC(Item item, NPC target, ref int damage, ref float knockback, ref bool crit)
	{
		if (crit)
			damage = (int)(damage * (CriticalDamage + 1));
	}
	public override void ModifyHitNPCWithProj(Projectile proj, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
	{
		if (crit)
			damage = (int)(damage * (CriticalDamage + 1));
	}
	public override void ModifyHitPvp(Item item, Player target, ref int damage, ref bool crit)
	{
		if (crit)
			damage = (int)(damage * (CriticalDamage + 1));
	}
	public override void ModifyHitPvpWithProj(Projectile proj, Player target, ref int damage, ref bool crit)
	{
		if (crit)
			damage = (int)(damage * (CriticalDamage + 1));
	}
	public int InvincibleFrameTime = 0;
	internal void UpdateDecrease(ref int value)
	{
		if (value > 0)
			value--;
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
		UpdateDecrease(ref SilverBuff);
		UpdateDecrease(ref Dashcool);
		UpdateDecrease(ref InvincibleFrameTime);
	}
	public override void PostUpdateMiscEffects()
	{
		if (CyanPedal > 0)
			Miss += 4;
		if (WhitePedal > 0)
			Miss += 2;
		if (CyanBranchCool > 0)
			Miss += 8;

		if (SilverBuff > 0)
		{
			Player.maxRunSpeed += 0.05f;
			Player.maxFallSpeed += 1.5f;
			Player.wingAccRunSpeed += 0.05f;
			Player.GetDamage(DamageClass.Generic) *= 1.05f;
		}
		if (Player.ownedProjectileCounts[ModContent.ProjectileType<Hepuyuan>()] + Player.ownedProjectileCounts[ModContent.ProjectileType<HepuyuanDown>()] > 0)
			Player.maxFallSpeed += 10000f;
	}
	public override bool PreHurt(bool pvp, bool quiet, ref int damage, ref int hitDirection, ref bool crit, ref bool customDamage, ref bool playSound, ref bool genGore, ref PlayerDeathReason damageSource, ref int cooldownCounter)
	{
		if (damage < 3)
			return false;
		if (OrangeStick > 0)
			OrangeStickCool = 300;
		if (CyanBranch > 0)
			CyanBranchCool = 600;
		float vl = Player.velocity.Length();
		if (vl > 30)
			vl = 30;
		if (vl < 5)
			vl = 3 * vl - 10;
		float Misp = (Miss + vl * Miss * 0.1f) / 100f;
		Misp = (float)(-1.1 / (Misp + 1) + 1.1);
		if (Main.rand.NextFloat(0, 1f) <= Misp && InvincibleFrameTime == 0)
		{
			CombatText.NewText(new Rectangle((int)Player.Center.X - 10, (int)Player.Center.Y - 10, 20, 20), Color.Cyan, "Miss");
			InvincibleFrameTime = 30;
			if (Player.longInvince)
				InvincibleFrameTime = 45;

			return false;
		}
		if (InvincibleFrameTime > 0)
			return false;

		return base.PreHurt(pvp, quiet, ref damage, ref hitDirection, ref crit, ref customDamage, ref playSound, ref genGore, ref damageSource, ref cooldownCounter);
	}
}
