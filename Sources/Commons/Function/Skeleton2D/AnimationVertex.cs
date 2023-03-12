using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Function.Skeleton2D
{
    /// <summary>
    /// 绑定在当前顶点上的骨骼加权数据
    /// </summary>
    internal class BoneBinding
    {
        public Bone2D Bone
        {
            get; set;
        }

        public Vector2 BindPosition
        {
            get; set;
        }

        public float Weight
        {
            get; set;
        }
    }
    internal class AnimationVertex
    {
        public Vector2 UV
        {
            get; set;
        }

        public List<BoneBinding> BoneBindings
        {
            get; set;
        }
    }
}
