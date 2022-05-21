using Newtonsoft.Json;

namespace Everglow.Sources.Modules.Food.DataStructures
{
    public class FoodDuration
    {
        public double TotalSeconds
        {
            get
            {
                return TotalFrames / (double)FramesPerSecond;
            }
        }

        public double TotalMinutes
        {
            get
            {
                return TotalFrames / (double)FramesPerMinute;
            }
        }

        public double TotalHours
        {
            get
            {
                return TotalFrames / (double)FramesPerHour;
            }
        }

        public int TotalFrames
        {
            get
            {
                return (((m_hours * 60 + m_minutes) * 60 + m_seconds) * 60) + m_frames;
            }
        }

        [JsonProperty(PropertyName = "Hours")]
        private int m_hours;
        [JsonProperty(PropertyName = "Minutes")]
        private int m_minutes;
        [JsonProperty(PropertyName = "Seconds")]
        private int m_seconds;
        [JsonProperty(PropertyName = "Frames")]
        private int m_frames;

        private const int FramesPerSecond = 60;
        private const int FramesPerMinute = 3600;
        private const int FramesPerHour = 216000;

        public FoodDuration(int hours, int minutes, int seconds, int frames)
        {
            m_hours = hours;
            m_minutes = minutes;
            m_seconds = seconds;
            m_frames = frames;

            Normalize();
        }

        public FoodDuration(int minutes, int seconds, int frames)
            : this(0, minutes, seconds, frames)
        {

        }

        public FoodDuration(int seconds, int frames)
            : this(0, 0, seconds, frames)
        {

        }

        public FoodDuration(int frames)
            : this(0, 0, 0, frames)
        {

        }

        private void Normalize()
        {
            int carry = m_frames / 60;
            m_frames %= 60;

            m_seconds += carry;
            carry = m_seconds / 60;
            m_seconds %= 60;

            m_minutes += carry;
            carry = m_minutes / 60;
            m_minutes %= 60;

            m_hours += carry;
        }
    }
}