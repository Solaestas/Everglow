using Everglow.Commons.Utilities;
using Microsoft.Xna.Framework;

namespace Everglow.UnitTests.Core;

[TestClass]
public class GraphicsUtilsTest
{
	[TestMethod]
	public void BezierCurve_NoException()
	{
		// 测试用例 1: 3个随机点
		Vector2[] testArray1 = new Vector2[]
		{
			new Vector2(0.34f, -1.23f),
			new Vector2(2.45f, 0.78f),
			new Vector2(-1.56f, 3.21f),
		};

		// 测试用例 2: 4个点，包含零值和边界值
		Vector2[] testArray2 = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(5.67f, -0.89f),
			new Vector2(-3.45f, 2.34f),
			new Vector2(1.23f, 4.56f),
		};

		// 测试用例 3: 5个点，模拟路径
		Vector2[] testArray3 = new Vector2[]
		{
			new Vector2(0f, 0f),
			new Vector2(1.5f, 0.8f),
			new Vector2(2.7f, -0.3f),
			new Vector2(3.2f, 1.9f),
			new Vector2(4.5f, 0.5f),
		};

		// 测试用例 4: 6个点，X轴递增
		Vector2[] testArray4 = new Vector2[]
		{
			new Vector2(0.1f, 2.3f),
			new Vector2(1.2f, -1.1f),
			new Vector2(2.3f, 0.5f),
			new Vector2(3.4f, 3.2f),
			new Vector2(4.5f, -2.8f),
			new Vector2(5.6f, 1.7f),
		};

		// 测试用例 5: 8个点，对称分布
		Vector2[] testArray5 = new Vector2[]
		{
			new Vector2(-2.0f, 1.0f),
			new Vector2(-1.5f, 2.0f),
			new Vector2(-1.0f, 1.5f),
			new Vector2(-0.5f, 0.0f),
			new Vector2(0.5f, 0.0f),
			new Vector2(1.0f, -1.5f),
			new Vector2(1.5f, -2.0f),
			new Vector2(2.0f, -1.0f),
		};

		List<Vector2> _;
		_ = GraphicsUtils.BezierCurve(testArray1);
		_ = GraphicsUtils.BezierCurve(testArray2);
		_ = GraphicsUtils.BezierCurve(testArray3);
		_ = GraphicsUtils.BezierCurve(testArray4);
		_ = GraphicsUtils.BezierCurve(testArray5);
	}

	[TestMethod]
	public void BezierCurve_EmptyArray_NoException()
	{
		// 空数组
		Vector2[] testArray6 = [];
		List<Vector2> result = GraphicsUtils.BezierCurve(testArray6);
		Assert.IsNotNull(result);
		Assert.IsEmpty(result);
	}

	[TestMethod]
	public void BezierCurve_ElementLessThanFour_NoChange()
	{
		var a = new Vector2(0.0f, 0.0f);
		var b = new Vector2(1.0f, 1.0f);
		var c = new Vector2(2.0f, 2.0f);

		// 单元素数组
		Vector2[] testArray7 = [a];
		List<Vector2> result = GraphicsUtils.BezierCurve(testArray7);
		Assert.IsNotNull(result);
		Assert.HasCount(testArray7.Length, result);
		Assert.AreEqual(a, result[0]);

		// 两个元素的数组
		Vector2[] testArray8 = [a, b];
		result = GraphicsUtils.BezierCurve(testArray8);
		Assert.IsNotNull(result);
		Assert.HasCount(testArray8.Length, result);
		Assert.AreEqual(a, result[0]);
		Assert.AreEqual(b, result[1]);

		// 三个元素的数组
		Vector2[] testArray9 = [a, b, c];
		result = GraphicsUtils.BezierCurve(testArray9);
		Assert.IsNotNull(result);
		Assert.HasCount(testArray9.Length, result);
		Assert.AreEqual(a, result[0]);
		Assert.AreEqual(b, result[1]);
		Assert.AreEqual(c, result[2]);
	}

	[TestMethod]
	public void BezierCurve_NaNOrInfinityValues_ThrowsArgumentOutOfRangeException()
	{
		// 包含NaN或Infinity的数组
		Vector2[] testArray10 = new Vector2[]
		{
			new Vector2(0.0f, 0.0f),
			new Vector2(float.NaN, 1.0f),
			new Vector2(1.0f, float.PositiveInfinity),
			new Vector2(0.0f, 0.0f),
		};

		Assert.ThrowsExactly<ArgumentOutOfRangeException>(() => GraphicsUtils.BezierCurve(testArray10));
	}
}