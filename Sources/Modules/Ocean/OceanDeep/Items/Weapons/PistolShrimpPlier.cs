using Terraria.Audio;
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


namespace Everglow.Ocean.OceanDeep.Items.Weapons
{
	// Token: 0x020003FC RID: 1020
    public class PistolShrimpPlier : ModItem
	{
		// Token: 0x06001381 RID: 4993 RVA: 0x0008E404 File Offset: 0x0008C604
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault(".");
			// base.Tooltip.SetDefault(".");
            //base.DisplayName.AddTranslation(GameCulture.Chinese, "爆音虾钳");
			//base.Tooltip.AddTranslation(GameCulture.Chinese, "打碎敌人的耳膜,放出200分贝的音波\n静止的敌人高概率闪避,移动的敌人更容易被击中\n距离越近命中率越高");
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0008E45C File Offset: 0x0008C65C
		public override void SetDefaults()
		{
			base.Item.damage = 300;
			base.Item.DamageType = DamageClass.Magic;
			base.Item.mana = 11;
			base.Item.width = 28;
			base.Item.height = 30;
			base.Item.useTime = 40;
			base.Item.useAnimation = 40;
			base.Item.useStyle = 5;
			base.Item.noMelee = true;
			base.Item.knockBack = 6f;
			base.Item.value = 600000;
			base.Item.rare = 11;
			base.Item.UseSound = SoundID.Item8;
			base.Item.autoReuse = true;
			base.Item.shoot = base.Mod.Find<ModProjectile>("CrackSoundWave").Type;
        }
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
            SoundEngine.PlaySound(new SoundStyle("Everglow/Ocean/Sounds/Item/烟花爆炸"), (int)position.X + player.direction * 36, (int)position.Y - 9);
            for (int k = 0; k < 4; k++)
            {
                Projectile.NewProjectile(null, position.X + player.direction * 36f, position.Y - 9f, 0, 0, type, damage, knockback, Main.myPlayer, k, 0f);
            }
			return false;
        }
	}
}
