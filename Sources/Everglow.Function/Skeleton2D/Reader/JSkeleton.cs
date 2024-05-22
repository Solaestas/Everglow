using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Everglow.Commons.Skeleton2D.Reader;
/// <summary>
/// Json 骨架数据格式
/// </summary>
public class JSkeleton
{
	[JsonProperty("skeleton")]
	public JSkeletonInfo SkeletonInfo;

	[JsonProperty("bones")]
	public List<JBone> Bones;

	[JsonProperty("slots")]
	public List<JSlot> Slots;

	[JsonProperty("skins")]
	public List<JSkin> Skins;

	[JsonProperty(PropertyName = "animations", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public Dictionary<string, JAnimation> Animations;

	public JSkeleton()
	{
	}


}
