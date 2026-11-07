using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Soso.Utils.Concurrency
{
	public class SosoConcurrentQueue<T>
	{
		private LinkedList<T> _values;
		private int _lock = 0;
		
		public int Count
		{
			get
			{
				AcquireLock();
				try
				{
					return _values.Count;
				}
				finally
				{
					ReleaseLock();
				}
			}
		}

		public SosoConcurrentQueue()
		{
			_values = new LinkedList<T>();
		}

		public void EnqueueFront(T item, CancellationToken cancellationToken = default)
		{
			AcquireLock(cancellationToken);
			try
			{
				_values.AddFirst(item);
			}
			finally
			{
				ReleaseLock();
			}
		}

		public void EnqueueBack(T item, CancellationToken cancellationToken = default)
		{
			AcquireLock(cancellationToken);
			try
			{
				_values.AddLast(item);
			}
			finally
			{
				ReleaseLock();
			}
		}

		public bool TryPeek(out T? item, CancellationToken cancellationToken = default)
		{
			AcquireLock(cancellationToken);
			try
			{
				if (_values.Count == 0)
				{
					item = default;
					return false;
				}
				item = _values.First.Value;
				return true;
			}
			finally
			{
				ReleaseLock();
			}
		}

		public bool TryDequeue(out T? item, CancellationToken cancellationToken = default)
		{
			AcquireLock(cancellationToken);
			try
			{
				if (_values.Count == 0)
				{
					item = default;
					return false;
				}
				item = _values.First.Value;
				_values.RemoveFirst();
				return true;
			}
			finally
			{
				ReleaseLock();
			}
		}

		public void Clear(CancellationToken cancellationToken = default)
		{
			AcquireLock(cancellationToken);
			try
			{
				_values.Clear();
			}
			finally
			{
				ReleaseLock();
			}
		}

		public T[] ToArray(CancellationToken cancellationToken = default)
		{
			AcquireLock(cancellationToken);
			try
			{
				var array = new T[_values.Count];
				_values.CopyTo(array, 0);
				return array;
			}
			finally
			{
				ReleaseLock();
			}
		}
		
		private void AcquireLock(CancellationToken cancellationToken = default)
		{
			// If cancellation is requested, we should immediately throw
			cancellationToken.ThrowIfCancellationRequested();
			
			SpinWait wait = new SpinWait();
			while (TryAcquireLock() == false)
			{
				cancellationToken.ThrowIfCancellationRequested();
				
				wait.SpinOnce();
			}
		}

		private bool TryAcquireLock()
		{
			return Interlocked.Exchange(ref _lock, 1) == 0;
		}

		private void ReleaseLock()
		{
			Interlocked.Exchange(ref _lock, 0);
		}
	}
}
