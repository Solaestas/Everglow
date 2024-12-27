using Everglow.Commons.UI.StringDrawerSystem.DrawerItems;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.TextDrawers;

namespace Everglow.Commons.UI.StringDrawerSystem
{
	public class StringDrawer
	{
		public delegate void AddDrawerItemHandle(DrawerItem drawerItem);

		public List<DrawerItem> DrawerItems = [];
		private List<Vector2> _lineSize = [];
		private Vector2 _size;
		public Vector2 Size => _size;

		public event AddDrawerItemHandle OnAddDrawerItem;

		public StringParameters DefaultParameters = new StringParameters();
		public int Line => _lineSize.Count;

		public void ResetAnimation()
		{
			foreach (DrawerItem item in DrawerItems)
			{
				item.ResetAnimation();
			}
		}

		public void CalculationLineSize()
		{
			int line = 0;
			_lineSize.Clear();
			_size = Vector2.Zero;
			List<DrawerItem> lineItems = DrawerItems.FindAll(i => i.Line == line);
			while (lineItems.Count > 0)
			{
				Vector2 size = Vector2.Zero;
				foreach (DrawerItem item in lineItems)
				{
					item.OnCalculationLineSize(ref size);
				}
				foreach (DrawerItem item in DrawerItems)
				{
					item.PostCalculationLineSize(line, ref size);
				}
				_lineSize.Add(size);
				_size.X = Math.Max(size.X, _size.X);
				_size.Y += size.Y;
				line++;
				lineItems = DrawerItems.FindAll(i => i.Line == line);
			}
		}

		public void SetPosition(Vector2 pos)
		{
			if (_lineSize.Count == 0)
				return;
			Vector2[] linePos = new Vector2[_lineSize.Count];
			float[] lineHeight = new float[_lineSize.Count];
			linePos[0] = pos;
			for (int i = 1; i < linePos.Length; i++)
			{
				linePos[i].X = pos.X;
				linePos[i].Y += _lineSize[i - 1].Y;
				linePos[i].Y += linePos[i - 1].Y;
			}
			foreach (DrawerItem item in DrawerItems)
			{
				item.ProcessingLineHeight(ref lineHeight[item.Line], _lineSize[item.Line]);
			}
			foreach (DrawerItem item in DrawerItems)
			{
				item.SetPosition(lineHeight[item.Line], _lineSize[item.Line], ref linePos[item.Line]);
			}
		}

		public void WordWrap(float wrapWidth)
		{
			if (DrawerItems.Count == 0)
				return;
			int addLine = 0, nowLine = 0;
			float width = wrapWidth;
			float nowWidth = 0f;
			for (int i = 0; i < DrawerItems.Count; i++)
			{
				var item = DrawerItems[i];
				if (nowLine != item.Line)
				{
					nowLine = item.Line;
					nowWidth = 0f;
					width = wrapWidth;
				}
				item.Line += addLine;
				float itemLength = item.GetSize().X;
				nowWidth += itemLength;
				if (nowWidth == width)
				{
					nowWidth = 0f;
					width = wrapWidth;
				}
				else if (nowWidth > width)
				{
					int line = item.Line, lineCache = line;
					width = item.WordWrap(ref i, DrawerItems, ref line,
						width - nowWidth + itemLength, wrapWidth);
					addLine += line - lineCache;
					nowWidth = 0f;
				}
			}
			CalculationLineSize();
		}

		public void Init(string initText)
		{
			if (string.IsNullOrEmpty(initText))
			{
				return;
			}

			DrawerItems.Clear();
			OnAddDrawerItem = null;

			int line = 0;
			Array.ForEach(initText.Split('\n'), text =>
			{
				// tIndex: text index, 无需匹配的普通内容
				// teIndex: text end index, 无需匹配的普通内容的结尾
				// dIndex: drawer index, []内的内容
				// sIndex: string index, ''内的内容
				// nIndex: name index, 特殊处理项的匹配名
				// pIndex: parameter index, 参数名
				int tIndex = 0, teIndex = 0, dIndex = -1, sIndex = -1, nIndex = -1, pIndex = -1;

				bool escActivable = false;
				StringParameters stringParameters = null;
				string parameterName = string.Empty,
					parameterValue = string.Empty,
					drawerName = string.Empty;
				for (int i = 0; i < text.Length;)
				{
					if (!escActivable && text[i] == '/')
					{
						escActivable = true;
						i++;
					}
					else
					{
						if (escActivable)
						{
							if (text[i] == '[' || text[i] == ']' || text[i] == '\'' || text[i] == '/' ||
								text[i] == ',' || text[i] == '=')
								text = text.Remove(i - 1, 1);
							else
								i++;
							escActivable = false;
						}
						else
						{
							if (text[i] == '[')
							{
								nIndex = i;
								dIndex = i;
								teIndex = i;
								if (stringParameters == null || !stringParameters.IsEmpty)
									stringParameters = new StringParameters();
							}
							else if (text[i] == ']' && dIndex != -1)
							{
								if (tIndex < teIndex)
								{
									var di = TextDrawer.Create(this, text[tIndex..teIndex], line);
									OnAddDrawerItem?.Invoke(di);
									DrawerItems.Add(di);
								}
								var d = DrawerItemLoader.Instance.GetDrawerItem(this, text[dIndex..(i + 1)], drawerName, stringParameters, line);
								OnAddDrawerItem?.Invoke(d);
								DrawerItems.Add(d);
								tIndex = i + 1;
								dIndex = -1;
								sIndex = -1;
							}
							else if (text[i] == '\'' && dIndex != -1)
							{
								if (sIndex == -1)
								{
									sIndex = i;
								}
								else
								{
									parameterValue = text[(sIndex + 1)..i];
									stringParameters[parameterName] = parameterValue;
									sIndex = -1;
								}
							}
							else if (text[i] == ',' && dIndex != -1)
							{
								if (nIndex == dIndex)
								{
									drawerName = text[(nIndex + 1)..i];
									nIndex = -1;
								}
								pIndex = i;
							}
							else if (text[i] == '=')
							{
								if (pIndex != -1)
								{
									parameterName = text[(pIndex + 1)..i];
									pIndex = -1;
								}
							}
							i++;
						}
					}
				}
				if (tIndex != text.Length)
				{
					var di = TextDrawer.Create(this, text[tIndex..text.Length], line);
					OnAddDrawerItem?.Invoke(di);
					DrawerItems.Add(di);
				}
				if (string.IsNullOrEmpty(text))
				{
					var di = TextDrawer.Create(this, text, line);
					OnAddDrawerItem?.Invoke(di);
					DrawerItems.Add(di);
				}
				line++;
			});
			CalculationLineSize();
		}

		public void Draw(SpriteBatch sb)
		{
			foreach (DrawerItem item in DrawerItems)
			{
				item.Draw(sb);
			}
		}
	}
}