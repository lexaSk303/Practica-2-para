using System.Diagnostics;
using System.ComponentModel;

public class Program
{
    public static async Task Main()
    {
        // Используем dotnet --info как безопасную утилиту.
        ProcessStartInfo startInfo = new()
        {
            FileName = "dotnet",
            UseShellExecute = false,
            CreateNoWindow = true
        };

        // TODO 1: Добавляем аргумент "--info" через ArgumentList
        startInfo.ArgumentList.Add("--info");

        // TODO 2: Указываем рабочую директорию
        startInfo.WorkingDirectory = Environment.CurrentDirectory;

        using Process process = new()
        {
            StartInfo = startInfo
        };

        Stopwatch stopwatch = new();

        try
        {
            // TODO 3: Запускаем секундомер
            stopwatch.Start();

            // TODO 4: Запускаем процесс
            process.Start();

            // TODO 5: Выводим PID процесса
            Console.WriteLine($"PID: {process.Id}");

            // TODO 6: Дожидаемся завершения асинхронно 
            await process.WaitForExitAsync();

            // TODO 7: Останавливаем секундомер
            stopwatch.Stop();

            // TODO 8: Выводим длительность и ExitCode
            Console.WriteLine($"Длительность: {stopwatch.Elapsed}");
            Console.WriteLine($"Exit code: {process.ExitCode}");
        }
        catch (Win32Exception ex)
        {
            // TODO 9: Сообщаем об ошибке запуска 
            Console.WriteLine($"Ошибка запуска процесса: {ex.Message}");
        }
    }
}