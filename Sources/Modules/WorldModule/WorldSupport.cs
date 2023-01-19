using Everglow.Sources.Commons.Core.DataStructures;
using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Function.NetUtils;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Draw;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using ReLogic.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent;
using Terraria.GameInput;
using Terraria.Graphics.Capture;
using Terraria.IO;
using Terraria.Localization;
using Terraria.ID;
using Terraria.Map;
using Terraria.Audio;
using static System.Net.Mime.MediaTypeNames;
using Terraria.UI.Chat;
using MonoMod.Cil;
using Mono.Cecil.Cil;
using Terraria.Utilities;
using Terraria.Graphics.Light;
using Terraria.Social;
using System.ComponentModel;
using Everglow.Sources.Modules.WorldModule;

namespace Everglow.Sources.Modules.WorldModule
{
    internal class WorldSupport : IModule
    {
        static Dictionary<Type, Dictionary<string, object>> cache;
        public string Name => nameof(WorldModule);
        public void Load()
        {
            cache = new();
            try
            {
                Hooks.Load();
            }
            catch (Exception e)
            {
                Hooks.Unload();
                Everglow.Instance.Logger.Debug(e);
                throw;
            }
        }
        public void Unload()
        {
            cache = null;
            Hooks.Unload();
        }
        internal static void SetCache<T>(string key, T value)
        {
            Type type = typeof(T);
            if (cache.TryGetValue(type, out var dic))
            {
                dic[key] = value;
            }
            else
            {
                cache[type] = new()
                {
                    [key] = value
                };
            }
        }
        internal static T GetCache<T>(string key)
        {
            if (cache.TryGetValue(typeof(T), out var dic) && dic.TryGetValue(key, out var value))
            {
                return (T)value;
            }
            return default;
        }
        internal static List<string> FindCloudFiles(Predicate<string> filter)
        {
            List<string> files = new();
            if(NetUtils.IsServer)
            {
                return files;
            }
            var cloud = SocialAPI.Cloud;
            if (cloud is not null)
            {
                List<string> cloudfiles = cloud.GetFiles().ToList();
                cloudfiles.ForEach(f =>
                {
                    if (filter(f))
                    {
                        files.Add(f);
                    }
                });
            }
            return files;
        }
        internal static bool FileFilter(string p) => p.EndsWith(".wld") ||
                p.EndsWith(".wld.bak") ||
                p.EndsWith(".twld") ||
                p.EndsWith(".twld.bak");
        internal static void FindFiles(List<string> container, string root, Predicate<string> filter)
        {
            if(NetUtils.IsServer)
            {
                return;
            }
            string[] dirsandfiles = Directory.GetFileSystemEntries(root);
            Parallel.ForEach(dirsandfiles, path =>
            {
                if (File.Exists(path))
                {
                    if (filter(path))
                    {
                        container.Add(path);
                    }
                }
                else if(Directory.Exists(path)) 
                {
                    FindFiles(container, path, filter);
                }
            });
        }
        class Hooks
        {
            internal static void Unload()
            {
                ILHooks.Unload();
                OnHooks.Unload();
            }
            internal static void Load()
            {
                ILHooks.Load();
                OnHooks.Load();
            }
            class ILHooks
            {
                internal static void Unload()
                {
                }
                internal static void Load()
                {
                    IL.Terraria.Main.DoDraw += Main_DoDraw;
                    IL.Terraria.Main.DrawMap += Main_DrawMap;
                    IL.Terraria.Main.DrawBackground += Main_DrawBackground;
                    IL.Terraria.Main.OldDrawBackground += Main_OldDrawBackground;
                    IL.Terraria.Graphics.Light.TileLightScanner.GetTileLight += TileLightScanner_GetTileLight;
                    IL.Terraria.Player.UpdateBiomes += Player_UpdateBiomes;

                    IL.Terraria.Main.ErasePlayer += Main_ErasePlayer;
                    IL.Terraria.Main.EraseWorld += Main_EraseWorld;
                    IL.Terraria.Main.DoUpdateInWorld += Main_DoUpdateInWorld;
                    //TODO 支持需要的钩子
                }

                private static void Main_DoUpdateInWorld(ILContext il)
                {
                    ILCursor c = new(il);
                    if (!c.TryGotoNext(MoveType.After, i => i.MatchCall("Terraria.ModLoader.IO.WorldIO", "EraseWorld")))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tMain_DoUpdateInWorld\n\tMatch A Point To Skip Update Time");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                }
                private static void EraseWorldPerWorld(int i)
                {
                    List<string> files = new();
                    try
                    {
                        WorldFileData data = Main.WorldList[i];
                        FindFiles(files,
                            Path.Combine(Main.WorldPath,
                                nameof(World.SaveType.PerPlayer),
                                data.GetFileName(false)),
                                FileFilter);
                        if (data.IsCloudSave)
                        {
                            files.AddRange(FindCloudFiles(FileFilter));
                        }
                        foreach (string file in files.ToHashSet())
                        {
                            FileUtilities.Delete(file, data.IsCloudSave);
                        }
                    }
                    catch
                    {
                        files.Insert(0, "[Debug]Files Collect Data:");
                        Everglow.Instance.Logger.Debug(string.Join("\n\t", files));
                        throw;
                    }
                }
                private static void Main_EraseWorld(ILContext il)
                {
                    ILCursor c = new(il);
                    if (!c.TryGotoNext(MoveType.After, i => i.MatchCall("Terraria.ModLoader.IO.WorldIO", "EraseWorld")))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tMain_EraseWorld\n\tMatch A Point To Delete World Files");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    c.Emit(OpCodes.Ldarg, 0);
                    c.EmitDelegate(EraseWorldPerWorld);
                }
                private static void EraseWorldPerPlayer(int i)
                {
                    List<string> files = new();
                    try
                    {
                        PlayerFileData data = Main.PlayerList[i];
                        FindFiles(files,
                            Path.Combine(Main.WorldPath,
                                nameof(World.SaveType.PerPlayer),
                                data.GetFileName(false)),
                                FileFilter);
                        if(data.IsCloudSave)
                        {
                            files.AddRange(FindCloudFiles(FileFilter));
                        }
                        foreach(string file in files.ToHashSet())
                        {
                            FileUtilities.Delete(file, data.IsCloudSave);
                        }
                    }
                    catch
                    {
                        files.Insert(0, "[Debug]Files Collect Data:");
                        Everglow.Instance.Logger.Debug(string.Join("\n\t", files));
                        throw;
                    }
                }
                private static void Main_ErasePlayer(ILContext il)
                {
                    ILCursor c = new(il);
                    if (!c.TryGotoNext(MoveType.After, i => i.MatchCall("Terraria.ModLoader.IO.PlayerIO", "ErasePlayer")))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tMain_ErasePlayer\n\tMatch A Point To Delete World Files");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    c.Emit(OpCodes.Ldarg, 0);
                    c.EmitDelegate(EraseWorldPerPlayer);
                }
                private static void HideUnderWorld_Player_UpdateBiomes(ref bool inhell)
                {
                    if (!inhell || (WorldManager.activing?.EnableUnderground ?? true))
                    {
                        return;
                    }
                    inhell = false;
                }
                private static bool HijackPlayerBiomes(Player player, Point where)
                {
                    return WorldManager.activing?.HijackPlayerBiomes(player, where) ?? false;
                }
                private static void Player_UpdateBiomes(ILContext il)
                {
                    ILCursor c = new(il);
                    if (!c.TryGotoNext(MoveType.After, i => i.MatchStloc(0)))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tPlayer_UpdateBiomes\n\tMatch A Point To Hijack Player Biomes");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    ILLabel skipret = c.DefineLabel();
                    c.Emit(OpCodes.Ldarg, 0);
                    c.Emit(OpCodes.Ldloc, 0);
                    c.EmitDelegate(HijackPlayerBiomes);
                    c.Emit(OpCodes.Brfalse, skipret);
                    c.Emit(OpCodes.Ret);
                    c.MarkLabel(skipret);
                    if (!c.TryGotoNext(MoveType.After, i => i.MatchStloc(10)))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tPlayer_UpdateBiomes\n\tMatch A Point To Hide Underworld When Need");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    c.Emit(OpCodes.Ldloca, 10);
                    c.EmitDelegate(HideUnderWorld_Player_UpdateBiomes);
                }
                /// <summary>
                /// 
                /// </summary>
                /// <returns>true意味着开启地下，所以不需要跳过地狱光照</returns>
                private static bool HideUnderWorld_TileLightScanner_GetTileLight()
                {
                    //开启或者默认时不需要跳过地狱光照
                    return WorldManager.activing?.EnableUnderground ?? true;
                }
                /// <summary>
                /// 
                /// </summary>
                /// <param name="tile"></param>
                /// <param name="x"></param>
                /// <param name="y"></param>
                /// <param name="random"></param>
                /// <param name="lightcolor"></param>
                /// <returns>true意味劫持了光照，false意味着不改动或者继续施加原版</returns>
                private static bool HijackTileLight(Tile tile, int x, int y, FastRandom random, ref Color lightcolor)
                {
                    //开启或者默认时不需要直接返回
                    return WorldManager.activing?.HijackTileLight(tile, x, y, random, ref lightcolor) ?? false;
                }
                private static void TileLightScanner_GetTileLight(ILContext il)
                {
                    ILCursor c = new(il);
                    if (!c.TryGotoNext(i => i.MatchLdarg(2), i => i.MatchLdsfld(typeof(Main), nameof(Main.worldSurface))))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tTileLightScanner_GetTileLight\n\tMatch A Point To Hijack Tile Light");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    ILLabel skipret = c.DefineLabel();
                    c.Emit(OpCodes.Ldloc, 0);
                    c.Emit(OpCodes.Ldarg, 1);
                    c.Emit(OpCodes.Ldarg, 2);
                    c.Emit(OpCodes.Ldloc, 1);
                    c.Emit(OpCodes.Ldarga, 3);
                    c.EmitDelegate(HijackTileLight);
                    c.Emit(OpCodes.Brfalse, skipret);
                    c.Emit(OpCodes.Ret);
                    c.MarkLabel(skipret);
                    if (!c.TryGotoNext(
                        i => i.MatchLdarg(0),
                        i => i.MatchLdloc(0),
                        i => i.MatchLdarg(1),
                        i => i.MatchLdarg(2),
                        i => i.MatchLdarg(3)))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tTileLightScanner_GetTileLight\n\tMatch A Point To Hide Underworld When Need");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    ILLabel skipapplyhelllight = c.DefineLabel();
                    c.EmitDelegate(HideUnderWorld_TileLightScanner_GetTileLight);
                    c.Emit(OpCodes.Brfalse, skipapplyhelllight);
                    if (!c.TryGotoNext(MoveType.After, i => i.MatchCall(typeof(TileLightScanner), "ApplyHellLight")))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tTileLightScanner_GetTileLight\n\tMatch A Point To Hide Underworld When Need");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    c.MarkLabel(skipapplyhelllight);
                }
                private static void HideUnderWorld_Main_OldDrawBackground(ref double num)
                {
                    //开启或者默认时不需要修改
                    if (WorldManager.activing?.EnableUnderground ?? true)
                    {
                        return;
                    }
                    num = Main.maxTilesY;
                }
                private static void Main_OldDrawBackground(ILContext il)
                {
                    ILCursor c = new(il);
                    if (!c.TryGotoNext(i => i.MatchLdcI4(0), i => i.MatchStloc(20)))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tMain_OldDrawBackground\n\tMatch A Point To Hide Underworld When Need");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    c.Emit(OpCodes.Ldloca, 18);
                    c.EmitDelegate(HideUnderWorld_Main_OldDrawBackground);
                }
                private static void HideUnderWorld_Main_DrawBackground(ref double num)
                {
                    //开启或者默认时不需要修改
                    if (WorldManager.activing?.EnableUnderground ?? true)
                    {
                        return;
                    }
                    num = Main.maxTilesY;
                }
                private static void Main_DrawBackground(ILContext il)
                {
                    ILCursor c = new(il);
                    if (!c.TryGotoNext(i => i.MatchLdcR4(255), i => i.MatchLdcR4(1)))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tMain_DrawBackground\n\tMatch A Point To Hide Underworld When Need");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    c.Emit(OpCodes.Ldloca, 2);
                    c.EmitDelegate(HideUnderWorld_Main_DrawBackground);
                }
                /// <summary>
                /// 绘制地图容器
                /// </summary>
                /// <param name="ContainerRange">容器需求的至少大小,Map.png的(40,4,848,240)</param>
                private static void DrawMapContainer(float num1, float num2)
                {
                    int x = (int)(num1 + Main.mapFullscreenScale * 10f);
                    int y = (int)(num2 + Main.mapFullscreenScale * 10f);
                    int width = (int)((Main.maxTilesX - 40) * Main.mapFullscreenScale);
                    int height = (int)((Main.maxTilesY - 40) * Main.mapFullscreenScale);
                    Rectangle destinationRectangle = new Rectangle(x, y, width, height);
                    SetCache("ContainerRange", destinationRectangle);
                    WorldManager.activing?.DrawCustomMap(destinationRectangle);
                }
                /// <summary>
                /// 绘制地图遮罩
                /// </summary>
                private static void PostDrawMapContent()
                {
                    World world = WorldManager.activing;
                    if (world is null || !world.UseCustomMap)
                    {
                        return;
                    }
                    world.PostDrawMapContent(GetCache<Rectangle>("ContainerRange"));
                }
                private static void ChangeMapScaleLimit(ref float minscale)
                {
                    float xscale = Main.screenWidth / (float)Main.maxTilesX * 0.6f;
                    float yscale = Main.screenHeight / (float)Main.maxTilesY * 0.6f;
                    minscale = Math.Min(xscale, yscale);
                }
                private static void Main_DrawMap(ILContext il)
                {
                    ILCursor c = new(il);
                    if (!c.TryGotoNext(MoveType.After, i => i.MatchStloc(36)))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tMain_DrawMap\n\tMatch Clamp Min Scale");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    c.Emit(OpCodes.Ldloca, 36);
                    c.EmitDelegate(ChangeMapScaleLimit);

                    if (!c.TryGotoNext(i => i.MatchLdsfld(typeof(Main), nameof(Main.screenPosition)),
                        i => i.MatchLdsfld(typeof(Main), nameof(Main.screenWidth)),
                        i => i.MatchLdsfld(typeof(Main), nameof(Main.screenHeight)),
                        i => i.MatchCall(typeof(Main), "DrawMapFullscreenBackground")))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tMain_DrawMap\n\tMatch Main.DrawMapFullscreenBackground");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    ILLabel skipcustom = c.DefineLabel();
                    ILLabel skiporig = c.DefineLabel();
                    c.EmitDelegate(() => WorldManager.activing?.UseCustomMap ?? false);
                    c.Emit(OpCodes.Brfalse, skipcustom);
                    c.Emit(OpCodes.Ldloc, 1);
                    c.Emit(OpCodes.Ldloc, 2);
                    c.EmitDelegate(DrawMapContainer);
                    c.Emit(OpCodes.Br, skiporig);
                    c.MarkLabel(skipcustom);
                    if (!c.TryGotoNext(i => i.MatchLdsfld(typeof(Main), nameof(Main.mouseLeft))))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tMain_DrawMap\n\tMatch Main.mouseLeft");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    c.MarkLabel(skiporig);
                    if (!c.TryGotoNext(i => i.MatchLdloc(18),
                        i => i.MatchBrfalse(out _),
                        i => i.MatchLdsfld(typeof(Main), nameof(Main.spriteBatch)),
                        i => i.MatchCallvirt(typeof(SpriteBatch), nameof(SpriteBatch.End))))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tMain_DrawMap\n\tMatch SpriteBatch.End");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    c.EmitDelegate(PostDrawMapContent);
                }
                private static void DrawEnterWorld(GameTime time)
                {
                    if (Main.gameMenu && (WorldManager.currenthistory?.buffer ?? null) is not null)
                    {
                        Main.UIScale = 1;
                        WorldManager.currenthistory.buffer.DrawScreenWhenEnterWorld(time);
                    }
                }
                private static void Main_DoDraw(ILContext il)
                {
                    ILCursor c = new(il);
                    if (!c.TryGotoNext(MoveType.After, i => i.MatchStsfld(typeof(Main), nameof(Main.HoverItem))))
                    {
                        Everglow.Instance.Logger.Error("IL Patch Is Failed:WorldSupport.\n\tMain_DoDraw\n\tMatch A Point To Draw When Enter World");
                        throw new OperationCanceledException("IL Patch Is Failed.");
                    }
                    c.Emit(OpCodes.Ldarg, 0);
                    c.EmitDelegate(DrawEnterWorld);
                }
            }
            class OnHooks
            {
                static ConstructorInfo TileMap_ctor;
                internal static void Unload()
                {
                    On.Terraria.WorldGen.clearWorld -= WorldGen_clearWorld;
                    On.Terraria.WorldGen.setWorldSize -= WorldGen_setWorldSize;
                    TileMap_ctor = null;
                }
                internal static void Load()
                {
                    On.Terraria.WorldGen.setWorldSize += WorldGen_setWorldSize;
                    On.Terraria.WorldGen.clearWorld += WorldGen_clearWorld;
                    On.Terraria.Main.DrawUnderworldBackground += Main_DrawUnderworldBackground;
                    //TODO 支持需要的钩子
                }
                private static void Main_DrawUnderworldBackground(On.Terraria.Main.orig_DrawUnderworldBackground orig, Main self, bool flat)
                {
                    if (WorldManager.activing?.EnableUnderground ?? true)
                    {
                        orig(self, flat);
                    }
                }
                private static void WorldGen_clearWorld(On.Terraria.WorldGen.orig_clearWorld orig)
                {
                    SetWorldSize();
                    orig();
                }
                private static void WorldGen_setWorldSize(On.Terraria.WorldGen.orig_setWorldSize orig)
                {
                    SetWorldSize();
                    orig();
                }
                private static void SetWorldSize()
                {
                    WorldFileData data = Main.ActiveWorldFileData;
                    int fixedwidth = Math.Max(8400, data.WorldSizeX);
                    int fixedheight = Math.Max(2400, data.WorldSizeY);
                    if (fixedwidth > Main.tile.Width || fixedheight > Main.tile.Height)
                    {
                        fixedwidth = ((fixedwidth - 1) / 200 + 1) * 200;
                        fixedheight = ((fixedheight - 1) / 150 + 1) * 150;
                        //要不要考虑额外预留?
                        long ExpectedSize =
                            //服务端本体占用,单机模式直接切换不考虑此项
                            NetUtils.IsServer ? Process.GetCurrentProcess().PagedMemorySize64 : 0 +
                            //地图大小差距
                            //每物块占用:22=四种TileDate占用:(8+2+2+2)+Map占用4+小地图绘制缓冲4
                            (fixedwidth * fixedheight - Main.tile.Width * Main.tile.Height) * 22;
                        long AvailableBytes = GC.GetGCMemoryInfo().TotalAvailableMemoryBytes;
                        if (ExpectedSize > AvailableBytes)
                        {
                            throw new OutOfMemoryException(
                                $"The remaining operating space is smaller than the expected required space:" +
                                $"[{ExpectedSize}/{AvailableBytes}]");
                        }
                        TileMap_ctor ??= typeof(Tilemap).GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic,
                            new Type[]
                            {
                                typeof(ushort),
                                typeof(ushort)
                            });
                        Main.tile = (Tilemap)TileMap_ctor.Invoke(new object[]
                        {
                            (ushort)fixedwidth,
                            (ushort)fixedheight
                        });
                        Main.Map = new(data.WorldSizeX, data.WorldSizeY);
                        Main.mapMaxX = data.WorldSizeX;
                        Main.mapMaxY = data.WorldSizeY;
                        int fixedMapWidth = data.WorldSizeX / Main.textureMaxWidth + 2;
                        int fixedMapHeight = data.WorldSizeY / Main.textureMaxHeight + 2;
                        if (fixedMapWidth > Main.mapTargetX || fixedMapHeight > Main.mapTargetY)
                        {
                            Main.mapTargetX = Math.Max(5, fixedMapWidth);
                            Main.mapTargetY = Math.Max(3, fixedMapHeight);
                            Main.instance.mapTarget = new RenderTarget2D[Main.mapTargetX, Main.mapTargetY];
                            Main.initMap = new bool[Main.mapTargetX, Main.mapTargetY];
                            Main.mapWasContentLost = new bool[Main.mapTargetX, Main.mapTargetY];
                            Main.instance.TilePaintSystem = new();
                            Main.instance.TilesRenderer = new(Main.instance.TilePaintSystem);
                            Main.instance.WallsRenderer = new(Main.instance.TilePaintSystem);
                        }
                    }
                }
            }
        }
    }
}