using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Scheduler
{
    /// <summary>
    /// Реализация пула потоков с фиксированным количеством потоков
    /// и поддержкой приоритетов выполнения задач
    /// </summary>
    public class FixedThreadPool: IFixedThreadPool
    {
        // Флаг остановки пула
        private volatile bool _stopped = false;
        private bool isDisposed;

        // Рабочие потоки
        private readonly Thread[] _threads;

        // Очередь задач с приоритетами
        private readonly PriorityQueue<Task> _taskQueue = new PriorityQueue<Task>();

        /// <summary>
        /// Создает пул с указанным количеством потоков
        /// </summary>
        /// <param name="threadCount">Количество потоков (должно быть > 0)</param>
        /// <exception cref="ArgumentException">Если threadCount <= 0</exception>
        public FixedThreadPool(int threadCount)
        {
            if (threadCount <= 0)
                throw new ArgumentException("Thread count must be positive", nameof(threadCount));
            // Создаем и запускаем рабочие потоки
            _threads = new Thread[threadCount];
            for (int i = 0; i < _threads.Length; i++)
            {
                _threads[i] = new Thread(DoWork)
                {
                    Name = $"Thread#{i}",
                    IsBackground = true // Потоки завершится при закрытии приложения
                };
                _threads[i].Start();
            }
        }
        /// <summary>
        /// Метод, в котором будут выполняться задачи
        /// </summary>
        private void DoWork() {
            while (true)
            {
                // Пытаемся получить задачу из очереди
                if (_taskQueue.TryDequeue(out Task task))
                {
                    task.Execute(); // Выполняем задачу
                }
                else if (!_stopped)
                {
                    // Если пул не остановлен и очередь пуста -небольшая пауза
                    Thread.Sleep(10);
                }
                // Если пул остановлен - завершаем поток
                else return;
            }  
        }

        /// <summary>
        /// Останавливает пул потоков. Новые задачи не принимаются.
        /// Дожидается завершения всех текущих задач.
        /// </summary>
        public void Stop() 
        {
            // Запрещаем добавление новых задач
            _stopped = true;

            foreach (var thread in _threads)
            {
                thread.Join(); // Ожидаем завершения каждого потока
            }
        }

        /// <summary>
        /// Добавляет задачу в очередь на выполнение
        /// </summary>
        /// <param name="task">Задача для выполнения</param>
        /// <param name="priority">Приоритет выполнения</param>
        /// <returns>true - задача принята, false - пул остановлен</returns>
        public bool Execute(Task task, Priority priority)
        {
            // Если пул не остановлен, то ставим задачу в очередь
            if (!_stopped)
            {
                _taskQueue.Enqueue(task, priority);
                return true;
            }
            return false;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!isDisposed)
            {
                if (disposing)
                {
                    this.Stop();
                }
                isDisposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
