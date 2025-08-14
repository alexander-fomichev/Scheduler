using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    /// <summary>
    /// Интерфейс задачи для выполнения в FixedThreadPool
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Выполнение задачи. Будет вызвано в одном из рабочих потоков.
        /// </summary>
        void Execute();
    }
}
