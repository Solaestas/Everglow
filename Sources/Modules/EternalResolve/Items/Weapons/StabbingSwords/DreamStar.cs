using MythMod.EternalResolveMod.Common.Modulars.RefineSystemModular;
using Terraria.DataStructures;

namespace MythMod.EternalResolveMod.Items.Weapons.Stabbings.Contents
{
    public class DreamStar : LevelItem
    {
        public override string[] ChineseText => new string[]
        {
             "◆ 攻击无视目标25%防御",
             "◆ 攻击无视目标50%防御",
             "◆ 攻击无视目标75%防御",
             "◆ 攻击无视目标100%防御",
             "◆ 攻击额外造成玩家10%最大魔力值的伤害"
        };
        public override string[] EnglishText => new string[]
        {
            "◆ Ignores 25% of enemies' defense",
            "◆ Ignores 50% of enemies' defense",
            "◆ Ignores 75% of enemies' defense",
            "◆ Ignores 100% of enemies' defense",
            "◆ Deals extra damage equals to 10% of your max mana"
        };
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation(Chinese, "圣器 · 梦辰星");
            DisplayName.AddTranslation(English, "The Star of Dream");
            base.SetStaticDefaults();
        }
        public override void SetDefaults()
        {
            ToStabbing(4);
            Item.damage = 14;
            Item.knockBack = 1.5f;
            Item.value = Item.sellPrice(0, 0, 75);
            Item.shoot = ModContent.ProjectileType<DreamStar_Pro>();
            Item.GetGlobalItem<WeaponRefine>().CanLevelUp = true;
            Item.GetGlobalItem<WeaponRefine>().LevelMax = 5;
            base.SetDefaults();
        }
        public override bool AltFunctionUse(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<StabPower>()] < 1;
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.altFunctionUse == 2)
            {
                Projectile.NewProjectile(source, position, Vector2.Zero, ModContent.ProjectileType<StabPower>(), damage, knockback, player.whoAmI, 0f, 0f);
                return false;
            }
            return true;
        }
        public override bool CanUseItem(Player player)
        {
            return player.ownedProjectileCounts[ModContent.ProjectileType<StabPower>()] < 1;
        }
    }
}