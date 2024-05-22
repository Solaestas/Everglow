using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Everglow.Commons.Skeleton2D.Reader;

/// <summary>
/// Json: Bone 段存储的是骨头的信息
/// </summary>
public class JBone
{
	/// <summary>
	/// 骨头的名字，在每一个骨架中，骨头的名字是唯一的
	/// </summary>
	[JsonProperty("name")]
	public string Name;

	[DefaultValue("")]
	[JsonProperty(PropertyName = "parent", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string Parent = "";

	/// <summary>
	/// 骨头的长度，这个东西在运行时没什么用，主要是为了Debug用的
	/// </summary>
	[DefaultValue(0)]
	[JsonProperty(PropertyName = "length", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public float Length = 0f;

	/// <summary>
	/// TPose的时候该骨头相对于它的父亲节点的x的位置属性，如果这个属性没有出现在bone的属性列表里面的话，它的值是0
	/// </summary>
	[DefaultValue(0f)]
	[JsonProperty(PropertyName = "x", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public float PosX = 0f;

	/// <summary>
	///  TPose的时候该骨头相对于它的父亲节点的y的位置属性，如果这个属性没有出现在bone的属性列表里面的话，它的值是0
	/// </summary>
	[DefaultValue(0f)]
	[JsonProperty(PropertyName = "y", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public float PosY = 0f;

	/// <summary>
	/// TPose的时候该骨头相对于它的父亲节点的旋转角度，如果这个属性没有出现在bone的属性列表里面的话，它的值是0
	/// </summary>
	[DefaultValue(0f)]
	[JsonProperty(PropertyName = "rotation", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public float Rotation = 0f;

	/// <summary>
	/// TPose的时候该骨头相对于它的父亲节点的X方向缩放倍数（暂时不用）
	/// </summary>
	[DefaultValue(1f)]
	[JsonProperty(PropertyName = "scaleX", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public float ScaleX = 1f;

	/// <summary>
	/// TPose的时候该骨头相对于它的父亲节点的Y方向缩放倍数（暂时不用）
	/// </summary>
	[DefaultValue(1f)]
	[JsonProperty(PropertyName = "scaleY", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public float ScaleY = 1f;

	/// <summary>
	/// 骨头的颜色RGBA（Debug用）
	/// </summary>
	[DefaultValue("#ffffffff")]
	[JsonProperty(PropertyName = "color", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
	public string Color = "#ffffffff";

	//       {
	//	"name": "branch-1",
	//	"parent": "root",
	//	"length": 487.04,
	//	"rotation": 27.67,
	//	"x": -579.3,
	//	"y": -239.11,
	//	"color": "1a8600ff"
	//},
	public JBone()
	{
	}
}
