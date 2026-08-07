using Everglow.Commons.Mechanics.Mission.PlayerSide.Primitives;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements.MissionDetail;

public class UIRewardsPanel : UIBlock
{
	private UIRewardsStripe _rewardStripe;
	private MissionIconGroup iconGroup;

	public MissionIconGroup IconGroup => iconGroup;

	public UIRewardsPanel(MissionIconGroup iconGroup)
	{
		this.iconGroup = iconGroup;
	}

	public void SetIconGroup(MissionIconGroup iconGroup)
	{
		this.iconGroup = iconGroup;
		_rewardStripe.IconGroup = iconGroup;

		if (iconGroup != null && IconGroup.IconCount > 1)
		{
		}
		else
		{
		}
	}

	public override void OnInitialization()
	{
		base.OnInitialization();
		PanelColor = Color.Transparent;
		BorderColor = Color.Transparent;
		Info.SetMargin(0);
		Info.IsSensitive = true;

		_rewardStripe = new UIRewardsStripe();
		_rewardStripe.Info.HiddenOverflow = true;
		Register(_rewardStripe);
	}

	public override void Calculation()
	{
		base.Calculation();

		_rewardStripe.Info.Width.SetFull();
		_rewardStripe.Info.Height.SetFull();
	}

	public class UIRewardsStripe : BaseElement
	{
		private const int Padding = 144;

		private float globalMotionOffset = 0;

		private UIMissionRewardHorizontalScrollbar _scrollBar;

		private List<UIRewardsStripeItem> iconUIElements;
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
					iconUIElements = iconGroup.Icons.Select(icon => new UIRewardsStripeItem(icon)).ToList();
					foreach (var iE in iconUIElements)
					{
						Register(iE);
					}
				}
			}
		}

		public override void OnInitialization()
		{
			_scrollBar = new UIMissionRewardHorizontalScrollbar();
			Register(_scrollBar);
			_scrollBar.WheelValue = 0f;
			base.OnInitialization();
		}

		public override void Calculation()
		{
			_scrollBar.Info.Width.SetFull();
			_scrollBar.Info.Height.SetValue(20);
			_scrollBar.Info.Top.SetValue(30, 1f);
			_scrollBar.Info.Left.SetEmpty();
			base.Calculation();
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
			float targetOffset = -Padding * _scrollBar.WheelValue;

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

				var lerpValue = Math.Clamp(Math.Abs(offsetX) / (Padding * scale) - 2, 0, 1);

				var width = 64;
				float drawColorV = (1 - lerpValue) * 0.6f + 0.4f;
				float alpha = (1 - lerpValue) * 0.7f + 0.3f;
				var drawColor = new Color(drawColorV, drawColorV, drawColorV, alpha);

				float bgCV = lerpValue * 0.04f + 0.2f;
				float bgA = (1 - lerpValue) * 0.7f + 0.3f;
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
				icon.Scale = 1;
			}
		}

		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);
		}

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

	public class UIRewardsStripeItem : UIBlock
	{
		private readonly MissionIconBase _icon;

		public Color Color { get; set; } = Color.White;

		public float Scale { get; set; } = 1f;

		public UIRewardsStripeItem(MissionIconBase icon)
		{
			_icon = icon;
			Events.OnMouseHover += Events_OnMouseHover;
		}

		private void Events_OnMouseHover(BaseElement baseElement)
		{
			MissionContainer.Instance.MouseText = _icon.Tooltip;
		}

		protected override void DrawSelf(SpriteBatch sb)
		{
			UIMissionBlock.DrawMissionPanel(sb, Info.TotalHitBox, Color.White * (PanelColor.A / 255f));
			int x = Info.TotalHitBox.X;
			int y = Info.TotalHitBox.Y;
			int w = Info.TotalHitBox.Width;
			int h = Info.TotalHitBox.Height;
			Texture2D tex_side = ModAsset.MissionTopicFrame_bottom_side.Value;
			sb.Draw(tex_side, new Rectangle(x + (w - 51) / 2, y + h - 7, 51, 7), null, Color.White * (PanelColor.A / 255f));
		}

		protected override void DrawChildren(SpriteBatch sb)
		{
			_icon.Draw(sb, HitBox, Color, Scale * 2.5f);
		}
	}
}
