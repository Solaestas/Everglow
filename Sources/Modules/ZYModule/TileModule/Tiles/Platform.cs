//using Everglow.Sources.Modules.ZYModule.Commons.Core;
//using Everglow.Sources.Modules.ZYModule.Commons.Function;

//namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

//internal abstract class Platform : DynamicTile
//{
//    public Platform()
//    {

//    }
//    public Platform(Vector2 pos, Vector2 vel, float width, Direction dir)
//    {
//        position = pos;
//        this.width = width;
//        direction = dir;
//        velocity = vel;
//    }

//    public readonly Direction direction;
//    public readonly float width;
//    public virtual bool CanBeIgnore => true;
//    public override ICollider Collider => direction switch
//    {
//        Direction.Top or Direction.Bottom => new CLine(position - new Vector2(width / 2, 0), position + new Vector2(width / 2, 0)),
//        Direction.Left or Direction.Right => new CLine(position - new Vector2(0, width / 2), position + new Vector2(0, width / 2)),
//        _ => throw new Exception("Invalid Platform Direction")
//    };
//    public override Direction MoveCollision(CRectangle rect, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
//    {
//        var collider = Collider;
//        Vector2 rV = move - this.velocity;
//        Vector2 pos = rect.pos;

//        if ((ignorePlats && CanBeIgnore && direction == Direction.Top) || collider.Colliding(rect))
//        {
//            return Direction.None;
//        }

//        if(Vector2.Dot(direction.ToVector2(), rV) > 0)
//        {
//            return Direction.None;
//        }

//        var target = rect.pos + rV;
//        var dir = Vector2.Max(move.Normalize_S(), new Vector2(0.01f, 0.01f));
//        do
//        {
//            switch (direction)
//            {
//                case Direction.Top:
//                    rect.pos.X = MathUtils.Approach(rect.pos.X, target.X, dir.X);
//                    rect.pos.Y = MathUtils.Approach(rect.pos.Y, target.Y, dir.Y);
//                    if (rect.Colliding(collider))
//                    {
//                        rect.pos.Y = position.Y - rect.size.Y;
//                        rect.pos.X = target.X;
//                        rect.pos += this.velocity;
//                        move = rect.pos - pos;
//                        velocity.Y = this.velocity.Y;
//                        return Direction.Bottom;
//                    }
//                    break;
//                case Direction.Bottom:
//                    rect.pos.X = MathUtils.Approach(rect.pos.X, target.X, dir.X);
//                    rect.pos.Y = MathUtils.Approach(rect.pos.Y, target.Y, dir.Y);
//                    if (rect.Colliding(collider))
//                    {
//                        rect.pos.Y = position.Y;
//                        rect.pos.X = target.X;
//                        rect.pos += this.velocity;
//                        move = rect.pos - pos;
//                        velocity.Y = this.velocity.Y;
//                        return Direction.Top;
//                    }
//                    break;
//                case Direction.Left:
//                    rect.pos.Y = MathUtils.Approach(rect.pos.Y, target.Y, dir.Y);
//                    rect.pos.X = MathUtils.Approach(rect.pos.X, target.X, dir.X);
//                    if (rect.Colliding(collider))
//                    {
//                        rect.pos.X = position.X - rect.size.X;
//                        rect.pos.Y = target.Y;
//                        rect.pos += this.velocity;
//                        move = rect.pos - pos;
//                        velocity.X = this.velocity.X;
//                        return Direction.Right;
//                    }
//                    break;
//                case Direction.Right:
//                    rect.pos.Y = MathUtils.Approach(rect.pos.Y, target.Y, dir.Y);
//                    rect.pos.X = MathUtils.Approach(rect.pos.X, target.X, dir.X);
//                    if (rect.Colliding(collider))
//                    {
//                        rect.pos.X = position.X;
//                        rect.pos.Y = target.Y;
//                        rect.pos += this.velocity;
//                        move = rect.pos - pos;
//                        velocity.X = this.velocity.X;
//                        return Direction.Left;
//                    }
//                    break;
//                default:
//                    throw new NotImplementedException();
//            }
//        } while (target != rect.pos);
//        rect.pos += this.velocity;
//        move = rect.pos - pos;
//        return Direction.None;
//    }
//    public override bool OnTile(Entity entity, bool fallThrough = false)
//    {
//        if (entity is Player player)
//        {
//            return Collider.Colliding(new CRectangle(entity.position + Vector2.UnitY * player.gravDir, entity.Size)) && !fallThrough;
//        }
//        return Collider.Colliding(new CRectangle(entity.position + Vector2.UnitY, entity.Size)) && !fallThrough;
//    }
//    public override void StandingBegin(Entity entity)
//    {
//        entity.velocity.Y = 0;
//        entity.velocity.X = MathUtils.Approach(entity.velocity.X, 0, velocity.X / 2);
//    }
//    public override void StandingMoving(Entity entity)
//    {
//        entity.position += velocity;
//        entity.velocity.Y = 0;
//    }
//    public override void StandingLeaving(Entity entity)
//    {
//        entity.velocity += velocity;
//    }
//    public override Vector2 GetSafePlayerPosition(Projectile hook)
//    {
//        Vector2 target = hook.position;
//        switch (direction)
//        {
//            case Direction.Left:
//            case Direction.Right:
//                target.X = position.X;
//                break;
//            case Direction.Top:
//            case Direction.Bottom:
//                target.Y = position.Y;
//                break;
//            default:
//                throw new Exception("Invalid Platform Direction");
//        }
//        return target;
//    }
//    public override Vector2 GetSafeHookPosition(Projectile hook)
//    {
//        return hook.position;
//    }
//    public override Vector2 GetHookMovement(Projectile hook)
//    {
//        return velocity;
//    }
//    public override void Draw()
//    {
//        Main.spriteBatch.Draw(TextureType.GetValue(),
//            position - Main.screenPosition,
//            new Rectangle(0, 0, 1, (int)width),
//            Color.White,
//            direction.ToRotation(),
//            new Vector2(0, width / 2f), 1,
//            SpriteEffects.None, 0);
//    }
//}