using Terraria.DataStructures;

namespace Everglow.Sources.Modules.OceanModule.Items.Weapons
{
    public class RampageShark : ModItem
    {
        //暴走鲨
        //狂热度C：每秒获得2点，最高不超过16，有以下效果
        //武器精准度随着C升高而下降
        //有一定概率以散弹的形式同时打出很多子弹，概率为(C% + 33%暴击率)，其中暴击率部份只计算100%以内的部分
        //75%的概率不消耗弹药
        //停止使用时，狂热度未清零前会以每秒3点下降，此期间无法使用
        //击退力加算40%C;
        //C满值是攻速为200%
        public float CrazyValue = 0;//C
        public override void SetDefaults()
        {
            Item.damage = 88;
            Item.width = 72;
            Item.height = 34;
            Item.value = 60845;
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.noMelee = true;
            Item.knockBack = 5;
            Item.noUseGraphic = true;
            Item.crit = 4;
            Item.useAmmo = AmmoID.Bullet;
            Item.shootSpeed = 27;
            Item.shoot = ProjectileID.Bullet;
            Item.autoReuse = true;
            Item.useTime = 6;
            Item.useAnimation = 6;
        }

        public override void UpdateInventory(Player player)
        {
            if (!player.controlUseItem || player.HeldItem != Item)
            {
                if (CrazyValue > 0)
                {
                    CrazyValue -= 1 / 20f;
                }
                else
                {
                    CrazyValue = 0;
                }
            }
        }
        public override void HoldItem(Player player)
        {
            if(player.controlUseItem)
            {
                if(CrazyValue < 16)
                {
                    CrazyValue += 1 / 30f;
                }
                else
                {
                    CrazyValue = 16;
                }
            }
        }
        public override bool CanUseItem(Player player)
        {
            if (player.controlUseItem)
            {
                return true;
            }
            else
            {
                return CrazyValue == 0;
            }
        }
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            if (player.ownedProjectileCounts[ModContent.ProjectileType<Projectiles.Weapons.RampageShark>()] <= 0 && CrazyValue == 0)
            {
                Projectile.NewProjectile(Item.GetSource_FromAI(), position, velocity, ModContent.ProjectileType<Projectiles.Weapons.RampageShark>(), damage, knockback, player.whoAmI);
            }
            return false;
        }
        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            Main.NewText(CrazyValue, Color.Green);
            return Main.rand.NextBool(4);
        }
        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.Megashark, 1)
                .AddTile(TileID.MythrilAnvil)
                .Register();
        }
    }
}
