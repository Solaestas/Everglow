using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Everglow.JourneysContinue.Items.Projectiles;
using Everglow.JourneysContinue.Items.Dusts;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics.PackedVector;

namespace Everglow.JourneysContinue.Items.Weapons.ChargedGuns
{
	public class Rampage : ChargedGunItem
	{
		//public override string Texture => "Everglow/JourneysContinue/Items/Weapons/Rampage";
		public override void SetDefaults()
		{
			Item.damage = 88;
			Item.width = 72;
			Item.height = 24;
			Item.value = 60845;
			Item.rare = ItemRarityID.Yellow;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.DamageType = DamageClass.Ranged;
			Item.noMelee = true;
			Item.knockBack = 5;
			Item.crit = 20;
			Item.useAmmo = AmmoID.Bullet;
			Item.shootSpeed = 27;
			Item.shoot = ModContent.ProjectileType<RampageBullet>();
			Item.autoReuse = true;

			//Stationary State
			Item.useTime = 30;
			Item.useAnimation = 30;

			//constumzied variables check ChargedGunItem.cs for more info
			charged = false;
			chargeItemID = ItemID.DirtBlock;
			energyLeft = 0;
			maxEnergy = 3000;
			energyPerShot = 100;
			passiveEnergyDecreaseRate = 1;

		}

		//it counts how many frames have elapsed since the playuer charge the gun
		//360 frams are 6 seconds
		public int frameCounter = 0;
		//controls how long the player has to wait until they can charge the gun again
		public int chargeCD = 360;


		/*
		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		*/

		public override void StationaryBehavior(Player player)
		{
			charged = false;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.noUseGraphic = false;
		}

		public override void ChargedBehavior(Player player)
		{
			// Set the itemAnimation and useAnimation to 15 when the weapon is charged
			Item.useTime = 15;
			Item.useAnimation = 15;
			Main.NewText("The gun is fully charged! Additional effects activated.", Color.LightBlue);
			energyLeft = maxEnergy;

		}

		public override void ChargeGun(Player player)
		{
			Main.NewText("The gun is charging! Consuming the item " + ItemID.Search.GetName(chargeItemID), Color.Yellow);

			if (ConsumeItemHandler(player))
			{
				Item.noUseGraphic = true;
				//play the animation here
				Main.NewText("Animation is plyaed.", Color.LightBlue);
				Item.noUseGraphic = false;

				charged = true;
				Main.NewText("Successful charging!", Color.Red);
			}
			else
			{
				Main.NewText("Unsuccessful charging!", Color.Red);
			}
			
		}

		public override void UpdateInventory(Player player)
		{
			frameCounter++;
			if (energyLeft > 0)
			{
				//this method here is getting called 60 times per second
				PassiveEnergyDecrement();
			}
			else
			if (energyLeft <= 0)
			{
				StationaryBehavior(player);
			}
		}

		public override bool AltFunctionUse(Player player)
		{
			return true;
		}

		public override Vector2? HoldoutOffset() => new Vector2(-11f, -4f);

		public override bool? UseItem(Player player)
		{

			// Check if the AltFunctionUse condition is met (right-click)
			// Check if at least 6 seconds has elapsed since the last charing action
			if (player.altFunctionUse == 2 && frameCounter >= chargeCD)
			{
				/*
				if (charged == false)
				{
					//ChargeGun when right - clicking
					ChargeGun(player);
				}
				*/
				ChargeGun(player);

				if (charged == true)
				{
					// Call ChargedBehavior
					ChargedBehavior(player);
				}
				else
				{
					StationaryBehavior(player);
				}
				//reset recharge CD
				frameCounter = 0;

			}

			// Return true to indicate that the item was used
			return true;
		}

		public override void UseItemFrame(Player player)
		{
			//prevent the recoil visual effect from taking place if the player right-clicked to charge the weapon
			if (player.altFunctionUse == 2)
			{
				return;
			}
			//the amount of recoli effect applied while firing the weapon is a substraction of the itemAnimation value clamped between 0 and the useAnimation value
			//the value of subtraction is controled by this variable:
			//the higher the value, the less recoil effect is applied
			int subtractionValue = 21;
			//the reocil effect applied is different if the weapon is charged or not
			//When the weapon is charged, useTime and useAnimation are both reduced to 15
			if (charged)
			{
				subtractionValue = 12;

			}

			int recoilEffectOffset = MathHelper.Clamp(player.itemAnimation - subtractionValue, 0, Item.useAnimation);
			player.itemLocation -= new Vector2(recoilEffectOffset * player.direction, 0).RotatedBy(player.itemRotation);

		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			//prevent the weapon from vanilla shooting bulletes if the player right-clicked to charge the weapon
			if (player.altFunctionUse == 2)
			{
				return false;
			}
			//if the weapon is not charged, shoot the bullet normally
			if (charged == false)
			{
				var BulletShellOffset = new Vector2(-3, -13);
				var d = Dust.NewDustDirect(player.Center + BulletShellOffset + velocity * 0.7f - new Vector2(4, 8), 0, 0, ModContent.DustType<BulletShellJC>(), velocity.X, velocity.Y, 0, default, 1f);
				d.velocity = velocity.RotatedBy(-1.57 * player.direction - Main.rand.NextFloat(0.8f, 1.2f) * player.direction) * 0.4f;
				d.noGravity = false;
				d.scale = 1f;

				var MuzzleOffset = new Vector2(12, 8);

				MuzzleOffset.Y = MuzzleOffset.Y * player.direction * -1;

				MuzzleOffset = MuzzleOffset.RotatedBy(velocity.ToRotation());
				Projectile.NewProjectileDirect(source, position + MuzzleOffset, velocity, type, damage, knockback, player.whoAmI);
				return false;
			}
			//if the weapon is charged, shoot the bullet with additional effects
			else if (charged == true)
			{
				energyLeft = Math.Max(0, energyLeft - energyPerShot);
				Main.NewText("Charged shot is fired, energy left is " + energyLeft, Color.LightBlue);

				var BulletShellOffset = new Vector2(-3, -13);
				var d = Dust.NewDustDirect(player.Center + BulletShellOffset + velocity * 0.7f - new Vector2(4, 8), 0, 0, ModContent.DustType<BulletShellJC>(), velocity.X, velocity.Y, 0, default, 1f);
				d.velocity = velocity.RotatedBy(-1.57 * player.direction - Main.rand.NextFloat(0.8f, 1.2f) * player.direction) * 0.4f;
				d.noGravity = false;
				d.scale = 1f;

				var MuzzleOffset = new Vector2(12, 8);

				MuzzleOffset.Y = MuzzleOffset.Y * player.direction * -1;

				MuzzleOffset = MuzzleOffset.RotatedBy(velocity.ToRotation());
				Projectile.NewProjectileDirect(source, position + MuzzleOffset, velocity, type, damage, knockback, player.whoAmI);
				return false;
			}
			return false;

		}

		public override bool CanConsumeAmmo(Item ammo, Player player)
		{
			//prevent ammo consumpation when charging the weapon by right-clicking
			if (player.altFunctionUse == 2)
			{
				return false;
			}
			return true;
		}
		
		public override void AddRecipes()
		{
			CreateRecipe()
			.AddIngredient(ItemID.DirtBlock, 10)
			.AddTile(TileID.WorkBenches)
				.Register();
		}

		/*
		public override void OnHitNPC(Player player, NPC target, int damage, float knockBack, bool crit)
		{
			// 给怪物加上我们之前做的Buff，持续10秒
			target.AddBuff(ModContent.BuffType<SuperToxic>(), 600);
		}
		*/
	}
}
