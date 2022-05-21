using Terraria.DataStructures;

namespace Everglow.Sources.Modules.Food
{
    public class FoodPojectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player player = Main.player[projectile.owner];
            FoodModPlayer FoodModPlayer = player.GetModPlayer<FoodModPlayer>();

            if (source is EntitySource_ItemUse_WithAmmo)
            {
                var sourceAmmo = source as EntitySource_ItemUse_WithAmmo;
                if (FoodModPlayer.StarfruitBuff)
                {
                    var newSource = projectile.GetSource_FromThis();
                    var velocity = projectile.velocity.RotatedBy(MathHelper.ToRadians(180f));
                    Projectile.NewProjectile(newSource, projectile.position, velocity, projectile.type,
                        projectile.damage, projectile.knockBack, projectile.owner);
                }
            }
        }
    }
}
