using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Scheduler
{
    /// <summary>
    /// Реализация задачи для выполнения в FixedThreadPool
    /// </summary>
    public class Task : ITask
    {
        private Action action;

        /// <summary>
        /// Создает новую задачу
        /// </summary>
        /// <param name="action">Действие для выполнения</param>
        public Task(Action action)
        {
            this.action = action;
        }

        /// <summary>
        /// Выполняет задачу. Обрабатывает и логирует исключения.
        /// </summary>
        public void Execute()
        {
            try
            {
                action?.Invoke();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Task failed: {ex.Message}");
            }
        }

    }
}
