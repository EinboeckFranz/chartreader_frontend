using System.Collections.Generic;

namespace GraphToCsvFrontend.generators;

public abstract class Generator
{
    public abstract string? GetInputPath();
    public abstract void GenerateCsv(string outputDirectory);

    protected static void CallEngine(string inputPath, string outputPath)
    {
        var path = Path.Combine(Environment.CurrentDirectory, "generatorEngine", "__main__.py");
        var temp = Process.Start("cmd.exe", new List<string>
        {
            $"/C python {path} {inputPath} {outputPath}"
        });
        var log = temp.StandardOutput.ReadToEnd();
        temp.WaitForExit();
        Console.WriteLine(log);
        if (log != null && !log.Contains($"CSV-File saved to {outputPath}."))
            throw new ConvertToGraphException(log);
    }

    protected static void OpenFileExplorer(string directory)
    {
        new Process {
            StartInfo = new ProcessStartInfo
            {
                Arguments = directory,
                FileName = "explorer.exe"
            }
        }.Start();
    }
}