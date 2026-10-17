using System.Diagnostics;

static async Task<(int ExitCode, string Output, string Error)> RunBashAsync(string command)
{
    var psi = new ProcessStartInfo
    {
        FileName = "/bin/bash",
        RedirectStandardOutput = true,
        RedirectStandardError = true,
        UseShellExecute = false,
        CreateNoWindow = true
    };
    psi.ArgumentList.Add("-c");
    psi.ArgumentList.Add(command);

    using var process = new Process { StartInfo = psi };
    process.Start();

    var outputTask = process.StandardOutput.ReadToEndAsync();
    var errorTask = process.StandardError.ReadToEndAsync();
    await process.WaitForExitAsync();

    return (process.ExitCode, await outputTask, await errorTask);
}

var (code, output, error) = await RunBashAsync("ls -la /home");
Console.WriteLine($"Код выхода: {code}");
Console.WriteLine(output);
if (code != 0) Console.WriteLine($"Ошибка: {error}");