using Everglow.Sources.Modules.MythModule.TheTusk.Projectiles;
using Terraria.DataStructures;
namespace Everglow.Sources.Modules.MythModule.TheTusk.Items.Weapons
{
    public class ToothKnife : ModItem
    {
        //TODO:暴击后在地上召唤獠牙刺
        public override void SetDefaults()
        {
            Item.width = 40;
            Item.height = 48;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 25;
            Item.knockBack = 6;
            Item.crit = 6;

            Item.value = 4000;
            Item.rare = ItemRarityID.Green;
            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<Projectiles.Weapon.Tusk0>();
            Item.shootSpeed = 8f;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            return false;
        }
        public override void ModifyHitNPC(Player player, NPC target, ref int damage, ref float knockBack, ref bool crit)
        {
            if(crit)
            {
                Projectile.NewProjectile(Item.GetSource_OnHit(target), target.Center + new Vector2(80, 0), new Vector2(Main.rand.NextFloat(-1f, 1f), 0), ModContent.ProjectileType<EarthTusk>(), Item.damage, Item.knockBack * 0.8f, player.whoAmI, 2, 1);
                Projectile.NewProjectile(Item.GetSource_OnHit(target), target.Center + new Vector2(-80, 0), new Vector2(Main.rand.NextFloat(-1f, 1f), 0), ModContent.ProjectileType<EarthTusk>(), Item.damage, Item.knockBack * 0.8f, player.whoAmI, 2, -1);
            }
        }
    }
}
