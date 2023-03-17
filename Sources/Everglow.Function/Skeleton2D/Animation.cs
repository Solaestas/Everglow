namespace Everglow.Commons.Skeleton2D;

public class Track
{
	public List<IKeyFrame> KeyFrames
	{
		get; set;
	}

	public Track()
	{
		KeyFrames = new List<IKeyFrame>();
	}

	public void AddKeyFrame(IKeyFrame keyframe)
	{
		KeyFrames.Add(keyframe);
	}

	public void LocateKeyFrames(float t, out IKeyFrame cur, out IKeyFrame next)
	{
		if (KeyFrames.Count == 0)
		{
			// 某些关键帧有不支持的功能
			cur = null;
			next = null;
			return;
		}
		int l = 0, r = KeyFrames.Count - 1;
		int ans = r;
		while (l <= r)
		{
			int mid = (l + r) / 2;
			if (t >= KeyFrames[mid].Time)
			{
				l = mid + 1;
				ans = mid;
			}
			else
			{
				r = mid - 1;
			}
		}
		cur = KeyFrames[ans];
		if (ans >= KeyFrames.Count - 1)
		{
			next = null;
		}
		else
		{
			next = KeyFrames[ans + 1];
		}
	}
}
public class Timeline
{
	public List<Track> Tracks
	{
		get; set;
	}
	public Timeline()
	{
		Tracks = new List<Track>();
	}

	public void AddTrack(Track track)
	{
		Tracks.Add(track);
	}
}

public class Animation
{
	public string Name
	{
		get; set;
	}

	/// <summary>
	/// 一个动画可以操作多个骨头，每个BoneTimeline作用在一个骨头上
	/// </summary>
	public List<Timeline> BonesTimeline
	{
		get; set;
	}

	/// <summary>
	/// 一个动画可以操作多个插槽，每个时间轴里包含很多个关键帧
	/// </summary>
	public List<Timeline> SlotsTimeline
	{
		get; set;
	}
}
