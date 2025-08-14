using Scheduler;
public class PriorityQueueTests
{
    [Fact]
    public void TryDequeue_ReturnsFalse_ForEmptyQueue()
    {
        var queue = new PriorityQueue<string>();
        Assert.False(queue.TryDequeue(out _));
    }

    [Fact]
    public void Dequeues_HighPriorityFirst()
    {
        var queue = new PriorityQueue<string>();
        queue.Enqueue("LOW", Priority.LOW);
        queue.Enqueue("NORMAL", Priority.NORMAL);
        queue.Enqueue("HIGH", Priority.HIGH);

        Assert.True(queue.TryDequeue(out var item));
        Assert.Equal("HIGH", item);
    }

    [Fact]
    public void Maintains_3HighTo1NormalRatio()
    {
        var queue = new PriorityQueue<string>();
        // Добавляем 4 HIGH и 1 NORMAL
        queue.Enqueue("H1", Priority.HIGH);
        queue.Enqueue("H2", Priority.HIGH);
        queue.Enqueue("H3", Priority.HIGH);
        queue.Enqueue("H4", Priority.HIGH);
        queue.Enqueue("N1", Priority.NORMAL);

        var results = new List<string>();
        while (queue.TryDequeue(out var item))
            results.Add(item);

        Assert.Equal(new[] { "H1", "H2", "H3", "N1", "H4" }, results);
    }

    [Fact]
    public void SkipsLowPriority_WhenHigherExists()
    {
        var queue = new PriorityQueue<string>();
        queue.Enqueue("LOW", Priority.LOW);
        queue.Enqueue("NORMAL", Priority.NORMAL);

        Assert.True(queue.TryDequeue(out var item));
        Assert.Equal("NORMAL", item);
    }
}