using Everglow.Sources.Commons.ModuleSystem;
using ReLogic.Content;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Terraria.Audio;
using Terraria.GameContent.UI.States;
using Terraria.IO;
using Terraria.Social;
using Terraria.UI;

namespace Everglow.Sources.Modules.ZY.WorldSystem
{
    [ModuleDependency(typeof(WorldSystem))]
    internal abstract class World : IModule
    {
        public WorldFileData data;
        public abstract string WorldName { get; }
        /// <summary>
        /// Code为存档version的高位
        /// </summary>
        public uint WorldNameCode
        {
            get
            {
                byte[] bs = MD5.Create().ComputeHash(WorldName.ToByteArray())[0..4];
                string str = string.Empty;
                foreach (var b in bs)
                {
                    str += b.ToString("x");
                }
                return uint.Parse(str, System.Globalization.NumberStyles.HexNumber);
            }
        }
        public ulong FileVersion
        {
            get
            {
                return (ulong)WorldNameCode << 32 | Version;
            }
        }
        /// <summary>
        /// Version为存档version的低位
        /// </summary>
        public abstract uint Version { get; }
        public virtual Point DefaultSpawnPoint => new Point(data.WorldSizeX / 2, data.WorldSizeY / 2);
        public Point size;
        public bool SinglePlay => data is not null;
        public virtual Asset<Texture2D> WorldIcon =>
            ModContent.Request<Texture2D>("Terraria/Images/UI/Icon" + (data.IsHardMode ? "Hallow" : "") + (data.HasCorruption ? "Corruption" : "Crimson"),
                AssetRequestMode.ImmediateLoad);

        public string Name => WorldName;

        public string Description => WorldName;
        public World()
        {

        }
        public World(WorldFileData data)
        {
            this.data = data;
            size.X = data.WorldSizeX;
            size.Y = data.WorldSizeY;
        }
        public static World CreateInstance(string name)
        {
            foreach (var world in Everglow.ModuleManager.FindModule<World>())
            {
                if (world.WorldName == name)
                {
                    return Activator.CreateInstance(world.GetType()) as World;
                }
            }
            return null;
        }
        /// <summary>
        /// 世界生成，使用Main.status输出进度
        /// </summary>
        public virtual void GenerateWorld()
        {

        }
        /// <summary>
        /// 从服务端调用的世界生成，使用Console输出进度
        /// </summary>
        public virtual void GenerateWorld_Server()
        {

        }
        public virtual WorldFileData CreateMetaData(string displayName, string fileName, int GameMode, string seed)
        {
            string path = $"{Main.SavePath}\\Worlds\\{fileName}.wld";
            WorldFileData data = new WorldFileData(path, false)
            {
                Name = displayName,
                GameMode = GameMode,
                CreationTime = DateTime.Now,
                Metadata = FileMetadata.FromCurrentSettings(FileType.World),
                WorldGeneratorVersion = FileVersion,
                UniqueId = Guid.NewGuid()
            };
            data.SetFavorite(favorite: false);
            data.SetSeed(seed);
            data.SetWorldSize(4200, 1200);
            this.data = data;
            return data;
        }
        public void SetBaseWorldData()
        {
            Main.mapMaxX = Main.maxTilesX = data.WorldSizeX;
            Main.mapMaxY = Main.maxTilesY = data.WorldSizeY;
            Main.worldName = data.Name;
            Main.WorldFileMetadata = data.Metadata;
            Main.worldID = int.Parse(data.UniqueId.ToString().ToUpper()[^5..^1], System.Globalization.NumberStyles.HexNumber);
        }
        /// <summary>
        /// 替代了原版UI中的开始游戏
        /// </summary>
        /// <param name="evt"></param>
        /// <param name="listeningElement"></param>
        public virtual void EnterWorld(UIMouseEvent evt, UIElement listeningElement)
        {
            data.SetAsActive();
            SetBaseWorldData();
            SoundEngine.PlaySound(SoundID.MenuOpen);
            Main.GetInputText("");
            Task.Run(() =>
            {
                //单人游戏
                if (!Main.menuMultiplayer)
                {
                    if (!File.Exists(data.Path))
                    {
                        Main.menuMode = 10;//888是有UI的 10是只有文字的
                        Main.MenuUI.SetState(new UIWorldLoad());
                        WorldGen.clearWorld();
                        //TODO MythMod
                        Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.MythMod.Common.WorldSystem.WorldCreate_Def");
                        Main.spawnTileX = DefaultSpawnPoint.X;
                        Main.spawnTileY = DefaultSpawnPoint.Y;
                        GenerateWorld();
                        WorldFile.SaveWorld();
                    }
                    Main.menuMode = 10;
                    Main.statusText = Terraria.Localization.Language.GetTextValue("Mods.MythMod.Common.WorldSystem.WorldEnter_Def");
                    WorldGen.playWorld();
                }
                else//多人游戏-开服
                {
                    if(SocialAPI.Network != null)
                    {
                        Main.menuMode = 889;
                    }else
                    {
                        Main.menuMode = 30;
                    }
                }
            });
        }
        /// <summary>
        /// 在服务端进入世界前执行的世界生成代码
        /// </summary>
        public virtual void EnterWorld_Server()
        {
            if (!File.Exists(data.Path))
            {
                Console.WriteLine("开始创建世界");
                SetBaseWorldData();
                Main.spawnTileX = DefaultSpawnPoint.X;
                Main.spawnTileY = DefaultSpawnPoint.Y;
                GenerateWorld_Server();
                WorldFile.SaveWorld();
            }
            WorldSystem.CurrentWorld = this;
        }
        public static string GetWorldName(ulong dataVersion)
        {
            uint mcode = (uint)(dataVersion >> 32);
            foreach (var world in Everglow.ModuleManager.FindModule<World>())
            {
                uint code = world.WorldNameCode;
                if (code == mcode)
                {
                    return world.WorldName;
                }
            }
            return "Terraria";
        }

        public static uint GetWorldVersion(ulong dataVersion)
        {
            return (uint)(dataVersion & 0xFF_FF_FF_FF);
        }
        public virtual void Load()
        {

        }
        public virtual void Unload()
        {
        }
    }
}
