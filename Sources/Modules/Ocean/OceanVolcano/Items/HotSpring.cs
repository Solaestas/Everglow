using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
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
using Terraria.Graphics.Shaders;
// Token: 0x02000363 RID: 867
namespace MythMod.Items.Weapons
{
	// Token: 0x02000363 RID: 867
    public class HotSpring : ModItem
	{
		// Token: 0x060010A9 RID: 4265 RVA: 0x00007320 File Offset: 0x00005520
		public override void SetStaticDefaults()
		{
            base.DisplayName.SetDefault("深海热泉");
			Item.staff[base.item.type] = true;
            base.DisplayName.AddTranslation(GameCulture.Chinese, "深海热泉");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.damage = 275;
			base.item.magic = true;
			base.item.mana = 26;
			base.item.width = 70;
			base.item.height = 68;
			base.item.useTime = 30;
			base.item.useAnimation = 30;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 2.4f;
			base.item.value = 100000;
			base.item.rare = 11;
			base.item.UseSound = SoundID.Item60;
			base.item.autoReuse = true;
            base.item.shoot = base.mod.ProjectileType("深海热泉");
			base.item.shootSpeed = 0.1f;
            base.item.flame = true;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0007B59C File Offset: 0x0007979C
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, mod.ProjectileType("深海热泉"), damage, knockBack, player.whoAmI, 1f);

            return false;
        }
        // Token: 0x060010AB RID: 4267 RVA: 0x0007B59C File Offset: 0x0007979C
        public override void PostUpdate()
        {
            Lighting.AddLight((int)((base.item.position.X + (float)(base.item.width / 1.05f)) / 16f), (int)(base.item.position.Y  / 16f), 0.125f, 0.025f, 0.025f);
        }
	}
}
