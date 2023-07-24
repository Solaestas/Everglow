using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace MythMod.Items.Volcano
{
	public class EvilKiss : ModItem
	{
		public override void SetStaticDefaults()
		{
			base.DisplayName.SetDefault("");
			base.Tooltip.SetDefault("");
			base.DisplayName.AddTranslation(GameCulture.Chinese, "恶魔之吻");
			base.Tooltip.AddTranslation(GameCulture.Chinese, "击中怪物造成毁灭一般的伤害,有概率直接爆炸");
            GetGlowMask = MythMod.SetStaticDefaultsGlowMask(this);
        }
        public static short GetGlowMask = 0;
        public override void SetDefaults()
        {
            item.glowMask = GetGlowMask;
            base.item.useStyle = 3;
			base.item.useTurn = false;
			base.item.useAnimation = 5;
			base.item.useTime = 5;
			base.item.width = 36;
			base.item.height = 36;
			base.item.damage = 240;
            item.crit = 27;
            base.item.melee = true;
			base.item.knockBack = 6.5f;
			base.item.UseSound = SoundID.Item1;
			base.item.useTurn = true;
			base.item.autoReuse = true;
			base.item.value = Item.buyPrice(0, 3, 0, 0);
			base.item.rare = 11;
		}
        public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(5) == 0)
			{
				Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("Flame"), 0f, 0f, 0, default(Color), 1f);
			}
		}
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            target.AddBuff(24, 900, false);
            target.AddBuff(189, 900, false);
            for(int i = 0;i < 15;i++)
            {
                Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, mod.DustType("Flame"), 0f, 0f, 0, default(Color), 1f);
                Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, mod.DustType("Flame"), 0f, 0f, 0, default(Color), 1.5f);
            }
            target.StrikeNPC((int)(damage * Main.rand.NextFloat(0.85f, 1.15f)), 0, 1, Main.rand.Next(100) > 50 ? false : true);
            target.StrikeNPC((int)(damage * Main.rand.NextFloat(0.85f, 1.15f)), 0, 1, Main.rand.Next(100) > 50 ? false : true);
            target.StrikeNPC((int)(damage * Main.rand.NextFloat(0.85f, 1.15f)), 0, 1, Main.rand.Next(100) > 50 ? false : true);
            if(Main.rand.Next(10) == 1)
            {
                target.StrikeNPC((int)(damage * Main.rand.NextFloat(8.5f, 11.5f)), 0, 1, Main.rand.Next(100) > 50 ? false : true);
                for (int k = 0; k <= 10; k++)
                {
                    float a = (float)Main.rand.Next(0, 720) / 360 * 3.141592653589793238f;
                    float m = (float)Main.rand.Next(0, 50000);
                    float l = (float)Main.rand.Next((int)m, 50000) / 1800;
                    int num4 = Projectile.NewProjectile(target.Center.X, target.Center.Y, (float)((float)l * Math.Cos((float)a)) * 0.36f, (float)((float)l * Math.Sin((float)a)) * 0.36f, base.mod.ProjectileType("火山溅射"), damage, 0, Main.myPlayer, 0f, 0f);
                    Main.projectile[num4].scale = (float)Main.rand.Next(7000, 13000) / 10000f;
                }
            }
        }
    }
}
