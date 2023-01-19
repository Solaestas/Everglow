using Everglow.Sources.Commons.Function.NetUtils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria.GameContent.Events;
using Terraria.IO;
using Terraria.ModLoader.IO;
using Terraria.Utilities;

namespace Everglow.Sources.Modules.WorldModule
{
    /// <summary>
    /// public可读写,internal许读不许写,private不许读写
    /// </summary>
    internal static class WorldManager
    {
        public static bool AnyTryEnterInProgress { get; internal set; }
        static Dictionary<string, World> worlddic = new();
        /// <summary>
        /// activing不为null时此项不应为null
        /// </summary>
        internal static WorldHistory currenthistory;
        /// <summary>
        /// 当前处于的世界
        /// </summary>
        internal static World activing;
        static TagCompound cacheDatas = new();
        static MethodInfo InnerSaveWorld;
        public static bool Activing<T>() where T : World
        {
            return activing is not null && activing.GetType().TypeHandle.Equals(typeof(T).TypeHandle);
        }
        internal static void Register(World world)
        {
            ModTypeLookup<World>.Register(world);
            worlddic[world.FullName] = world;
        }
        public static bool TryGetCache<T>(string key, out T data) => cacheDatas.TryGet(key, out data);
        internal static ProgressToken TryEnter(World world, Action<ProgressToken.State> whenInvalid = null)
        {
            ProgressToken token = new ProgressToken();
            if (whenInvalid is not null)
            {
                token.WhenInvalid += whenInvalid;
            }
            if (!AnyTryEnterInProgress)
            {
                try
                {
                    AnyTryEnterInProgress = true;
                    Task.Factory.StartNew(() => Enter(world, token));
                }
                catch (Exception e)
                {
                    token.Exception(e);
                }
            }
            else
            {
                token.StopByOther("There is a world occupied in processing");
            }
            return token;
        }
        internal static ProgressToken TryEnter<T>(Action<ProgressToken.State> whenInvalid = null) where T : World
        {
            ProgressToken token = new ProgressToken();
            if (whenInvalid is not null)
            {
                token.WhenInvalid += whenInvalid;
            }
            if (!AnyTryEnterInProgress)
            {
                try
                {
                    if (worlddic.TryGetValue(ModContent.GetInstance<T>().FullName, out World target))
                    {
                        AnyTryEnterInProgress = true;
                        Task.Factory.StartNew(() => Enter(target, token));
                    }
                    else
                    {
                        token.Exception(new KeyNotFoundException(ModContent.GetInstance<T>().FullName));
                    }
                }
                catch (Exception e)
                {
                    token.Exception(e);
                }
            }
            else
            {
                token.StopByOther("There is a world occupied in processing");
            }
            return token;
        }
        static void Enter(World world, ProgressToken token)
        {
            token.StartRun();
            try
            {
                Main.gameMenu = true;
                Main.statusText = $"Try enter {world.FullName}";
                currenthistory ??= new(Main.ActiveWorldFileData);
                currenthistory.buffer = world;
                if (NetUtils.IsSingle)
                {
                    Load_SinglePlayer(currenthistory, token);
                }
                else
                {
                    CallServerToStartNew(currenthistory, token);
                }
            }
            catch when (token.IsCancelled)
            {
                TryBack();
                //TODO
                //倒序依次尝试返回History
                //仍然失败返回主世界
                //仍然失败返回选择世界页面
            }
            catch (Exception e)
            {
                token.Exception(e);
                TryBack();
                //TODO
                //倒序依次尝试返回History
                //仍然失败返回主世界
                //仍然失败返回选择世界页面
            }
            AnyTryEnterInProgress = false;
        }
        static void WriteCache()
        {
            cacheDatas["BirthdayParty.CelebratingNPCs"] = BirthdayParty.CelebratingNPCs;
            cacheDatas["BirthdayParty.GenuineParty"] = BirthdayParty.GenuineParty;
            cacheDatas["BirthdayParty.ManualParty"] = BirthdayParty.ManualParty;
            cacheDatas["BirthdayParty.PartyDaysOnCooldown"] = BirthdayParty.PartyDaysOnCooldown;
            cacheDatas["CultistRitual.delay"] = CultistRitual.delay;
            cacheDatas["DD2Event.DownedInvasionT1"] = DD2Event.DownedInvasionT1;
            cacheDatas["DD2Event.DownedInvasionT2"] = DD2Event.DownedInvasionT2;
            cacheDatas["DD2Event.DownedInvasionT3"] = DD2Event.DownedInvasionT3;
            cacheDatas["LanternNight.GenuineLanterns"] = LanternNight.GenuineLanterns;
            cacheDatas["LanternNight.LanternNightsOnCooldown"] = LanternNight.LanternNightsOnCooldown;
            cacheDatas["LanternNight.ManualLanterns"] = LanternNight.ManualLanterns;
            cacheDatas["LanternNight.NextNightIsLanternNight"] = LanternNight.NextNightIsLanternNight;
            cacheDatas["Main.anglerQuest"] = Main.anglerQuest < 41 ? Main.anglerQuest : 0;
            cacheDatas["Main.anglerWhoFinishedToday"] = Main.anglerWhoFinishedToday;
            cacheDatas["Main.bloodMoon"] = Main.bloodMoon;
            cacheDatas["Main.bottomWorld"] = Main.bottomWorld;
            cacheDatas["Main.caveBackStyle"] = Main.caveBackStyle;
            cacheDatas["Main.caveBackX"] = Main.caveBackX;
            cacheDatas["Main.cloudBGActive"] = Main.cloudBGActive;
            cacheDatas["Main.dayTime"] = Main.dayTime;
            cacheDatas["Main.dontStarveWorld"] = Main.dontStarveWorld;
            cacheDatas["Main.drunkWorld"] = Main.drunkWorld;
            cacheDatas["Main.dungeonX"] = Main.dungeonX;
            cacheDatas["Main.dungeonY"] = Main.dungeonY;
            cacheDatas["Main.eclipse"] = Main.eclipse;
            cacheDatas["Main.fastForwardTime"] = Main.fastForwardTime;
            cacheDatas["Main.forceHalloweenForToday"] = Main.forceHalloweenForToday;
            cacheDatas["Main.forceXMasForToday"] = Main.forceXMasForToday;
            cacheDatas["Main.GameMode"] = Main.GameMode;
            cacheDatas["Main.getGoodWorld"] = Main.getGoodWorld;
            cacheDatas["Main.hardMode"] = Main.hardMode;
            cacheDatas["Main.hellBackStyle"] = Main.hellBackStyle;
            cacheDatas["Main.iceBackStyle"] = Main.iceBackStyle;
            cacheDatas["Main.invasionDelay"] = Main.invasionDelay;
            cacheDatas["Main.invasionSize"] = Main.invasionSize;
            cacheDatas["Main.invasionSizeStart"] = Main.invasionSizeStart;
            cacheDatas["Main.invasionType"] = Main.invasionType;
            cacheDatas["Main.invasionX"] = Main.invasionX;
            cacheDatas["Main.jungleBackStyle"] = Main.jungleBackStyle;
            cacheDatas["Main.leftWorld"] = Main.leftWorld;
            cacheDatas["Main.maxRain"] = Main.maxRain;
            cacheDatas["Main.maxTilesX"] = Main.maxTilesX;
            cacheDatas["Main.maxTilesY"] = Main.maxTilesY;
            cacheDatas["Main.moonPhase"] = Main.moonPhase;
            cacheDatas["Main.moonType"] = Main.moonType;
            cacheDatas["Main.notTheBeesWorld"] = Main.notTheBeesWorld;
            cacheDatas["Main.numClouds"] = Main.numClouds;
            cacheDatas["Main.raining"] = Main.raining;
            cacheDatas["Main.rainTime"] = Main.rainTime;
            cacheDatas["Main.rightWorld"] = Main.rightWorld;
            cacheDatas["Main.rockLayer"] = Main.rockLayer;
            cacheDatas["Main.slimeRainTime"] = Main.slimeRainTime;
            cacheDatas["Main.spawnTileX"] = Main.spawnTileX;
            cacheDatas["Main.spawnTileY"] = Main.spawnTileY;
            cacheDatas["Main.sundialCooldown"] = Main.sundialCooldown;
            cacheDatas["Main.tenthAnniversaryWorld"] = Main.tenthAnniversaryWorld;
            cacheDatas["Main.time"] = Main.time;
            cacheDatas["Main.topWorld"] = Main.topWorld;
            cacheDatas["Main.treeStyle"] = Main.treeStyle;
            cacheDatas["Main.treeX"] = Main.treeX;
            cacheDatas["Main.windSpeedTarget"] = Main.windSpeedTarget;
            cacheDatas["Main.worldID"] = Main.worldID;
            cacheDatas["Main.worldName"] = Main.worldName;
            cacheDatas["Main.worldSurface"] = Main.worldSurface;
            cacheDatas["NPC.boughtBunny"] = NPC.boughtBunny;
            cacheDatas["NPC.boughtCat"] = NPC.boughtCat;
            cacheDatas["NPC.boughtDog"] = NPC.boughtDog;
            cacheDatas["NPC.combatBookWasUsed"] = NPC.combatBookWasUsed;
            cacheDatas["NPC.downedAncientCultist"] = NPC.downedAncientCultist;
            cacheDatas["NPC.downedBoss1"] = NPC.downedBoss1;
            cacheDatas["NPC.downedBoss2"] = NPC.downedBoss2;
            cacheDatas["NPC.downedBoss3"] = NPC.downedBoss3;
            cacheDatas["NPC.downedChristmasIceQueen"] = NPC.downedChristmasIceQueen;
            cacheDatas["NPC.downedChristmasSantank"] = NPC.downedChristmasSantank;
            cacheDatas["NPC.downedChristmasTree"] = NPC.downedChristmasTree;
            cacheDatas["NPC.downedClown"] = NPC.downedClown;
            cacheDatas["NPC.downedDeerclops"] = NPC.downedDeerclops;
            cacheDatas["NPC.downedEmpressOfLight"] = NPC.downedEmpressOfLight;
            cacheDatas["NPC.downedFishron"] = NPC.downedFishron;
            cacheDatas["NPC.downedFrost"] = NPC.downedFrost;
            cacheDatas["NPC.downedGoblins"] = NPC.downedGoblins;
            cacheDatas["NPC.downedGolemBoss"] = NPC.downedGolemBoss;
            cacheDatas["NPC.downedHalloweenKing"] = NPC.downedHalloweenKing;
            cacheDatas["NPC.downedHalloweenTree"] = NPC.downedHalloweenTree;
            cacheDatas["NPC.downedMartians"] = NPC.downedMartians;
            cacheDatas["NPC.downedMechBoss1"] = NPC.downedMechBoss1;
            cacheDatas["NPC.downedMechBoss2"] = NPC.downedMechBoss2;
            cacheDatas["NPC.downedMechBoss3"] = NPC.downedMechBoss3;
            cacheDatas["NPC.downedMechBossAny"] = NPC.downedMechBossAny;
            cacheDatas["NPC.downedMoonlord"] = NPC.downedMoonlord;
            cacheDatas["NPC.downedPirates"] = NPC.downedPirates;
            cacheDatas["NPC.downedPlantBoss"] = NPC.downedPlantBoss;
            cacheDatas["NPC.downedQueenBee"] = NPC.downedQueenBee;
            cacheDatas["NPC.downedQueenSlime"] = NPC.downedQueenSlime;
            cacheDatas["NPC.downedSlimeKing"] = NPC.downedSlimeKing;
            cacheDatas["NPC.downedTowerNebula"] = NPC.downedTowerNebula;
            cacheDatas["NPC.downedTowerSolar"] = NPC.downedTowerSolar;
            cacheDatas["NPC.downedTowerStardust"] = NPC.downedTowerStardust;
            cacheDatas["NPC.downedTowerVortex"] = NPC.downedTowerVortex;
            cacheDatas["NPC.killCount"] = NPC.killCount;
            cacheDatas["NPC.LunarApocalypseIsUp"] = NPC.LunarApocalypseIsUp;
            cacheDatas["NPC.savedAngler"] = NPC.savedAngler;
            cacheDatas["NPC.savedBartender"] = NPC.savedBartender;
            cacheDatas["NPC.savedGoblin"] = NPC.savedGoblin;
            cacheDatas["NPC.savedGolfer"] = NPC.savedGolfer;
            cacheDatas["NPC.savedMech"] = NPC.savedMech;
            cacheDatas["NPC.savedStylist"] = NPC.savedStylist;
            cacheDatas["NPC.savedTaxCollector"] = NPC.savedTaxCollector;
            cacheDatas["NPC.savedWizard"] = NPC.savedWizard;
            cacheDatas["NPC.TowerActiveNebula"] = NPC.TowerActiveNebula;
            cacheDatas["NPC.TowerActiveSolar"] = NPC.TowerActiveSolar;
            cacheDatas["NPC.TowerActiveStardust"] = NPC.TowerActiveStardust;
            cacheDatas["NPC.TowerActiveVortex"] = NPC.TowerActiveVortex;
            cacheDatas["Sandstorm.Happening"] = Sandstorm.Happening;
            cacheDatas["Sandstorm.IntendedSeverity"] = Sandstorm.IntendedSeverity;
            cacheDatas["Sandstorm.Severity"] = Sandstorm.Severity;
            cacheDatas["Sandstorm.TimeLeft"] = Sandstorm.TimeLeft;
            cacheDatas["WorldFileData.CreationTime"] = Main.ActiveWorldFileData.CreationTime.ToBinary();
            cacheDatas["WorldFileData.SeedText"] = Main.ActiveWorldFileData.SeedText;
            cacheDatas["WorldFileData.UniqueId"] = Main.ActiveWorldFileData.UniqueId.ToByteArray();
            cacheDatas["WorldFileData.WorldGeneratorVersion"] = Main.ActiveWorldFileData.WorldGeneratorVersion;
            cacheDatas["WorldGen.altarCount"] = WorldGen.altarCount;
            cacheDatas["WorldGen.corruptBG"] = WorldGen.corruptBG;
            cacheDatas["WorldGen.crimson"] = WorldGen.crimson;
            cacheDatas["WorldGen.crimsonBG"] = WorldGen.crimsonBG;
            cacheDatas["WorldGen.desertBG"] = WorldGen.desertBG;
            cacheDatas["WorldGen.hallowBG"] = WorldGen.hallowBG;
            cacheDatas["WorldGen.jungleBG"] = WorldGen.jungleBG;
            cacheDatas["WorldGen.mushroomBG"] = WorldGen.mushroomBG;
            cacheDatas["WorldGen.oceanBG"] = WorldGen.oceanBG;
            cacheDatas["WorldGen.SavedOreTiers.Adamantite"] = WorldGen.SavedOreTiers.Adamantite;
            cacheDatas["WorldGen.SavedOreTiers.Cobalt"] = WorldGen.SavedOreTiers.Cobalt;
            cacheDatas["WorldGen.SavedOreTiers.Copper"] = WorldGen.SavedOreTiers.Copper;
            cacheDatas["WorldGen.SavedOreTiers.Gold"] = WorldGen.SavedOreTiers.Gold;
            cacheDatas["WorldGen.SavedOreTiers.Iron"] = WorldGen.SavedOreTiers.Iron;
            cacheDatas["WorldGen.SavedOreTiers.Mythril"] = WorldGen.SavedOreTiers.Mythril;
            cacheDatas["WorldGen.SavedOreTiers.Silver"] = WorldGen.SavedOreTiers.Silver;
            cacheDatas["WorldGen.shadowOrbCount"] = WorldGen.shadowOrbCount;
            cacheDatas["WorldGen.shadowOrbSmashed"] = WorldGen.shadowOrbSmashed;
            cacheDatas["WorldGen.snowBG"] = WorldGen.snowBG;
            cacheDatas["WorldGen.spawnMeteor"] = WorldGen.spawnMeteor;
            cacheDatas["WorldGen.treeBG1"] = WorldGen.treeBG1;
            cacheDatas["WorldGen.treeBG2"] = WorldGen.treeBG2;
            cacheDatas["WorldGen.treeBG3"] = WorldGen.treeBG3;
            cacheDatas["WorldGen.treeBG4"] = WorldGen.treeBG4;
            using (MemoryStream memory = new())
            {
                using (BinaryWriter writer = new(memory))
                {
                    WorldGen.TreeTops.Save(writer);
                }
                cacheDatas["WorldGen.TreeTops"] = memory.GetBuffer();
            }
            cacheDatas["WorldGen.underworldBG"] = WorldGen.underworldBG;
        }
        static void Load_SinglePlayer(WorldHistory history, ProgressToken token)
        {
            InnerSaveWorld ??= typeof(WorldFile).GetMethod("InternalSaveWorld", BindingFlags.NonPublic | BindingFlags.Static);
            InnerSaveWorld.Invoke(null, new object[] { history.root.IsCloudSave, false });
            Player.InternalSaveMap(history.root.IsCloudSave);
            typeof(Player).GetMethod("InternalSavePlayerFile", BindingFlags.NonPublic | BindingFlags.Static).Invoke(null, new object[] { Main.ActivePlayerFileData });
            if (history.History.Count == 0)
            {
                WriteCache();
            }
            string bufferpath = history.buffer.GetFilePath(history);
            if (history.buffer.HowSave != World.SaveType.None && FileUtilities.Exists(bufferpath, history.root.IsCloudSave))
            {
                WorldFileData data = WorldFile.GetAllMetadata(bufferpath, history.root.IsCloudSave);
                if (data is null)
                {
                    token.StopByOther($"Can't load meta data from {bufferpath}.");
                }
                if (data.WorldSizeX != history.buffer.Width || data.WorldSizeY != history.buffer.Height)
                {
                    token.Exception(new FileLoadException("Metadata does not match"));
                }
                data.SetAsActive();
                WorldGen.playWorld();
            }
            else
            {
                WorldFileData newdata;
                if (history.buffer.HowSave == World.SaveType.None)
                {
                    newdata = new(null, history.root.IsCloudSave)
                    {
                        Name = $"{history.root.Name}_{history.buffer.Name}_{history.buffer.Name}",
                        GameMode = history.root.GameMode,
                        WorldSizeX = history.buffer.Width,
                        WorldSizeY = history.buffer.Height,
                    };
                }
                else
                {
                    newdata = new(bufferpath, history.root.IsCloudSave)
                    {
                        Name = $"{history.root.Name}_{history.buffer.Name}_{history.buffer.Name}",
                        GameMode = history.root.GameMode,
                        CreationTime = DateTime.Now,
                        Metadata = FileMetadata.FromCurrentSettings(FileType.World),
                        WorldGeneratorVersion = 1065151889409UL,
                        UniqueId = Guid.NewGuid(),
                        WorldSizeX = history.buffer.Width,
                        WorldSizeY = history.buffer.Height,
                    };
                    newdata.SetSeed(history.root.SeedText);
                }
                newdata.SetAsActive();
                WorldGen.clearWorld();
                Main.maxTilesX = history.buffer.Width;
                Main.maxTilesY = history.buffer.Height;
                WorldGen.setWorldSize();
                Main.spawnTileX = Main.maxTilesX / 2;
                Main.spawnTileY = Main.maxTilesY / 2;
                Main.worldSurface = (double)Main.maxTilesY * 0.3;
                Main.rockLayer = (double)Main.maxTilesY * 0.5;
                WorldGen.waterLine = Main.maxTilesY;
                Main.weatherCounter = 18000;
                Cloud.resetClouds();
                history.buffer.CreateWorld(new GameTime());
                InnerSaveWorld.Invoke(null, new object[] { history.root.IsCloudSave, false });
                WorldGen.playWorld();
            }
            history.history.Enqueue(history.buffer);
            activing = history.buffer;
            history.buffer = null;
        }
        static void CallServerToStartNew(WorldHistory history, ProgressToken token)
        {

        }

        public static ProgressToken TryBack(bool exitall = false)
        {
            ProgressToken token = new();
            if (currenthistory is null)
            {
                token.Over();
                AnyTryEnterInProgress = false;
            }
            else
            {
                if (exitall)
                {
                    Task.Factory.StartNew(() =>
                    {
                        Main.gameMenu = true;
                        try
                        {
                            currenthistory.root.SetAsActive();
                            WorldGen.playWorld();
                            currenthistory = null;
                            activing = null;
                            token.Over();
                        }
                        catch when (token.IsCancelled)
                        {
                            Main.menuMode = MenuID.WorldSelect;
                            currenthistory = null;
                            activing = null;
                            //TODO
                        }
                        catch (Exception e)
                        {
                            token.Exception(e);
                            Main.menuMode = MenuID.WorldSelect;
                            currenthistory = null;
                            activing = null;
                            //TODO
                        }
                        AnyTryEnterInProgress = false;
                    });
                }
                else
                {
                    Task.Factory.StartNew(() =>
                    {
                        if (currenthistory.history.TryDequeue(out currenthistory.buffer))
                        {
                            Load_SinglePlayer(currenthistory, token);
                        }
                        else
                        {
                            TryBack(true);
                        }
                        AnyTryEnterInProgress = false;
                    });
                }
            }
            return token;
        }
    }
}