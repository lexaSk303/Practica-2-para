using System.Diagnostics;

public record Order(int Id, decimal Amount);

public class Program
{
    private static void ProcessOrder(Order order)
    {
        Console.WriteLine(
            $"Заказ {order.Id}: поток " +
            $"{Environment.CurrentManagedThreadId}"
        );
        Thread.Sleep(100);
        decimal total = order.Amount * 1.05m;
        Console.WriteLine(
            $"Заказ {order.Id} завершён. Сумма: {total:F2}"
        );
    }

    public static void Main()
    {
        Order[] orders =
        {
            new(1, 100),
            new(2, 200),
            new(3, 300),
            new(4, 400),
            new(5, 500),
            new(6, 600),
            new(7, 700),
            new(8, 800)
        };

        // TODO 1: Измерение времени последовательной обработки
        Console.WriteLine("=== Последовательная обработка ===");
        Stopwatch seqStopwatch = Stopwatch.StartNew();

        foreach (var order in orders)
        {
            ProcessOrder(order);
        }

        seqStopwatch.Stop();
        TimeSpan seqTime = seqStopwatch.Elapsed;
        Console.WriteLine($"Время последовательной обработки: {seqTime}\n");

        // TODO 2: Создание массива потоков
        Thread[] threads = new Thread[orders.Length];

        Console.WriteLine("=== Многопоточная обработка ===");
        Stopwatch parStopwatch = Stopwatch.StartNew();

        // TODO 3 и 4: Создание и запуск потоков
        for (int i = 0; i < orders.Length; i++)
        {
            Order currentOrder = orders[i];

            threads[i] = new Thread(() => ProcessOrder(currentOrder));
            threads[i].Start();
        }
        // TODO 5: Ожидание завершения всех потоков (Join)
        foreach (var thread in threads)
        {
            thread.Join(); 
        }

        parStopwatch.Stop();
        TimeSpan parTime = parStopwatch.Elapsed;
        // TODO 6: Вывод итогов сравнения
        Console.WriteLine("\n=== Итоги ===");
        Console.WriteLine($"Последовательное время: {seqTime}");
        Console.WriteLine($"Многопоточное время:    {parTime}");

        if (parTime < seqTime)
        {
            Console.WriteLine("Вывод: Многопоточная обработка оказалась быстрее (задержки перекрылись).");
        }
        else
        {
            Console.WriteLine("Вывод: Многопоточная обработка не дала выигрыша (возможны накладные расходы или нехватка ядер).");
        }
    }
}