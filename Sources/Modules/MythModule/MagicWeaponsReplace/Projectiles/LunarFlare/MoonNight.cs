using Everglow.Sources.Commons.Function.ObjectPool;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Enumerator;
using IL.Terraria.Graphics;
using Microsoft.Xna.Framework.Graphics;
using MonoMod.Cil;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.GameContent.Liquid;
using Terraria.Graphics.Effects;
using Terraria.Graphics.Light;
using Terraria.ModLoader;
using Terraria.Utilities;
using static Humanizer.On;

namespace Everglow.Sources.Modules.MythModule.MagicWeaponsReplace.Projectiles.LunarFlare
{
    internal class MoonNight : ModSystem
    {
        internal static int Timer
        {
            get => timer;
            set
            {
                called = true;
                timer = value;
            }
        }
        static int timer = -1;
        static bool called;
        Texture2D MoonTexture;
        internal static List<MoonNightStar> stars = new();
        Effect lerpeffect;
        public override void Load()
        {
            On.Terraria.Main.DrawBG += Main_DrawBG;
            On.Terraria.Main.DoDraw_Tiles_Solid += Main_DoDraw_Tiles_Solid;
            On.Terraria.Main.DoDraw_Tiles_NonSolid += Main_DoDraw_Tiles_NonSolid;
            On.Terraria.Main.DoDraw_WallsAndBlacks += Main_DoDraw_WallsAndBlacks;
            On.Terraria.GameContent.Drawing.TileDrawing.DrawMultiTileVines += TileDrawing_DrawMultiTileVines;
            lerpeffect = ModContent.Request<Effect>("Everglow/Sources/Modules/MythModule/Effects/TextureLerp", AssetRequestMode.ImmediateLoad).Value;
        }
        public override void Unload()
        {
            On.Terraria.Main.DrawBG -= Main_DrawBG;
            On.Terraria.Main.DoDraw_Tiles_Solid -= Main_DoDraw_Tiles_Solid;
            On.Terraria.Main.DoDraw_Tiles_NonSolid -= Main_DoDraw_Tiles_NonSolid;
            On.Terraria.Main.DoDraw_WallsAndBlacks -= Main_DoDraw_WallsAndBlacks;
            On.Terraria.GameContent.Drawing.TileDrawing.DrawMultiTileVines -= TileDrawing_DrawMultiTileVines;
        }
        public override void PostUpdateEverything()
        {
            if (!called)
            {
                if (timer > 0)
                {
                    timer--;
                }
                else
                {
                    timer = -1;
                }
            }
            called = false;
        }
        internal static void GenerateStars(int count)
        {
            var pool = PrepareStarPoint();
            for (int i = 0; i < count; i++)
            {
                stars.Add(new MoonNightStar(pool.Get()));
            }
        }
        private void Main_DrawBG(On.Terraria.Main.orig_DrawBG orig, Main self)
        {
            orig(self);
            if (Main.gameMenu)
            {
                timer = -1;
            }
            if (timer == -1)
            {
                return;
            }
            if (timer < 0)
            {
                return;
            }
            if (timer > 600)
            {
                timer = 600;
            }
            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value,
                Vector2.Zero,
                new Rectangle(0, 0, 1, 1),
                Color.Lerp(Color.Transparent, Color.Black, timer / 600f),
                0,
                Vector2.Zero,
                new Vector2(Main.screenWidth, Main.screenHeight),
                SpriteEffects.None,
                0);
            MoonTexture ??= ModContent.Request<Texture2D>((GetType().Namespace + "/Moon").Replace(".", "/"), AssetRequestMode.ImmediateLoad).Value;
            Main.spriteBatch.Draw(MoonTexture,
                new Vector2(Main.screenWidth / 2, 0) - Vector2.Lerp(Vector2.UnitY * MoonTexture.Height / 2, Vector2.Zero, timer / 600f),
                null,
                Color.White * (timer / 600f),
                Main.GameUpdateCount % 6000 / 6000f * MathHelper.TwoPi,
                MoonTexture.Size() / 2,
                1,
                SpriteEffects.None,
                0);
            foreach (var star in stars)
            {
                star.Update();
                star.Draw();
            }
            stars.RemoveAll(star => !star.active);
        }
        /*
        #region
        private void Main_DrawTileInWater(On.Terraria.Main.orig_DrawTileInWater orig, Vector2 drawOffset, int x, int y)
        {
            if (Main.tile[x, y] != null && Main.tile[x, y].HasTile && Main.tile[x, y].TileType == 518)
            {
                Main.instance.LoadTiles(Main.tile[x, y].TileType);
                Tile tile = Main.tile[x, y];
                int num = (int)(tile.LiquidAmount / 16);
                num -= 3;
                if (WorldGen.SolidTile(x, y - 1, false) && num > 8)
                {
                    num = 8;
                }
                Rectangle value = new Rectangle(tile.TileFrameX, tile.WallFrameY, 16, 16);
                if (Main.drawToScreen && timer != -1)
                {
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                    lerpeffect.Parameters["lerp"].SetValue(timer / 750f);
                    lerpeffect.Parameters["lerptarget"].SetValue(Asset<Texture2D>.DefaultValue);
                    lerpeffect.CurrentTechnique.Passes[0].Apply();
                    Main.spriteBatch.Draw(TextureAssets.Tile[tile.TileType].Value, new Vector2((float)(x * 16), (float)(y * 16 - num)) + drawOffset, new Rectangle?(value), Lighting.GetColor(x, y), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                    Main.spriteBatch.End();
                    Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                }
                else
                {
                    Main.spriteBatch.Draw(TextureAssets.Tile[tile.TileType].Value, new Vector2((float)(x * 16), (float)(y * 16 - num)) + drawOffset, new Rectangle?(value), Lighting.GetColor(x, y), 0f, default(Vector2), 1f, SpriteEffects.None, 0f);
                }
            }
        }
        private void LiquidRenderer_InternalDraw(ILContext il)
        {
            ILCursor cursor = new(il);
            if (cursor.TryGotoNext(i => i.MatchCallvirt(typeof(TileBatch), nameof(TileBatch.Begin))))
            {
                cursor.EmitDelegate(() =>
                {
                    if (timer != -1)
                    {
                        Main.tileBatch.End();
                        Main.tileBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, lerpeffect, Matrix.Identity);
                        lerpeffect.Parameters["lerp"].SetValue(timer / 750f);
                        lerpeffect.Parameters["lerptarget"].SetValue(Asset<Texture2D>.DefaultValue);
                        lerpeffect.CurrentTechnique.Passes[0].Apply();
                    }
                });
            }
        }
        #endregion
        */
        private void TileDrawing_DrawMultiTileVines(On.Terraria.GameContent.Drawing.TileDrawing.orig_DrawMultiTileVines orig, Terraria.GameContent.Drawing.TileDrawing self)
        {
            if (timer != -1)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                lerpeffect.Parameters["lerp"].SetValue(timer / 750f);
                lerpeffect.Parameters["lerptarget"].SetValue(Asset<Texture2D>.DefaultValue);
                lerpeffect.CurrentTechnique.Passes[0].Apply();
            }
            orig(self);
        }
        private void Main_DoDraw_WallsAndBlacks(On.Terraria.Main.orig_DoDraw_WallsAndBlacks orig, Main self)
        {
            if (timer == -1)
            {
                orig(self);
                return;
            }
            if (!Main.drawToScreen)
            {
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                lerpeffect.Parameters["lerp"].SetValue(timer / 750f);
                lerpeffect.Parameters["lerptarget"].SetValue(Asset<Texture2D>.DefaultValue);
                lerpeffect.CurrentTechnique.Passes[0].Apply();
                Main.spriteBatch.Draw(self.blackTarget, Main.sceneTilePos - Main.screenPosition, Color.White);
                TimeLogger.DetailedDrawTime(13);
                Main.spriteBatch.Draw(self.wallTarget, Main.sceneWallPos - Main.screenPosition, Color.White);
                TimeLogger.DetailedDrawTime(14);
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                Overlays.Scene.Draw(Main.spriteBatch, RenderLayers.Walls);
            }
            else
            {
                orig(self);
            }
        }
        private void Main_DoDraw_Tiles_NonSolid(On.Terraria.Main.orig_DoDraw_Tiles_NonSolid orig, Main self)
        {
            if (timer == -1)
            {
                orig(self);
                return;
            }
            if (!Main.drawToScreen)
            {
                self.TilesRenderer.PreDrawTiles(solidLayer: false, !Main.drawToScreen, intoRenderTargets: false);

                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                lerpeffect.Parameters["lerp"].SetValue(timer / 750f);
                lerpeffect.Parameters["lerptarget"].SetValue(Asset<Texture2D>.DefaultValue);
                lerpeffect.CurrentTechnique.Passes[0].Apply();
                Main.spriteBatch.Draw(self.tile2Target, Main.sceneTile2Pos - Main.screenPosition, Color.White);
                TimeLogger.DetailedDrawTime(15);
                Main.spriteBatch.End();
                try
                {
                    self.TilesRenderer.PostDrawTiles(solidLayer: false, !Main.drawToScreen, intoRenderTargets: false);
                }
                catch (Exception e)
                {
                    TimeLogger.DrawException(e);
                }

                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
            }
            else
            {
                orig(self);
            }
        }
        private void Main_DoDraw_Tiles_Solid(On.Terraria.Main.orig_DoDraw_Tiles_Solid orig, Main self)
        {
            if (timer == -1)
            {
                orig(self);
                return;
            }
            if (!Main.drawToScreen)
            {
                self.TilesRenderer.PreDrawTiles(solidLayer: true, !Main.drawToScreen, intoRenderTargets: false);
                try
                {
                    if (Main.drawToScreen)
                    {
                        //self.DrawTiles(solidLayer: true, !Main.drawToScreen, intoRenderTargets: false);
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                        self.TilesRenderer.Draw(true, !Main.drawToScreen, false);
                    }
                    else
                    {
                        Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                        lerpeffect.Parameters["lerp"].SetValue(timer / 750f);
                        lerpeffect.Parameters["lerptarget"].SetValue(Asset<Texture2D>.DefaultValue);
                        lerpeffect.CurrentTechnique.Passes[0].Apply();
                        Main.spriteBatch.Draw(self.tileTarget, Main.sceneTilePos - Main.screenPosition, Color.White);
                        Main.spriteBatch.End();
                        Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                        TimeLogger.DetailedDrawTime(17);
                    }
                }
                catch (Exception e)
                {
                    TimeLogger.DrawException(e);
                }
                Main.spriteBatch.End();
                self.TilesRenderer.PostDrawTiles(solidLayer: true, !Main.drawToScreen, intoRenderTargets: false);
                Main.spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, Main.Transform);
                try
                {
                    Main.player[Main.myPlayer].hitReplace.DrawFreshAnimations(Main.spriteBatch);
                    Main.player[Main.myPlayer].hitTile.DrawFreshAnimations(Main.spriteBatch);
                }
                catch (Exception e2)
                {
                    TimeLogger.DrawException(e2);
                }
                Main.spriteBatch.End();
            }
            else
            {
                orig(self);
            }
        }
        private static WeightedRandom<Point> PrepareStarPoint()
        {
            WeightedRandom<Point> pool = new();
            int startx = (int)(Main.screenPosition.X / 16);
            int endx = (int)((Main.screenPosition.X + Main.screenWidth) / 16 + 1);
            int starty = (int)(Main.screenPosition.Y / 16);
            int endy = (int)((Main.screenPosition.Y + Main.screenHeight) / 16 + 1);
            for (int i = startx; i <= endx; i++)
            {
                for (int j = starty; j <= endy; j++)
                {
                    if (WorldGen.InWorld(i, j))
                    {
                        Tile tile = Main.tile[i, j];
                        if (tile.HasTile)
                        {
                            if (Main.tileSolid[tile.TileType])
                            {
                                pool.Add(new Point(i, j), 0.6);
                            }
                            else
                            {
                                pool.Add(new Point(i, j), 0.3);
                            }
                        }
                        else
                        {
                            pool.Add(new Point(i, j), Math.Pow(1 + Math.Pow(Math.E, 1 - (j - starty) / (endy - starty)), 6));
                        }
                    }
                }
            }
            return pool;
        }
        internal class MoonNightStar
        {
            static Texture2D Star;
            Vector2 center;
            float scale, scalemax, rotation;
            bool bigger = true;
            internal bool active = true;
            int timeleft = 0;
            int attacktimer;
            public MoonNightStar(Point point)
            {
                center = point.ToVector2() * 16 + Main.rand.NextVector2Circular(8, 8);
                scalemax = Main.rand.NextFloat(0.8f, 3.2f);
                rotation = Main.rand.NextFloat(MathHelper.TwoPi);
                Star ??= ModContent.Request<Texture2D>((GetType().Namespace + "/Star").Replace(".", "/"), AssetRequestMode.ImmediateLoad).Value;
            }
            public void Draw()
            {
                Main.spriteBatch.Draw(Star,
                    center - Main.screenPosition,
                    null,
                    Color.White * (scale / scalemax),
                    rotation,
                    Star.Size() / 2,
                    scale,
                    SpriteEffects.None,
                    0);
            }
            public void Update()
            {
                if (Main.gameMenu || Main.gamePaused)
                {
                    return;
                }
                timeleft++;
                if (attacktimer > 0)
                {
                    attacktimer--;
                }
                if (bigger)
                {
                    scale = MathHelper.Lerp(0, scalemax, timeleft / 60f);
                    if (timeleft == 60)
                    {
                        bigger = false;
                    }
                }
                else
                {
                    scale = MathHelper.Lerp(0, scalemax, (120 - timeleft) / 60f);
                }
                if (scale == 0 && !bigger)
                {
                    active = false;
                }
            }
        }
    }
}
