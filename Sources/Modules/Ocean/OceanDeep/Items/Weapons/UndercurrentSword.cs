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
namespace MythMod.Items.Weapons
{
    public class UndercurrentSword : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.AddTranslation(GameCulture.Chinese, "深渊暗流");
            Tooltip.SetDefault("击中敌人引发浑浊的深渊爆炸");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            item.damage = 300;//伤害
            item.melee = true;//是否是近战
            item.width = 58;//宽
            item.height = 58;//高
            item.useTime = 27;//使用时挥动间隔时间
            item.rare = 4;//品质
            item.useAnimation = 16;//挥动时动作持续时间
            item.useStyle = 1;//使用动画，这里是挥动
            item.knockBack = 1.5f;//击退
            item.UseSound = SoundID.Item1;//挥动声音
            item.autoReuse = true;//能否持续挥动
            item.crit = 8;//暴击
            item.value = 45000;//价值，1表示一铜币，这里是100铂金币
            item.shoot = mod.ProjectileType("AbyssBeam");//??????????yoyo??
            item.scale = 1f;//大小
            item.shootSpeed = 5f;//??Ч???????
        }
        public override void AddRecipes()
        {
            ModRecipe modRecipe = new ModRecipe(base.mod);
            modRecipe.AddIngredient(null, "DarkSeaBar", 7);
            modRecipe.requiredTile[0] = 412;
            modRecipe.SetResult(this, 1);
            modRecipe.AddRecipe();
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 360, true);
            for (int i = 0; i < 228; i++)
            {
                int num = Dust.NewDust(target.position, target.width, target.height, 127, target.velocity.X * 0f, target.velocity.Y * 0f, 150, new Color(Main.DiscoR, 100, 255), 1.2f);
                Main.dust[num].noGravity = true;
            }
            if (target.type == 488)
            {
                return;
            }
            float num1 = (float)damage * 0.04f;
            if ((int)num1 == 0)
            {
                return;
            }
            Projectile.NewProjectile(target.Center.X, target.Center.Y, 2f, 2f, base.mod.ProjectileType("AbyssBomb"), damage, knockback, player.whoAmI, 0f, 0f);
        }
        // Token: 0x060002D2 RID: 722 RVA: 0x000354AC File Offset: 0x000336AC
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
			Lighting.AddLight(new Vector2((float)hitbox.X, (float)hitbox.Y), (float)(255) * 0.04f / 255f, (float)(255) * 0.016f / 255f, (float)(255) * 0 / 255f);
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 96, 0f, 0f, 0, Color.Blue, 1f);
            if (Main.rand.Next(0,6) == 4)
            {
                Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, 127, 0f, 0f, 0, Color.White, 0.8f);
            }
        }
    }
}
