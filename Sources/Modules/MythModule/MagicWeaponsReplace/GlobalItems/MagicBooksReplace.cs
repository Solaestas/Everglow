using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.GlobalItems
{
    public class MagicBooksReplace : GlobalItem
    {
        public override void SetDefaults(Item item)
        {
            if (item.type == ItemID.WaterBolt)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.BookofSkulls)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.DemonScythe)
            {
                item.autoReuse = true;
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.CursedFlames)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.GoldenShower)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.CrystalStorm)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.MagnetSphere)
            {
                item.noUseGraphic = true;
            }
            if (item.type == ItemID.RazorbladeTyphoon)
            {
                item.noUseGraphic = true;
            }
            base.SetDefaults(item);
        }
        public override bool? UseItem(Item item, Player player)
        {
            if(item.type == ItemID.WaterBolt)
            {
                int aimType = ModContent.ProjectileType<Projectiles.WaterBolt.WaterBoltBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero,aimType,0,0,player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.WaterBolt.WaterBoltArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.DemonScythe)
            {
                int aimType = ModContent.ProjectileType<Projectiles.DemonScythe.DemonScytheBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.DemonScythe.DemonScytheArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.MagnetSphere)
            {
                int aimType = ModContent.ProjectileType<Projectiles.MagnetSphere.MagnetSphereBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.MagnetSphere.MagnetSphereArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.RazorbladeTyphoon)
            {
                int aimType = ModContent.ProjectileType<Projectiles.RazorbladeTyphoon.RazorbladeTyphoonBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.RazorbladeTyphoon.RazorbladeTyphoonArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.CursedFlames)
            {
                int aimType = ModContent.ProjectileType<Projectiles.CursedFlames.CursedFlamesBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.CursedFlames.CursedFlamesArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.CrystalStorm)
            {
                int aimType = ModContent.ProjectileType<Projectiles.CrystalStorm.CrystalStormBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.CrystalStorm.CrystalStormArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.BookofSkulls)
            {
                int aimType = ModContent.ProjectileType<Projectiles.BookofSkulls.BookofSkullsBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.BookofSkulls.BookofSkullsArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            if (item.type == ItemID.GoldenShower)
            {
                int aimType = ModContent.ProjectileType<Projectiles.GoldenShower.GoldenShowerBook>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
                aimType = ModContent.ProjectileType<Projectiles.GoldenShower.GoldenShowerArray>();
                if (player.ownedProjectileCounts[aimType] < 1)
                {
                    Projectile.NewProjectile(player.GetSource_FromAI(), player.Center, Vector2.Zero, aimType, 0, 0, player.whoAmI);
                }
            }
            return base.UseItem(item, player);
        }
        public override bool Shoot(Item item, Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (item.type == ItemID.WaterBolt)
            {
                return false;
            }
            if (item.type == ItemID.DemonScythe)
            {
                return false;
            }
            if (item.type == ItemID.BookofSkulls)
            {
                return false;
            }
            if (item.type == ItemID.GoldenShower)
            {
                return false;
            }
            if (item.type == ItemID.CursedFlames)
            {
                return false;
            }
            if (item.type == ItemID.CrystalStorm)
            {
                return false;
            }
            if (item.type == ItemID.MagnetSphere)
            {
                return false;
            }
            if (item.type == ItemID.RazorbladeTyphoon)
            {
                return false;
            }
            return base.Shoot(item, player, source, position, velocity, type, damage, knockback);
        }
    }
}
