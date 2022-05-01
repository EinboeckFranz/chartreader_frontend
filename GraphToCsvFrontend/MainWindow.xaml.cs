namespace GraphToCsvFrontend;

public partial class MainWindow
{
    private const string FolderImgPath = "img\\folder.png";

    private bool _enableFolderSelection;
    private string? _currentPath = "";
    private Generator _currentGenerator = new FileGenerator();

    public MainWindow() => InitializeComponent();

    #region TitleBar Button Click Listener
    private void ButtonMinimize_OnClick(object sender, RoutedEventArgs e) 
        => Application.Current.MainWindow!.WindowState = System.Windows.WindowState.Minimized;

    private void ButtonMaximize_OnClick(object sender, RoutedEventArgs e)
    {
        Application.Current.MainWindow!.WindowState = Application.Current.MainWindow.WindowState != System.Windows.WindowState.Maximized
            ? System.Windows.WindowState.Maximized
            : System.Windows.WindowState.Normal;
    }

    private void ButtonQuit_OnClick(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    
    private void Border_OnMouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.LeftButton == MouseButtonState.Pressed) 
            DragMove();
    }
    #endregion

    #region OnClick Listener
    private void ChangeInputMethod_OnClick(object? sender, RoutedEventArgs e)
    {
        _enableFolderSelection = !_enableFolderSelection;
        _currentGenerator = _enableFolderSelection ? new DirectoryGenerator() : new FileGenerator();
        SetButtonContent(_enableFolderSelection);
    }
    
    private void SelectGraphPath_OnClick(object? sender, RoutedEventArgs e)
    {
        _currentPath = _currentGenerator.GetInputPath();
        if (_currentPath == null)
        {
            GenerateCsvBtn.CanBePressed = false;
            ImgGraph.Source = null;
            ShowedImageFileName.Content = "";
            return;
        }
            
        ImgGraph.Source = _enableFolderSelection ? PathToImage(FolderImgPath) : PathToImage(_currentPath);
        ShowedImageFileName.Content = $"Path: {_currentPath}";
        GenerateCsvBtn.CanBePressed = true;
    }

    private void GenerateCsv_OnClick(object? sender, RoutedEventArgs e)
    {
        var dialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog
        {
            Multiselect = false,
            Description = "Select Output-Location",
            UseDescriptionForTitle = true
        };
        if (!dialog.ShowDialog() ?? false)
            return;

        var outputDirectory = dialog.SelectedPath;
        
        if (!Directory.Exists(outputDirectory))
            return;
        
        BusyIndicator.IsBusy = true;
        Task.Factory
            .StartNew(() => { _currentGenerator.GenerateCsv(outputDirectory); })
            .ContinueWith(_ => BusyIndicator.IsBusy = false, TaskScheduler.FromCurrentSynchronizationContext());
    }
    #endregion
    
    #region Helper
    private static BitmapImage PathToImage(string imgPath)
    {
        var image = new BitmapImage();
        image.BeginInit();
        image.UriSource = new Uri(imgPath, UriKind.RelativeOrAbsolute);
        image.CacheOption = BitmapCacheOption.OnLoad;
        image.EndInit();
        return image;
    }
    
    private void SetButtonContent(bool enableFolderSelection)
    {
        ChangeSelectionBtn.ButtonContent = !enableFolderSelection ? "Use Folder Selection" : "Use File Selection";
        SelectGraphBtn.ButtonContent = enableFolderSelection ? "Select Folder" : "Select File";
        GenerateCsvBtn.ButtonContent = enableFolderSelection ? "Generate .csv Files" : "Generate .csv File";
    }
    #endregion
}