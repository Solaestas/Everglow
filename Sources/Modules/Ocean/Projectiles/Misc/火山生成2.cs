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
    public class 火山生成2 : ModProjectile
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
            projectile.velocity.Y += 20f;
        }
        // Token: 0x06001DC3 RID: 7619 RVA: 0x0017F570 File Offset: 0x0017D770
        public override void OnHitNPC(NPC target, int damage, float knockback, bool crit)
        {
        }
        public override void Kill(int timeLeft)
        {
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            int num = 0;
            if (!Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16].active() && Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16].wall != mod.WallType("熔岩石墙"))
            {
                int ix = (int)projectile.position.X / 16, jx = (int)projectile.position.Y / 16 + 1;
                num = Main.rand.Next(3440, 5120);
                if (num < 4240 && num >= 3440 && Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16 + 1].type == mod.TileType("玄武岩礁石"))
                {
                    WorldGen.PlaceTile((int)projectile.position.X / 16, (int)projectile.position.Y / 16, (ushort)mod.TileType("小硫磺石"), true, false, -1, 0);
                    int i = (int)projectile.position.X / 16, j = (int)projectile.position.Y / 16;
                    short num8 = (short)(Main.rand.Next(0, 6));
                    if (Main.tile[i, j].type == (ushort)mod.TileType("小硫磺石"))
                    {
                        Main.tile[i, j].frameX = (short)(num8 * 36);
                        Main.tile[i, j - 1].frameX = (short)(num8 * 36);
                    }
                }
                if (num < 5040 && num >= 4240 && Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16 + 1].type == mod.TileType("玄武岩礁石"))//橄榄石晶体
                {
                    WorldGen.PlaceTile((int)projectile.position.X / 16, (int)projectile.position.Y / 16, (ushort)mod.TileType("小橄榄石晶体"), true, false, -1, 0);
                    int i = (int)projectile.position.X / 16, j = (int)projectile.position.Y / 16;
                    short num8 = (short)(Main.rand.Next(0, 6));
                    if (Main.tile[i, j].type == (ushort)mod.TileType("小橄榄石晶体"))
                    {
                        Main.tile[i, j].frameX = (short)(num8 * 36);
                        Main.tile[i, j - 1].frameX = (short)(num8 * 36);
                    }
                }
                if (num < 5120 && num >= 5040 && Main.tile[(int)projectile.position.X / 16, (int)projectile.position.Y / 16 + 1].type == mod.TileType("玄武岩礁石"))//熔岩心石
                {
                    WorldGen.PlaceTile((int)projectile.position.X / 16, (int)projectile.position.Y / 16, (ushort)mod.TileType("熔岩心石"), true, false, -1, 0);
                }
            }
        }
    }
}
