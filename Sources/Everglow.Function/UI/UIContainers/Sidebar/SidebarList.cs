using Everglow.Commons.UI.UIElements;

namespace Everglow.Commons.UI.UIContainers.Sidebar
{
	internal class SidebarList : BaseElement
	{
		private float elementSpacing = 16f;
		private List<SidebarUIElement> elements = new List<SidebarUIElement>();

		public override bool Register(BaseElement element)
		{
			if (element is SidebarUIElement)
			{
				element.Info.Width.SetValue(0f, 1f);

				var op = base.Register(element);
				if (!op)
					return op;

				elements.Add((SidebarUIElement)element);
				Calculation();
				return op;
			}
			else
				return false;
		}

		public override bool Remove(BaseElement element)
		{
			if (!(element is SidebarUIElement))
				return false;
			var op = elements.Remove((SidebarUIElement)element) && base.Remove(element);
			Calculation();
			return op;
		}

		public override void RemoveAll()
		{
			base.RemoveAll();
			elements.Clear();
			Calculation();
		}

		public void CalculationQuickElementCenter()
		{
			int count = 0;
			foreach (var element in elements)
			{
				if (element.IsVisible)
				{
					element.Center = Info.Location + new Vector2(Info.HitBox.Width / 2f, 5f + (2 * count + 1) * (elementSpacing / 2f + Info.HitBox.Width / 2f));
					count++;
				}
			}
		}

		public void SwapCenter(SidebarUIElement element, SidebarUIElement element1)
		{
			int container = element.IndexInList;
			element.IndexInList = element1.IndexInList;
			element1.IndexInList = container;
		}

		public override void Update(GameTime gt)
		{
			base.Update(gt);
			CalculationQuickElementCenter();
			SidebarUIElement movingElement = null;
			int index = -1;
			for (int i = 0; i < elements.Count; i++)
			{
				if (elements[i].IsMoveing)
				{
					movingElement = elements[i];
					index = i;
					break;
				}
			}
			if (movingElement == null || index < 0)
				return;
			if (index > 0 && movingElement.Info.Location.Y <
				(elements[index - 1].Center.Y + elements[index - 1].Info.Size.Y / 2f + elementSpacing / 2f))
			{
				SwapCenter(movingElement, elements[index - 1]);
			}
			if (index < (elements.Count - 1) && movingElement.Info.Location.Y >
				(elements[index + 1].Center.Y - elements[index + 1].Info.Size.Y / 2f - elementSpacing / 2f))
			{
				SwapCenter(movingElement, elements[index + 1]);
			}
		}

		public void SortElements()
		{
			elements.Sort((x1, x2) =>
			{
				if (x1.IsVisible == x2.IsVisible)
				{
					return x1.IndexInList.CompareTo(x2.IndexInList);
				}
				else if (x1.IsVisible)
					return -1;
				else
					return 1;
			});
		}

		public override void Calculation()
		{
			Info.Height.SetValue(elements.Count * (Info.Size.X + elementSpacing) + 10f, 0f);
			base.Calculation();
		}
	}
}