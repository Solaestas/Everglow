using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

internal class DCircle : DynamicTile
{
    public Circle circle;
    public override Collider Collider => new CCircle(circle);
    public Rotation angularVelocity;
    public Rotation rotation;
    public float μ;
    public override void Leave(Entity entity)
    {
        entity.velocity += velocity;
    }

    public override Direction MoveCollision(AABB aabb, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
    {
        Vector2 rvel = move - this.velocity;
        Vector2 pos0 = aabb.position;
        Vector2 target = aabb.position + rvel;
        if (!circle.Intersect(aabb.Scan(rvel)))
        {
            return Direction.None;
        }

        var smallCircle = new Circle(circle.position, circle.radius - 7);
        if(smallCircle.Intersect(aabb))
        {
            return Direction.Inside;
        }

        bool onGround = velocity.Y == 0;
        Direction result = Direction.None;
        do
        {
            aabb.position = MathUtils.Approach(aabb.position, target, 1);
            if (circle.Intersect(aabb, out var dir))
            {
                switch (dir)
                {
                    case Direction.Top:
                        velocity.Y = this.velocity.Y == 0 && onGround ?
                            Quick.AirSpeed :
                            this.velocity.Y;
                        aabb.position.Y = circle.position.Y + circle.radius;
                        aabb.position.X -= angularVelocity * circle.radius;
                        result = Direction.Top;
                        break;
                    case Direction.Bottom:
                        velocity.Y = this.velocity.Y == 0 && onGround ?
                            Quick.AirSpeed :
                            this.velocity.Y;
                        aabb.position.Y = circle.position.Y - aabb.Height;
                        aabb.position.X += angularVelocity * circle.radius;
                        result = Direction.Bottom;
                        break;
                    case Direction.Left:
                        velocity.X = this.velocity.X;
                        aabb.position.X = circle.position.X + circle.radius;
                        aabb.position.Y += angularVelocity * circle.radius;
                        result = Direction.Top;
                        break;
                    case Direction.Right:
                        velocity.X = this.velocity.X;
                        aabb.position.X = circle.position.X - aabb.Width;
                        aabb.position.Y -= angularVelocity * circle.radius;
                        result = Direction.Right;
                        break;
                    case Direction.TopLeft:
                        
                        break;
                    case Direction.TopRight:
                        break;
                    case Direction.BottomLeft:
                        break;
                    case Direction.BottomRight:
                        break;
                    default:
                        break;
                }
                move = aabb.position - pos0 + this.velocity;
                break;
            }
        } while (aabb.position != target);
        return result;
    }

    public override void Stand(Entity entity, bool newStand)
    {

    }
}