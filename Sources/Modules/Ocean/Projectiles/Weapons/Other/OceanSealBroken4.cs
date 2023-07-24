using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader.IO;
namespace MythMod.Projectiles.projectile2
{
    //135596
    public class OceanSealBroken4 : ModProjectile
    {
        //4444444
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("海洋封印碎块");
        }
        //7359668
        public override void SetDefaults()
        {
            projectile.width = 44;
            projectile.height = 44;
            projectile.aiStyle = -1;
            projectile.friendly = false;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
            projectile.timeLeft = 900;
            projectile.hostile = false;
            projectile.penetrate = 1;
            projectile.scale = 1f;
            this.cooldownSlot = 1;
        }
        //55555
        private bool initialization = true;
        private float b;
        public override void AI()
        {
            projectile.velocity.Y += 0.1f;
            base.projectile.rotation += (float)System.Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) / 35f;
        }
        //14141414141414
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)base.projectile.position.X, (int)base.projectile.position.Y, 27, 1f, 0f);
            float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
            for (int a = 0; a < 90; a++)
            {
                Vector2 vector = projectile.position;
                Vector2 v = new Vector2(0, Main.rand.NextFloat(6f, 7.5f)).RotatedByRandom(Math.PI * 2);
                int num = Dust.NewDust(vector - new Vector2(4, 4), projectile.width, projectile.height, mod.DustType("Crystal2"), v.X, v.Y, 0, default(Color), 1.4f);
                Main.dust[num].noGravity = false;
                Main.dust[num].fadeIn = 1f + (float)Main.rand.NextFloat(-0.5f, 0.5f) * 0.1f;
            }
            Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石1"), 1f);
            Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石2"), 1f);
            Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石5"), 1f);
            Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石5"), 1f);
            Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石5"), 1f);
            Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石5"), 1f);
            Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石3"), 1f);
            Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石4"), 1f);
        }
    }
}