using System.Collections.Generic;

namespace GraphToCsvFrontend.generators;

public abstract class Generator
{
    public abstract string? GetInputPath();
    public abstract void GenerateCsv(string outputDirectory);

    protected static void CallEngine(string inputPath, string outputPath)
    {
        var path = Path.Combine(Environment.CurrentDirectory, "generatorEngine", "__main__.py");
        var processInfo = new ProcessStartInfo
        {
            Arguments = $"/C python {path} {inputPath} {outputPath}",
            FileName = "cmd.exe",
            RedirectStandardOutput = true
        };
        
        var temp = Process.Start(processInfo);
        var log = temp?.StandardOutput.ReadToEnd();
        temp?.WaitForExit();
        if (log != null && !log.Contains($"CSV-File saved "))
            throw new ConvertToGraphException(log);
    }

    protected static void OpenFileExplorer(string directory) => Process.Start("explorer.exe", directory);
}