using Terraria.Audio;
using Everglow.Yggdrasil.Common.Elevator.Tiles;
using Everglow.Commons.CustomTiles.Tiles;
using Everglow.Commons.CustomTiles;
using Everglow.Commons.CustomTiles.DataStructures;
using Everglow.Commons.Vertex;

namespace Everglow.Yggdrasil.Common.Elevator;

internal class YggdrasilElevator : DBlock
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
	/// 电梯上的灯有没有开
	/// </summary>
	internal bool LampOn = false;
	/// <summary>
	/// 初始化的检验
	/// </summary>
	internal bool CheckDefault = false;

	public override void OnCollision(AABB aabb, Direction dir)
	{
		//此区域仅用作测试
		//Main.NewText(StartCoordY);
		//Main.NewText(DetentionTime, Color.Red);
		//Main.NewText(AccelerateTimeLeft, Color.Green);
		//Main.NewText(PauseTime, Color.Blue);
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

		Vector2 TileCenter = Center / 16f;
		int TCX = (int)TileCenter.X;
		int TCY = (int)TileCenter.Y;
		//电梯平台的半宽度
		int TCWidth = (int)(size.X / 32f);

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
			if (PauseTime == 0)
			{
				//检测站台,到了就停
				for (int dx = 0; dx < 5; dx++)
				{
					if (TCX - (TCWidth + dx) < Main.maxTilesX - 20 && TCX + TCWidth + dx < Main.maxTilesX - 20 && TCY + (1 - RunningDirection) * RunningDirection < Main.maxTilesY - 20 && TCY + (1 - RunningDirection) * RunningDirection > 20 && TCX + TCWidth + dx > 20 && TCX - (TCWidth + dx) > 20)
					{
						Tile tileleft = Main.tile[TCX - (TCWidth + dx), TCY + (1 - RunningDirection) * RunningDirection];
						Tile tileright = Main.tile[TCX + TCWidth + dx, TCY + (1 - RunningDirection) * RunningDirection];
						if (tileleft.TileType == ModContent.TileType<LiftLamp>() && tileleft.TileFrameY == 36)
							PauseTime = 240;
						if (tileright.TileType == ModContent.TileType<LiftLamp>() && tileright.TileFrameY == 36)
							PauseTime = 240;
					}
				}
			}
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
	}
	private void CheckRunningDirection()
	{
		Vector2 TileCenter = Center / 16f;
		int TCX = (int)TileCenter.X;
		int TCY = (int)TileCenter.Y;
		//电梯平台的半宽度
		int TCWidth = (int)(size.X / 32f);
		int Lamp = 0;
		int distanceToWinch = 1000;
		//向上1000格检索绞盘
		for (int dy = 0; dy < 1000; dy++)
		{
			if (TCX < Main.maxTilesX - 20 && TCY + dy * RunningDirection < Main.maxTilesY - 20 && TCY + dy * RunningDirection > 20 && TCX > 20)
			{
				Tile tile0 = Main.tile[TCX, TCY + dy * RunningDirection];
				if (tile0.TileType == ModContent.TileType<Winch>())
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
		if (distanceToWinch > 3)
		{
			for (int dy = 0; dy < distanceToWinch; dy++)
			{
				for (int dx = 0; dx < 5; dx++)
				{
					if (TCX - (TCWidth + dx) < Main.maxTilesX - 20 && TCX + TCWidth + dx < Main.maxTilesX - 20 && TCY + dy * RunningDirection < Main.maxTilesY - 20 && TCY + dy * RunningDirection > 20 && TCX + TCWidth + dx > 20 && TCX - (TCWidth + dx) > 20)
					{
						Tile tileleft = Main.tile[TCX - (TCWidth + dx), TCY + dy * RunningDirection];
						Tile tileright = Main.tile[TCX + TCWidth + dx, TCY + dy * RunningDirection];
						if (tileleft.TileType == ModContent.TileType<LiftLamp>() && tileleft.TileFrameY == 36)
							Lamp++;
						if (tileright.TileType == ModContent.TileType<LiftLamp>() && tileright.TileFrameY == 36)
							Lamp++;
					}
				}
			}
		}
		else
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
		//绘制区完全不用管
		if (position.X / 16f < Main.maxTilesX - 28 && position.Y / 16f < Main.maxTilesY - 28 && position.X / 16f > 28 && position.Y / 16f > 28)
		{
			Color drawc = Lighting.GetColor((int)(Center.X / 16f), (int)(Center.Y / 16f) - 3);
			Main.spriteBatch.Draw(YggdrasilContent.QuickTexture("Common/Elevator/SkyTreeLift"), position - Main.screenPosition, new Rectangle(0, 0, (int)size.X, (int)size.Y), drawc);
			Texture2D LiftFramework = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellLightOff");

			if (LampOn)
				Lighting.AddLight((int)(position.X / 16f) + 1, (int)(position.Y / 16f) - 3, 1f, 0.8f, 0f);

			var drawcLampGlow = new Color(255, 255, 255, 0);

			Texture2D LiftLampOff = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellLightOff");
			Texture2D LiftLampOn = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellLampOn");
			Texture2D LiftLampGlow = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellLampOnGlow");
			Texture2D LiftRopeTop = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftRope");
			Texture2D LiftRope = YggdrasilContent.QuickTexture("Common/Elevator/Textures/Rope");

			Main.spriteBatch.Draw(LiftFramework, Center - Main.screenPosition + new Vector2(0, -46), null, drawc, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
			Main.spriteBatch.Draw(LiftRopeTop, Center - Main.screenPosition + new Vector2(0, -110), null, drawc, 0, new Vector2(48, 15), 1, SpriteEffects.None, 0);

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
					Tile tile0 = Main.tile[(int)(position.X / 16f) + 2 + dx, (int)((position.Y - f * 12) / 16f) - 7];
					if (tile0.TileType == ModContent.TileType<Winch>() && tile0.HasTile)
						break;
				}
			}
			if (bars.Count > 2)
			{
				Main.graphics.GraphicsDevice.Textures[0] = LiftRope;
				Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, bars.ToArray(), 0, bars.Count - 2);
			}
			Main.spriteBatch.End();
			Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.GameViewMatrix.TransformationMatrix);

			if (!LampOn)
				Main.spriteBatch.Draw(LiftLampOff, Center - Main.screenPosition + new Vector2(0, -46), null, drawc, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
			else
			{
				Main.spriteBatch.Draw(LiftLampOn, Center - Main.screenPosition + new Vector2(0, -46), null, drawc, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
				Main.spriteBatch.Draw(LiftLampGlow, Center - Main.screenPosition + new Vector2(0, -46), null, drawcLampGlow, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
			}
			Vector2 ButtomPosition = new Vector2(-11, -33) + Center;
			if ((Main.MouseWorld - ButtomPosition).Length() < 10)
			{
				if (Main.SmartCursorIsUsed)
				{
					Texture2D LiftButtomHighLight = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellMiddleButtomHightLight");
					if (LampOn)
						LiftButtomHighLight = YggdrasilContent.QuickTexture("Common/Elevator/Textures/SkyTreeLiftShellMiddleButtomOnHightLight");
					Main.spriteBatch.Draw(LiftButtomHighLight, Center - Main.screenPosition + new Vector2(0, -46), null, Color.White, 0, new Vector2(48, 54), 1, SpriteEffects.None, 0);
				}
				if (Main.mouseRight && Main.mouseRightRelease)
				{
					SoundEngine.PlaySound(SoundID.Unlock, ButtomPosition);
					LampOn = !LampOn;
				}
			}
		}
	}
	public override void DrawToMap(Vector2 mapTopLeft, Vector2 mapX2Y2AndOff, Rectangle? mapRect, float mapScale)
	{
		base.DrawToMap(mapTopLeft, mapX2Y2AndOff, mapRect, mapScale);
	}
}
public class ElevatorSummonSystem : ModSystem
{
	public override void PostUpdateEverything()
	{
		if (Main.mouseRight && Main.mouseRightRelease)
		{

		}
	}
}