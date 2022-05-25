using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Terraria.GameContent;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles
{
    internal abstract class Block : DynamicTile, IGrabbable
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
        public override int KnockBack => 0;
        public override int Damage => 0;
        public override TextureType TextureType => TextureType.WhitePixel;
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
    }

}