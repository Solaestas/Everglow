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
    public class 珊瑚random : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // base.DisplayName.SetDefault("珊瑚random");
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
            Projectile.velocity.Y += 30f;
        }
        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(70, 600);
            target.AddBuff(69, 600);
        }
        public override void Kill(int timeLeft)
        {
            OceanContentPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<OceanContentPlayer>();
            int num = Main.rand.Next(0, 3455);
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 150 && num >= 0)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.伞房叶状珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 2));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.伞房叶状珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 72);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 220 && num >= 150)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 2, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.巨大鹿角珊瑚>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 250 && num >= 220)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 4, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.巨大海鸡冠>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16 - 2;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.巨大海鸡冠>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 144);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 144);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 144);
                        Main.tile[i, j + 4].TileFrameX = (short)(num8 * 144);
                        Main.tile[i, j + 5].TileFrameX = (short)(num8 * 144);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 144);
                    }
                    /*
                    if (Main.tile[i + 1, j].type == (ushort)mod.TileType("巨大海鸡冠"))
                    {
                        Main.tile[i + 1, j].frameX = (short)(num8 * 144);
                        Main.tile[i + 1, j + 2].frameX = (short)(num8 * 144);
                        Main.tile[i + 1, j + 3].frameX = (short)(num8 * 144);
                        Main.tile[i + 1, j + 4].frameX = (short)(num8 * 144);
                        Main.tile[i + 1, j + 5].frameX = (short)(num8 * 144);
                        Main.tile[i + 1, j + 1].frameX = (short)(num8 * 144);
                    }*/
                    /*if (Main.tile[i - 1, j].type == (ushort)mod.TileType("巨大海鸡冠"))
                    {
                        Main.tile[i - 1, j].frameX = (short)(num8 * 144);
                        Main.tile[i - 1, j + 2].frameX = (short)(num8 * 144);
                        Main.tile[i - 1, j + 3].frameX = (short)(num8 * 144);
                        Main.tile[i - 1, j + 4].frameX = (short)(num8 * 144);
                        Main.tile[i - 1, j + 5].frameX = (short)(num8 * 144);
                        Main.tile[i - 1, j + 1].frameX = (short)(num8 * 144);
                    }*/
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 280 && num >= 250)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 5, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.巨大柳珊瑚>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 430 && num >= 280)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.石芝珊瑚>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num < 580 && num >= 430)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.脑珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16;
                    short num8 = (short)(Main.rand.Next(0, 3));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.脑珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 54);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 730 && num >= 580)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.榔头珊瑚>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 880 && num >= 730)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.蜂巢珊瑚>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 1030 && num >= 880)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.白枝海绵>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 1130 && num >= 1030)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.小脑珊瑚>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 1230 && num >= 1130)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.小蜂巢珊瑚>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 1330 && num >= 1230)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.细枝鹿角珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16;
                    short num8 = (short)(Main.rand.Next(0, 2));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.细枝鹿角珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 36);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num < 1430 && num >= 1330)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.石松鹿角珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16;
                    short num8 = (short)(Main.rand.Next(0, 2));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.石松鹿角珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 36);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 1530 && num >= 1430)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.黄色海绵>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 6));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.黄色海绵>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 36);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 36);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 36);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 1630 && num >= 1530)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.海鸡冠>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 2));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.海鸡冠>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 36);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 36);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 36);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 1700 && num >= 1630)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.蓝海树珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 2));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.蓝海树珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 36);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 36);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 36);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 1800 && num >= 1700)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.紫海柳>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.紫海柳>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 36);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 36);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 36);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 1900 && num >= 1800)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.大柳珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 2, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if(Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.大柳珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 72);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 1930 && num >= 1900)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.红珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 6));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.红珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 54);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 2000 && num >= 1930)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.蓝色鹿角珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.蓝色鹿角珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 54);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 2050 && num >= 2000)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 2, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.桶状海绵>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 2100 && num >= 2050)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 2, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.紫色海绵>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 2, j = (int)Projectile.position.Y / 16 - 2;
                    short num8 = (short)(Main.rand.Next(0, 6));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.紫色海绵>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 54);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 2200 && num >= 2100)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.大桌珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.大桌珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 108);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 108);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 108);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 + 1, (int)Projectile.position.Y / 16 + 1].HasTile)
            {
                if (num < 2300 && num >= 2200)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 + 1, (int)Projectile.position.Y / 16 + 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.松枝鹿角珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 + 1, j = (int)Projectile.position.Y / 16 + 1;
                    short num8 = (short)(Main.rand.Next(0, 2));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.松枝鹿角珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 36);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 + 1, (int)Projectile.position.Y / 16 + 1].HasTile)
            {
                if (num < 2400 && num >= 2300)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 + 1, (int)Projectile.position.Y / 16 + 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.气泡珊瑚>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num < 2500 && num >= 2400)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.甜甜圈珊瑚>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 2510 && num >= 2500)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 2, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.唐冠螺>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 2, j = (int)Projectile.position.Y / 16 - 2;
                    short num8 = (short)(Main.rand.Next(0, 3));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.唐冠螺>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 72);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num < 2520 && num >= 2510)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.大理石芋螺>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 2));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.大理石芋螺>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 54);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num < 2540 && num >= 2531)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.字码芋螺>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 2));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.字码芋螺>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 54);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num < 2550 && num >= 2540)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.鹦鹉螺>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16;
                    short num8 = (short)(Main.rand.Next(0, 6));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.鹦鹉螺>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 48);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 48);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num < 2530 && num >= 2520)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.黑星宝螺>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.黑星宝螺>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 48);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 48);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 48);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num == 2531)
                {
                    if (Main.rand.Next(50) == 0)
                    {
                        WorldGen.PlaceTile((int)Projectile.position.X / 16 - 1, (int)Projectile.position.Y / 16 - 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.黄金宝螺>(), true, false, -1, 0);
                        int i = (int)Projectile.position.X / 16 - 1, j = (int)Projectile.position.Y / 16 - 1;
                        short num8 = (short)(Main.rand.Next(0, 4));
                        if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.黄金宝螺>())
                        {
                            Main.tile[i, j].TileFrameX = (short)(num8 * 48);
                            Main.tile[i, j + 2].TileFrameX = (short)(num8 * 48);
                            Main.tile[i, j + 1].TileFrameX = (short)(num8 * 48);
                        }
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 2560 && num >= 2550)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 2, (int)Projectile.position.Y / 16 - 2, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.马蹄钟螺>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 2, j = (int)Projectile.position.Y / 16 - 2;
                    short num8 = (short)(Main.rand.Next(0, 2));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.马蹄钟螺>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 90);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 90);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 90);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 90);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num < 2650 && num >= 2600)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 4, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.黄色柳珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 4, j = (int)Projectile.position.Y / 16 - 4;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.黄色柳珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 96);
                        Main.tile[i, j + 5].TileFrameX = (short)(num8 * 96);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 96);
                        Main.tile[i, j + 4].TileFrameX = (short)(num8 * 96);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 96);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 96);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1].HasTile)
            {
                if (num < 2750 && num >= 2650)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.菊花海葵>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16 + 1;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.菊花海葵>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 18);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1].HasTile)
            {
                if (num < 2850 && num >= 2750)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.紫点海葵>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16 + 1;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.紫点海葵>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 18);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1].HasTile)
            {
                if (num < 2950 && num >= 2850)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.黄色小海绵>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16 + 1;
                    short num8 = (short)(Main.rand.Next(0, 8));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.黄色小海绵>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 18);
                        Main.tile[i, j - 1].TileFrameX = (short)(num8 * 18);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1].HasTile)
            {
                if (num < 2980 && num >= 2950)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.紫色小海绵>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16 + 1;
                    short num8 = (short)(Main.rand.Next(0, 8));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.紫色小海绵>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 18);
                        Main.tile[i, j - 1].TileFrameX = (short)(num8 * 18);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1].HasTile)
            {
                if (num < 3000 && num >= 2980)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 + 1, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.红色小海绵>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16 + 1;
                    short num8 = (short)(Main.rand.Next(0, 8));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.红色小海绵>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 18);
                        Main.tile[i, j - 1].TileFrameX = (short)(num8 * 18);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 3100 && num >= 3000)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 2, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.白色海鳃>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16 - 2;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.白色海鳃>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 54);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 54);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 3200 && num >= 3100)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 3, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.红色海鳃>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16 - 3;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.红色海鳃>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 4].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 72);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num < 3300 && num >= 3200)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.红色海葵>(), true, false, -1, 0);
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16].HasTile)
            {
                if (num < 3400 && num >= 3300)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.花鹿角珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16;
                    short num8 = (short)(Main.rand.Next(0, 6));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.花鹿角珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 36);
                        Main.tile[i, j - 1].TileFrameX = (short)(num8 * 36);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 - 2].HasTile)
            {
                if (num < 3420 && num >= 3400)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16 - 2, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.巨大橙色海绵>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16, j = (int)Projectile.position.Y / 16 - 2;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.巨大橙色海绵>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 162);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 162);
                        Main.tile[i, j - 1].TileFrameX = (short)(num8 * 162);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 162);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 162);
                        Main.tile[i, j - 2].TileFrameX = (short)(num8 * 162);
                        Main.tile[i, j - 3].TileFrameX = (short)(num8 * 162);
                        Main.tile[i, j - 4].TileFrameX = (short)(num8 * 162);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 3, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 3440 && num >= 3420 && mplayer.ZoneDeepocean)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 3, (int)Projectile.position.Y / 16 - 3, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.金柳珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 3, j = (int)Projectile.position.Y / 16 - 3;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.金柳珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 4].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 72);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 72);
                    }
                }
            }
            if (!Main.tile[(int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 1].HasTile)
            {
                if (num < 3455 && num >= 3440)
                {
                    WorldGen.PlaceTile((int)Projectile.position.X / 16 - 4, (int)Projectile.position.Y / 16 - 5, (ushort)ModContent.TileType<Everglow.Ocean.Tiles.大金柳珊瑚>(), true, false, -1, 0);
                    int i = (int)Projectile.position.X / 16 - 4, j = (int)Projectile.position.Y / 16 - 5;
                    short num8 = (short)(Main.rand.Next(0, 4));
                    if (Main.tile[i, j].TileType == (ushort)ModContent.TileType<Everglow.Ocean.Tiles.大金柳珊瑚>())
                    {
                        Main.tile[i, j].TileFrameX = (short)(num8 * 90);
                        Main.tile[i, j + 2].TileFrameX = (short)(num8 * 90);
                        Main.tile[i, j + 4].TileFrameX = (short)(num8 * 90);
                        Main.tile[i, j + 3].TileFrameX = (short)(num8 * 90);
                        Main.tile[i, j + 5].TileFrameX = (short)(num8 * 90);
                        Main.tile[i, j + 6].TileFrameX = (short)(num8 * 90);
                        Main.tile[i, j + 1].TileFrameX = (short)(num8 * 90);
                    }
                }
            }
        }
    }
}
