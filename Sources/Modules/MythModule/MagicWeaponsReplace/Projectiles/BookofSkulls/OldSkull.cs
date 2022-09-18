
namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.BookofSkulls
{
    public class OldSkull : GlobalProjectile
    {
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit)
        {
            if(Main.player[projectile.owner].GetModPlayer<GlobalItems.MagicBookPlayer>().MagicBookLevel == 1)
            {
                if(projectile.type == ProjectileID.BookOfSkullsSkull)
                {
                    int type = ModContent.ProjectileType<BoneSpike>();
                    Player player = Main.player[projectile.owner];

                    if (player.ownedProjectileCounts[type] < 12)
                    {
                        for (int x = 0; x < 3; x++)
                        {
                            player.GetModPlayer<GlobalItems.MagicBookPlayer>().WaterBoltHasHit = 0;
                            Projectile.NewProjectileDirect(projectile.GetSource_FromAI(), player.Center + new Vector2(0, -30 * player.gravDir), new Vector2(0, 18 * player.gravDir), type, player.HeldItem.damage / 2, player.HeldItem.knockBack, projectile.owner, Main.rand.NextFloat(-1.5f, 7f), Main.rand.NextFloat(0.65f, 0.95f));
                        }
                    }
                }
            }
            base.OnHitNPC(projectile, target, damage, knockback, crit);
        }
    }
}