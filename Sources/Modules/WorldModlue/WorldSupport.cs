using Everglow.Sources.Commons.Core.ModuleSystem;
using Everglow.Sources.Commons.Function.NetUtils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Terraria.IO;

namespace Everglow.Sources.Modules.WorldModlue
{
    internal class WorldSupport : IModule
    {
        public string Name => nameof(WorldModlue);
        public void Load()
        {
            Hooks.Load();
        }
        public void Unload()
        {
            Hooks.Unload();
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
                    //TODO 支持需要的钩子
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
                        int fixedMapWidth = data.WorldSizeX / Main.textureMaxWidth + 2;
                        int FixedMapHeight = data.WorldSizeY / Main.textureMaxHeight + 2;
                        if (fixedMapWidth > Main.mapTargetX || FixedMapHeight > Main.mapTargetY)
                        {
                            Main.mapTargetX = Math.Max(5, fixedMapWidth);
                            Main.mapTargetY = Math.Max(3, FixedMapHeight);
                            Main.instance.mapTarget = new RenderTarget2D[Main.mapTargetX, Main.mapTargetY];
                            Main.initMap = new bool[Main.mapTargetX, Main.mapTargetY];
                            Main.mapWasContentLost = new bool[Main.mapTargetX, Main.mapTargetY];
                        }
                    }
                }
            }
        }
    }
}
