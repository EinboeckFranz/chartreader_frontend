namespace GraphToCsvFrontend.customControls;

public partial class CustomButton
{
    public event EventHandler<RoutedEventArgs>? CustomButtonOnClick;

    private string _buttonContent = "";
    public string ButtonContent
    {
        get => _buttonContent;
        set
        {
            _buttonContent = value;
            ButtonToPress.Content = value;
        }
    }

    private bool _canBePressed = true;
    public bool CanBePressed
    {
        get => _canBePressed;
        set
        {
            _canBePressed = value;
            ButtonToPress.IsEnabled = value;
        }
    }
    
    public CustomButton() => InitializeComponent();

    private void ButtonToPress_OnClick(object sender, RoutedEventArgs e) => CustomButtonOnClick?.Invoke(this, e);
}