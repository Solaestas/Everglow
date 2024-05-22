using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Everglow.Commons.Skeleton2D.Reader;

/// <summary>
/// Json: Skin块每个皮肤都描述了可分配给每个槽位的附件
/// </summary>
public class JSkin
{
	/// <summary>
	/// 插槽的名字，对于每个skeleton来说，slot的名字是唯一的
	/// </summary>
	[JsonProperty("name")]
	public string Name;

	/// <summary>
	/// 在TPose的状态下该插槽里挂载的挂件的名字
	/// </summary>
	[JsonProperty("attachments")]
	[JsonConverter(typeof(AttachmentsJsonConverter))]
	public JAttachments Attachments;
}
