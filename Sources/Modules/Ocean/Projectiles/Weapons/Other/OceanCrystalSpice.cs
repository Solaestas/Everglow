using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.ModLoader.IO;
namespace Everglow.Ocean.Projectiles.projectile2
{
    //135596
    public class OceanCrystalSpice : ModProjectile
    {
        //4444444
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("海洋晶石");
        }
        //7359668
        public override void SetDefaults()
        {
            Projectile.width = 44;
            Projectile.height = 44;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.timeLeft = 900;
            Projectile.hostile = true;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            Projectile.alpha = 255;
            this.CooldownSlot = 1;
        }
        //55555
        private bool initialization = true;
        private float b;
        public override void AI()
        {
            if(Projectile.alpha > 50)
            {
                Projectile.alpha -= 10;
            }
            Projectile.velocity.Y += 0.1f;
            int dustID = Dust.NewDust(Projectile.position + new Vector2(11, 11), Projectile.width / 2, Projectile.height / 2, 67, Projectile.velocity.X * 0.03f, Projectile.velocity.Y * 0.03f, 201, default(Color), 1f);/*粉尘效果不用管*/
            base.Projectile.rotation = (float)System.Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) + (float)Math.PI * 0.25f;
        }
        //14141414141414
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, new Vector2(base.Projectile.position.X, base.Projectile.position.Y));
            float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
            for (int a = 0; a < 20; a++)
            {
                Vector2 vector = base.Projectile.Center;
                Vector2 v = new Vector2(0, Main.rand.NextFloat(6f, 7.5f)).RotatedByRandom(Math.PI * 2);
                int num = Dust.NewDust(vector - new Vector2(4, 4), 2, 2, ModContent.DustType<Everglow.Ocean.Dusts.Crystal2>(), v.X, v.Y, 0, default(Color), 1.4f);
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