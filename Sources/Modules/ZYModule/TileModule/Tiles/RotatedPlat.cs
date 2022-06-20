//using Everglow.Sources.Modules.ZYModule.Commons.Core;
//using Everglow.Sources.Modules.ZYModule.Commons.Function;

//namespace Everglow.Sources.Modules.ZYModule.TileModule.Tiles
//{
//    internal class RotatedPlat : DynamicTile
//    {
//        public float rotation;
//        public float rotVelocity;
//        public float width;
//        public override ICollider Collider => new CLine(
//            position - new Vector2(0, width / 2).RotatedBy(rotation),
//            position + new Vector2(0, width / 2).RotatedBy(rotation));
//        public Vector2 Begin => position - -new Vector2(0, width / 2).RotatedBy(rotation);
//        public Vector2 End => position + new Vector2(0, width / 2).RotatedBy(rotation);
//        public Vector2 Face => Vector2.UnitX.RotatedBy(rotation);
//        public override void Move()
//        {
//            this.position += this.oldVelocity;
//            this.rotation += this.rotVelocity;
//        }
//        public override Direction MoveCollision(CRectangle rect, ref Vector2 velocity, ref Vector2 move, bool ignorePlats = false)
//        {
//            const float StepLength = 1f;
//            Vector2 rV = velocity - move;
//            Vector2 pos = rect.pos;
//            Vector2[] vs = new Vector2[] { Begin, End, Begin + rV, End + rV };
//            ICollider collider = Collider;
//            //if(!rect.Colliding(CollisionUtils.LineToAABB(vs.MinValue(), vs.MaxValue())) ||
//            //    rect.Colliding(collider))
//            //{
//            //    return Direction.None;
//            //}
//            Vector2 target = pos + rV;
//            do
//            {
//                rect.pos = MathUtils.Approach(rect.pos, target, StepLength);
//                if (rect.Colliding(collider))
//                {
//                    var result = Direction.Top;
//                    if(new CLine(rect.BottomLeft, rect.BottomRight).Colliding(collider))
//                    {
//                        result = Direction.Bottom;
//                    }
//                    rect.pos += Face / 20 + this.velocity;
//                    move = rect.pos - pos;
//                    var normal = Face.NormalLine();
//                    velocity = normal * Vector2.Dot(normal, velocity) + this.velocity;
//                    return result;
//                }
//            } while (rect.pos != target);
//            rect.pos = pos;
//            return Direction.None;
//        }
//        public override void Draw()
//        {
//            Main.spriteBatch.Draw(TextureType.GetValue(),
//                position - Main.screenPosition,
//                new Rectangle(0, 0, 1, (int)width),
//                Color.White,
//                rotation,
//                new Vector2(0, width / 2),
//                1, SpriteEffects.None, 0);
//        }
//    }
//}
