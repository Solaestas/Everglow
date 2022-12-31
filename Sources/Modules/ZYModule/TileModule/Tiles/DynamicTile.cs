using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;
using IDrawable = Everglow.Sources.Modules.ZYModule.Commons.Core.IDrawable;
using IUpdateable = Everglow.Sources.Modules.ZYModule.Commons.Core.IUpdateable;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles
{
    internal interface IHookable
    {
        public void SetHookPosition(Projectile hook);
    }
    internal interface IGrabbable
    {
        public void OnGrab(Player player);
        public void EndGrab(Player player);
        public bool CanGrab(Player player);
    }
    /// <summary>
    /// 一个基础的运动方块
    /// </summary>
    internal abstract class DynamicTile : IActive, IMoveable, IUpdateable, IDrawable
    {
        /// <summary>
        /// 默认绘制使用的贴图
        /// </summary>
        public virtual TextureType TextureType => TextureType.WhitePixel;
        /// <summary>
        /// 检验碰撞的碰撞箱
        /// </summary>
        public virtual Collider Collider
        {
            get;
        }
        /// <summary>
        /// 默认地图绘制使用的颜色
        /// </summary>
        public virtual Color MapColor
        {
            get;
        }
        /// <summary>
        /// 位置
        /// </summary>
        protected Vector2 position;
        /// <summary>
        /// 速度
        /// </summary>
        protected Vector2 velocity;
        /// <summary>
        /// 上一帧的速度，为了避免碰撞判定后修改速度导致出现差错，使用<see cref="oldVelocity"/>来决定移动距离
        /// </summary>
        protected Vector2 oldVelocity;
        /// <summary>
        /// 在数组中的下标
        /// </summary>
        protected int whoAmI = -1;
        /// <summary>
        /// 在数组中的下标
        /// </summary>
        public virtual int WhoAmI
        {
            get
            {
                if (whoAmI == -1)
                {
                    Everglow.Instance.Logger.Warn("检测到whoAmI未赋值，可能直接调用list.add或Tile未加入TileSystem");
                }
                return whoAmI;
            }
            set
            {
                if (whoAmI != -1)
                {
                    throw new InvalidOperationException("请勿对whoAmI重复赋值");
                }
                whoAmI = value;
            }
        }
        /// <summary>
        /// 是否激活
        /// </summary>
        public bool Active { get; set; } = true;
        /// <summary>
        /// 速度
        /// </summary>
        public virtual Vector2 Position { get => position; set => position = value; }
        /// <summary>
        /// 位移
        /// </summary>
        public virtual Vector2 Velocity { get => velocity; set => velocity = value; }
        public virtual bool IsGrabbable => false;
        /// <summary>
        /// 更新顺序不固定，可以自己重写
        /// </summary>
        public virtual void Update()
        {
            Move();
            AI();
            oldVelocity = velocity;
        }
        public virtual void Kill()
        {
            Active = false;
        }
        /// <summary>
        /// 空的，用来执行物块运动逻辑
        /// </summary>
        public virtual void AI()
        {

        }
        public virtual void Draw()
        {
            Main.spriteBatch.Draw(TextureType.GetValue(), position - Main.screenPosition, null, Color.White);
        }
        public virtual void Move()
        {
            position += oldVelocity;
        }
        public abstract Direction MoveCollision(AABB aabb, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false);
        public virtual bool Collision(Collider collider)
        {
            return Collider.Collision(collider);
        }
        public virtual void OnCollision(AABB aabb, Direction dir)
        {

        }
        public virtual void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
        {

        }
        public virtual void Stand(EntityHandler entity, bool newStand) { }
        public virtual void Leave(EntityHandler entity) { }
        [Obsolete]
        public virtual Vector2 GetMove(EntityHandler entity)
        {
            return Velocity;
        }
    }

}