using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.ClientUtilities.Colections
{
	public class FastList<T> : IEnumerable<T>
	{
		public T[] _Array;
		public int Count;
		public int Capacity;
		private int requestedCount;
		private EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;

		public T this[int Index]
		{
			get
			{
				if (Index < 0 || Index >= Count)
					throw new IndexOutOfRangeException(nameof(Index));

				return _Array[Index];
			}

			set
			{
				if (Index < 0 || Index >= Count)
					throw new IndexOutOfRangeException(nameof(Index));

				_Array[Index] = value;
			}
		}

		public FastList()
		{
			_Array = null;
			Count = 0;
		}

		public FastList(int Cap)
		{
			requestedCount = Cap;
			Allocate();
		}

		public FastList(T[] Collection)
		{
			AddRange(Collection);
		}


		private void setItem(int Index, T Item)
		{
			if (Index >= Count)
				throw new IndexOutOfRangeException(nameof(Index));

			_Array[Index] = Item;
		}


		public void Add(T Item)
		{
			if (Count == Capacity)
			{
				requestedCount = 1;
				Allocate();
			}

			_Array[Count++] = Item;
		}

		public void AddRange(T[] Collection)
		{
			if (Collection == null) throw new ArgumentNullException(nameof(Collection));
			int count = Collection.Length;

			if (count == 0)
				return;
			requestedCount = count;
			Allocate();
			for (int i = 0; i < Collection.Length; ++i)
				_Array[this.Count++] = Collection[i];
		}

		public void Insert(int Index, T Item)
		{
			if (Count == Capacity)
			{
				requestedCount = 1;
				Allocate();
			}
			if (Index < Count)
			{
				for (int i = Count; i > Index; --i)
					_Array[i] = _Array[i - 1];

				_Array[Index] = Item;
				++Count;
			}
			else
				Add(Item);
		}

		public void InsertRange(int Index, T[] Collection)
		{
			if (Collection == null) throw new ArgumentNullException(nameof(Collection));
			int count = Collection.Length;
			if (count == 0)
				return;
			int it = 0;
			for (int i = 0; i < Collection.Length; ++i)
			{
				Insert(Index + it, Collection[i]);
				++it;
			}
		}


		public bool Contains(T Item)
		{
			if (_Array == null || _Array.Length == 0)
				return false;

			for (int i = 0; i < Count; ++i)
			{
				if (equalityComparer.Equals(_Array[i], Item))
					return true;
			}

			return false;
		}

		public int IndexOf(T Item)
		{
			int index = -1;
			for (int i = 0; i < Count; i++)
			{

				if (equalityComparer.Equals(_Array[i], Item))
					index = i;
			}
			return index;

		

		}

		public bool Remove(T ITem)
		{
			int index = IndexOf(ITem);
			if (index != -1)
				this.RemoveAt(index);
			return index != -1;
		}

		public void RemoveAt(int Index)
		{
			if (Index < 0 || Index >= Count)
				throw new ArgumentOutOfRangeException("index");
			--Count;
			_Array[Index] = default(T);
			for (int i = Index; i < Count; ++i)
				_Array[i] = _Array[i + 1];

			_Array[Count] = default(T);
		}

		public void RemoveRange(int Index, int Count)
		{
			if (Count <= 0)
				return;

			for (int i = Index; i <= Count; ++i)
			{
				if (Count <= 0)
					break;
				RemoveAt(i);
				i--;
				Count--;
			}
		}

		public void Clear()
		{
			this.Count = 0;
		}

		public void Reverse()
		{
			Array.Reverse(_Array, 0, Count);
		}

		public T[] ToArray()
		{
			T[] array = new T[Count];
			Array.Copy(_Array, array, Count);
			return array;
		}

		private void Allocate()
		{
			int DesireCount = requestedCount + Count;

			if (DesireCount <= Capacity)
				return;

			int max = Capacity << 1;
			if (32 > max)
				max = 32;
			if (DesireCount > max)
				max = DesireCount;

			T[] array = new T[Capacity = max];
			if (_Array != null && this.Count > 0)
				_Array.CopyTo(array, 0);
			_Array = array;
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _Array.Take(Count).GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}