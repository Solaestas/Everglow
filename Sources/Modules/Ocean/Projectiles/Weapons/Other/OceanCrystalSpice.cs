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
    public class OceanCrystalSpice : ModProjectile
    {
        //4444444
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("海洋晶石");
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
            projectile.hostile = true;
            projectile.penetrate = 1;
            projectile.scale = 1f;
            projectile.alpha = 255;
            this.cooldownSlot = 1;
        }
        //55555
        private bool initialization = true;
        private float b;
        public override void AI()
        {
            if(projectile.alpha > 50)
            {
                projectile.alpha -= 10;
            }
            projectile.velocity.Y += 0.1f;
            int dustID = Dust.NewDust(projectile.position + new Vector2(11, 11), projectile.width / 2, projectile.height / 2, 67, projectile.velocity.X * 0.03f, projectile.velocity.Y * 0.03f, 201, default(Color), 1f);/*粉尘效果不用管*/
            base.projectile.rotation = (float)System.Math.Atan2((double)projectile.velocity.Y, (double)projectile.velocity.X) + (float)Math.PI * 0.25f;
        }
        //14141414141414
        public override void Kill(int timeLeft)
        {
            Main.PlaySound(2, (int)base.projectile.position.X, (int)base.projectile.position.Y, 27, 1f, 0f);
            float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
            for (int a = 0; a < 20; a++)
            {
                Vector2 vector = base.projectile.Center;
                Vector2 v = new Vector2(0, Main.rand.NextFloat(6f, 7.5f)).RotatedByRandom(Math.PI * 2);
                int num = Dust.NewDust(vector - new Vector2(4, 4), 2, 2, mod.DustType("Crystal2"), v.X, v.Y, 0, default(Color), 1.4f);
                Main.dust[num].noGravity = false;
                Main.dust[num].fadeIn = 1f + (float)Main.rand.NextFloat(-0.5f, 0.5f) * 0.1f;
            }
            //Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石1"), 1f);
            //Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石2"), 1f);
            //Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石3"), 1f);
            //Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石2"), 1f);
            //Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石3"), 1f);
            //Gore.NewGore(base.projectile.position, base.projectile.velocity * scaleFactor, base.mod.GetGoreSlot("Gores/海洋晶石4"), 1f);
        }
    }
}