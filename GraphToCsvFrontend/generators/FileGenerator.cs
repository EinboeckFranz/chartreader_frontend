namespace GraphToCsvFrontend.generators;

public class FileGenerator: Generator
{
    private string? _fileInputPath;

    public override string? GetInputPath()
    {
        var fileDialog = new OpenFileDialog
        {
            Title = "Select an Graph Image",
            Filter = "Images (*.png)|*.png|All files (*.*)|*.*",
            InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures),
            Multiselect = false
        };
        if (fileDialog.ShowDialog() != true)
            return null;
        var selectedPath = fileDialog.FileName;
        
        _fileInputPath = selectedPath;
        return selectedPath;
    }
    
    public override void GenerateCsv(string outputDirectory)
    {
        if (_fileInputPath == null)
            return;
        if (!Directory.Exists(outputDirectory) || !File.Exists(_fileInputPath))
            return;

        var inputPath = _fileInputPath;
        var outputPath = Path.Combine(outputDirectory, Path.GetFileName(_fileInputPath).Replace(".png", ".csv"));
        
        try
        {
            CallEngine(inputPath, outputPath);
        }
        catch (ConvertToGraphException)
        {
            System.Windows.MessageBox.Show($"Could not convert this graph ({inputPath}) to an csv File!", "ChartReader", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        OpenFileExplorer(outputDirectory);
    }
}
