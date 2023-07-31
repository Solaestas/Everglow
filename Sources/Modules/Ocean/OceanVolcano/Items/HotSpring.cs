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
namespace Everglow.Ocean.Items.Weapons
{
	// Token: 0x02000363 RID: 867
    public class HotSpring : ModItem
	{
		// Token: 0x060010A9 RID: 4265 RVA: 0x00007320 File Offset: 0x00005520
		public override void SetStaticDefaults()
		{
            // base.DisplayName.SetDefault("深海热泉");
			Item.staff[base.Item.type] = true;
            // base.DisplayName.AddTranslation(GameCulture.Chinese, "深海热泉");
            GetGlowMask = Everglow.Ocean.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            Item.glowMask = GetGlowMask;
            base.Item.damage = 275;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 26;
			base.Item.width = 70;
			base.Item.height = 68;
			base.Item.useTime = 30;
			base.Item.useAnimation = 30;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 2.4f;
			base.Item.value = 100000;
			base.Item.rare = 11;
			base.Item.UseSound = SoundID.Item60;
			base.Item.autoReuse = true;
            base.Item.shoot =ModContent.ProjectileType<Everglow.Ocean.Projectiles.深海热泉>();
			base.Item.shootSpeed = 0.1f;
            base.Item.flame = true;
		}

		// Token: 0x060010AB RID: 4267 RVA: 0x0007B59C File Offset: 0x0007979C
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            Projectile.NewProjectile(position.X, position.Y, speedX, speedY, ModContent.ProjectileType<Everglow.Ocean.Projectiles.深海热泉>(), damage, knockBack, player.whoAmI, 1f);

            return false;
        }
        // Token: 0x060010AB RID: 4267 RVA: 0x0007B59C File Offset: 0x0007979C
        public override void PostUpdate()
        {
            Lighting.AddLight((int)((base.Item.position.X + (float)(base.Item.width / 1.05f)) / 16f), (int)(base.Item.position.Y  / 16f), 0.125f, 0.025f, 0.025f);
        }
	}
}
