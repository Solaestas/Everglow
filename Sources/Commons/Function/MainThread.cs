using Everglow.Sources.Commons.Core.ModuleSystem;

namespace Everglow.Sources.Commons.Function
{
    internal class MainThread : IModule
    {
        private static object m_lock = new object();
        private List<Action> tasks = new List<Action>();
        public string Name => "MainThread";
        public void Load()
        {
            On.Terraria.Main.Update += Main_Update;
        }

        public void Unload()
        {
            tasks = null;
        }
        public static void Add(Action task)
        {
            lock (m_lock)
            {
                Everglow.ModuleManager.GetModule<MainThread>().tasks.Add(task);
            }
        }
        public static Color[] DelayGetColorArray(Texture2D texture)
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
            lock (m_lock)
            {
                foreach (var task in tasks)
                {
                    try
                    {
                        task.Invoke();
                    }
                    catch (Exception ex)
                    {
                        throw;
                    }
                }
                tasks.Clear();
            }
            orig(self, gameTime);
        }
    }
}
