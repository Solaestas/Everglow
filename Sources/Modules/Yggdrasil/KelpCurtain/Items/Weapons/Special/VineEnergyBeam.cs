using Terraria.GameContent;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons.Special;

public class VineEnergyBeam : ModProjectile
{
	public Vector2 StartPosition;
	public Vector2 EndPosition;

	private float timer = 0f;
	private float pulseTimer = 0f;
	private float pulse = 0f;

	public override void SetStaticDefaults()
	{
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 0;
	}

	public override void SetDefaults()
	{
		Projectile.width = 2;
		Projectile.height = 2;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 2;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.alpha = 100;
	}

	public override void AI()
	{
		timer += 0.1f;
		pulseTimer += 0.2f;
		pulse = (float)Math.Sin(pulseTimer) * 0.3f + 0.7f;

		Player player = Main.player[Projectile.owner];
		if (player.active)
		{
			StartPosition = player.Center;
		}

		Projectile.Center = (StartPosition + EndPosition) / 2f;
		Projectile.rotation = (EndPosition - StartPosition).ToRotation();

		// 生成粒子效果 - 使用更深的颜色
		if (Main.rand.NextBool(5))
		{
			Vector2 lineDirection = (EndPosition - StartPosition).SafeNormalize(Vector2.UnitX);
			float lineLength = Vector2.Distance(StartPosition, EndPosition);
			Vector2 randomPos = StartPosition + lineDirection * Main.rand.NextFloat(lineLength);

			// 使用更深的绿色粒子
			Color[] dustColors = new Color[]
			{
				new Color(20, 80, 20),     // 深绿
				new Color(30, 100, 30),    // 中深绿
				new Color(40, 120, 40),    // 中绿
				new Color(60, 140, 60),    // 浅深绿
				new Color(80, 160, 80),     // 浅绿
			};

			Color dustColor = dustColors[Main.rand.Next(dustColors.Length)];
			Dust dust = Dust.NewDustPerfect(randomPos, DustID.TerraBlade, Vector2.Zero, 0, dustColor, 0.8f);
			dust.noGravity = true;
		}
	}

	public override bool PreDraw(ref Color lightColor)
	{
		Texture2D texture = TextureAssets.MagicPixel.Value;
		Vector2 start = StartPosition - Main.screenPosition;
		Vector2 end = EndPosition - Main.screenPosition;
		Vector2 direction = end - start;
		float length = direction.Length();

		if (length <= 0)
		{
			return false;
		}

		direction.Normalize();

		float alphaPulse = (float)Math.Sin(pulseTimer * 1.5f) * 0.2f + 0.8f;

		Rectangle sourceRect = new Rectangle(0, 0, 1, 1);

		// 1. 最外层暗影 - 更深的绿色
		Color shadowColor = new Color(10, 50, 10, 30) * alphaPulse;
		float shadowWidth = 14f * pulse;

		Main.EntitySpriteDraw(
			texture,
			start,
			sourceRect,
			shadowColor,
			direction.ToRotation(),
			new Vector2(0, 0.5f),
			new Vector2(length, shadowWidth),
			SpriteEffects.None,
			0
		);

		// 2. 中层辉光 - 深绿色
		Color glowColor = new Color(20, 80, 20, 60) * alphaPulse;
		float glowWidth = 10f * pulse;

		Main.EntitySpriteDraw(
			texture,
			start,
			sourceRect,
			glowColor,
			direction.ToRotation(),
			new Vector2(0, 0.5f),
			new Vector2(length, glowWidth),
			SpriteEffects.None,
			0
		);

		// 3. 主能量束 - 中深绿色
		Color mainColor = new Color(40, 120, 40, 120) * alphaPulse;
		float mainWidth = 6f * pulse;

		Main.EntitySpriteDraw(
			texture,
			start,
			sourceRect,
			mainColor,
			direction.ToRotation(),
			new Vector2(0, 0.5f),
			new Vector2(length, mainWidth),
			SpriteEffects.None,
			0
		);

		// 4. 高光核心 - 深黄绿色
		Color highlightColor = new Color(80, 180, 60, 180);
		float highlightWidth = 2.5f * pulse;

		Main.EntitySpriteDraw(
			texture,
			start,
			sourceRect,
			highlightColor,
			direction.ToRotation(),
			new Vector2(0, 0.5f),
			new Vector2(length, highlightWidth),
			SpriteEffects.None,
			0
		);

		// 5. 中心亮线 - 中黄绿色
		Color centerColor = new Color(120, 200, 100, 200);
		float centerWidth = 1f * pulse;

		Main.EntitySpriteDraw(
			texture,
			start,
			sourceRect,
			centerColor,
			direction.ToRotation(),
			new Vector2(0, 0.5f),
			new Vector2(length, centerWidth),
			SpriteEffects.None,
			0
		);

		// 6. 能量流动效果 - 使用更深的颜色
		DrawEnergyFlow(start, end, direction, length);

		return false;
	}

	private void DrawEnergyFlow(Vector2 start, Vector2 end, Vector2 direction, float length)
	{
		Texture2D flowTexture = TextureAssets.MagicPixel.Value;
		Rectangle sourceRect = new Rectangle(0, 0, 1, 1);

		// 沿着能量线绘制流动光点
		int segments = 8;
		for (int i = 0; i < segments; i++)
		{
			float progress = (i / (float)segments) + (timer * 0.3f % 1f);
			if (progress > 1f)
			{
				progress -= 1f;
			}

			Vector2 segmentPos = Vector2.Lerp(start, end, progress);

			// 使用更深的颜色
			Color segmentColor;
			if (progress < 0.3f)
			{
				segmentColor = new Color(30, 100, 30, 180); // 起点附近 - 深绿
			}
			else if (progress < 0.7f)
			{
				segmentColor = new Color(50, 140, 50, 200); // 中间 - 中深绿
			}
			else
			{
				segmentColor = new Color(70, 160, 60, 180); // 终点附近 - 深黄绿
			}

			float segmentAlpha = (float)Math.Sin(progress * MathHelper.Pi) * 0.8f;
			segmentColor *= segmentAlpha;

			float segmentSize = 4f * (float)Math.Sin(progress * MathHelper.Pi) * pulse;

			Main.EntitySpriteDraw(
				flowTexture,
				segmentPos,
				sourceRect,
				segmentColor,
				0f,
				new Vector2(0.5f, 0.5f),
				new Vector2(segmentSize, segmentSize),
				SpriteEffects.None,
				0
			);
		}

		// 绘制起点和终点的能量节点 - 使用更深的颜色
		DrawEnergyNode(start, true); // 起点
		DrawEnergyNode(end, false);  // 终点
	}

	private void DrawEnergyNode(Vector2 position, bool isStart)
	{
		Texture2D nodeTexture = TextureAssets.MagicPixel.Value;
		Rectangle sourceRect = new Rectangle(0, 0, 1, 1);

		float nodePulse = (float)Math.Sin(pulseTimer * 3f) * 0.2f + 0.8f;

		// 起点和终点使用更深的颜色
		Color nodeColor;
		float nodeSize;

		if (isStart)
		{
			// 起点 - 更深的绿色
			nodeColor = new Color(20, 100, 20, 180);
			nodeSize = 5f * nodePulse;

			// 起点的内核心
			Main.EntitySpriteDraw(
				nodeTexture,
				position,
				sourceRect,
				new Color(50, 150, 50, 160),
				0f,
				new Vector2(0.5f, 0.5f),
				new Vector2(3f * nodePulse, 3f * nodePulse),
				SpriteEffects.None,
				0
			);
		}
		else
		{
			// 终点 - 更深的黄绿色
			nodeColor = new Color(60, 160, 50, 200);
			nodeSize = 7f * nodePulse;

			// 终点的内核心
			Main.EntitySpriteDraw(
				nodeTexture,
				position,
				sourceRect,
				new Color(100, 200, 80, 180),
				0f,
				new Vector2(0.5f, 0.5f),
				new Vector2(4f * nodePulse, 4f * nodePulse),
				SpriteEffects.None,
				0
			);
		}

		Main.EntitySpriteDraw(
			nodeTexture,
			position,
			sourceRect,
			nodeColor,
			0f,
			new Vector2(0.5f, 0.5f),
			new Vector2(nodeSize, nodeSize),
			SpriteEffects.None,
			0
		);
	}
}