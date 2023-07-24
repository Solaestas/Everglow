using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Localization;
using Terraria;
namespace MythMod.Items.Weapons
{
    public class WaveBlade : ModItem
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.AddTranslation(GameCulture.Chinese, "浪花之刃");
            base.Tooltip.AddTranslation(GameCulture.Chinese, "浪花之刃");
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "OceanBlueBar", 12);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override void SetDefaults()
        {
            item.damage = 237;
            item.melee = true;
            item.width = 60;
            item.height = 80;
            item.useTime = 36;
            item.rare = 11;
            item.useAnimation = 12;
            item.useStyle = 1;
            item.knockBack = 1.4f;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit =8;
            item.value = 40000;
            item.scale = 1f;
            item.shoot = mod.ProjectileType("WaveBall");
            item.shootSpeed = 12;
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
			float shootSpeed = base.item.shootSpeed;
			Projectile.NewProjectile((float)position.X, (float)position.Y, (float)speedX, (float)speedY, (int)type, (int)damage * 7, (float)knockBack, player.whoAmI, 0f, 0f);
			return false;
		}
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height / 3, 33, 0f, 0f, 0, default(Color), 1f);
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, (int)(hitbox.Height / 1.5f), 33, 0f, 0f, 0, default(Color), 1.5f);
            int num = Main.rand.Next(3);
            if (num == 0)
            {
                num = 56;
            }
            else if (num == 1)
            {
                num = 56;
            }
            else
            {
                num = 277;
            }
            if (Main.rand.Next(3) == 0)
			{
                Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, num, 0f, 0f, 0, default(Color), 0.7f);
            }
        }
    }
}
