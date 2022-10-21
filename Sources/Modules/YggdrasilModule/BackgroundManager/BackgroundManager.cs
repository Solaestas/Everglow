using Everglow.Sources.Commons.Function.ImageReader;
using Everglow.Sources.Commons.Function.Vertex;
using Everglow.Sources.Modules.MythModule.TheFirefly.NPCs.Bosses;
using Everglow.Sources.Modules.MythModule.Common;
using Everglow.Sources.Modules.MythModule.TheFirefly.WorldGeneration;

namespace Everglow.Sources.Modules.YggdrasilModule.BackgroundManager
{
    public class BackgroundManager
    {
        public class WorldBackground
        {
            public float Layer = 2;//���Ʋ�,����
            public int Priority = 0;//ͬ���»������ȼ�
            public Vector2 Center;//��������
            public Color Color;//��ɫ
            public Rectangle DrawRectangle;//������
            public bool Active;//�Ƿ���ƣ����ȼ����
            public Texture2D Texture;//ͼƬ
            public float scale = 1;//��С
        }
        public class BoardBackground : WorldBackground
        {
            public bool XClamp = false;
            public bool YClamp = true;
        }
        public class PointBackground : WorldBackground
        {
            public float Rotation = 0;
            public Vector2 Velocity;
            public float[] ai = new float[8];
            public void Update()
            {

            }
            public void SpecialDraw(SpriteBatch spriteBatch)
            {

            }
        }
    }
}