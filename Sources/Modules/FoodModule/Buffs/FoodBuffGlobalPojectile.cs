using Terraria.DataStructures;

namespace Everglow.Sources.Modules.FoodModule.Buffs
{
    public class FoodBuffGlobalPojectile : GlobalProjectile
    {
        public override void OnSpawn(Projectile projectile, IEntitySource source)
        {
            Player player = Main.LocalPlayer;
            if (player != null && player.active && !player.dead)
            {
                if (player.GetModPlayer<FoodBuffModPlayer>().BlueHawaiiBuff)
                {
                    if (projectile.owner == player.whoAmI)
                    {
                        projectile.velocity *= 2;
                    }
                }
                if (player.GetModPlayer<FoodBuffModPlayer>().CantaloupeJellyBuff)
                {
                    if (projectile.owner == player.whoAmI && projectile.penetrate >= 0)
                    {
                        projectile.penetrate++;
                    }
                }

            }
        }
        public override bool OnTileCollide(Projectile projectile, Vector2 oldVelocity)
        {
            Player player = Main.LocalPlayer;
            if (player != null && player.active && !player.dead)
            {
                if (player.GetModPlayer<FoodBuffModPlayer>().CaramelPuddingBuff)
                    if (projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f)
                    {
                        projectile.velocity.X = oldVelocity.X * -0.9f;
                    }
                if (projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f)
                {
                    projectile.velocity.Y = oldVelocity.Y * -0.9f;
                }
            }
            return false;
        }
    public override void AI(Projectile projectile)
    {
        Player player = Main.LocalPlayer;
        if (player != null && player.active && !player.dead)
        {
            if (player.GetModPlayer<FoodBuffModPlayer>().DreamYearningBuff)
            {
                if (projectile.owner == player.whoAmI)
                {
                    if (!projectile.friendly && projectile.penetrate >= 0)
                    {
                        return;
                    }
                    Vector2 destination = projectile.Center;
                    bool locatedTarget = false;
                    for (int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].CanBeChasedBy(projectile, false) && projectile.WithinRange(Main.npc[i].Center, 250) && (projectile.tileCollide || Collision.CanHit(projectile.Center, 1, 1, Main.npc[i].Center, 1, 1)))
                        {
                            destination = (Main.npc[i]).Center;
                            locatedTarget = true;
                            break;
                        }
                    }
                    if (locatedTarget)
                    {
                        Vector2 homeDirection = Vector2.Normalize(destination - projectile.Center);
                        projectile.velocity = projectile.velocity + homeDirection * 2;
                    }
                }
            }
        }
        base.AI(projectile);
    }
}
}
