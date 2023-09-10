using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace Everglow.Ocean.OceanVolcano.Items
{
	public class EvilKiss : ModItem
	{
		public override void SetStaticDefaults()
		{
			// base.DisplayName.SetDefault("");
			// base.Tooltip.SetDefault("");
			// base.// DisplayName.AddTranslation(GameCulture.Chinese, "恶魔之吻");
			// base.Tooltip.AddTranslation(GameCulture.Chinese, "击中怪物造成毁灭一般的伤害,有概率直接爆炸");
        }
        public override void SetDefaults()
        {
            base.Item.useStyle = 3;
			base.Item.useTurn = false;
			base.Item.useAnimation = 5;
			base.Item.useTime = 5;
			base.Item.width = 36;
			base.Item.height = 36;
			base.Item.damage = 240;
            Item.crit = 27;
            base.Item.DamageType = DamageClass.Melee/* tModPorter Suggestion: Consider MeleeNoSpeed for no attack speed scaling */;
			base.Item.knockBack = 6.5f;
			base.Item.UseSound = SoundID.Item1;
			base.Item.useTurn = true;
			base.Item.autoReuse = true;
			base.Item.value = Item.buyPrice(0, 3, 0, 0);
			base.Item.rare = 11;
		}
        public override void MeleeEffects(Player player, Rectangle hitbox)
		{
			if (Main.rand.Next(5) == 0)
			{
				Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, ModContent.DustType<Everglow.Ocean.Dusts.Flame>(), 0f, 0f, 0, default(Color), 1f);
			}
		}
        public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(24, 900, false);
            target.AddBuff(189, 900, false);
            for(int i = 0;i < 15;i++)
            {
                Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, ModContent.DustType<Everglow.Ocean.Dusts.Flame>(), 0f, 0f, 0, default(Color), 1f);
                Dust.NewDust(new Vector2(target.position.X, target.position.Y), target.width, target.height, ModContent.DustType<Everglow.Ocean.Dusts.Flame>(), 0f, 0f, 0, default(Color), 1.5f);
            }
            target.StrikeNPC((int)(damageDone * Main.rand.NextFloat(0.85f, 1.15f)), 0, 1, Main.rand.Next(100) > 50 ? false : true);
            target.StrikeNPC((int)(damageDone * Main.rand.NextFloat(0.85f, 1.15f)), 0, 1, Main.rand.Next(100) > 50 ? false : true);
            target.StrikeNPC((int)(damageDone * Main.rand.NextFloat(0.85f, 1.15f)), 0, 1, Main.rand.Next(100) > 50 ? false : true);
            if(Main.rand.Next(10) == 1)
            {
                target.StrikeNPC((int)(damageDone * Main.rand.NextFloat(8.5f, 11.5f)), 0, 1, Main.rand.Next(100) > 50 ? false : true);
                for (int k = 0; k <= 10; k++)
                {
                    float a = (float)Main.rand.Next(0, 720) / 360 * 3.141592653589793238f;
                    float m = (float)Main.rand.Next(0, 50000);
                    float l = (float)Main.rand.Next((int)m, 50000) / 1800;
                    int num4 = Projectile.NewProjectile(null, target.Center.X, target.Center.Y, (float)((float)l * Math.Cos((float)a)) * 0.36f, (float)((float)l * Math.Sin((float)a)) * 0.36f,ModContent.ProjectileType<Everglow.Ocean.OceanVolcano.Projectiles.火山溅射>(), damageDone, 0, Main.myPlayer, 0f, 0f);
                    Main.projectile[num4].scale = (float)Main.rand.Next(7000, 13000) / 10000f;
                }
            }
        }
    }
}
