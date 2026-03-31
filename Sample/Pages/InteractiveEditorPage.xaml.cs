namespace Sample.Pages;

public partial class InteractiveEditorPage : ContentPage
{
    private const string DefaultDiagram =
        "graph LR\n" +
        "    A[Input] --> B[Process]\n" +
        "    B --> C[Output]";

    public InteractiveEditorPage()
    {
        InitializeComponent();
        DiagramEditor.Text = DefaultDiagram;
        DiagramControl.DiagramText = DefaultDiagram;
    }

    private void OnPanToggled(object? sender, ToggledEventArgs e)
        => DiagramControl.AllowPanning = e.Value;

    private void OnZoomToggled(object? sender, ToggledEventArgs e)
        => DiagramControl.AllowZooming = e.Value;

    private void OnEditorTextChanged(object sender, TextChangedEventArgs e)
    {
        var text = e.NewTextValue ?? string.Empty;
        DiagramControl.DiagramText = text;

        var parseError = DiagramControl.ParseError;
        if (!string.IsNullOrWhiteSpace(parseError))
        {
            ParseErrorLabel.Text = $"Parse error: {parseError}";
            ParseErrorLabel.IsVisible = true;
        }
        else
        {
            ParseErrorLabel.Text = string.Empty;
            ParseErrorLabel.IsVisible = false;
        }
    }
}
