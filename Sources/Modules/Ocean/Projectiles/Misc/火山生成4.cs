using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MythMod.MiscImplementation;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.World.Generation;
using MythMod.Tiles;
using Terraria.ModLoader;

namespace MythMod.Projectiles.Ocean
{
    // Token: 0x0200054E RID: 1358
    public class 火山生成4 : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("火山生成");
        }
        public override void SetDefaults()
        {
            base.projectile.width = 20;
            base.projectile.height = 20;
            base.projectile.friendly = true;
            base.projectile.alpha = 255;
            base.projectile.timeLeft = 600;
            base.projectile.penetrate = 1;
            projectile.extraUpdates = (int)2f;
            base.projectile.magic = true;
            projectile.ignoreWater = true;
            projectile.tileCollide = true;
        }
        public override void AI()
        {
            projectile.velocity.Y += 2.5f;
        }
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void Kill(int timeLeft)//熔岩心石
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            if (projectile.ai[0] != 20)
            {
                if (!Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16].active())
                {
                    WorldGen.PlaceTile((int)projectile.position.X / 16, (int)projectile.position.Y / 16, (ushort)mod.TileType("熔岩心石"), true, false, -1, 0);
                }
            }
            else
            {
                WorldGen.PlaceTile((int)projectile.position.X / 16 - 1, (int)projectile.position.Y / 16 - 1, (ushort)mod.TileType("地热"), true, false, -1, 0);
            }
        }
    }
}
