# 🚀 Scheduler - Thread Pool with Priority Support

![.NET](https://img.shields.io/badge/.NET-5C2D91?logo=.net)

Реализация пула потоков с приоритетной очередью задач на C#. Поддерживает три уровня приоритета: `HIGH`, `NORMAL`, `LOW`.

---

## 📦 Структура решения

```
Scheduler/
├── src/
│   └── Scheduler/               # Основной проект (библиотека)
│       ├── PriorityQueue.cs     # Очередь с приоритетами
│       ├── FixedThreadPool.cs   # Пул потоков
│       ├── ITask.cs             # Интерфейс задачи
│       └── ...
├── tests/
│   └── Scheduler.Tests/         # Юнит-тесты (xUnit)
│       ├── PriorityQueueTests.cs
│       └── ...
└── README.md                    # Этот файл
```

---

## ⚡ Быстрый старт

1. **Клонирование репозитория**:
   ```bash
   git clone https://github.com/yourname/Scheduler.git
   cd Scheduler
   ```

2. **Сборка**:
   ```bash
   dotnet build
   ```

3. **Тестирование**:
   ```bash
   dotnet test
   ```

---

## 🛠️ Пример использования

```csharp
using Scheduler;

// Создаем пул с 4 потоками
using var pool = new FixedThreadPool(4);

// Добавляем задачи
pool.Execute(new Task(() => Console.WriteLine("High")), Priority.HIGH);
pool.Execute(new Task(() => Console.WriteLine("Normal")), Priority.NORMAL);

// Останавливаем пул
pool.Stop();
```

---

## 🔍 Особенности

✅ **Приоритеты**: `HIGH` > `NORMAL` > `LOW`\
✅ **Соотношение**: 3 задачи `HIGH` = 1 задача `NORMAL`\
✅ **Потокобезопасность**: `lock` + `ConcurrentQueue`\
✅ **Обработка ошибок**: Исключения в задачах логируются

---

> Проект создан для демонстрации работы с многопоточностью в .NET.  
> Для production-использования требуется доработка!
