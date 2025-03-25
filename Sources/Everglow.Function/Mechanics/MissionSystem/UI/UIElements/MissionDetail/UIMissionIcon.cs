using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements.MissionDetail;

public class UIMissionIcon : UIBlock
{
	private const int ButtonSize = 50;
	private const int ButtonTop = 218;
	private const int ButtonLeftRight = 10;

	private UIMissionCarousel carousel;
	private UIBlock prevBtn;
	private UIBlock nextBtn;
	private MissionIconGroup iconGroup;

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
		carousel.IconGroup = iconGroup;

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
		BorderColor = Color.Transparent;
		Info.SetMargin(0);
		Info.IsSensitive = true;

		var scale = MissionContainer.Scale;

		carousel = new UIMissionCarousel();
		carousel.Info.Width.SetFull();
		carousel.Info.Height.SetFull();
		carousel.Info.HiddenOverflow = true;
		Register(carousel);

		prevBtn = new UIBlock();
		prevBtn.Info.CanBeInteract = true;
		prevBtn.Info.IsHidden = true;
		prevBtn.Info.IsVisible = false;
		prevBtn.Info.IsSensitive = true;
		prevBtn.Info.Width.SetValue(ButtonSize * scale);
		prevBtn.Info.Height.SetValue(ButtonSize * scale);
		prevBtn.Info.Left.SetValue(ButtonLeftRight * scale);
		prevBtn.Info.Top.SetValue(ButtonTop * scale);
		prevBtn.PanelColor = Color.Transparent;
		prevBtn.BorderWidth = 0;
		prevBtn.Events.OnLeftDown += e =>
		{
			if (!IconGroup.IsFirstIcon)
			{
				IconGroup.Prev();
			}
		};
		Register(prevBtn);
		var prevIcon = new UIImage(ModAsset.MissionIconBack.Value, Color.White);
		prevIcon.Info.Width.SetValue(0, 0.8f);
		prevIcon.Info.Height.SetValue(0, 0.8f);
		prevBtn.Register(prevIcon);
		prevIcon.Info.SetToCenter();
		prevIcon.Events.OnMouseHover += e => prevIcon.Color = IconGroup.IsFirstIcon ? Color.White : new Color(1f, 1f, 1f, 0f);
		prevIcon.Events.OnMouseOut += e => prevIcon.Color = Color.White;

		nextBtn = new UIBlock();
		nextBtn.Info.CanBeInteract = true;
		nextBtn.Info.IsHidden = true;
		nextBtn.Info.IsVisible = false;
		nextBtn.Info.IsSensitive = true;
		nextBtn.Info.Width.SetValue(ButtonSize * scale);
		nextBtn.Info.Height.SetValue(ButtonSize * scale);
		nextBtn.Info.Left.SetValue(Info.Width.Pixel - ButtonLeftRight * scale - ButtonSize * scale);
		nextBtn.Info.Top.SetValue(ButtonTop * scale);
		nextBtn.PanelColor = Color.Transparent;
		nextBtn.BorderWidth = 0;
		nextBtn.Events.OnLeftDown += e =>
		{
			if (!IconGroup.IsLastIcon)
			{
				IconGroup.Next();
			}
		};
		Register(nextBtn);
		var nextIcon = new UIImage(ModAsset.MissionIconNext.Value, Color.White);
		nextBtn.Register(nextIcon);
		nextIcon.Info.Width.SetValue(0, 0.8f);
		nextIcon.Info.Height.SetValue(0, 0.8f);
		nextIcon.Info.SetToCenter();
		nextIcon.Events.OnMouseHover += e => nextIcon.Color = IconGroup.IsLastIcon ? Color.White : new Color(1f, 1f, 1f, 0f);
		nextIcon.Events.OnMouseOut += e => nextIcon.Color = Color.White;
	}

	public class UIMissionCarousel : BaseElement
	{
		private const int Padding = 144;
		private const int MaxWidth = 128;
		private const int MinWidth = 64;

		private float globalMotionOffset = 0;

		private List<UIMissionIconCarouselItem> iconUIElements;
		private MissionIconGroup iconGroup;

		public MissionIconGroup IconGroup
		{
			get => iconGroup;
			set
			{
				iconGroup = value;

				ChildrenElements.Clear();
				if (iconGroup != null)
				{
					iconUIElements = iconGroup.Icons.Select(icon => new UIMissionIconCarouselItem(icon)).ToList();
					foreach (var iE in iconUIElements)
					{
						Register(iE);
					}
				}
			}
		}

		public override void Update(GameTime gt)
		{
			base.Update(gt);

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

			for (int i = 0; i < iconUIElements.Count; i++)
			{
				var scale = MissionContainer.Scale;
				var offsetX = (int)((Padding * i + globalMotionOffset) * MissionContainer.Scale);
				var offsetY = 0;

				var lerpValue = Math.Clamp(Math.Abs(offsetX) / (Padding * scale), 0, 1);
				var width = (int)MathHelper.Lerp((int)(MaxWidth * scale), (int)(MinWidth * scale), lerpValue);

				float drawColorV = (1 - lerpValue) * 0.6f + 0.4f;
				float alpha = (1 - lerpValue) * 0.7f + 0.3f;
				var drawColor = new Color(drawColorV, drawColorV, drawColorV, alpha);

				float bgCV = lerpValue * 0.04f + 0.2f;
				float bgA = (1 - lerpValue) * 0.2f + 0.6f;
				var bgC = new Color(bgCV, bgCV, bgCV, bgA);

				offsetX += HitBox.Width / 2 - width;
				offsetY += HitBox.Height / 2 - width;

				var icon = iconUIElements[i];
				icon.PanelColor = bgC;
				icon.BorderColor = Color.Gray;
				icon.Info.HiddenOverflow = true;
				icon.Info.Width.SetValue(width * 2);
				icon.Info.Height.SetValue(width * 2);
				icon.Info.Left.SetValue(offsetX);
				icon.Info.Top.SetValue(offsetY);
				icon.Color = drawColor;
				icon.Scale = width / (float)MaxWidth;
			}
		}

		public override void Draw(SpriteBatch sb) => base.Draw(sb);

		protected override void DrawChildren(SpriteBatch sb)
		{
			if (IconGroup == null || IconGroup.IconCount == 0)
			{
				return;
			}

			int centerIndex = 0;
			float minValue = 1f;

			for (int i = 0; i < iconGroup.IconCount; i++)
			{
				var offsetX = Padding * i + globalMotionOffset;
				var value = Math.Clamp(Math.Abs(offsetX) / (Padding * 0.9f), 0, 1);
				if (value < minValue)
				{
					minValue = value;
					centerIndex = i;
				}
			}

			for (int i = 0; i < iconUIElements.Count; i++)
			{
				if (i != centerIndex)
				{
					iconUIElements[i].Draw(sb);
				}
			}

			iconUIElements[centerIndex].Draw(sb);
		}
	}

	public class UIMissionIconCarouselItem : UIBlock
	{
		private readonly MissionIconBase _icon;

		public Color Color { get; set; } = Color.White;

		public float Scale { get; set; } = 1f;

		public UIMissionIconCarouselItem(MissionIconBase icon)
		{
			_icon = icon;
			Events.OnMouseHover += Events_OnMouseHover;
		}

		private void Events_OnMouseHover(BaseElement baseElement)
		{
			MissionContainer.Instance.MouseText = _icon.Tooltip;
		}

		protected override void DrawChildren(SpriteBatch sb)
		{
			_icon.Draw(sb, HitBox, Color, Scale * 2.5f);
		}
	}
}