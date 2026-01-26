using Everglow.Myth.LanternMoon.Projectiles.LanternKing;
using Everglow.Myth.LanternMoon.Projectiles.PerWave15;
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

	public void BrakeGoldenShieldEffect()
	{
		for (int g = 0; g < 150; g++)
		{
			float value = MathF.Pow(Main.rand.NextFloat(), 0.3f);
			Vector2 offsetPos = new Vector2(0, -value * 135).RotatedByRandom(2.6f);
			offsetPos.Y *= 85f / 135f;
			Vector2 newVelocity = offsetPos / 2.4f;
			var sparkFlame = new LanternGoldenShieldFragiles
			{
				Velocity = newVelocity,
				Active = true,
				Visible = true,
				Position = Main.MouseWorld + offsetPos,
				RotateSpeed = Main.rand.NextFloat(-0.3f, 0.3f),
				Rotate2Speed = Main.rand.NextFloat(-0.5f, 0.5f),
				VelocityRotateSpeed = Main.rand.NextFloat(0.15f, 0.45f) * (g % 2 - 0.5f) * 0.2f,
				Rotation = Main.rand.NextFloat(MathHelper.TwoPi),
				Rotation2 = Main.rand.NextFloat(MathHelper.TwoPi),
				MaxTime = Main.rand.Next(155, 200),
				Scale = Main.rand.NextFloat(0.6f, 1.5f),
				Frame = Main.rand.Next(0, 4),
			};
			Ins.VFXManager.Add(sparkFlame);
		}
		for (int x = 0; x < 72; x++)
		{
			float value = MathF.Pow(Main.rand.NextFloat(), 0.3f);
			Vector2 offsetPos = new Vector2(0, -value * 135).RotatedByRandom(2.6f);
			offsetPos.Y *= 85f / 135f;
			Vector2 newVelocity = offsetPos / 2f;
			var spark = new LanternGoldenShieldStar()
			{
				Velocity = newVelocity,
				Active = true,
				Visible = true,
				Position = Main.MouseWorld + offsetPos,
				RotateSpeed = 0,
				Rotation = 0,
				MaxTime = Main.rand.Next(50, 100),
				Scale = Main.rand.NextFloat(0.5f, 1f),
			};
			Ins.VFXManager.Add(spark);
		}
	}

	public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
	{
		//ShakerManager.AddShaker(Main.MouseWorld, new Vector2(0, 1).RotatedByRandom(MathHelper.TwoPi), 6, 0.8f, 16, 0.9f, 0.8f, 30); ;
		//var matrix = new LanternGhostKing_Wheel3Mark()
		//{
		//	Active = true,
		//	Visible = true,
		//	Position = Main.MouseWorld,
		//	Rotation = 0.585f,
		//	MaxTime = 1500,
		//	ExtraUpdate = 6,
		//	Scale = 0.25f,
		//};
		//Ins.VFXManager.Add(matrix);
		//BrakeGoldenShieldEffect();
		// ExplodeEffect(Main.MouseWorld);
		//Projectile.NewProjectileDirect(Item.GetSource_FromAI(), Main.MouseWorld, Vector2.zeroVector, ModContent.ProjectileType<LanternGhostKingExplosion>(), 50, 0f, player.whoAmI);

		Projectile.NewProjectileDirect(Item.GetSource_FromAI(), Main.MouseWorld, Vector2.zeroVector, ModContent.ProjectileType<SmallLanternGroup>(),20, 0f, Main.myPlayer, Main.rand.Next(4), 0);

		//float addValue = Main.rand.NextFloat(6.283f);
		//for (int x = 0; x < 5; x++)
		//{
		//	float minDis = 600;
		//	NPC target = null;
		//	foreach (var npc in Main.npc)
		//	{
		//		if (npc != null && npc.active)
		//		{
		//			Vector2 dis = npc.Center - Main.MouseWorld;
		//			if (dis.Length() < minDis)
		//			{
		//				minDis = dis.Length();
		//				target = npc;
		//			}
		//		}
		//	}
		//	if (target != null)
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

		// Projectile.NewProjectile(Item.GetSource_FromAI(), Main.MouseWorld + new Vector2(0, -600), Vector2.Zero, ModContent.ProjectileType<LanternFlowLine>(), 40, 0f, player.whoAmI, 0, 0);
		return false;
	}
}