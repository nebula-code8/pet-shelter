using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class VolunteerRegistrationWindow : Window
{
    private readonly VolunteerRegistrationViewModel _viewModel;

    public VolunteerRegistrationWindow()
    {
        InitializeComponent();

        _viewModel = new VolunteerRegistrationViewModel();
        DataContext = _viewModel;
    }

    private void RegisterButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (!_viewModel.Register())
            return;

        LoginWindow loginWindow = new();
        loginWindow.Show();

        Close();
    }

    private void BackButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        LoginWindow loginWindow = new();
        loginWindow.Show();

        Close();
    }
}