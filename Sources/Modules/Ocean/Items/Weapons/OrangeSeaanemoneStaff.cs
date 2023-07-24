using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using System;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Collections.Generic;
using System.IO;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Achievements;
namespace MythMod.Items.Weapons.OceanWeapons
{
	// Token: 0x02000363 RID: 867
    public class OrangeSeaanemoneStaff : ModItem
	{
		// Token: 0x060010A9 RID: 4265 RVA: 0x00007320 File Offset: 0x00005520
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("");
			Item.staff[base.item.type] = true;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "菊花海葵召唤杖");
		}

		// Token: 0x060010AA RID: 4266 RVA: 0x0007B4A8 File Offset: 0x000796A8
		public override void SetDefaults()
		{
			base.item.damage = 200;
			base.item.magic = true;
			base.item.mana = 3;
			base.item.width = 88;
			base.item.height = 86;
			base.item.useTime = 24;
			base.item.useAnimation = 24;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 1.5f;
			base.item.value = 3000;
			base.item.rare = 11;
			base.item.UseSound = SoundID.Item60;
			base.item.autoReuse = true;
			base.item.shootSpeed = 1f;
            base.item.shoot = base.mod.ProjectileType("菊花海葵");
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            float shootSpeed = base.item.shootSpeed;
            Vector2 vector = player.RotatedRelativePoint(player.MountedCenter, true);
            float num = (float)Main.mouseX + Main.screenPosition.X - vector.X;
            float num2 = (float)Main.mouseY + Main.screenPosition.Y - vector.Y;
            if (player.gravDir == -1f)
            {
                num2 = Main.screenPosition.Y + (float)Main.screenHeight - (float)Main.mouseY - vector.Y;
            }
            float num3 = (float)Math.Sqrt((double)(num * num + num2 * num2));
            if ((float.IsNaN(num) && float.IsNaN(num2)) || (num == 0f && num2 == 0f))
            {
                num = (float)player.direction;
            }
            else
            {
                num3 = shootSpeed / num3;
            }
            num = 0f;
            num2 = 0f;
            vector.X = (float)Main.mouseX + Main.screenPosition.X;
            vector.Y = (float)Main.mouseY + Main.screenPosition.Y;
            Projectile.NewProjectile(vector.X, vector.Y, num, num2, base.mod.ProjectileType("菊花海葵"), damage, knockBack, player.whoAmI, 0f, 0f);
            return false;
        }
        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "OrangeSeaAnemone", 8); //需要一个材料
            recipe.AddIngredient(null, "OceanDustCore", 8); //需要一个材料
            recipe.requiredTile[0] = 412;
            recipe.SetResult(this, 1); //制作一个武器
            recipe.AddRecipe();
        }
        // Token: 0x060010AB RID: 4267 RVA: 0x0007B59C File Offset: 0x0007979C
    }
}
