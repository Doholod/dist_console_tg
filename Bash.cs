using System.Diagnostics;

// IN: команда для BASH
// OUTPUT: ExitCode - код ошибки
// Output - строка с выходными значениями
// Error - строка с ошибкой

class Bash
{
    static public async Task<(int ExitCode, string Output, string Error)> RunBashAsync(string command)
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
}