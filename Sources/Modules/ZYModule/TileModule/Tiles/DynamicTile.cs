using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using IDrawable = Everglow.Sources.Modules.ZYModule.Commons.Core.IDrawable;
using IUpdateable = Everglow.Sources.Modules.ZYModule.Commons.Core.IUpdateable;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles
{
    internal interface IHookable
    {
        public void SetHookPosition(Projectile hook);
        public Vector2 GetSafePlayerPosition(Projectile hook);
    }
    internal interface IGrabbable
    {
        public void OnGrab(Player player);
        public void EndGrab(Player player);
        public bool CanGrab(Player player);
    }
    internal abstract class DynamicTile : IActive, IMoveable, IUpdateable, IDrawable
    {
        public virtual TextureType TextureType => TextureType.WhitePixel;
        public virtual Collider Collider
        {
            get;
        }
        public virtual Color MapColor
        {
            get;
        }

        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 oldVelocity;
        protected int whoAmI = -1;
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
        public bool Active { get; set; } = true;
        public Vector2 Position { get => position; set => position = value; }
        public Vector2 Velocity
        {
            get => oldVelocity;
            set => velocity = value;
        }

        public void Update()
        {
            AI();
            Move();
            oldVelocity = velocity;
        }
        public virtual void Kill()
        {
            Active = false;
        }
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
        public virtual void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
        {

        }
        public abstract void Stand(Entity entity, bool newStand);
        public abstract Vector2 GetTrueVelocity(Entity entity);
        public abstract void Leave(Entity entity);
    }

}