using ReLogic.Content;


namespace Everglow.Sources.Modules.ZY.Commons.Function;

internal static class Quick
{
    public static GraphicsDevice GD => Main.instance.GraphicsDevice;
    public static SpriteBatch SB => Main.spriteBatch;

    public static Asset<Texture2D> RequestTexture(string path, bool async = true) =>
        ModContent.Request<Texture2D>("Everglow/Sources/ZY/" + path,
        async ? AssetRequestMode.AsyncLoad : AssetRequestMode.ImmediateLoad);
    public static Asset<Effect> RequestEffect(string path, bool async = false) =>
        ModContent.Request<Effect>("Everglow/Sources/ZY/" + path,
        async ? AssetRequestMode.AsyncLoad : AssetRequestMode.ImmediateLoad);

    public static void Log(object obj)
    {
        Everglow.Instance.Logger.Info(obj);
        Main.NewText(obj, Color.Green);
        Console.WriteLine(obj);
    }
}
