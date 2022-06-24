using Terraria.DataStructures;

namespace Everglow.Sources.Modules.MythModule.Genshin.Weapons
{
    public class PrimordialJadeWingedSpear : ModItem
    {
        public override void SetStaticDefaults()
        {
        }
        public override void SetDefaults()
        {
            Item.rare = 8;
            Item.value = 500000;

            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAnimation = 180;
            Item.useTime = 180;
            Item.UseSound = SoundID.Item71;
            Item.autoReuse = false;

            Item.damage = 674;
            Item.crit = 22;
            Item.knockBack = 6.5f;
            Item.noUseGraphic = true;
            Item.DamageType = DamageClass.Melee;
            Item.noMelee = true; 

            Item.shootSpeed = 0.3f;
            Item.shoot = ModContent.ProjectileType<Projectiles.PrimordialJadeWingedSpear>();
        } 
    }
}
