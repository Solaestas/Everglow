namespace Everglow.Commons.Skeleton2D;

public abstract class InterpolationMethod
{
	public static InterpolationMethod Lerp = new Lerp();
	public static InterpolationMethod Step = new Step();
	public abstract float GetValue(float t);
}

public class Lerp : InterpolationMethod
{
	public override float GetValue(float t)
	{
		return t;
	}
}

public class Step : InterpolationMethod
{
	public override float GetValue(float t)
	{
		return t >= 0 ? 1 : 0;
	}
}

public class Curve : InterpolationMethod
{
	private Vector2 m_pA, m_pB;
	public Curve(Vector2 A, Vector2 B)
	{
		m_pA = A;
		m_pB = B;
	}
	public override float GetValue(float t)
	{
		Vector2 p0 = Vector2.Zero;
		Vector2 p1 = Vector2.One;
		Vector2 p = 3 * m_pA * t * (1 - t) * (1 - t) + 3 * m_pB * t * t * (1 - t) + p1 * t * t * t;
		return p.Y;
	}
}


public interface IKeyFrame
{
	public float Time
	{
		get; set;
	}
	public InterpolationMethod Interpolation
	{
		get; set;
	}

	public void Interpolate(float t, IKeyFrame nextKFrame);
}

public abstract class BoneKeyFrame : IKeyFrame
{
	public Bone2D Bone
	{
		get; set;
	}
	public float Time
	{
		get; set;
	}
	public InterpolationMethod Interpolation
	{
		get; set;
	}

	public abstract void Interpolate(float t, IKeyFrame nextKFrame);
}

public abstract class SlotKeyFrame : IKeyFrame
{
	public Slot Slot
	{
		get; set;
	}
	public float Time
	{
		get; set;
	}
	public InterpolationMethod Interpolation
	{
		get; set;
	}

	public abstract void Interpolate(float t, IKeyFrame nextKFrame);
}

public class BoneTranslationKeyFrame : BoneKeyFrame
{
	public BoneTranslationKeyFrame(float time, Bone2D bone, InterpolationMethod interp, Vector2 translation)
	{
		Time = time;
		Bone = bone;
		Interpolation = interp;
		m_translation = translation;
	}

	private Vector2 m_translation;

	public override void Interpolate(float t, IKeyFrame nextKFrame)
	{
		if (nextKFrame == null)
		{
			Bone.Position = m_translation;
			return;
		}
		var nextK = nextKFrame as BoneTranslationKeyFrame;
		float factor = MathHelper.Clamp((t - Time) / (nextKFrame.Time - Time), 0, 1f);
		Bone.Position = Vector2.Lerp(m_translation, nextK.m_translation, Interpolation.GetValue(factor));
	}
}
public class BoneRotationKeyFrame : BoneKeyFrame
{
	public BoneRotationKeyFrame(float time, Bone2D bone, InterpolationMethod interp, float rotation)
	{
		Time = time;
		Bone = bone;
		Interpolation = interp;
		m_rotation = rotation;
	}

	private float m_rotation;

	public override void Interpolate(float t, IKeyFrame nextKFrame)
	{
		if (nextKFrame == null)
		{
			Bone.Rotation = m_rotation;
			return;
		}
		var nextK = nextKFrame as BoneRotationKeyFrame;
		float factor = MathHelper.Clamp((t - Time) / (nextKFrame.Time - Time), 0, 1f);
		Bone.Rotation = MathHelper.Lerp(m_rotation, nextK.m_rotation, Interpolation.GetValue(factor));
	}
}


public class SlotAttachmentKeyFrame : SlotKeyFrame
{
	public SlotAttachmentKeyFrame(float time, Slot slot, Attachment attachment)
	{
		Time = time;
		Slot = slot;
		m_attachment = attachment;
	}

	private Attachment m_attachment;

	public override void Interpolate(float t, IKeyFrame nextKFrame)
	{
		if (nextKFrame == null)
		{
			Slot.Attachment = m_attachment;
			return;
		}
		var nextK = nextKFrame as SlotAttachmentKeyFrame;
		Slot.Attachment = m_attachment;
	}
}
