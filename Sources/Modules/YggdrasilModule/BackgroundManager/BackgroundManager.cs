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
            public float Layer = 2;//绘制层,景深
            public int Priority = 0;//同层下绘制优先级
            public Vector2 Center;//中心坐标
            public Color Color;//颜色
            public Rectangle DrawRectangle;//绘制域
            public bool Active;//是否绘制，优先级最高
            public Texture2D Texture;//图片
            public float scale = 1;//大小
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