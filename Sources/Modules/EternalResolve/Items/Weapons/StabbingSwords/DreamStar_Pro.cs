using MythMod.EternalResolveMod.Common.Modulars.RefineSystemModular;

namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Contents
{
    public class DreamStar_Pro : StabbingProjectile
    {
        public override int SoundTimer => 10;
        public override void SetDefaults()
        {
            Projectile.localNPCHitCooldown = 10;
            Projectile.GetGlobalProjectile<StabbingDrawer>().Color = Color.Gold;
            base.SetDefaults();
        }
        public override void ModifyHitNPC(NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection)
        {
            if (Main.LocalPlayer.HeldItem.type == ModContent.ItemType<DreamStar>())
            {
                Item Item = Main.LocalPlayer.HeldItem;
                if (Item.GetGlobalItem<WeaponRefine>().Level == 1)
                {
                    damage += target.defense / 2 + target.defense / 4;
                }

                if (Item.GetGlobalItem<WeaponRefine>().Level == 2)
                {
                    damage += target.defense / 2 + target.defense / 2;
                }

                if (Item.GetGlobalItem<WeaponRefine>().Level == 3)
                {
                    damage += target.defense + target.defense / 4;
                }

                if (Item.GetGlobalItem<WeaponRefine>().Level == 4)
                {
                    damage += target.defense + target.defense / 2;
                }

                if (Item.GetGlobalItem<WeaponRefine>().Level == 5)
                {
                    damage += Main.LocalPlayer.statManaMax2 / 10;
                }
            }
            base.ModifyHitNPC(target, ref damage, ref knockback, ref crit, ref hitDirection);
        }
    }
}
