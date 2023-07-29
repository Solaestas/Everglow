using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Everglow.Ocean.MiscImplementation;
using Terraria;
using Terraria.GameContent.Generation;
using Everglow.Ocean.Tiles;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace Everglow.Ocean.Projectiles.Ocean
{
    // Token: 0x0200054E RID: 1358
    public class 火山生成4 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // // base.DisplayName.SetDefault("火山生成");
        }
        public override void SetDefaults()
        {
            base.Projectile.width = 20;
            base.Projectile.height = 20;
            base.Projectile.friendly = true;
            base.Projectile.alpha = 255;
            base.Projectile.timeLeft = 600;
            base.Projectile.penetrate = 1;
            Projectile.extraUpdates = (int)2f;
            base.Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
        }
        public override void AI()
        {
            Projectile.velocity.Y += 2.5f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void Kill(int timeLeft)//熔岩心石
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            if (Projectile.ai[0] != 20)
            {
                if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].HasTile)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)Mod.Find<ModTile>("熔岩心石").Type, true, false, -1, 0);
                }
            }
            else
            {
                WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)Mod.Find<ModTile>("地热").Type, true, false, -1, 0);
            }
        }
    }
}
