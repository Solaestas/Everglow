using Everglow.Commons.MissionSystem;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.UI.UIContainers.Mission.UIElements;

public class UIMissionIcon : UIImage
{
	private UIBlock prevBtn;
	private UIBlock nextBtn;
	private MissionIconGroup iconGroup;

	public MissionIconGroup IconGroup
	{
		get => iconGroup;
	}

	public UIMissionIcon(MissionIconGroup iconGroup, Color color)
		: base(null, color)
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

		prevBtn = new UIBlock();
		prevBtn.Info.CanBeInteract = true;
		prevBtn.Info.IsHidden = true;
		prevBtn.Info.IsVisible = false;

		prevBtn.Info.IsSensitive = true;
		prevBtn.Info.Width.SetValue(0, 0.3f);
		prevBtn.Info.Height.SetValue(0, 0.3f);
		prevBtn.Info.RightMargin.SetValue(0, 1.1f);
		prevBtn.Info.Top.SetValue(0, 0.1f);
		prevBtn.Events.OnLeftClick += e =>
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
			prevBtn.PanelColor = Color.Black;
		};
		Register(prevBtn);

		nextBtn = new UIBlock();
		nextBtn.Info.CanBeInteract = true;
		nextBtn.Info.IsHidden = true;
		nextBtn.Info.IsVisible = false;

		nextBtn.Info.Width.SetValue(0, 0.3f);
		nextBtn.Info.Height.SetValue(0, 0.3f);
		nextBtn.Info.Left.SetValue(0, 1.1f);
		nextBtn.Info.Top.SetValue(0, 0.6f);
		nextBtn.Events.OnLeftClick += e =>
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
			nextBtn.PanelColor = Color.Black;
		};
		Register(nextBtn);
	}

	protected override void DrawSelf(SpriteBatch sb)
	{
		base.DrawSelf(sb);

		if (IconGroup != null)
		{
			IconGroup.Draw(sb, Info.TotalHitBox);
		}
	}
}