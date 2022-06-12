using Everglow.Sources.Commons.Core.ModuleSystem;

namespace Everglow.Sources.Commons.Function;

internal class Future<T>
{
    private bool isLoaded;
    private T value;
    public T Value
    {
        get
        {
            if(!IsLoaded)
            {
                throw new InvalidOperationException("Hasn't been Loaded");
            }
            return value;
        }
        set
        {
            if(IsLoaded)
            {
                throw new InvalidOperationException("Has been Loaded");
            }
            isLoaded = true;
            this.value = value;
        }
    }
    public bool IsLoaded => isLoaded;
}
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
    public static void Call(Action task)
    {
        Everglow.ModuleManager.GetModule<MainThread>().tasks.Push(task);
    }
    public static Future<Color[]> GetColors(Texture2D texture)
    {
        Future<Color[]> future = new Future<Color[]>();
        Call(() =>
        {
            Color[] temp = new Color[texture.Width * texture.Height];
            texture.GetData(temp);
            future.Value = temp;
        });
        return future;
    }
    public static Future<T> GetData<T>(Func<T> func)
    {
        Future<T> future = new Future<T>();
        Call(() =>
        {
            future.Value = func();
        });
        return future;
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
