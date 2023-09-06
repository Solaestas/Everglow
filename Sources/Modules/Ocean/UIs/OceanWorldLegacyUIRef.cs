/*
using Terraria.World.Generation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using ReLogic.Graphics;
using MythMod.MiscImplementation;
//using System.Drawing;
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.IO;
using Terraria.ID;
using System;
using Terraria.DataStructures;
using Terraria.Localization;
using System.Collections.Generic;
using System.IO;
using Terraria.GameInput;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Shaders;
using Terraria.ModLoader.IO;
using Terraria.GameContent.Achievements;
using Terraria.GameContent.UI.Elements;
using Terraria.GameContent.Liquid;
using Terraria.Graphics;

namespace MythMod.UI.OceanContinent
{
    public class OceanContinent : UIState
    {
        private bool living = true;
        public static bool Open = false;
        private OceanWorldMain OceanWorldMain = new OceanWorldMain();
        public override void OnInitialize()
        {
            OceanWorldMain = new OceanWorldMain();
            OceanWorldMain.Width.Set(614, 0);
            OceanWorldMain.Height.Set(104, 0);
            OceanWorldMain.Left.Set(Main.screenWidth * 0.5f - 461, 0);
            OceanWorldMain.Top.Set(Main.screenHeight * 0.5f - 104, 0);

            Append(OceanWorldMain);
        }
        Vector2 offset;
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            Player player = Main.player[Main.myPlayer];
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            float shopx2 = innerDimensions.X;
            float shopy2 = innerDimensions.Y;
            float shopx = innerDimensions.X;
            float shopy = innerDimensions.Y;
        }
    }
    public class OceanWorldMain : UIElement
    {
        private float derta = 0;
        private float derta1 = 0;
        public static float jindu = 0;
        private int VolC = 5;
        private int VolH1 = 0;
        private int VolH2 = 0;
        private int VolH3 = 0;
        private int VolH4 = 0;
        private int VolH5 = 0;
        private int VolH6 = 0;
        private int VolH7 = 0;
        private int VolH8 = 0;
        private int VolH9 = 0;
        private int VolH10 = 0;
        private int VolH11 = 0;
        private int VolH12 = 0;
        private float seabedH = 0;
        private int shoreHouse = 0;
        public override void Draw(SpriteBatch spriteBatch)
        {
            Main.hardMode = true;
            Vector2 vector = new Vector2((float)Main.mouseX, (float)Main.mouseY);
            Player player = Main.player[Main.myPlayer];
            CalculatedStyle innerDimensions = base.GetInnerDimensions();
            float shopx = innerDimensions.X;
            float shopy = innerDimensions.Y;
            Mod mod = ModLoader.GetMod("MythMod");
            MythPlayer mplayer = Main.player[Main.myPlayer].GetModPlayer<MythPlayer>();
            if(Main.maxTilesX == 4200)
            {
                player.position = new Vector2(20, Main.maxTilesY / 10f + 60) * 16f;
            }
            if (Main.maxTilesX == 6400)
            {
                player.position = new Vector2(20, Main.maxTilesY / 10f + 120) * 16f;
            }
            if (Main.maxTilesX == 8400)
            {
                player.position = new Vector2(20, Main.maxTilesY / 10f + 300) * 16f;
            }
            spriteBatch.Draw(mod.GetTexture("UIImages/海洋世界生成进度条0"), new Vector2(0, 0), null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 0.6f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("UIImages/海洋世界生成框架底色"), new Vector2(shopx, shopy), null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("UIImages/海洋世界生成进度条2"), new Vector2(shopx + 56, shopy), new Rectangle(56,0,(int)(510 * derta1),104), Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("UIImages/海洋世界生成进度条1"), new Vector2(shopx + 48, shopy), new Rectangle(48, 0, (int)(540 * jindu / 1000f), 104), Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("UIImages/海洋世界生成进度条1Head"), new Vector2(shopx + 48 + (int)(540 * jindu / 1000f) * 1.5f, shopy), new Rectangle(562, 0, 24, 104), Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            spriteBatch.Draw(mod.GetTexture("UIImages/海洋世界生成框架"), new Vector2(shopx, shopy), null, Microsoft.Xna.Framework.Color.White, 0f, Vector2.Zero, 1.5f, SpriteEffects.None, 0f);
            base.Draw(spriteBatch);
            if(OceanContinent.Open)
            {
                Point origin = new Point(0, 0);
                player.noFallDmg = true;
                if(jindu == 0)
                {
                    for (int i = 0; i < 200; i++)
                    {
                        Main.npc[i].active = false;
                    }
                    Main.ActiveWorldFileData = new WorldFileData("Ocean/" + Main.worldName + "Ocean" + ".wld", false);
                    if(Main.maxTilesX == 6400)
                    {
                        VolC += 2;
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        VolC += 6;
                    }
                    seabedH = 0;
                    jindu += 1;
                    if(mplayer.wExpert)
                    {
                        Main.expertMode = true;
                        mplayer.wExpert = false;
                    }
                    if (mplayer.wMyth)
                    {
                        MythWorld.Myth = true;
                        mplayer.wMyth = false;
                    }
                }
                if(jindu >= 1)
                {
                    jindu += 1;
                }
                if (jindu >= 2 && jindu < 12)//生成矿物
                {
                    string text0 = "生成矿物…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    ShapeData shapeData101 = new ShapeData();
                    GenAction genAction = new Modifiers.Blotches(2, 0.4);
                    WorldUtils.Gen(origin, new Shapes.Rectangle(1, 1), Actions.Chain(new GenAction[]
                    {genAction.Output(shapeData101)}));
                    if (Main.maxTilesX == 4200)
                    {
                        for (int l = (int)((jindu - 2) / 10f * Main.maxTilesX); l < (jindu - 1) / 10f * Main.maxTilesX; l++)
                        {
                            for (int m = Main.maxTilesY / 2; m < Main.maxTilesY - 30; m++)
                            {
                                if (Main.rand.Next(4000) == 1)
                                {
                                    int ox = 0;
                                    int oy = 0;
                                    for (int num9 = Main.rand.Next(5, m / 50 + 6); num9 > 0; num9--)
                                    {
                                        if (Main.rand.Next(0, 10000) > 5000)
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                }));
                                                ox += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                 }));
                                                ox -= 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                 }));
                                                oy += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                 }));
                                                oy -= 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 6400)
                    {
                        for (int l = (int)((jindu - 2) / 10f * Main.maxTilesX); l < (jindu - 1) / 10f * Main.maxTilesX; l++)
                        {
                            for (int m = (int)(Main.maxTilesY * 0.56f); m < Main.maxTilesY - 30; m++)
                            {
                                if (Main.rand.Next(4000) == 1)
                                {
                                    int ox = 0;
                                    int oy = 0;
                                    for (int num9 = Main.rand.Next(5, m / 50 + 6); num9 > 0; num9--)
                                    {
                                        if (Main.rand.Next(0, 10000) > 5000)
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                }));
                                                ox += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                 }));
                                                ox -= 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                 }));
                                                oy += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                 }));
                                                oy -= 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        for (int l = (int)((jindu - 2) / 10f * Main.maxTilesX); l < (jindu - 1) / 10f * Main.maxTilesX; l++)
                        {
                            for (int m = (int)(Main.maxTilesY * 0.71f); m < Main.maxTilesY - 30; m++)
                            {
                                if (Main.rand.Next(4000) == 1)
                                {
                                    int ox = 0;
                                    int oy = 0;
                                    for (int num9 = Main.rand.Next(5, m / 50 + 6); num9 > 0; num9--)
                                    {
                                        if (Main.rand.Next(0, 10000) > 5000)
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                }));
                                                ox += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                 }));
                                                ox -= 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                 }));
                                                oy += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData101), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("沧流矿"), 0)
                                                 }));
                                                oy -= 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (jindu >= 12 && jindu < 22)//生成矿物
                {
                    string text0 = "生成矿物…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    ShapeData shapeData102 = new ShapeData();
                    GenAction genAction = new Modifiers.Blotches(2, 0.4);
                    WorldUtils.Gen(origin, new Shapes.Rectangle(1, 1), Actions.Chain(new GenAction[]
                    {genAction.Output(shapeData102)}));
                    if (Main.maxTilesX == 4200)
                    {
                        for (int l = (int)((jindu - 12) / 10f * Main.maxTilesX); l < (jindu - 11) / 10f * Main.maxTilesX; l++)
                        {
                            for (int m = (int)(Main.maxTilesY * 0.7f); m < Main.maxTilesY - 30; m++)
                            {
                                if (Main.rand.Next(9000) == 1)
                                {
                                    int ox = 0;
                                    int oy = 0;
                                    for (int num9 = Main.rand.Next(5, m / 50 + 6); num9 > 0; num9--)
                                    {
                                        if (Main.rand.Next(0, 10000) > 5000)
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                }));
                                                ox += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                 }));
                                                ox -= 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                 }));
                                                oy += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                 }));
                                                oy -= 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 6400)
                    {
                        for (int l = (int)((jindu - 12) / 10f * Main.maxTilesX); l < (jindu - 11) / 10f * Main.maxTilesX; l++)
                        {
                            for (int m = (int)(Main.maxTilesY * 0.75f); m < Main.maxTilesY - 30; m++)
                            {
                                if (Main.rand.Next(9000) == 1)
                                {
                                    int ox = 0;
                                    int oy = 0;
                                    for (int num9 = Main.rand.Next(5, m / 50 + 6); num9 > 0; num9--)
                                    {
                                        if (Main.rand.Next(0, 10000) > 5000)
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                }));
                                                ox += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                 }));
                                                ox -= 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                 }));
                                                oy += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                 }));
                                                oy -= 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        for (int l = (int)((jindu - 12) / 10f * Main.maxTilesX); l < (jindu - 11) / 10f * Main.maxTilesX; l++)
                        {
                            for (int m = (int)(Main.maxTilesY * 0.8f); m < Main.maxTilesY - 30; m++)
                            {
                                if (Main.rand.Next(9000) == 1)
                                {
                                    int ox = 0;
                                    int oy = 0;
                                    for (int num9 = Main.rand.Next(5, m / 50 + 6); num9 > 0; num9--)
                                    {
                                        if (Main.rand.Next(0, 10000) > 5000)
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                }));
                                                ox += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                 }));
                                                ox -= 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Main.rand.Next(0, 10000) > 5000)
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                 }));
                                                oy += 1;
                                            }
                                            else
                                            {
                                                WorldUtils.Gen(new Point(l + ox, m + oy), new ModShapes.All(shapeData102), Actions.Chain(new GenAction[]
                                                 {
                                                     new Actions.PlaceTile((ushort)mod.TileType("渊海矿"), 0)
                                                 }));
                                                oy -= 1;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (jindu == 100)//火山基带
                {
                    string text0 = "构建火山框架…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        int H = 0;//顶部高度
                        int L = 0;//下表面深度
                        for (int l = (int)(Main.maxTilesX * 0.55f); l < (int)(Main.maxTilesX * 0.95f); l++)
                        {
                            if (l < (int)(Main.maxTilesX * 0.9f))
                            {
                                if (H >= 50 && H <= 200)
                                {
                                    H += (int)((H * H) / 10000f + Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H < 10)
                                {
                                    H += (int)(Main.rand.Next(0, 45) / 30f);
                                }
                                if (H >= 10 && H < 50)
                                {
                                    H += (int)(Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H > 200)
                                {
                                    H += (int)(Main.rand.Next(-120, 120) / 30f);
                                }
                            }
                            if (l >= (int)(Main.maxTilesX * 0.8f))
                            {
                                if (H >= 50 && H <= 200)
                                {
                                    H -= (int)((H * H) / 10000f + Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H < 10)
                                {
                                    H -= (int)(Main.rand.Next(0, 45) / 30f);
                                }
                                if (H >= 10 && H < 50)
                                {
                                    H -= (int)(Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H > 200)
                                {
                                    H -= (int)((H * H) / 6000f + Main.rand.Next(-60, 135) / 30f);
                                }
                            }
                            L = H * 2;
                            for (int m = 0; m < Main.maxTilesY; m++)
                            {
                                if (m < Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) + L && m > Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) - H && Main.tile[l, m].type != mod.TileType("沧流矿")
                                            && Main.tile[l, m].type != mod.TileType("渊海矿")
                                            && Main.tile[l, m].type != mod.TileType("Basalt")
                                            && Main.tile[l, m].type != mod.TileType("橄榄石")
                                            && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                            && Main.tile[l, m].type != mod.TileType("熔岩石")
                                            && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                {
                                    if (Main.rand.Next(4000) == 1)
                                    {
                                        int i = 0;
                                        int j = 0;
                                        for (int n = Main.rand.Next(0, 50); n < 60; n++)
                                        {
                                            Main.tile[l + i, m + j].type = (ushort)mod.TileType("硫磺矿");
                                            Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                            Main.tile[l + i, m + j].active(true);
                                            if (Main.rand.Next(10) > 5)
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i += 1;
                                                }
                                                else
                                                {
                                                    j += 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i -= 1;
                                                }
                                                else
                                                {
                                                    j -= 1;
                                                }
                                            }
                                        }
                                    }
                                    else if (Main.rand.Next(7000) != 1)
                                    {
                                        Main.tile[l, m].type = (ushort)mod.TileType("Basalt");
                                        Main.tile[l, m + 2].wall = (ushort)mod.WallType("玄武岩墙");
                                        Main.tile[l, m].active(true);
                                    }
                                    else
                                    {
                                        int i = 0;
                                        int j = 0;
                                        for (int n = Main.rand.Next(0, 75); n < 90; n++)
                                        {
                                            if (Math.Abs(n - 45) < 8)
                                            {
                                                Main.tile[l + i, m + j].type = (ushort)mod.TileType("橄榄石");
                                                Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                                Main.tile[l + i, m + j].active(true);
                                            }
                                            else
                                            {
                                                Main.tile[l + i, m + j].type = (ushort)mod.TileType("橄榄石矿");
                                                Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                                Main.tile[l + i, m + j].active(true);
                                            }
                                            if (Main.rand.Next(10) > 5)
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i += 1;
                                                }
                                                else
                                                {
                                                    j += 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i -= 1;
                                                }
                                                else
                                                {
                                                    j -= 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else if (Main.maxTilesX == 6400)
                    {
                        int H = 0;//顶部高度
                        int L = 0;//下表面深度
                        for (int l = (int)(Main.maxTilesX * 0.55f); l < (int)(Main.maxTilesX * 0.95f); l++)
                        {
                            if (l < (int)(Main.maxTilesX * 0.9f))
                            {
                                if (H >= 50 && H <= 200)
                                {
                                    H += (int)((H * H) / 10000f + Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H < 10)
                                {
                                    H += (int)(Main.rand.Next(0, 45) / 30f);
                                }
                                if (H >= 10 && H < 50)
                                {
                                    H += (int)(Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H > 200)
                                {
                                    H += (int)(Main.rand.Next(-120, 120) / 30f);
                                }
                            }
                            if (l >= (int)(Main.maxTilesX * 0.8f))
                            {
                                if (H >= 50 && H <= 200)
                                {
                                    H -= (int)((H * H) / 10000f + Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H < 10)
                                {
                                    H -= (int)(Main.rand.Next(0, 45) / 30f);
                                }
                                if (H >= 10 && H < 50)
                                {
                                    H -= (int)(Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H > 200)
                                {
                                    H -= (int)((H * H) / 6000f + Main.rand.Next(-60, 135) / 30f);
                                }
                            }
                            L = H * 2;
                            for (int m = 0; m < Main.maxTilesY; m++)
                            {
                                if (m < Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) + L && m > Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) - H && Main.tile[l, m].type != mod.TileType("沧流矿")
                                            && Main.tile[l, m].type != mod.TileType("渊海矿")
                                            && Main.tile[l, m].type != mod.TileType("Basalt")
                                            && Main.tile[l, m].type != mod.TileType("橄榄石")
                                            && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                            && Main.tile[l, m].type != mod.TileType("熔岩石")
                                            && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                {
                                    if (Main.rand.Next(4000) == 1)
                                    {
                                        int i = 0;
                                        int j = 0;
                                        for (int n = Main.rand.Next(0, 50); n < 60; n++)
                                        {
                                            Main.tile[l + i, m + j].type = (ushort)mod.TileType("硫磺矿");
                                            Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                            Main.tile[l + i, m + j].active(true);
                                            if (Main.rand.Next(10) > 5)
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i += 1;
                                                }
                                                else
                                                {
                                                    j += 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i -= 1;
                                                }
                                                else
                                                {
                                                    j -= 1;
                                                }
                                            }
                                        }
                                    }
                                    else if (Main.rand.Next(7000) != 1)
                                    {
                                        Main.tile[l, m].type = (ushort)mod.TileType("Basalt");
                                        Main.tile[l, m + 2].wall = (ushort)mod.WallType("玄武岩墙");
                                        Main.tile[l, m].active(true);
                                    }
                                    else
                                    {
                                        int i = 0;
                                        int j = 0;
                                        for (int n = Main.rand.Next(0, 75); n < 90; n++)
                                        {
                                            if (Math.Abs(n - 45) < 8)
                                            {
                                                Main.tile[l + i, m + j].type = (ushort)mod.TileType("橄榄石");
                                                Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                                Main.tile[l + i, m + j].active(true);
                                            }
                                            else
                                            {
                                                Main.tile[l + i, m + j].type = (ushort)mod.TileType("橄榄石矿");
                                                Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                                Main.tile[l + i, m + j].active(true);
                                            }
                                            if (Main.rand.Next(10) > 5)
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i += 1;
                                                }
                                                else
                                                {
                                                    j += 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i -= 1;
                                                }
                                                else
                                                {
                                                    j -= 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        int H = 0;//顶部高度
                        int L = 0;//下表面深度
                        for (int l = (int)(Main.maxTilesX * 0.55f); l < (int)(Main.maxTilesX * 0.95f); l++)
                        {
                            if (l < (int)(Main.maxTilesX * 0.9f))
                            {
                                if (H >= 50 && H <= 200)
                                {
                                    H += (int)((H * H) / 10000f + Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H < 10)
                                {
                                    H += (int)(Main.rand.Next(0, 45) / 30f);
                                }
                                if (H >= 10 && H < 50)
                                {
                                    H += (int)(Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H > 200)
                                {
                                    H += (int)(Main.rand.Next(-120, 120) / 30f);
                                }
                            }
                            if (l >= (int)(Main.maxTilesX * 0.8f))
                            {
                                if (H >= 50 && H <= 200)
                                {
                                    H -= (int)((H * H) / 10000f + Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H < 10)
                                {
                                    H -= (int)(Main.rand.Next(0, 45) / 30f);
                                }
                                if (H >= 10 && H < 50)
                                {
                                    H -= (int)(Main.rand.Next(-60, 135) / 30f);
                                }
                                if (H > 200)
                                {
                                    H -= (int)((H * H) / 6000f + Main.rand.Next(-60, 135) / 30f);
                                }
                            }
                            L = H * 2;
                            for (int m = 0; m < Main.maxTilesY; m++)
                            {
                                if (m < Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) + L && m > Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) - H && Main.tile[l, m].type != mod.TileType("沧流矿")
                                            && Main.tile[l, m].type != mod.TileType("渊海矿")
                                            && Main.tile[l, m].type != mod.TileType("Basalt")
                                            && Main.tile[l, m].type != mod.TileType("橄榄石")
                                            && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                            && Main.tile[l, m].type != mod.TileType("熔岩石")
                                            && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                {
                                    if (Main.rand.Next(4000) == 1)
                                    {
                                        int i = 0;
                                        int j = 0;
                                        for (int n = Main.rand.Next(0, 50); n < 60; n++)
                                        {
                                            Main.tile[l + i, m + j].type = (ushort)mod.TileType("硫磺矿");
                                            Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                            Main.tile[l + i, m + j].active(true);
                                            if (Main.rand.Next(10) > 5)
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i += 1;
                                                }
                                                else
                                                {
                                                    j += 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i -= 1;
                                                }
                                                else
                                                {
                                                    j -= 1;
                                                }
                                            }
                                        }
                                    }
                                    else if (Main.rand.Next(7000) != 1)
                                    {
                                        Main.tile[l, m].type = (ushort)mod.TileType("Basalt");
                                        Main.tile[l, m + 2].wall = (ushort)mod.WallType("玄武岩墙");
                                        Main.tile[l, m].active(true);
                                    }
                                    else
                                    {
                                        int i = 0;
                                        int j = 0;
                                        for (int n = Main.rand.Next(0, 75); n < 90; n++)
                                        {
                                            if (Math.Abs(n - 45) < 8)
                                            {
                                                Main.tile[l + i, m + j].type = (ushort)mod.TileType("橄榄石");
                                                Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                                Main.tile[l + i, m + j].active(true);
                                            }
                                            else
                                            {
                                                Main.tile[l + i, m + j].type = (ushort)mod.TileType("橄榄石矿");
                                                Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                                Main.tile[l + i, m + j].active(true);
                                            }
                                            if (Main.rand.Next(10) > 5)
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i += 1;
                                                }
                                                else
                                                {
                                                    j += 1;
                                                }
                                            }
                                            else
                                            {
                                                if (Main.rand.Next(10) > 5)
                                                {
                                                    i -= 1;
                                                }
                                                else
                                                {
                                                    j -= 1;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (jindu == 101)//主火山
                {
                    string text0 = "建造大火山…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    int H = 0;
                    int Xc = 0;
                    for (int l = (int)(Main.maxTilesX * 0.65f); l < (int)(Main.maxTilesX * 0.75f); l++)
                    {
                        if(H < Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) + 150)
                        {
                            if (Main.maxTilesX == 4200)
                            {
                                H += (int)((H * H) / 100000f + Main.rand.Next(-60, 130) / 30f);
                            }
                            if (Main.maxTilesX == 6400)
                            {
                                H += (int)((H * H) / 100000f + Main.rand.Next(-60, 110) / 30f);
                            }
                            if (Main.maxTilesX == 8400)
                            {
                                H += (int)((H * H) / 100000f + Main.rand.Next(-60, 100) / 30f);
                            }
                        }
                        else
                        {
                            H += (int)(Main.rand.Next(-60, 60) / 30f);
                            if(Xc == 0)
                            {
                                Xc = (int)(Main.maxTilesX * 0.75f) - l;
                            }
                        }
                        int L = H / 3;
                        for (int m = 0; m < Main.maxTilesY * 0.7f; m++)
                        {
                            if (m < Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) + L && m > Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) - H / 1.5f && Main.tile[l, m].type != mod.TileType("沧流矿")
                                        && Main.tile[l, m].type != mod.TileType("渊海矿")
                                        && Main.tile[l, m].type != mod.TileType("Basalt")
                                        && Main.tile[l, m].type != mod.TileType("橄榄石")
                                        && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                        && Main.tile[l, m].type != mod.TileType("熔岩石")
                                        && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                            {
                                if (Main.rand.Next(4000) == 1)
                                {
                                    int i = 0;
                                    int j = 0;
                                    for (int n = Main.rand.Next(0, 50); n < 60; n++)
                                    {
                                        Main.tile[l + i, m + j].type = (ushort)mod.TileType("硫磺矿");
                                        Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                        Main.tile[l + i, m + j].active(true);
                                        if (Main.rand.Next(10) > 5)
                                        {
                                            if (Main.rand.Next(10) > 5)
                                            {
                                                i += 1;
                                            }
                                            else
                                            {
                                                j += 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Main.rand.Next(10) > 5)
                                            {
                                                i -= 1;
                                            }
                                            else
                                            {
                                                j -= 1;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Main.tile[l, m].type = (ushort)mod.TileType("Basalt");
                                    Main.tile[l, m + 2].wall = (ushort)mod.WallType("玄武岩墙");
                                    Main.tile[l, m].active(true);
                                }
                            }
                        }
                    }
                    for (int l = (int)(Main.maxTilesX * 0.75f); l < (int)(Main.maxTilesX * 0.85f); l++)
                    {
                        if (l > Main.maxTilesX * 0.75f + Xc)
                        {
                            H -= (int)((H * H) / 100000f + Main.rand.Next(-60, 130) / 30f);
                        }
                        else
                        {
                            H += (int)(Main.rand.Next(-60, 60) / 30f);
                        }
                        int L = H / 3;
                        for (int m = 0; m < Main.maxTilesY * 0.7f; m++)
                        {
                            if (m < Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) + L && m > Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) - H / 1.5f && Main.tile[l, m].type != mod.TileType("沧流矿")
                                        && Main.tile[l, m].type != mod.TileType("渊海矿")
                                        && Main.tile[l, m].type != mod.TileType("Basalt")
                                        && Main.tile[l, m].type != mod.TileType("橄榄石")
                                        && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                        && Main.tile[l, m].type != mod.TileType("熔岩石")
                                        && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                            {
                                if (Main.rand.Next(4000) == 1)
                                {
                                    int i = 0;
                                    int j = 0;
                                    for (int n = Main.rand.Next(0, 50); n < 60; n++)
                                    {
                                        Main.tile[l + i, m + j].type = (ushort)mod.TileType("硫磺矿");
                                        Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                        Main.tile[l + i, m + j].active(true);
                                        if (Main.rand.Next(10) > 5)
                                        {
                                            if (Main.rand.Next(10) > 5)
                                            {
                                                i += 1;
                                            }
                                            else
                                            {
                                                j += 1;
                                            }
                                        }
                                        else
                                        {
                                            if (Main.rand.Next(10) > 5)
                                            {
                                                i -= 1;
                                            }
                                            else
                                            {
                                                j -= 1;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Main.tile[l, m].type = (ushort)mod.TileType("Basalt");
                                    Main.tile[l, m + 2].wall = (ushort)mod.WallType("玄武岩墙");
                                    Main.tile[l, m].active(true);
                                }
                            }
                        }
                    }
                }
                if (jindu == 103)//别的火山口
                {
                    string text0 = "建造别的火山…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX != 8400)
                    {
                        for (int count = 0; count < VolC; count++)
                        {
                            int VCenter = (int)Main.rand.NextFloat(Main.maxTilesX * 0.6f + count / (float)VolC * Main.maxTilesX * 0.3f + 15, Main.maxTilesX * 0.6f + (count + 1) / (float)VolC * Main.maxTilesX * 0.3f - 15);
                            if (Math.Abs(VCenter - Main.maxTilesX * 0.75f) < 30)
                            {
                                count += 1;
                            }
                            else
                            {
                                if (count == 0)
                                {
                                    VolH1 = VCenter;
                                }
                                if (count == 1)
                                {
                                    VolH2 = VCenter;
                                }
                                if (count == 2)
                                {
                                    VolH3 = VCenter;
                                }
                                if (count == 3)
                                {
                                    VolH4 = VCenter;
                                }
                                if (count == 4)
                                {
                                    VolH5 = VCenter;
                                }
                                if (count == 5)
                                {
                                    VolH6 = VCenter;
                                }
                                if (count == 6)
                                {
                                    VolH7 = VCenter;
                                }
                                if (count == 7)
                                {
                                    VolH8 = VCenter;
                                }
                                if (count == 8)
                                {
                                    VolH9 = VCenter;
                                }
                                if (count == 9)
                                {
                                    VolH10 = VCenter;
                                }
                                if (count == 10)
                                {
                                    VolH11 = VCenter;
                                }
                                if (count == 11)
                                {
                                    VolH12 = VCenter;
                                }
                                int H = 0;
                                int Xc = 0;
                                for (int l = VCenter - (int)(Main.maxTilesX * 0.09f); l < VCenter; l++)
                                {
                                    if (H < Main.maxTilesY / 2 - 470 + (int)(Main.maxTilesY * 0.3f))
                                    {
                                        if (Main.maxTilesX == 4200)
                                        {
                                            H += (int)((H * H) / 120000f + Main.rand.Next(-60, 130) / 30f);
                                        }
                                        if (Main.maxTilesX == 6400)
                                        {
                                            H += (int)((H * H) / 100000f + Main.rand.Next(-60, 112) / 30f);
                                        }
                                        if (Main.maxTilesX == 8400)
                                        {
                                            H += (int)((H * H) / 100000f + Main.rand.Next(-60, 90) / 30f);
                                        }
                                    }
                                    else
                                    {
                                        H += (int)(Main.rand.Next(-60, 60) / 30f);
                                        if (Xc == 0)
                                        {
                                            Xc = VCenter - l;
                                        }
                                    }
                                    int L = (int)(H / 2.4f);
                                    for (int m = 0; m < Main.maxTilesY * 0.7f; m++)
                                    {
                                        if (m < Main.maxTilesY / 2 - 470 + (int)(Main.maxTilesY * 0.3f) + L && m > Main.maxTilesY / 2 - 470 + (int)(Main.maxTilesY * 0.3f) - H / 1.5f && Main.tile[l, m].type != mod.TileType("沧流矿")
                                            && Main.tile[l, m].type != mod.TileType("渊海矿")
                                            && Main.tile[l, m].type != mod.TileType("Basalt")
                                            && Main.tile[l, m].type != mod.TileType("橄榄石")
                                            && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                            && Main.tile[l, m].type != mod.TileType("熔岩石")
                                            && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                        {
                                            if (Main.rand.Next(4000) == 1)
                                            {
                                                int i = 0;
                                                int j = 0;
                                                for (int n = Main.rand.Next(0, 50); n < 60; n++)
                                                {
                                                    Main.tile[l + i, m + j].type = (ushort)mod.TileType("硫磺矿");
                                                    Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                                    Main.tile[l + i, m + j].active(true);
                                                    if (Main.rand.Next(10) > 5)
                                                    {
                                                        if (Main.rand.Next(10) > 5)
                                                        {
                                                            i += 1;
                                                        }
                                                        else
                                                        {
                                                            j += 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Main.rand.Next(10) > 5)
                                                        {
                                                            i -= 1;
                                                        }
                                                        else
                                                        {
                                                            j -= 1;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Main.tile[l, m].type = (ushort)mod.TileType("Basalt");
                                                Main.tile[l, m + 2].wall = (ushort)mod.WallType("玄武岩墙");
                                                Main.tile[l, m].active(true);
                                            }
                                        }
                                    }
                                }
                                for (int l = VCenter; l < VCenter + (int)(Main.maxTilesX * 0.09f); l++)
                                {
                                    if (l > VCenter + Xc)
                                    {
                                        H -= (int)((H * H) / 120000f + Main.rand.Next(-60, 130) / 30f);
                                    }
                                    else
                                    {
                                        H += (int)(Main.rand.Next(-60, 60) / 30f);
                                    }
                                    int L = H / 3;
                                    for (int m = 0; m < Main.maxTilesY * 0.7f; m++)
                                    {
                                        if (m < Main.maxTilesY / 2 - 470 + (int)(Main.maxTilesY * 0.3f) + L && m > Main.maxTilesY / 2 - 470 + (int)(Main.maxTilesY * 0.3f) - H / 1.5f
                                            && Main.tile[l, m].type != mod.TileType("沧流矿")
                                            && Main.tile[l, m].type != mod.TileType("渊海矿")
                                            && Main.tile[l, m].type != mod.TileType("Basalt")
                                            && Main.tile[l, m].type != mod.TileType("橄榄石")
                                            && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                            && Main.tile[l, m].type != mod.TileType("熔岩石")
                                            && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                        {
                                            Main.tile[l, m].type = (ushort)mod.TileType("Basalt");
                                            Main.tile[l, m + 2].wall = (ushort)mod.WallType("玄武岩墙");
                                            Main.tile[l, m].active(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int count = 0; count < VolC; count++)
                        {
                            int VCenter = (int)Main.rand.NextFloat(Main.maxTilesX * 0.6f + count / (float)VolC * Main.maxTilesX * 0.3f + 15, Main.maxTilesX * 0.6f + (count + 1) / (float)VolC * Main.maxTilesX * 0.3f - 15);
                            if (Math.Abs(VCenter - Main.maxTilesX * 0.75f) < 30)
                            {
                                count += 1;
                            }
                            else
                            {
                                if (count == 0)
                                {
                                    VolH1 = VCenter;
                                }
                                if (count == 1)
                                {
                                    VolH2 = VCenter;
                                }
                                if (count == 2)
                                {
                                    VolH3 = VCenter;
                                }
                                if (count == 3)
                                {
                                    VolH4 = VCenter;
                                }
                                if (count == 4)
                                {
                                    VolH5 = VCenter;
                                }
                                if (count == 5)
                                {
                                    VolH6 = VCenter;
                                }
                                if (count == 6)
                                {
                                    VolH7 = VCenter;
                                }
                                if (count == 7)
                                {
                                    VolH8 = VCenter;
                                }
                                if (count == 8)
                                {
                                    VolH9 = VCenter;
                                }
                                if (count == 9)
                                {
                                    VolH10 = VCenter;
                                }
                                if (count == 10)
                                {
                                    VolH11 = VCenter;
                                }
                                if (count == 11)
                                {
                                    VolH12 = VCenter;
                                }
                                int H = 0;
                                int Xc = 0;
                                for (int l = VCenter - (int)(Main.maxTilesX * 0.09f); l < VCenter; l++)
                                {
                                    if (H < Main.maxTilesY / 2 - 470 + (int)(Main.maxTilesY * 0.3f))
                                    {
                                        if (Main.maxTilesX == 4200)
                                        {
                                            H += (int)((H * H) / 120000f + Main.rand.Next(-60, 130) / 30f);
                                        }
                                        if (Main.maxTilesX == 6400)
                                        {
                                            H += (int)((H * H) / 100000f + Main.rand.Next(-60, 105) / 30f);
                                        }
                                        if (Main.maxTilesX == 8400)
                                        {
                                            H += (int)((H * H) / 100000f + Main.rand.Next(-60, 100) / 30f);
                                        }
                                    }
                                    else
                                    {
                                        H += (int)(Main.rand.Next(-60, 60) / 30f);
                                        if (Xc == 0)
                                        {
                                            Xc = VCenter - l;
                                        }
                                    }
                                    int L = (int)(H / 2.4f);
                                    for (int m = 0; m < Main.maxTilesY * 0.7f; m++)
                                    {
                                        if (m < Main.maxTilesY / 2 - 470 + (int)(Main.maxTilesY * 0.3f) + L && m > Main.maxTilesY / 2 - 470 + (int)(Main.maxTilesY * 0.3f) - H / 1.5f && Main.tile[l, m].type != mod.TileType("沧流矿")
                                            && Main.tile[l, m].type != mod.TileType("渊海矿")
                                            && Main.tile[l, m].type != mod.TileType("Basalt")
                                            && Main.tile[l, m].type != mod.TileType("橄榄石")
                                            && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                            && Main.tile[l, m].type != mod.TileType("熔岩石")
                                            && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                        {
                                            if (Main.rand.Next(4000) == 1)
                                            {
                                                int i = 0;
                                                int j = 0;
                                                for (int n = Main.rand.Next(0, 50); n < 60; n++)
                                                {
                                                    Main.tile[l + i, m + j].type = (ushort)mod.TileType("硫磺矿");
                                                    Main.tile[l + i, m + 2 + j].wall = (ushort)mod.WallType("玄武岩墙");
                                                    Main.tile[l + i, m + j].active(true);
                                                    if (Main.rand.Next(10) > 5)
                                                    {
                                                        if (Main.rand.Next(10) > 5)
                                                        {
                                                            i += 1;
                                                        }
                                                        else
                                                        {
                                                            j += 1;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Main.rand.Next(10) > 5)
                                                        {
                                                            i -= 1;
                                                        }
                                                        else
                                                        {
                                                            j -= 1;
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Main.tile[l, m].type = (ushort)mod.TileType("Basalt");
                                                Main.tile[l, m + 2].wall = (ushort)mod.WallType("玄武岩墙");
                                                Main.tile[l, m].active(true);
                                            }
                                        }
                                    }
                                }
                                for (int l = VCenter; l < VCenter + (int)(Main.maxTilesX * 0.09f); l++)
                                {
                                    if (l > VCenter + Xc)
                                    {
                                        H -= (int)((H * H) / 120000f + Main.rand.Next(-60, 130) / 30f);
                                    }
                                    else
                                    {
                                        H += (int)(Main.rand.Next(-60, 60) / 30f);
                                    }
                                    int L = H / 3;
                                    for (int m = 0; m < Main.maxTilesY * 0.7f; m++)
                                    {
                                        if (m < Main.maxTilesY / 2 - 470 + (int)(Main.maxTilesY * 0.3f) + L && m > Main.maxTilesY / 2 - 470 + (int)(Main.maxTilesY * 0.3f) - H / 1.5f
                                            && Main.tile[l, m].type != mod.TileType("沧流矿")
                                            && Main.tile[l, m].type != mod.TileType("渊海矿")
                                            && Main.tile[l, m].type != mod.TileType("Basalt")
                                            && Main.tile[l, m].type != mod.TileType("橄榄石")
                                            && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                            && Main.tile[l, m].type != mod.TileType("熔岩石")
                                            && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                        {
                                            Main.tile[l, m].type = (ushort)mod.TileType("Basalt");
                                            Main.tile[l, m + 2].wall = (ushort)mod.WallType("玄武岩墙");
                                            Main.tile[l, m].active(true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (jindu == 105)//火山大陆架左侧
                {
                    string text0 = "生成火山大陆…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        float ym = 0;
                        for (int l = (int)(Main.maxTilesX * 0.50f); l < (int)(Main.maxTilesX * 0.65f); l++)
                        {
                            ym = (l - Main.maxTilesX * 0.5f) / (float)(Main.maxTilesX * 0.15f);
                            for (int m = Main.maxTilesY / 2 - 400; m < Main.maxTilesY * 0.6f; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 395) + (Math.Cos(ym * Math.PI) + 1) * Main.maxTilesY * 0.15f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 1].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m <= (Main.maxTilesY / 2 - 395) + (Math.Cos(ym * Math.PI) + 1) * Main.maxTilesY * 0.15f && m > (Main.maxTilesY / 2 - 400) + (Math.Cos(ym * Math.PI) + 1) * Main.maxTilesY * 0.15f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                    }
                    else if (Main.maxTilesX == 6400)
                    {
                        float ym = 0;
                        for (int l = (int)(Main.maxTilesX * 0.50f); l < (int)(Main.maxTilesX * 0.65f); l++)
                        {
                            ym = (l - Main.maxTilesX * 0.5f) / (float)(Main.maxTilesX * 0.15f);
                            for (int m = Main.maxTilesY / 2 - 400; m < Main.maxTilesY * 0.64f; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 395) + (Math.Cos(ym * Math.PI) + 1) * Main.maxTilesY * 0.15f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 1].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m <= (Main.maxTilesY / 2 - 395) + (Math.Cos(ym * Math.PI) + 1) * Main.maxTilesY * 0.15f && m > (Main.maxTilesY / 2 - 400) + (Math.Cos(ym * Math.PI) + 1) * Main.maxTilesY * 0.15f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        float ym = 0;
                        for (int l = (int)(Main.maxTilesX * 0.50f); l < (int)(Main.maxTilesX * 0.65f); l++)
                        {
                            ym = (l - Main.maxTilesX * 0.5f) / (float)(Main.maxTilesX * 0.15f);
                            for (int m = Main.maxTilesY / 2 - 470; m < Main.maxTilesY * 0.6f; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 465) + (Math.Cos(ym * Math.PI) + 1) * Main.maxTilesY * 0.22f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 1].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m <= (Main.maxTilesY / 2 - 465) + (Math.Cos(ym * Math.PI) + 1) * Main.maxTilesY * 0.15f && m > (Main.maxTilesY / 2 - 470) + (Math.Cos(ym * Math.PI) + 1) * Main.maxTilesY * 0.22f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                    }

                    jindu += 1;
                }
                if (jindu == 106)//火山右侧大陆架
                {
                    string text0 = "生成火山大陆…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        float ym = 0;
                        for (int l = (int)(Main.maxTilesX * 0.85f); l < (int)(Main.maxTilesX); l++)
                        {
                            ym = (l - Main.maxTilesX * 0.85f) / (float)(Main.maxTilesX * 0.15f);
                            for (int m = Main.maxTilesY / 2 - 400; m < Main.maxTilesY * 0.6f; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 395) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 1].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m <= (Main.maxTilesY / 2 - 395) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f && m > (Main.maxTilesY / 2 - 400) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                    }
                    else if (Main.maxTilesX == 6400)
                    {
                        float ym = 0;
                        for (int l = (int)(Main.maxTilesX * 0.85f); l < (int)(Main.maxTilesX); l++)
                        {
                            ym = (l - Main.maxTilesX * 0.85f) / (float)(Main.maxTilesX * 0.15f);
                            for (int m = Main.maxTilesY / 2 - 400; m < Main.maxTilesY * 0.64f; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 395) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 1].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m <= (Main.maxTilesY / 2 - 395) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f && m > (Main.maxTilesY / 2 - 400) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        float ym = 0;
                        for (int l = (int)(Main.maxTilesX * 0.85f); l < (int)(Main.maxTilesX); l++)
                        {
                            ym = (l - Main.maxTilesX * 0.85f) / (float)(Main.maxTilesX * 0.15f);
                            for (int m = Main.maxTilesY / 2 - 470; m < Main.maxTilesY * 0.6f; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 465) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.22f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 1].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m <= (Main.maxTilesY / 2 - 465) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f && m > (Main.maxTilesY / 2 - 470) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.22f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                    }
                    jindu += 1;
                }
                if (jindu == 107)//火山大陆
                {
                    string text0 = "生成火山大陆…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        float ym = 0;
                        for (int l = (int)(Main.maxTilesX * 0.65f); l < (int)(Main.maxTilesX * 0.85f); l++)
                        {
                            if (ym <= 20 && l < (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            if (ym > 20 && l < (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-1.2f, 0.6f);
                            }
                            if (ym >= 0 && l > (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-1.2f, 0.6f);
                            }
                            if (ym < 0 && l > (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            for (int m = Main.maxTilesY / 2 - 550; m < Main.maxTilesY * 0.6f; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 395) - ym)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 2].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m <= (Main.maxTilesY / 2 - 395) - ym && m > (Main.maxTilesY / 2 - 400) - ym)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                    }
                    else if (Main.maxTilesX == 6400)
                    {
                        float ym = 0;
                        for (int l = (int)(Main.maxTilesX * 0.65f); l < (int)(Main.maxTilesX * 0.85f); l++)
                        {
                            if (ym <= 20 && l < (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            if (ym > 20 && l < (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-1.2f, 0.6f);
                            }
                            if (ym >= 0 && l > (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-1.2f, 0.6f);
                            }
                            if (ym < 0 && l > (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            for (int m = Main.maxTilesY / 2 - 550; m < Main.maxTilesY * 0.64f; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 395) - ym)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 2].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m <= (Main.maxTilesY / 2 - 395) - ym && m > (Main.maxTilesY / 2 - 400) - ym)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        float ym = 0;
                        for (int l = (int)(Main.maxTilesX * 0.65f); l < (int)(Main.maxTilesX * 0.85f); l++)
                        {
                            if (ym <= 20 && l < (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            if (ym > 20 && l < (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-1.2f, 0.6f);
                            }
                            if (ym >= 0 && l > (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-1.2f, 0.6f);
                            }
                            if (ym < 0 && l > (int)(Main.maxTilesX * 0.75f))
                            {
                                ym += Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            for (int m = Main.maxTilesY / 2 - 550; m < Main.maxTilesY * 0.6f; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 465) - ym)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 1].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m <= (Main.maxTilesY / 2 - 465) - ym && m > (Main.maxTilesY / 2 - 470) - ym)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                    }
                    jindu += 1;
                }
                if(Math.Abs(seabedH) > 15)
                {
                    seabedH *= 0.95f;
                }
                if (jindu == 200)//生成大陆架
                {
                    string text0 = "生成大陆斜坡…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    float ym = 0;
                    for (int l = 0; l < Main.maxTilesX / 8; l++)
                    {
                        seabedH += Main.rand.NextFloat(-0.35f, 0.35f);
                        ym = l / (float)(Main.maxTilesX / 8f) - 0.1f;
                        if(Main.maxTilesX == 4200)
                        {
                            for (int m = Main.maxTilesY / 2 - 400 + (int)seabedH; m < Main.maxTilesY - 10; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 395) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f + (int)seabedH)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 1].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m < (Main.maxTilesY / 2 - 395) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f + (int)seabedH && m > (Main.maxTilesY / 2 - 400) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f + (int)seabedH)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                        if (Main.maxTilesX == 6400)
                        {
                            for (int m = Main.maxTilesY / 2 - 445 + (int)seabedH; m < Main.maxTilesY - 10; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 440) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f + (int)seabedH)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 1].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m < (Main.maxTilesY / 2 - 440) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f + (int)seabedH && m > (Main.maxTilesY / 2 - 445) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f + (int)seabedH)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                        if (Main.maxTilesX == 8400)
                        {
                            for (int m = Main.maxTilesY / 2 - 645 + (int)seabedH; m < Main.maxTilesY - 10; m++)
                            {
                                if (m > (Main.maxTilesY / 2 - 640) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 397;
                                        Main.tile[l, m + 1].wall = 187;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                                if (m < (Main.maxTilesY / 2 - 640) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f + (int)seabedH && m > (Main.maxTilesY / 2 - 645) - (Math.Cos(ym * Math.PI) - 1) * Main.maxTilesY * 0.15f + (int)seabedH)
                                {
                                    if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                    {
                                        Main.tile[l, m].type = 53;
                                        Main.tile[l, m].active(true);
                                    }
                                }
                            }
                        }
                    }
                }
                if(jindu == 201)//设置珊瑚礁
                {
                    string text0 = "创建珊瑚礁…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    for (int l = Main.maxTilesX / 24;l < Main.maxTilesX * 0.35f; l++)
                    {
                        for (int m = Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f) - 150; m < Main.maxTilesY / 2 - 404 + (int)(Main.maxTilesY * 0.3f); m++)
                        {
                            if (Main.rand.Next(3000) == 1)
                            {
                                if (Main.rand.Next(200) >= 100)
                                {
                                    float r = Main.rand.NextFloat(2f, 7f);
                                    for (float xm = l - r; xm < l + r; xm += 1)
                                    {
                                        for (float ym = m - r; ym < m + r; ym += 1)
                                        {
                                            if (new Vector2(xm - l, ym - m).Length() <= r)
                                            {
                                                Main.tile[(int)(xm), (int)(ym)].type = (ushort)mod.TileType("黄纽扣珊瑚");
                                                if (new Vector2(xm - l, ym - m).Length() <= r - 1)
                                                {
                                                    Main.tile[(int)(xm), (int)(ym)].wall = (ushort)mod.WallType("黄纽扣珊瑚墙");
                                                }
                                                Main.tile[(int)(xm), (int)(ym)].active(true);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    float r = Main.rand.NextFloat(2f, 7f);
                                    for (float xm = l - r; xm < l + r; xm += 1)
                                    {
                                        for (float ym = m - r; ym < m + r; ym += 1)
                                        {
                                            if (new Vector2(xm - l, ym - m).Length() <= r)
                                            {
                                                Main.tile[(int)(xm), (int)(ym)].type = (ushort)mod.TileType("青纽扣珊瑚");
                                                if (new Vector2(xm - l, ym - m).Length() <= r - 1)
                                                {
                                                    Main.tile[(int)(xm), (int)(ym)].wall = (ushort)mod.WallType("青纽扣珊瑚墙");
                                                }
                                                Main.tile[(int)(xm), (int)(ym)].active(true);
                                            }
                                        }
                                    }
                                }
                            }
                            if (Main.rand.Next(2000) == 1)
                            {
                                if(Main.rand.Next(2000) > 1000)
                                {
                                    Vector2 v = new Vector2(l, m);
                                    for (int z = Main.rand.Next(100); z < 150; z += 1)
                                    {
                                        if (Main.rand.Next(8) == 1)
                                        {
                                            if (Main.rand.Next(2) == 1)
                                            {
                                                v.Y += 1;
                                            }
                                            else
                                            {
                                                v.Y -= 1;
                                            }
                                        }
                                        else
                                        {
                                            v.X += 1;
                                        }
                                        float thick = 1;
                                        if (thick < 5)
                                        {
                                            thick += Main.rand.NextFloat(-0.5f, 0.8f);
                                        }
                                        else
                                        {
                                            thick -= Main.rand.NextFloat(-0.5f, 1.2f);
                                        }
                                        if (thick < 0.8f)
                                        {
                                            thick += 0.2f;
                                        }
                                        for (float x = -thick; x < thick; x += 1)
                                        {
                                            for (float y = -thick; y < thick; y += 1)
                                            {
                                                if (Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("沧流矿")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("渊海矿")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("Basalt")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("橄榄石")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("硫磺矿")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("熔岩石")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("黄纽扣珊瑚")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("青纽扣珊瑚")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("龙息矿") && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != 53 && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != 397 && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("橄榄石矿"))
                                                {
                                                    Main.tile[(int)(v.X + x), (int)(v.Y + y)].type = 397;
                                                    Main.tile[(int)(v.X + x), (int)(v.Y + y) + 1].wall = 187;
                                                    Main.tile[(int)(v.X + x), (int)(v.Y + y)].active(true);
                                                }
                                                if (Main.rand.Next(50) == 1)
                                                {
                                                    Projectile.NewProjectile((v.X + x) * 16 - 40, (v.Y + y) * 16 - 120, 0, -2, mod.ProjectileType("珊瑚random"), 0, 0, Main.myPlayer, 10, 0f);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    Vector2 v = new Vector2(l, m);
                                    for (int z = Main.rand.Next(100); z < 150; z += 1)
                                    {
                                        if (Main.rand.Next(8) == 1)
                                        {
                                            if (Main.rand.Next(2) == 1)
                                            {
                                                v.Y += 1;
                                            }
                                            else
                                            {
                                                v.Y -= 1;
                                            }
                                        }
                                        else
                                        {
                                            v.X += 1;
                                        }
                                        float thick = 1;
                                        if (thick < 5)
                                        {
                                            thick += Main.rand.NextFloat(-0.5f, 0.8f);
                                        }
                                        else
                                        {
                                            thick -= Main.rand.NextFloat(-0.5f, 1.2f);
                                        }
                                        if (thick < 0.8f)
                                        {
                                            thick += 0.2f;
                                        }
                                        for (float x = -thick; x < thick; x += 1)
                                        {
                                            for (float y = -thick; y < thick; y += 1)
                                            {
                                                if (Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("沧流矿")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("渊海矿")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("Basalt")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("橄榄石")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("硫磺矿")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("熔岩石")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("黄纽扣珊瑚")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("青纽扣珊瑚")
                                                && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("龙息矿") && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != 53 && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != 397 && Main.tile[(int)(v.X + x), (int)(v.Y + y)].type != mod.TileType("橄榄石矿"))
                                                {
                                                    if(Main.rand.Next(100) < 90)
                                                    {
                                                        Main.tile[(int)(v.X + x), (int)(v.Y + y)].type = 315;
                                                        Main.tile[(int)(v.X + x), (int)(v.Y + y) + 1].wall = 187;
                                                        Main.tile[(int)(v.X + x), (int)(v.Y + y)].active(true);
                                                    }
                                                }
                                                if (Main.rand.Next(500) == 1)
                                                {
                                                    Projectile.NewProjectile((v.X + x) * 16 - 40, (v.Y + y) * 16 - 120, 0, -2, mod.ProjectileType("珊瑚random"), 0, 0, Main.myPlayer, 10, 0f);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (jindu >= 300 && jindu <= 369)//海床
                {
                    string text0 = "铺设海床…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    for (int l = Main.maxTilesX / 8 + (int)(Main.maxTilesX * ((jindu - 300) / 80f)); l < Main.maxTilesX / 8 + (int)(Main.maxTilesX * ((jindu + 1 - 300) / 80f)); l++)
                    {
                        seabedH += Main.rand.NextFloat(-0.5f,0.5f);
                        if(Main.maxTilesX == 4200)
                        {
                            for (int m = Main.maxTilesY / 2 - 408 + (int)(Main.maxTilesY * 0.3f) + (int)seabedH; m < Main.maxTilesY - 10; m++)
                            {
                                if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                {
                                    Main.tile[l, m].type = 397;
                                    Main.tile[l, m + 1].wall = 187;
                                    Main.tile[l, m].active(true);
                                }
                            }
                        }
                        if (Main.maxTilesX == 6400)
                        {
                            for (int m = Main.maxTilesY / 2 - 453 + (int)(Main.maxTilesY * 0.3f) + (int)seabedH; m < Main.maxTilesY - 10; m++)
                            {
                                if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                {
                                    Main.tile[l, m].type = 397;
                                    Main.tile[l, m + 1].wall = 187;
                                    Main.tile[l, m].active(true);
                                }
                            }
                        }
                        if (Main.maxTilesX == 8400)
                        {
                            for (int m = Main.maxTilesY / 2 - 653 + (int)(Main.maxTilesY * 0.3f) + (int)seabedH; m < Main.maxTilesY - 10; m++)
                            {
                                if (Main.tile[l, m].type != mod.TileType("沧流矿")
                                    && Main.tile[l, m].type != mod.TileType("渊海矿")
                                    && Main.tile[l, m].type != mod.TileType("Basalt")
                                    && Main.tile[l, m].type != mod.TileType("橄榄石")
                                    && Main.tile[l, m].type != mod.TileType("硫磺矿")
                                    && Main.tile[l, m].type != mod.TileType("熔岩石")
                                    && Main.tile[l, m].type != mod.TileType("龙息矿") && Main.tile[l, m].type != 53 && Main.tile[l, m].type != 397 && Main.tile[l, m].type != mod.TileType("橄榄石矿"))
                                {
                                    Main.tile[l, m].type = 397;
                                    Main.tile[l, m + 1].wall = 187;
                                    Main.tile[l, m].active(true);
                                }
                            }
                        }
                    }
                }
                if(jindu == 402)//小火山口挖洞
                {
                    string text0 = "给火山口挖洞…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    for (int count = 0; count < VolC; count++)
                    {
                        int VCenter = 0;
                        if (VolH1 != 0 && count == 0)
                        {
                            VCenter = VolH1;
                        }
                        if (VolH2 != 0 && count == 1)
                        {
                            VCenter = VolH2;
                        }
                        if (VolH3 != 0 && count == 2)
                        {
                            VCenter = VolH3;
                        }
                        if (VolH4 != 0 && count == 3)
                        {
                            VCenter = VolH4;
                        }
                        if (VolH5 != 0 && count == 4)
                        {
                            VCenter = VolH5;
                        }
                        if (VolH6 != 0 && count == 5)
                        {
                            VCenter = VolH6;
                        }
                        if (VolH7 != 0 && count == 6)
                        {
                            VCenter = VolH7;
                        }
                        if (VolH8 != 0 && count == 7)
                        {
                            VCenter = VolH8;
                        }
                        if (VolH9 != 0 && count == 8)
                        {
                            VCenter = VolH9;
                        }
                        if (VolH10 != 0 && count == 9)
                        {
                            VCenter = VolH10;
                        }
                        if (VolH11 != 0 && count == 10)
                        {
                            VCenter = VolH11;
                        }
                        if (VolH12 != 0 && count == 11)
                        {
                            VCenter = VolH12;
                        }
                        if (VCenter != 0)
                        {
                            float De = Main.rand.NextFloat(0.5f,0.59f);
                            float xh = 0;
                            float Rh = 8;
                            for (int m = 0; m < (int)(Main.maxTilesY * 0.55f); m++)
                            {
                                xh += Main.rand.NextFloat(-0.2f, 0.2f);
                                if (Math.Abs(xh) > 6)
                                {
                                    xh *= 0.9f;
                                }
                                if (m < Main.maxTilesY * 0.4f)
                                {
                                    Rh += Main.rand.NextFloat(-0.2f, 0.2f);
                                    if (Math.Abs(Rh) > 10)
                                    {
                                        Rh *= 0.9f;
                                    }
                                    if (Math.Abs(Rh) < 5)
                                    {
                                        Rh *= 1.2f;
                                    }
                                }
                                if (m >= Main.maxTilesY * 0.4f)
                                {
                                    Rh += Main.rand.NextFloat(-0.2f, 0.4f);
                                    if (Math.Abs(Rh) < 5)
                                    {
                                        Rh *= 1.2f;
                                    }
                                }
                                Vector2 v = new Vector2(VCenter + xh, m);
                                for (int x = (int)(v.X - Rh / 2f); x < (int)(v.X + Rh / 2f); x++)
                                {
                                    for (int y = (int)(v.Y - Rh / 2f); y < (int)(v.Y + Rh / 2f); y++)
                                    {
                                        if (new Vector2(v.X - x, v.Y - y).Length() <= Rh / 2f)
                                        {
                                            WorldGen.KillTile(x, y, false, false, true);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (jindu == 403)//火山口挖洞
                {
                    string text0 = "给大火山口挖洞…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    float xh = 0;
                    float Rh = 12;

                    for (int m = 0; m < (int)(Main.maxTilesY * 0.61f); m++)
                    {
                        xh += Main.rand.NextFloat(-0.2f, 0.2f);
                        if (Math.Abs(xh) > 9)
                        {
                            xh *= 0.9f;
                        }
                        if (m < Main.maxTilesY * 0.4f)
                        {
                            Rh += Main.rand.NextFloat(-0.2f, 0.2f);
                            if (Math.Abs(Rh) > 15)
                            {
                                Rh *= 0.9f;
                            }
                            if (Math.Abs(Rh) < 6)
                            {
                                Rh *= 1.2f;
                            }
                        }
                        if (m >= Main.maxTilesY * 0.4f)
                        {
                            Rh += Main.rand.NextFloat(-0.2f, 0.4f);
                            if (Math.Abs(Rh) < 6)
                            {
                                Rh *= 1.2f;
                            }
                        }
                        Vector2 v = new Vector2(Main.maxTilesX * 0.75f + xh, m);
                        for (int x = (int)(v.X - Rh / 2f); x < (int)(v.X + Rh / 2f); x++)
                        {
                            for (int y = (int)(v.Y - Rh / 2f); y < (int)(v.Y + Rh / 2f); y++)
                            {
                                if (new Vector2(v.X - x, v.Y - y).Length() <= Rh / 2f)
                                {
                                    WorldGen.KillTile(x, y, false, false, true);
                                }
                            }
                        }
                    }
                }
                if (jindu == 404)//火山地下
                {
                    string text0 = "创建火山地下…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        float Rh = 12;
                        Vector2 vk = new Vector2((int)(Main.maxTilesX * 0.62f), (int)(Main.maxTilesY * 0.67f));
                        for (int i = 0; i < 900; i++)
                        {
                            if (i < 300 && Rh < 120)
                            {
                                Rh += Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            if (i > 600 && Rh > 12)
                            {
                                Rh -= Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            vk.X += 1;
                            vk.Y += Main.rand.NextFloat(-0.4f, 0.4f);
                            Rh += Main.rand.NextFloat(-0.8f, 0.8f);
                            for (int x = (int)(vk.X - Rh / 2f); x < (int)(vk.X + Rh / 2f); x++)
                            {
                                for (int y = (int)(vk.Y - Rh / 2f); y < (int)(vk.Y + Rh / 2f); y++)
                                {
                                    if (new Vector2(vk.X - x, vk.Y - y).Length() <= Rh / 2f)
                                    {
                                        WorldGen.KillTile(x, y, false, false, true);
                                    }
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 6400)
                    {
                        float Rh = 12;
                        Vector2 vk = new Vector2((int)(Main.maxTilesX * 0.62f), (int)(Main.maxTilesY * 0.67f));
                        for (int i = 0; i < 1400; i++)
                        {
                            if (i < 450 && Rh < 180)
                            {
                                Rh += Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            if (i > 900 && Rh > 18)
                            {
                                Rh -= Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            vk.X += 1;
                            vk.Y += Main.rand.NextFloat(-0.4f, 0.4f);
                            Rh += Main.rand.NextFloat(-0.8f, 0.8f);
                            for (int x = (int)(vk.X - Rh / 2f); x < (int)(vk.X + Rh / 2f); x++)
                            {
                                for (int y = (int)(vk.Y - Rh / 2f); y < (int)(vk.Y + Rh / 2f); y++)
                                {
                                    if (new Vector2(vk.X - x, vk.Y - y).Length() <= Rh / 2f)
                                    {
                                        WorldGen.KillTile(x, y, false, false, true);
                                    }
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        float Rh = 12;
                        Vector2 vk = new Vector2((int)(Main.maxTilesX * 0.62f), (int)(Main.maxTilesY * 0.67f));
                        for (int i = 0; i < 1900; i++)
                        {
                            if (i < 600 && Rh < 240)
                            {
                                Rh += Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            if (i > 1200 && Rh > 24)
                            {
                                Rh -= Main.rand.NextFloat(-0.6f, 1.2f);
                            }
                            vk.X += 1;
                            vk.Y += Main.rand.NextFloat(-0.4f, 0.4f);
                            Rh += Main.rand.NextFloat(-0.8f, 0.8f);
                            for (int x = (int)(vk.X - Rh / 2f); x < (int)(vk.X + Rh / 2f); x++)
                            {
                                for (int y = (int)(vk.Y - Rh / 2f); y < (int)(vk.Y + Rh / 2f); y++)
                                {
                                    if (new Vector2(vk.X - x, vk.Y - y).Length() <= Rh / 2f)
                                    {
                                        WorldGen.KillTile(x, y, false, false, true);
                                    }
                                }
                            }
                        }
                    }
                }
                if (jindu == 405)//火山地下熔岩晶石
                {
                    string text0 = "生成熔岩晶石…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    Vector2 vk = new Vector2((int)(Main.maxTilesX * 0.62f), (int)(Main.maxTilesY * 0.67f));
                        for (int i = 0; i < 900; i++)
                        {
                            vk.X += 1;
                            vk.Y += Main.rand.NextFloat(-0.4f, 0.4f);
                            if (Main.rand.Next(20) == 1)
                            {
                                Projectile.NewProjectile(vk.X * 16, vk.Y * 16, 0, -2, mod.ProjectileType("火山生成3"), 0, 0, Main.myPlayer, 10, 0f);
                            }
                        }
                }
                if (jindu >= 406 && jindu <= 426)//火山洞穴
                {
                    string text0 = "随机挖洞中…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        float Rh = 12;
                        Vector2 vk = new Vector2((int)(Main.maxTilesX * 0.68f + (jindu - 406) / 20f * Main.maxTilesX * 0.2f), (int)(Main.maxTilesY * Main.rand.NextFloat(0.14f, 0.6f)));
                        Vector2 vl = new Vector2(0, 1);
                        float xl = 0;
                        float yl = 0;
                        int XYl = 1;
                        for (int i = Main.rand.Next(0,1200); i < 1800; i++)
                        {
                            if (vk.X < Main.maxTilesX * 0.72f)
                            {
                                XYl = 1;
                            }
                            if (vk.X > Main.maxTilesX * 0.8f)
                            {
                                XYl = 2;
                            }
                            if (XYl == 1)
                            {
                                xl += Main.rand.NextFloat(0, 1.8f);
                                if(Main.rand.Next(20) == 0)
                                {
                                    XYl = 2;
                                }
                            }
                            if (XYl == 2)
                            {
                                xl += Main.rand.NextFloat(-1.8f, 0f);
                                if (Main.rand.Next(20) == 0)
                                {
                                    XYl = 1;
                                }
                            }
                            yl = Main.rand.NextFloat(-3f, 12f);
                            vl = vl + new Vector2(xl, yl);
                            vl = vl / vl.Length();
                            vk += vl;
                            if(Rh < 25 && Rh > 3)
                            {
                                Rh += Main.rand.NextFloat(-1.8f, 1.8f);
                            }
                            else if(Rh < 3)
                            {
                                Rh += Main.rand.NextFloat(0f, 1.8f);
                            }
                            else
                            {
                                Rh += Main.rand.NextFloat(-1.8f, 0f);
                            }
                            for (int x = (int)(vk.X - Rh / 2f); x < (int)(vk.X + Rh / 2f); x++)
                            {
                                for (int y = (int)(vk.Y - Rh / 2f); y < (int)(vk.Y + Rh / 2f); y++)
                                {
                                    if (new Vector2(vk.X - x, vk.Y - y).Length() <= Rh / 2f)
                                    {
                                        WorldGen.KillTile(x, y, false, false, true);
                                    }
                                    if(Main.rand.Next(2000) == 1)
                                    {
                                        int rk = Main.rand.Next(4, 27); 
                                        for(int ik = 0;ik < rk;ik++)
                                        {
                                            Main.tile[x, y].lava(true);
                                            Main.tile[x, y].liquid = byte.MaxValue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 6400)
                    {
                        float Rh = 12;
                        Vector2 vk = new Vector2((int)(Main.maxTilesX * 0.71f + (jindu - 406) / 20f * Main.maxTilesX * 0.2f), (int)(Main.maxTilesY * Main.rand.NextFloat(0.27f, 0.65f)));
                        Vector2 vl = new Vector2(0, 1);
                        float xl = 0;
                        float yl = 0;
                        int XYl = 1;
                        for (int i = Main.rand.Next(0, 1200); i < 2700; i++)
                        {
                            if (vk.X < Main.maxTilesX * 0.73f)
                            {
                                XYl = 1;
                            }
                            if (vk.X > Main.maxTilesX * 0.79f)
                            {
                                XYl = 2;
                            }
                            if (XYl == 1)
                            {
                                xl += Main.rand.NextFloat(0, 1.8f);
                                if (Main.rand.Next(20) == 0)
                                {
                                    XYl = 2;
                                }
                            }
                            if (XYl == 2)
                            {
                                xl += Main.rand.NextFloat(-1.8f, 0f);
                                if (Main.rand.Next(20) == 0)
                                {
                                    XYl = 1;
                                }
                            }
                            yl = Main.rand.NextFloat(-3f, 12f);
                            vl = vl + new Vector2(xl, yl);
                            vl = vl / vl.Length();
                            vk += vl;
                            if (Rh < 25 && Rh > 3)
                            {
                                Rh += Main.rand.NextFloat(-1.8f, 1.8f);
                            }
                            else if (Rh < 3)
                            {
                                Rh += Main.rand.NextFloat(0f, 1.8f);
                            }
                            else
                            {
                                Rh += Main.rand.NextFloat(-1.8f, 0f);
                            }
                            for (int x = (int)(vk.X - Rh / 2f); x < (int)(vk.X + Rh / 2f); x++)
                            {
                                for (int y = (int)(vk.Y - Rh / 2f); y < (int)(vk.Y + Rh / 2f); y++)
                                {
                                    if (new Vector2(vk.X - x, vk.Y - y).Length() <= Rh / 2f)
                                    {
                                        WorldGen.KillTile(x, y, false, false, true);
                                    }
                                    if (Main.rand.Next(2000) == 1)
                                    {
                                        int rk = Main.rand.Next(4, 27);
                                        for (int ik = 0; ik < rk; ik++)
                                        {
                                            Main.tile[x, y].lava(true);
                                            Main.tile[x, y].liquid = byte.MaxValue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        float Rh = 12;
                        Vector2 vk = new Vector2((int)(Main.maxTilesX * 0.68f + (jindu - 406) / 20f * Main.maxTilesX * 0.2f), (int)(Main.maxTilesY * Main.rand.NextFloat(0.3f, 0.68f)));
                        Vector2 vl = new Vector2(0, 1);
                        float xl = 0;
                        float yl = 0;
                        int XYl = 1;
                        for (int i = Main.rand.Next(0, 1200); i < 3600; i++)
                        {
                            if (vk.X < Main.maxTilesX * 0.7f)
                            {
                                XYl = 1;
                            }
                            if (vk.X > Main.maxTilesX * 0.8f)
                            {
                                XYl = 2;
                            }
                            if (XYl == 1)
                            {
                                xl += Main.rand.NextFloat(0, 1.8f);
                                if (Main.rand.Next(20) == 0)
                                {
                                    XYl = 2;
                                }
                            }
                            if (XYl == 2)
                            {
                                xl += Main.rand.NextFloat(-1.8f, 0f);
                                if (Main.rand.Next(20) == 0)
                                {
                                    XYl = 1;
                                }
                            }
                            yl = Main.rand.NextFloat(-3f, 12f);
                            vl = vl + new Vector2(xl, yl);
                            vl = vl / vl.Length();
                            vk += vl;
                            if (Rh < 25 && Rh > 3)
                            {
                                Rh += Main.rand.NextFloat(-1.8f, 1.8f);
                            }
                            else if (Rh < 3)
                            {
                                Rh += Main.rand.NextFloat(0f, 1.8f);
                            }
                            else
                            {
                                Rh += Main.rand.NextFloat(-1.8f, 0f);
                            }
                            for (int x = (int)(vk.X - Rh / 2f); x < (int)(vk.X + Rh / 2f); x++)
                            {
                                for (int y = (int)(vk.Y - Rh / 2f); y < (int)(vk.Y + Rh / 2f); y++)
                                {
                                    if (new Vector2(vk.X - x, vk.Y - y).Length() <= Rh / 2f)
                                    {
                                        WorldGen.KillTile(x, y, false, false, true);
                                    }
                                    if (Main.rand.Next(2000) == 1)
                                    {
                                        int rk = Main.rand.Next(4, 27);
                                        for (int ik = 0; ik < rk; ik++)
                                        {
                                            Main.tile[x, y].lava(true);
                                            Main.tile[x, y].liquid = byte.MaxValue;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (jindu == 437)//火山洞穴附着物3
                {
                    string text0 = "生成洞穴附着物…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.6f); k < (int)(Main.maxTilesX * 0.75f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.24f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(800) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成2"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 6400)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.6f); k < (int)(Main.maxTilesX * 0.75f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.3f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(800) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成2"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.6f); k < (int)(Main.maxTilesX * 0.75f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.36f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(800) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成2"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                }
                if (jindu == 450)//种上椰子树
                {
                    string text0 = "种上椰子树…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        for (int k = 20; k < Main.maxTilesX / 20 - 50; k++)
                        {
                            for (int l = 20; l < Main.maxTilesY - 20; l++)
                            {
                                Tile tile = Main.tile[k, l];
                                Tile tile2 = Main.tile[k, l - 1];
                                if (!(!tile.active() || tile.halfBrick() || tile.slope() != 0) && !(tile2.wall != 0 || tile2.liquid != 0) && !(!WorldGen.EmptyTileCheck(k - 1, k + 1, l - 30, l - 1, 20)) && Main.rand.Next(4) == 0)
                                {
                                    int num2 = WorldGen.genRand.Next(10, 21);
                                    int num3 = WorldGen.genRand.Next(-8, 9);
                                    num3 *= 2;
                                    short num4 = 0;
                                    for (int j = 0; j < num2; j++)
                                    {
                                        tile = Main.tile[k, l - 1 - j];
                                        if (j == 0)
                                        {
                                            tile.active(true);
                                            tile.type = 323;
                                            tile.frameX = 66;
                                            tile.frameY = 0;
                                        }
                                        else if (j == num2 - 1)
                                        {
                                            tile.active(true);
                                            tile.type = 323;
                                            tile.frameX = (short)(22 * WorldGen.genRand.Next(4, 7));
                                            tile.frameY = num4;
                                        }
                                        else
                                        {
                                            if ((int)num4 != num3)
                                            {
                                                float num5 = (float)j / (float)num2;
                                                bool flag = num5 >= 0.25f && ((num5 < 0.5f && WorldGen.genRand.Next(13) == 0) || (num5 < 0.7f && WorldGen.genRand.Next(9) == 0) || num5 >= 0.95f || WorldGen.genRand.Next(5) != 0 || true);
                                                if (flag)
                                                {
                                                    short num6 = (short)Math.Sign(num3);
                                                    num4 += (short)(num6 * 2);
                                                }
                                            }
                                            tile.active(true);
                                            tile.type = 323;
                                            tile.frameX = (short)(22 * WorldGen.genRand.Next(0, 3));
                                            tile.frameY = num4;
                                        }
                                    }
                                    WorldGen.RangeFrame(k - 2, l - num2 - 1, k + 2, l + 1);
                                    if (Main.netMode == 2)
                                    {
                                        NetMessage.SendTileSquare(-1, k, (int)((double)l - (double)num2 * 0.5), num2 + 1, TileChangeType.None);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int k = 20; k < Main.maxTilesX / 20 - 110; k++)
                        {
                            for (int l = 20; l < Main.maxTilesY - 20; l++)
                            {
                                Tile tile = Main.tile[k, l];
                                Tile tile2 = Main.tile[k, l - 1];
                                if (!(!tile.active() || tile.halfBrick() || tile.slope() != 0) && !(tile2.wall != 0 || tile2.liquid != 0) && !(!WorldGen.EmptyTileCheck(k - 1, k + 1, l - 30, l - 1, 20)) && Main.rand.Next(4) == 0)
                                {
                                    int num2 = WorldGen.genRand.Next(10, 21);
                                    int num3 = WorldGen.genRand.Next(-8, 9);
                                    num3 *= 2;
                                    short num4 = 0;
                                    for (int j = 0; j < num2; j++)
                                    {
                                        tile = Main.tile[k, l - 1 - j];
                                        if (j == 0)
                                        {
                                            tile.active(true);
                                            tile.type = 323;
                                            tile.frameX = 66;
                                            tile.frameY = 0;
                                        }
                                        else if (j == num2 - 1)
                                        {
                                            tile.active(true);
                                            tile.type = 323;
                                            tile.frameX = (short)(22 * WorldGen.genRand.Next(4, 7));
                                            tile.frameY = num4;
                                        }
                                        else
                                        {
                                            if ((int)num4 != num3)
                                            {
                                                float num5 = (float)j / (float)num2;
                                                bool flag = num5 >= 0.25f && ((num5 < 0.5f && WorldGen.genRand.Next(13) == 0) || (num5 < 0.7f && WorldGen.genRand.Next(9) == 0) || num5 >= 0.95f || WorldGen.genRand.Next(5) != 0 || true);
                                                if (flag)
                                                {
                                                    short num6 = (short)Math.Sign(num3);
                                                    num4 += (short)(num6 * 2);
                                                }
                                            }
                                            tile.active(true);
                                            tile.type = 323;
                                            tile.frameX = (short)(22 * WorldGen.genRand.Next(0, 3));
                                            tile.frameY = num4;
                                        }
                                    }
                                    WorldGen.RangeFrame(k - 2, l - num2 - 1, k + 2, l + 1);
                                    if (Main.netMode == 2)
                                    {
                                        NetMessage.SendTileSquare(-1, k, (int)((double)l - (double)num2 * 0.5), num2 + 1, TileChangeType.None);
                                    }
                                }
                            }
                        }
                    }
                }
                if (jindu == 451)//继续种椰子树
                {
                    string text0 = "种上椰子树…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    for (int k = (int)(Main.maxTilesX * 0.65f); k < (int)(Main.maxTilesX * 0.86f); k++)
                    {
                        for (int l = 20; l < Main.maxTilesY - 20; l++)
                        {
                            Tile tile = Main.tile[k, l];
                            Tile tile2 = Main.tile[k, l - 1];
                            if (!(!tile.active() || tile.halfBrick() || tile.slope() != 0) && !(tile2.wall != 0 || tile2.liquid != 0) && !(!WorldGen.EmptyTileCheck(k - 1, k + 1, l - 30, l - 1, 20)) && Main.rand.Next(4) == 0)
                            {
                                int num2 = WorldGen.genRand.Next(10, 21);
                                int num3 = WorldGen.genRand.Next(-8, 9);
                                num3 *= 2;
                                short num4 = 0;
                                for (int j = 0; j < num2; j++)
                                {
                                    tile = Main.tile[k, l - 1 - j];
                                    if (j == 0)
                                    {
                                        tile.active(true);
                                        tile.type = 323;
                                        tile.frameX = 66;
                                        tile.frameY = 0;
                                    }
                                    else if (j == num2 - 1)
                                    {
                                        tile.active(true);
                                        tile.type = 323;
                                        tile.frameX = (short)(22 * WorldGen.genRand.Next(4, 7));
                                        tile.frameY = num4;
                                    }
                                    else
                                    {
                                        if ((int)num4 != num3)
                                        {
                                            float num5 = (float)j / (float)num2;
                                            bool flag = num5 >= 0.25f && ((num5 < 0.5f && WorldGen.genRand.Next(13) == 0) || (num5 < 0.7f && WorldGen.genRand.Next(9) == 0) || num5 >= 0.95f || WorldGen.genRand.Next(5) != 0 || true);
                                            if (flag)
                                            {
                                                short num6 = (short)Math.Sign(num3);
                                                num4 += (short)(num6 * 2);
                                            }
                                        }
                                        tile.active(true);
                                        tile.type = 323;
                                        tile.frameX = (short)(22 * WorldGen.genRand.Next(0, 3));
                                        tile.frameY = num4;
                                    }
                                }
                                WorldGen.RangeFrame(k - 2, l - num2 - 1, k + 2, l + 1);
                                if (Main.netMode == 2)
                                {
                                    NetMessage.SendTileSquare(-1, k, (int)((double)l - (double)num2 * 0.5), num2 + 1, TileChangeType.None);
                                }
                            }
                        }
                    }
                }
                if (jindu >= 460 && jindu < 490)//真正的给海加水
                {
                    string text0 = "给海洋灌水中…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        for (int l = (int)(160 + (jindu - 460)); l < 160 + (Main.maxTilesX - 160) / 30f * (jindu - 459); l++)
                        {
                            for (int m = Main.maxTilesY / 2 - 360; m < Main.maxTilesY / 2 + 300; m++)
                            {
                                if (l < Main.maxTilesX * 0.6f || l > Main.maxTilesX * 0.9f)
                                //if (Math.Abs(Main.maxTilesX * 0.75f - l) > 25 && Math.Abs(VolH1 - l) > 18&& Math.Abs(VolH2 - l) > 18 && Math.Abs(VolH3 - l) > 18&& Math.Abs(VolH4 - l) > 18 && Math.Abs(VolH5 - l) > 18&& Math.Abs(VolH6 - l) > 18 && Math.Abs(VolH7 - l) > 18&& Math.Abs(VolH8 - l) > 18 && Math.Abs(VolH9 - l) > 18&& Math.Abs(VolH10 - l) > 18 && Math.Abs(VolH11 - l) > 18&& Math.Abs(VolH12 - l) > 18)
                                {
                                    Main.tile[l, m].liquid = byte.MaxValue;
                                }
                                else
                                {
                                    //Main.tile[l, m].lava(true);
                                    //Main.tile[l, m].liquid = byte.MaxValue;
                                }
                            }
                        }
                    }
                    else if (Main.maxTilesX == 6400)
                    {
                        for (int l = (int)(160 + (jindu - 460)); l < 160 + (Main.maxTilesX - 160) / 30f * (jindu - 459); l++)
                        {
                            for (int m = Main.maxTilesY / 2 - 426; m < Main.maxTilesY / 2 + 300; m++)
                            {
                                if (l < Main.maxTilesX * 0.6f || l > Main.maxTilesX * 0.9f)
                                //if (Math.Abs(Main.maxTilesX * 0.75f - l) > 25 && Math.Abs(VolH1 - l) > 18&& Math.Abs(VolH2 - l) > 18 && Math.Abs(VolH3 - l) > 18&& Math.Abs(VolH4 - l) > 18 && Math.Abs(VolH5 - l) > 18&& Math.Abs(VolH6 - l) > 18 && Math.Abs(VolH7 - l) > 18&& Math.Abs(VolH8 - l) > 18 && Math.Abs(VolH9 - l) > 18&& Math.Abs(VolH10 - l) > 18 && Math.Abs(VolH11 - l) > 18&& Math.Abs(VolH12 - l) > 18)
                                {
                                    Main.tile[l, m].liquid = byte.MaxValue;
                                }
                                else
                                {
                                    //Main.tile[l, m].lava(true);
                                    //Main.tile[l, m].liquid = byte.MaxValue;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int l = (int)(160 + (jindu - 460)); l < 160 + (Main.maxTilesX - 160) / 30f * (jindu - 459); l++)
                        {
                            for (int m = Main.maxTilesY / 2 - 480; m < Main.maxTilesY / 2 + 300; m++)
                            {
                                if (l < Main.maxTilesX * 0.6f || l > Main.maxTilesX * 0.9f)
                                //if (Math.Abs(Main.maxTilesX * 0.75f - l) > 25 && Math.Abs(VolH1 - l) > 18&& Math.Abs(VolH2 - l) > 18 && Math.Abs(VolH3 - l) > 18&& Math.Abs(VolH4 - l) > 18 && Math.Abs(VolH5 - l) > 18&& Math.Abs(VolH6 - l) > 18 && Math.Abs(VolH7 - l) > 18&& Math.Abs(VolH8 - l) > 18 && Math.Abs(VolH9 - l) > 18&& Math.Abs(VolH10 - l) > 18 && Math.Abs(VolH11 - l) > 18&& Math.Abs(VolH12 - l) > 18)
                                {
                                    Main.tile[l, m].liquid = byte.MaxValue;
                                }
                                else
                                {
                                    //Main.tile[l, m].lava(true);
                                    //Main.tile[l, m].liquid = byte.MaxValue;
                                }
                            }
                        }
                    }
                }
                if (jindu == 527)//火山洞穴附着物4
                {
                    string text0 = "生成洞穴沉积物…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.75f); k < (int)(Main.maxTilesX * 0.9f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.24f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(800) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成2"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 6400)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.75f); k < (int)(Main.maxTilesX * 0.9f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.3f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(800) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成2"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.6f); k < (int)(Main.maxTilesX * 0.75f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.36f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(800) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成2"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                }
                if (jindu == 550)//罐子
                {
                    string text0 = "生成罐子…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    for (int i = (int)(Main.maxTilesX * 0.6f); i < (int)(Main.maxTilesX * 0.9f); i++)
                    {
                        for (int j = (int)(Main.maxTilesY * 0.67f - 400); j < (int)(Main.maxTilesY * 0.67f + 150); j++)
                        {
                            if (Main.rand.Next(20) == 2)
                            {
                                WorldGen.PlacePot(i, j, 28, 15);
                            }
                        }
                    }
                }
                if (jindu == 570)//火山洞穴附着物1
                {
                    string text0 = "生成洞穴附着物…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.75f); k < (int)(Main.maxTilesX * 0.9f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.24f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(1600) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 6400)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.75f); k < (int)(Main.maxTilesX * 0.9f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.3f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(1600) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.75f); k < (int)(Main.maxTilesX * 0.9f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.36f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(1600) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                }
                if (jindu == 610)//火山洞穴附着物2
                {
                    string text0 = "生成洞穴附着物…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.6f); k < (int)(Main.maxTilesX * 0.75f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.24f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(1600) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 6400)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.6f); k < (int)(Main.maxTilesX * 0.75f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.3f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(1600) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        for (int k = (int)(Main.maxTilesX * 0.6f); k < (int)(Main.maxTilesX * 0.75f); k++)
                        {
                            for (int l = (int)(Main.maxTilesY * 0.36f); l < (int)(Main.maxTilesY * 0.88f); l++)
                            {
                                if (Main.rand.Next(1600) == 1 && Main.tile[k, l].type != mod.TileType("Basalt"))
                                {
                                    Projectile.NewProjectile(k * 16, l * 16, 0, 2, mod.ProjectileType("火山生成"), 0, 0, Main.myPlayer, 10, 0f);
                                }
                            }
                        }
                    }
                }
                if (jindu == 611)//火山地下框架
                {
                    for(int l = 0;l < 11; l++)
                    {
                        float K = Main.rand.NextFloat(-2.5f, 2.5f);
                        int Leng = Main.rand.Next(40, 260);
                        float Thick = Main.rand.NextFloat(1.2f, 3.5f);
                        Vector2 Start = new Vector2(Main.rand.NextFloat((int)(Main.maxTilesX * 0.58f), (int)(Main.maxTilesX * 0.83f)), Main.rand.NextFloat((int)(Main.maxTilesY * 0.64f), (int)(Main.maxTilesY * 0.70f)));
                        for (int i = (int)(Main.maxTilesX * 0.58f); i < (int)(Main.maxTilesX * 0.92f); i++)
                        {
                            for (int j = (int)(Main.maxTilesY * 0.60f); j < (int)(Main.maxTilesY * 0.74f); j++)
                            {
                                float Dx = i - Start.X;
                                float Dy = j - Start.Y;
                                if (Math.Abs(Dx - K * Dy) / Math.Sqrt(1 + K * K) < Thick && !Main.tile[i, j].active())
                                {
                                    Main.tile[i, j].type = (ushort)(mod.TileType("Gypsum"));
                                    Main.tile[i, j].active(true);
                                }
                            }
                        }
                    }
                }
                if (jindu == 612)//火山地下框架
                {
                    string text0 = "创建宝藏地带…";
                    int sY = 0;
                    for (int l = 20; l < Main.maxTilesY - 20; l++)
                    {
                        if (Main.tile[(int)(Main.maxTilesX * 0.54f), l].liquid == byte.MaxValue)
                        {
                            sY = l;
                            break;
                        }
                    }
                    float thick = 1;
                    float Ya = -2;
                    for (int k = (int)(Main.maxTilesX * 0.54f); k < (int)(Main.maxTilesX * 0.62f); k++)
                    {
                        if (thick < 4)
                        {
                            thick += Main.rand.NextFloat(0.05f, 0.24f);
                        }
                        if (thick > 7)
                        {
                            thick -= Main.rand.NextFloat(0.15f, 0.24f);
                        }
                        if (thick >= 4 && thick <= 7)
                        {
                            thick += Main.rand.NextFloat(-0.05f, 0.05f);
                        }
                        Ya += Main.rand.NextFloat(-0.35f, 0.35f);
                        if (Ya > 1.5f)
                        {
                            Ya -= 0.5f;
                        }
                        if (Ya < -5.5f)
                        {
                            Ya += 0.5f;
                        }
                        for (int z = 0; z < thick; z++)
                        {
                            if (!Main.tile[k, (int)(sY + z + Ya)].active())
                            {
                                Main.tile[k, (int)(sY + z + Ya)].type = (ushort)(mod.TileType("ShoreMud"));
                                Main.tile[k, (int)(sY + z + Ya)].active(true);
                            }
                        }
                        for (int z = 0; z < 300; z++)
                        {
                            if (Main.tile[k, (int)(sY + z + Ya + 1)].wall == 0)
                            {
                                Main.tile[k, (int)(sY + z + Ya + 1)].wall = (ushort)(mod.WallType("ShoreMudWall"));
                            }
                        }
                        if (Main.rand.Next(34) == 1)
                        {
                            float Yb = Ya - 1f;
                            int T = Main.rand.Next(4, 15);
                            for (int k0 = 0; k0 < T; k0++)
                            {
                                if (k0 < T - 3)
                                {
                                    if (Yb > Ya - 9f)
                                    {
                                        Yb -= Main.rand.NextFloat(0.4f, 2.9f);
                                    }
                                    else
                                    {
                                        Yb += Main.rand.NextFloat(-1f, 1f);
                                    }
                                }
                                else
                                {
                                    if (Yb < Ya - 2f)
                                    {
                                        Yb += Main.rand.NextFloat(0.4f, 2.9f);
                                    }
                                }
                                for (int z = 0; z < Ya - Yb + 2; z++)
                                {
                                    if (!Main.tile[k + k0, (int)(sY + z + Yb)].active())
                                    {
                                        Main.tile[k + k0, (int)(sY + z + Yb)].type = (ushort)(mod.TileType("ShoreMud"));
                                        Main.tile[k + k0, (int)(sY + z + Yb)].active(true);
                                    }
                                }
                            }
                        }
                        if (Main.rand.Next(16) == 1)
                        {
                            float Yb = Ya - 1f;
                            int T = Main.rand.Next(4, 8);
                            for (int k0 = 0; k0 < T; k0++)
                            {
                                if (k0 < T - 2)
                                {
                                    if (Yb > Ya - 3f)
                                    {
                                        Yb -= Main.rand.NextFloat(0.2f, 1.1f);
                                    }
                                    else
                                    {
                                        Yb += Main.rand.NextFloat(-0.5f, 0.5f);
                                    }
                                }
                                else
                                {
                                    if (Yb < Ya + 1f)
                                    {
                                        Yb += Main.rand.NextFloat(0.2f, 1.1f);
                                    }
                                }
                                for (int z = 0; z < Ya - Yb + 2; z++)
                                {
                                    if (!Main.tile[k + k0, (int)(sY + z + Yb)].active())
                                    {
                                        Main.tile[k + k0, (int)(sY + z + Yb)].type = (ushort)(mod.TileType("Basalt"));
                                        Main.tile[k + k0, (int)(sY + z + Yb)].active(true);
                                    }
                                }
                            }
                        }
                    }
                    int Xd = (int)(Main.maxTilesX * 0.57f);
                    int Yd = sY - 37;
                    //if (Main.maxTilesX == 6400)
                    //{
                    //    Xd = 3000;
                    //    Yd = 920;
                    //}
                    //if (Main.maxTilesX == 8400)
                    //{
                    //    Xd = 4000;
                    //    Yd = 910;
                    //}

                    int m = 0;
                    Texture2D tex = mod.GetTexture("UIImages/Ocean/巨大红树墙壁");
                    Color[] colorTex = new Color[tex.Width * tex.Height];
                    tex.GetData(colorTex);

                    for (int y = 0; y < tex.Height; y += 1)
                    {
                        for (int x = 0; x < tex.Width; x += 1)
                        {
                            if (new Color(colorTex[x + y * tex.Width].R, colorTex[x + y * tex.Width].G, colorTex[x + y * tex.Width].B) == new Color(31, 53, 12))
                            {
                                Main.tile[x + Xd, y + Yd].wall = (ushort)mod.WallType("RedTreeLeavesWall");
                                Main.tile[x + Xd, y + Yd].active(false);
                            }
                        }
                        for (int x = 0; x < tex.Width; x += 1)
                        {
                            if (new Color(colorTex[x + y * tex.Width].R, colorTex[x + y * tex.Width].G, colorTex[x + y * tex.Width].B) == new Color(36, 38, 22))
                            {
                                Main.tile[x + Xd, y + Yd].wall = (ushort)mod.WallType("RedTreeWall");
                                Main.tile[x + Xd, y + Yd].active(false);
                            }
                        }
                    }
                    Texture2D tex2 = mod.GetTexture("UIImages/Ocean/巨大红树");
                    Color[] colorTex2 = new Color[tex2.Width * tex2.Height];
                    tex2.GetData(colorTex2);

                    for (int y = 0; y < tex2.Height; y += 1)
                    {
                        for (int x = 0; x < tex2.Width; x += 1)
                        {
                            if (new Color(colorTex2[x + y * tex2.Width].R, colorTex2[x + y * tex2.Width].G, colorTex2[x + y * tex2.Width].B) == new Color(58, 61, 35))
                            {
                                Main.tile[x + Xd, y + Yd].type = (ushort)mod.TileType("RedTreeLarge");
                                Main.tile[x + Xd, y + Yd].active(true);
                            }
                        }
                        for (int x = 0; x < tex2.Width; x += 1)
                        {
                            if (new Color(colorTex2[x + y * tex2.Width].R, colorTex2[x + y * tex2.Width].G, colorTex2[x + y * tex2.Width].B) == new Color(48, 82, 19))
                            {
                                Main.tile[x + Xd, y + Yd].type = (ushort)mod.TileType("RedTreeLeaves");
                                Main.tile[x + Xd, y + Yd].active(true);
                            }
                        }
                    }
                    Texture2D tex3 = mod.GetTexture("UIImages/Ocean/灯塔墙");
                    Color[] colorTex3 = new Color[tex3.Width * tex3.Height];
                    tex3.GetData(colorTex3);

                    for (int y = 0; y < tex3.Height; y += 1)
                    {
                        for (int x = 0; x < tex3.Width; x += 1)
                        {
                            if (new Color(colorTex3[x + y * tex3.Width].R, colorTex3[x + y * tex3.Width].G, colorTex3[x + y * tex3.Width].B) == new Color(40, 40, 37))
                            {
                                Main.tile[x + Xd + 54, y + Yd - 22].wall = 5;
                                Main.tile[x + Xd + 54, y + Yd - 22].active(false);
                            }
                        }
                    }
                    Texture2D tex4 = mod.GetTexture("UIImages/Ocean/灯塔");
                    Color[] colorTex4 = new Color[tex4.Width * tex4.Height];
                    tex4.GetData(colorTex4);

                    for (int y = 0; y < tex4.Height; y += 1)
                    {
                        for (int x = 0; x < tex4.Width; x += 1)
                        {
                            if (new Color(colorTex4[x + y * tex4.Width].R, colorTex4[x + y * tex4.Width].G, colorTex4[x + y * tex4.Width].B) == new Color(95, 96, 87))
                            {
                                Main.tile[x + Xd + 54, y + Yd - 22].type = 38;
                                Main.tile[x + Xd + 54, y + Yd - 22].active(true);
                            }
                        }
                        for (int x = 0; x < tex4.Width; x += 1)
                        {
                            if (new Color(colorTex4[x + y * tex4.Width].R, colorTex4[x + y * tex4.Width].G, colorTex4[x + y * tex4.Width].B) == new Color(86, 62, 40))
                            {
                                Main.tile[x + Xd + 54, y + Yd - 22].type = 214;
                                Main.tile[x + Xd + 54, y + Yd - 22].active(true);
                            }
                        }
                        for (int x = 0; x < tex4.Width; x += 1)
                        {
                            if (new Color(colorTex4[x + y * tex4.Width].R, colorTex4[x + y * tex4.Width].G, colorTex4[x + y * tex4.Width].B) == new Color(158, 98, 98))
                            {
                                Main.tile[x + Xd + 54, y + Yd - 22].type = 19;
                                Main.tile[x + Xd + 54, y + Yd - 22].frameY = 162;
                                Main.tile[x + Xd + 54, y + Yd - 22].active(true);
                            }
                        }
                        for (int x = 0; x < tex4.Width; x += 1)
                        {
                            if (new Color(colorTex4[x + y * tex4.Width].R, colorTex4[x + y * tex4.Width].G, colorTex4[x + y * tex4.Width].B) == new Color(175, 126, 82))
                            {
                                WorldGen.PlaceDoor(x + Xd + 54, y + Yd - 22, 10);
                            }
                        }
                        for (int x = 0; x < tex4.Width; x += 1)
                        {
                            if (new Color(colorTex4[x + y * tex4.Width].R, colorTex4[x + y * tex4.Width].G, colorTex4[x + y * tex4.Width].B) == new Color(255, 0, 0))
                            {
                                WorldGen.PlaceTile(x + Xd + 54, y + Yd - 22, 42, false, false, -1, 6);
                            }
                        }
                        for (int x = 0; x < tex4.Width; x += 1)
                        {
                            if (new Color(colorTex4[x + y * tex4.Width].R, colorTex4[x + y * tex4.Width].G, colorTex4[x + y * tex4.Width].B) == new Color(139, 104, 255))
                            {
                                WorldGen.PlaceTile(x + Xd + 54, y + Yd - 22, mod.TileType("Lamp"));
                            }
                        }
                    }
                }
                if (jindu == 660)//宝藏层
                {
                    string text0 = "创建宝藏地带…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    if (Main.maxTilesX == 4200)
                    {
                        float Rh = 4;
                        for (int i = (int)(Main.maxTilesX * 0.62f + 300); i < (int)(Main.maxTilesX * 0.62f + 600); i++)
                        {
                            for (int j = (int)(Main.maxTilesY * 0.67f - 150); j < (int)(Main.maxTilesY * 0.67f + 150); j++)
                            {
                                float k = i - (Main.maxTilesX * 0.62f + 450);
                                float l = j - Main.maxTilesY * 0.67f;
                                float k2 = i - (Main.maxTilesX * 0.62f + 420);
                                float l2 = j - Main.maxTilesY * 0.67f;
                                float k3 = i - (Main.maxTilesX * 0.62f + 480);
                                float l3 = j - Main.maxTilesY * 0.67f;
                                float k4 = i - (Main.maxTilesX * 0.62f + 450);
                                float l4 = j - Main.maxTilesY * 0.67f - 20;
                                float k5 = i - (Main.maxTilesX * 0.62f + 360);
                                float l5 = j - Main.maxTilesY * 0.67f + 40;
                                float k6 = i - (Main.maxTilesX * 0.62f + 510);
                                float l6 = j - Main.maxTilesY * 0.67f + 40;
                                float k7 = i - (Main.maxTilesX * 0.62f + 420);
                                float l7 = j - Main.maxTilesY * 0.67f + 40;
                                if (Math.Abs(Math.Sqrt(k * k + l * l) - 150) < Rh)
                                {
                                    int x = (int)(i + ((Main.maxTilesX * 0.62f + 450) - i) / 12f);
                                    int y = (int)(j + (Main.maxTilesY * 0.67f - j) / 2f);
                                    Main.tile[x, y].type = (ushort)(mod.TileType("熔岩石"));
                                    Main.tile[x, y].active(true);
                                    Main.tile[x, y + 1].type = 374;
                                    Main.tile[x, y + 1].active(true);
                                }
                                if(Math.Sqrt(k * k + l * l) < 150)
                                {
                                    int x = (int)(i + ((Main.maxTilesX * 0.62f + 450) - i) / 12f);
                                    int y = (int)(j + (Main.maxTilesY * 0.67f - j) / 2f);
                                    Main.tile[x, y].wall = (ushort)mod.WallType("熔岩石墙");
                                    if(Main.tile[x, y].type != mod.TileType("熔岩石") && Main.tile[x, y].type != mod.TileType("熔岩心石") && Main.tile[x, y].type != mod.TileType("地热") && Main.tile[x, y].type != mod.TileType("赤炼魔戒") && Main.tile[x, y].type != mod.TileType("流火之翼") && Main.tile[x, y].type != 374 && Main.tile[x, y].type != mod.TileType("熔火心晶") && Main.tile[x, y].type != mod.TileType("赤月") && Main.tile[x, y].type != mod.TileType("灼烧之怒") && Main.tile[x, y].type != mod.TileType("熔珠"))
                                    {
                                        WorldGen.KillTile(x, y, false, false, true);
                                    }
                                    if (Main.maxTilesY * 0.67f - j < -50)
                                    {
                                        Main.tile[x, y].lava(true);
                                        Main.tile[x, y].liquid = byte.MaxValue;
                                    }
                                    else
                                    {
                                        if(i % 30 == 0 && j % 40 == 0)
                                        {
                                            int xi = Main.rand.Next(-5, 5);
                                            int yi = Main.rand.Next(-5, 5);
                                            Main.tile[x + xi, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi, y + yi].active(true);
                                            Main.tile[x + xi + 1, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 1, y + yi].active(true);
                                            Main.tile[x + xi + 2, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 2, y + yi].active(true);
                                            if (Math.Sqrt(k * k + l * l) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("地热"), true, false, -1, 0);
                                            }
                                            else if(Math.Sqrt(k2 * k2 + l2 * l2) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("赤炼魔戒"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k3 * k3 + l3 * l3) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("流火之翼"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k5 * k5 + l5 * l5) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("赤月"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k6 * k6 + l6 * l6) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("熔珠"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k7 * k7 + l7 * l7) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 2), (ushort)mod.TileType("灼烧之怒"), true, false, -1, 0);
                                            }
                                            else
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi) - 2, (ushort)mod.TileType("熔岩心石"), true, false, -1, 0);
                                            }
                                        }
                                        if (i % 30 == 15 && j % 40 == 20)
                                        {
                                            int xi = Main.rand.Next(-5, 5);
                                            int yi = Main.rand.Next(-5, 5);
                                            Main.tile[x + xi, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi, y + yi].active(true);
                                            Main.tile[x + xi + 1, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 1, y + yi].active(true);
                                            Main.tile[x + xi + 2, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 2, y + yi].active(true);
                                            if (Math.Sqrt(k4 * k4 + l4 * l4) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 5), (ushort)mod.TileType("熔火心晶"), true, false, -1, 0);
                                            }
                                            else
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi) - 2, (ushort)mod.TileType("熔岩心石"), true, false, -1, 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 6400)
                    {
                        float Rh = 4;
                        for (int i = (int)(Main.maxTilesX * 0.665f + 225); i < (int)(Main.maxTilesX * 0.665f + 675); i++)
                        {
                            for (int j = (int)(Main.maxTilesY * 0.67f - 225); j < (int)(Main.maxTilesY * 0.67f + 225); j++)
                            {
                                float k = i - (Main.maxTilesX * 0.665f + 450);
                                float l = j - Main.maxTilesY * 0.67f;
                                float k2 = i - (Main.maxTilesX * 0.665f + 420);
                                float l2 = j - Main.maxTilesY * 0.67f;
                                float k3 = i - (Main.maxTilesX * 0.665f + 480);
                                float l3 = j - Main.maxTilesY * 0.67f;
                                float k4 = i - (Main.maxTilesX * 0.665f + 450);
                                float l4 = j - Main.maxTilesY * 0.67f - 20;
                                float k5 = i - (Main.maxTilesX * 0.62f + 360);
                                float l5 = j - Main.maxTilesY * 0.67f + 40;
                                float k6 = i - (Main.maxTilesX * 0.62f + 510);
                                float l6 = j - Main.maxTilesY * 0.67f + 40;
                                float k7 = i - (Main.maxTilesX * 0.62f + 420);
                                float l7 = j - Main.maxTilesY * 0.67f + 40;
                                if (Math.Abs(Math.Sqrt(k * k + l * l) - 225) < Rh)
                                {
                                    int x = (int)(i + ((Main.maxTilesX * 0.665f + 450) - i) / 12f);
                                    int y = (int)(j + (Main.maxTilesY * 0.67f - j) / 2f);
                                    Main.tile[x, y].type = (ushort)(mod.TileType("熔岩石"));
                                    Main.tile[x, y].active(true);
                                    Main.tile[x, y + 1].type = 374;
                                    Main.tile[x, y + 1].active(true);
                                }
                                if (Math.Sqrt(k * k + l * l) < 225)
                                {
                                    int x = (int)(i + ((Main.maxTilesX * 0.665f + 450) - i) / 12f);
                                    int y = (int)(j + (Main.maxTilesY * 0.67f - j) / 2f);
                                    Main.tile[x, y].wall = (ushort)mod.WallType("熔岩石墙");
                                    if (Main.tile[x, y].type != mod.TileType("熔岩石") && Main.tile[x, y].type != mod.TileType("熔岩心石") && Main.tile[x, y].type != mod.TileType("地热") && Main.tile[x, y].type != mod.TileType("赤炼魔戒") && Main.tile[x, y].type != mod.TileType("流火之翼") && Main.tile[x, y].type != 374 && Main.tile[x, y].type != mod.TileType("熔火心晶") && Main.tile[x, y].type != mod.TileType("赤月") && Main.tile[x, y].type != mod.TileType("灼烧之怒") && Main.tile[x, y].type != mod.TileType("熔珠"))
                                    {
                                        WorldGen.KillTile(x, y, false, false, true);
                                    }
                                    if (Main.maxTilesY * 0.67f - j < -50)
                                    {
                                        Main.tile[x, y].lava(true);
                                        Main.tile[x, y].liquid = byte.MaxValue;
                                    }
                                    else
                                    {
                                        if (i % 30 == 0 && j % 40 == 0)
                                        {
                                            int xi = Main.rand.Next(-5, 5);
                                            int yi = Main.rand.Next(-5, 5);
                                            Main.tile[x + xi, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi, y + yi].active(true);
                                            Main.tile[x + xi + 1, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 1, y + yi].active(true);
                                            Main.tile[x + xi + 2, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 2, y + yi].active(true);
                                            if (Math.Sqrt(k * k + l * l) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("地热"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k2 * k2 + l2 * l2) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("赤炼魔戒"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k3 * k3 + l3 * l3) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("流火之翼"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k5 * k5 + l5 * l5) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("赤月"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k6 * k6 + l6 * l6) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("熔珠"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k7 * k7 + l7 * l7) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 2), (ushort)mod.TileType("灼烧之怒"), true, false, -1, 0);
                                            }
                                            else
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi) - 2, (ushort)mod.TileType("熔岩心石"), true, false, -1, 0);
                                            }
                                        }
                                        if (i % 30 == 15 && j % 40 == 20)
                                        {
                                            int xi = Main.rand.Next(-5, 5);
                                            int yi = Main.rand.Next(-5, 5);
                                            Main.tile[x + xi, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi, y + yi].active(true);
                                            Main.tile[x + xi + 1, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 1, y + yi].active(true);
                                            Main.tile[x + xi + 2, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 2, y + yi].active(true);
                                            if (Math.Sqrt(k4 * k4 + l4 * l4) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 5), (ushort)mod.TileType("熔火心晶"), true, false, -1, 0);
                                            }
                                            else
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi) - 2, (ushort)mod.TileType("熔岩心石"), true, false, -1, 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        float Rh = 4;
                        for (int i = (int)(Main.maxTilesX * 0.69f + 150); i < (int)(Main.maxTilesX * 0.69f + 750); i++)
                        {
                            for (int j = (int)(Main.maxTilesY * 0.67f - 300); j < (int)(Main.maxTilesY * 0.67f + 300); j++)
                            {
                                float k = i - (Main.maxTilesX * 0.69f + 450);
                                float l = j - Main.maxTilesY * 0.67f;
                                float k2 = i - (Main.maxTilesX * 0.69f + 420);
                                float l2 = j - Main.maxTilesY * 0.67f;
                                float k3 = i - (Main.maxTilesX * 0.69f + 480);
                                float l3 = j - Main.maxTilesY * 0.67f;
                                float k4 = i - (Main.maxTilesX * 0.69f + 450);
                                float l4 = j - Main.maxTilesY * 0.67f - 20;
                                float k5 = i - (Main.maxTilesX * 0.62f + 360);
                                float l5 = j - Main.maxTilesY * 0.67f + 40;
                                float k6 = i - (Main.maxTilesX * 0.62f + 510);
                                float l6 = j - Main.maxTilesY * 0.67f + 40;
                                float k7 = i - (Main.maxTilesX * 0.62f + 420);
                                float l7 = j - Main.maxTilesY * 0.67f + 40;
                                if (Math.Abs(Math.Sqrt(k * k + l * l) - 300) < Rh)
                                {
                                    int x = (int)(i + ((Main.maxTilesX * 0.69f + 450) - i) / 12f);
                                    int y = (int)(j + (Main.maxTilesY * 0.67f - j) / 2f);
                                    Main.tile[x, y].type = (ushort)(mod.TileType("熔岩石"));
                                    Main.tile[x, y].active(true);
                                    Main.tile[x, y + 1].type = 374;
                                    Main.tile[x, y + 1].active(true);
                                }
                                if (Math.Sqrt(k * k + l * l) < 300)
                                {
                                    int x = (int)(i + ((Main.maxTilesX * 0.69f + 450) - i) / 12f);
                                    int y = (int)(j + (Main.maxTilesY * 0.67f - j) / 2f);
                                    Main.tile[x, y].wall = (ushort)mod.WallType("熔岩石墙");
                                    if (Main.tile[x, y].type != mod.TileType("熔岩石") && Main.tile[x, y].type != mod.TileType("熔岩心石") && Main.tile[x, y].type != mod.TileType("地热") && Main.tile[x, y].type != mod.TileType("赤炼魔戒") && Main.tile[x, y].type != mod.TileType("流火之翼") && Main.tile[x, y].type != 374 && Main.tile[x, y].type != mod.TileType("熔火心晶") && Main.tile[x, y].type != mod.TileType("赤月") && Main.tile[x, y].type != mod.TileType("灼烧之怒") && Main.tile[x, y].type != mod.TileType("熔珠"))
                                    {
                                        WorldGen.KillTile(x, y, false, false, true);
                                    }
                                    if (Main.maxTilesY * 0.67f - j < -50)
                                    {
                                        Main.tile[x, y].lava(true);
                                        Main.tile[x, y].liquid = byte.MaxValue;
                                    }
                                    else
                                    {
                                        if (i % 30 == 0 && j % 40 == 0)
                                        {
                                            int xi = Main.rand.Next(-5, 5);
                                            int yi = Main.rand.Next(-5, 5);
                                            Main.tile[x + xi, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi, y + yi].active(true);
                                            Main.tile[x + xi + 1, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 1, y + yi].active(true);
                                            Main.tile[x + xi + 2, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 2, y + yi].active(true);
                                            if (Math.Sqrt(k * k + l * l) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("地热"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k2 * k2 + l2 * l2) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("赤炼魔戒"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k3 * k3 + l3 * l3) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("流火之翼"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k5 * k5 + l5 * l5) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("赤月"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k6 * k6 + l6 * l6) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 3), (ushort)mod.TileType("熔珠"), true, false, -1, 0);
                                            }
                                            else if (Math.Sqrt(k7 * k7 + l7 * l7) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 2), (ushort)mod.TileType("灼烧之怒"), true, false, -1, 0);
                                            }
                                            else
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi) - 2, (ushort)mod.TileType("熔岩心石"), true, false, -1, 0);
                                            }
                                        }
                                        if (i % 30 == 15 && j % 40 == 20)
                                        {
                                            int xi = Main.rand.Next(-5, 5);
                                            int yi = Main.rand.Next(-5, 5);
                                            Main.tile[x + xi, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi, y + yi].active(true);
                                            Main.tile[x + xi + 1, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 1, y + yi].active(true);
                                            Main.tile[x + xi + 2, y + yi].type = (ushort)(mod.TileType("熔岩石"));
                                            Main.tile[x + xi + 2, y + yi].active(true);
                                            if (Math.Sqrt(k4 * k4 + l4 * l4) < 20)
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi - 5), (ushort)mod.TileType("熔火心晶"), true, false, -1, 0);
                                            }
                                            else
                                            {
                                                WorldGen.PlaceTile((x + xi + 1), (y + yi) - 2, (ushort)mod.TileType("熔岩心石"), true, false, -1, 0);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (false)
                //if (jindu >= 404 && jindu <= 449)//随机挖洞
                {
                    int X = Main.rand.Next((int)(Main.maxTilesX * 0.6f), (int)(Main.maxTilesX * 0.9f));
                    int Y = Main.rand.Next((int)(Main.maxTilesY * 0.65f), (int)(Main.maxTilesY * 0.8f));
                    int num = WorldGen.genRand.Next(2);
                    if (num == 0)
                    {
                        int num2 = WorldGen.genRand.Next(7, 9);
                        float num3 = (float)WorldGen.genRand.Next(100) * 0.01f;
                        float num4 = 1f - num3;
                        if (WorldGen.genRand.Next(2) == 0)
                        {
                            num3 = -num3;
                        }
                        if (WorldGen.genRand.Next(2) == 0)
                        {
                            num4 = -num4;
                        }
                        Vector2 vector4 = new Vector2((float)X, (float)Y);
                        for (int i = 0; i < num2; i++)
                        {
                            vector4 = WorldGen.digTunnel(vector4.X, vector4.Y, num3, num4, WorldGen.genRand.Next(6, 20), WorldGen.genRand.Next(4, 9), false);
                            num3 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
                            num4 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
                            if ((double)num3 < -1.5)
                            {
                                num3 = -1.5f;
                            }
                            if ((double)num3 > 1.5)
                            {
                                num3 = 1.5f;
                            }
                            if ((double)num4 < -1.5)
                            {
                                num4 = -1.5f;
                            }
                            if ((double)num4 > 1.5)
                            {
                                num4 = 1.5f;
                            }
                            float num5 = (float)WorldGen.genRand.Next(100) * 0.01f;
                            float num6 = 1f - num5;
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                num5 = -num5;
                            }
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                num6 = -num6;
                            }
                            Vector2 vector2 = WorldGen.digTunnel(vector4.X, vector4.Y, num5, num6, WorldGen.genRand.Next(30, 50), WorldGen.genRand.Next(3, 6), false);
                            WorldGen.TileRunner((int)vector2.X, (int)vector2.Y, (double)WorldGen.genRand.Next(10, 20), WorldGen.genRand.Next(5, 10), -1, false, 0f, 0f, false, true);
                        }
                        return;
                    }
                    if (num == 1)
                    {
                        int num7 = WorldGen.genRand.Next(15, 30);
                        float num8 = (float)WorldGen.genRand.Next(100) * 0.01f;
                        float num9 = 1f - num8;
                        if (WorldGen.genRand.Next(2) == 0)
                        {
                            num8 = -num8;
                        }
                        if (WorldGen.genRand.Next(2) == 0)
                        {
                            num9 = -num9;
                        }
                        Vector2 vector3 = new Vector2((float)X, (float)Y);
                        for (int j = 0; j < num7; j++)
                        {
                            vector3 = WorldGen.digTunnel(vector3.X, vector3.Y, num8, num9, WorldGen.genRand.Next(5, 15), WorldGen.genRand.Next(2, 6), true);
                            num8 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
                            num9 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
                            if ((double)num8 < -1.5)
                            {
                                num8 = -1.5f;
                            }
                            if ((double)num8 > 1.5)
                            {
                                num8 = 1.5f;
                            }
                            if ((double)num9 < -1.5)
                            {
                                num9 = -1.5f;
                            }
                            if ((double)num9 > 1.5)
                            {
                                num9 = 1.5f;
                            }
                        }
                    }
                }
                if (jindu == 693)//消去物品//海边房屋
                {
                    string text0 = "搭建滨海渔夫小屋…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    for (int m = 0; m < Main.maxTilesY - 10; m++)
                    {
                        if (Main.tile[60, m].type == 53)
                        {
                            shoreHouse = m - 30;
                            break;
                        }
                    }
                    bool labble = true;
                    for (int l = 59; l < 120; l++)
                    {
                        if (l <= 60)
                        {
                            Main.tile[l, shoreHouse - 1].wall = 152;
                        }
                        if (l >= 118)
                        {
                            Main.tile[l, shoreHouse - 1].wall = 152;
                        }
                        if (l <= 98 && l >= 82)
                        {
                            Main.tile[l, shoreHouse - 1].wall = 152;
                        }
                        if (l == 97)
                        {
                            for (int i = 0; i < 10; i++)
                            {
                                Main.tile[l + i, shoreHouse - i - 13].type = 322;
                                Main.tile[l + i, shoreHouse - i - 13].active(true);
                                Main.tile[l + i, shoreHouse - i - 12].type = 322;
                                Main.tile[l + i, shoreHouse - i - 12].active(true);
                                for (int j = 0; j < i; j++)
                                {
                                    Main.tile[l + i, shoreHouse - i - 12 + j + 1].wall = 151;
                                }
                                if (i == 9)
                                {
                                    Main.tile[l + i + 1, shoreHouse - i - 13].type = 322;
                                    Main.tile[l + i + 1, shoreHouse - i - 13].active(true);
                                    Main.tile[l + i + 2, shoreHouse - i - 13].type = 322;
                                    Main.tile[l + i + 2, shoreHouse - i - 13].active(true);
                                    for (int j = 0; j < i + 1; j++)
                                    {
                                        Main.tile[l + i + 1, shoreHouse - i - 12 + j].wall = 151;
                                    }
                                    WorldGen.PlaceTile(l + i + 1, shoreHouse - i - 15, mod.TileType("棕榈木风向标"));//14X
                                    NPC.NewNPC(16 * (l + i + 1), 16 * (shoreHouse - i - 4), 369);
                                    for (int j = 0; j < i + 1; j++)
                                    {
                                        Main.tile[l + i + 2, shoreHouse - i - 12 + j].wall = 151;
                                    }
                                }
                            }
                            for (int i = 10; i > 0; i--)
                            {
                                Main.tile[l + i + 11, shoreHouse + i - 23].type = 322;
                                Main.tile[l + i + 11, shoreHouse + i - 23].active(true);
                                Main.tile[l + i + 11, shoreHouse + i - 22].type = 322;
                                Main.tile[l + i + 11, shoreHouse + i - 22].active(true);
                                for (int j = 10; j > i - 1; j--)
                                {
                                    Main.tile[l + i + 10, shoreHouse + i - 11 - j - 1].wall = 151;
                                }
                            }
                        }
                        if (l == 61)//左侧房屋
                        {
                            for (int i = shoreHouse - 1; i > shoreHouse - 11; i--)
                            {
                                Main.tile[l, i].type = 322;
                                Main.tile[l, i].active(true);
                                if (i == shoreHouse - 10)
                                {
                                    for (int x = 62; x < 82; x++)
                                    {
                                        if (x != 72)
                                        {
                                            Main.tile[x, i].type = 19;
                                            Main.tile[x, i].frameY = 17 * 18;
                                            Main.tile[x, i].active(true);
                                        }
                                        else
                                        {
                                            Main.tile[x, i].type = 322;
                                            Main.tile[x, i].active(true);
                                            WorldGen.PlaceTile(x, i + 1, 34, false, false, -1, 21);
                                            Main.tile[x, i + 1].frameX += 54;
                                            Main.tile[x + 1, i + 1].frameX += 54;
                                            Main.tile[x - 1, i + 1].frameX += 54;
                                            Main.tile[x, i + 2].frameX += 54;
                                            Main.tile[x + 1, i + 2].frameX += 54;
                                            Main.tile[x - 1, i + 2].frameX += 54;
                                            Main.tile[x, i + 3].frameX += 54;
                                            Main.tile[x + 1, i + 3].frameX += 54;
                                            Main.tile[x - 1, i + 3].frameX += 54;
                                        }
                                        if (x == 64)
                                        {
                                            WorldGen.PlaceTile(x, i - 1, 94, false, false, -1, 0);
                                        }
                                    }
                                    Main.tile[81, i - 1].type = 332;
                                    Main.tile[81, i - 1].active(true);
                                    Main.tile[80, i - 1].type = 332;
                                    Main.tile[80, i - 1].active(true);
                                    Main.tile[79, i - 1].type = 332;
                                    Main.tile[79, i - 1].active(true);
                                    Main.tile[79, i - 2].type = 332;
                                    Main.tile[79, i - 2].active(true);
                                    Main.tile[78, i - 1].type = 332;
                                    Main.tile[78, i - 1].active(true);
                                    for (int x = 68; x < 76; x++)
                                    {
                                        Main.tile[x, i - 9].type = 19;
                                        Main.tile[x, i - 9].frameY = 17 * 18;
                                        Main.tile[x, i - 9].active(true);
                                        if (x == 69)
                                        {
                                            Main.tile[x + 1, i - 13].type = 19;
                                            Main.tile[x + 1, i - 13].frameY = 17 * 18;
                                            Main.tile[x + 1, i - 13].active(true);
                                            Main.tile[x + 2, i - 13].type = 19;
                                            Main.tile[x + 2, i - 13].frameY = 17 * 18;
                                            Main.tile[x + 2, i - 13].active(true);
                                            WorldGen.PlaceTile(x + 2, i - 14, 319, false, false, -1, 0);
                                        }
                                    }
                                }
                            }
                            for (int i = shoreHouse - 4; i > shoreHouse - 11; i--)
                            {
                                Main.tile[82, i].type = 322;
                                Main.tile[82, i].active(true);
                            }
                            for (int i = shoreHouse - 11; i > shoreHouse - 35; i--)
                            {
                                if (i == shoreHouse - 11)
                                {
                                    Main.tile[59, i].type = 322;
                                    Main.tile[59, i].active(true);
                                    Main.tile[60, i].type = 322;
                                    Main.tile[60, i].active(true);
                                    Main.tile[83, i].type = 322;
                                    Main.tile[83, i].active(true);
                                    Main.tile[84, i].type = 322;
                                    Main.tile[84, i].active(true);
                                    WorldGen.PlaceTile(83, i + 1, 55, false, false, -1, 1);
                                }
                                if (i == shoreHouse - 12)
                                {
                                    Main.tile[58, i].type = 322;
                                    Main.tile[58, i].active(true);
                                    Main.tile[59, i].type = 322;
                                    Main.tile[59, i].active(true);
                                    Main.tile[60, i].type = 322;
                                    Main.tile[60, i].active(true);
                                    Main.tile[61, i].type = 322;
                                    Main.tile[61, i].active(true);
                                    Main.tile[82, i].type = 322;
                                    Main.tile[82, i].active(true);
                                    Main.tile[83, i].type = 322;
                                    Main.tile[83, i].active(true);
                                    Main.tile[84, i].type = 322;
                                    Main.tile[84, i].active(true);
                                    Main.tile[85, i].type = 322;
                                    Main.tile[85, i].active(true);
                                }
                                if (i == shoreHouse - 13)
                                {
                                    Main.tile[58, i].type = 322;
                                    Main.tile[58, i].active(true);
                                    Main.tile[62, i].type = 322;
                                    Main.tile[62, i].active(true);
                                    Main.tile[61, i].type = 322;
                                    Main.tile[61, i].active(true);
                                    Main.tile[82, i].type = 322;
                                    Main.tile[82, i].active(true);
                                    Main.tile[81, i].type = 322;
                                    Main.tile[81, i].active(true);
                                    Main.tile[85, i].type = 322;
                                    Main.tile[85, i].active(true);
                                    for (int k = i; k < shoreHouse - 1; k++)
                                    {
                                        Main.tile[81, k + 1].wall = 151;
                                        Main.tile[62, k + 1].wall = 151;
                                    }
                                }
                                if (i == shoreHouse - 14)
                                {
                                    Main.tile[63, i].type = 322;
                                    Main.tile[63, i].active(true);
                                    Main.tile[62, i].type = 322;
                                    Main.tile[62, i].active(true);
                                    Main.tile[80, i].type = 322;
                                    Main.tile[80, i].active(true);
                                    Main.tile[81, i].type = 322;
                                    Main.tile[81, i].active(true);
                                    for (int k = i; k < shoreHouse - 1; k++)
                                    {
                                        Main.tile[80, k + 1].wall = 151;
                                        Main.tile[63, k + 1].wall = 151;
                                    }
                                }
                                if (i == shoreHouse - 15)
                                {
                                    Main.tile[63, i].type = 322;
                                    Main.tile[63, i].active(true);
                                    Main.tile[64, i].type = 322;
                                    Main.tile[64, i].active(true);
                                    Main.tile[80, i].type = 322;
                                    Main.tile[80, i].active(true);
                                    Main.tile[79, i].type = 322;
                                    Main.tile[79, i].active(true);
                                    for (int k = i; k < shoreHouse - 1; k++)
                                    {
                                        Main.tile[79, k + 1].wall = 151;
                                        Main.tile[64, k + 1].wall = 151;
                                    }
                                    WorldGen.PlaceTile(64, i + 1, 42, false, false, -1, 6);
                                    WorldGen.PlaceTile(79, i + 1, 42, false, false, -1, 6);
                                    Main.tile[64, i + 1].frameX = 18;
                                    Main.tile[79, i + 1].frameX = 18;
                                    Main.tile[64, i + 2].frameX = 18;
                                    Main.tile[79, i + 2].frameX = 18;
                                }
                                if (i == shoreHouse - 16)
                                {
                                    Main.tile[65, i].type = 322;
                                    Main.tile[65, i].active(true);
                                    Main.tile[64, i].type = 322;
                                    Main.tile[64, i].active(true);
                                    Main.tile[78, i].type = 322;
                                    Main.tile[78, i].active(true);
                                    Main.tile[79, i].type = 322;
                                    Main.tile[79, i].active(true);
                                    for (int k = i; k < shoreHouse - 1; k++)
                                    {
                                        Main.tile[78, k + 1].wall = 151;
                                        Main.tile[65, k + 1].wall = 151;
                                    }
                                }
                                if (i == shoreHouse - 17)
                                {
                                    Main.tile[65, i].type = 322;
                                    Main.tile[65, i].active(true);
                                    Main.tile[66, i].type = 322;
                                    Main.tile[66, i].active(true);
                                    Main.tile[78, i].type = 322;
                                    Main.tile[78, i].active(true);
                                    Main.tile[77, i].type = 322;
                                    Main.tile[77, i].active(true);
                                    for (int k = i; k < shoreHouse - 1; k++)
                                    {
                                        Main.tile[77, k + 1].wall = 151;
                                        Main.tile[66, k + 1].wall = 151;
                                    }
                                    for (int k = i - 1; k < shoreHouse - 1; k++)
                                    {
                                        Main.tile[76, k + 1].wall = 151;
                                        Main.tile[67, k + 1].wall = 151;
                                    }
                                }
                                if (i == shoreHouse - 18)
                                {
                                    Main.tile[67, i].type = 322;
                                    Main.tile[67, i].active(true);
                                    Main.tile[66, i].type = 322;
                                    Main.tile[66, i].active(true);
                                    Main.tile[76, i].type = 322;
                                    Main.tile[76, i].active(true);
                                    Main.tile[77, i].type = 322;
                                    Main.tile[77, i].active(true);
                                    for (int j = 1; j < 7; j++)
                                    {
                                        Main.tile[67, i - j].type = 322;
                                        Main.tile[67, i - j].active(true);
                                        Main.tile[76, i - j].type = 322;
                                        Main.tile[76, i - j].active(true);
                                    }
                                }
                                if (i == shoreHouse - 24)
                                {
                                    Main.tile[64, i].type = 322;
                                    Main.tile[64, i].active(true);
                                    Main.tile[77, i].type = 322;
                                    Main.tile[77, i].active(true);
                                    Main.tile[66, i].type = 322;
                                    Main.tile[66, i].active(true);
                                    Main.tile[78, i].type = 322;
                                    Main.tile[78, i].active(true);
                                    Main.tile[65, i].type = 322;
                                    Main.tile[65, i].active(true);
                                    Main.tile[79, i].type = 322;
                                    Main.tile[79, i].active(true);
                                    WorldGen.PlaceTile(78, i + 1, mod.TileType("海盗船旗帜"));
                                    WorldGen.PlaceTile(65, i + 1, mod.TileType("海盗船旗帜"));
                                }
                                if (i == shoreHouse - 25)
                                {
                                    Main.tile[64, i].type = 322;
                                    Main.tile[64, i].active(true);
                                    Main.tile[77, i].type = 322;
                                    Main.tile[77, i].active(true);
                                    Main.tile[66, i].type = 322;
                                    Main.tile[66, i].active(true);
                                    Main.tile[78, i].type = 322;
                                    Main.tile[78, i].active(true);
                                    Main.tile[65, i].type = 322;
                                    Main.tile[65, i].active(true);
                                    Main.tile[79, i].type = 322;
                                    Main.tile[79, i].active(true);
                                    Main.tile[76, i].wall = 151;
                                    Main.tile[67, i].wall = 151;
                                    Main.tile[82, i + 14].wall = 151;
                                    Main.tile[61, i + 14].wall = 151;
                                }
                                if (i == shoreHouse - 26)
                                {
                                    Main.tile[67, i].type = 322;
                                    Main.tile[67, i].active(true);
                                    Main.tile[76, i].type = 322;
                                    Main.tile[76, i].active(true);
                                    Main.tile[77, i].type = 322;
                                    Main.tile[77, i].active(true);
                                    Main.tile[66, i].type = 322;
                                    Main.tile[66, i].active(true);
                                    Main.tile[64, i].type = 322;
                                    Main.tile[64, i].active(true);
                                    Main.tile[79, i].type = 322;
                                    Main.tile[79, i].active(true);
                                }
                                if (i == shoreHouse - 27)
                                {
                                    Main.tile[75, i].type = 322;
                                    Main.tile[75, i].active(true);
                                    Main.tile[76, i].type = 322;
                                    Main.tile[76, i].active(true);
                                    Main.tile[68, i].type = 322;
                                    Main.tile[68, i].active(true);
                                    Main.tile[67, i].type = 322;
                                    Main.tile[67, i].active(true);
                                    for (int k = i; k < shoreHouse - 1; k++)
                                    {
                                        Main.tile[75, k + 1].wall = 151;
                                        Main.tile[68, k + 1].wall = 151;
                                    }
                                }
                                if (i == shoreHouse - 28)
                                {
                                    Main.tile[75, i].type = 322;
                                    Main.tile[75, i].active(true);
                                    Main.tile[74, i].type = 322;
                                    Main.tile[74, i].active(true);
                                    Main.tile[68, i].type = 322;
                                    Main.tile[68, i].active(true);
                                    Main.tile[69, i].type = 322;
                                    Main.tile[69, i].active(true);
                                    for (int k = i; k < shoreHouse - 1; k++)
                                    {
                                        Main.tile[74, k + 1].wall = 151;
                                        Main.tile[69, k + 1].wall = 151;
                                    }
                                }
                                if (i == shoreHouse - 29)
                                {
                                    Main.tile[73, i].type = 322;
                                    Main.tile[73, i].active(true);
                                    Main.tile[74, i].type = 322;
                                    Main.tile[74, i].active(true);
                                    Main.tile[70, i].type = 322;
                                    Main.tile[70, i].active(true);
                                    Main.tile[69, i].type = 322;
                                    Main.tile[69, i].active(true);
                                    for (int k = i; k < shoreHouse - 1; k++)
                                    {
                                        Main.tile[73, k + 1].wall = 151;
                                        Main.tile[70, k + 1].wall = 151;
                                    }
                                }
                                if (i == shoreHouse - 30)
                                {
                                    Main.tile[73, i].type = 322;
                                    Main.tile[73, i].active(true);
                                    Main.tile[72, i].type = 322;
                                    Main.tile[72, i].active(true);
                                    Main.tile[70, i].type = 322;
                                    Main.tile[70, i].active(true);
                                    Main.tile[71, i].type = 322;
                                    Main.tile[71, i].active(true);
                                    for (int k = i; k < shoreHouse - 1; k++)
                                    {
                                        Main.tile[72, k + 1].wall = 151;
                                        Main.tile[71, k + 1].wall = 151;
                                    }
                                }
                                if (i == shoreHouse - 31)
                                {
                                    Main.tile[72, i].type = 322;
                                    Main.tile[72, i].active(true);
                                    Main.tile[71, i].type = 322;
                                    Main.tile[71, i].active(true);
                                    for (int x = 72; x < 77; x++)
                                    {
                                        for (int y = shoreHouse - 17; y < shoreHouse - 12; y++)
                                        {
                                            Main.tile[x, y].wall = 21;
                                        }
                                    }
                                }
                            }
                        }
                        if (l == 117)//右侧房屋框架
                        {
                            for (int i = shoreHouse - 1; i > shoreHouse - 12; i--)
                            {
                                Main.tile[l, i].type = 322;
                                Main.tile[l, i].active(true);
                            }
                            for (int i = 0; i < 20; i++)
                            {
                                if (117 - i == 108 || 117 - i == 101)
                                {
                                    Main.tile[117 - i, shoreHouse - 12].type = 322;
                                    Main.tile[117 - i, shoreHouse - 12].active(true);
                                }
                                else
                                {
                                    Main.tile[117 - i, shoreHouse - 12].type = 19;
                                    Main.tile[117 - i, shoreHouse - 12].frameY = 17 * 18;
                                    Main.tile[117 - i, shoreHouse - 12].active(true);
                                }
                                if (i > 1)
                                {
                                    for (int j = 0; j < 11; j++)
                                    {
                                        Main.tile[117 - i + 1, shoreHouse - 11 + j].wall = 151;
                                    }
                                }
                                if (i == 19)
                                {
                                    for (int j = 0; j < 8; j++)
                                    {
                                        Main.tile[117 - i, shoreHouse - 11 + j].type = 322;
                                        Main.tile[117 - i, shoreHouse - 11 + j].active(true);
                                    }
                                    WorldGen.PlaceDoor(117 - i, shoreHouse - 2, 10, 29);
                                }
                                if (i == 18)
                                {
                                    for (int j = 0; j < 6; j++)
                                    {
                                        Main.tile[117 - i + j, shoreHouse - 6].type = 19;
                                        Main.tile[117 - i + j, shoreHouse - 6].frameY = 17 * 18;
                                        Main.tile[117 - i + j, shoreHouse - 6].active(true);
                                    }
                                }
                            }
                            WorldGen.Place4x2(113, shoreHouse - 1, 79, 1, 22); //1√//床
                            WorldGen.Place1xX(111, shoreHouse - 1, 93, 18);//2X,1√//地灯
                                                                           //WorldGen.Place3x3(108, shoreHouse, 14, 26);//1X,2X,3X
                            WorldGen.PlaceTile(108, shoreHouse - 1, 14, true, false, -1, 26);//桌子
                            WorldGen.PlaceTile(110, shoreHouse - 1, 15, true, false, -1, 29);//椅子
                            WorldGen.PlaceTile(106, shoreHouse - 1, 15, true, false, -1, 29);//椅子
                            WorldGen.PlaceTile(108, shoreHouse - 3, mod.TileType("鱼子酱寿司"));
                            WorldGen.Place6x4Wall(108, shoreHouse - 6, 242, 25);//地图
                            WorldGen.PlaceTile(108, shoreHouse - 11, 34, true, false, -1, 23);//大吊灯
                            WorldGen.PlaceTile(101, shoreHouse - 11, 42, true, false, -1, 27);//吊灯
                            WorldGen.PlaceTile(102, shoreHouse - 13, 100, true, false, -1, 18);//地灯
                            WorldGen.PlaceTile(115, shoreHouse - 13, 100, true, false, -1, 18);//地灯
                            WorldGen.PlaceTile(104, shoreHouse - 13, 89, true, false, -1, 22);//沙发
                            WorldGen.PlaceTile(107, shoreHouse - 13, 101, true, false, -1, 23);//书架
                            WorldGen.PlaceTile(106, shoreHouse - 18, mod.TileType("鲨鱼骸骨"));//鲨鱼骸骨
                            WorldGen.PlaceTile(109, shoreHouse - 13, 104, true, false, -1, 18);//时钟
                            WorldGen.PlaceTile(112, shoreHouse - 13, 89, true, false, -1, 21);//长凳
                            WorldGen.Place3x3Wall(113, shoreHouse - 8, 240, 46);//9X,8√救生圈
                            WorldGen.Place3x3Wall(115, shoreHouse - 5, (ushort)mod.TileType("大橙色海星"), 0);//海星
                            WorldGen.Place3x3Wall(102, shoreHouse - 4, 240, 47);//9X,8√船舵
                            WorldGen.Place3x3Wall(78, shoreHouse - 6, 240, 49);//9X,8√船舵
                            WorldGen.PlaceTile(100, shoreHouse - 7, 21, false, false, -1, 31);
                            Main.chest[0].item[0].SetDefaults(mod.ItemType("Pearl"), false);
                            Main.chest[0].item[0].stack = Main.rand.Next(40, 60);
                            WorldGen.PlaceTile(99, shoreHouse - 8, mod.TileType("花鹿角珊瑚"));
                            WorldGen.PlaceTile(102, shoreHouse - 8, mod.TileType("鹦鹉螺"));
                            WorldGen.PlaceTile(104, shoreHouse - 9, mod.TileType("大理石芋螺"));
                            WorldGen.PlaceDoor(82, shoreHouse - 2, 10, 29);
                            WorldGen.PlaceTile(104, shoreHouse - 10, 213);
                            WorldGen.PlaceTile(104, shoreHouse - 11, 213);
                            WorldGen.PlaceTile(103, shoreHouse - 9, 213);
                            WorldGen.PlaceTile(103, shoreHouse - 10, 213);
                            WorldGen.PlaceTile(103, shoreHouse - 11, 213);
                            WorldGen.PlaceTile(102, shoreHouse - 10, 213);
                            WorldGen.PlaceTile(102, shoreHouse - 11, 213);
                            WorldGen.PlaceTile(100, shoreHouse - 9, 213);
                            WorldGen.PlaceTile(100, shoreHouse - 10, 213);
                            WorldGen.PlaceTile(100, shoreHouse - 11, 213);
                            WorldGen.PlaceTile(99, shoreHouse - 10, 213);
                            WorldGen.PlaceTile(99, shoreHouse - 11, 213);
                            WorldGen.PlaceTile(105, shoreHouse - 3, 213);
                            WorldGen.PlaceTile(105, shoreHouse - 2, 213);
                            WorldGen.PlaceTile(105, shoreHouse - 4, 213);
                            WorldGen.PlaceTile(105, shoreHouse - 5, 213);
                            WorldGen.PlaceTile(105, shoreHouse - 6, 213);
                            WorldGen.PlaceTile(105, shoreHouse - 7, 213);
                            WorldGen.PlaceTile(105, shoreHouse - 8, 213);
                            WorldGen.PlaceTile(105, shoreHouse - 9, 213);
                            WorldGen.PlaceTile(105, shoreHouse - 10, 213);
                            WorldGen.PlaceTile(105, shoreHouse - 11, 213);
                            WorldGen.PlaceTile(68, shoreHouse + 1, 270);//灯瓶
                            WorldGen.PlaceTile(74, shoreHouse + 1, 271);//灯瓶
                            WorldGen.PlaceTile(113, shoreHouse + 1, 270);//灯瓶
                            WorldGen.PlaceTile(110, shoreHouse + 1, mod.TileType("贝壳风铃"));//风铃
                            WorldGen.PlaceTile(107, shoreHouse + 1, 271);//灯瓶
                            WorldGen.PlaceTile(72, shoreHouse - 1, 376);//箱子
                            WorldGen.PlaceTile(70, shoreHouse - 1, 376);//箱子
                            WorldGen.PlaceTile(74, shoreHouse - 2, 209);//大炮1X2X
                            WorldGen.PlaceTile(90, shoreHouse + 1, mod.TileType("礁鲨标本"));
                        }
                        if ((l < 76 || l > 80) && (l < 100 || l > 104))
                        {
                            if (l < 76)
                            {
                                if (labble && !Main.tile[l, shoreHouse + 20].active() && !Main.tile[l + 1, shoreHouse + 20].active() && !Main.tile[l + 2, shoreHouse + 20].active() && l > 62)
                                {
                                    labble = false;
                                    Main.tile[l, shoreHouse].type = 19;
                                    Main.tile[l, shoreHouse].frameY = 17 * 18;
                                    Main.tile[l, shoreHouse].active(true);
                                    Main.tile[l + 1, shoreHouse].type = 19;
                                    Main.tile[l + 1, shoreHouse].frameY = 17 * 18;
                                    Main.tile[l + 1, shoreHouse].active(true);
                                    Main.tile[l + 2, shoreHouse].type = 19;
                                    Main.tile[l + 2, shoreHouse].frameY = 17 * 18;
                                    Main.tile[l + 2, shoreHouse].active(true);
                                    for (int o = 1; o < 150; o++)
                                    {
                                        if (Main.tile[l, shoreHouse + o].type == 53)
                                        {
                                            break;
                                        }
                                        Main.tile[l, shoreHouse + o].type = 213;
                                        Main.tile[l, shoreHouse + o].active(true);
                                    }
                                    for (int o = 1; o < 150; o++)
                                    {
                                        if (Main.tile[l + 2, shoreHouse + o].type == 53)
                                        {
                                            break;
                                        }
                                        Main.tile[l + 2, shoreHouse + o].type = 213;
                                        Main.tile[l + 2, shoreHouse + o].active(true);
                                    }
                                    for (int o = 1; o < 150; o++)
                                    {
                                        if (Main.tile[l + 1, shoreHouse + o].type == 53)
                                        {
                                            break;
                                        }
                                        if (o % 2 == 1)
                                        {
                                            Main.tile[l + 1, shoreHouse + o].type = 213;
                                            Main.tile[l + 1, shoreHouse + o].active(true);
                                        }
                                    }
                                }
                                else
                                {
                                    if (!Main.tile[l, shoreHouse].active())
                                    {
                                        Main.tile[l, shoreHouse].type = 322;
                                        Main.tile[l, shoreHouse].active(true);
                                    }
                                }
                            }
                            else
                            {
                                Main.tile[l, shoreHouse].type = 322;
                                Main.tile[l, shoreHouse].active(true);
                            }
                        }
                        else
                        {
                            Main.tile[l, shoreHouse].type = 19;
                            Main.tile[l, shoreHouse].frameY = 17 * 18;
                            Main.tile[l, shoreHouse].active(true);
                            if (l == 76)
                            {
                                WorldGen.SlopeTile(l, shoreHouse, 1);
                            }
                            if (l == 104)
                            {
                                WorldGen.SlopeTile(l, shoreHouse, 2);
                            }
                            if (l == 76)
                            {
                                for (int j = l; j < l + 10; j++)
                                {
                                    Main.tile[j, shoreHouse + j - l].type = 19;
                                    Main.tile[j, shoreHouse + j - l].frameY = 17 * 18;
                                    Main.tile[j, shoreHouse + j - l].active(true);
                                    WorldGen.SlopeTile(j, shoreHouse + j - l, 1);
                                }
                                for (int j = 104; j > 94; j--)
                                {
                                    Main.tile[j, shoreHouse + 104 - j].type = 19;
                                    Main.tile[j, shoreHouse + 104 - j].frameY = 17 * 18;
                                    Main.tile[j, shoreHouse + 104 - j].active(true);
                                    WorldGen.SlopeTile(j, shoreHouse + 104 - j, 2);
                                }
                                for (int j = 94; j > 85; j--)
                                {
                                    Main.tile[j, shoreHouse + 10].type = 19;
                                    Main.tile[j, shoreHouse + 10].frameY = 17 * 18;
                                    Main.tile[j, shoreHouse + 10].active(true);
                                }
                                int direc1 = 1;
                                int j2 = 92;
                                for (int z = shoreHouse + 11; z < Main.maxTilesY - 20; z++)
                                {
                                    if (j2 >= 104)
                                    {
                                        direc1 = -1;
                                        if (z > shoreHouse + 15)
                                        {
                                            Main.tile[j2, z].type = 19;
                                            Main.tile[j2, z].frameY = 17 * 18;
                                            Main.tile[j2, z].active(true);
                                        }
                                    }
                                    if (j2 <= 94)
                                    {
                                        direc1 = 1;
                                        Main.tile[j2, z].type = 19;
                                        Main.tile[j2, z].frameY = 17 * 18;
                                        Main.tile[j2, z].active(true);
                                    }
                                    if (Main.tile[j2 + direc1, z + 1].active() && Main.tile[j2 + direc1, z + 1].type != 53)
                                    {
                                        direc1 *= -1;
                                        if (Main.tile[j2, z].type != 53)
                                        {
                                            Main.tile[j2, z].type = 19;
                                            Main.tile[j2, z].frameY = 17 * 18;
                                            Main.tile[j2, z].active(true);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    j2 += direc1;
                                    if (Main.tile[j2, z].type == 53)
                                    {
                                        break;
                                    }
                                    if (!Main.tile[j2, z].active())
                                    {
                                        Main.tile[j2, z].type = 19;
                                        Main.tile[j2, z].frameY = 17 * 18;
                                        Main.tile[j2, z].active(true);
                                        WorldGen.SlopeTile(j2, z, (3 - direc1) / 2);
                                        if (z == shoreHouse + 11)
                                        {
                                            WorldGen.SlopeTile(j2 - direc1, z - 1, (3 - direc1) / 2);
                                        }
                                    }
                                }
                                int j3 = 88;
                                for (int z = shoreHouse + 11; z < Main.maxTilesY - 20; z++)
                                {
                                    if (j3 >= 86)
                                    {
                                        direc1 = -1;
                                        if (z > shoreHouse + 15)
                                        {
                                            Main.tile[j3, z].type = 19;
                                            Main.tile[j3, z].frameY = 17 * 18;
                                            Main.tile[j3, z].active(true);
                                        }
                                    }
                                    if (j3 <= 76)
                                    {
                                        direc1 = 1;
                                        Main.tile[j3, z].type = 19;
                                        Main.tile[j3, z].frameY = 17 * 18;
                                        Main.tile[j3, z].active(true);
                                    }
                                    if (Main.tile[j3 + direc1, z + 1].active() && Main.tile[j3 + direc1, z + 1].type != 53)
                                    {
                                        direc1 *= -1;
                                        if (Main.tile[j3, z].type != 53)
                                        {
                                            Main.tile[j3, z].type = 19;
                                            Main.tile[j3, z].frameY = 17 * 18;
                                            Main.tile[j3, z].active(true);
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                    j3 += direc1;
                                    if (Main.tile[j3, z].type == 53)
                                    {
                                        break;
                                    }
                                    if (!Main.tile[j3, z].active())
                                    {
                                        Main.tile[j3, z].type = 19;
                                        Main.tile[j3, z].frameY = 17 * 18;
                                        Main.tile[j3, z].active(true);
                                        WorldGen.SlopeTile(j3, z, (3 - direc1) / 2);
                                        if (z == shoreHouse + 11)
                                        {
                                            WorldGen.SlopeTile(j3 - direc1, z - 1, (3 - direc1) / 2);
                                        }
                                    }
                                }
                            }
                        }
                        if (l % 10 == 5)
                        {
                            for (int m = shoreHouse; m < shoreHouse + 48; m++)
                            {
                                Main.tile[l, m].wall = 151;
                                if(m - 1 > shoreHouse + 18 && m - 1 < shoreHouse + 22 && !Main.tile[l, m - 1].active())
                                {
                                    Main.tile[l, m - 1].type = (ushort)mod.TileType("稀疏藤壶" + (Main.rand.Next(3) + 1).ToString());
                                    Main.tile[l, m - 1].active(true);
                                }
                                if (m - 1 > shoreHouse + 22 && !Main.tile[l, m - 1].active())
                                {
                                    Main.tile[l, m - 1].type = (ushort)mod.TileType("藤壶" + (Main.rand.Next(3) + 1).ToString());
                                    Main.tile[l, m - 1].active(true);
                                }
                            }
                        }
                        for (int k = 60; k < 120; k++)
                        {
                            for (int m = shoreHouse - 30; m < shoreHouse + 50; m++)
                            {
                                if (Main.tile[k, m].type == 19 || Main.tile[k, m].type == 322)
                                {
                                    //WorldGen.PoundPlatform(k, m);
                                    //WorldGen.PoundPlatform(k, m);
                                    //WorldGen.PoundPlatform(k, m);
                                    //Main.tile[k, m].frameNumber(2);
                                }
                            }
                        }
                    }
                    WorldGen.PlaceTile(70, shoreHouse - 20, mod.TileType("炸药桶"), false, false, -1, 0);//24X
                    WorldGen.PlaceTile(71, shoreHouse - 20, mod.TileType("荧光虾酱"), false, false, -1, 0);//24X
                    WorldGen.PlaceTile(73, shoreHouse - 20, 318, false, false, -1, 0);//24X
                    WorldGen.PlaceTile(75, shoreHouse - 23, mod.TileType("唐冠螺"), false, false, -1, 0);//26X
                    WorldGen.PlaceTile(76, shoreHouse - 11, 185, false, false, -1, 4);
                    WorldGen.PlaceTile(93, shoreHouse - 1, 92);//1X3X4X2X
                    WorldGen.PlaceTile(86, shoreHouse - 1, 92);//1X3X4X2X
                    WorldGen.PlaceTile(71, shoreHouse - 4, 376);//箱子3X
                    WorldGen.PlaceTile(65, shoreHouse - 11, mod.TileType("红酒"), false, false, -1, 0);//1X2X0X
                    for (int i = 60; i < 82; i++)
                    {
                        for (int j = shoreHouse; j > shoreHouse - 32; j--)
                        {
                            if (Main.tile[i, j].wall == 151)
                            {
                                if (!Main.tile[i, j].active() && Main.rand.Next(2) == 1)
                                {
                                    Main.tile[i, j].type = 51;
                                    Main.tile[i, j].active(true);
                                }
                            }
                        }
                    }
                }
                if (jindu == 694)//火山熔岩石
                {
                    string text0 = "让火山熔化…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    for (int k = 0; k < Main.maxTilesX / 25; k++)
                    {
                        int i = (int)(Main.maxTilesX * 0.75f) + Main.rand.Next(-(Main.maxTilesX / 9) - 29, Main.maxTilesX / 9 - 29);
                        int j = Main.rand.Next(0, Main.maxTilesY - 50);
                        if (Main.tile[i, j].type == (ushort)mod.TileType("Basalt"))
                        {
                            int FTimes = Main.rand.Next(15, 90);
                            float DeltaH = 0;
                            float r = 2;
                            for (int u = 0; u < FTimes; u++)
                            {
                                DeltaH += Main.rand.NextFloat(-0.35f, 0.35f);
                                if (u < FTimes / 3 && r < 14)
                                {
                                    r += Main.rand.NextFloat(-0.15f, 0.5f);
                                }
                                if (r >= 12)
                                {
                                    r += Main.rand.NextFloat(-0.5f, 0.1f);
                                }
                                if (u > FTimes / 3 * 2 && r > 0.25f)
                                {
                                    r += Main.rand.NextFloat(-0.5f, 0.1f);
                                }
                                if (r <= 1)
                                {
                                    Main.tile[i, j].type = (ushort)mod.TileType("MeltingLava");
                                }
                                else
                                {
                                    for (int v = -(int)(r / 2f); v < (int)(r / 2f); v++)
                                    {
                                        for (int w = -(int)(r / 2f); w < (int)(r / 2f); w++)
                                        {
                                            if (Main.tile[i + v + u, (int)(j + w + DeltaH)].type == (ushort)mod.TileType("Basalt"))
                                            {
                                                Main.tile[i + v + u, (int)(j + w + DeltaH)].type = (ushort)mod.TileType("MeltingLava");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    for (int k = 0; k < Main.maxTilesX / 15; k++)
                    {
                        int i = (int)(Main.maxTilesX * 0.75f) + Main.rand.Next(-(Main.maxTilesX / 9) - 29, Main.maxTilesX / 9 - 29);
                        int j = Main.rand.Next(0, Main.maxTilesY - 50);
                        if (Main.tile[i, j].type == (ushort)mod.TileType("Basalt"))
                        {
                            int FTimes = Main.rand.Next(15, 45);
                            float DeltaH = 0;
                            float r = 2;
                            for (int u = 0; u < FTimes; u++)
                            {
                                DeltaH += Main.rand.NextFloat(-0.35f, 0.35f);
                                if (u < FTimes / 3 && r < 7)
                                {
                                    r += Main.rand.NextFloat(-0.15f, 0.5f);
                                }
                                if (r >= 6)
                                {
                                    r += Main.rand.NextFloat(-0.5f, 0.1f);
                                }
                                if (u > FTimes / 3 * 2 && r > 0.25f)
                                {
                                    r += Main.rand.NextFloat(-0.5f, 0.1f);
                                }
                                if (r <= 1)
                                {
                                    Main.tile[i, j].type = (ushort)mod.TileType("MeltingLava");
                                }
                                else
                                {
                                    for (int v = -(int)(r / 2f); v < (int)(r / 2f); v++)
                                    {
                                        for (int w = -(int)(r / 2f); w < (int)(r / 2f); w++)
                                        {
                                            if (Main.tile[i + v + u, (int)(j + w + DeltaH)].type == (ushort)mod.TileType("Basalt"))
                                            {
                                                Main.tile[i + v + u, (int)(j + w + DeltaH)].type = (ushort)mod.TileType("MeltingLava");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (jindu == 695)//沉船
                {
                    string text0 = "一场风暴掀翻了一艘船…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    int Xd = 2000;
                    int Yd = 455;
                    if (Main.maxTilesX == 6400)
                    {
                        Xd = 3000;
                        Yd = 920;
                    }
                    if (Main.maxTilesX == 8400)
                    {
                        Xd = 4000;
                        Yd = 910;
                    }

                    int m = 0;
                    Texture2D tex = mod.GetTexture("UIImages/Ocean/沉船");
                    Color[] colorTex = new Color[tex.Width * tex.Height];
                    tex.GetData(colorTex);

                    for (int y = 0; y < tex.Height; y += 1)
                    {
                        for (int x = 0; x < tex.Width; x += 1)
                        {
                            if (new Color(colorTex[x + y * tex.Width].R, colorTex[x + y * tex.Width].G, colorTex[x + y * tex.Width].B) == new Color(89, 43, 27))
                            {
                                //WorldGen.PlaceTile(x + 2000, y + 100, mod.TileType("朽木"));
                                Main.tile[x + Xd, y + Yd].type = (ushort)mod.TileType("朽木");
                                Main.tile[x + Xd, y + Yd].active(true);
                            }
                            if (new Color(colorTex[x + y * tex.Width].R, colorTex[x + y * tex.Width].G, colorTex[x + y * tex.Width].B) == new Color(37, 25, 16))
                            {
                                Main.tile[x + Xd, y + Yd].wall = (ushort)mod.WallType("朽木墙");
                                Main.tile[x + Xd, y + Yd].active(false);
                            }
                        }
                    }
                    Texture2D tex2 = mod.GetTexture("UIImages/Ocean/沉船A");
                    Color[] colorTex2 = new Color[tex2.Width * tex2.Height];
                    tex2.GetData(colorTex2);
                    for (int y = 0; y < tex2.Height; y += 1)
                    {
                        for (int x = 0; x < tex2.Width; x += 1)
                        {
                            if (new Color(colorTex2[x + y * tex2.Width].R, colorTex2[x + y * tex2.Width].G, colorTex2[x + y * tex2.Width].B) == new Color(89, 43, 27))
                            {
                                //WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("朽木"));
                                Main.tile[x + Xd, y + Yd].type = (ushort)mod.TileType("朽木");
                                Main.tile[x + Xd, y + Yd].active(true);
                            }
                        }
                    }
                    for (int y = 0; y < tex2.Height; y += 1)
                    {
                        for (int x = 0; x < tex2.Width; x += 1)
                        {
                            Color color = new Color(colorTex2[x + y * tex2.Width].R, colorTex2[x + y * tex2.Width].G, colorTex2[x + y * tex2.Width].B);
                            if (color == new Color(6, 6, 7))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("海盗船旗帜"));
                                short num = (short)(Main.rand.Next(0, 4));
                                Main.tile[x + Xd, y + Yd].frameX = (short)(num * 72);
                                Main.tile[x + Xd, y + Yd + 1].frameX = (short)(num * 72);
                                Main.tile[x + Xd, y + Yd + 2].frameX = (short)(num * 72);
                                Main.tile[x + Xd, y + Yd + 3].frameX = (short)(num * 72);
                                Main.tile[x + Xd, y + Yd + 4].frameX = (short)(num * 72);
                            }
                            if (color == new Color(216, 104, 67))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("CorruptDoorClosed"));
                            }
                            if (color == new Color(226, 109, 70))
                            {
                                Main.tile[x + Xd, y + Yd].type = (ushort)mod.TileType("CorruptPlatform");
                                Main.tile[x + Xd, y + Yd].active(true);
                                WorldGen.SlopeTile(x + Xd, y + Yd, 2);
                            }
                            if (color == new Color(226, 109, 140))
                            {
                                Main.tile[x + Xd, y + Yd].type = (ushort)mod.TileType("CorruptPlatform");
                                Main.tile[x + Xd, y + Yd].active(true);
                                WorldGen.SlopeTile(x + Xd, y + Yd, 1);
                            }
                            if (color == new Color(226, 109, 105))
                            {
                                Main.tile[x + Xd, y + Yd].type = (ushort)mod.TileType("CorruptPlatform");
                                Main.tile[x + Xd, y + Yd].active(true);
                            }
                            if (color == new Color(14, 37, 38))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("藻华"), false, true, -1, 0);
                            }
                            if (color == new Color(216, 18, 0))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("CorruptWoodChandelier"), false, true, -1, 0);
                                Main.tile[x + Xd, y + Yd].frameX += 54;
                                Main.tile[x + Xd, y + Yd + 1].frameX += 54;
                                Main.tile[x + Xd, y + Yd + 2].frameX += 54;
                                Main.tile[x + Xd + 1, y + Yd].frameX += 54;
                                Main.tile[x + Xd + 1, y + Yd + 1].frameX += 54;
                                Main.tile[x + Xd + 1, y + Yd + 2].frameX += 54;
                                Main.tile[x + Xd - 1, y + Yd].frameX += 54;
                                Main.tile[x + Xd - 1, y + Yd + 1].frameX += 54;
                                Main.tile[x + Xd - 1, y + Yd + 2].frameX += 54;
                            }
                            if (color == new Color(216, 20, 0))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("CorruptWoodChair"), false, true, -1, 0);
                                Main.tile[x + Xd, y + Yd].frameX = 18;
                                Main.tile[x + Xd, y + Yd - 1].frameX = 18;
                            }
                            if (color == new Color(216, 22, 0))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("CorruptWoodChair"), false, true, -1, 0);
                            }
                            if (color == new Color(216, 24, 0))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("CorruptWoodTable"), false, true, -1, 0);
                            }
                            if (color == new Color(216, 26, 0))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("CorruptWoodLamp"), false, true, -1, 0);
                                Main.tile[x + Xd, y + Yd].frameX = 18;
                                Main.tile[x + Xd, y + Yd - 1].frameX = 18;
                                Main.tile[x + Xd, y + Yd - 2].frameX = 18;
                            }
                            if (color == new Color(255, 148, 0))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("大橙色海星"), false, true, -1, 0);
                            }
                            if (color == new Color(145, 176, 255))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("大蓝色海星"), false, true, -1, 0);
                            }
                            if (color == new Color(109, 83, 50))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, 245, false, true, -1, 4);
                            }
                            if (color == new Color(159, 111, 88))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, 242, false, true, -1, 26);
                            }
                            if (color == new Color(95, 95, 95))
                            {
                                WorldGen.PlaceTile(x + Xd, y + Yd, mod.TileType("CorruptWoodChest"), false, false, -1, 0);
                            }
                        }
                    }
                }
                if(jindu == 695)//正在生成水晶洞穴
                {
                    for (int ia = 0; ia < 62; ia++)
                    {
                        int X = Main.rand.Next((int)(Main.maxTilesX * 0.82f), (int)(Main.maxTilesX * 0.86f));
                        int Y = Main.rand.Next((int)(Main.maxTilesY * 0.36f), (int)(Main.maxTilesY * 0.54f));
                        int num = WorldGen.genRand.Next(2);
                        if (num == 0)
                        {
                            int num2 = WorldGen.genRand.Next(7, 9);
                            float num3 = (float)WorldGen.genRand.Next(100) * 0.01f;
                            float num4 = 1f - num3;
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                num3 = -num3;
                            }
                            if (WorldGen.genRand.Next(2) == 0)
                            {
                                num4 = -num4;
                            }
                            Vector2 vector4 = new Vector2((float)X, (float)Y);
                            for (int i = 0; i < num2; i++)
                            {
                                vector4 = WorldGen.digTunnel(vector4.X, vector4.Y, num3, num4, WorldGen.genRand.Next(6, 20), WorldGen.genRand.Next(4, 9), false);
                                num3 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
                                num4 += (float)WorldGen.genRand.Next(-20, 21) * 0.1f;
                                if ((double)num3 < -1.5)
                                {
                                    num3 = -1.5f;
                                }
                                if ((double)num3 > 1.5)
                                {
                                    num3 = 1.5f;
                                }
                                if ((double)num4 < -1.5)
                                {
                                    num4 = -1.5f;
                                }
                                if ((double)num4 > 1.5)
                                {
                                    num4 = 1.5f;
                                }
                                float num5 = (float)WorldGen.genRand.Next(100) * 0.01f;
                                float num6 = 1f - num5;
                                if (WorldGen.genRand.Next(2) == 0)
                                {
                                    num5 = -num5;
                                }
                                if (WorldGen.genRand.Next(2) == 0)
                                {
                                    num6 = -num6;
                                }
                                int Size = WorldGen.genRand.Next(3, 6);
                                int Steps = WorldGen.genRand.Next(30, 50);
                                double strength = (double)WorldGen.genRand.Next(10, 20);
                                int steps = WorldGen.genRand.Next(5, 10);
                                Vector2 vector2 = WorldGen.digTunnel(vector4.X, vector4.Y, num5, num6, Steps, Size, false);
                                WorldGen.TileRunner((int)vector2.X, (int)vector2.Y, strength + 2, steps, mod.TileType("Crystal"), false, 0f, 0f, false, true);
                                WorldGen.TileRunner((int)vector2.X, (int)vector2.Y, strength - 2, steps, mod.TileType("Crystal2"), false, 0f, 0f, false, true);
                            }
                        }
                    }
                    for (int i = (int)(Main.maxTilesX * 0.80f); i < (int)(Main.maxTilesX * 0.88f); i++)
                    {
                        for (int j = (int)(Main.maxTilesY * 0.13f); j < (int)(Main.maxTilesY * 0.58f); j++)
                        {
                            if (Main.tile[i, j].type == mod.TileType("Crystal2"))
                            {
                                Main.tile[i, j].wall = (ushort)mod.WallType("CrystalWall");
                                Main.tile[i, j].active(false);
                            }
                        }
                    }
                    //string text0 = "生成水晶洞穴…";
                    //int Sscale = 3;
                    //Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    //int Xe = (int)(Main.maxTilesX * 0.87f);
                    //int Ye = (int)(Main.maxTilesY * 0.37f + (jindu - 695) * 40);
                    //for (int kp = 0; kp < 4; kp++)
                    //{
                    //    int Xef = Xe + Main.rand.Next(-40, 40);
                    //    int lengt = Main.rand.Next(22, 88);
                    //    int Hid = 0;
                    //    int HidMax = Main.rand.Next(7, 24);
                    //    for (int i = 0; i < lengt; i++)
                    //    {
                    //        for (int j = -Hid; j < Hid + 1; j++)
                    //        {
                    //            if (j == -Hid)
                    //            {
                    //                Main.tile[Xef + i, Ye + j].type = (ushort)mod.TileType("Crystal");
                    //                Main.tile[Xef + i, Ye + j].active(true);
                    //            }
                    //            if (j == -Hid + 1)
                    //            {
                    //                Main.tile[Xef + i, Ye + j].type = (ushort)mod.TileType("Crystal");
                    //                Main.tile[Xef + i, Ye + j].active(true);
                    //            }
                    //            if (j == -Hid + 2)
                    //            {
                    //                Main.tile[Xef + i, Ye + j].type = (ushort)mod.TileType("Crystal");
                    //                Main.tile[Xef + i, Ye + j].active(true);
                    //            }
                    //            if (j == Hid)
                    //            {
                    //                Main.tile[Xef + i, Ye + j].type = (ushort)mod.TileType("Crystal");
                    //                Main.tile[Xef + i, Ye + j].active(true);
                    //            }
                    //            if (j == Hid - 1)
                    //            {
                    //                Main.tile[Xef + i, Ye + j].type = (ushort)mod.TileType("Crystal");
                    //                Main.tile[Xef + i, Ye + j].active(true);
                    //            }
                    //            if (j == Hid - 2)
                    //            {
                    //                Main.tile[Xef + i, Ye + j].type = (ushort)mod.TileType("Crystal");
                    //                Main.tile[Xef + i, Ye + j].active(true);
                    //            }
                    //            if (Math.Abs(j) <= Hid - 2)
                    //            {
                    //                if (Main.rand.Next(35) != 0)
                    //                {
                    //                    Main.tile[Xef + i, Ye + j].active(false);
                    //                    Main.tile[Xef + i, Ye + j].wall = (ushort)mod.WallType("CrystalWall");
                    //                }
                    //                else
                    //                {
                    //                    Main.tile[Xef + i, Ye + j].active(false);
                    //                    Main.tile[Xef + i, Ye + j].wall = (ushort)mod.WallType("CrystalWall");
                    //                }
                    //            }
                    //        }
                    //        if (Hid < HidMax && i < lengt - HidMax)
                    //        {
                    //            Hid++;
                    //        }
                    //        if (i >= lengt - HidMax && Hid > 0)
                    //        {
                    //            Hid--;
                    //        }
                    //    }
                    //    Ye += (int)(Hid * 2.5f + Main.rand.Next(-3, 30));
                    //}
                }
                if (jindu >= 900 && jindu < 950)//正在令世界平坦
                {
                    string text0 = "正在令世界平坦…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    float m = (Main.maxTilesX - 40) / 50f;
                    for (int k = (int)(20 + m * (jindu - 900)); k < 20 + (m) * (jindu - 899); k++)
                    {
                        float value = (float)k / (float)Main.maxTilesX;
                        for (int l = 20; l < Main.maxTilesY - 20; l++)
                        {
                            //WorldGen.PoundTile(k, l);
                            if (Main.tile[k, l].type != 322)
                            {
                                Tile.SmoothSlope(k, l, false);
                            }
                            WorldGen.TileFrame(k, l, true, false);
                            WorldGen.SquareWallFrame(k, l, true);
                        }
                    }
                }
                if (jindu == 950)//最后设置
                {
                    string text0 = "等待开始游戏…";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    Player localPlayer = Main.LocalPlayer;
                    Tile tile = Main.tile[(int)(localPlayer.Center.X / 16f), (int)(localPlayer.Center.Y / 16f)];
                    int num = (int)(localPlayer.Center.X / 16f);
                    int num2 = (int)(localPlayer.Center.Y / 16f);
                    localPlayer.FindSpawn();
                    if (localPlayer.SpawnX == num && localPlayer.SpawnY == num2)
                    {
                        localPlayer.RemoveSpawn();
                        return;
                    }
                    if (Player.CheckSpawn(num, num2))
                    {
                        localPlayer.ChangeSpawn(num, num2);
                    }
                }
                if (jindu >= 1000)//结束任务
                {
                    string text0 = "我们开始吧！";
                    int Sscale = 3;
                    Utils.DrawBorderStringFourWay(Main.spriteBatch, Main.fontMouseText, text0, shopx + 246 - text0.Length * Sscale, shopy + 180, Color.White, Color.Black, Vector2.Zero, Sscale);
                    OceanContinent.Open = false;
                    player.noFallDmg = false;
                }
            }
        }
    }
}*/