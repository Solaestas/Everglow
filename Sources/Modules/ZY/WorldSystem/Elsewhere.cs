using ReLogic.Content;
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
        //在Load时给WorldSystem里添加一个假的世界
        public override void Load()
        {
            Everglow.ModuleManager.GetModule<WorldSystem>().AddWorld(CreateInstance(WorldName), "Elsewhere", "Elsewhere", 3, "Test");
        }
    }
}