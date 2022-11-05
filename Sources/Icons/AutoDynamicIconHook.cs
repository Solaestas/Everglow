using Terraria.UI;
using MonoMod.RuntimeDetour.HookGen;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;


namespace Everglow.Sources.Icons
{
    internal class AutoDymanicIconHook : ILoadable
    {
        MethodInfo HookTarget;
        public void Load(Mod mod)
        {
            if (HookTarget is not null)
            {
                return;
            }
            Assembly assembly = typeof(Terraria.ModLoader.UI.UICommon).Assembly;
            Type[] types;
            try
            {
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                types = (from Type t in e.Types where t is not null select t).ToArray();
            }
            foreach (var type in types)
            {
                if (type.Name == "UIModItem")
                {
                    HookTarget = type.GetMethod("OnInitialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    break;
                }
            }
            HookEndpointManager.Add(MethodBase.GetMethodFromHandle(HookTarget.MethodHandle), HookMethod);
        }
        public void Unload()
        {
            if (HookTarget is null)
            {
                return;
            }
            HookTarget = null;
            HookEndpointManager.Remove(MethodBase.GetMethodFromHandle(HookTarget.MethodHandle), HookMethod);
        }
        private void HookMethod(Action<UIElement> orig, UIElement self)
        {
            orig(self);
            string Name = (string)self.GetType().GetMethod("get_ModName", BindingFlags.Public | BindingFlags.Instance).Invoke(self, null);
            if (Name == Everglow.Instance.Name)
            {
                self.RemoveChild((UIElement)self.GetType().GetField("_modIcon", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self));
                self.Append(new DynamicIconUI(ModContent.Request<Texture2D>("Everglow/Sources/Icons/EverglowIcon",
                                                                            AssetRequestMode.ImmediateLoad).Value,
                                                                            new Rectangle(0, 0, 80, 80),
                                                                            3));
            }
        }
        public class DynamicIconUI : UIElement, IColorable
        {
            public Texture2D Texture;
            public Rectangle Frame;
            public Vector2 Size;
            public int Interval;
            public int Timer;
            public Color Color { get; set; } = Color.White;
            public DynamicIconUI(Texture2D texture, Rectangle frame, Vector2 size, int interval)
            {
                Texture = texture;
                Frame = frame;
                Size = size;
                Interval = interval;
            }
            //#region Use To Debug
            //public override string ToString()
            //{
            //    return $"[{nameof(Texture)}]:{{IsNull?{Texture is null},Name:{Texture.Name},Width:{Texture?.Width ?? 0},Height:{Texture?.Height ?? 0}}}\n" +
            //        $"[{nameof(Frame)}]:{Frame}\n" +
            //        $"[{nameof(Size)}]:{Size}\n" +
            //        $"[{nameof(Timer)}]:{Timer}" +
            //        $"[{nameof(Interval)}]:{Interval}\n" +
            //        $"[Scale]:{new Vector2(Size.X / Frame.Width, Size.Y / Frame.Height)}";
            //}
            //#endregion
            public DynamicIconUI(Texture2D texture, Rectangle frame, int interval) : this(texture, frame, frame.Size(), interval)
            {

            }
            public override void OnInitialize()
            {
                Timer = 0;
            }
            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                Timer++;
                if (Timer % Interval == 0)
                {
                    if (Timer / Interval * Frame.Height >= Texture.Height)
                    {
                        Timer = 0;
                    }
                    Frame = new Rectangle(0, Timer / Interval * Frame.Height, Frame.Width, Frame.Height);
                }
                Vector2 scale = new(Size.X / Frame.Width, Size.Y / Frame.Height);
                spriteBatch.Draw(Texture,
                    GetDimensions().Position(),
                    Frame,
                    Color,
                    0,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0);
            }
        }
    }
}