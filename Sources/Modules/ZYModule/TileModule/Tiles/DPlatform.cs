#define DEBUG
using Everglow.Sources.Modules.ZYModule.Commons.Core;
using Everglow.Sources.Modules.ZYModule.Commons.Core.Collide;
using Everglow.Sources.Modules.ZYModule.Commons.Core.DataStructures;
using Everglow.Sources.Modules.ZYModule.Commons.Function;

namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles;

internal abstract class DPlatform : DynamicTile, IHookable
{
    private readonly Dictionary<(Direction, Direction), Direction> hitEdges = new Dictionary<(Direction, Direction), Direction>()
    {
        [(Direction.Top, Direction.Right)] = Direction.TopRight,
        [(Direction.Right, Direction.Bottom)] = Direction.BottomRight,
        [(Direction.Bottom, Direction.Left)] = Direction.BottomLeft,
        [(Direction.Top, Direction.Left)] = Direction.TopLeft,
        [(Direction.Top, Direction.Bottom)] = Direction.Inside,
        [(Direction.Right, Direction.Left)] = Direction.Inside,
        [(Direction.Top, Direction.None)] = Direction.Top,
        [(Direction.Right, Direction.None)] = Direction.Right,
        [(Direction.Bottom, Direction.None)] = Direction.Bottom,
        [(Direction.Left, Direction.None)] = Direction.Left,
    };
    public DPlatform()
    {

    }
    protected DPlatform(Vector2 position, Vector2 velocity, float width)
    {
        this.position = position;
        this.velocity = velocity;
        this.width = width;
    }
    protected DPlatform(Vector2 position, Vector2 velocity, float width, Rotation rotation, float miu)
    {
        this.position = position;
        this.velocity = velocity;
        this.width = width;
        this.rotation = rotation;
        this.miu = miu;
    }

    /// <summary>
    /// 宽度
    /// </summary>
    public float width;
    /// <summary>
    /// 角度，面朝右为0
    /// </summary>
    public Rotation rotation;
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

        var smallBox = aabb;
        smallBox.TopLeft += Vector2.One;
        smallBox.BottomRight -= Vector2.One;
        if(smallBox.Intersect(Edge))
        {
            return Direction.None;
        }
        bool onGround = velocity.Y == 0;
        Vector2 rvel = move - this.velocity;
        Vector2 pos0 = aabb.position;
        Vector2 target = aabb.position + rvel;
        Direction result = Direction.None;
        if (Vector2.Dot(rotation.XAxis, aabb.Center - position) < 0)
        {
            return Direction.None;
        }
        aabb.position.Y = MathUtils.Approach(aabb.position.Y, target.Y, 1);
        Test.TestLog(velocity.Y);

        do
        {
            aabb.position = MathUtils.Approach(aabb.position, target, 1);
            if (Edge.Intersect(aabb, out var directions))
            {
                result = hitEdges[directions.Tuple];
                if (result == Direction.Inside)
                {
                    if (-MathHelper.PiOver4 * 3 < rotation.Angle && rotation.Angle <= -MathHelper.PiOver4 ||
                        (MathHelper.PiOver4 < rotation.Angle && rotation.Angle <= MathHelper.PiOver4 * 3))
                    {
                        result = aabb.Center.Y > position.Y ? Direction.Top : Direction.Right;
                    }
                    else
                    {
                        result = aabb.Center.X > position.X ? Direction.Left : Direction.Right;
                    }
                }
                if(result == Direction.Bottom)
                {
                    float min = Math.Min(Edge.begin.Y, Edge.end.Y);
                    aabb.position.Y = min - aabb.Height;
                    aabb.position.X = target.X;
                    velocity.Y = this.velocity.Y;
                }
                else if(result == Direction.Top)
                {
                    float max = Math.Max(Edge.begin.Y, Edge.end.Y);
                    aabb.position.Y = max;
                    aabb.position.X = target.X;
                    velocity.Y = this.velocity.Y;
                    velocity.Y = onGround && this.velocity.Y == 0 ?
                        Quick.AirSpeed :
                        this.velocity.Y;
                }
                else if(result == Direction.Right)
                {
                    float min = Math.Min(Edge.begin.X, Edge.end.X);
                    aabb.position.X = min - aabb.Width;
                    aabb.position.Y = target.Y;
                    velocity.X = this.velocity.X;
                }
                else if(result == Direction.Left)
                {
                    float max = Math.Max(Edge.begin.X, Edge.end.X);
                    aabb.position.X = max;
                    aabb.position.Y = target.Y;
                    velocity.X = this.velocity.X;
                }
                else if (Math.Tan(Math.Abs(rotation.Angle + MathHelper.PiOver2)) <= miu)
                {
                    Direction h = result & (Direction.Right | Direction.Left);
                    result = result & (~Direction.Right) & (~Direction.Left);
                    if ((rotation.Angle < 0 && result == Direction.Top) || (rotation.Angle > 0 && result == Direction.Bottom))
                    {
                        result = Direction.None;
                    }
                    else
                    {
                        aabb.position.X = target.X;
                        aabb.position.Y = (h == Direction.Left ? aabb.Left : aabb.Right) * Edge.K + Edge.B - aabb.Height;
                    }
                }
                else
                {
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
            rotation.Angle,
            new Vector2(0, width / 2f), 1,
            SpriteEffects.None, 0);
    }

    public void SetHookPosition(Projectile hook)
    {
        float proj = Vector2.Dot(hook.Center - position, rotation.YAxis);
        hook.Center = position + rotation.YAxis * proj;
    }
    public override void Leave(Entity entity)
    {
        entity.velocity += velocity;
    }
    public override void Stand(Entity entity, bool newStand)
    {
        if (newStand)
        {
            entity.velocity -= velocity;
        }
    }
}