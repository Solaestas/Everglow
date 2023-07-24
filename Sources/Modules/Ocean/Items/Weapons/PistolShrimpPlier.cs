﻿using Terraria.ID;
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


namespace MythMod.Items.Weapons.OceanWeapons
{
	// Token: 0x020003FC RID: 1020
    public class PistolShrimpPlier : ModItem
	{
		// Token: 0x06001381 RID: 4993 RVA: 0x0008E404 File Offset: 0x0008C604
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault(".");
			base.Tooltip.SetDefault(".");
            base.DisplayName.AddTranslation(GameCulture.Chinese, "爆音虾钳");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "打碎敌人的耳膜,放出200分贝的音波\n静止的敌人高概率闪避,移动的敌人更容易被击中\n距离越近命中率越高");
		}

		// Token: 0x06001382 RID: 4994 RVA: 0x0008E45C File Offset: 0x0008C65C
		public override void SetDefaults()
		{
			base.item.damage = 300;
			base.item.magic = true;
			base.item.mana = 11;
			base.item.width = 28;
			base.item.height = 30;
			base.item.useTime = 40;
			base.item.useAnimation = 40;
			base.item.useStyle = 5;
			base.item.noMelee = true;
			base.item.knockBack = 6f;
			base.item.value = 600000;
			base.item.rare = 11;
			base.item.UseSound = SoundID.Item8;
			base.item.autoReuse = true;
			base.item.shoot = base.mod.ProjectileType("CrackSoundWave");
        }
        // Token: 0x0600462B RID: 17963 RVA: 0x0027BBA8 File Offset: 0x00279DA8
		public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
		{
            Main.PlaySound(mod.GetLegacySoundSlot(SoundType.Item, "Sounds/Item/烟花爆炸"), (int)position.X + player.direction * 36, (int)position.Y - 9);
            for (int k = 0; k < 4; k++)
            {
                Projectile.NewProjectile(position.X + player.direction * 36f, position.Y - 9f, 0, 0, type, damage, knockBack, Main.myPlayer, k, 0f);
            }
			return false;
        }
	}
}
