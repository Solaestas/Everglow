using Everglow.Myth.LanternMoon.Items.Weapons;

namespace Everglow.Myth.LanternMoon.Items;

[Pipeline(typeof(WCSPipeline_PointWrap))]
public class KeroseneLanternFlameThrower_UI_Bar : Visual
{
	public override CodeLayer DrawLayer => CodeLayer.PostDrawDusts;

	public Player Owner = null;
	public Item TargetItem = null;
	public bool HoldingButtom = false;
	public bool HoverButtom = false;
	public float Fade = 0;
	public float ButtomValue = 0f;
	public float AmmoValue = 1f;

	public override void Update()
	{
		// Data validation
		if (Owner?.HeldItem is null)
		{
			throw new InvalidOperationException("Owner must be initialized correctly before updating.");
		}

		// Ensure player holding specfic item
		if (Owner.HeldItem.type != ModContent.ItemType<KeroseneLanternFlameThrower>()
			|| Owner.HeldItem.ModItem is not KeroseneLanternFlameThrower kero
			|| kero.Visual != this)
		{
			Fade -= 0.1f;
			if (Fade <= 0)
			{
				Active = false;
				return;
			}
		}
		else
		{
			KeroseneLanternFlameThrower kThrower = Owner.HeldItem.ModItem as KeroseneLanternFlameThrower;
			if (kThrower is not null)
			{
				ButtomValue = kThrower.PowerRate;
				AmmoValue = kThrower.AmmoAmount;
				HoverButtom = false;
				Vector2 buttomWorldPos = Owner.MountedCenter + new Vector2(0, -40) + new Vector2(78 * ButtomValue - 39, 3);
				if(Main.MouseWorld.X > buttomWorldPos.X - 5 && Main.MouseWorld.X < buttomWorldPos.X + 5)
				{
					if (Main.MouseWorld.Y > buttomWorldPos.Y - 8 && Main.MouseWorld.Y < buttomWorldPos.Y + 8)
					{
						if(Main.mouseLeft && Main.mouseLeftRelease)
						{
							HoldingButtom = true;
						}
						HoverButtom = true;
					}
				}
				if (HoldingButtom)
				{
					HoverButtom = true;
					kThrower.PowerRate = Math.Clamp((Main.MouseWorld.X - Owner.MountedCenter.X + 39) / 78f, 0, 1f);
					ButtomValue = kThrower.PowerRate;
					if(!Main.mouseLeft)
					{
						HoldingButtom = false;
					}
				}
			}
			if (Fade < 1f)
			{
				Fade += 0.1f;
			}
			else
			{
				Fade = 1f;
			}
		}

		TargetItem = Owner.HeldItem;

		if (Main.mapFullscreen)
		{
			return;
		}
	}

	public override void Draw()
	{
		Texture2D tex = ModAsset.KeroseneLanternFlameThrower_UI.Value;
		Vector2 drawPos = Owner.MountedCenter + new Vector2(0, -40);
		Rectangle ui_framework = new Rectangle(0, 0, 114, 26);
		Rectangle powerBar = new Rectangle(18, 28, 78, 32);
		Rectangle powerBar_realtime = powerBar;
		powerBar_realtime.Width = (int)(ButtomValue * 78);

		Rectangle ammoBar = new Rectangle(18, 62, 78, 2);
		Rectangle ammoBar_realtime = ammoBar;
		ammoBar_realtime.Width = (int)(AmmoValue / 100f * 78);

		Rectangle buttom = new Rectangle(10, 38, 6, 12);
		Rectangle buttom_highlight = new Rectangle(104, 36, 10, 16);

		Color envColor = Lighting.GetColor(drawPos.ToTileCoordinates());
		Color flameColor = Color.Lerp(new Color(0.6f, 0f, 0f, 0), new Color(1f, 1f, 1f, 0), ButtomValue);
		Ins.Batch.Draw(tex, drawPos, ui_framework, envColor, 0, ui_framework.Size() * 0.5f, Fade, SpriteEffects.None);
		Ins.Batch.Draw(tex, drawPos + new Vector2(0, 3), powerBar_realtime, flameColor, 0, powerBar.Size() * 0.5f, Fade, SpriteEffects.None);
		Ins.Batch.Draw(tex, drawPos + new Vector2(0, 10), ammoBar_realtime, envColor, 0, ammoBar.Size() * 0.5f, Fade, SpriteEffects.None);
		if(AmmoValue < 100)
		{
			Rectangle ammoBar_end = new Rectangle(98, 62, 2, 2);
			Ins.Batch.Draw(tex, drawPos + new Vector2(78 * AmmoValue / 100f - 39, 10), ammoBar_end, envColor, 0, ammoBar_end.Size() * 0.5f, Fade, SpriteEffects.None);
		}
		Ins.Batch.Draw(tex, drawPos + new Vector2(78 * ButtomValue - 39, 3), buttom, envColor, 0, buttom.Size() * 0.5f, Fade, SpriteEffects.None);
		if(HoverButtom)
		{
			Color hoverColor = new Color(0.6f, 0.2f, 0f, 0);
			if(Main.mouseLeft)
			{
				hoverColor = new Color(1f, 1f, 1f, 0);
			}
			Ins.Batch.Draw(tex, drawPos + new Vector2(78 * ButtomValue - 39, 3), buttom_highlight,hoverColor, 0, buttom_highlight.Size() * 0.5f, Fade, SpriteEffects.None);
		}
	}
}