using Everglow.Commons.Mechanics.MissionSystem.Core;
using Everglow.Commons.Mechanics.MissionSystem.Primitives;
using Everglow.Commons.Mechanics.MissionSystem.Shared.Icons;
using Everglow.Commons.UI.StringDrawerSystem.DrawerItems.ImageDrawers;
using Everglow.Commons.Utilities;

namespace Everglow.Commons.Mechanics.MissionSystem.Objectives;

public class GiveItemObjective : MissionObjectiveBase
{
	public GiveItemObjective()
	{
	}

	public GiveItemObjective(ItemRequirement requirement, int npcType)
	{
		DemandGiveItem = requirement;
		NPCType = npcType >= NPCID.None
			? npcType
			: throw new InvalidDataException($"NPC type should more than 0.");

		StartText = "请给我一些东西。";

		EndText = "谢谢你！";
	}

	public GiveItemObjective(ItemRequirement requirement, int npcType, string startText, string endText)
	{
		DemandGiveItem = requirement;
		NPCType = npcType >= NPCID.None
			? npcType
			: throw new InvalidDataException($"NPC type should more than 0.");

		StartText = !string.IsNullOrEmpty(startText)
			? startText
			: throw new ArgumentNullException("Argument 'text' should not be empty!");

		EndText = !string.IsNullOrEmpty(endText)
			? endText
			: throw new ArgumentNullException("Argument 'text' should not be empty!");
	}

	public int NPCType { get; set; }

	public string StartText { get; set; }

	public string EndText { get; set; }

	public ItemRequirement DemandGiveItem { get; set; }

	public override float Progress => DemandGiveItem.GetInventoryProgress(Main.LocalPlayer.inventory);

	public bool IsTalkingToNPC => NPCType == NPCID.None || (NPCType > NPCID.None && Main.LocalPlayer.talkNPC >= NPCID.None && Main.npc[Main.LocalPlayer.talkNPC].type == NPCType);

	public override void OnInitialize()
	{
		base.OnInitialize();
		AssetUtils.LoadVanillaItemTextures(DemandGiveItem.Items);
		AssetUtils.LoadVanillaNPCTextures([NPCType]);
	}

	public override bool CheckCompletion() => IsTalkingToNPC && DemandGiveItem.GetInventoryProgress(Main.LocalPlayer.inventory) >= 1f;

	public override void Update()
	{
		base.Update();

		if (IsTalkingToNPC)
		{
			Main.npcChatText = StartText;
		}
	}

	/// <summary>
	/// Remove required items from player inventory.
	/// </summary>
	/// <param name="inventory"></param>
	public void RemoveItem(IEnumerable<Item> inventory)
	{
		var stackCount = DemandGiveItem.Requirement;
		foreach (var inventoryItem in inventory.Where(x => DemandGiveItem.Items.Contains(x.type)))
		{
			if (inventoryItem.stack < stackCount)
			{
				stackCount -= inventoryItem.stack;
				inventoryItem.stack = 0;
			}
			else
			{
				inventoryItem.stack -= stackCount;
				break;
			}
		}
	}

	public override void Complete()
	{
		// Make sure the items can only be removed once.
		if (!Completed)
		{
			RemoveItem(Main.LocalPlayer.inventory);
		}

		if (IsTalkingToNPC)
		{
			Main.npcChatText = EndText;
		}

		base.Complete();
	}

	public override void GetObjectivesIcon(MissionIconGroup iconGroup)
	{
		var npc = new NPC();
		npc.SetDefaults(NPCType);
		iconGroup.Add(NPCMissionIcon.Create(NPCType, npc.TypeName));

		foreach (var item in DemandGiveItem.Items)
		{
			iconGroup.Add(ItemMissionIcon.Create(item, new Item(item).Name));
		}
	}

	public override void GetObjectivesText(List<string> lines)
	{
		var npc = new NPC();
		npc.SetDefaults(NPCType);

		var progress = $"({Main.LocalPlayer.inventory.Where(i => DemandGiveItem.Items.Contains(i.type)).Sum(i => i.stack)}/{DemandGiveItem.Requirement})";
		if (DemandGiveItem.Items.Count > 1)
		{
			var itemString = string.Join(' ', DemandGiveItem.Items.ConvertAll(i => ItemDrawer.Create(i)));
			lines.Add($"向{npc.TypeName}提交{itemString}合计{DemandGiveItem.Requirement}个 {progress}\n");
		}
		else
		{
			lines.Add($"向{npc.TypeName}提交{ItemDrawer.Create(DemandGiveItem.Items.First())}{DemandGiveItem.Requirement}个 {progress}\n");
		}
	}
}