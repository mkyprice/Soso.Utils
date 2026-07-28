using System;
using System.Collections.Generic;
using System.Threading;

namespace Soso.Utils.Concurrency
{
	public class SosoConcurrentSortedQueue<TKey, TValue>
		where TKey : IComparable<TKey>
	{
		private readonly struct SortedNode
		{
			public readonly TKey Key;
			public readonly TValue Value;
			public readonly ulong SequenceNumber;
			
			public SortedNode(TKey key, TValue value, ulong sequenceNumber)
			{
				Key = key;
				Value = value;
				SequenceNumber = sequenceNumber;
			}
		}
		
		private class SortedNodeComparer : IComparer<SortedNode>
		{
			private readonly IComparer<TKey> _comparer;
			public SortedNodeComparer(IComparer<TKey> comparer)
			{
				_comparer = comparer;
			}
			public int Compare(SortedNode x, SortedNode y)
			{
				int keyComparison = _comparer.Compare(x.Key, y.Key);
				if (keyComparison != 0) return keyComparison;
				return x.SequenceNumber.CompareTo(y.SequenceNumber);
			}
		}

		private SortedSet<SortedNode> _values;
		private ulong _sequenceNumber;
		private readonly object _lock = new object();

		public int Count
		{
			get
			{
				lock (_lock) return _values.Count;
			}
		}

		public SosoConcurrentSortedQueue() : this(Comparer<TKey>.Default)
		{
		}

		public SosoConcurrentSortedQueue(IComparer<TKey> comparer)
		{
			_values = new SortedSet<SortedNode>(new SortedNodeComparer(comparer));
		}

		public void Add(TKey key, TValue item, CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();
			
			lock (_lock)
			{
				_sequenceNumber++;
				var node = new SortedNode(key, item, _sequenceNumber);
				_values.Add(node);
			}
		}

		public bool TryPeekFirst(out TValue? item, CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();

			lock (_lock)
			{
				if (_values.Count == 0)
				{
					item = default;
					return false;
				}
				var node = _values.Min;
				item = node.Value;
				return true;
			}
		}

		public bool TryPeekLast(out TValue? item, CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();

			lock (_lock)
			{
				if (_values.Count == 0)
				{
					item = default;
					return false;
				}
				var node = _values.Max;
				item = node.Value;
				return true;
			}
		}

		public bool TryDequeueFirst(out TValue? item, CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();

			lock (_lock)
			{
				if (_values.Count == 0)
				{
					item = default;
					return false;
				}
				var node = _values.Min;
				_values.Remove(node);
				item = node.Value;
				return true;
			}
		}

		public bool TryDequeueLast(out TValue? item, CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();

			lock (_lock)
			{
				if (_values.Count == 0)
				{
					item = default;
					return false;
				}
				var node = _values.Max;
				_values.Remove(node);
				item = node.Value;
				return true;
			}
		}

		public void Clear(CancellationToken cancellationToken = default)
		{
			cancellationToken.ThrowIfCancellationRequested();

			lock (_lock)
			{
				_values.Clear();
				_sequenceNumber = 0;
			}
		}
	}
}
