using Everglow.Sources.Modules.FoodModule.Projectiles;
using Everglow.Sources.Modules.MEACModule.Projectiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Modules.FoodModule.Items.Weapons
{
    public class FryingPanItem : ModItem
    {
        public override void SetDefaults()
        {
            Item.useStyle = 1;
            Item.width = 1;
            Item.height = 1;

            Item.knockBack = 2.5f;
            Item.damage = 500;
            Item.rare = ItemRarityID.Green;

            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true;
            Item.noUseGraphic = true;

            Item.value = Item.sellPrice(gold: 1);
        }
        public override bool CanUseItem(Player player)
        {
            if (base.CanUseItem(player))
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    if (player.altFunctionUse != 2)//左键
                    {
                        Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<FryingPan>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
                        proj.scale *= Item.scale;
                    }
                    else//右键
                    {
                        Projectile proj = Projectile.NewProjectileDirect(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<FryingPan>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
                        proj.scale *= Item.scale;
                        (proj.ModProjectile as MeleeProj).isRightClick = true;//指定为右键
                        (proj.ModProjectile as MeleeProj).attackType = 100;//切换到弹幕的蓄力斩攻击方式

                    }
                }
                return false;
            }
            return base.CanUseItem(player);
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
