using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia;

namespace PetShelter.Uix.Views;

public partial class ErrorDialog : Window
{
    public static void ShowError(Window owner, string message)
    {
        var dialog = new Window
        {
            Title = "Greška",
            Width = 360,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };
        var panel = new StackPanel { Margin = new Avalonia.Thickness(24), Spacing = 16 };
        panel.Children.Add(new TextBlock
            { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap, FontSize = 14 });
        var closeButton = new Button { Content = "U redu", Padding = new Avalonia.Thickness(16, 8) };
        closeButton.Click += (_, __) => dialog.Close();
        panel.Children.Add(closeButton);
        dialog.Content = panel;
        dialog.ShowDialog(owner);
    }
}

public partial class ConformationDialog : Window
{
    public static Task<bool> ShowAsync(Window owner, string title, string message)
    {
        var dialogCompletitionSource = new TaskCompletionSource<bool>();
        var dialog = new Window
        {
            Title = title,
            Width = 380,
            Height = 200,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };
        var panel = new StackPanel { Margin = new Avalonia.Thickness(24), Spacing = 16 };
        panel.Children.Add(new TextBlock
        {
            Text = message,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            FontSize = 14
        });
        var buttonBlock = new StackPanel { Orientation = Avalonia.Layout.Orientation.Horizontal, Spacing = 12 };
        var yesButton = new Button { Content = "Da", Padding = new Avalonia.Thickness(16, 8) };
        var noButton = new Button { Content = "Ne", Padding = new Avalonia.Thickness(16, 8) };
        yesButton.Click += (_, _) =>
        {
            dialogCompletitionSource.SetResult(true);
            dialog.Close();
        };
        noButton.Click += (_, _) =>
        {
            dialogCompletitionSource.SetResult(false);
            dialog.Close();
        };
        buttonBlock.Children.Add(yesButton);
        buttonBlock.Children.Add(noButton);
        panel.Children.Add(buttonBlock);
        dialog.Content = panel;
        dialog.ShowDialog(owner);
        return dialogCompletitionSource.Task;
    }
}

public partial class InputDialog : Window
{
    private readonly TextBox _inputBox;
    private string _textBlockContent;

    public InputDialog(string title = "Enter Text", string textBlockContent = "Enter Text")
    {
        Title = title;
        Width = 350;
        Height = 350;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        var root = new Grid { Margin = new Thickness(20), RowDefinitions = new RowDefinitions("Auto,*,Auto") };
        root.Children.Add(new TextBlock { Text = _textBlockContent, Margin = new Thickness(0, 0, 0, 10) });
        _inputBox = new TextBox
        {
            AcceptsReturn = true,
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch
        };
        Grid.SetRow(_inputBox, 1);
        root.Children.Add(_inputBox);
        var buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 10,
            Margin = new Thickness(0, 15, 0, 0)
        };
        var cancelButton = new Button { Content = "Cancel", Width = 90 };
        cancelButton.Click += (_, _) => { Close(null); };
        var okButton = new Button { Content = "OK", Width = 90 };
        okButton.Click += (_, _) => { Close(_inputBox.Text); };
        buttonPanel.Children.Add(cancelButton);
        buttonPanel.Children.Add(okButton);
        root.Children.Add(buttonPanel);
        Content = root;
    }

    public static async Task<string?> ShowAsync(Window owner, string title = "Enter Text",
        string textBlockContent = "Enter Text")
    {
        var dialog = new InputDialog(title, textBlockContent);
        return await dialog.ShowDialog<string?>(owner);
    }
}

public partial class PopupWindow : Window
{
    public static async Task ShowMessage(Window owner, string title, string message)
    {
        var dialog = new Window
        {
            Title = title,
            Width = 360,
            Height = 180,
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            CanResize = false
        };
        var panel = new StackPanel { Margin = new Avalonia.Thickness(24), Spacing = 16 };
        panel.Children.Add(new TextBlock
            { Text = message, TextWrapping = Avalonia.Media.TextWrapping.Wrap, FontSize = 14 });
        var closeButton = new Button { Content = "U redu", Padding = new Avalonia.Thickness(16, 8) };
        closeButton.Click += (_, __) => dialog.Close();
        panel.Children.Add(closeButton);
        dialog.Content = panel;
        await dialog.ShowDialog(owner);
    }
}