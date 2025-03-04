using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.UI.UIElements;
using Terraria.GameContent;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

public class UIMissionIcon : UIBlock
{
	private const int Padding = 176;
	private const int MaxWidth = 108;
	private const int MinWidth = 78;

	private UIBlock prevBtn;
	private UIBlock nextBtn;
	private MissionIconGroup iconGroup;

	private float globalMotionOffset = 0;

	public MissionIconGroup IconGroup
	{
		get => iconGroup;
	}

	public UIMissionIcon(MissionIconGroup iconGroup)
	{
		this.iconGroup = iconGroup;
	}

	public void SetIconGroup(MissionIconGroup iconGroup)
	{
		this.iconGroup = iconGroup;

		if (iconGroup != null && IconGroup.IconCount > 1)
		{
			prevBtn.Info.CanBeInteract = true;
			prevBtn.Info.IsHidden = false;
			prevBtn.Info.IsVisible = true;

			nextBtn.Info.CanBeInteract = true;
			nextBtn.Info.IsHidden = false;
			nextBtn.Info.IsVisible = true;
		}
		else
		{
			prevBtn.Info.CanBeInteract = false;
			prevBtn.Info.IsHidden = true;
			prevBtn.Info.IsVisible = false;

			nextBtn.Info.CanBeInteract = false;
			nextBtn.Info.IsHidden = true;
			nextBtn.Info.IsVisible = false;
		}
	}

	public override void OnInitialization()
	{
		base.OnInitialization();
		PanelColor = Color.Transparent;
		BorderColor = Color.Gray;
		Info.HiddenOverflow = true;
		Info.SetMargin(0);

		var scale = MissionContainer.Scale;

		prevBtn = new UIBlock();

		prevBtn.Info.CanBeInteract = true;
		prevBtn.Info.IsHidden = true;
		prevBtn.Info.IsVisible = false;

		prevBtn.Info.IsSensitive = true;
		prevBtn.Info.Width.SetValue(30);
		prevBtn.Info.Height.SetValue(30);
		prevBtn.Info.Left.SetValue(Info.Width.Pixel / 2 - 220 * scale);
		prevBtn.Info.Top.SetValue(180 * scale);

		prevBtn.PanelColor = Color.Transparent;
		prevBtn.BorderWidth = 0;
		prevBtn.Events.OnLeftDown += e =>
		{
			if (!IconGroup.IsFirstIcon)
			{
				IconGroup.Prev();
			}
		};
		prevBtn.Events.OnMouseHover += e =>
		{
			prevBtn.PanelColor = Color.Gray;
		};
		prevBtn.Events.OnMouseOver += e =>
		{
			prevBtn.PanelColor = Color.Gray;
		};
		prevBtn.Events.OnMouseOut += e =>
		{
			prevBtn.PanelColor = Color.Transparent;
		};
		Register(prevBtn);

		var prevIcon = new UIImage(ModAsset.MissionIconBack.Value, Color.White);
		prevIcon.Info.Width.SetValue(0, 0.8f);
		prevIcon.Info.Height.SetValue(0, 0.8f);
		prevBtn.Register(prevIcon);
		prevIcon.Info.SetToCenter();

		nextBtn = new UIBlock();

		nextBtn.Info.CanBeInteract = true;
		nextBtn.Info.IsHidden = true;
		nextBtn.Info.IsVisible = false;

		nextBtn.Info.IsSensitive = true;
		nextBtn.Info.Width.SetValue(30);
		nextBtn.Info.Height.SetValue(30);
		nextBtn.Info.Left.SetValue(Info.Width.Pixel / 2 + 160 * scale);
		nextBtn.Info.Top.SetValue(180 * scale);

		nextBtn.PanelColor = Color.Transparent;
		nextBtn.BorderWidth = 0;
		nextBtn.Events.OnLeftDown += e =>
		{
			if (!IconGroup.IsLastIcon)
			{
				IconGroup.Next();
			}
		};
		nextBtn.Events.OnMouseHover += e =>
		{
			nextBtn.PanelColor = Color.Gray;
		};
		nextBtn.Events.OnMouseOver += e =>
		{
			nextBtn.PanelColor = Color.Gray;
		};
		nextBtn.Events.OnMouseOut += e =>
		{
			nextBtn.PanelColor = Color.Transparent;
		};
		Register(nextBtn);

		var nextIcon = new UIImage(ModAsset.MissionIconNext.Value, Color.White);
		nextBtn.Register(nextIcon);
		nextIcon.Info.Width.SetValue(0, 0.8f);
		nextIcon.Info.Height.SetValue(0, 0.8f);
		nextIcon.Info.SetToCenter();
	}

	public override void Update(GameTime gt)
	{
		if (IconGroup == null)
		{
			return;
		}

		// Update global offset moving to current icon
		float deltaTime = (float)gt.ElapsedGameTime.TotalSeconds;
		float targetOffset = -Padding * IconGroup.CurrentIndex;

		float lerpFactor = 1f - (float)Math.Pow(0.01f, deltaTime);
		globalMotionOffset = MathHelper.Lerp(globalMotionOffset, targetOffset, lerpFactor);

		if (Math.Abs(globalMotionOffset - targetOffset) < 0.5f)
		{
			globalMotionOffset = targetOffset;
		}
	}

	protected override void DrawChildren(SpriteBatch sb)
	{
		if (iconGroup == null || iconGroup.IconCount <= 0)
		{
			return;
		}

		var scale = MissionContainer.Scale;
		var globalOffset = (int)globalMotionOffset;

		Rectangle centerDest = default;
		int centerIndex = 0;
		float minValue = 1f;
		Color centerColor = default;
		Color centerBgColor = default;

		for (int i = 0; i < iconGroup.IconCount; i++)
		{
			var offsetX = Padding * i + globalOffset;

			var lerpValue = Math.Clamp(Math.Abs(offsetX) / (Padding * 0.9f), 0, 1);
			if (lerpValue < minValue)
			{
				minValue = lerpValue;
				centerIndex = i;
			}
		}

		for (int i = 0; i < iconGroup.IconCount; i++)
		{
			var offsetX = (int)((Padding * i + globalOffset) * scale);

			var lerpValue = Math.Clamp(Math.Abs(offsetX) / (Padding * 0.9f), 0, 1);
			var width = (int)MathHelper.Lerp((int)(MaxWidth * scale), (int)(MinWidth * scale), lerpValue);

			var drawDest = new Rectangle(
				Info.TotalHitBox.Center.X - width + offsetX,
				Info.TotalHitBox.Center.Y - width,
				width * 2,
				width * 2);

			var drawColorV = (1 - lerpValue) * 0.6f + 0.4f;
			var alpha = (1 - lerpValue) * 0.4f + 0.6f;
			var drawColor = new Color(drawColorV, drawColorV, drawColorV, alpha);

			var bgColorV = (1 - lerpValue) * 0.6f + 0.3f;
			var bgAlpha = (1 - lerpValue) * 0.6f + 0.4f;
			var bgColor = new Color(bgColorV, bgColorV, bgColorV, bgAlpha);

			if (i == centerIndex)
			{
				centerDest = drawDest;
				centerColor = drawColor;
				centerBgColor = bgColor;
			}
			else
			{
				Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, drawDest, bgColor);
				IconGroup.Icons[i].Draw(sb, drawDest, drawColor);
			}
		}

		Main.spriteBatch.Draw(TextureAssets.MagicPixel.Value, centerDest, centerBgColor);
		IconGroup.Icons[centerIndex].Draw(sb, centerDest, centerColor);

		base.DrawChildren(sb);
	}
}