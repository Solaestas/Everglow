namespace Everglow.Sources.Commons.Core.Profiler
{
    internal class UnitInfo
    {
        public int Count
        {
            get; set;
        }

        public double TimeInMs
        {
            get; set;
        }

        public string Name
        {
            get; set;
        }

        public UnitInfo(string name, int count, double timeInMs)
        {
            Name = name;
            Count = count;
            TimeInMs = timeInMs;
        }

        public UnitInfo()
        {
            Name = "";
            Count = 0;
            TimeInMs = 0;
        }
    }
}
