using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Everglow.Ocean.MiscImplementation
{
	// Token: 0x0200073D RID: 1853
	public class BezierCurve
	{
		// Token: 0x06001FB7 RID: 8119 RVA: 0x000090B7 File Offset: 0x000072B7
		public BezierCurve(params Vector2[] controls)
		{
			this.ControlPoints = controls;
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x000090C6 File Offset: 0x000072C6
		public Vector2 Evaluate(float T)
		{
			if (T < 0f)
			{
				T = 0f;
			}
			if (T > 1f)
			{
				T = 1f;
			}
			return this.PrivateEvaluate(this.ControlPoints, T);
		}

		// Token: 0x06001FB9 RID: 8121 RVA: 0x000E8614 File Offset: 0x000E6814
		public List<Vector2> GetPoints(int amount)
		{
			float num = 1f / (float)amount;
			List<Vector2> list = new List<Vector2>();
			for (float num2 = 0f; num2 <= 1f; num2 += num)
			{
				list.Add(this.Evaluate(num2));
			}
			return list;
		}

		// Token: 0x06001FBA RID: 8122 RVA: 0x000E8654 File Offset: 0x000E6854
		private Vector2 PrivateEvaluate(Vector2[] points, float T)
		{
			if (points.Length > 2)
			{
				Vector2[] array = new Vector2[points.Length - 1];
				for (int i = 0; i < points.Length - 1; i++)
				{
					array[i] = Vector2.Lerp(points[i], points[i + 1], T);
				}
				return this.PrivateEvaluate(array, T);
			}
			return Vector2.Lerp(points[0], points[1], T);
		}

		// Token: 0x04000302 RID: 770
		public Vector2[] ControlPoints;
	}
}
