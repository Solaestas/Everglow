using Everglow.Sources.Modules.MythModule.TheFirefly.Projectiles;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;
using Everglow.Sources.Modules.MythModule.Common;
using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.TheFirefly.Items.Weapons
{
    public class EvilChrysalis : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("EvilChrysalis");
            Tooltip.SetDefault("召唤蛾子攻击敌人,蛾子在战斗过程中因为疲劳,伤害会渐渐下降,最低降至5%\n脱离战斗,或者重新召唤后伤害会恢复正常\n蛾子伤害倍率 25%\n萤火弹伤害倍率 100%\n右键持续消耗法力生成一个法阵,为附近的所有蛾子缓缓回复能量,每隔一秒释放出蛾子存在个数的蓝色幻影(倍率70%)攻击敌人\n法阵消耗的法力正比于蛾子存在数量");
            GetGlowMask = MythContent.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            Item.damage = 24;
            Item.mana = 6;
            Item.width = 50;
            Item.height = 50;
            Item.useTime = 36;
            Item.useAnimation = 36;
            Item.useStyle = 1;
            Item.noMelee = true;
            Item.knockBack = 2.25f;
            Item.value = 2100;
            Item.rare = 2;
            Item.UseSound = SoundID.Item44;
            Item.shoot = ModContent.ProjectileType<Projectiles.EvilChrysalis>();
            Item.autoReuse = true;
            Item.shootSpeed = 10f;
            Item.DamageType = DamageClass.Summon;
            Item.noUseGraphic = true;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(source, position, velocity, type, damage, knockback, player.whoAmI, Main.rand.NextFloat(0, 200f), Main.rand.NextFloat(0, 200f));
            return false;
        }
        public override bool CanUseItem(Player player)
        {
            if (base.CanUseItem(player))
            {
                if (Main.myPlayer == player.whoAmI)
                {
                    if (player.altFunctionUse == 2 && player.statMana >= player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.GlowMoth>()] && player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.MothMagicArray>()] == 0)
                    {
                        Projectile.NewProjectile(player.GetSource_ItemUse(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<EvilChrysalisRightClick>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
                        Projectile.NewProjectile(player.GetSource_ItemUse(Item), Main.MouseWorld, Vector2.Zero, ModContent.ProjectileType<MothMagicArray>(), player.GetWeaponDamage(Item), Item.knockBack, player.whoAmI);
                        return false;
                    }
                }
            }
            return base.CanUseItem(player);
        }
        public override bool AltFunctionUse(Player player)
        {
            return true;
        }
    }
}
