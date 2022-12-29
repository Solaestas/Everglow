using Everglow.Sources.Modules.FoodModule.Projectiles;
using Everglow.Sources.Modules.MEACModule.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.FoodModule.Items.Weapons
{
    public class FryingPanItem : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.useAnimation = 10;
            Item.width = 1;
            Item.height = 1;

            Item.knockBack = 5f;
            Item.damage = 25;
            Item.rare = ItemRarityID.Green;

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.shootSpeed = 7.5f;
            Item.shoot = ModContent.ProjectileType<FryingPan>();

            Item.value = Item.sellPrice(gold: 2);
        }


        public override bool CanUseItem(Player player)
        {
            if (base.CanUseItem(player))
            {
                if (Main.myPlayer == player.whoAmI )
                {
                    foreach (Projectile projectile in Main.projectile)
                    {
                        if (projectile.owner == player.whoAmI && projectile.type == ModContent.ProjectileType<FryingPan>() && projectile.active)
                        {
                            return false;
                        }
                    }
                    if (player.altFunctionUse == 2)//右键
                    {
                        Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<FryingPan>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
                        proj.scale *= Item.scale;
                        (proj.ModProjectile as MeleeProj).isRightClick = true;//指定为右键
                        (proj.ModProjectile as MeleeProj).attackType = 100;//切换到弹幕的蓄力斩攻击方式
                    }
                    if (player.altFunctionUse != 2)
                    {
                        return true;
                    }
                }
                return false;
            }
            return base.CanUseItem(player);
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse != 2)//左键
            {
                Projectile proj = Projectile.NewProjectileDirect(source, player.Center, velocity, type, damage, knockback, Main.LocalPlayer.whoAmI, 0f, 0f);
                proj.scale *= Item.scale;
            }
            
            return false;
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
        public override void AddRecipes()
        {

        }
    }
}
