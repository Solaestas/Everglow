using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using IDrawable = Everglow.Sources.Modules.ZYModule.Commons.Core.IDrawable;
using IUpdateable = Everglow.Sources.Modules.ZYModule.Commons.Core.IUpdateable;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles
{
    public interface IDynamicTile : IActive, IUpdateable, IDrawable
    {
        public Direction MoveCollision(CRectangle rect, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false);
        public bool Collision(ICollider collider);
        public int Damage
        {
            get;
        }
        public int WhoAmI
        {
            get; set;
        }
        public void StandingBegin(Entity entity);
        public void StandingMoving(Entity entity);
        public void StandingLeaving(Entity entity);
        public bool OnTile(Entity entity, bool fallThrough = false);
        public void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale);
    }
    internal interface IHookable
    {
        public Vector2 GetSafeHookPosition(Projectile hook);
        public Vector2 GetSafePlayerPosition(Projectile hook);
        public Vector2 GetHookMovement(Projectile hook);
    }
    internal interface IGrabbable
    {
        public void OnGrab(Player player);
        public void EndGrab(Player player);
        public bool CanGrab(Player player);
    }
    internal interface IPickable
    {
        public int Health
        {
            get; set;
        }
        public void PickTile(int pickPower);
    }
    internal abstract class DynamicTile : IDynamicTile, IHookable
    {
        public virtual TextureType TextureType => TextureType.WhitePixel;
        public virtual int Damage => 0;
        public virtual int KnockBack => 0;
        public virtual bool TileCollding => true;
        public virtual ICollider Collider
        {
            get;
        }
        public virtual Color MapColor
        {
            get;
        }

        public Vector2 position;
        public Vector2 velocity;
        private int whoAmI = -1;
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
        /// 加载顺序问题，要用oldvel进行移动
        /// </summary>
        public Vector2 oldVelocity;
        public bool Active { get; set; } = true;
        public void Update()
        {
            AI();
            Move();
            oldVelocity = velocity;
        }
        public void Kill()
        {
            if (PreKill())
            {
                Active = false;
            }
        }
        public virtual bool PreKill()
        {
            return true;
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
        public virtual void OnTileColliding()
        {

        }
        public virtual Direction MoveCollision(CRectangle rect, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
        {
            return Direction.None;
        }
        public virtual bool Collision(ICollider collider)
        {
            return Collider.Colliding(collider);
        }
        public virtual Vector2 GetSafeHookPosition(Projectile hook)
        {
            return hook.position;
        }
        public virtual Vector2 GetSafePlayerPosition(Projectile hook)
        {
            return hook.position;
        }
        public virtual void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
        {

        }
        public virtual Vector2 GetHookMovement(Projectile hook)
        {
            return Vector2.Zero;
        }
        public virtual void StandingBegin(Entity entity)
        {

        }
        public virtual void StandingMoving(Entity entity)
        {

        }
        public virtual void StandingLeaving(Entity entity)
        {

        }

        public virtual bool OnTile(Entity entity, bool fallThrough = false)
        {
            return false;
        }
    }

}