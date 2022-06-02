using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;
using Microsoft.Xna.Framework;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

internal abstract class DPlatform : DynamicTile, IHookable
{
    public DPlatform()
    {

    }
    public DPlatform(Vector2 pos, Vector2 vel, float width, Rotation rot)
    {
        position = pos;
        this.width = width;
        rotation = rot;
        velocity = vel;
    }
    /// <summary>
    /// 角度，面朝右为0
    /// </summary>
    public Rotation rotation;
    /// <summary>
    /// 宽度
    /// </summary>
    public float width;
    /// <summary>
    /// 摩擦系数
    /// </summary>
    public float miu;
    public Edge Edge => new Edge(position - rotation.YAxis * width / 2, position + rotation.YAxis * width / 2);
    public override Collider Collider => new CEdge(Edge);
    public override Direction MoveCollision(AABB aabb, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
    {
        if (ignorePlats)
        {
            return Direction.None;
        }

        AABB smallBox = aabb;
        smallBox.TopLeft += Vector2.One * 2;
        smallBox.BottomRight -= Vector2.One * 2;
        if(smallBox.Intersect(Edge))
        {
            return Direction.None;
        }

        Vector2 rvel = move - this.velocity;
        Vector2 pos0 = aabb.position;
        Vector2 target = aabb.position + rvel;
        Direction result = Direction.None;
        if(Vector2.Dot(rotation.XAxis,rvel) > 0)
        {
            return Direction.None;
        }
        do
        {
            aabb.position = MathUtils.Approach(aabb.position, target, 1);
            if(Edge.Intersect(aabb, out var point))
            {
                Vector2 offset = Vector2.Dot(point - position, rotation.YAxis) * rotation.YAxis + position - point;
                aabb.position += Vector2.Dot(target - aabb.position, rotation.YAxis) * rotation.YAxis + offset;
                float angle = rotation.Angle + MathHelper.PiOver2;
                int sign = Math.Sign(angle);
                if (Math.Tan(sign * angle) <= miu)
                {
                    result = Direction.Bottom;
                }
                else
                {
                    result = rotation.Angle switch
                    {
                        >= -MathHelper.Pi and < -MathHelper.PiOver2 => Direction.BottomRight,
                        >= -MathHelper.PiOver2 and < 0 => Direction.BottomLeft,
                        >= 0 and < MathHelper.PiOver2 => Direction.TopRight,
                        >= MathHelper.PiOver2 and < MathHelper.Pi => Direction.TopLeft,
                        _ => throw new NotImplementedException(),
                    };
                    velocity = Vector2.Dot(velocity, rotation.YAxis) * rotation.YAxis + this.velocity;
                }
                
                move = aabb.position - pos0 + this.velocity;
                break;
            }
        } while (aabb.position != target);

        return result;
    }
    public override void Draw()
    {
        Main.spriteBatch.Draw(TextureType.GetValue(),
            position - Main.screenPosition,
            new Rectangle(0, 0, 1, (int)width),
            Color.White,
            rotation,
            new Vector2(0, width / 2f), 1,
            SpriteEffects.None, 0);
    }

    public void SetHookPosition(Projectile hook)
    {
        float proj = Vector2.Dot(hook.position - position, rotation.YAxis);
        hook.position = position + rotation.YAxis * proj;
    }
    public override void Leave(Entity entity)
    {
        entity.velocity += velocity;
    }
    public override void Stand(Entity entity, bool newStand)
    {
        if(newStand)
        {
            entity.velocity -= velocity;
        }
    }
}