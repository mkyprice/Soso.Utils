using Soso.Utils.Concurrency;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soso.Utils.Tests
{
	[TestFixture]
	public class SosoConcurrentQueueTests
	{
		private SosoConcurrentQueue<int> _queue;

		[SetUp]
		public void Setup()
		{
			_queue = new SosoConcurrentQueue<int>();
		}

		[Test]
		public void EnqueueBack_And_TryDequeue_WorksCorrectly()
		{
			_queue.EnqueueBack(10);
			_queue.EnqueueBack(20);

			Assert.That(_queue.Count, Is.EqualTo(2));

			bool success1 = _queue.TryDequeue(out int item1);
			Assert.That(success1, Is.True);
			Assert.That(item1, Is.EqualTo(10));

			bool success2 = _queue.TryDequeue(out int item2);
			Assert.That(success2, Is.True);
			Assert.That(item2, Is.EqualTo(20));

			Assert.That(_queue.Count, Is.EqualTo(0));
		}

		[Test]
		public void EnqueueFront_PrependsItemsCorrectly()
		{
			_queue.EnqueueBack(2);
			_queue.EnqueueFront(1); // 1 should jump to the front
        
			_queue.TryDequeue(out int first);
			_queue.TryDequeue(out int second);

			Assert.That(first, Is.EqualTo(1));
			Assert.That(second, Is.EqualTo(2));
		}

		[Test]
		public void TryDequeue_OnEmptyQueue_ReturnsFalse()
		{
			bool success = _queue.TryDequeue(out int item);
        
			Assert.That(success, Is.False);
			Assert.That(item, Is.EqualTo(0));
		}

		// --- 2. Cancellation Behavior ---

		[Test]
		public void Enqueue_WithCanceledToken_ThrowsOperationCanceledException()
		{
			var cts = new CancellationTokenSource();
			cts.Cancel(); // Cancel immediately

			Assert.Throws<OperationCanceledException>(() => 
				_queue.EnqueueBack(1, cts.Token));
        
			Assert.That(_queue.Count, Is.EqualTo(0), "State should not be mutated if canceled");
		}

		// --- 3. Concurrency Stress Tests ---

		[Test]
		public void Concurrent_Enqueue_DoesNotLoseItems()
		{
			int totalItems = 10000;
        
			// Blast the queue with multiple threads simultaneously
			Parallel.For(0, totalItems, i => 
			{
				if (i % 2 == 0)
					_queue.EnqueueBack(i);
				else
					_queue.EnqueueFront(i);
			});

			// If the lock wasn't working, Count would be lower due to race conditions
			Assert.That(_queue.Count, Is.EqualTo(totalItems));
		}

		[Test]
		public void Concurrent_EnqueueAndDequeue_MaintainsIntegrity()
		{
			int totalItems = 10000;
			int successfullyDequeued = 0;

			// Start a consumer task that constantly polls for items
			var consumer = Task.Run(() => 
			{
				int dequeuedCount = 0;
				while (dequeuedCount < totalItems)
				{
					if (_queue.TryDequeue(out _))
					{
						dequeuedCount++;
					}
				}
				successfullyDequeued = dequeuedCount;
			});

			// Start a producer task that blasts items into the queue
			var producer = Task.Run(() => 
			{
				Parallel.For(0, totalItems, i => 
				{
					_queue.EnqueueBack(i);
				});
			});

			// Wait for both to finish processing
			Task.WaitAll(producer, consumer);

			Assert.That(successfullyDequeued, Is.EqualTo(totalItems));
			Assert.That(_queue.Count, Is.EqualTo(0));
		}
	}
}
