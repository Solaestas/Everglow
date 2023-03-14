namespace Everglow.Commons.UI.UIElements;

public class UIContainerPanel : BaseElement
{
	private class InnerPanel : BaseElement
	{
		public override Rectangle HiddenOverflowRectangle => ParentElement.HiddenOverflowRectangle;

		public override Rectangle GetCanHitBox() => Rectangle.Intersect(ParentElement.GetCanHitBox(), ParentElement.Info.TotalHitBox);

		public InnerPanel()
		{
			Info.Width.Percent = 1f;
			Info.Height.Percent = 1f;
		}

		public override bool ContainsPoint(Point point)
		{
			return ParentElement.ContainsPoint(point);
		}

		protected override void DrawChildren(SpriteBatch sb)
		{
			ChildrenElements.ForEach(element =>
			{
				if (element.IsVisible && !element.Info.IsHidden && element.Info.TotalHitBox.Intersects(ParentElement.Info.TotalHitBox))
					element.Draw(sb);
			});
		}
	}

	private InnerPanel _innerPanel;
	private VerticalScrollbar _verticalScrollbar;
	private HorizontalScrollbar _horizontalScrollbar;
	private float verticalWhellValue;
	private float horizontalWhellValue;
	private Vector2 innerPanelMinLocation;
	private Vector2 innerPanelMaxLocation;

	public UIContainerPanel()
	{
		Info.HiddenOverflow = true;
		Info.Width.Percent = 1f;
		Info.Height.Percent = 1f;
		Info.SetMargin(4f);
	}

	public void SetVerticalScrollbar(VerticalScrollbar scrollbar) => _verticalScrollbar = scrollbar;

	public void SetHorizontalScrollbar(HorizontalScrollbar scrollbar) => _horizontalScrollbar = scrollbar;

	public void SetVerticalWhell(float whell)
	{
		if (_verticalScrollbar != null)
			_verticalScrollbar.WheelValue = whell;
	}

	public void SetHorizontalWhell(float whell)
	{
		if (_horizontalScrollbar != null)
			_horizontalScrollbar.WheelValue = whell;
	}

	public override void OnInitialization()
	{
		base.OnInitialization();
		_innerPanel = new InnerPanel();
		Register(_innerPanel);
	}

	public int GetElementsCount() => _innerPanel.ChildrenElements.Count;

	public override void Update(GameTime gt)
	{
		base.Update(gt);
		if (_verticalScrollbar != null && verticalWhellValue != _verticalScrollbar.WheelValue)
		{
			verticalWhellValue = _verticalScrollbar.WheelValue;
			Calculation();
		}

		if (_horizontalScrollbar != null && horizontalWhellValue != _horizontalScrollbar.WheelValue)
		{
			horizontalWhellValue = _horizontalScrollbar.WheelValue;
			Calculation();
		}
	}

	public bool AddElement(BaseElement element)
	{
		bool flag = _innerPanel.Register(element);
		if (flag)
			Calculation();

		return flag;
	}

	public void AddElements(List<BaseElement> elements)
	{
		foreach (var element in elements)
		{
			if (element == null || ChildrenElements.Contains(element) || element.ParentElement != null)
				continue;

			element.SetParentElement(_innerPanel);
			if (!element.Info.InitDone)
				element.OnInitialization();

			_innerPanel.ChildrenElements.Add(element);
		}
		Calculation();
	}

	public bool RemoveElement(BaseElement element)
	{
		bool flag = _innerPanel.Remove(element);
		if (flag)
			Calculation();

		return flag;
	}

	public void ClearAllElements()
	{
		var list = new List<BaseElement>(_innerPanel.ChildrenElements);
		foreach (var child in list)
		{
			_innerPanel.Remove(child);
		}

		Calculation();
	}

	private void CalculationInnerPanelSize()
	{
		innerPanelMinLocation = Vector2.Zero;
		innerPanelMaxLocation = Vector2.Zero;
		Vector2 v = Vector2.Zero;
		foreach (var element in _innerPanel.ChildrenElements)
		{
			v.X = element.Info.TotalLocation.X - _innerPanel.Info.Location.X;
			v.Y = element.Info.TotalLocation.Y - _innerPanel.Info.Location.Y;
			if (innerPanelMinLocation.X > v.X)
				innerPanelMinLocation.X = v.X;

			if (innerPanelMinLocation.Y > v.Y)
				innerPanelMinLocation.Y = v.Y;

			v.X = element.Info.TotalLocation.X + element.Info.TotalSize.X - _innerPanel.Info.Location.X;
			v.Y = element.Info.TotalLocation.Y + element.Info.TotalSize.Y - _innerPanel.Info.Location.Y;

			if (innerPanelMaxLocation.X < v.X)
				innerPanelMaxLocation.X = v.X;

			if (innerPanelMaxLocation.Y < v.Y)
				innerPanelMaxLocation.Y = v.Y;
		}
	}

	public override void Calculation()
	{
		base.Calculation();
		CalculationInnerPanelSize();

		float maxY = innerPanelMaxLocation.Y - _innerPanel.Info.TotalSize.Y;
		if (maxY < innerPanelMinLocation.Y)
			maxY = innerPanelMinLocation.Y;

		_innerPanel.Info.Top.Pixel = -MathHelper.Lerp(innerPanelMinLocation.Y, maxY, verticalWhellValue);

		float maxX = innerPanelMaxLocation.X - _innerPanel.Info.TotalSize.X;
		if (maxX < innerPanelMinLocation.X)
			maxX = innerPanelMinLocation.X;

		_innerPanel.Info.Left.Pixel = -MathHelper.Lerp(innerPanelMinLocation.X, maxX, horizontalWhellValue);

		_innerPanel.Calculation();

		if (_verticalScrollbar != null)
			_verticalScrollbar.WhellValueMult = MathHelper.Max(0f, _innerPanel.Info.TotalSize.Y / (innerPanelMaxLocation.Y - innerPanelMinLocation.Y) * 5f);
	}
}
