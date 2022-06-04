using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Everglow.Sources.Modules.ZYModule.TileModule.EntityColliding;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

internal class DCircle : DynamicTile
{
    public Circle circle;
    /// <summary>
    /// 角速度，顺时针为正
    /// </summary>
    public Rotation angularVelocity;
    /// <summary>
    /// 旋转角度
    /// </summary>
    public Rotation rotation;
    /// <summary>
    /// 摩擦系数，影响是否能站立在圆上
    /// </summary>
    public float miu;
    /// <summary>
    /// 控制实体随着圆旋转距离的系数
    /// </summary>
    public float lambda;

    public DCircle(Circle circle, Rotation angularVelocity, Rotation rotation, float miu, float lambda)
    {
        this.circle = circle;
        this.angularVelocity = angularVelocity;
        this.rotation = rotation;
        this.miu = miu;
        this.lambda = lambda;
    }

    public DCircle(Circle circle, float miu)
    {
        this.circle = circle;
        this.miu = miu;
    }

    public DCircle(Circle circle)
    {
        this.circle = circle;
    }

    public DCircle()
    {
    }

    public float LinearVelocitiy => angularVelocity.Angle * circle.radius;
    public override Collider Collider => new CCircle(circle);
    public override void Leave(EntityHandler entity)
    {
        //entity.velocity += velocity;
    }

    public override Direction MoveCollision(AABB aabb, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
    {
        throw new NotImplementedException();
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
                        aabb.position.X = target.X;
                        aabb.position.X -= LinearVelocitiy * lambda;
                        result = Direction.Top;
                        break;
                    case Direction.Bottom:
                        velocity.Y = this.velocity.Y == 0 && onGround ?
                            Quick.AirSpeed :
                            this.velocity.Y;
                        aabb.position.Y = circle.position.Y - aabb.Height - circle.radius;
                        aabb.position.X = target.X;
                        aabb.position.X += LinearVelocitiy * lambda;
                        result = Direction.Bottom;
                        break;
                    case Direction.Left:
                        velocity.X = this.velocity.X;
                        aabb.position.X = circle.position.X + circle.radius;
                        aabb.position.Y = target.Y;
                        aabb.position.Y += LinearVelocitiy * lambda;
                        result = Direction.Top;
                        break;
                    case Direction.Right:
                        velocity.X = this.velocity.X;
                        aabb.position.X = circle.position.X - aabb.Width - circle.radius;
                        aabb.position.Y = target.X;
                        aabb.position.Y -= LinearVelocitiy * lambda;
                        result = Direction.Right;
                        break;
                    case Direction.TopLeft:
                        Vector2 t = velocity;
                        Rotation rot = (aabb.TopLeft - circle.position).ToRot();
                        int sign = Math.Sign(rot.XAxis.Cross(-rvel));
                        float leftRot = (target - aabb.position).Length() / circle.radius;
                        rot -= leftRot * sign;
                        velocity = -rot.YAxis * Math.Abs(velocity.Length()) * sign;
                        Debug.Assert(CollisionUtils.FloatEquals(t.Length(), velocity.Length()));
                        //aabb.position = circle.position + rot.XAxis * circle.radius + rot.YAxis * LinearVelocitiy * lambda;
                        result = Direction.TopLeft;
                        break;
                    case Direction.TopRight:
                        rot = (aabb.TopRight - circle.position).ToRot();
                        aabb.position += velocity.NormalizeSafe() * Vector2.Dot(velocity, rot.XAxis);
                        velocity = velocity.NormalizeSafe() * Vector2.Dot(velocity, rot.YAxis) + this.velocity;
                        aabb.position = circle.position + rot.XAxis * circle.radius + rot.YAxis * LinearVelocitiy * lambda;
                        result = Direction.TopRight;
                        break;
                    case Direction.BottomLeft:
                        rot = (aabb.BottomLeft - circle.position).ToRot();
                        if(Math.Tan(-MathHelper.PiOver2 - rot.Angle) <= miu)
                        {
                            velocity.Y = 0;
                            leftRot = (target.X - aabb.position.X) / circle.radius;
                            rot += (rvel.X > 0 ? 1 : -1) * leftRot;
                            aabb.position = circle.position + rot.XAxis * circle.radius + rot.YAxis * LinearVelocitiy * lambda;
                            aabb.position.Y -= aabb.Height;
                            result = Direction.Bottom;
                            break;
                        }
                        aabb.position += velocity.NormalizeSafe() * Vector2.Dot(velocity, rot.XAxis);
                        velocity = velocity.NormalizeSafe() * Vector2.Dot(velocity, rot.YAxis) + this.velocity;
                        aabb.position = circle.position + rot.XAxis * circle.radius + rot.YAxis * LinearVelocitiy * lambda;
                        result = Math.Tan(-MathHelper.PiOver2 - rot.Angle) <= miu ? Direction.Bottom : Direction.BottomLeft;
                        break;
                    case Direction.BottomRight:
                        rot = (aabb.BottomRight - circle.position).ToRot();
                        aabb.position += velocity.NormalizeSafe() * Vector2.Dot(velocity, rot.XAxis);
                        velocity = velocity.NormalizeSafe() * Vector2.Dot(velocity, rot.YAxis) + this.velocity;
                        aabb.position = circle.position + rot.XAxis * circle.radius + rot.YAxis * LinearVelocitiy * lambda;
                        result = Math.Tan(MathHelper.PiOver2 + rot.Angle) <= miu ? Direction.Bottom : Direction.BottomRight;
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

    public override void Stand(EntityHandler entity, bool newStand)
    {
        if(newStand)
        {
            entity.extraVelocity -= this.velocity;
        }
    }

    public override void Draw()
    {
        Main.spriteBatch.Draw(TextureType.Circle.GetValue(),
            circle.position - Main.screenPosition,
            null,
            Color.White,
            rotation.Angle,
            new Vector2(64, 64),
            circle.radius / 64f,
            SpriteEffects.None, 0);
    }
}