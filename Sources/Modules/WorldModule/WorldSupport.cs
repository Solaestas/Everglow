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

namespace Everglow.Sources.Modules.WorldModule
{
    internal class WorldSupport : IModule
    {
        static Dictionary<Type, Dictionary<string, object>> cache;
        public string Name => nameof(WorldModule);
        public void Load()
        {
            cache = new();
            Hooks.Load();
        }
        public void Unload()
        {
            cache = null;
            Hooks.Unload();
        }
        internal static void SetCache<T>(string key,T value)
        {
            Type type = typeof(T);
            if(cache.TryGetValue(type,out var dic))
            {
                dic[key] = value;
            }
            else
            {
                cache[type] = new() { { key, value } };
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
                    IL.Terraria.Main.DrawMap += Main_DrawMap;
                    //TODO 支持需要的钩子
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
                private static void PostDrawMapContent()
                {
                    World world = WorldManager.activing;
                    if (world is null || !world.UseCustomMap)
                    {
                        return;
                    }
                    world.PostDrawMapContent(GetCache<Rectangle>("ContainerRange"));
                }
                private static void Main_DrawMap(ILContext il)
                {
                    ILCursor cursor = new(il);
                    if (!cursor.TryGotoNext(i => i.MatchLdsfld(typeof(Main), nameof(Main.screenPosition)),
                        i => i.MatchLdsfld(typeof(Main), nameof(Main.screenWidth)),
                        i => i.MatchLdsfld(typeof(Main), nameof(Main.screenHeight)),
                        i => i.MatchCall(typeof(Main), "DrawMapFullscreenBackground")))
                    {
                        throw new OperationCanceledException("Can't match Main.DrawMapFullscreenBackground");
                    }
                    ILLabel skipcustom = cursor.DefineLabel();
                    ILLabel skiporig = cursor.DefineLabel();
                    cursor.EmitDelegate(() => WorldManager.activing?.UseCustomMap ?? false);
                    cursor.Emit(OpCodes.Brfalse, skipcustom);
                    cursor.Emit(OpCodes.Ldloc, 1);
                    cursor.Emit(OpCodes.Ldloc, 2);
                    cursor.EmitDelegate(DrawMapContainer);
                    cursor.Emit(OpCodes.Br, skiporig);
                    cursor.MarkLabel(skipcustom);
                    if (!cursor.TryGotoNext(i => i.MatchLdsfld(typeof(Main), nameof(Main.mouseLeft))))
                    {
                        throw new OperationCanceledException("Can't match Main.mouseLeft");
                    }
                    cursor.MarkLabel(skiporig);
                    if (!cursor.TryGotoNext(i => i.MatchLdloc(18),
                        i => i.MatchBrfalse(out _),
                        i => i.MatchLdsfld(typeof(Main), nameof(Main.spriteBatch)),
                        i => i.MatchCallvirt(typeof(SpriteBatch), nameof(SpriteBatch.End))))
                    {
                        return;
                    }
                    cursor.EmitDelegate(PostDrawMapContent);
                }
            }
            class OnHooks
            {
                static ConstructorInfo TileMap_ctor;
                const int MapStyle_NoMap = 0;
                const int MapStyle_RightTop = 1;
                const int MapStyle_FullVirtual = 2;
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
                    //TODO 支持需要的钩子
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
