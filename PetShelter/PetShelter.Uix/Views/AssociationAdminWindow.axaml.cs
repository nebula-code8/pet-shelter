using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class AssociationAdminWindow : Window
{
    private readonly AssociationAdminViewModel _viewModel;

    public AssociationAdminWindow(long userId)
    {
        InitializeComponent();

        _viewModel = new AssociationAdminViewModel(userId);

        DataContext = _viewModel;
    }

    private void VolunteersButton_Click(object? sender, RoutedEventArgs e)
    {
        if (_viewModel.Association == null) return;
        VolunteersWindow window = new(_viewModel.UserId, _viewModel.Association.Id, _viewModel.Association.Name);
        window.Show();

        Close();
    }

    private void AnimalsButton_Click(object? sender, RoutedEventArgs e)
    {
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

    private async void FinancesButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (_viewModel.Association == null)
            return;

        FinancesWindow window = new(
            _viewModel.Association.Id,
            _viewModel.Association.Name
        );

        await window.ShowDialog(this);
    }

    private void LogoutButton_Click(object? sender, RoutedEventArgs e)
    {
        MainWindow mainWindow = new();
        mainWindow.Show();

        Close();
    }
}