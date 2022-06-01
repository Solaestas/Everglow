using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles
{
    internal abstract class Block : DynamicTile, IGrabbable, IHookable
    {
        internal static readonly Direction[] _AngleToInfo = new Direction[]
        {
            Direction.Right,
            Direction.Bottom,
            Direction.Left,
            Direction.Top
        };
        public Vector2 size;
        public Block()
        {
            oldVelocity = Vector2.Zero;
        }
        public Block(Vector2 position, Vector2 size)
        {
            this.position = position;
            this.size = size;
        }
        public override Color MapColor => new Color(255, 255, 255, 255);
        public override TextureType TextureType => TextureType.WhitePixel;
        public AABB AABB => new AABB(position, size);
        public override Collider Collider => new CAABB(new AABB(position, size));
        public override Direction MoveCollision(AABB aabb, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
        {
            AABB collider = AABB;

            Vector2 rvel = move - this.velocity;
            Vector2 pos0 = aabb.position;


            //if (!aabb.Scan(rvel).Intersect(collider))
            //{
            //    return Direction.None;
            //}

            const float SmallScale = 7;
            AABB smallBox = collider;
            smallBox.TopLeft += new Vector2(SmallScale, SmallScale);
            smallBox.BottomRight -= new Vector2(SmallScale, SmallScale);
            if (smallBox.Intersect(aabb))
            {
                return Direction.Inside;
            }

            bool onGround = velocity.Y == 0;
            bool? isX = null;
            Vector2 target = aabb.position + rvel;
            do
            {
                aabb.position = MathUtils.Approach(aabb.position, target, 1);
                if (aabb.Intersect(collider, out var area))
                {
                    isX = area.size.X < area.size.Y;
                    break;
                }
            } while (aabb.position != target);

            if(isX is null)
            {
                return Direction.None;
            }
            else if (isX.Value)
            {
                aabb.position.X = aabb.Left < collider.Left ? collider.Left - aabb.size.X : collider.Right;
                aabb.position.Y = target.Y;
                move = aabb.position - pos0 + this.velocity;
                velocity.X = this.velocity.X;
                return aabb.Left < collider.Left ? Direction.Right : Direction.Left;
            }
            else
            {
                aabb.position.Y = aabb.Top < collider.Top ? collider.Top - aabb.size.Y : collider.Bottom;
                aabb.position.X = target.X;
                move = aabb.position - pos0 + this.velocity;
                velocity.Y = this.velocity.Y == 0 ?
                    (onGround ? 0 : CollisionUtils.Epsilon * 10) :
                    this.velocity.Y;
                return aabb.Top < collider.Top ? Direction.Bottom : Direction.Top;
            }
        }
        public override void Stand(Entity entity, bool newStand)
        {
            if (newStand)
            {
                entity.velocity -= this.velocity / 2;
            }
        }
        public override void Leave(Entity entity)
        {
            entity.velocity += this.velocity;
        }
        public override Vector2 GetTrueVelocity(Entity entity)
        {
            return entity.velocity + this.velocity;
        }

        //public override void StandingBegin(Entity entity)
        //{
        //    entity.velocity.Y = 0;
        //    //避免滑下去，就速度减半好了
        //    entity.velocity.X = MathUtils.Approach(entity.velocity.X, 0, velocity.X / 2);
        //}
        //public override void StandingMoving(Entity entity)
        //{
        //    entity.position += velocity;
        //    entity.velocity.Y = 0;
        //    if (entity is Player player)
        //    {
        //        OnPlayerStanding(player);
        //    }
        //}
        //public override void StandingLeaving(Entity entity)
        //{
        //    entity.velocity += velocity;
        //}
        //public override Vector2 GetSafeHookPosition(Projectile hook)
        //{
        //    Vector2 target = hook.position;
        //    var size = this.size;
        //    if (hook.position.X + hook.width > position.X + size.X)
        //    {
        //        target.X = position.X + this.size.X - hook.width;
        //    }
        //    else if (hook.position.X < position.X)
        //    {
        //        target.X = position.X;
        //    }
        //    if (hook.position.Y + hook.height > position.Y + size.Y)
        //    {
        //        target.Y = position.Y + this.size.Y - hook.height;
        //    }
        //    else if (hook.position.Y < position.Y)
        //    {
        //        target.Y = position.Y;
        //    }
        //    return target;
        //}
        //public override Vector2 GetHookMovement(Projectile hook)
        //{
        //    return velocity;
        //}
        //public override bool OnTile(Entity entity, bool fallThrough = false)
        //{
        //    return Collider.Colliding(new CRectangle(entity.position + Vector2.UnitY * ((entity as Player)?.gravDir ?? 1), entity.Size));
        //}
        public virtual bool CanGrab(Player player)
        {
            return AABB.Intersect(new AABB(player.position + player.slideDir * Vector2.UnitX, player.Size));
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
                        candraw = drawdes.Intersects(mapRect.Value);
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
            if (target != position)
            {
                velocity *= 0;
                oldVelocity *= 0;
            }
        }
        public override void Draw()
        {
            Main.spriteBatch.Draw(TextureType.GetValue(), position - Main.screenPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), Color.White);
        }

        public virtual void SetHookPosition(Projectile hook)
        {
            hook.position = Vector2.Clamp(hook.position, position, position + size - hook.Size);
            hook.position += this.velocity;
        }

        public Vector2 GetSafePlayerPosition(Projectile hook)
        {
            return hook.position;
        }
    }

}