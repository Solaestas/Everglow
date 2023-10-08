using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Everglow.Commons.GroundLayer.Basics
{
	public class StructCollection<T>(int size = 5000, T defaultValue = default) where T : struct, IUniqueID<int>
	{
		Dictionary<int, int> map = new();
		T[] _container = new T[size];
		int _ptr = 0;
		T _default = defaultValue;
		public ref T this[int uniqueID]
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
		public bool Add(T content)
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
		public bool Remove(int uniqueID)
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
		public void Clear()
		{
			_ptr = 0;
			map.Clear();
		}
		public bool Contains(int uniqueID)
		{
			return map.ContainsKey(uniqueID);
		}
		public bool Resize(int newSize)
		{
			if (newSize < _ptr)
			{
				return false;
			}
			var newContainer = new T[newSize];
			Array.Copy(_container, newContainer, _ptr);
			_container = newContainer;
			return true;
		}
		public T[] GetUpdateElements()
		{
			return _container[0.._ptr];
		}
		public void Sort(Comparison<T> comparison = null)
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