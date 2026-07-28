using Soso.Utils.Concurrency;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Soso.Utils.Tests
{

	[TestFixture]
	public class SosoConcurrentSortedQueueTests
	{
		private SosoConcurrentSortedQueue<int, string> _queue;

		[SetUp]
		public void Setup()
		{
			// Re-initialize before every test. 
			// Using int for priority key, string for the value.
			_queue = new SosoConcurrentSortedQueue<int, string>();
		}

		// --- 1. Basic Sorting Behavior ---

		[Test]
		public void TryDequeueFirst_RetrievesItemsInAscendingKeyOrder()
		{
			_queue.Add(3, "High");
			_queue.Add(1, "Low");
			_queue.Add(2, "Medium");

			_queue.TryDequeueFirst(out string? first);
			_queue.TryDequeueFirst(out string? second);
			_queue.TryDequeueFirst(out string? third);

			Assert.That(first, Is.EqualTo("Low"));   // Key 1
			Assert.That(second, Is.EqualTo("Medium")); // Key 2
			Assert.That(third, Is.EqualTo("High"));  // Key 3
		}

		[Test]
		public void TryDequeueLast_RetrievesItemsInDescendingKeyOrder()
		{
			_queue.Add(3, "High");
			_queue.Add(1, "Low");
			_queue.Add(2, "Medium");

			_queue.TryDequeueLast(out string? first);
			_queue.TryDequeueLast(out string? second);
			_queue.TryDequeueLast(out string? third);

			Assert.That(first, Is.EqualTo("High"));   // Key 3
			Assert.That(second, Is.EqualTo("Medium")); // Key 2
			Assert.That(third, Is.EqualTo("Low"));  // Key 1
		}

		// --- 2. Duplicate Key Handling (Sequence Number Logic) ---

		[Test]
		public void TryDequeueFirst_WithDuplicateKeys_MaintainsFIFOOrder()
		{
			// All items have the exact same priority key (1)
			_queue.Add(1, "First-In");
			_queue.Add(1, "Second-In");
			_queue.Add(1, "Third-In");

			_queue.TryDequeueFirst(out string? first);
			_queue.TryDequeueFirst(out string? second);
			_queue.TryDequeueFirst(out string? third);

			Assert.That(first, Is.EqualTo("First-In"));
			Assert.That(second, Is.EqualTo("Second-In"));
			Assert.That(third, Is.EqualTo("Third-In"));
		}

		[Test]
		public void TryDequeueLast_WithDuplicateKeys_ActsAsLIFOForThatPriority()
		{
			// Because DequeueLast pulls the Max sequence number, 
			// it grabs the newest item of that priority level first.
			_queue.Add(1, "Oldest");
			_queue.Add(1, "Middle");
			_queue.Add(1, "Newest");

			_queue.TryDequeueLast(out string? first);
        
			Assert.That(first, Is.EqualTo("Newest"));
		}

		// --- 3. Edge Cases ---

		[Test]
		public void TryDequeue_OnEmptyQueue_ReturnsFalse()
		{
			bool successFirst = _queue.TryDequeueFirst(out string? itemFirst);
			bool successLast = _queue.TryDequeueLast(out string? itemLast);
        
			Assert.That(successFirst, Is.False);
			Assert.That(itemFirst, Is.Null);

			Assert.That(successLast, Is.False);
			Assert.That(itemLast, Is.Null);
		}

		[Test]
		public void Clear_RemovesAllItemsAndResetsCount()
		{
			_queue.Add(1, "A");
			_queue.Add(2, "B");
        
			Assert.That(_queue.Count, Is.EqualTo(2));
        
			_queue.Clear();
        
			Assert.That(_queue.Count, Is.EqualTo(0));
			Assert.That(_queue.TryDequeueFirst(out _), Is.False);
		}

		// --- 4. Concurrency Stress Tests ---

		[Test]
		public void Concurrent_Add_DoesNotLoseItems()
		{
			int totalItems = 10000;
        
			Parallel.For(0, totalItems, i => 
			{
				// Randomly assign a few different priority buckets
				int priority = i % 5; 
				_queue.Add(priority, $"Item {i}");
			});

			Assert.That(_queue.Count, Is.EqualTo(totalItems));
		}

		[Test]
		public void Concurrent_AddAndDequeue_MaintainsIntegrity()
		{
			int totalItems = 10000;
			var dequeuedItems = new ConcurrentBag<string>();

			// Start a consumer task
			var consumer = Task.Run(() => 
			{
				int attempts = 0;
				// Loop until we get all items (with a safety breakout)
				while (dequeuedItems.Count < totalItems && attempts < totalItems * 10)
				{
					if (_queue.TryDequeueFirst(out string? item))
					{
						dequeuedItems.Add(item!);
					}
					attempts++;
				}
			});

			// Start a producer task
			var producer = Task.Run(() => 
			{
				Parallel.For(0, totalItems, i => 
				{
					_queue.Add(i % 10, $"Item {i}");
				});
			});

			Task.WaitAll(producer, consumer);

			Assert.That(dequeuedItems.Count, Is.EqualTo(totalItems));
			Assert.That(_queue.Count, Is.EqualTo(0));
		}
	}
}
