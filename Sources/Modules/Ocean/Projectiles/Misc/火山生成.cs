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
    public class 火山生成 : ModProjectile
    {
        // Token: 0x06001DC0 RID: 7616 RVA: 0x0000C2EA File Offset: 0x0000A4EA
        public override void SetStaticDefaults()
        {
            // // base.DisplayName.SetDefault("火山生成");
        }

        // Token: 0x06001DC1 RID: 7617 RVA: 0x0017F224 File Offset: 0x0017D424
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

        // Token: 0x06001DC2 RID: 7618 RVA: 0x0017F28C File Offset: 0x0017D48C
        public override void AI()
        {
            Projectile.velocity.Y += 20f;
        }
        // Token: 0x06001DC3 RID: 7619 RVA: 0x0017F570 File Offset: 0x0017D770
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
        }
        public override void Kill(int timeLeft)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            int num = 0;
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1].HasTile && Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].WallType != Mod.Find<ModWall>("熔岩石墙").Type)
            {
                int ix = (int)Projectile.position.X / 16 - 1, jx = (int)Projectile.position.Y / 16 - 1;
                if(jx < Main.maxTilesY * 0.55f)
                {
                    num = Main.rand.Next(0, 3440);
                }
                if (jx >= Main.maxTilesY * 0.55f && jx < Main.maxTilesY * 0.6f)
                {
                    num = Main.rand.Next(0, 4440);
                }
                if (jx > Main.maxTilesY * 0.6f)
                {
                    num = Main.rand.Next(0, 5040);
                }
                if (num < 3440 && num >= 0 && Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1].TileType == Mod.Find<ModTile>("玄武岩礁石").Type)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)Mod.Find<ModTile>("喷火口").Type, true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 8));
                    if (Main.tile[i, j].TileType == (ushort)Mod.Find<ModTile>("喷火口").Type)
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 80);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 80);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 80);
                    }
                }
                if (num < 4240 && num >= 3440 && Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1].TileType == Mod.Find<ModTile>("玄武岩礁石").Type)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 4, (ushort)Mod.Find<ModTile>("硫磺石").Type, true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 4, j = (int)Projectile.position.Y / 16 - 4;
                    short num8 = (short)(Main.rand.Next(0, 6));
                    if (Main.tile[i, j].TileType == (ushort)Mod.Find<ModTile>("硫磺石").Type)
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 96);
                        Main.tile[i, j + 5].TileFrameX = (short)(num8 * 96);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 96);
                        Main.tile[i, j + 4].TileFrameX = (short)(num8 * 96);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 96);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 96);
                    }
                }
                if (num < 5040 && num >= 4240 && Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1].TileType == Mod.Find<ModTile>("玄武岩礁石").Type)//橄榄石晶体
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 2, (ushort)Mod.Find<ModTile>("橄榄石晶体").Type, true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16 - 2;
                    short num8 = (short)(Main.rand.Next(0, 6));
                    if (Main.tile[i, j].TileType == (ushort)Mod.Find<ModTile>("橄榄石晶体").Type)
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 80);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 80);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 80);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 80);
                    }
                }
            }
        }
    }
}
