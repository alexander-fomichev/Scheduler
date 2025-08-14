using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    /// <summary>
    /// Приоритет выполнения задачи в FixedThreadPool
    /// </summary>
    public enum Priority
    {
        /// <summary>Низкий приоритет - выполняются последними</summary>
        LOW = 0,
        /// <summary>Обычный приоритет - выполняются после HIGH задач</summary>
        NORMAL = 1,
        /// <summary>Высокий приоритет - выполняются в первую очередь</summary>
        HIGH = 2
    }
}
