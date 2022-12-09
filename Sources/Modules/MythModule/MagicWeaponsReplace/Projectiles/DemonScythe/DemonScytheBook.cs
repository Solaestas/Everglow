﻿using Terraria.Audio;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.DemonScythe
{
    internal class DemonScytheBook : MagicBookProjectile//
    {
        public override void SetDef()
        {
            DustType = DustID.DemonTorch;
            ItemType = ItemID.DemonScythe;
            UseGlow = false;
            effectColor = new Color(75, 0, 225, 175);

            //string pathBase = "MagicWeaponsReplace/Textures/";
            //FrontTexPath = pathBase + "DemonScythe_A";
            //PaperTexPath = pathBase + "DemonScythe_C";
            //BackTexPath = pathBase + "DemonScythe_B";

            //TexCoordTop = new Vector2(8, 0);
            //TexCoordLeft = new Vector2(0, 24);
            //TexCoordDown = new Vector2(20, 30);
            //TexCoordRight = new Vector2(28, 4);
        }
        public override void SpecialAI()
        {
            Player player = Main.player[Projectile.owner];
            if(player.itemTime == 2 && player.HeldItem.type == ItemType)
            {
                Vector2 velocity = Utils.SafeNormalize(Main.MouseWorld - Projectile.Center, Vector2.Zero) * player.HeldItem.shootSpeed;
                Projectile p = Projectile.NewProjectileDirect(Projectile.GetSource_FromAI(), Projectile.Center + Utils.SafeNormalize(velocity, Vector2.Zero) * 25, velocity * 16, ModContent.ProjectileType<DemonScythePlus>()/*ProjectileID.DemonScythe*/, player.HeldItem.damage, player.HeldItem.knockBack, player.whoAmI);
                SoundEngine.PlaySound(new SoundStyle("Everglow/Sources/Modules/MythModule/MagicWeaponsReplace/Sounds/DemonScyth"), Projectile.Center);
                p.CritChance = player.GetWeaponCrit(player.HeldItem);
            }
        }
    }
}