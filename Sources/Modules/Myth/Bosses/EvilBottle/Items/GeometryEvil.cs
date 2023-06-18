using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using System;
using Everglow.Myth.MiscItems.Projectiles.Weapon.Magic;
using Everglow.Myth.Bosses.EvilBottle.Projectiles;

namespace Everglow.Myth.Bosses.EvilBottle.Items
{
    public class GeometryEvil : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault(".");
			// base.Tooltip.SetDefault(".");
            //base.DisplayName.AddTranslation(GameCulture.Chinese, "几何魔焰");
			//base.Tooltip.AddTranslation(GameCulture.Chinese, "");
        }
        public override void SetDefaults()
        {
            base.Item.damage = 54;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 28;
			base.Item.width = 28;
			base.Item.height = 30;
			base.Item.useTime = 27;
			base.Item.useAnimation = 27;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 6f;
			base.Item.value = 2000;
			base.Item.rare = 3;
			base.Item.UseSound = SoundID.Item8;
			base.Item.autoReuse = true;
			base.Item.shoot = ModContent.ProjectileType<BoneFeather>();
			base.Item.shootSpeed = 9f;
        }
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			Vector2 v0 = velocity;
            float Rot = Main.rand.NextFloat(0, (float)Math.PI * 2);
            double io = Main.rand.NextFloat(0, 10f);
            Vector2 P1 = new Vector2(0, 50).RotatedBy(Rot);
            Vector2 P2 = new Vector2(0, 50).RotatedBy(Rot + Math.PI * 0.6666666667d);
            Vector2 P3 = new Vector2(0, 50).RotatedBy(Rot + Math.PI * 1.3333333333d);
            for (int i = 0; i < 6; i++)
            {
                float m = i / 6f;
                Vector2 v = (P1 * m + P2 * (1 - m)) * 0.1f + v0;
                Projectile.NewProjectile(source, player.Center.X, player.Center.Y, v.X, v.Y, ModContent.ProjectileType<DarkFlameball3>(), damage, 0f, Main.myPlayer, 0f, 0f);
            }
            for (int i = 0; i < 6; i++)
            {
                float m = i / 6f;
                Vector2 v = (P2 * m + P3 * (1 - m)) * 0.1f + v0;
                Projectile.NewProjectile(source, player.Center.X, player.Center.Y, v.X, v.Y, ModContent.ProjectileType<DarkFlameball3>(), damage, 0f, Main.myPlayer, 0f, 0f);
            }
            for (int i = 0; i < 6; i++)
            {
                float m = i / 6f;
                Vector2 v = (P3 * m + P1 * (1 - m)) * 0.1f + v0;
                Projectile.NewProjectile(source, player.Center.X, player.Center.Y, v.X, v.Y, ModContent.ProjectileType<DarkFlameball3>(), damage, 0f, Main.myPlayer, 0f, 0f);
            }
            return false;
        }
    }
}
