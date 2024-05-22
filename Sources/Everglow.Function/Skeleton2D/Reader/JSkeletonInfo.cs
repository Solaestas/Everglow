using System.ComponentModel;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Everglow.Commons.Skeleton2D.Reader;

/// <summary>
/// Json: Skeleton 段存储的数据是关于骨架的一些元数据
/// </summary>
public class JSkeletonInfo
{
	[JsonProperty("hash")]
	[JsonRequired]
	public string Hash
	{
		get; set;
	}

	[JsonProperty("spine")]
	[JsonRequired]
	public string SpineVersion
	{
		get; set;
	}

	[JsonProperty("x")]
	[JsonRequired]
	public float BoundingBoxX
	{
		get; set;
	}

	[JsonProperty("y")]
	[JsonRequired]
	public float BoundingBoxY
	{
		get; set;
	}

	[JsonProperty("width")]
	[JsonRequired]
	public float BoundingBoxWidth
	{
		get; set;
	}

	[JsonProperty("height")]
	[JsonRequired]
	public float BoundingBoxHeight
	{
		get; set;
	}

	[JsonProperty("images")]
	[JsonRequired]
	public string ImagesPath
	{
		get; set;
	}

	[DefaultValue(30)]
	[JsonProperty(PropertyName = "fps", DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]

	public int FramesPerSecond
	{
		get; set;
	}

	[JsonProperty("audio")]
	public string AudioPath
	{
		get; set;
	}

	public JSkeletonInfo()
	{
	}
}
