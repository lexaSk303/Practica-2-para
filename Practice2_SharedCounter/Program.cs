using System;

public class Program
{
    private static int completedOrders = 0;

    public static void Main()
    {
        const int threadCount = 8;
        const int incrementsPerThread = 1_000_000;

        Console.WriteLine("=== ТЕСТ 1: Обычный инкремент ===");
        RunTest(threadCount, incrementsPerThread, useInterlocked: false);

        Console.WriteLine("\n=== ТЕСТ 2: Атомарный инкремент ===");
        RunTest(threadCount, incrementsPerThread, useInterlocked: true);
    }

    private static void RunTest(int threadCount, int incrementsPerThread, bool useInterlocked)
    {
        completedOrders = 0;
        Thread[] threads = new Thread[threadCount];

        for (int i = 0; i < threadCount; i++)
        {
            threads[i] = new Thread(() =>
            {
                for (int j = 0; j < incrementsPerThread; j++)
                {
                    if (useInterlocked)
                    {
                        Interlocked.Increment(ref completedOrders);
                    }
                    else
                    {
                        completedOrders++;
                    }
                }
            });
        }
        foreach (Thread thread in threads)
        {
            thread.Start();
        }
        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        int expected = threadCount * incrementsPerThread;
        int actual = completedOrders;
        int lost = expected - actual;

        Console.WriteLine($"Ожидалось: {expected}");
        Console.WriteLine($"Получено:  {actual}");

        if (lost == 0)
        {
            Console.WriteLine("Результат: потерь нет");
        }
        else
        {
            Console.WriteLine($"Результат: ошибка! Потеряно {lost} инкрементов из-за гонки данных.");
        }
    }
}