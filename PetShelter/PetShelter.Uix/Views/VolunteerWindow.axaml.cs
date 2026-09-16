using Avalonia.Controls;
using Avalonia.Interactivity;

namespace PetShelter.Uix.Views;

public partial class VolunteerWindow : Window
{
    private readonly long _userId;

    public VolunteerWindow(long userId)
    {
        InitializeComponent();

        _userId = userId;
    }

    private async void AdoptersButton_Click(object? sender, RoutedEventArgs e)
    {
        AllAdoptersWindow window = new();
        await window.ShowDialog(this);
    }

    private async void AdoptionRequestsButton_Click(object? sender, RoutedEventArgs e)
    {
        AdoptionRequestsWindow window = new();
        await window.ShowDialog(this);
    }

    private void LogoutButton_Click(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new();
        mainWindow.Show();

        Close();
    }

    private void InitializeComponent()
    {
        Avalonia.Markup.Xaml.AvaloniaXamlLoader.Load(this);
    }
}
