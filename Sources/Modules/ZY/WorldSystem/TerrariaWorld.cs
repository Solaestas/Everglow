using Terraria.IO;

namespace Everglow.Sources.Modules.ZY.WorldSystem
{
    internal class TerrariaWorld : World
    {
        public TerrariaWorld()
        {
        }

        public TerrariaWorld(WorldFileData data) : base(data)
        {
        }

        public override string WorldName => "Terraria";

        public override uint Version => (uint)(Main.WorldGeneratorVersion & 0x00_00_00_00_FF_FF_FF_FFul);

        public override void GenerateWorld()
        {
            WorldGen.GenerateWorld(data.Seed);
        }
    }
}
