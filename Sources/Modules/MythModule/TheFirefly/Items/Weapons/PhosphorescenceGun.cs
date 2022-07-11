using Everglow.Sources.Modules.MythModule.Bosses.CorruptMoth.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Dusts;
using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Weapons
{
    public class PhosphorescenceGun : ModItem
    {
        public override void SetStaticDefaults()
        {
            GetGlowMask = MythContent.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;

            Item.width = 64;
            Item.height = 40;
            Item.rare = 2;
            Item.value = 2000;


            Item.useTime = 48;
            Item.useAnimation = 48;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.autoReuse = true;
            Item.UseSound = SoundID.Item36;


            Item.DamageType = DamageClass.Ranged;
            Item.damage = 10;
            Item.knockBack = 6f;
            Item.noMelee = true;
            Item.noUseGraphic = true;


            Item.shoot = ProjectileID.PurificationPowder;
            Item.shootSpeed = 10f;
            Item.useAmmo = AmmoID.Bullet;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            GunShakePlayer Gsplayer = player.GetModPlayer<GunShakePlayer>();
            Gsplayer.FlyCamPosition = new Vector2(0, 32).RotatedByRandom(6.283);
            const int NumProjectiles = 4;
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.PhosphorescenceGun>()] < 1)
            {
                Projectile.NewProjectileDirect(source, position + velocity * 2.0f - new Vector2(0, 4), Vector2.Zero, ModContent.ProjectileType<Projectiles.PhosphorescenceGun>(), damage, knockback, player.whoAmI, 1f, Item.useAnimation);
            }
            else
            {
                for (int x = 0; x < Main.projectile.Length; x++)
                {
                    if (Main.projectile[x].active)
                    {
                        if (Main.projectile[x].type == ModContent.ProjectileType<Projectiles.PhosphorescenceGun>())
                        {
                            if (Main.projectile[x].owner == player.whoAmI)
                            {
                                Main.projectile[x].ai[0] = 1f;
                                Main.projectile[x].ai[1] = Item.useAnimation;
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < NumProjectiles; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);
                Projectile.NewProjectileDirect(source, position + velocity * 2.0f - new Vector2(0, 4), newVelocity, type, damage, knockback, player.whoAmI);
            }
            for (int i = 0; i < NumProjectiles * 3; i++)
            {
                Vector2 newVelocity = velocity.RotatedByRandom(MathHelper.ToRadians(15));
                newVelocity *= 1f - Main.rand.NextFloat(0.3f);
                Projectile.NewProjectileDirect(source, position + velocity * 2.0f - new Vector2(0, 4), newVelocity, ModContent.ProjectileType<PhosphorescenceBullet>(), (int)(damage * 0.26f), knockback, player.whoAmI);
                for (int z = 0; z < 3; z++)
                {
                    Vector2 v = newVelocity * z / 3f;
                    Dust.NewDust(position + newVelocity * 3f - new Vector2(0, 4), 0, 0, ModContent.DustType<MothBlue>(), v.X, v.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                    v = newVelocity * (z + 0.5f) / 3f;
                    Dust.NewDust(position + newVelocity * 3f - new Vector2(0, 4), 0, 0, ModContent.DustType<MothBlue2>(), v.X, v.Y, 0, default(Color), Main.rand.NextFloat(0.8f, 1.7f));
                }
            }
            return false;
        }
    }
    class GunShakePlayer : ModPlayer
    {
        public Vector2 FlyCamPosition = Vector2.Zero;
        public override void ModifyScreenPosition()
        {
            FlyCamPosition *= 0.25f;
            Main.screenPosition += FlyCamPosition;
        }
    }
}
