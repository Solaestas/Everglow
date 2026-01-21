using Everglow.Myth.LanternMoon.Projectiles.LanternKing;
using Everglow.Myth.LanternMoon.VFX;
using Terraria.DataStructures;

namespace Everglow.Myth.LanternMoon.Items;

public class LanternBombRemoteControl : ModItem
{
	public override string LocalizationCategory => Everglow.Commons.Utilities.LocalizationUtils.Categories.MagicWeapons;

	public override void SetDefaults()
	{
		Item.DamageType = DamageClass.Magic;
		Item.useStyle = ItemUseStyleID.Swing;
		Item.rare = ItemRarityID.White;
		Item.noUseGraphic = true;
		Item.autoReuse = false;
		Item.useTurn = true;
		Item.mana = 7;
		Item.width = 20;
		Item.height = 38;
		Item.useAnimation = 10;
		Item.useTime = 10;
		Item.value = 10000;
		Item.shoot = ProjectileID.WoodenArrowFriendly;
		Item.shootSpeed = 10;
		Item.damage = 9999;
		Item.knockBack = 15;
	}

	public override bool AltFunctionUse(Player player)
	{
		return true;
	}

	public Vector2 RandomVector2(float maxLength, float minLength = 0)
	{
		if (maxLength <= minLength)
		{
			maxLength = minLength + 0.001f;
		}
		return new Vector2(Main.rand.NextFloat(minLength, maxLength), 0).RotatedByRandom(MathHelper.TwoPi);
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		//ExplodeEffect(Main.MouseWorld);
		Projectile.NewProjectileDirect(Item.GetSource_FromAI(),Main.MouseWorld, Vector2.zeroVector, ModContent.ProjectileType<LanternGhostKingExplosion>(), 50, 0f, player.whoAmI);

		//Projectile.NewProjectileDirect(Item.GetSource_FromAI(), Main.MouseWorld, Vector2.zeroVector, ModContent.ProjectileType<SmallLanternGroup_LanternRain>(), 50, 0f, player.whoAmI, Main.rand.Next(4), 0);

		//float addValue = Main.rand.NextFloat(6.283f);
		//for (int x = 0; x < 5; x++)
		//{
		//	float minDis = 600;
		//	NPC target = null;
		//	foreach(var npc in Main.npc)
		//	{
		//		if(npc != null && npc.active)
		//		{
		//			Vector2 dis = npc.Center - Main.MouseWorld;
		//			if(dis.Length() < minDis)
		//			{
		//				minDis = dis.Length();
		//				target = npc;
		//			}
		//		}
		//	}
		//	if(target != null)
		//	{
		//		Projectile p0 = Projectile.NewProjectileDirect(Item.GetSource_FromAI(), target.Center + new Vector2(2000, 0).RotatedBy(x / 5f * MathHelper.TwoPi + addValue), new Vector2(-11, 0).RotatedBy(x / 5f * MathHelper.TwoPi + addValue), ModContent.ProjectileType<LanternFlow>(), 85, 0f, player.whoAmI, 0.02f, 0);
		//		LanternFlow lanternF = p0.ModProjectile as LanternFlow;
		//		lanternF.OwnerNPC = target;
		//		lanternF.MinDisToNPC = 500;
		//		lanternF.VelDecay = 0.995f;
		//		lanternF.RotateSpeed = -0.0598575436f;
		//		lanternF.BestRotateSpeed = 0;
		//		lanternF.BestVelDecay = 0;
		//	}
		//}

		//Projectile.NewProjectile(Item.GetSource_FromAI(), Main.MouseWorld + new Vector2(0, -600), Vector2.Zero, ModContent.ProjectileType<LanternFlowLine>(), 40, 0f, player.whoAmI, 0, 0);
		return false;
	}


}