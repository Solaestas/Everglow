using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Modules.FoodModule.Buffs;
using Terraria.DataStructures;
using Everglow.Resources.ItemList.Weapons.Ranged;
using Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.GlobalItems;
using Terraria.Localization;

namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class FoodBuffGlobalItem : GlobalItem
    {
        public override bool InstancePerEntity => true;
        float shootspeed;
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.GetModPlayer<FoodBuffModPlayer>().StarfruitBuff) 
            {
                if (Shotguns.vanillaShotguns.Contains(item.type))
                {
                    Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(5)) * (1f - Main.rand.NextFloat(0.1f));
                    if (item.type == ItemID.Xenopopper)
                    {
                        int num = Projectile.NewProjectile(source, position, newVelocity / 4, ProjectileID.Xenopopper, damage, knockback, player.whoAmI);
                        Main.projectile[num].localAI[0] = type;
                        Main.projectile[num].localAI[1] = velocity.Length();

                    }
                    else
                    {
                        Projectile.NewProjectileDirect(source, position, newVelocity, type, damage, knockback, player.whoAmI);
                    }

                    return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
                }
            }
            if (player.GetModPlayer<FoodBuffModPlayer>().FriedEggBuff)
            {
                if (Consumables.vanillaConsumables.Contains(item.type))
                {
                    Projectile.NewProjectile(source, position,1.5f*velocity, type, damage, knockback, player.whoAmI, 0);
                    return false;
                }
            }

            return true;
        }
    }
}
