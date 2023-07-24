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
    public class 火山生成3 : ModProjectile
    {
        // Token: 0x06001DC0 RID: 7616 RVA: 0x0000C2EA File Offset: 0x0000A4EA
        public override void SetStaticDefaults()
        {
            base.DisplayName.SetDefault("火山生成");
        }

        // Token: 0x06001DC1 RID: 7617 RVA: 0x0017F224 File Offset: 0x0017D424
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

        // Token: 0x06001DC2 RID: 7618 RVA: 0x0017F28C File Offset: 0x0017D48C
        public override void AI()
        {
            projectile.velocity.Y -= 20f;
        }
        // Token: 0x06001DC3 RID: 7619 RVA: 0x0017F570 File Offset: 0x0017D770
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void Kill(int timeLeft)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            int num = 0;
            if (!Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16].active() && Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16 - 3].type == mod.TileType("玄武岩礁石"))
            {
                int ix = (int)projectile.position.X / 16, jx = (int)projectile.position.Y / 16 + 1;
                num = Main.rand.Next(0, 1200);
                if (num < 200 && num >= 0)
                {
                    WorldGen.PlaceTile((int)projectile.position.X / 16, (int)projectile.position.Y / 16, (ushort)mod.TileType("熔岩结晶"), true, false, -1, 0);
                }
                if (num < 400 && num >= 200)
                {
                    WorldGen.PlaceTile((int)projectile.position.X / 16, (int)projectile.position.Y / 16, (ushort)mod.TileType("熔岩结晶2"), true, false, -1, 0);
                }
                if (num < 600 && num >= 400)
                {
                    WorldGen.PlaceTile((int)projectile.position.X / 16, (int)projectile.position.Y / 16, (ushort)mod.TileType("熔岩结晶3"), true, false, -1, 0);
                }
                if (num < 800 && num >= 600)
                {
                    WorldGen.PlaceTile((int)projectile.position.X / 16, (int)projectile.position.Y / 16, (ushort)mod.TileType("熔岩结晶4"), true, false, -1, 0);
                }
                if (num < 1000 && num >= 800)
                {
                    WorldGen.PlaceTile((int)projectile.position.X / 16, (int)projectile.position.Y / 16, (ushort)mod.TileType("熔岩结晶5"), true, false, -1, 0);
                }
                if (num < 1200 && num >= 1000)
                {
                    WorldGen.PlaceTile((int)projectile.position.X / 16, (int)projectile.position.Y / 16, (ushort)mod.TileType("熔岩结晶6"), true, false, -1, 0);
                }
            }
        }
    }
}
