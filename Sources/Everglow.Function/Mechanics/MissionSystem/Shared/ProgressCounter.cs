using Terraria.ModLoader.IO;

namespace Everglow.Commons.Mechanics.MissionSystem.Shared;

public class ProgressCounter
{
	public int Value { get; private set; }

	/// <summary>
	/// Add number to Counter
	/// </summary>
	/// <param name="num"></param>
	public void Count(int num = 1)
	{
		if (num <= 0)
		{
			throw new InvalidDataException("Count must be greater than 0");
		}

		Value += num;
	}

	public class CollectItemRequirementSerializer : TagSerializer<ProgressCounter, TagCompound>
	{
		public override TagCompound Serialize(ProgressCounter value) => new TagCompound()
		{
			[nameof(Value)] = value.Value,
		};

		public override ProgressCounter Deserialize(TagCompound tag) => new ProgressCounter()
		{
			Value = tag.GetInt(nameof(Value)),
		};
	}
}