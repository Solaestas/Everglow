using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.GroundLayer.Basics
{
	public class StructCollection<TKey, TValue>(int size = 5000, TValue defaultValue = default) where TValue : struct, IUniqueID<TKey> where TKey : IComparable<TKey>
	{
		Dictionary<TKey, int> map = new();
		TValue[] _container = new TValue[size];
		int _ptr = 0;
		TValue _default = defaultValue;
		public int Count => _ptr;
		public ref TValue this[TKey uniqueID]
		{
			get
			{
				if (map.TryGetValue(uniqueID, out int index))
				{
					return ref _container[index];
				}
				return ref _default;
			}
		}
		public bool Add(ref TValue content)
		{
			if (_ptr == _container.Length)
			{
				return false;
			}
			_container[_ptr] = content;
			map[content.UniqueID] = _ptr;
			_ptr++;
			return true;
		}
		public bool Remove(TKey uniqueID)
		{
			if (!map.TryGetValue(uniqueID, out int index))
			{
				return false;
			}
			_container[index] = _container[_ptr - 1];
			map.Remove(uniqueID);
			_ptr--;
			return true;
		}
		public bool TryGet(TKey uniqueID, ref TValue content)
		{
			if (map.TryGetValue(uniqueID, out int index))
			{
				content = _container[index];
				return true;
			}
			content = _default;
			return false;
		}
		public void Clear()
		{
			_ptr = 0;
			map.Clear();
		}
		public bool Contains(TKey uniqueID)
		{
			return map.ContainsKey(uniqueID);
		}
		public bool Resize(int newSize)
		{
			if (newSize < _ptr)
			{
				return false;
			}
			var newContainer = new TValue[newSize];
			Array.Copy(_container, newContainer, _ptr);
			_container = newContainer;
			return true;
		}
		public TValue[] GetUpdateElements()
		{
			return _container[0.._ptr];
		}
		public void Sort(Comparison<TValue> comparison = null)
		{
			comparison ??= (t1, t2) => t1.UniqueID.CompareTo(t2.UniqueID);
			Array.Sort(_container[0.._ptr], comparison);
			map.Clear();
			for (int i = 0; i < _ptr; i++)
			{
				map[_container[i].UniqueID] = i;
			}
		}
	}
}