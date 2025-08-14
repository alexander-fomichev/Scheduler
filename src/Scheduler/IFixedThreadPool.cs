using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Scheduler
{
    /// <summary>
    /// Интерфейс пула потоков с фиксированным количеством потоков
    /// </summary>
    public interface IFixedThreadPool: IDisposable
    {
        /// <summary>
        /// Добавляет задачу в очередь на выполнение с указанным приоритетом
        /// </summary>
        /// <param name="task">Задача для выполнения</param>
        /// <param name="priority">Приоритет выполнения</param>
        /// <returns>true - задача принята, false - пул остановлен</returns>
        bool Execute(Task task, Priority priority);
        /// <summary>
        /// Останавливает пул потоков. После вызова новые задачи не принимаются.
        /// Дожидается завершения всех текущих задач.
        /// </summary>
        void Stop();
    }
}
