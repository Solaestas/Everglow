using Everglow.Sources.Commons.ModuleSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Terraria.GameContent.UI.Elements;
using Terraria.IO;
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
        public virtual Asset<Texture2D> WorldIcon => ModContent.Request<Texture2D>("Terraria/Images/UI/Icon" + (data.IsHardMode ? "Hallow" : "") + (data.HasCorruption ? "Corruption" : "Crimson"));

        public string Name => WorldName;

        public string Description => WorldName;

        public static Dictionary<ulong, World> Worlds = new Dictionary<ulong, World>();
        public World()
        {

        }
        public World(WorldFileData data)
        {
            this.data = data;
            size.X = data.WorldSizeX;
            size.Y = data.WorldSizeY;
        }

        public abstract void GenerateWorld();
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
        public virtual void SetBaseWorldData()
        {
            Main.maxTilesX = data.WorldSizeX;
            Main.maxTilesY = data.WorldSizeY;
            Main.mapMaxX = data.WorldSizeX;
            Main.mapMaxY = data.WorldSizeY;
            Main.worldSurface = data.WorldSizeY / 3;
            Main.undergroundBackground = data.WorldSizeY / 2;
        }
        public virtual void EnterWorld(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.ActiveWorldFileData = data;
            Main.WorldFileMetadata = FileMetadata.FromCurrentSettings(FileType.World);
            Main.worldName = Main.ActiveWorldFileData.Name;
            SetBaseWorldData();
            Main.menuMode = 10;
            if (!File.Exists(data.Path))
            {
                WorldGen.clearWorld();
                //TODO Hjson
                Main.statusText = "开始创建世界";
                GenerateWorld();
                Main.spawnTileX = DefaultSpawnPoint.X;
                Main.spawnTileY = DefaultSpawnPoint.Y;
                WorldFile.SaveWorld();
            }
            //WorldFile.LoadWorld(false);
            Main.statusText = "正在进入世界";
            WorldGen.playWorld();
        }
        public static string GetWorldName(WorldFileData data)
        {
            uint mcode = (uint)(data.WorldGeneratorVersion >> 32);
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

        public static uint GetWorldVersion(WorldFileData data)
        {
            return (uint)(data.WorldGeneratorVersion & 0xFF_FF_FF_FF);
        }
        public void Load()
        {
            Worlds.Add(FileVersion, this);
        }

        public void Unload()
        {
            Worlds = null;
        }
    }
}
