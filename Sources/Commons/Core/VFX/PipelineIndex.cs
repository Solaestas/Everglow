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
            using var it = indices.GetEnumerator();
            if (!it.MoveNext())
            {
                throw new ArgumentException("Indices count should > 0");
            }

            current.index = it.Current;
            while (it.MoveNext())
            {
                current.next = new PipelineIndex(it.Current);
                current = current.next;
            }
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
