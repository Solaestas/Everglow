using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.UI;
using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

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
		prevBtn.Info.Left.SetValue(0, 1.1f);
		prevBtn.Info.Top.SetValue(0, 0.1f);

		prevBtn.PanelColor = MissionContainer.Instance.GetThemeColor(style: MissionContainer.ColorStyle.Light);
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
			prevBtn.PanelColor = MissionContainer.Instance.GetThemeColor(style: MissionContainer.ColorStyle.Light);
		};
		Register(prevBtn);

		var prevIcon = new UIImage(ModAsset.ArrowUp.Value, Color.White);
		prevIcon.Info.Width.SetValue(0, 0.8f);
		prevIcon.Info.Height.SetValue(0, 0.8f);
		prevBtn.Register(prevIcon);
		prevIcon.Info.SetToCenter();

		nextBtn = new UIBlock();

		nextBtn.Info.CanBeInteract = true;
		nextBtn.Info.IsHidden = true;
		nextBtn.Info.IsVisible = false;

		nextBtn.Info.IsSensitive = true;
		nextBtn.Info.Width.SetValue(0, 0.3f);
		nextBtn.Info.Height.SetValue(0, 0.3f);
		nextBtn.Info.Left.SetValue(0, 1.1f);
		nextBtn.Info.Top.SetValue(0, 0.6f);

		nextBtn.PanelColor = MissionContainer.Instance.GetThemeColor(style: MissionContainer.ColorStyle.Light);
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
			nextBtn.PanelColor = MissionContainer.Instance.GetThemeColor(style: MissionContainer.ColorStyle.Light);
		};
		Register(nextBtn);

		var nextIcon = new UIImage(ModAsset.ArrowDown.Value, Color.White);
		nextBtn.Register(nextIcon);
		nextIcon.Info.Width.SetValue(0, 0.8f);
		nextIcon.Info.Height.SetValue(0, 0.8f);
		nextIcon.Info.SetToCenter();
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