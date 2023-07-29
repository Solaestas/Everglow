using System;
using Microsoft.Xna.Framework;
using Terraria;

namespace Everglow.Ocean.MiscImplementation
{
	// Token: 0x0200073E RID: 1854
	public class Circle
	{
		// Token: 0x06001FBB RID: 8123 RVA: 0x000090F3 File Offset: 0x000072F3
		public Circle(Vector2 center, float radius)
		{
			this.Center = center;
			this.Radius = radius;
		}

		// Token: 0x06001FBC RID: 8124 RVA: 0x000E86D8 File Offset: 0x000E68D8
		private Vector2 RandomPointUnitCircle()
		{
			double num = Math.Sqrt(Main.rand.NextDouble());
			double num2 = Main.rand.NextDouble() * 6.2831854820251465;
			return new Vector2((float)(num * Math.Cos(num2)), (float)(num * Math.Sin(num2)));
		}

		// Token: 0x06001FBD RID: 8125 RVA: 0x00009109 File Offset: 0x00007309
		public Vector2 RandomPointInCircle()
		{
			return this.Center + this.RandomPointUnitCircle() * this.Radius;
		}

		// Token: 0x06001FBE RID: 8126 RVA: 0x000E8724 File Offset: 0x000E6924
		public Vector2 RandomPointOnCircleEdge()
		{
			Vector2 value = this.RandomPointUnitCircle();
			value.Normalize();
			return this.Center + value * this.Radius;
		}

		// Token: 0x04000303 RID: 771
		public float Radius;

		// Token: 0x04000304 RID: 772
		public Vector2 Center;
	}
}
