using Everglow.Commons.CustomTiles.Tiles;
using Everglow.Commons.CustomTiles;
using Everglow.Commons.CustomTiles.DataStructures;
using Everglow.Commons.Vertex;
using Everglow.Minortopography.Common.Elevator.Tiles;
using Terraria.DataStructures;

namespace Everglow.Minortopography.Common.Elevator;

internal class PineTreeLiftTile : DBlock
{
	/// <summary>
	/// 接下来运行的方向
	/// </summary>
	internal int RunningDirection = 1;
	/// <summary>
	/// 停靠时间
	/// </summary>
	internal int PauseTime = 0;
	/// <summary>
	/// 持续加速时间
	/// </summary>
	internal int AccelerateTimeLeft = 0;
	/// <summary>
	/// 因故障卡顿的滞留时间
	/// </summary>
	internal int DetentionTime = 0;
	/// <summary>
	/// 起始Y坐标
	/// </summary>
	internal float StartCoordY = 0;
	/// <summary>
	/// 初始化的检验
	/// </summary>
	internal bool CheckDefault = false;
	/// <summary>
	/// 灯位置
	/// </summary>
	internal Vector2 LanternPos = Vector2.Zero;
	/// <summary>
	/// 灯速度
	/// </summary>
	internal Vector2 LanternVel = Vector2.Zero;

	public override void OnCollision(AABB aabb, Direction dir)
	{
	}

	public override void AI()
	{
		if (!CheckDefault)
		{
			StartCoordY = position.Y;
			CheckDefault = true;
		}
		//碰撞体积,高度要+1要不然会被吸走，紫幽可以试着修复这个Bug（<= 16高度就会被原版的物块吸附贯穿）
		size = new Vector2(96, 17);

		//电梯平台的半宽度


		if (PauseTime > 0)
		{
			//停机减速
			PauseTime--;
			velocity *= 0.9f;
			if (velocity.Length() < 0.01f)
				velocity *= 0;
		}
		else
		{
			//开机加速
			PauseTime = 0;
			if (velocity.Length() < 2f)
				velocity.Y += 0.1f * RunningDirection;
			AccelerateTimeLeft--;
		}
		if (AccelerateTimeLeft > 0)
		{
			AccelerateTimeLeft--;
			PauseTime = 0;
		}
		else
		{
			AccelerateTimeLeft = 0;
		}
		//要启动了
		if (PauseTime == 2)
			CheckRunningDirection();
		//停机时间检测
		if (Math.Abs(velocity.Y) <= 0.2f)
		{
			DetentionTime++;
			//停机太久,判定为卡死，重启
			if (DetentionTime > 300)
			{
				PauseTime = 10;
				DetentionTime = 0;
			}
		}
		else
		{
			DetentionTime = 0;
		}
		//位置太高需要重启
		if (position.Y < StartCoordY - 1)
		{
			if (PauseTime == 0 && AccelerateTimeLeft == 0)
				PauseTime = 300;
		}
		UpdateLantern();
	}
	private void UpdateLantern()
	{
		Vector2 worldPos = Center + new Vector2(1, -54) + LanternPos;
		LanternPos += LanternVel;
		LanternVel += (velocity - oldVelocity) * 1.5f;
		Point16 worldCoord = new Point16((int)(worldPos.X / 16), (int)(worldPos.Y / 16));
		Tile tile = Main.tile[Math.Clamp(worldCoord.X, 20, Main.maxTilesX - 20), Math.Clamp(worldCoord.Y, 20, Main.maxTilesY - 20)];
		if(tile.wall <= 0)
		{
			LanternVel.X += Main.windSpeedCurrent / 10f;
		}
		LanternVel -= LanternPos * 0.05f;
		LanternVel *= 0.95f;
		foreach(Player player in Main.player)
		{
			if(player.active)
			{
				if((player.Center - worldPos).Length() < 20)
				{
					if(LanternPos.Length() < 10f)
					{
						LanternVel += player.velocity * 0.5f;
					}
				}
			}
		}
	}
	private void CheckRunningDirection()
	{
		Vector2 TileCenter = Center / 16f;
		int TCX = (int)TileCenter.X;
		int TCY = (int)TileCenter.Y;
		//电梯平台的半宽度
		int Lamp = 0;
		int distanceToWinch = 1000;
		//向上1000格检索绞盘
		for (int dy = 0; dy < 1000; dy++)
		{
			if (TCX < Main.maxTilesX - 20 && TCY + dy * RunningDirection < Main.maxTilesY - 20 && TCY + dy * RunningDirection > 20 && TCX > 20)
			{
				Tile tile0 = Main.tile[TCX, TCY + dy * RunningDirection];
				if (tile0.TileType == ModContent.TileType<PineWinch>())
				{
					distanceToWinch = dy;
					if (RunningDirection == -1)
						distanceToWinch -= 12;
					break;
				}
				if (tile0.HasTile)
				{
					distanceToWinch = dy;
					if (RunningDirection == -1)
						distanceToWinch -= 12;
					break;
				}
			}
			if (distanceToWinch != 1000)
				break;
		}
		if (distanceToWinch <= 3)
		{
			PauseTime = 240;
		}
		if (Lamp == 0)
			RunningDirection *= -1;
		AccelerateTimeLeft = 60;
	}
	public override Color MapColor => new Color(122, 91, 79);
	public override void Draw()
	{
		if (position.X / 16f < Main.maxTilesX - 28 && position.Y / 16f < Main.maxTilesY - 28 && position.X / 16f > 28 && position.Y / 16f > 28)
		{
			Color drawc = Lighting.GetColor((int)(Center.X / 16f), (int)(Center.Y / 16f) - 3);
			Main.spriteBatch.Draw(ModAsset.PineLiftTile.Value, position - Main.screenPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), drawc);
			Texture2D liftShell = ModAsset.PineLiftShell.Value;
			Texture2D liftShellBack = ModAsset.PineLiftShell_background.Value;
			Texture2D liftLantern = ModAsset.PineLiftLantern.Value;
			Texture2D liftLanternFirefly = ModAsset.PineLiftLanternFirefly.Value;

			var drawcLampGlow = new Color(255, 255, 255, 0);

			Texture2D liftCable = ModAsset.VineRope.Value;

			float lightValue = MathF.Sin((float)(Main.time * 0.2)) * 0.3f + 0.4f + Math.Min(LanternVel.Length() * 0.2f, 2f);
			Main.spriteBatch.Draw(liftShellBack, Center - Main.screenPosition + new Vector2(0, -80), null, new Color(0.2f * lightValue + drawc.R / 255f * 0.2f, 0.8f * lightValue + drawc.G / 255f * 0.2f, drawc.B / 255f * 0.2f, drawc.A / 255f), 0, liftShell.Size() / 2f, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(liftLantern, Center - Main.screenPosition + new Vector2(1, -54) + LanternPos, null, drawc, (LanternPos + new Vector2(0, 20)).ToRotation() - 1.57f, liftLantern.Size() / 2f, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(liftShell, Center - Main.screenPosition + new Vector2(0, -80), null, drawc, 0, liftShell.Size() / 2f, 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(liftLanternFirefly, Center - Main.screenPosition + new Vector2(1, -52) + LanternPos + new Vector2(MathF.Sin((float)Main.time * 0.24f) * 3.1f, MathF.Sin((float)Main.time * 0.08f + 0.6f) * 5.7f), new Rectangle(0, 10 * (int)((Main.time * 0.2) % 4), 10, 10), drawcLampGlow, 0, new Vector2(5), 0.5f, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(liftLanternFirefly, Center - Main.screenPosition + new Vector2(1, -52) + LanternPos + new Vector2(MathF.Sin((float)Main.time * 0.18f + 2.6f) * 3.1f, MathF.Sin((float)Main.time * 0.09f + 4.6f) * 5.7f), new Rectangle(0, 10 * (int)((Main.time * 0.2 + 15) % 4), 10, 10), drawcLampGlow, 0, new Vector2(5), 1f, SpriteEffects.None, 0);

			Lighting.AddLight((int)(position.X / 16f) + 1, (int)(position.Y / 16f) - 3, 0.2f * lightValue, 0.8f * lightValue, 0f);

			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
			var bars = new List<Vertex2D>();
			for (int f = 0; f < 1000; f++)
			{
				Color drawcRope = Lighting.GetColor((int)(position.X / 16f) + 2, (int)((position.Y - f * 12) / 16f) - 7);

				bars.Add(new Vertex2D(Center - Main.screenPosition + new Vector2(-4, -125 - f * 12), drawcRope, new Vector3(0, f % 2, 0)));
				bars.Add(new Vertex2D(Center - Main.screenPosition + new Vector2(4, -125 - f * 12), drawcRope, new Vector3(1, f % 2, 0)));

				int dx = 1;
				if ((int)(position.X / 16f) + 2 + dx < Main.maxTilesX - 28 && (int)((position.Y - f * 12) / 16f) - 7 < Main.maxTilesY - 28 && (int)(position.X / 16f) + 2 + dx > 28 && (int)((position.Y - f * 12) / 16f) - 7 > 28)
				{
					Tile tile0 = Main.tile[(int)(position.X / 16f) + 2 + dx, Math.Max((int)((position.Y - f * 12) / 16f) - 7, 20)];
					if (tile0.TileType == ModContent.TileType<PineWinch>() && tile0.HasTile)
						break;
				}
				if(f == 999)
				{
					Kill();
					return;
				}
			}
			if (bars.Count > 2)
			{
				Main.graphics.GraphicsDevice.Textures[0] = liftCable;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
			bars = new List<Vertex2D>();
			Vector2 lanternTail = Center - Main.screenPosition + new Vector2(1, -60) + LanternPos;
			Vector2 lanternTie = Center - Main.screenPosition + new Vector2(1, -90);
			for (int x = 0;x < 10;x++)
			{
				float value = x / 10f;
				Vector2 normolized = Utils.SafeNormalize(lanternTail - lanternTie, Vector2.zeroVector).RotatedBy(1.57) * 2;
				float gravityDragY = MathF.Sqrt((0.5f - Math.Abs(value - 0.5f)) * 2) * 3;
				bars.Add(new Vertex2D(lanternTail * value + lanternTie * (1 - value) + new Vector2(0, gravityDragY) + normolized, drawc, new Vector3(0, x / 10f, 0)));
				bars.Add(new Vertex2D(lanternTail * value + lanternTie * (1 - value) + new Vector2(0, gravityDragY) - normolized, drawc, new Vector3(1, x / 10f, 0)));
			}
			if (bars.Count > 2)
			{
				Main.graphics.GraphicsDevice.Textures[0] = liftCable;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);
		}
	}
	public override void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
	{
		base.DrawToMap(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
	}
}