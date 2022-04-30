namespace GraphToCsvFrontend.generators;

public abstract class Generator
{
    public abstract string? GetInputPath();
    public abstract void GenerateCsv(string outputDirectory, BusyIndicator indicator);

    protected static void CallEngine(string inputPath, string outputPath, int fileIndex, BusyIndicator indicator)
    {
        indicator.BusyContent = $"Processing File {fileIndex}...";
        var process = new Process
        {
            StartInfo =
            {
                FileName = "cmd.exe",
                WorkingDirectory = $@"{Path.Combine(Environment.CurrentDirectory, "generatorEngine")}",
                Arguments = $"/C chartreader.exe {inputPath} {outputPath}"    
            }    
        };
        process.Start();
        var log = process.StandardOutput.ReadToEnd();
        process.WaitForExit();
        if (!log.Contains($"CSV-File saved to {outputPath}."))
            throw new ConvertToGraphException("Error occurred while generating csv-File");
        Console.WriteLine(log);
    }

    protected static void OpenFileExplorer(string directory)
    {
        Process.Start(new ProcessStartInfo
        {
            Arguments = directory,
            FileName = "explorer.exe"
        });
    }
}