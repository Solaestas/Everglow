using System.IO.Pipes;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.Graphics.Capture;
using Terraria.IO;
using Terraria.Localization;
using Terraria.Net;
using Terraria.Social;
using Terraria.Utilities;
using Terraria.WorldBuilding;

namespace Everglow.Sources.Modules.SubWorldModule
{
    internal class SubworldSystem : ModSystem
    {
        public static void SetUp()
        {
            Player.Hooks.OnEnterWorld += OnEnterWorld;
            subworlds = new();
            while (waitregister.TryDequeue(out var world))
            {
                subworlds.Add(world);
            }
            playerLocations = new();
        }
        public override void Unload()
        {
            Player.Hooks.OnEnterWorld -= OnEnterWorld;
        }
        public static Subworld Current => current;
        public static bool IsActive(string id)
        {
            return (current?.FullName) == id;
        }
        public static bool IsActive<T>() where T : Subworld
        {
            return (current?.GetType()) == typeof(T);
        }
        public static bool AnyActive(Mod mod)
        {
            return (current?.Mod) == mod;
        }
        public static bool AnyActive() => current is not null;
        public static bool AnyActive<T>() where T : Mod
        {
            return (current?.Mod) == ModContent.GetInstance<T>();
        }
        public static string CurrentPath => current.SpecailPath ?? Path.Combine(Main.WorldPath, "Subworlds", Path.GetFileNameWithoutExtension(main.Path), current.Mod.Name + "_" + current.Name + ".wld");
        private static void BeginEntering(int index)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                if (current is null)
                {
                    main = Main.ActiveWorldFileData;
                }
                current = subworlds[index];
                Task.Factory.StartNew(new Action(ExitWorldCallBack));
            }
            else
            {
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    new SubworldPacket(0, (ushort)index);
                    //ModPacket packet = Everglow.Instance.GetPacket(256);
                    //packet.Write(0);
                    //packet.Write((ushort)index);
                    //packet.Send(-1, -1);
                }
            }
        }
        public static bool Enter(string id)
        {
            if (current == cache)
            {
                for (int i = 0; i < subworlds.Count; i++)
                {
                    if (subworlds[i].FullName == id)
                    {
                        BeginEntering(i);
                        return true;
                    }
                }
            }
            return false;
        }
        public static bool Enter<T>() where T : Subworld
        {
            if (current == cache)
            {
                for (int i = 0; i < subworlds.Count; i++)
                {
                    if (subworlds[i].GetType() == typeof(T))
                    {
                        BeginEntering(i);
                        return true;
                    }
                }
            }
            return false;
        }
        public static void Exit()
        {
            if (current is not null && current == cache)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    current = null;
                    Task.Factory.StartNew(new Action(ExitWorldCallBack));
                }
                else
                {
                    if (Main.netMode == NetmodeID.MultiplayerClient)
                    {
                        new SubworldPacket(1, 0);
                        //ModPacket packet = Everglow.Instance.GetPacket(256);
                        //packet.Write(1);
                        //packet.Send(-1, -1);
                        Main.LocalPlayer.TryGetModPlayer(ModContent.Find<ModPlayer>(""), out ModPlayer modPlayer);
                    }
                }
            }
        }
        public static void StartSubserver(string id)
        {
            Process process = new();
            string fileName = Environment.ProcessPath;
            process.StartInfo.FileName = fileName.Remove(fileName.LastIndexOf("."));
            process.StartInfo.Arguments = string.Concat(new string[]
            {
                "tModLoader.dll -server -showserverconsole -world \"",
                Program.LaunchParameters["-world"],
                "\" -subworld \"",
                id,
                "\""
            });
            process.StartInfo.UseShellExecute = true;
            Task.Factory.StartNew(new Action<object>(SubserverCallBack), id);
            process.Start();
        }
        internal static void GenerateSubworlds()
        {
            main = Main.ActiveWorldFileData;
            bool isCloudSave = main.IsCloudSave;
            foreach (Subworld subworld in subworlds)
            {
                if (subworld.ShouldSave)
                {
                    current = subworld;
                    LoadSubworld(CurrentPath, isCloudSave);
                    WorldFile.SaveWorld(isCloudSave, false);
                    Main.ActiveWorldFileData = main;
                }
            }
        }
        internal static void EraseSubworlds(int index)
        {
            WorldFileData worldFileData = Main.WorldList[index];
            string path = Path.Combine(Main.WorldPath, "Subworlds", Path.GetFileNameWithoutExtension(worldFileData.Path));
            if (FileUtilities.Exists(path, worldFileData.IsCloudSave))
            {
                FileUtilities.Delete(path, worldFileData.IsCloudSave, false);
            }
        }
        internal static bool LoadIntoSubworld()
        {
            if (Program.LaunchParameters.TryGetValue("-subworld", out string b))
            {
                for (int i = 0; i < subworlds.Count; i++)
                {
                    if (subworlds[i].FullName == b)
                    {
                        Main.myPlayer = 255;
                        main = Main.ActiveWorldFileData;
                        current = subworlds[i];
                        LoadWorld();
                        Console.Title = Main.worldName;
                        for (int j = 0; j < Netplay.Clients.Length; j++)
                        {
                            Netplay.Clients[j].Id = j;
                            Netplay.Clients[j].Reset();
                            Netplay.Clients[j].ReadBuffer = null;
                        }
                        new Thread(new ThreadStart(ServerCallBack)).Start();
                        return true;
                    }
                }
                Main.instance.Exit();
            }
            return false;
        }
        private static void SubserverCallBack(object id)
        {
            using (NamedPipeServerStream namedPipeServerStream = new((string)id, PipeDirection.In, -1))
            {
                while (true)
                {
                    namedPipeServerStream.WaitForConnection();
                    int num = namedPipeServerStream.ReadByte();
                    int num2 = namedPipeServerStream.ReadByte();
                    int num3 = namedPipeServerStream.ReadByte();
                    int num4 = num3 << 8 | num2;
                    byte[] array = new byte[num4];
                    namedPipeServerStream.Read(array, 2, num4 - 2);
                    array[0] = (byte)num2;
                    array[1] = (byte)num3;
                    Netplay.Clients[num].Socket.AsyncSend(array, 0, num4, delegate (object state)
                    {
                    }, true);
                    namedPipeServerStream.Disconnect();
                }
            }
        }
        private static void ServerCallBack()
        {
            using (NamedPipeServerStream namedPipeServerStream = new NamedPipeServerStream("World", PipeDirection.In, -1))
            {
                while (!Netplay.Disconnect)
                {
                    namedPipeServerStream.WaitForConnection();
                    MessageBuffer messageBuffer = NetMessage.buffer[namedPipeServerStream.ReadByte()];
                    namedPipeServerStream.Read(messageBuffer.readBuffer, 0, 2);
                    int num = BitConverter.ToUInt16(messageBuffer.readBuffer, 0);
                    namedPipeServerStream.Read(messageBuffer.readBuffer, 2, num - 2);
                    if (messageBuffer.readBuffer[2] == NetmodeID.MultiplayerClient)
                    {
                        Netplay.Clients[messageBuffer.whoAmI].Socket = new SubserverSocket(messageBuffer.whoAmI);
                        Netplay.Clients[messageBuffer.whoAmI].IsActive = true;
                        Netplay.HasClients = true;
                    }
                    messageBuffer.GetData(2, num - 2, out _);
                    namedPipeServerStream.Disconnect();
                }
            }
        }
        internal static void ExitWorldCallBack()
        {
            if (Main.netMode != NetmodeID.Server)
            {
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    WorldFile.CacheSaveTime();
                }
                Main.invasionProgress = -1;
                Main.invasionProgressDisplayLeft = 0;
                Main.invasionProgressAlpha = 0f;
                Main.invasionProgressIcon = 0;
                cache?.OnExit();
                noReturn = false;
                hideUnderworld = false;
                current?.OnEnter();
                Main.gameMenu = true;
                SoundEngine.StopTrackedSounds();
                CaptureInterface.ResetFocus();
                Main.ActivePlayerFileData.StopPlayTimer();
                Player.SavePlayer(Main.ActivePlayerFileData, false);
                Player.ClearPlayerTempInfo();
                Rain.ClearRain();
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                WorldFile.SaveWorld();
            }
            SystemLoader.OnWorldUnload();
            typeof(ModLoader).Assembly.GetType("Terraria.ModLoader.IO.TileIO").GetMethod("PostExitWorldCleanup", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, null);
            Main.fastForwardTime = false;
            Main.UpdateTimeRate();
            WorldGen.noMapUpdate = true;
            if (cache is not null && cache.NoPlayerSaving && Main.netMode != NetmodeID.Server)
            {
                PlayerFileData fileData = Player.GetFileData(Main.ActivePlayerFileData.Path, Main.ActivePlayerFileData.IsCloudSave);
                if (fileData is not null)
                {
                    fileData.Player.whoAmI = Main.myPlayer;
                    fileData.SetAsActive();
                }
            }
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                LoadWorld();
            }
            else
            {
                NetMessage.SendData(MessageID.Hello, -1, -1, null, 0, 0f, 0f, 0f, 0, 0, 0);
            }
        }
        internal static void LoadWorld()
        {
            WorldGen.gen = true;
            WorldGen.loadFailed = false;
            WorldGen.loadSuccess = false;
            WorldGen.worldBackup = true;
            Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);
            bool flag = current is not null;
            bool isCloudSave = main.IsCloudSave;
            string text = flag ? CurrentPath : main.Path;
            if (current?.ShouldSave ?? false)
            {
                if (FileUtilities.Exists(text, isCloudSave))
                {
                    var data = WorldFile.GetAllMetadata(text, isCloudSave);
                    if (data.WorldSizeX > Main.tile.Width || data.WorldSizeY > Main.tile.Height)
                    {
                        int fixedx = (int)(Math.Floor(data.WorldSizeX / 200f) * 200);
                        int fixedy = (int)(Math.Floor(data.WorldSizeY / 150f) * 150);
                        Main.maxTilesX = fixedx;
                        Main.maxTilesY = fixedy;
                        Main.tile = (Tilemap)TileMapConstructor.Invoke(new object[] { (ushort)(fixedx + 1), (ushort)(fixedy + 1) });
                        Main.Map = new(Main.maxTilesX, Main.maxTilesY);
                        Main.mapMinX = 0;
                        Main.mapMinY = 0;
                        Main.mapMaxX = Main.maxTilesX;
                        Main.mapMaxY = Main.maxTilesY;
                        Main.worldSurface = Main.maxTilesY * 0.3;
                        Main.rockLayer = Main.maxTilesY * 0.5;
                        WorldGen.lavaLine = (int)(Main.rockLayer + Main.maxTilesY) / 2 + Main.rand.Next(50, 80);
                        Main.instance.mapTarget = new RenderTarget2D[(Main.maxTilesX / Main.textureMaxWidth) + 1,
                            (Main.maxTilesY / Main.textureMaxHeight) + 1];
                        Main.mapWasContentLost = new bool[Main.instance.mapTarget.GetLength(0), Main.instance.mapTarget.GetLength(1)];
                        Main.initMap = new bool[Main.instance.mapTarget.GetLength(0), Main.instance.mapTarget.GetLength(1)];
                        Main.instance.TilePaintSystem = new();
                        Main.instance.TilesRenderer = new(Main.instance.TilePaintSystem);
                        Main.instance.WallsRenderer = new(Main.instance.TilePaintSystem);
                        Main.bottomWorld = Main.maxTilesY * 16;
                        Main.rightWorld = Main.maxTilesX * 16;
                        Main.maxSectionsX = (Main.maxTilesX - 1) / 200 + 1;
                        Main.maxSectionsY = (Main.maxTilesY - 1) / 150 + 1;
                    }
                }
                if (current is null)
                {
                    Main.ActiveWorldFileData = main;
                }
                LoadWorldFile(text, isCloudSave);
                if (WorldGen.loadFailed)
                {
                    LoadWorldFile(text, isCloudSave);
                    if (WorldGen.loadFailed)
                    {
                        if (FileUtilities.Exists(text + ".bak", isCloudSave))
                        {
                            FileUtilities.Copy(text, text + ".bad", isCloudSave, true);
                            FileUtilities.Copy(text + ".bak", text, isCloudSave, true);
                            FileUtilities.Delete(text + ".bak", isCloudSave, false);
                            string text2 = Path.ChangeExtension(text, ".twld");
                            if (FileUtilities.Exists(text2, isCloudSave))
                            {
                                FileUtilities.Copy(text2, text2 + ".bad", isCloudSave, true);
                            }
                            if (FileUtilities.Exists(text2 + ".bak", isCloudSave))
                            {
                                FileUtilities.Copy(text2 + ".bak", text2, isCloudSave, true);
                                FileUtilities.Delete(text2 + ".bak", isCloudSave, false);
                            }
                            LoadWorldFile(text, isCloudSave);
                            if (WorldGen.loadFailed)
                            {
                                LoadWorldFile(text, isCloudSave);
                                if (WorldGen.loadFailed)
                                {
                                    FileUtilities.Copy(text, text + ".bak", isCloudSave, true);
                                    FileUtilities.Copy(text + ".bad", text, isCloudSave, true);
                                    FileUtilities.Delete(text + ".bad", isCloudSave, false);
                                    if (FileUtilities.Exists(text2, isCloudSave))
                                    {
                                        FileUtilities.Copy(text2, text2 + ".bak", isCloudSave, true);
                                    }
                                    if (FileUtilities.Exists(text2 + ".bad", isCloudSave))
                                    {
                                        FileUtilities.Copy(text2 + ".bad", text2, isCloudSave, true);
                                        FileUtilities.Delete(text2 + ".bad", isCloudSave, false);
                                    }
                                }
                            }
                        }
                        else
                        {
                            WorldGen.worldBackup = false;
                        }
                    }
                }
            }
            cache?.OnUnload();
            if (flag)
            {
                Main.worldName = Language.GetTextValue("Mods." + current.Mod.Name + ".SubworldName." + current.Name);
                if (WorldGen.loadFailed)
                {
                    Everglow.Instance.Logger.Warn("Failed to load \"" + Main.worldName + (WorldGen.worldBackup ? "\" from file" : "\" from file, no backup"));
                }
                if (!WorldGen.loadSuccess)
                {
                    LoadSubworld(text, isCloudSave);
                }
                current.OnLoad();
            }
            else
            {
                if (!WorldGen.loadSuccess)
                {
                    Everglow.Instance.Logger.Error("Failed to load \"" + main.Name + (WorldGen.worldBackup ? "\" from file" : "\" from file, no backup"));
                    Main.menuMode = 0;
                    if (Main.netMode == NetmodeID.Server)
                    {
                        Netplay.Disconnect = true;
                    }
                    return;
                }
            }
            WorldGen.gen = false;
            if (Main.netMode != NetmodeID.Server)
            {
                if (Main.mapEnabled)
                {
                    Main.Map.Load();
                }
                Main.sectionManager.SetAllFramesLoaded();
                while (Main.mapEnabled && Main.loadMapLock)
                {
                    Main.statusText = Language.GetTextValue("LegacyWorldGen.68") + " " + ((int)((Main.loadMapLastX / (float)Main.maxTilesX * 100f) + 1f)).ToString() + "%";
                    Thread.Sleep(0);
                }
                Player localPlayer = Main.LocalPlayer;
                if (Main.anglerWhoFinishedToday.Contains(localPlayer.name))
                {
                    Main.anglerQuestFinished = true;
                }
                localPlayer.Spawn(PlayerSpawnContext.SpawningIntoWorld);
                Main.ActivePlayerFileData.StartPlayTimer();
                Player.Hooks.EnterWorld(Main.myPlayer);
                Main.resetClouds = true;
                Main.gameMenu = false;
            }
        }
        private static void OnEnterWorld(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                cache?.OnUnload();
                current?.OnLoad();
            }
            cache = current;
        }
        internal static void LoadSubworld(string path, bool fromCloud)
        {
            WorldFileData worldFileData = new(path, fromCloud)
            {
                Name = Main.worldName,
                GameMode = Main.GameMode,
                CreationTime = DateTime.Now,
                Metadata = FileMetadata.FromCurrentSettings(FileType.World),
                WorldGeneratorVersion = 1065151889409UL
            };
            worldFileData.SetSeed(main.SeedText);
            using (MD5 md = MD5.Create())
            {
                worldFileData.UniqueId = new Guid(md.ComputeHash(Encoding.ASCII.GetBytes(Path.GetFileNameWithoutExtension(main.Path) + current.Name)));
            }
            Main.ActiveWorldFileData = worldFileData;
            if (current.Width > Main.tile.Width || current.Height > Main.tile.Height)
            {
                int fixedx = (int)(Math.Floor(current.Width / 200f) * 200);
                int fixedy = (int)(Math.Floor(current.Height / 150f) * 150);
                if (FileUtilities.Exists(path, fromCloud))
                {
                    var data = WorldFile.GetAllMetadata(path, fromCloud);
                    fixedx = data.WorldSizeX;
                    fixedy = data.WorldSizeY;
                }
                Main.maxTilesX = fixedx;
                Main.maxTilesY = fixedy;
                Main.tile = (Tilemap)TileMapConstructor.Invoke(new object[] { (ushort)(fixedx + 1), (ushort)(fixedy + 1) });
                Main.Map = new(Main.maxTilesX, Main.maxTilesY);
                Main.mapMinX = 0;
                Main.mapMinY = 0;
                Main.mapMaxX = Main.maxTilesX;
                Main.mapMaxY = Main.maxTilesY;
                Main.worldSurface = Main.maxTilesY * 0.3;
                Main.rockLayer = Main.maxTilesY * 0.5;
                WorldGen.lavaLine = (int)(Main.rockLayer + Main.maxTilesY) / 2 + Main.rand.Next(50, 80);
                Main.instance.mapTarget = new RenderTarget2D[(Main.maxTilesX / Main.textureMaxWidth) + 1,
                    (Main.maxTilesY / Main.textureMaxHeight) + 1];
                Main.mapWasContentLost = new bool[Main.instance.mapTarget.GetLength(0), Main.instance.mapTarget.GetLength(1)];
                Main.initMap = new bool[Main.instance.mapTarget.GetLength(0), Main.instance.mapTarget.GetLength(1)];
                Main.instance.TilePaintSystem = new();
                Main.instance.TilesRenderer = new(Main.instance.TilePaintSystem);
                Main.instance.WallsRenderer = new(Main.instance.TilePaintSystem);
                Main.bottomWorld = Main.maxTilesY * 16;
                Main.rightWorld = Main.maxTilesX * 16;
                Main.maxSectionsX = (Main.maxTilesX - 1) / 200 + 1;
                Main.maxSectionsY = (Main.maxTilesY - 1) / 150 + 1;
            }
            Main.spawnTileX = Main.maxTilesX / 2;
            Main.spawnTileY = Main.maxTilesY / 2;
            WorldGen.clearWorld();
            Main.worldSurface = Main.maxTilesY * 0.3;
            Main.rockLayer = Main.maxTilesY * 0.5;
            WorldGen.waterLine = Main.maxTilesY;
            Main.weatherCounter = int.MaxValue;
            Cloud.resetClouds();
            float num = 0f;
            for (int i = 0; i < current.Tasks.Count; i++)
            {
                num += current.Tasks[i].Weight;
            }
            WorldGenerator.CurrentGenerationProgress = new GenerationProgress
            {
                TotalWeight = num
            };
            WorldGenConfiguration config = current.Config;
            for (int j = 0; j < current.Tasks.Count; j++)
            {
                WorldGen._genRand = new UnifiedRandom(worldFileData.Seed);
                Main.rand = new UnifiedRandom(worldFileData.Seed);
                GenPass genPass = current.Tasks[j];
                WorldGenerator.CurrentGenerationProgress.Start(genPass.Weight);
                genPass.Apply(WorldGenerator.CurrentGenerationProgress, config?.GetPassConfiguration(genPass.Name));
                WorldGenerator.CurrentGenerationProgress.End();
            }
            WorldGenerator.CurrentGenerationProgress = null;
            SystemLoader.OnWorldLoad();
        }
        internal static void LoadWorldFile(string path, bool fromCloud)
        {
            bool flag = fromCloud && SocialAPI.Cloud is not null;
            if (FileUtilities.Exists(path, flag))
            {
                if (current is not null)
                {
                    WorldFileData allMetadata = WorldFile.GetAllMetadata(path, fromCloud);
                    if (allMetadata is not null)
                    {
                        Main.ActiveWorldFileData = allMetadata;
                    }
                }
                using (MemoryStream memoryStream = new MemoryStream(FileUtilities.ReadAllBytes(path, flag)))
                {
                    using (BinaryReader binaryReader = new BinaryReader(memoryStream))
                    {
                        try
                        {
                            int num = WorldFile.LoadWorld_Version2(binaryReader);
                            binaryReader.Close();
                            memoryStream.Close();
                            SystemLoader.OnWorldLoad();
                            typeof(ModLoader).Assembly.GetType("Terraria.ModLoader.IO.WorldIO").GetMethod("Load", BindingFlags.Static | BindingFlags.NonPublic).Invoke(null, new object[]
                            {
                                path,
                                flag
                            });
                            bool flag5 = num != 0;
                            if (flag5)
                            {
                                WorldGen.loadFailed = true;
                                WorldGen.loadSuccess = false;
                            }
                            else
                            {
                                WorldGen.loadSuccess = true;
                                WorldGen.loadFailed = false;
                                WorldGen.waterLine = Main.maxTilesY;
                                Liquid.QuickWater(2, -1, -1);
                                WorldGen.WaterCheck();
                                Liquid.quickSettle = true;
                                int num2 = 0;
                                int num3 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                                float num4 = 0f;
                                while (Liquid.numLiquid > 0 && num2 < 100000)
                                {
                                    num2++;
                                    float num5 = (num3 - Liquid.numLiquid + LiquidBuffer.numLiquidBuffer) / (float)num3;
                                    bool flag6 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > num3;
                                    if (flag6)
                                    {
                                        num3 = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                                    }
                                    bool flag7 = num5 > num4;
                                    if (flag7)
                                    {
                                        num4 = num5;
                                    }
                                    else
                                    {
                                        num5 = num4;
                                    }
                                    Main.statusText = Language.GetTextValue("LegacyWorldGen.27") + " " + ((int)(num5 * 100f / 2f + 50f)).ToString() + "%";
                                    Liquid.UpdateLiquid();
                                }
                                Liquid.quickSettle = false;
                                Main.weatherCounter = int.MaxValue;
                                Cloud.resetClouds();
                                WorldGen.WaterCheck();
                                bool flag8 = Main.slimeRainTime > 0.0;
                                if (flag8)
                                {
                                    Main.StartSlimeRain(false);
                                }
                                WorldFile.SetOngoingToTemps();
                            }
                        }
                        catch
                        {
                            WorldGen.loadFailed = true;
                            WorldGen.loadSuccess = false;
                            try
                            {
                                binaryReader.Close();
                                memoryStream.Close();
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
        }
        internal static List<Subworld> subworlds;
        internal static Queue<Subworld> waitregister = new();
        public static Dictionary<RemoteAddress, int> playerLocations;
        internal static Subworld current;
        internal static Subworld cache;
        internal static WorldFileData main;
        public static bool noReturn;
        public static bool hideUnderworld;
        private static ConstructorInfo _tileMapConstructor;
        private static ConstructorInfo TileMapConstructor
        {
            get
            {
                if (_tileMapConstructor is null)
                {
                    try
                    {
                        _tileMapConstructor = typeof(Tilemap).GetConstructor(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic, new Type[] { typeof(ushort), typeof(ushort) });
                        if (_tileMapConstructor is null)
                        {
                            Everglow.Instance.Logger.Error("Can't Find TileMap's ConstructorInfo\n" + new NullReferenceException(nameof(_tileMapConstructor)).ToString());
                            throw new Exception("找不到TileMap的构造方法");
                        }
                        else
                        {
                            return _tileMapConstructor;
                        }
                    }
                    catch (Exception ex)
                    {
                        Everglow.Instance.Logger.Error(ex);
                        throw new Exception("找不到TileMap的构造方法");
                    }
                }
                else
                {
                    return _tileMapConstructor;
                }
            }
            set
            {
                _tileMapConstructor = value;
            }
        }
    }
}