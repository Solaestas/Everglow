using Everglow.Sources.Commons.Function.Skeleton2D;
using Everglow.Sources.Commons.Function.Skeleton2D.Reader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.ModLoader;
using static Humanizer.In;
using Terraria.ID;

namespace Everglow.Example.Skeleton
{
	public class SkeletonPlayerLayer : PlayerDrawLayer
	{

		private Skeleton2D skeleton2D;
		public override void Load()
		{
			var data = ModContent.GetFileBytes("Everglow/Resources/Animations/spineboy-ess.json");
			skeleton2D = Skeleton2DReader.ReadSkeleton(data, "Everglow/Resources/Animations/");
			base.Load();
		}
		public override Position GetDefaultPosition()
		{
			return new BeforeParent(PlayerDrawLayers.ProjectileOverArm);
		}

		public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
		{
			return true;
		}

		protected override void Draw(ref PlayerDrawSet drawInfo)
		{
			var player = drawInfo.drawPlayer;
			if (player.HeldItem == null || player.HeldItem.type == ItemID.None)
			{
				skeleton2D.Position = player.Center;
				skeleton2D.Rotation = 0f;
				// skeleton2D.InverseKinematics(Main.MouseWorld);
				skeleton2D.PlayAnimation("idle", (float)Main.time % 300 / 60f);
				skeleton2D.DrawDebugView(Main.spriteBatch);
			}
		}
	}
}
