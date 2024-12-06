using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.UI.UIContainers.Sidebar
{
	internal delegate void HandleTrigger(SidebarUIElement quickElement);

	internal abstract class SidebarUIElement : UIImage
	{
		public bool IsMoveing => waitTime >= WaitTime;
		private int waitTime = 0;
		public int WaitTime = 30;
		private Vector2 waitToMouse = Vector2.Zero;
		private bool mouseDown = false;
		public Vector2 Center = Vector2.Zero;
		public string Tooltip = string.Empty;

		public event HandleTrigger OnTrigger;

		public SidebarUIElement(Texture2D texture, Color color) : base(texture, color)
		{
			Style = CalculationStyle.LockAspectRatioMainWidth;
			Info.Width.SetValue(0f, 1f);
		}

		public override void LoadEvents()
		{
			base.LoadEvents();
			Events.OnLeftDown += Events_OnLeftDown;
			Events.OnLeftUp += Events_OnLeftUp;
		}

		private void Events_OnLeftUp(BaseElement baseElement)
		{
			mouseDown = false;
			if (!IsMoveing)
			{
				OnTrigger?.Invoke(this);
			}
		}

		private void Events_OnLeftDown(BaseElement baseElement)
		{
			mouseDown = true;
		}

		public override void Update(GameTime gt)
		{
			base.Update(gt);
			if (mouseDown)
				waitTime++;
			else
				waitTime = 0;

			if (IsMoveing)
			{
				var c = Main.MouseScreen;
				if (c.X + Info.TotalSize.X / 2f > ParentElement.Info.Location.X + ParentElement.Info.Size.X)
				{
					c.X = ParentElement.Info.Location.X + ParentElement.Info.Size.X - Info.TotalSize.X / 2f;
				}
				if (c.X - Info.TotalSize.X / 2f < ParentElement.Info.Location.X)
				{
					c.X = ParentElement.Info.Location.X + Info.TotalSize.X / 2f;
				}
				if (c.Y + Info.TotalSize.Y / 2f > ParentElement.Info.Location.Y + ParentElement.Info.Size.Y)
				{
					c.Y = ParentElement.Info.Location.Y + ParentElement.Info.Size.Y - Info.TotalSize.Y / 2f;
				}
				if (c.Y - Info.TotalSize.Y / 2f < ParentElement.Info.Location.Y)
				{
					c.Y = ParentElement.Info.Location.Y + Info.TotalSize.Y / 2f;
				}
				MoveTo(c);
			}
			else
				MoveTo(Center);
		}

		public bool MoveTo(Vector2 center)
		{
			if (waitToMouse != center)
			{
				waitToMouse += (center - waitToMouse) / 4f;
				Info.Left.SetValue(waitToMouse.X - (ParentElement == null ? 0 : ParentElement.Info.Location.X) - Info.Size.X / 2f, 0f);
				Info.Top.SetValue(waitToMouse.Y - (ParentElement == null ? 0 : ParentElement.Info.Location.Y) - Info.Size.Y / 2f, 0f);
				Calculation();
				return false;
			}
			else
				return true;
		}
	}
}