using Terraria.UI;
using MonoMod.RuntimeDetour.HookGen;
using Terraria.GameContent.UI.Elements;
using ReLogic.Content;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Enumerator;

namespace Everglow.Sources.Icons
{
    internal class AutoDynamicIconHook : ILoadable
    {
        MethodInfo HookTarget;
        public void Load(Mod mod)
        {
            //˵�������Ѿ�����,ֱ������
            if (HookTarget is not null)
            {
                return;
            }
            //��ȡ�ϼ�����
            Assembly assembly = typeof(Terraria.ModLoader.UI.UICommon).Assembly;
            //׼������
            Type[] types;
            try
            {
                //���Խ�����ע������
                types = assembly.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                //���쳣��ȡ���Ѿ��ɹ��õ�������
                types = (from Type t in e.Types where t is not null select t).ToArray();
            }
            //ɸѡĿ��
            foreach (var type in types)
            {
                //�ҵ�Ŀ��
                if (type.Name == "UIModItem")
                {
                    //��ȡĿ�����͵ĳ�ʼ���������
                    HookTarget = type.GetMethod("OnInitialize", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                    break;
                }
            }
            //ͨ������ҹ�,ʵ����Ӧ�ý���һ��HookTarget�п�,��������
            HookEndpointManager.Add(MethodBase.GetMethodFromHandle(HookTarget.MethodHandle), HookMethod);
        }
        public void Unload()
        {
            //˵��û����,û��Ҫж��
            if (HookTarget is null)
            {
                return;
            } // TODO: Fix an object reference error on mod reload without introducing other bugs
            HookTarget = null;
            HookEndpointManager.Remove(MethodBase.GetMethodFromHandle(HookTarget.MethodHandle), HookMethod);
        }
        private void HookMethod(Action<UIElement> orig, UIElement self)
        {
            //����ԭ�淽��,�ѻ������ݼ���
            orig(self);
            //��ȡ���������mod�������ж��ǲ����Լ�
            string Name = (string)self.GetType().GetMethod("get_ModName", BindingFlags.Public | BindingFlags.Instance).Invoke(self, null);
            if (Name == Everglow.Instance.Name)
            {
                //�Ƴ�ԭ���ģ��ͼ��,ʵ����Ӧ�öԷ������һ���п�,��������
                self.RemoveChild((UIElement)self.GetType().GetField("_modIcon", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(self));
                //��ͼ�����ɶ�̬UI�Ž�ȥ
                self.Append(new DynamicIconUI(ModContent.Request<Texture2D>("Everglow/Sources/Icons/Icon_content", AssetRequestMode.ImmediateLoad).Value, new Rectangle(0, 0, 80, 80), 3));
                //�ѱ߿����ɶ�̬UI�Ž�ȥ
                var border = new DynamicIconUI(ModContent.Request<Texture2D>("Everglow/Sources/Icons/Icon_framework", AssetRequestMode.ImmediateLoad).Value, new Rectangle(0, 0, 96, 96), 3);
                //�߿��ͼ���16,������Ҫ������ƫ��(8,8)
                border.Left.Set(-8, 0);
                border.Top.Set(-8, 0);
                self.Append(border);
                //��ֹ�߿򳬳��װ�
                //self.OverflowHidden = true;
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
            /// <summary>
            /// ��̬ͼƬ��UI,ֻ���ܵ���֡ͼ,��Ҳ���Ը������draw
            /// </summary>
            /// <param name="texture">Ҫ���Ƶ�ͼƬ</param>
            /// <param name="frame">ÿ֡�Ĵ�С</param>
            /// <param name="size">��Ӧ�����Ŵ�С����</param>
            /// <param name="interval">ˢ�¼��</param>
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
            /// <summary>
            /// ��̬ͼƬ��UI,ֻ���ܵ���֡ͼ,��Ҳ���Ը������draw
            /// </summary>
            /// <param name="texture">Ҫ���Ƶ�ͼƬ</param>
            /// <param name="frame">ÿ֡�Ĵ�С</param>
            /// <param name="interval">ˢ�¼��</param>
            public DynamicIconUI(Texture2D texture, Rectangle frame, int interval) : this(texture, frame, frame.Size(), interval)
            {

            }
            public override void OnInitialize()
            {
                //����ˢ�¼����ʱ��
                Timer = 0;
            }
            protected override void DrawSelf(SpriteBatch spriteBatch)
            {
                //���¼�ʱ��
                Timer++;
                //����û�֡��
                if (Timer % Interval == 0)
                {
                    //�����һ֡�ִ���ͼ�ײ�(����ͼƬ����)
                    if (Timer / Interval * Frame.Height >= Texture.Height)
                    {
                        //���ü�ʱ��
                        Timer = 0;
                    }
                    //����֡����
                    Frame = new Rectangle(0, Timer / Interval * Frame.Height, Frame.Width, Frame.Height);
                }
                //��ȡ����Ӧ���ű���
                Vector2 scale = new(Size.X / Frame.Width, Size.Y / Frame.Height);
                //��ͼ
                spriteBatch.Draw(Texture,
                    GetDimensions().Position(),
                    Frame,
                    Color,
                    0,
                    Vector2.Zero,
                    scale,
                    SpriteEffects.None,
                    0);
                //�����Ҫ�л������Ļ���ͼƬ�Ļ�,�����Լ���ͼƬ����
                //Ȼ���Texture,Frame,Size,Interval���¸�ֵ���ɽ����л�
            }
        }
    }
}