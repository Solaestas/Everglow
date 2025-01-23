using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.GameInput;

namespace Everglow.Yggdrasil.YggdrasilTown.Items.Accessories;

public abstract class TelescopicPlatform : ModItem
{
	/// <summary>
	/// 升降平台最多升高多高
	/// </summary>
	public int MaxHeight;

	/// <summary>
	/// 升降平台下面的支撑有多少节
	/// </summary>
	public int PillarCount;

	/// <summary>
	/// 平台的上升与下降速度
	/// </summary>
	public int MoveSpeed;

	// 分别为平台的区域，前支柱的区域，后支柱的区域，贴图上的支柱应该倾斜45度
	public Rectangle BodyRect;
	public Rectangle PillarFrontRect;
	public Rectangle PillarBackRect;
	public float BodyDrawOffsetY;

	public Texture2D Texture2;

	public override void SetDefaults()
	{
		Item.accessory = true;
	}

	public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
	{
		return
			(incomingItem.ModItem is not TelescopicPlatform && equippedItem.ModItem is TelescopicPlatform) ||
			(incomingItem.ModItem is TelescopicPlatform && equippedItem.ModItem is not TelescopicPlatform);
	}

	public override void UpdateAccessory(Player player, bool hideVisual)
	{
		var modPlayer = player.GetModPlayer<TelescopePlatformPlayer>();
		if (modPlayer != null)
		{
			modPlayer.Platform = this;
		}
	}
}

public class TelescopePlatformPlayer : ModPlayer
{
	public TelescopicPlatform Platform;

	/// <summary>
	/// 当前升高到了多高的位置
	/// </summary>
	public int NowHeight = 0;

	public Vector2 PlatformBasePos;
	public bool InPlatform = false;

	public override void ResetEffects()
	{
		if (InPlatform)
		{
			if (Platform != null)
			{
				NowHeight = Math.Clamp(NowHeight, 0, Platform.MaxHeight);
			}
			Player.gravity = 0;
			Player.noFallDmg = true;
		}
		Platform = null;
	}

	public override void PostUpdate()
	{
		if (Platform == null)
		{
			InPlatform = false;
			return;
		}

		// 这里不加24的话会有奇怪的 bug 发生，在非常靠近地面的时候角色会飞起来一段距离
		if (Player.velocity.Length() != 0 && InPlatform)
		{
			Player.position = new Vector2(0, -NowHeight - 24) + PlatformBasePos;
			NowHeight = 0;
			InPlatform = false;
			return;
		}
		if (NowHeight != 0)
		{
			Player.position = new Vector2(0, -NowHeight - 24) + PlatformBasePos;
			InPlatform = true;
		}
		else
		{
			if (InPlatform)
			{
				Player.position = new Vector2(0, -NowHeight - 24) + PlatformBasePos;
				NowHeight = 0;
				InPlatform = false;
			}
		}
	}

	public override void ProcessTriggers(TriggersSet triggersSet)
	{
		if (Player.velocity.Length() != 0)
		{
			InPlatform = false;
			return;
		}
		if (Platform == null || Platform.MaxHeight == 0)
		{
			NowHeight = 0;
			InPlatform = false;
			return;
		}
		int direction = 0;

		// 同时按下的时候要停住
		if (triggersSet.Up)
		{
			direction++;
		}
		if (triggersSet.Down)
		{
			direction--;
		}

		if (direction == 0)
		{
			return;
		}

		if (NowHeight == 0)
		{
			PlatformBasePos = Player.position;
		}

		int move = Platform.MoveSpeed * direction;
		NowHeight = Math.Clamp(NowHeight + move, 0, Platform.MaxHeight);
	}

	public override void DrawEffects(PlayerDrawSet drawInfo, ref float r, ref float g, ref float b, ref float a, ref bool fullBright)
	{
		if (Platform == null || NowHeight == 0)
		{
			return;
		}
		float perHeight = (float)(NowHeight + 24) / Platform.PillarCount;
		float pillarLength = MathF.Sqrt(MathF.Pow(Platform.PillarFrontRect.Width, 2) + MathF.Pow(Platform.PillarFrontRect.Height, 2));
		float pillarAngle = MathF.Asin(perHeight / pillarLength * 1.2f);
		float pillarWidth = pillarLength * MathF.Cos(pillarAngle);
		Vector2 bodyPos = Player.Bottom - new Vector2(Platform.BodyRect.Width / 2, Platform.BodyRect.Height - Platform.BodyDrawOffsetY);

		// 下面的支柱
		for (int i = 0; i < Platform.PillarCount; i++)
		{
			float height = perHeight * i;
			Vector2 pos = Player.Bottom + new Vector2(0, height + perHeight / 2) - Main.screenPosition;
			Point point = new Point((int)Player.Bottom.X / 16, ((int)Player.Bottom.Y + (int)height) / 16);
			float angle = pillarAngle - MathF.PI / 4;
			Main.spriteBatch.Draw(Platform.Texture2, pos, Platform.PillarBackRect, Lighting.GetColor(point), angle, Platform.PillarBackRect.Size() / 2, 1f, SpriteEffects.None, 0f);
			Main.spriteBatch.Draw(Platform.Texture2, pos, Platform.PillarFrontRect, Lighting.GetColor(point), -angle, Platform.PillarFrontRect.Size() / 2, 1f, SpriteEffects.None, 0f);
		}
		Main.spriteBatch.Draw(Platform.Texture2, bodyPos - Main.screenPosition, Platform.BodyRect, Lighting.GetColor(new Point((int)Player.Center.X / 16, (int)Player.Center.Y / 16)));
	}
}