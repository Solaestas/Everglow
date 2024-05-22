using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Everglow.Commons.Skeleton2D.Reader;


/// <summary>
/// Json: Slots数据块描述的是渲染顺序以及2D图片的挂件挂载到哪些插孔清单
/// </summary>
public class JSlot
{
	/// <summary>
	/// 插槽的名字，对于每个skeleton来说，slot的名字是唯一的
	/// </summary>
	[JsonProperty("name")]
	public string Name;

	/// <summary>
	/// 插槽所在的骨头的名字
	/// </summary>
	[JsonProperty("bone")]
	public string Bone;

	/// <summary>
	/// 在TPose的状态下该插槽里挂载的挂件的名字
	/// </summary>
	[JsonProperty("attachment")]
	public string Attachment;
}
