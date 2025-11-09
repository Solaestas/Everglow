using Everglow.Commons.DataStructures;
using Everglow.Commons.Physics.MassSpringSystem;
using Everglow.Commons.TileHelper;
using Everglow.Yggdrasil.KelpCurtain.Items.Weapons.Special;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using Terraria.GameContent.Drawing;
using static Everglow.Commons.TileHelper.HangingTile_LengthAdjustingSystem;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.ForestRainVines;

public class ForestRainVineTile_Thin : HangingTile
{
	public override void InitHanging()
	{
		MaxWireStyle = 1;
		CanGrasp = true;

		RopeUnitMass = 3f;
		SingleLampMass = 250f;
		Elasticity = 200f;
		UnitLength = 6f;
	}

	public override bool TileFrame(int i, int j, ref bool resetFrame, ref bool noBreak)
	{
		if(j >= 20)
		{
			Tile tileUp = Main.tile[i, j - 1];
			if(tileUp.Slope != SlopeType.Solid)
			{
				tileUp.Slope = SlopeType.Solid;
			}
		}
		return base.TileFrame(i, j, ref resetFrame, ref noBreak);
	}

	public override void DrawCable(Rope rope, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing, Color color = default)
	{
		base.DrawCable(rope, pos, spriteBatch, tileDrawing, color);
	}

	public override void DrawWinch(int i, int j, SpriteBatch spriteBatch)
	{
		Color lightColor = Lighting.GetColor(i, j);
		var zero = new Vector2(Main.offScreenRange, Main.offScreenRange);
		if (Main.drawToScreen)
		{
			zero = Vector2.Zero;
		}
		Texture2D tex = (Texture2D)ModContent.Request<Texture2D>(Texture);
		Rectangle frame = new Rectangle(0, 0, 16, 16);
		spriteBatch.Draw(tex, new Vector2(i, j) * 16 - Main.screenPosition + zero, frame, lightColor, 0, Vector2.zeroVector, 1, SpriteEffects.None, 0);
	}

	public override void DrawRopeUnit(SpriteBatch spriteBatch, Texture2D texture, Vector2 drawPos, Point tilePos, Rope rope, int index, float rotation, Color tileLight)
	{
		var masses = rope.Masses;
		Rectangle frame;
		Vector2 offset = new Vector2(0, 0);
		drawPos += offset;
		int randomByPos = tilePos.X;
		if (index <= masses.Length - 10)
		{
			frame = new Rectangle(0, 8 + ((index + randomByPos) % 18) * 8, 16, 8);
			spriteBatch.Draw(texture, drawPos, frame, tileLight, rotation, frame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		}
		else
		{
			int contraryIndex = masses.Length - index;
			frame = new Rectangle(0, 152 + (10 - contraryIndex) * 8, 16, 8);
			spriteBatch.Draw(texture, drawPos, frame, tileLight, rotation, frame.Size() * 0.5f, 1f, SpriteEffects.None, 0);
		}
	}

	public override void OnAdjustmentStart(Point fixPoint, Player player)
	{
		// 检查是否手持藤曼修理魔杖
		if (player.HeldItem?.type == ModContent.ItemType<VineRepairWand>())
		{
			// 直接调用法杖的开始调整方法
			(player.HeldItem.ModItem as VineRepairWand)?.StartAdjustment(player, fixPoint);
		}
	}

	public override void OnAdjustmentUpdate(Point fixPoint, Player player, int deltaLength)
	{
		// 检查是否手持藤曼修理魔杖
		if (player.HeldItem?.type == ModContent.ItemType<VineRepairWand>())
		{
			// 直接调用法杖的更新调整方法
			(player.HeldItem.ModItem as VineRepairWand)?.UpdateAdjustment(player, fixPoint, deltaLength);
		}
		else
		{
			// 如果没有手持法杖，结束调整
			OnAdjustmentEnd(fixPoint, player);
		}
	}

	public override void OnAdjustmentEnd(Point fixPoint, Player player)
	{
		// 直接调用法杖的结束调整方法
		if (player.HeldItem?.type == ModContent.ItemType<VineRepairWand>())
		{
			(player.HeldItem.ModItem as VineRepairWand)?.EndAdjustment();
		}
		else
		{
			// 即使没有手持法杖，也尝试结束调整
			var wand = ModContent.GetInstance<VineRepairWand>();
			wand.EndAdjustment();
		}
	}

	/// <summary>
	/// 当手持藤曼修理魔杖时允许调整长度
	/// </summary>
	/// <param name="i"></param>
	/// <param name="j"></param>
	/// <returns></returns>
	public override bool IsCanRightClick(int i, int j)
	{
		Player player = Main.LocalPlayer;
		return player.HeldItem != null && player.HeldItem.type == ModContent.ItemType<VineRepairWand>();
	}

	#region 按钮重绘
	public override void DrawDefaultPanel(HangingTile_LengthAdjustingSystem hangingSystem, Player player, Color color, ref Queue<DrawStack> drawStacks)
	{
		drawStacks = new Queue<DrawStack>();

		// 背景改为淡绿色半透明（保持父类面板结构）
		DrawBackgroundPanel(hangingSystem, new Color(0.8f, 1f, 0.8f, 0.5f), ref drawStacks);

		float maxCos = 0;
		int maxK = 0;
		Vector2 rotCenter = hangingSystem.FixPoint.ToWorldCoordinates();
		// 完全保留父类的旋转计算逻辑，不添加摆动偏移
		Vector2 cut = new Vector2(1, 0).RotatedBy(hangingSystem.HandleRotation + MathHelper.Pi);

		// 保留父类的校准尾迹计算逻辑
		for (int k = -10; k < 10; k++)
		{
			Vector2 cut2 = new Vector2(0, -1).RotatedBy(k / 20f * MathHelper.TwoPi);
			float cosValue = Vector2.Dot(cut, cut2);
			if (cosValue > maxCos)
			{
				maxCos = cosValue;
				maxK = k;
			}
		}

		// 绘制校准尾迹（仅替换为绿色主题）
		// 基础色改为绿色系，保留原有透明度逻辑
		Color baseGreen = new Color(0.3f, 0.8f, 0.4f); // 主绿色
		Color newDrawColor = Color.Lerp(baseGreen, color, 0.2f); // 轻微融合原色调

		float nowFrameY = hangingSystem.StartFrameY60 / 60f + hangingSystem.HandleRotation * 2;
		// 警告色改为绿红色渐变（保持原有警告逻辑）
		if (nowFrameY < 5)
		{
			newDrawColor = Color.Lerp(newDrawColor, new Color(0.9f, 0.3f, 0.3f, 0.8f), (5 - nowFrameY) / 4f);
		}
		if (nowFrameY > MaxCableLength - 5)
		{
			newDrawColor = Color.Lerp(newDrawColor, new Color(0.9f, 0.3f, 0.3f, 0.8f), (nowFrameY - (MaxCableLength - 5)) / 4f);
		}

		// 完全保留父类的透明度计算逻辑
		float tK = 12f;
		if (hangingSystem.TimeToKill > 0)
		{
			tK = hangingSystem.TimeToKill;
		}
		float fade = Math.Min(hangingSystem.Timer, tK) / 12f;

		// === 扩展效果1：添加能量脉动光环 ===
		DrawEnergyPulse(hangingSystem, rotCenter, fade, ref drawStacks);

		// 绘制线条（保持父类的长度、粗细和角度逻辑，仅改颜色）
		for (int k = -10; k < 10; k++)
		{
			Vector2 cut2 = new Vector2(0, -1).RotatedBy(k / 20f * MathHelper.TwoPi);
			float cosValue = Vector2.Dot(cut, cut2);

			if (k == maxK)
			{
				// 保留黑色轮廓，确保与父类风格一致
				DrawLine_Black(
					hangingSystem,
					rotCenter + cut2 * (4 * hangingSystem.PanelRange - 12),
					rotCenter + cut2 * (4 * hangingSystem.PanelRange + 36),
					16, ref drawStacks);

				// 主线条替换为绿色
				DrawLine(
					rotCenter + cut2 * (4 * hangingSystem.PanelRange - 12),
					rotCenter + cut2 * (4 * hangingSystem.PanelRange + 36),
					16, newDrawColor * fade, 1f, ref drawStacks);

				// === 扩展效果2：为主线条添加能量流动效果 ===
				DrawEnergyFlowAlongLine(
					hangingSystem,
					rotCenter + cut2 * (4 * hangingSystem.PanelRange - 12),
					rotCenter + cut2 * (4 * hangingSystem.PanelRange + 36),
					newDrawColor, fade, ref drawStacks);
			}
			else
			{
				// 副线条替换为绿色
				DrawLine(
					rotCenter + cut2 * 4 * hangingSystem.PanelRange,
					rotCenter + cut2 * (4 * hangingSystem.PanelRange + 24),
					12, newDrawColor * fade, cosValue, ref drawStacks);
			}
		}

		// 边界颜色替换为深绿色（保持父类绘制逻辑）
		DrawBound(hangingSystem, new Color(0.2f, 0.6f, 0.3f), player, ref drawStacks);

		// 方向环保持父类的稳定角度，仅替换为绿色
		DrawDirectionRing(hangingSystem, newDrawColor,
			hangingSystem.HandleRotation - MathHelper.PiOver2,
			ref drawStacks);

		// === 扩展效果3：添加旋转粒子效果 ===
		DrawRotatingParticles(hangingSystem, rotCenter, newDrawColor, fade, ref drawStacks);

		// === 扩展效果4：添加中心能量核心 ===
		DrawEnergyCore(hangingSystem, rotCenter, newDrawColor, fade, ref drawStacks);
	}

	private void DrawEnergyPulse(HangingTile_LengthAdjustingSystem hangingSystem, Vector2 center, float fade, ref Queue<DrawStack> drawStacks)
	{
		float pulseTime = (float)Main.timeForVisualEffects * 0.05f;
		float pulseScale = 1f + (float)Math.Sin(pulseTime) * 0.2f;

		// 外层脉动光环
		Color pulseColor = new Color(0.4f, 0.9f, 0.5f, 0.3f) * fade;
		DrawPulseRing(center, 80f * pulseScale, 6f, pulseColor, ref drawStacks);

		// 内层脉动光环
		float innerPulseTime = pulseTime + MathHelper.PiOver2;
		float innerPulseScale = 1f + (float)Math.Sin(innerPulseTime) * 0.15f;
		Color innerPulseColor = new Color(0.6f, 1f, 0.7f, 0.4f) * fade;
		DrawPulseRing(center, 60f * innerPulseScale, 4f, innerPulseColor, ref drawStacks);
	}

	private void DrawPulseRing(Vector2 center, float radius, float width, Color color, ref Queue<DrawStack> drawStacks)
	{
		int segments = 24;
		for (int i = 0; i < segments; i++)
		{
			float angle1 = i / (float)segments * MathHelper.TwoPi;
			float angle2 = (i + 1) / (float)segments * MathHelper.TwoPi;

			Vector2 start = center + new Vector2((float)Math.Cos(angle1), (float)Math.Sin(angle1)) * radius;
			Vector2 end = center + new Vector2((float)Math.Cos(angle2), (float)Math.Sin(angle2)) * radius;
			DrawLine(start, end, width, color, 1f, ref drawStacks);
		}
	}

	// === 扩展方法：沿线条绘制能量流动效果 ===
	private void DrawEnergyFlowAlongLine(HangingTile_LengthAdjustingSystem hangingSystem, Vector2 start, Vector2 end, Color baseColor, float fade, ref Queue<DrawStack> drawStacks)
	{
		float flowTime = (float)Main.timeForVisualEffects * 0.1f;
		Vector2 direction = Vector2.Normalize(end - start);
		float length = Vector2.Distance(start, end);

		// 在主要线条上添加流动光点
		int flowPoints = 8;
		for (int i = 0; i < flowPoints; i++)
		{
			float progress = (i / (float)flowPoints) + (flowTime % 1f);
			if (progress > 1f)
			{
				progress -= 1f;
			}

			Vector2 pointPos = start + direction * length * progress;

			// 流动光点的颜色和大小 - 修复颜色计算
			float pointAlpha = (float)Math.Sin(progress * MathHelper.Pi) * 0.8f;
			Color pointColor = new Color(0.8f, 1f, 0.8f, pointAlpha) * fade;
			float pointSize = 4f * (float)Math.Sin(progress * MathHelper.Pi);

			// 绘制流动光点
			DrawEnergyPoint(pointPos, pointSize, pointColor, ref drawStacks);
		}
	}

	private void DrawEnergyPoint(Vector2 position, float size, Color color, ref Queue<DrawStack> drawStacks)
	{
		// 使用两条交叉线绘制能量点
		float halfSize = size * 0.5f;

		// 水平线
		DrawLine(position - new Vector2(halfSize, 0), position + new Vector2(halfSize, 0),
				 size * 0.6f, color, 1f, ref drawStacks);

		// 垂直线
		DrawLine(position - new Vector2(0, halfSize), position + new Vector2(0, halfSize),
				 size * 0.6f, color, 1f, ref drawStacks);
	}

	private void DrawRotatingParticles(HangingTile_LengthAdjustingSystem hangingSystem, Vector2 center, Color baseColor, float fade, ref Queue<DrawStack> drawStacks)
	{
		float rotationTime = (float)Main.timeForVisualEffects * 0.03f;
		int particleCount = 6;
		float orbitRadius = 45f;

		for (int i = 0; i < particleCount; i++)
		{
			float angle = rotationTime + i * MathHelper.TwoPi / particleCount;
			float pulse = 0.7f + (float)Math.Sin(rotationTime * 3f + i) * 0.3f;

			Vector2 particlePos = center + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * orbitRadius;

			// 修复颜色计算 - 使用Color.Lerp然后乘以fade
			Color particleColor = Color.Lerp(baseColor, new Color(1f, 1f, 0.8f), 0.3f);
			particleColor = new Color(particleColor.R, particleColor.G, particleColor.B, (byte)(150 * pulse)) * fade;

			float particleSize = 3f * pulse;

			DrawEnergyPoint(particlePos, particleSize, particleColor, ref drawStacks);
		}
	}

	// === 扩展方法：绘制中心能量核心 ===
	private void DrawEnergyCore(HangingTile_LengthAdjustingSystem hangingSystem, Vector2 center, Color baseColor, float fade, ref Queue<DrawStack> drawStacks)
	{
		float corePulse = 0.8f + (float)Math.Sin(Main.timeForVisualEffects * 0.08f) * 0.2f;

		// 外层核心光晕
		Color outerCoreColor = new Color(0.5f, 1f, 0.6f, 0.4f) * fade;
		DrawPulseRing(center, 12f * corePulse, 4f, outerCoreColor, ref drawStacks);

		// 内层核心
		Color innerCoreColor = new Color(0.7f, 1f, 0.8f, 0.6f) * fade;
		DrawPulseRing(center, 6f * corePulse, 3f, innerCoreColor, ref drawStacks);

		// 中心亮点
		Color centerColor = new Color(1f, 1f, 0.9f, 0.8f) * fade;
		DrawEnergyPoint(center, 4f * corePulse, centerColor, ref drawStacks);
	}
	#endregion
}