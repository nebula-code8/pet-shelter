using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class UsersWindow : Window
{
    private readonly long _userId;

    public UsersWindow(long userId)
    {
        InitializeComponent();

        _userId = userId;
        DataContext = new UsersViewModel();
    }

    private void BackButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        AdminWindow adminWindow = new(_userId);
        adminWindow.Show();

        Close();
    }
}