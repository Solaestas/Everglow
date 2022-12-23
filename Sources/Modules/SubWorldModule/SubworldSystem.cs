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
using Terraria.ID;
using System;
using System.Runtime.CompilerServices;
using Terraria.ModLoader.IO;
using Terraria.Graphics;
using Terraria.UI;
using Terraria.Map;

namespace Everglow.Sources.Modules.SubWorldModule
{
    internal class SubworldSystem : ModSystem
    {
        internal static Dictionary<RemoteAddress, int> playerLocations = new();
        internal static Dictionary<string, Subworld> subworlds = new();
        internal static Subworld current;
        internal static Subworld cache;
        internal static WorldFileData root;
        public static IReadOnlyDictionary<RemoteAddress, int> PlayerLocations => playerLocations;
        public static Subworld Current => current;
        public static string CurrentPath
        {
            get
            {
                return current.HowSaveWorld switch
                {
                    Subworld.SaveSetting.PerPlayer
                        => Path.Combine(Main.PlayerPath,
                            nameof(Subworld),
                            Path.GetFileNameWithoutExtension(Main.ActivePlayerFileData.Path),
                            current.FullName + ".wld"),
                    Subworld.SaveSetting.PerWorld
                        => Path.Combine(Main.WorldPath,
                            nameof(Subworld),
                            Path.GetFileNameWithoutExtension(root.Path),
                            current.FullName + ".wld"),
                    Subworld.SaveSetting.Public
                        => Path.Combine(Main.SavePath,
                            nameof(Subworld),
                            current.FullName + ".wld"),
                    _ => throw new InvalidOperationException("You're not supposed to run it here.")
                };
            }
        }
        internal static void Register(Subworld subworld)
        {
            ModTypeLookup<Subworld>.Register(subworld);
            subworlds[subworld.FullName] = subworld;
            subworld.SetupContent();
        }
        public override void OnModLoad()
        {
            current = cache = null;
            root = null;
            Player.Hooks.OnEnterWorld += Hooks_OnEnterWorld;
        }
        public override void OnModUnload()
        {
            playerLocations.Clear();
            subworlds.Clear();
            current = cache = null;
            root = null;
        }
        private void Hooks_OnEnterWorld(Player player)
        {
            if (Main.netMode != NetmodeID.Server)
            {
                cache?.OnUnload();
                current?.OnLoad();
            }
            cache = current;
        }
        public static bool IsActive<T>() where T : Subworld => ModContent.GetInstance<T>() == current;
        static void Enter(Subworld target)
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                return;
            }
            if (!subworlds.ContainsKey(target.FullName))
            {
                Everglow.Instance.Logger.Error("The historical record is wrong.");
                ExitAll();
                return;
            }
            BeginEntering(target);
        }
        public static bool Enter<T>() where T : Subworld
        {
            if (Main.netMode != NetmodeID.SinglePlayer)
            {
                return false;
            }
            if (subworlds.TryGetValue(ModContent.GetInstance<T>().FullName, out Subworld target))
            {
                BeginEntering(target);
                return true;
            }
            return false;
        }
        public static void Exit() => ExitNow();
        internal static void ExitAll()
        {
            if (current != null && current == cache)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    current = null;
                    Task.Factory.StartNew(ExitWorldCallBack);
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    //TODO:联机
                    //ModPacket packet = ModContent.GetInstance<SubworldLibrary>().GetPacket();
                    //packet.Write((byte)1);
                    //packet.Send();
                }
            }
        }
        internal static void ExitNow()
        {
            if (current != null && current == cache)
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    current = null;
                    Task.Factory.StartNew(ExitWorldCallBack);
                }
                else if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    //TODO:联机
                    //ModPacket packet = ModContent.GetInstance<SubworldLibrary>().GetPacket();
                    //packet.Write((byte)1);
                    //packet.Send();
                }
            }
        }
        internal static bool LoadIntoSubworlds()
        {
            if (Program.LaunchParameters.TryGetValue("-subworld", out string fullname))
            {
                if (subworlds.TryGetValue(fullname, out Subworld target))
                {
                    Main.myPlayer = 255;
                    root = Main.ActiveWorldFileData;
                    current = target;
                    LoadWorld();
                    Console.Title = Main.worldName;

                    for (int j = 0; j < Netplay.Clients.Length; j++)
                    {
                        Netplay.Clients[j].Id = j;
                        Netplay.Clients[j].Reset();
                        Netplay.Clients[j].ReadBuffer = null; // not used, saves 262kb
                    }

                    new Thread(new ThreadStart(ServerCallBack)).Start();

                    return true;
                }
                Main.instance.Exit();
            }
            return false;
        }
        static void ServerCallBack()
        {
            using NamedPipeServerStream pipe = new NamedPipeServerStream("World", PipeDirection.In, -1);
            while (!Netplay.Disconnect)
            {
                pipe.WaitForConnection();

                MessageBuffer buffer = NetMessage.buffer[pipe.ReadByte()];

                pipe.Read(buffer.readBuffer, 0, 2);
                int length = BitConverter.ToUInt16(buffer.readBuffer, 0);
                pipe.Read(buffer.readBuffer, 2, length - 2);

                if (buffer.readBuffer[2] == 1)
                {
                    Netplay.Clients[buffer.whoAmI].Socket = new SubserverSocket(buffer.whoAmI);
                    Netplay.Clients[buffer.whoAmI].IsActive = true;
                    Netplay.HasClients = true;
                }

                //string str = "R" + buffer.readBuffer[2] + "(" + length + ") " + buffer.whoAmI + " ";
                //for (int i = 0; i < length; i++)
                //{
                //	str += buffer.readBuffer[i] + " ";
                //}
                //ModContent.GetInstance<SubworldLibrary>().Logger.Info(str);

                buffer.GetData(2, length - 2, out var _);

                pipe.Disconnect();
            }

            // no caching; world loading is very infrequent
            typeof(ModLoader).Assembly.GetType("Terraria.ModLoader.IO.TileIO").GetMethod("PostExitWorldCleanup", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, null);
        }
        static void LoadWorld()
        {
            bool isSubworld = current != null;

            WorldGen.gen = true;
            WorldGen.loadFailed = false;
            WorldGen.loadSuccess = false;

            Main.rand = new UnifiedRandom((int)DateTime.Now.Ticks);

            bool cloud = root.IsCloudSave;
            string path = isSubworld ? CurrentPath : root.Path;

            cache?.OnUnload();

            if (!isSubworld || current.HowSaveWorld != Subworld.SaveSetting.NoSave)
            {
                if (!isSubworld)
                {
                    Main.ActiveWorldFileData = root;
                }

                TryLoadWorldFile(path, cloud, 0);
            }

            if (isSubworld)
            {
                Main.worldName = Language.GetTextValue("Mods.Everglow.Subworld.Name" + current.Name);
                if (WorldGen.loadFailed)
                {
                    Everglow.Instance.Logger.Warn("Failed to load \"" + Main.worldName + (WorldGen.worldBackup ? "\" from file" : "\" from file, no backup"));
                }
                if (!WorldGen.loadSuccess)
                {
                    LoadSubworld(path, cloud);
                }
                current.OnLoad();
            }
            else if (!WorldGen.loadSuccess)
            {
                Everglow.Instance.Logger.Error("Failed to load \"" + root.Name + (WorldGen.worldBackup ? "\" from file" : "\" from file, no backup"));
                Main.menuMode = 0;
                if (Main.netMode == NetmodeID.Server)
                {
                    Netplay.Disconnect = true;
                }
                return;
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
                    Main.statusText = Lang.gen[68].Value + " " + (int)((float)Main.loadMapLastX / Main.maxTilesX * 100 + 1) + "%";
                    Thread.Sleep(0);
                }

                Player player = Main.LocalPlayer;
                if (Main.anglerWhoFinishedToday.Contains(player.name))
                {
                    Main.anglerQuestFinished = true;
                }
                player.Spawn(PlayerSpawnContext.SpawningIntoWorld);
                Main.ActivePlayerFileData.StartPlayTimer();
                Player.Hooks.EnterWorld(Main.myPlayer);
                Main.resetClouds = true;
                Main.gameMenu = false;
            }
        }
        static void LoadSubworld(string path, bool cloud)
        {
            WorldFileData data = new WorldFileData(path, cloud)
            {
                Name = Main.worldName,
                GameMode = Main.GameMode,
                CreationTime = DateTime.Now,
                Metadata = FileMetadata.FromCurrentSettings(FileType.World),
                WorldGeneratorVersion = Main.WorldGeneratorVersion
            };
            data.SetSeed(root.SeedText);
            using (MD5 md5 = MD5.Create())
            {
                data.UniqueId = new Guid(md5.ComputeHash(Encoding.ASCII.GetBytes(CurrentPath)));
            }
            Main.ActiveWorldFileData = data;

            Main.maxTilesX = current.Width;
            Main.maxTilesY = current.Height;
            Main.spawnTileX = Main.maxTilesX / 2;
            Main.spawnTileY = Main.maxTilesY / 2;
            WorldGen.setWorldSize();
            WorldGen.clearWorld();
            Main.worldSurface = Main.maxTilesY * 0.3;
            Main.rockLayer = Main.maxTilesY * 0.5;
            WorldGen.waterLine = Main.maxTilesY;
            Main.weatherCounter = int.MaxValue;
            Cloud.resetClouds();

            float weight = 0;
            for (int i = 0; i < current.Tasks.Count; i++)
            {
                weight += current.Tasks[i].Weight;
            }
            WorldGenerator.CurrentGenerationProgress = new GenerationProgress
            {
                TotalWeight = weight
            };

            WorldGenConfiguration config = current.Config;

            for (int i = 0; i < current.Tasks.Count; i++)
            {
                WorldGen._genRand = new UnifiedRandom(data.Seed);
                Main.rand = new UnifiedRandom(data.Seed);

                GenPass task = current.Tasks[i];

                WorldGenerator.CurrentGenerationProgress.Start(task.Weight);
                task.Apply(WorldGenerator.CurrentGenerationProgress, config?.GetPassConfiguration(task.Name));
                WorldGenerator.CurrentGenerationProgress.End();
            }
            WorldGenerator.CurrentGenerationProgress = null;

            SystemLoader.OnWorldLoad();
        }
        static void TryLoadWorldFile(string path, bool cloud, int tries)
        {
            LoadWorldFile(path, cloud);
            if (WorldGen.loadFailed)
            {
                if (tries == 1)
                {
                    if (FileUtilities.Exists(path + ".bak", cloud))
                    {
                        WorldGen.worldBackup = false;

                        FileUtilities.Copy(path, path + ".bad", cloud);
                        FileUtilities.Copy(path + ".bak", path, cloud);
                        FileUtilities.Delete(path + ".bak", cloud);

                        string tMLPath = Path.ChangeExtension(path, ".twld");
                        if (FileUtilities.Exists(tMLPath, cloud))
                        {
                            FileUtilities.Copy(tMLPath, tMLPath + ".bad", cloud);
                        }
                        if (FileUtilities.Exists(tMLPath + ".bak", cloud))
                        {
                            FileUtilities.Copy(tMLPath + ".bak", tMLPath, cloud);
                            FileUtilities.Delete(tMLPath + ".bak", cloud);
                        }
                    }
                    else
                    {
                        WorldGen.worldBackup = false;
                        return;
                    }
                }
                else if (tries == 3)
                {
                    FileUtilities.Copy(path, path + ".bak", cloud);
                    FileUtilities.Copy(path + ".bad", path, cloud);
                    FileUtilities.Delete(path + ".bad", cloud);

                    string tMLPath = Path.ChangeExtension(path, ".twld");
                    if (FileUtilities.Exists(tMLPath, cloud))
                    {
                        FileUtilities.Copy(tMLPath, tMLPath + ".bak", cloud);
                    }
                    if (FileUtilities.Exists(tMLPath + ".bad", cloud))
                    {
                        FileUtilities.Copy(tMLPath + ".bad", tMLPath, cloud);
                        FileUtilities.Delete(tMLPath + ".bad", cloud);
                    }

                    return;
                }
                TryLoadWorldFile(path, cloud, tries++);
            }
        }
        static void LoadWorldFile(string path, bool cloud)
        {
            bool flag = cloud && SocialAPI.Cloud != null;
            if (!FileUtilities.Exists(path, flag))
            {
                return;
            }

            if (current != null)
            {
                WorldFileData data = WorldFile.GetAllMetadata(path, cloud);
                if (data != null)
                {
                    Main.ActiveWorldFileData = data;
                }
            }

            using MemoryStream stream = new MemoryStream(FileUtilities.ReadAllBytes(path, flag));
            using BinaryReader reader = new BinaryReader(stream);

            try
            {
                int status = WorldFile.LoadWorld_Version2(reader);
                reader.Close();
                stream.Close();
                SystemLoader.OnWorldLoad();
                typeof(ModLoader).Assembly.GetType("Terraria.ModLoader.IO.WorldIO").GetMethod("Load", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { path, flag });
                if (status != 0)
                {
                    WorldGen.loadFailed = true;
                    WorldGen.loadSuccess = false;
                    return;
                }
                WorldGen.loadSuccess = true;
                WorldGen.loadFailed = false;
                WorldGen.waterLine = Main.maxTilesY;
                Liquid.QuickWater(2);
                WorldGen.WaterCheck();
                Liquid.quickSettle = true;
                int updates = 0;
                int amount = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                float num = 0;
                while (Liquid.numLiquid > 0 && updates < 100000)
                {
                    updates++;
                    float progress = (amount - Liquid.numLiquid + LiquidBuffer.numLiquidBuffer) / (float)amount;
                    if (Liquid.numLiquid + LiquidBuffer.numLiquidBuffer > amount)
                    {
                        amount = Liquid.numLiquid + LiquidBuffer.numLiquidBuffer;
                    }
                    if (progress > num)
                    {
                        num = progress;
                    }
                    else
                    {
                        progress = num;
                    }
                    Main.statusText = Lang.gen[27].Value + " " + (int)(progress * 100 / 2 + 50) + "%";
                    Liquid.UpdateLiquid();
                }
                Liquid.quickSettle = false;
                Main.weatherCounter = int.MaxValue;
                Cloud.resetClouds();
                WorldGen.WaterCheck();
                if (Main.slimeRainTime > 0)
                {
                    Main.StartSlimeRain(false);
                }
                WorldFile.SetOngoingToTemps();
            }
            catch
            {
                WorldGen.loadFailed = true;
                WorldGen.loadSuccess = false;
                try
                {
                    reader.Close();
                    stream.Close();
                }
                catch
                {
                }
            }
        }
        static void BeginEntering(Subworld target)
        {
            if (Main.netMode == NetmodeID.SinglePlayer)
            {
                if (current == null)
                {
                    root = Main.ActiveWorldFileData;
                }
                current = target;
                Task.Factory.StartNew(ExitWorldCallBack);
            }
            else if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                //TODO:联机
                //ModPacket packet = ModContent.GetInstance<SubworldLibrary>().GetPacket();
                //packet.Write((byte)0);
                //packet.Write((ushort)index);
                //packet.Send();
            }
        }
        static void ExitWorldCallBack()
        {
            int netMode = Main.netMode;

            if (netMode != NetmodeID.Server)
            {
                cache?.OnExit();

                if (netMode == 0)
                {
                    WorldFile.CacheSaveTime();
                }

                Main.invasionProgress = -1;
                Main.invasionProgressDisplayLeft = 0;
                Main.invasionProgressAlpha = 0;
                Main.invasionProgressIcon = 0;

                current?.OnEnter();

                Main.gameMenu = true;

                SoundEngine.StopTrackedSounds();
                CaptureInterface.ResetFocus();

                Main.ActivePlayerFileData.StopPlayTimer();
                Player.SavePlayer(Main.ActivePlayerFileData);
                Player.ClearPlayerTempInfo();

                Rain.ClearRain();
            }

            if (netMode != NetmodeID.MultiplayerClient)
            {
                WorldFile.SaveWorld();
            }
            SystemLoader.OnWorldUnload();

            Main.fastForwardTime = false;
            Main.UpdateTimeRate();
            WorldGen.noMapUpdate = true;

            if (cache != null && !cache.SavePlayer && netMode != NetmodeID.Server)
            {
                PlayerFileData playerData = Player.GetFileData(Main.ActivePlayerFileData.Path, Main.ActivePlayerFileData.IsCloudSave);
                if (playerData != null)
                {
                    playerData.Player.whoAmI = Main.myPlayer;
                    playerData.SetAsActive();
                }
            }

            if (netMode != NetmodeID.MultiplayerClient)
            {
                LoadWorld();
            }
            else
            {
                NetMessage.SendData(MessageID.Hello);
            }
        }
        public static void DeleteFileData<T>() where T : Subworld
        {
            Subworld c = current;
            current = ModContent.GetInstance<T>();
            if (current.HowSaveWorld != Subworld.SaveSetting.Public)
            {
                return;
            }
            string path = CurrentPath;
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            current = c;
        }
        public static string GetFilePath<T>() where T : Subworld
        {
            Subworld c = current;
            current = ModContent.GetInstance<T>();
            if (current.HowSaveWorld != Subworld.SaveSetting.Public)
            {
                return string.Empty;
            }
            string path = CurrentPath;
            current = c;
            return path;
        }
        #region 子世界的WorldSystem
        public override void OnWorldLoad()
            => current?.WorldSystem?.OnWorldLoad();
        public override void OnWorldUnload()
            => current?.WorldSystem?.OnWorldUnload();
        public override void ModifyScreenPosition()
            => current?.WorldSystem?.ModifyScreenPosition();
        public override void ModifyTransformMatrix(ref SpriteViewMatrix Transform)
            => current?.WorldSystem?.ModifyTransformMatrix(ref Transform);
        public override void UpdateUI(GameTime gameTime)
            => current?.WorldSystem?.UpdateUI(gameTime);
        public override void PreUpdateEntities()
            => current?.WorldSystem?.PreUpdateEntities();
        public override void PreUpdatePlayers()
            => current?.WorldSystem?.PreUpdatePlayers();
        public override void PostUpdatePlayers()
            => current?.WorldSystem?.PostUpdatePlayers();
        public override void PreUpdateNPCs()
            => current?.WorldSystem?.PreUpdateNPCs();
        public override void PostUpdateNPCs()
            => current?.WorldSystem?.PostUpdateNPCs();
        public override void PreUpdateGores()
            => current?.WorldSystem?.PreUpdateGores();
        public override void PostUpdateGores()
            => current?.WorldSystem?.PostUpdateGores();
        public override void PreUpdateProjectiles()
            => current?.WorldSystem?.PreUpdateProjectiles();
        public override void PostUpdateProjectiles()
            => current?.WorldSystem?.PostUpdateProjectiles();
        public override void PreUpdateItems()
            => current?.WorldSystem?.PreUpdateItems();
        public override void PostUpdateItems()
            => current?.WorldSystem?.PostUpdateItems();
        public override void PreUpdateDusts()
            => current?.WorldSystem?.PreUpdateDusts();
        public override void PostUpdateDusts()
            => current?.WorldSystem?.PostUpdateDusts();
        public override void PreUpdateTime()
            => current?.WorldSystem?.PreUpdateTime();
        public override void PostUpdateTime()
            => current?.WorldSystem?.PostUpdateTime();
        public override void PreUpdateWorld()
            => current?.WorldSystem?.PreUpdateWorld();
        public override void PostUpdateWorld()
            => current?.WorldSystem?.PostUpdateWorld();
        public override void PreUpdateInvasions()
            => current?.WorldSystem?.PreUpdateInvasions();
        public override void PostUpdateInvasions()
            => current?.WorldSystem?.PostUpdateInvasions();
        public override void PostUpdateEverything()
            => current?.WorldSystem?.PostUpdateEverything();
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
            => current?.WorldSystem?.ModifyInterfaceLayers(layers);
        public override void ModifyGameTipVisibility(IReadOnlyList<GameTipData> gameTips)
            => current?.WorldSystem?.ModifyGameTipVisibility(gameTips);
        public override void PostDrawInterface(SpriteBatch spriteBatch)
            => current?.WorldSystem?.PostDrawInterface(spriteBatch);
        public override void PreDrawMapIconOverlay(IReadOnlyList<IMapLayer> layers, MapOverlayDrawContext mapOverlayDrawContext)
            => current?.WorldSystem?.PreDrawMapIconOverlay(layers, mapOverlayDrawContext);
        public override void PostDrawFullscreenMap(ref string mouseText)
            => current?.WorldSystem?.PostDrawFullscreenMap(ref mouseText);
        public override void PostUpdateInput()
            => current?.WorldSystem?.PostUpdateInput();
        public override void PreSaveAndQuit()
            => current?.WorldSystem?.PreSaveAndQuit();
        public override void PostDrawTiles()
            => current?.WorldSystem?.PostDrawTiles();
        public override void ModifyTimeRate(ref double timeRate, ref double tileUpdateRate, ref double eventUpdateRate)
            => current?.WorldSystem?.ModifyTimeRate(ref timeRate, ref tileUpdateRate, ref eventUpdateRate);
        public override void SaveWorldData(TagCompound tag)
            => current?.WorldSystem?.SaveWorldData(tag);
        public override void LoadWorldData(TagCompound tag)
            => current?.WorldSystem?.LoadWorldData(tag);
        public override void NetSend(BinaryWriter writer)
            => current?.WorldSystem?.NetSend(writer);
        public override void NetReceive(BinaryReader reader)
            => current?.WorldSystem?.NetReceive(reader);
        public override bool HijackGetData(ref byte messageType, ref BinaryReader reader, int playerNumber)
            => current?.WorldSystem?.HijackGetData(ref messageType, ref reader, playerNumber) ?? false;
        public override bool HijackSendData(int whoAmI, int msgType, int remoteClient, int ignoreClient, NetworkText text, int number, float number2, float number3, float number4, int number5, int number6, int number7)
            => current?.WorldSystem?.HijackSendData(whoAmI, msgType, remoteClient, ignoreClient, text, number, number2, number3, number4, number5, number6, number7) ?? false;
        public override void ResetNearbyTileEffects()
            => current?.WorldSystem?.ResetNearbyTileEffects();
        public override void ModifyHardmodeTasks(List<GenPass> list)
            => current?.WorldSystem?.ModifyHardmodeTasks(list);
        public override void ModifySunLightColor(ref Color tileColor, ref Color backgroundColor)
            => current?.WorldSystem?.ModifySunLightColor(ref tileColor, ref backgroundColor);
        public override void ModifyLightingBrightness(ref float scale)
            => current?.WorldSystem?.ModifyLightingBrightness(ref scale);
        public override void TileCountsAvailable(ReadOnlySpan<int> tileCounts)
            => current?.WorldSystem?.TileCountsAvailable(tileCounts);
        #endregion
    }
}
