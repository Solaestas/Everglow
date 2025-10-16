using Everglow.Commons.Physics.Colliders;
using Everglow.Commons.Physics.DataStructures;
using Everglow.Commons.Utilities;
using Microsoft.Xna.Framework;

namespace Everglow.UnitTests.Function.Physics;

[TestClass]
public class ColliderTest
{
	public TestContext TestContext { get; set; }

	[TestMethod]
	public void TestEdgeCollider2D_Contains()
	{
		// 测试用例 1: 点在水平线段上
		var edge1 = new Edge(new Vector2(0, 0), new Vector2(10, 0));
		var collider1 = new EdgeCollider2D(edge1);

		Assert.IsTrue(collider1.Contains(new Vector2(5, 0))); // 线段中点
		Assert.IsTrue(collider1.Contains(new Vector2(0, 0))); // 起点
		Assert.IsTrue(collider1.Contains(new Vector2(10, 0))); // 终点
		Assert.IsFalse(collider1.Contains(new Vector2(5, 1))); // 在线段上方
		Assert.IsFalse(collider1.Contains(new Vector2(-1, 0))); // 在线段左侧延长线上
		Assert.IsFalse(collider1.Contains(new Vector2(11, 0))); // 在线段右侧延长线上

		// 测试用例 2: 点在垂直线段上
		var edge2 = new Edge(new Vector2(0, 0), new Vector2(0, 10));
		var collider2 = new EdgeCollider2D(edge2);

		Assert.IsTrue(collider2.Contains(new Vector2(0, 5))); // 线段中点
		Assert.IsTrue(collider2.Contains(new Vector2(0, 0))); // 起点
		Assert.IsTrue(collider2.Contains(new Vector2(0, 10))); // 终点
		Assert.IsFalse(collider2.Contains(new Vector2(1, 5))); // 在线段右侧
		Assert.IsFalse(collider2.Contains(new Vector2(0, -1))); // 在线段下方延长线上
		Assert.IsFalse(collider2.Contains(new Vector2(0, 11))); // 在线段上方延长线上

		// 测试用例 3: 点在斜线段上
		var edge3 = new Edge(new Vector2(0, 0), new Vector2(10, 10));
		var collider3 = new EdgeCollider2D(edge3);

		Assert.IsTrue(collider3.Contains(new Vector2(5, 5))); // 线段中点
		Assert.IsTrue(collider3.Contains(new Vector2(2, 2))); // 线段上的点
		Assert.IsTrue(collider3.Contains(new Vector2(8, 8))); // 线段上的点
		Assert.IsFalse(collider3.Contains(new Vector2(5, 6))); // 不在线段上
		Assert.IsFalse(collider3.Contains(new Vector2(6, 5))); // 不在线段上
		Assert.IsFalse(collider3.Contains(new Vector2(-1, -1))); // 在线段延长线上

		// 测试用例 4: 退化线段（起点和终点相同）
		var edge4 = new Edge(new Vector2(5, 5), new Vector2(5, 5));
		var collider4 = new EdgeCollider2D(edge4);

		Assert.IsTrue(collider4.Contains(new Vector2(5, 5))); // 与退化点重合
		Assert.IsFalse(collider4.Contains(new Vector2(5, 6))); // 不在退化点上
		Assert.IsFalse(collider4.Contains(new Vector2(6, 5))); // 不在退化点上

		// 测试用例 5: 边界情况，接近但不完全在线段上
		var edge5 = new Edge(new Vector2(0, 0), new Vector2(10, 0));
		var collider5 = new EdgeCollider2D(edge5);

		// 由于浮点数精度，这些点应该返回 false
		Assert.IsFalse(collider5.Contains(new Vector2(5, CollisionUtils.Epsilon * 1.01f)));
		Assert.IsFalse(collider5.Contains(new Vector2(5, -CollisionUtils.Epsilon * 1.01f)));

		// 但在容差范围内的点应该返回 true
		Assert.IsTrue(collider5.Contains(new Vector2(5, CollisionUtils.Epsilon / 2f)));
		Assert.IsTrue(collider5.Contains(new Vector2(5, -CollisionUtils.Epsilon / 2f)));
	}
}