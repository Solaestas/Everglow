using Everglow.Commons.Mechanics.Mission.Presentation.Icons;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.Mission.UI.UIElements.MissionDetail;

public class UIRewardsStripe : UIBlock
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
				if(iconUIElements.Count * Padding > Info.HitBox.Width)
				{
					_scrollBar = new UIMissionRewardHorizontalScrollbar();
					Register(_scrollBar);
					_scrollBar.WheelValue = 0f;
				}
			}
		}
	}

	public void SetIconGroup(MissionIconGroup iconGroup)
	{
		this.iconGroup = iconGroup;
		IconGroup = iconGroup;
	}

	public override void OnInitialization()
	{
		base.OnInitialization();
		PanelColor = Color.Transparent;
		BorderColor = Color.Transparent;
		Info.SetMargin(0);
		Info.IsSensitive = true;
		Info.HiddenOverflow = true;
		_scrollBar = new UIMissionRewardHorizontalScrollbar();
		Register(_scrollBar);
	}

	public override void Calculation()
	{
		if (_scrollBar is not null && ChildrenElements.Contains(_scrollBar))
		{
			_scrollBar.Info.Width.SetValue(-28, 1f);
			_scrollBar.Info.Height.SetValue(20);
			_scrollBar.Info.Top.SetValue(-60);
			_scrollBar.Info.Left.SetValue(14f);
		}
		base.Calculation();
	}

	public override void Update(GameTime gt)
	{
		base.Update(gt);

		if (IconGroup == null)
		{
			return;
		}
		if (iconUIElements == null)
		{
			return;
		}

		// Update global offset moving to current icon
		float targetOffset = -Padding * _scrollBar.WheelValue * iconUIElements.Count + (Info.HitBox.Width + Padding-128) * _scrollBar.WheelValue;

		globalMotionOffset = targetOffset;

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

			offsetY += HitBox.Height / 2 - width;

			var icon = iconUIElements[i];
			icon.BorderColor = Color.Gray;
			icon.Info.HiddenOverflow = true;
			icon.Info.Width.SetValue(width * 2);
			icon.Info.Height.SetValue(width * 2);
			icon.Info.Left.SetValue(offsetX);
			icon.Info.Top.SetValue(offsetY);
			icon.Color = Color.White;
			icon.PanelColor = Color.White;
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
		if (iconUIElements == null || iconUIElements.Count == 0)
		{
			return;
		}
		for (int i = 0; i < iconUIElements.Count; i++)
		{
			iconUIElements[i].Draw(sb);
		}
		if (ChildrenElements.Contains(_scrollBar))
		{
			_scrollBar.Draw(sb);
		}
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
