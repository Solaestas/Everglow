using Everglow.Sources.Commons.Core.ModuleSystem;

namespace Everglow.Sources.Commons.Core;

internal class Future<T>
{
    private bool isLoaded;
    private T value;
    public T Value
    {
        get
        {
            if (!IsLoaded)
            {
                throw new InvalidOperationException("Hasn't been Loaded");
            }
            return value;
        }
        set
        {
            if (IsLoaded)
            {
                throw new InvalidOperationException("Has been Loaded");
            }
            isLoaded = true;
            this.value = value;
        }
    }
    public bool IsLoaded => isLoaded;
}
internal class MainThreadContext
{
    private object m_lock = new object();
    private Stack<Action> tasks = new Stack<Action>();
    public void Load()
    {
        On.Terraria.Main.Update += Main_Update;
    }

    public void Unload()
    {
        On.Terraria.Main.Update -= Main_Update;
        tasks = null;
    }
    public void AddTask(Action task)
    {
        lock (m_lock)
        {
            Everglow.MainThreadContext.tasks.Push(task);
        }
    }
    public Future<Color[]> DelayGetColors(Texture2D texture)
    {
        Future<Color[]> future = new Future<Color[]>();
        AddTask(() =>
        {
            Color[] temp = new Color[texture.Width * texture.Height];
            texture.GetData(temp);
            future.Value = temp;
        });
        return future;
    }
    public Future<T> DelayGetData<T>(Func<T> func)
    {
        Future<T> future = new Future<T>();
        AddTask(() =>
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
        lock (m_lock)
        {
            if (tasks.Count != 0)
            {
                try
                {
                    while (tasks.Count > 0)
                    {
                        tasks.Pop().Invoke();
                    }
                }
                catch (Exception ex)
                {
                    Everglow.Instance.Logger.Error(ex);
                    Debug.Fail(ex.ToString());
                }
            }
        }
        orig(self, gameTime);
    }
}
