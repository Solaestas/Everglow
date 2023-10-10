namespace Everglow.Commons.GroundLayer.Basics
{
	public interface IUniqueID<TKey> where TKey : IComparable<TKey>
	{
		public TKey UniqueID { get; }
	}
}