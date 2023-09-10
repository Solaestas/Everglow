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
namespace Everglow.Ocean.Projectiles
{
    //135596
    public class OceanSealBroken3 : ModProjectile
    {
        //4444444
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("海洋封印碎块");
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
            Projectile.hostile = false;
            Projectile.penetrate = 1;
            Projectile.scale = 1f;
            this.CooldownSlot = 1;
        }
        //55555
        private bool initialization = true;
        private float b;
        public override void AI()
        {
            Projectile.velocity.Y += 0.1f;
            base.Projectile.rotation += (float)System.Math.Atan2((double)Projectile.velocity.Y, (double)Projectile.velocity.X) / 35f;
        }
        //14141414141414
        public override void Kill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.Item27, new Vector2(base.Projectile.position.X, base.Projectile.position.Y));
            float scaleFactor = (float)(Main.rand.Next(-200, 200) / 100);
            Gore.NewGore(base.Projectile.position, base.Projectile.velocity * scaleFactor, ModContent.Find<ModGore>("Everglow/海洋晶石1").Type, 1f);
            Gore.NewGore(base.Projectile.position, base.Projectile.velocity * scaleFactor, ModContent.Find<ModGore>("Everglow/海洋晶石2").Type, 1f);
            Gore.NewGore(base.Projectile.position, base.Projectile.velocity * scaleFactor, ModContent.Find<ModGore>("Everglow/海洋晶石5").Type, 1f);
            Gore.NewGore(base.Projectile.position, base.Projectile.velocity * scaleFactor, ModContent.Find<ModGore>("Everglow/海洋晶石5").Type, 1f);
            Gore.NewGore(base.Projectile.position, base.Projectile.velocity * scaleFactor, ModContent.Find<ModGore>("Everglow/海洋晶石5").Type, 1f);
            Gore.NewGore(base.Projectile.position, base.Projectile.velocity * scaleFactor, ModContent.Find<ModGore>("Everglow/海洋晶石5").Type, 1f);
            Gore.NewGore(base.Projectile.position, base.Projectile.velocity * scaleFactor, ModContent.Find<ModGore>("Everglow/海洋晶石3").Type, 1f);
            Gore.NewGore(base.Projectile.position, base.Projectile.velocity * scaleFactor, ModContent.Find<ModGore>("Everglow/海洋晶石4").Type, 1f);
            for (int a = 0; a < 90; a++)
            {
                Vector2 vector = Projectile.position;
                Vector2 v = new Vector2(0, Main.rand.NextFloat(6f, 7.5f)).RotatedByRandom(Math.PI * 2);
                int num = Dust.NewDust(vector - new Vector2(4, 4), Projectile.width, Projectile.height, ModContent.DustType<Everglow.Ocean.Dusts.Crystal2>(), v.X, v.Y, 0, default(Color), 1.4f);
                Main.dust[num].noGravity = false;
                Main.dust[num].fadeIn = 1f + (float)Main.rand.NextFloat(-0.5f, 0.5f) * 0.1f;
            }
        }
    }
}