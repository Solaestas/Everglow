using Terraria.IO;

namespace Everglow.Sources.Modules.ZY.WorldSystem
{
    internal class Elsewhere : World
    {
        public Elsewhere()
        {
        }

        public Elsewhere(WorldFileData data) : base(data)
        {
        }

        public override string WorldName => "Elsewhere";

        public override uint Version => 1;

        public override Asset<Texture2D> WorldIcon 
            => ModContent.Request<Texture2D>("Everglow/Sources/Modules/ZY/WorldSystem/IconOcean", 
                AssetRequestMode.ImmediateLoad);
        public override void GenerateWorld()
        {

        }
    }
}