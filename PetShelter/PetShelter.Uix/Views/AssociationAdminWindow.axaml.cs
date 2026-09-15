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
        VolunteerManagementWindow window = new(_viewModel.UserId, _viewModel.Association.Id, _viewModel.Association.Name);
        window.Show();

        Close();
    }

    private void AnimalsButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
    }

    private void AdoptersButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
    }

    private void AdoptionRequestsButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
    }

    private void FinancesButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
    }

    private void LogoutButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        LoginWindow loginWindow = new();
        loginWindow.Show();

        Close();
    }
}