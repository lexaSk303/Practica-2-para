using System.Diagnostics;
using System.ComponentModel;

public class Program
{
    public static async Task Main()
    {
        ProcessStartInfo startInfo = new()
        {
            FileName = "dotnet",
            UseShellExecute = false,
            CreateNoWindow = true,

            // TODO 1. Включаем перенаправление стандартного вывода (stdout)
            RedirectStandardOutput = true,

            // TODO 2. Включаем перенаправление стандартного потока ошибок (stderr)
            RedirectStandardError = true
        };

        // startInfo.ArgumentList.Add("--info");
        startInfo.FileName = "cmd.exe";
        startInfo.ArgumentList.Clear();
        startInfo.ArgumentList.Add("/c");
        startInfo.ArgumentList.Add("echo Сообщение в stdout & echo Ошибка в stderr 1>&2");

        using Process process = new()
        {
            StartInfo = startInfo
        };

        Stopwatch stopwatch = Stopwatch.StartNew();

        try
        {
            // TODO 3. Запускаем процесс
            process.Start();

            Console.WriteLine($"PID: {process.Id}");

            // TODO 4 и 5. Начинаем асинхронное чтение обоих потоков сразу.
            Task<string> stdOutTask = process.StandardOutput.ReadToEndAsync();
            Task<string> stdErrTask = process.StandardError.ReadToEndAsync();

            // TODO 6. Дожидаемся завершения процесса
            await process.WaitForExitAsync();

            // TODO 7. Получаем результаты чтения 
            string stdOut = await stdOutTask;
            string stdErr = await stdErrTask;

            // TODO 8. Останавливаем секундомер
            stopwatch.Stop();

            // TODO 9. Выводим длительность и ExitCode
            Console.WriteLine($"\nДлительность: {stopwatch.Elapsed}");
            Console.WriteLine($"Exit code: {process.ExitCode}");

            // TODO 10. Выводим stdout и stderr отдельно
            Console.WriteLine("\n=== STDOUT ===");
            Console.WriteLine(stdOut);

            Console.WriteLine("=== STDERR ===");
            Console.WriteLine(stdErr);
        }
        catch (Win32Exception ex)
        {
            stopwatch.Stop();
            Console.WriteLine($"Ошибка запуска: {ex.Message}");
        }
    }
}