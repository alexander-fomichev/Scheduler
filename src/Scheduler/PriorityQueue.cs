using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Scheduler
{
    /// <summary>
    /// Очередь с приоритетами.
    /// Реализует алгоритм: выдает 3 элемента с HIGH приоритетом на 1 элемент.
    /// LOW элементы выдаются только когда нет HIGH/NORMAL задач.
    /// </summary>
     internal class PriorityQueue<T>
    {
        // Очереди элементов для каждого приоритета
        private readonly ConcurrentDictionary<Priority, ConcurrentQueue<T>> _priorityQueue;

        // Счетчик выданных HIGH элементов
        private int highTaskCounter = 0;

        // Объект для синхронизации доступа к очереди
        private readonly object locker = new object();

        public PriorityQueue()
        {
            _priorityQueue = new ConcurrentDictionary<Priority, ConcurrentQueue<T>>();
            // Инициализируем очереди для всех приоритетов
            _priorityQueue[Priority.LOW] = new ConcurrentQueue<T>();
            _priorityQueue[Priority.NORMAL] = new ConcurrentQueue<T>();
            _priorityQueue[Priority.HIGH] = new ConcurrentQueue<T>();
        }
        /// <summary>
        /// Добавляет элемент в очередь согласно приоритету
        /// </summary>
        /// <param name="element">Задача</param>
        /// <param name="priority">Приоритет</param>
        public void Enqueue(T element, Priority priority)
        {
            _priorityQueue[priority].Enqueue(element);
        }

        /// <summary>
        /// Извлекает задачу из очереди согласно алгоритму приоритетов
        /// </summary>
        /// <param name="element">Извлеченная задача</param>
        /// <returns>true - если задача извлечена, false - если очередь пуста</returns>
        public bool TryDequeue(out T element)
        {
            lock (locker)
            {
                // 1. Проверяем HIGH задачи (если не превышен лимит 3:1)
                if (highTaskCounter < 3 && _priorityQueue[Priority.HIGH].TryDequeue(out element))
                {
                    highTaskCounter++;
                    return true;
                }
                // 2. Проверяем NORMAL задачи
                if (_priorityQueue[Priority.NORMAL].TryDequeue(out element))
                {
                    highTaskCounter = 0;
                    return true;
                }
                // 3. Проверяем LOW задачи (только если нет HIGH/NORMAL)
                if (_priorityQueue[Priority.LOW].TryDequeue(out element))
                    return true;
                // Очередь пуста
                return false;
            }
        }

    }
}
