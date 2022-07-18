namespace Everglow.Sources.Commons.Core.VFX
{
    /// <summary>
    /// 表示一种IVisual所需的所有Pipeline的排列
    /// </summary>
    internal class PipelineIndex : IEquatable<PipelineIndex>
    {
        public int index;
        public PipelineIndex next;
        public PipelineIndex(int index)
        {
            this.index = index;
        }

        public PipelineIndex(IEnumerable<int> indices)
        {
            var current = this;
            foreach (var i in indices)
            {
                if (current == null)
                {
                    current = new PipelineIndex(i);
                }
                else
                {
                    current.index = i;
                }
                current = current.next;
            }
        }
        public int GetDepth()
        {
            int count = 1;
            var next = this.next;
            while(next != null)
            {
                next = next.next;
                count++;
            }
            return count;
        }
        public bool Equals(PipelineIndex other)
        {
            return other != null && index == other.index && next == other.next;
        }

        public override bool Equals(object obj)
        {
            return obj != null && Equals(obj as PipelineIndex);
        }

        public override int GetHashCode()
        {
            if (next == null)
            {
                return index.GetHashCode();
            }
            return index.GetHashCode() + next.GetHashCode();
        }
    }
}
