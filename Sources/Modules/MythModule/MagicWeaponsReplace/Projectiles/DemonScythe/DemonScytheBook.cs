using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.DemonScythe
{
    internal class DemonScytheBook : MagicBookProjectile//
    {
        public override void SetDef()
        {
            DustType = DustID.DemonTorch;
            ItemType = ItemID.DemonScythe;
            UseGlow = false;
        }
        public override void SpecialAI()
        {
            Player player = Main.player[Projectile.owner];
            if(player.itemTime == 2 && player.HeldItem.type == ItemType)
            {
                Vector2 velocity = Utils.SafeNormalize(Main.MouseWorld - Projectile.Center, Vector2.Zero) * player.HeldItem.shootSpeed;
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + Utils.SafeNormalize(velocity, Vector2.Zero) * 25, velocity * 16, ModContent.ProjectileType<DemonScythePlus>()/*ProjectileID.DemonScythe*/, player.HeldItem.damage, player.HeldItem.knockBack, player.whoAmI);
                SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Sounds/DemonScyth"), Projectile.Center);
                p.CritChance = (int)(player.GetCritChance(DamageClass.Generic) + player.HeldItem.crit);
            }
        }
    }
}