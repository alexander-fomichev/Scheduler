using Scheduler;

public class FixedThreadPoolTests
{
    [Fact]
    public void Execute_ReturnsFalse_AfterStop()
    {
        var pool = new FixedThreadPool(1);
        pool.Stop();
        Assert.False(pool.Execute(new Scheduler.Task(() => {}), Priority.NORMAL));
    }

    [Fact]
    public void ExecutesAllTasks_BeforeStop()
    {
        var pool = new FixedThreadPool(1);
        var counter = 0;
        
        pool.Execute(new Scheduler.Task(() => Interlocked.Increment(ref counter)), Priority.NORMAL);
        pool.Execute(new Scheduler.Task(() => Interlocked.Increment(ref counter)), Priority.NORMAL);
        
        pool.Stop();
        Assert.Equal(2, counter);
    }

    [Fact]
    public void HandlesTaskExceptions_Gracefully()
    {
        var pool = new FixedThreadPool(1);
        var normalTaskExecuted = false;
        
        pool.Execute(new Scheduler.Task(() => throw new Exception()), Priority.HIGH);
        pool.Execute(new Scheduler.Task(() => normalTaskExecuted = true), Priority.NORMAL);
        
        Thread.Sleep(100);
        pool.Stop();
        
        Assert.True(normalTaskExecuted);
    }

    [Fact]
    public void ProcessesTasksConcurrently()
    {
        var pool = new FixedThreadPool(2);
        var startSignal = new ManualResetEvent(false);
        var tasksCompleted = 0;

        // Две задачи, которые будут ждать сигнала
        pool.Execute(new Scheduler.Task(() => {
            startSignal.WaitOne();
            Interlocked.Increment(ref tasksCompleted);
        }), Priority.NORMAL);

        pool.Execute(new Scheduler.Task(() => {
            startSignal.WaitOne();
            Interlocked.Increment(ref tasksCompleted);
        }), Priority.NORMAL);

        // Даем время на старт задач
        Thread.Sleep(50);
        startSignal.Set();
        
        Thread.Sleep(50);
        pool.Stop();
        
        Assert.Equal(2, tasksCompleted);
    }

    [Fact]
    public void Should_Dispose_Correctly_Without_Exceptions()
    {
        // Arrange
        var pool = new FixedThreadPool(2);

        // Act + Assert (проверяем, что Dispose не выбрасывает исключений)
        var exception = Record.Exception(() =>
        {
            pool.Execute(new Scheduler.Task(() => Thread.Sleep(100)), Priority.HIGH);
            pool.Dispose(); // Явный вызов Dispose
        });
        Assert.Null(exception); // нет исключений
    }

    [Fact]
    public void Should_Not_Accept_Tasks_After_Dispose()
    {
        // Arrange
        var pool = new FixedThreadPool(2);
        pool.Dispose();

        // Act
        var result = pool.Execute(new Scheduler.Task(() => { }), Priority.NORMAL);

        // Assert
        Assert.False(result); // задачи не принимаются
    }
}