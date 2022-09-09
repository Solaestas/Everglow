
namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.WaterBolt
{
    public class OldWaterBolt : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if(Main.player[projectile.owner].GetModPlayer<GlobalItems.MagicBookPlayer>().MagicBookLevel == 1)
            {
                if(projectile.type == ProjectileID.WaterBolt)
                {
                    int type = ModContent.ProjectileType<WaterTeleport>();
                    Player player = Main.player[projectile.owner];

                    player.GetModPlayer<GlobalItems.MagicBookPlayer>().WaterBoltHasHit += 1;
                    if (player.GetModPlayer<GlobalItems.MagicBookPlayer>().WaterBoltHasHit >= 5)
                    {
                        if(player.ownedProjectileCounts[type] < 6)
                        {
                            player.GetModPlayer<GlobalItems.MagicBookPlayer>().WaterBoltHasHit = 0;
                            Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), player.Center + new Vector2(0, -30 * player.gravDir), new Vector2(0, -18 * player.gravDir), type, 0, 0, projectile.owner, player.ownedProjectileCounts[type] + 1);
                        }
                    }
                }
            }
            base.OnHitNPC(projectile, target, damage, knockback, crit);
        }
    }
}