using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
using System;

namespace MythMod.Items.Weapons.OceanWeapons
{
    public class Wave : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("");
            DisplayName.AddTranslation(GameCulture.Chinese, "波江");
        }
        private int num = 0;
        private bool k = true;
        public override void SetDefaults()
        {
            item.damage = 222;
            item.melee = true;
            item.width = 68;
            item.height = 68;
            item.useTime = 29;
            item.rare = 11;
            item.useAnimation = 30;
            item.useStyle = 1;
            item.knockBack = 2;
            item.UseSound = SoundID.Item1;
            item.autoReuse = true;
            item.crit = 7;
            item.value = 14000;
            item.scale = 1f;
            item.shoot = mod.ProjectileType("Wave");
            item.shootSpeed = 5f;
            item.useTurn = false;
        }
        public override void OnHitNPC(Player player, NPC target, int damage, float knockback, bool crit)
        {
            if(Main.rand.Next(100) > 10)
            {
                int num = Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, base.mod.ProjectileType("WaveBallMini"), item.damage * 2, item.knockBack, Main.myPlayer, 0f, 0f);
                Main.projectile[num].timeLeft = 1;
            }
            else
            {
                int num = Projectile.NewProjectile(target.Center.X, target.Center.Y, 0, 0, base.mod.ProjectileType("WaveBall"), item.damage * 10, item.knockBack, Main.myPlayer, 0f, 0f);
                Main.projectile[num].timeLeft = 1;
            }
        }
        public override void MeleeEffects(Player player, Rectangle hitbox)
        {
            Dust.NewDust(new Vector2((float)hitbox.X, (float)hitbox.Y), hitbox.Width, hitbox.Height, mod.DustType("Wave"), 0f, 0f, 0,  default(Color), 0.6f);
        }
        public override bool Shoot(Player player, ref Vector2 position, ref float speedX, ref float speedY, ref int type, ref int damage, ref float knockBack)
        {
            Vector2 vp0 = new Vector2(-100f * player.direction + player.width / 2f, -50f).RotatedBy(0) + player.position;
            Vector2 pc = player.position + new Vector2(player.width, player.height) / 2;
            Projectile.NewProjectile(pc.X, pc.Y, 0, 0, mod.ProjectileType("WaveShader"), 0, 0, player.whoAmI);
            return false;
        }
    }
}
