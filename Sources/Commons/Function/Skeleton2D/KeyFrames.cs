using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Sources.Commons.Function.Skeleton2D
{
    internal enum InterpolationType
    {
        Linear,
        Step,
        Cubic
    }
    internal interface IKeyFrame
    {
        public float Time
        {
            get; set;
        }
        public InterpolationType Interpolation
        {
            get; set;
        }

        public void Interpolate(float t, IKeyFrame nextKFrame);
    }

    internal abstract class BoneKeyFrame : IKeyFrame
    {
        public Bone2D Bone
        {
            get; set;
        }
        public float Time
        {
            get; set;
        }
        public InterpolationType Interpolation
        {
            get; set;
        }

        public abstract void Interpolate(float t, IKeyFrame nextKFrame);
    }

    internal abstract class SlotKeyFrame : IKeyFrame
    {
        public Slot Slot
        {
            get; set;
        }
        public float Time
        {
            get; set;
        }
        public InterpolationType Interpolation
        {
            get; set;
        }

        public abstract void Interpolate(float t, IKeyFrame nextKFrame);
    }

    internal class BoneTranslationKeyFrame : BoneKeyFrame
    {
        public BoneTranslationKeyFrame(float time, Bone2D bone, InterpolationType type, Vector2 translation)
        {
            Time = time;
            Bone = bone;
            Interpolation = type;
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
            switch (Interpolation)
            {
                case InterpolationType.Linear:
                    Bone.Position = Vector2.Lerp(m_translation, nextK.m_translation, factor);
                    break;
                case InterpolationType.Step:
                    if (t >= Time)
                    {
                        Bone.Position = m_translation;
                    }
                    break;
            }
        }
    }
    internal class BoneRotationKeyFrame : BoneKeyFrame
    {
        public BoneRotationKeyFrame(float time, Bone2D bone, InterpolationType type, float rotation)
        {
            Time = time;
            Bone = bone;
            Interpolation = type;
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
            switch (Interpolation)
            {
                case InterpolationType.Linear:
                    Bone.Rotation = MathHelper.Lerp(m_rotation, nextK.m_rotation, factor);
                    break;
                case InterpolationType.Step:
                    if (t >= Time)
                    {
                        Bone.Rotation = m_rotation;
                    }
                    break;
            }
        }
    }


    internal class SlotAttachmentKeyFrame : SlotKeyFrame
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
            float factor = MathHelper.Clamp((t - Time) / (nextKFrame.Time - Time), 0, 1f);
            switch (Interpolation)
            {
                case InterpolationType.Linear:
                    Slot.Attachment = m_attachment;
                    break;
                case InterpolationType.Step:
                    Slot.Attachment = m_attachment;
                    break;
            }
        }
    }
}
