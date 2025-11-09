using Terraria.GameContent;

namespace Everglow.Yggdrasil.KelpCurtain.Items.Weapons.Special;

public class VineEnergyOrb : ModProjectile
{
	public Vector2 StartPosition;
	public Vector2 EndPosition;
	public float Progress = 0f;
	public float Speed = 0.03f;

	// 用于存储轨迹点以实现拖尾效果
	private List<Vector2> trailPositions = new List<Vector2>();
	private List<float> trailRotations = new List<float>();
	private const int MaxTrailLength = 20;

	// 添加动态效果变量
	private float timer = 0f;
	private float pulseTimer = 0f;
	private float pulse = 0f;

	public override void SetStaticDefaults()
	{
		ProjectileID.Sets.TrailCacheLength[Projectile.type] = 6;
		ProjectileID.Sets.TrailingMode[Projectile.type] = 0;
	}

	public override void SetDefaults()
	{
		Projectile.width = 4;
		Projectile.height = 4;
		Projectile.friendly = true;
		Projectile.hostile = false;
		Projectile.penetrate = -1;
		Projectile.timeLeft = 180;
		Projectile.tileCollide = false;
		Projectile.ignoreWater = true;
		Projectile.alpha = 100;
	}

	public override void AI()
	{
		// 更新计时器
		timer += 0.1f;
		pulseTimer += 0.2f;
		pulse = (float)Math.Sin(pulseTimer) * 0.3f + 0.7f;

		// 如果位置信息为零，则从ai参数读取
		if (StartPosition == Vector2.Zero)
		{
			StartPosition = Projectile.Center;
		}
		if (EndPosition == Vector2.Zero)
		{
			EndPosition = new Vector2(Projectile.ai[0], Projectile.ai[1]);
		}

		// 移动能量球：沿着贝塞尔曲线从起点移动到终点
		Progress += Speed;

		// 计算贝塞尔曲线上的位置
		Vector2 currentPosition = CalculateBezierPosition(Progress);

		// 计算移动方向（用于拖尾旋转）
		Vector2 velocity = currentPosition - Projectile.Center;
		float rotation = velocity != Vector2.Zero ? velocity.ToRotation() : Projectile.rotation;

		Projectile.Center = currentPosition;
		Projectile.rotation = rotation;

		// 更新拖尾
		UpdateTrail(currentPosition, rotation);

		// 动态大小和透明度
		Projectile.scale = 0.3f + pulse * 0.15f; 
		Projectile.alpha = (int)(80 + Math.Sin(pulseTimer * 2) * 20);

		// 生成粒子效果
		SpawnParticles(currentPosition);

		// 如果到达终点，销毁弹幕
		if (Progress >= 1f)
		{
			Projectile.Kill();
		}
	}

	private void SpawnParticles(Vector2 position)
	{
		// 随机生成粒子
		if (Main.rand.NextBool(3))
		{
			Color[] dustColors = new Color[]
			{
				new Color(30, 120, 30),
				new Color(60, 180, 60),
				new Color(120, 220, 120),
				new Color(180, 230, 100),
				new Color(220, 255, 150),
			};

			Color dustColor = dustColors[Main.rand.Next(dustColors.Length)];
			Dust dust = Dust.NewDustPerfect(
				position + Main.rand.NextVector2Circular(10, 10),
				DustID.TerraBlade,
				Main.rand.NextVector2Circular(2, 2),
				0, dustColor, 1f);
			dust.noGravity = true;
			dust.fadeIn = 1f;
		}
	}

	private void UpdateTrail(Vector2 newPosition, float newRotation)
	{
		// 添加新位置和旋转到拖尾
		trailPositions.Insert(0, newPosition);
		trailRotations.Insert(0, newRotation);

		// 限制拖尾长度
		if (trailPositions.Count > MaxTrailLength)
		{
			trailPositions.RemoveAt(trailPositions.Count - 1);
			trailRotations.RemoveAt(trailRotations.Count - 1);
		}
	}

	private Vector2 CalculateBezierPosition(float t)
	{
		// 计算控制点：在连线中点附近偏移，形成曲线
		Vector2 lineCenter = (StartPosition + EndPosition) / 2f;
		Vector2 lineDirection = (EndPosition - StartPosition).SafeNormalize(Vector2.UnitY);
		Vector2 perpendicular = new Vector2(-lineDirection.Y, lineDirection.X);

		// 使用基于时间的偏移，使曲线有动态变化
		float offsetAmount = 40f;
		float timeOffset = (float)Main.timeForVisualEffects * 0.03f;
		float sinOffset = (float)System.Math.Sin(timeOffset + Projectile.identity * 0.5f) * offsetAmount;

		Vector2 controlPoint = lineCenter + perpendicular * sinOffset;

		// 贝塞尔曲线计算
		float u = 1 - t;
		Vector2 position = u * u * StartPosition +
						  2 * u * t * controlPoint +
						  t * t * EndPosition;

		return position;
	}

	public override bool PreDraw(ref Color lightColor)
	{
		// 绘制细长拖尾
		DrawTrail();

		// 绘制能量球头部
		DrawHead();

		return false;
	}

	private void DrawHead()
	{
		Texture2D texture = GetOrbTexture();
		Vector2 headPosition = Projectile.Center - Main.screenPosition;
		float baseScale = Projectile.scale * 0.4f; // 头部大小减半

		// 1. 外层辉光
		Color outerGlow = new Color(40, 160, 40, 60) * (Projectile.alpha / 255f);
		float outerScale = baseScale * 1.8f * pulse;

		Main.EntitySpriteDraw(
			texture,
			headPosition,
			null,
			outerGlow,
			0f,
			texture.Size() * 0.5f,
			outerScale,
			SpriteEffects.None,
			0);

		// 2. 中层光晕
		Color middleGlow = new Color(80, 200, 80, 100) * (Projectile.alpha / 255f);
		float middleScale = baseScale * 1.3f * pulse;

		Main.EntitySpriteDraw(
			texture,
			headPosition,
			null,
			middleGlow,
			0f,
			texture.Size() * 0.5f,
			middleScale,
			SpriteEffects.None,
			0);

		// 3. 主能量球
		Color mainColor = new Color(120, 240, 120, 160) * (Projectile.alpha / 255f);
		float mainScale = baseScale * pulse;

		Main.EntitySpriteDraw(
			texture,
			headPosition,
			null,
			mainColor,
			0f,
			texture.Size() * 0.5f,
			mainScale,
			SpriteEffects.None,
			0
		);

		// 4. 内层高光
		Color innerColor = new Color(180, 255, 150, 200) * (Projectile.alpha / 255f);
		float innerScale = baseScale * 0.5f * pulse;

		Main.EntitySpriteDraw(
			texture,
			headPosition,
			null,
			innerColor,
			0f,
			texture.Size() * 0.5f,
			innerScale,
			SpriteEffects.None,
			0
		);

		// 5. 中心亮点
		Color centerColor = new Color(220, 255, 200, 220) * (Projectile.alpha / 255f);
		float centerScale = baseScale * 0.2f;

		Main.EntitySpriteDraw(
			texture,
			headPosition,
			null,
			centerColor,
			0f,
			texture.Size() * 0.5f,
			centerScale,
			SpriteEffects.None,
			0
		);
	}

	private void DrawTrail()
	{
		Texture2D texture = TextureAssets.MagicPixel.Value;
		Rectangle sourceRect = new Rectangle(0, 0, 1, 1);

		for (int i = 0; i < trailPositions.Count - 1; i++)
		{
			float progress = (float)i / trailPositions.Count;
			Vector2 currentPos = trailPositions[i] - Main.screenPosition;
			Vector2 nextPos = trailPositions[i + 1] - Main.screenPosition;

			// 计算两点之间的向量
			Vector2 segmentVector = nextPos - currentPos;
			float segmentLength = segmentVector.Length();

			if (segmentLength > 0)
			{
				Vector2 segmentDirection = segmentVector / segmentLength;
				float segmentRotation = segmentDirection.ToRotation();

				// 拖尾逐渐变小和透明
				float trailAlpha = (1f - progress) * 0.7f;
				float trailWidth = 8f * (1f - progress) * 0.5f; // 宽度加倍

				// 根据拖尾位置使用不同颜色
				Color trailColor;
				if (progress < 0.3f)
				{
					trailColor = new Color(80, 220, 80, (int)(180 * trailAlpha));
				}
				else if (progress < 0.7f)
				{
					trailColor = new Color(60, 200, 60, (int)(150 * trailAlpha));
				}
				else
				{
					trailColor = new Color(40, 180, 40, (int)(120 * trailAlpha));
				}

				trailColor *= Projectile.alpha / 255f;

				// 绘制细长拖尾段 - 使用矩形纹理
				Main.EntitySpriteDraw(
					texture,
					currentPos,
					sourceRect,
					trailColor,
					segmentRotation,
					new Vector2(0, 0.5f),
					new Vector2(segmentLength, trailWidth), 
					SpriteEffects.None,
					0);

				Color innerTrailColor = new Color(120, 255, 120, (int)(200 * trailAlpha)) * (Projectile.alpha / 255f);
				float innerTrailWidth = trailWidth * 0.4f; 

				Main.EntitySpriteDraw(
					texture,
					currentPos,
					sourceRect,
					innerTrailColor,
					segmentRotation,
					new Vector2(0, 0.5f),
					new Vector2(segmentLength, innerTrailWidth),
					SpriteEffects.None,
					0
				);
			}

			if (i % 3 == 0)
			{
				Texture2D orbTexture = GetOrbTexture();
				float pointScale = 0.1f + (1f - progress) * 0.2f;
				Color pointColor = new Color(150, 255, 150, (int)(200 * (1f - progress))) * (Projectile.alpha / 255f);

				Main.EntitySpriteDraw(
					orbTexture,
					currentPos,
					null,
					pointColor,
					0f,
					orbTexture.Size() * 0.5f,
					pointScale,
					SpriteEffects.None,
					0
				);
			}
		}
	}

	private Texture2D GetOrbTexture()
	{
		return (Texture2D)ModContent.Request<Texture2D>(Texture);
	}
}