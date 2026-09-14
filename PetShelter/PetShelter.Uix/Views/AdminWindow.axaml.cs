using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class AdminWindow : Window
{
    private readonly AdminViewModel _viewModel;

    public AdminWindow(long userId)
    {
        InitializeComponent();

        _viewModel = new AdminViewModel(userId);
        DataContext = _viewModel;
    }

    private void AssociationsButton_Click(object? sender, RoutedEventArgs e)
    {
        AssociationsWindow associationsWindow = new(_viewModel.UserId);
        associationsWindow.Show();

        Close();
    }

    private void UsersButton_Click(object? sender, RoutedEventArgs e)
    {
    }

    private void LogoutButton_Click(object? sender, RoutedEventArgs e)
    {
        LoginWindow loginWindow = new();
        loginWindow.Show();
        Close();
    }
}