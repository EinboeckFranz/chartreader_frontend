namespace GraphToCsvFrontend.generators;

public class DirectoryGenerator: Generator
{
    private string? _directoryInputPath;
    
    public override string? GetInputPath()
    {
        var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog
        {
            Multiselect = false,
            Description = "Select Folder",
            UseDescriptionForTitle = true,
        };
        if (!dialog.ShowDialog() ?? false)
            return null;
        var selectedPath = dialog.SelectedPath;
        
        _directoryInputPath = selectedPath;
        return selectedPath;
    }
    
    public override void GenerateCsv(string outputDirectory)
    {
        if (_directoryInputPath == null || !Directory.Exists(outputDirectory))
            return;
        
        var images = Directory.GetFiles(_directoryInputPath).Where(x => x.EndsWith(".png"));
        foreach (var (idx, imagePath) in images.Select((value, i) => (i, value)))
        {
            var outputPath = Path.Combine(outputDirectory, Path.GetFileName(imagePath.Replace(".png", ".csv")));
            try
            {
                CallEngine(imagePath, outputPath);
            }
            catch (ConvertToGraphException)
            {
                System.Windows.MessageBox.Show($"Could not convert this graph ({imagePath}) to an csv File!", "ChartReader", MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }
        OpenFileExplorer(outputDirectory);
    }
}