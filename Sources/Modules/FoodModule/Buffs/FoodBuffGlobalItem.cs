using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Everglow.Sources.Modules.FoodModule.Buffs;
using Terraria.DataStructures;
using Everglow.Resources.VanillaItemList.Weapons.Ranged;
using Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.GlobalItems;

namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class FoodBuffGlobalItem : GlobalItem
    {

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
            return true;
        }
    }
}
