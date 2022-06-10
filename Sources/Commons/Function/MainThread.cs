using Everglow.Sources.Commons.Core.ModuleSystem;

namespace Everglow.Sources.Commons.Function
{
    internal class MainThread : IModule
    {
        private Stack<Action> tasks = new Stack<Action>();
        public string Name => "MainThread";
        public void Load()
        {
            On.Terraria.Main.Update += Main_Update;
        }

        public void Unload()
        {
            tasks = null;
        }
        public static void Add(Action task) => Everglow.ModuleManager.GetModule<MainThread>().tasks.Push(task);
        public static Color[] GetColors(Texture2D texture)
        {
            Color[] colors = new Color[texture.Width * texture.Height];
            Add(() => texture.GetData(colors));
            return colors;
        }
        /// <summary>
        /// 挂在Update上保证服务端也能执行
        /// </summary>
        /// <param name="orig"></param>
        /// <param name="self"></param>
        /// <param name="gameTime"></param>
        private void Main_Update(On.Terraria.Main.orig_Update orig, Main self, GameTime gameTime)
        {
            if (tasks.Count != 0)
            {
                tasks.Pop().Invoke();
            }
            orig(self, gameTime);
        }
    }
}
