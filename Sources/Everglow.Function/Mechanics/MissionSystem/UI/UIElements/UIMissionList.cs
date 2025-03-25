using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Enums;
using Everglow.Commons.Mechanics.MissionSystem.Utilities;
using Everglow.Commons.UI.UIElements;
using Everglow.Commons.Vertex;

namespace Everglow.Commons.Mechanics.MissionSystem.UI.UIElements;

public class UIMissionList : UIBlock
{
	private MissionListContent _missionList;
	private UIMissionListScrollbar _missionScrollbar;

	public List<UIMissionItem> MissionItems => _missionList.Elements.ConvertAll(x => x as UIMissionItem);

	public override void OnInitialization()
	{
		Info.SetMargin(0);

		// Mission list
		_missionList = new MissionListContent();
		Register(_missionList);

		// Mission list scrollbar
		_missionScrollbar = new UIMissionListScrollbar();
		_missionScrollbar.Info.Left.SetValue(PositionStyle.Full - _missionScrollbar.Info.Width - (4f, 0f));
		_missionScrollbar.Info.Height.SetValue(PositionStyle.Full - (20, 0f));
		_missionList.SetVerticalScrollbar(_missionScrollbar);
		Register(_missionScrollbar);

		_missionList.Info.Width.SetValue(_missionScrollbar.Info.Left);
	}

	/// <summary>
	/// 刷新任务列表
	/// </summary>
	public void RefreshList(PoolType? poolType, MissionType? missionType, bool nPCMode, int sourceNPC)
	{
		// 筛选任务状态，获得初始列表
		var missions = poolType.HasValue
			? MissionManager.GetMissionPool(poolType.Value)
			: Enum.GetValues<PoolType>().Select(MissionManager.GetMissionPool).SelectMany(x => x);

		// 筛选来源NPC
		if (nPCMode) // NPC模式，去掉非对应NPC的未接取任务
		{
			missions = missions.Where(m => m.PoolType is not PoolType.Available && m.SourceNPC == sourceNPC);
		}
		else // 全局模式，去掉有来源NPC的未接取任务
		{
			missions = missions.Where(m => !(m.PoolType is PoolType.Available && m.SourceNPC >= 0));
		}

		// 筛选任务类型
		if (missionType.HasValue)
		{
			missions = missions.Where(m => m.MissionType == missionType);
		}

		// 排序
		missions = missions.Order(MissionComparer.Instance);

		// 生成任务UI元素
		List<BaseElement> elements = [];
		float ElementSpacing = 10 * MissionContainer.Scale;
		PositionStyle top = (4 * MissionContainer.Scale, 0f);
		foreach (var m in missions.ToList())
		{
			if (!m.IsVisible)
			{
				continue;
			}

			var element = (BaseElement)Activator.CreateInstance(m.BindingUIItem, [m]);
			element.OnInitialization();
			element.Info.Top.SetValue(top);
			element.Events.OnLeftDown += e =>
			{
				if (MissionContainer.Instance.SelectedItem != e)
				{
					MissionContainer.Instance.ChangeSelectedItem((UIMissionItem)e);
				}
			};

			elements.Add(element);

			top += element.Info.Height;
			top.Pixel += ElementSpacing;
		}

		_missionList.ClearAllElements();
		_missionList.AddElements(elements);
	}

	private class MissionListContent : UIContainerPanel
	{
		public override void Draw(SpriteBatch sb)
		{
			base.Draw(sb);
			var texture = ModAsset.MirrorChain.Value;

			// Draw mirrior chains (Left. Move with mission items synchronously)
			var vertices = new List<Vertex2D>();
			{
				float scale = MissionContainer.Scale;
				float width = 7 * scale;
				float height = 70 * scale;

				float startX = HitBox.X + 5 * scale;
				float endX = startX + width;

				float startY = HitBox.Y - 2 * scale;
				float endY = startY + height * (5 + 0.078f * scale);

				float startTexCoordY = 0 - 0.0285f * scale;
				float endTexCoordY = 5 + 0.078f * scale;

				float resourceOffset = 0.23f;
				startTexCoordY -= resourceOffset;
				endTexCoordY -= resourceOffset;

				float scrollOffset = -VerticalScrollDistance / height;
				startTexCoordY += scrollOffset;
				endTexCoordY += scrollOffset;

				vertices.Add(new Vector2(startX, startY), Color.White, new(0, startTexCoordY, 0));
				vertices.Add(new Vector2(endX, startY), Color.White, new(1, startTexCoordY, 0));
				vertices.Add(new Vector2(startX, endY), Color.White, new(0, endTexCoordY, 0));
				vertices.Add(new Vector2(endX, endY), Color.White, new(1, endTexCoordY, 0));
			}

			Main.graphics.GraphicsDevice.Textures[0] = texture;
			Main.graphics.GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);

			// Draw mirrior chains (Right. Used as scrollbar)
			vertices = new List<Vertex2D>();
			{
				float scale = MissionContainer.Scale;
				float width = 7 * scale;
				float height = 70 * scale;

				float startX = HitBox.X + HitBox.Width;
				float endX = startX + width;

				float startY = HitBox.Y - 2 * scale;
				float endY = startY + height * (5 + 0.078f * scale);

				float startTexCoordY = 0 - 2 * scale / height;
				float endTexCoordY = 5 + 0.078f * scale;

				float scrollOffset = -HitBox.Height * UIVerticalScrollbar.WheelValue / height;
				startTexCoordY += scrollOffset;
				endTexCoordY += scrollOffset;

				vertices.Add(new Vector2(startX, startY), Color.White, new(0, startTexCoordY, 0));
				vertices.Add(new Vector2(endX, startY), Color.White, new(1, startTexCoordY, 0));
				vertices.Add(new Vector2(startX, endY), Color.White, new(0, endTexCoordY, 0));
				vertices.Add(new Vector2(endX, endY), Color.White, new(1, endTexCoordY, 0));
			}
			Main.graphics.GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, vertices.ToArray(), 0, vertices.Count - 2);
		}
	}
}