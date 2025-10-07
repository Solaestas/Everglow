using Everglow.Commons.Physics.MassSpringSystem;
using Everglow.Commons.TileHelper;
using Terraria.Audio;
using Terraria.GameContent.Drawing;

namespace Everglow.Yggdrasil.KelpCurtain.Tiles.ForestRainVines;

public class ForestRainVineTwoTile : HangingTile
{
	private Dictionary<Point, Player> playerGraspRope = new Dictionary<Point, Player>();

	// 摆荡幅度上限（角度，建议设为 60°~80°，避免摆幅过大）
	private float maxSwingAngle = MathHelper.ToRadians(80f);

	// 角度缓冲（避免摆角刚好卡在上限时的抖动，建议 5°）
	private float angleBuffer = MathHelper.ToRadians(5f);

	public override void PostSetDefaults()
	{
		RopeUnitMass = 1f;
		SingleLampMass = 50f;
		MaxWireStyle = 1;
		Elasticity = 70f;
	}

	public override void DrawCable(Rope rope, Point pos, SpriteBatch spriteBatch, TileDrawing tileDrawing, Color color = default)
	{
		bool isPlayerGrasping = playerGraspRope.ContainsKey(pos);

		// 回声涂料
		if (!TileDrawing.IsVisible(Main.tile[pos]))
		{
			return;
		}

		var tile = Main.tile[pos];
		ushort type = tile.TileType;
		int paint = Main.tile[pos].TileColor;
		Texture2D tex = PaintedTextureSystem.TryGetPaintedTexture(ModAsset.ForestRainVineTwoTile_Path, type, 1, paint, tileDrawing);
		tex ??= ModAsset.ForestRainVineTwoTile.Value;
		var tileSpriteEffect = SpriteEffects.None;

		// 获取藤蔓末端位置（用于交互检测）
		Vector2 vineEndPosition = Vector2.Zero;
		Player interactingPlayer = null;
		const float detectionRange = 48f; // 检测范围（像素）

		// 获取发绳端物块信息
		var masses = rope.Masses;

		for (int i = 0; i < masses.Length; i++)
		{
			Mass thisMass = masses[i];
			if (i < MaxCableLength - tile.TileFrameY)
			{
				thisMass.IsStatic = true;
				thisMass.Position = pos.ToWorldCoordinates();
				continue;
			}
			else
			{
				thisMass.IsStatic = false;
			}

			Main.NewText(masses[1].Position - masses[0].Position);

			// 记录藤蔓末端位置（最后一个节点）
			if (i == masses.Length - 1)
			{
				vineEndPosition = thisMass.Position;

				// 当前藤曼无玩家抓取
				if (!isPlayerGrasping)
				{
					// 检测范围内的玩家
					foreach (Player player in Main.player)
					{
						if (player != null && player.active &&
							Vector2.Distance(player.Center, vineEndPosition) <= detectionRange)
						{
							interactingPlayer = player;
							break;
						}
					}
				}
			}

			int totalPushTime = 80;
			float pushForcePerFrame = 1.26f;
			float windCycle = 0;

			// 如果无玩家抓取，藤曼受到风力效果
			if(!isPlayerGrasping)
			{
				if (tileDrawing.InAPlaceWithWind((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), 1, 1))
				{
					windCycle = tileDrawing.GetWindCycle((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f), tileDrawing._sunflowerWindCounter);
				}

				float highestWindGridPushComplex = tileDrawing.GetHighestWindGridPushComplex(
					(int)((thisMass.Position.X - 8) / 16f),
					(int)((thisMass.Position.Y - 8) / 16f),
					1, 1, totalPushTime, pushForcePerFrame, 3, swapLoopDir: true);

				windCycle += highestWindGridPushComplex;
			}

			if (!Main.gamePaused)
			{
				rope.ApplyForceSpecial(i, new Vector2(windCycle / 64.0f, 0.4f * thisMass.Value));
			}

			// 支持发光涂料
			Color tileLight;
			if (color != default)
			{
				tileLight = color;
			}
			else
			{
				tileLight = Lighting.GetColor((int)((thisMass.Position.X - 8) / 16f), (int)((thisMass.Position.Y - 8) / 16f));
			}

			Vector2 toNextMass;
			if (i < masses.Length - 1)
			{
				Mass nextMass = masses[i + 1];
				toNextMass = nextMass.Position - thisMass.Position;
			}
			else
			{
				Mass passedMass = masses[i - 1];
				toNextMass = thisMass.Position - passedMass.Position;
			}
			Vector2 drawPos = thisMass.Position - Main.screenPosition;

			spriteBatch.Draw(tex, drawPos, new Rectangle(8 * (i % 4), 0, 8, 10), 
							 tileLight, toNextMass.ToRotation() - MathHelper.PiOver2,
							 new Vector2(4f, 0), 1f, tileSpriteEffect, 0);
		}

		// 下面,ai让我把切向力改为切向速度，但好像用不了。
		rope.ApplyForceSpecial(masses.Length - 1, new Vector2(0, Rope.Gravity * SingleLampMass));

		if (isPlayerGrasping)
		{
			Player player = playerGraspRope[pos];
			player.Center = vineEndPosition;

			if (masses.Length >= 2)
			{
				Mass endMass = masses[masses.Length - 1];
				Mass previousMass = masses[masses.Length - 2];
				Vector2 direction = endMass.Position - previousMass.Position;
				float rotation = direction.ToRotation() - MathHelper.PiOver2;

				player.fullRotation = rotation;
				player.velocity = Vector2.Zero;
				player.gravity = 0f;
			}

			Vector2 vineRootWorldPos = pos.ToWorldCoordinates();
			Vector2 swingVector = player.Center - vineRootWorldPos;

			// 使用角速度而不是直接施加力
			float angularForce = 0.5f;
			float maxSwingSpeed = 3f;

			if (player.controlLeft &&
				Math.Abs(masses[masses.Length - 1].Velocity.X) < maxSwingSpeed &&
				player.Center.X >= pos.X * 16f) // 使用物块坐标比较
			{
				// 施加切向速度而不是力
				Vector2 tangent = new Vector2(-swingVector.Y, swingVector.X).SafeNormalize(Vector2.Zero);
				if (tangent != Vector2.Zero)
				{
					masses[masses.Length - 1].Velocity += tangent * angularForce;

					// 限制最大速度
					if (masses[masses.Length - 1].Velocity.Length() > maxSwingSpeed)
					{
						masses[masses.Length - 1].Velocity = masses[masses.Length - 1].Velocity.SafeNormalize(Vector2.Zero) * maxSwingSpeed;
					}
				}
			}
			else if (player.controlRight &&
					 Math.Abs(masses[masses.Length - 1].Velocity.X) < maxSwingSpeed &&
					 player.Center.X <= pos.X * 16f)
			{
				Vector2 tangent = new Vector2(swingVector.Y, -swingVector.X).SafeNormalize(Vector2.Zero);
				if (tangent != Vector2.Zero)
				{
					masses[masses.Length - 1].Velocity += tangent * angularForce;
					if (masses[masses.Length - 1].Velocity.Length() > maxSwingSpeed)
					{
						masses[masses.Length - 1].Velocity = masses[masses.Length - 1].Velocity.SafeNormalize(Vector2.Zero) * maxSwingSpeed;
					}
				}
			}

			// 轻微的长度约束，防止过度拉伸
			for (int i = 0; i < rope.ElasticConstrains.Length; i++)
			{
				var spring = rope.ElasticConstrains[i];
				Vector2 delta = spring.B.Position - spring.A.Position;
				float currentLength = delta.Length();
				if (currentLength > spring.RestLength * 1.1f) // 允许10%的拉伸
				{
					Vector2 correction = delta * (1f - spring.RestLength / currentLength) * 0.1f;
					if (!spring.A.IsStatic)
					{
						spring.A.Position += correction;
					}

					if (!spring.B.IsStatic)
					{
						spring.B.Position -= correction;
					}
				}
			}

			if (player.controlJump)
			{
				// 恢复玩家重力
				player.gravity = Player.defaultGravity;
				playerGraspRope.Remove(pos);
				player.velocity = masses[masses.Length - 1].Velocity;
				player.fullRotation = 0f;
			}
		}

		// 处理玩家抓取逻辑
		if (interactingPlayer != null && vineEndPosition != Vector2.Zero)
		{
			// 当玩家按下W键时
			if (interactingPlayer.controlUp && !playerGraspRope.ContainsKey(pos))
			{
				playerGraspRope[pos] = interactingPlayer;
			}
		}
	}

	public Vector2 GetSpeedVector(Point orgin, Vector2 endPoint)
	{
		// 半径向量
		Vector2 radius = new Vector2(endPoint.X - orgin.X, endPoint.Y - orgin.Y);

		// 一个半径向量的垂直方向（逆时针 90°）
		Vector2 tangent1 = new Vector2(-radius.Y, radius.X);

		// 另一个是顺时针 90°
		Vector2 tangent2 = new Vector2(radius.Y, -radius.X);

		// 归一化，得到纯方向
		tangent1.Normalize();
		tangent2.Normalize();

		// 这里返回其中一个即可，但你需要根据实际“摆动方向”来选择
		// 比如可以结合上一次位置，判断到底是正转还是反转
		return tangent1;
	}
}