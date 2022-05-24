using Everglow.Sources.Modules.ZY.Commons.Function;
using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Terraria.GameContent;
using IDrawable = Everglow.Sources.Modules.ZYModule.Commons.Core.IDrawable;
using IUpdateable = Everglow.Sources.Modules.ZYModule.Commons.Core.IUpdateable;

namespace Everglow.Sources.Modules.ZYModule.TileModule
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
    internal abstract class Block : DynamicTile, IGrabbable
    {
        internal static readonly Direction[] _AngleToInfo = new Direction[]
        {
            Direction.Right,
            Direction.Bottom,
            Direction.Left,
            Direction.Top
        };
        public Block()
        {
            oldVelocity = Vector2.Zero;
        }
        public override Color MapColor => new Color(255, 255, 255, 255);
        public override int KnockBack => 0;
        public override int Damage => 0;
        public override TextureType TextureType => TextureType.WhitePixel;
        public Vector2 size;
        public override ICollider Collider => new CRectangle(position, size);
        public Vector2 Center => position + new Vector2(size.X / 2f, size.Y / 2f);
        public Vector2 Left => position + new Vector2(0, size.Y / 2f);
        public Vector2 Right => position + new Vector2(size.X, size.Y / 2f);
        public Vector2 Top => position + new Vector2(size.X / 2f, 0);
        public Vector2 Bottom => position + new Vector2(size.X / 2f, size.Y);
        public Vector2 LeftBottom => position + new Vector2(0, size.Y);
        public Vector2 RightBottom => position + new Vector2(size.X, size.Y);
        public Vector2 LeftTop => position + new Vector2(0, 0);
        public Vector2 RightTop => position + new Vector2(size.X, 0);
        public override Direction MoveCollision(CRectangle rect, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
        {
            var collider = Collider;

            Vector2 rV = move - this.velocity;
            if (rV == Vector2.Zero)
            {
                return Direction.None;
            }
            Vector2 pos = rect.pos;
            Vector2[] vs = new Vector2[4] { rect.TopLeft, rect.BottomRight, rect.TopLeft + rV, rect.BottomRight + rV };
            Vector2 min = Vector2.One * float.MaxValue, max = Vector2.Zero;
            foreach (var v in vs)
            {
                min = Vector2.Min(v, min);
                max = Vector2.Max(v, max);
            }
            var aabb = CollisionUtils.LineToAABB(min, max);
            if (!aabb.Colliding(collider))
            {
                return Direction.None;
            }
            if (collider.Colliding(rect))
            {
                for (int r = 1; r <= 8; r += 1)
                {
                    for (int a = 0; a < 5; a += 1)
                    {
                        var vec = new Vector2(r, 0).RotatedBy(a * MathHelper.PiOver2 - MathHelper.Pi);
                        var c = new CRectangle(rect.pos + vec, rect.size);
                        if (!collider.Colliding(c))
                        {
                            rect.pos += vec;
                            move -= vec;
                            goto GOTO_A;//跳出双重循环
                        }
                    }
                }

                if (collider.Colliding(rect))
                {
                    rect.pos += this.velocity;
                    move = rect.pos - pos;
                    return Direction.Inside;
                }
            }
        GOTO_A:
            const float stepLength = 1;
            Vector2 target = rect.pos + move - this.velocity;
            if (target == rect.pos)
            {
                return Direction.None;
            }
            Vector2 dir = rV.Normalize_S().Abs();
            dir = Vector2.Max(dir, new Vector2(0.01f, 0.01f));//防止浮点误差出事（指无限次循环）
            do
            {
                rect.pos.X = MathUtils.Approach(rect.pos.X, target.X, stepLength * dir.X);//横向移动
                if (collider.Colliding(rect))
                {
                    rect.pos.X = rect.pos.X > position.X + this.velocity.X + size.X / 2 ? position.X + size.X : position.X - rect.size.X;
                    rect.pos.Y = target.Y;
                    rect.pos += this.velocity;
                    move = rect.pos - pos;
                    velocity.X = this.velocity.X - Math.Sign(velocity.X) * KnockBack * MathUtils.Sqrt(Math.Abs(velocity.X));
                    return KnockBack != 0 ? Direction.None : rV.X > 0 ? Direction.Right : Direction.Left;
                }
                rect.pos.Y = MathUtils.Approach(rect.pos.Y, target.Y, stepLength * dir.Y);//纵向移动
                if (collider.Colliding(rect))
                {
                    rect.pos.Y = rect.pos.Y > position.Y + this.velocity.Y + size.Y / 2 ? position.Y + size.Y : position.Y - rect.size.Y;
                    rect.pos.X = target.X;
                    rect.pos += this.velocity;
                    move = rect.pos - pos;
                    velocity.Y = this.velocity.Y - Math.Sign(velocity.Y) * KnockBack * MathUtils.Sqrt(Math.Abs(velocity.Y));
                    return KnockBack != 0 ? Direction.None : rV.Y > 0 ? Direction.Bottom : Direction.Top;
                }
            } while (rect.pos != target);
            move = rect.pos - pos;
            return Direction.None;
        }
        public virtual void OnPlayerStanding(Player player)
        {

        }
        public override void StandingBegin(Entity entity)
        {
            entity.velocity.Y = 0;
            //避免滑下去，就速度减半好了
            entity.velocity.X = MathUtils.Approach(entity.velocity.X, 0, velocity.X / 2);
        }
        public override void StandingMoving(Entity entity)
        {
            entity.position += velocity;
            entity.velocity.Y = 0;
            if (entity is Player player)
            {
                OnPlayerStanding(player);
            }
        }
        public override void StandingLeaving(Entity entity)
        {
            entity.velocity += velocity;
        }
        public override Vector2 GetSafePlayerPosition(Projectile hook)
        {
            Vector2 target = Main.player[hook.owner].position;
            if (position.X > hook.position.X)
            {
                target.X = position.X - Main.player[hook.owner].width;
            }
            if (position.X + size.X < hook.position.X + hook.width)
            {
                target.X = position.X + size.X;
            }
            if (position.Y > hook.position.Y)
            {
                target.Y = position.Y - Main.player[hook.owner].height;
            }
            if (position.Y + size.Y < hook.position.Y + hook.height)
            {
                target.Y = position.Y + size.Y;
            }
            return target;
        }
        public override Vector2 GetSafeHookPosition(Projectile hook)
        {
            Vector2 target = hook.position;
            var size = this.size;
            if (hook.position.X + hook.width > position.X + size.X)
            {
                target.X = position.X + this.size.X - hook.width;
            }
            else if (hook.position.X < position.X)
            {
                target.X = position.X;
            }
            if (hook.position.Y + hook.height > position.Y + size.Y)
            {
                target.Y = position.Y + this.size.Y - hook.height;
            }
            else if (hook.position.Y < position.Y)
            {
                target.Y = position.Y;
            }
            return target;
        }
        public override Vector2 GetHookMovement(Projectile hook)
        {
            return velocity;
        }
        public override bool OnTile(Entity entity, bool fallThrough = false)
        {
            return Collider.Colliding(new CRectangle(entity.position + Vector2.UnitY * ((entity as Player)?.gravDir ?? 1), entity.Size));
        }
        public virtual bool CanGrab(Player player)
        {
            return Collider.Colliding(new CRectangle(player.position + player.slideDir * Vector2.UnitX, player.Size));
        }
        public virtual void OnGrab(Player player)
        {
            player.position.X = player.position.X > position.X + size.X / 2 ? position.X + size.X : position.X - player.width;
            player.position += velocity;
            player.velocity.X -= velocity.X;
        }
        public virtual void EndGrab(Player player)
        {
            player.velocity.X += velocity.X * 2;
            player.GetModPlayer<PlayerManager>().Jump(player.jump, player.velocity.Y + velocity.Y);
        }
        public override void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
        {
            Vector2 Dc = position + new Vector2(16, 16);
            int Dwidth = (int)(size.X / 16f);
            int Dheight = (int)(size.Y / 16f);
            for (int i = 0; i < Dwidth; i++)
            {
                for (int j = 0; j < Dheight; j++)
                {
                    Vector2 DrawPos = (Dc / 16f + new Vector2(i - 1, j - 1) - mapTopLeft) * mapScale + mapX2Y2AndOff;
                    Texture2D t = TextureAssets.MagicPixel.Value;
                    Rectangle drawdes = new Rectangle((int)DrawPos.X - 1, (int)DrawPos.Y - 1, 2, 2);
                    bool candraw;
                    if (mapRect != null)
                    {
                        candraw = drawdes.Intersects((Rectangle)mapRect);
                    }
                    else
                    {
                        candraw = true;
                    }
                    if (candraw)
                    {
                        Main.spriteBatch.Draw(t, DrawPos, new Rectangle(0, 0, 1, 1), MapColor, 0, Vector2.Zero, mapScale, SpriteEffects.None, 0);
                    }
                }
            }
        }
        public override void Move()
        {
            Vector2 target = position + oldVelocity;
            TileSystem.EnableDTCollision = false;
            position += Terraria.Collision.TileCollision(position, oldVelocity, (int)size.X, (int)size.Y);
            TileSystem.EnableDTCollision = true;
            if(target != position)
            {
                velocity *= 0;
                oldVelocity *= 0;
            }
        }
        public override void Draw()
        {
            Main.spriteBatch.Draw(TextureType.GetValue(), position - Main.screenPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }
    }
    internal class Circle : DynamicTile
    {

    }
    internal abstract class Platform : DynamicTile
    {
        public Platform(Vector2 pos, Vector2 vel, float width, Direction dir)
        {
            position = pos;
            this.width = width;
            direction = dir;
            velocity = vel;
        }

        public readonly Direction direction;
        public readonly float width;
        public override ICollider Collider => direction switch
        {
            Direction.Top or Direction.Bottom => new CRectangle(position - new Vector2(width, 0), new Vector2(width * 2, 1)),
            Direction.Left or Direction.Right => new CRectangle(position - new Vector2(0, width), new Vector2(1, width * 2)),
            _ => throw new Exception("Invalid Platform Direction")
        };
        public override Direction MoveCollision(CRectangle rect, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
        {
            var collider = Collider;

            if (ignorePlats || rect.Colliding(collider))
            {
                return Direction.None;
            }

            Vector2 rV = move - this.velocity;
            Vector2 pos = rect.pos;
            if (rV == Vector2.Zero)
            {
                return Direction.None;
            }
            Vector2[] vs = new Vector2[4] { rect.TopLeft, rect.BottomRight, rect.TopLeft + rV, rect.BottomRight + rV };
            Vector2 min = Vector2.One * float.MaxValue, max = Vector2.Zero;
            foreach (var v in vs)
            {
                min = Vector2.Min(v, min);
                max = Vector2.Max(v, max);
            }
            var aabb = CollisionUtils.LineToAABB(min, max);
            if (!aabb.Colliding(collider))
            {
                return Direction.None;
            }

            var t = velocity * direction.ToVector2();
            if (t.X >= 0 && t.Y >= 0)
            {
                return Direction.None;
            }

            var target = rect.pos + rV;
            var dir = Vector2.Max(move.Normalize_S(), new Vector2(0.01f, 0.01f));
            do
            {
                switch (direction)
                {
                    case Direction.Top:
                        rect.pos.X = MathUtils.Approach(rect.pos.X, target.X, dir.X);
                        rect.pos.Y = MathUtils.Approach(rect.pos.Y, target.Y, dir.Y);
                        if (rect.Colliding(collider))
                        {
                            rect.pos.Y = position.Y - rect.size.Y;
                            rect.pos.X = target.X;
                            rect.pos += this.velocity;
                            move = rect.pos - pos;
                            velocity.Y = this.velocity.Y;
                            return Direction.Bottom;
                        }
                        break;
                    case Direction.Bottom:
                        rect.pos.X = MathUtils.Approach(rect.pos.X, target.X, dir.X);
                        rect.pos.Y = MathUtils.Approach(rect.pos.Y, target.Y, dir.Y);
                        if (rect.Colliding(collider))
                        {
                            rect.pos.Y = position.Y;
                            rect.pos.X = target.X;
                            rect.pos += this.velocity;
                            move = rect.pos - pos;
                            velocity.Y = this.velocity.Y;
                            return Direction.Top;
                        }
                        break;
                    case Direction.Left:
                        rect.pos.Y = MathUtils.Approach(rect.pos.Y, target.Y, dir.Y);
                        rect.pos.X = MathUtils.Approach(rect.pos.X, target.X, dir.X);
                        if (rect.Colliding(collider))
                        {
                            rect.pos.X = position.X - rect.size.X;
                            rect.pos.Y = target.Y;
                            rect.pos += this.velocity;
                            move = rect.pos - pos;
                            velocity.X = this.velocity.X;
                            return Direction.Right;
                        }
                        break;
                    case Direction.Right:
                        rect.pos.Y = MathUtils.Approach(rect.pos.Y, target.Y, dir.Y);
                        rect.pos.X = MathUtils.Approach(rect.pos.X, target.X, dir.X);
                        if (rect.Colliding(collider))
                        {
                            rect.pos.X = position.X;
                            rect.pos.Y = target.Y;
                            rect.pos += this.velocity;
                            move = rect.pos - pos;
                            velocity.X = this.velocity.X;
                            return Direction.Left;
                        }
                        break;
                }
            } while (target != rect.pos);
            move = rect.pos - pos;
            return Direction.None;
        }
        public override bool OnTile(Entity entity, bool fallThrough = false)
        {
            if (entity is Player player)
            {
                return Collider.Colliding(new CRectangle(entity.position + Vector2.UnitY * player.gravDir, entity.Size)) && !fallThrough;
            }
            return Collider.Colliding(new CRectangle(entity.position + Vector2.UnitY, entity.Size)) && !fallThrough;
        }
        public override void StandingBegin(Entity entity)
        {
            entity.velocity.Y = 0;
            entity.velocity.X = MathUtils.Approach(entity.velocity.X, 0, velocity.X / 2);
        }
        public override void StandingMoving(Entity entity)
        {
            entity.position += velocity;
            entity.velocity.Y = 0;
        }
        public override void StandingLeaving(Entity entity)
        {
            entity.velocity += velocity;
        }
        public override Vector2 GetSafePlayerPosition(Projectile hook)
        {
            Vector2 target = hook.position;
            switch (direction)
            {
                case Direction.Left:
                case Direction.Right:
                    target.X = position.X;
                    break;
                case Direction.Top:
                case Direction.Bottom:
                    target.Y = position.Y;
                    break;
                default:
                    throw new Exception("Invalid Platform Direction");
            }
            return target;
        }
        public override Vector2 GetSafeHookPosition(Projectile hook)
        {
            return hook.position;
        }
        public override Vector2 GetHookMovement(Projectile hook)
        {
            return velocity;
        }
    }

}