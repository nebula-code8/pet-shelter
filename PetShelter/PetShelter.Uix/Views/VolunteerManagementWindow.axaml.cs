using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class VolunteerManagementWindow : Window
{
    private readonly VolunteerManagementViewModel _viewModel;
    private readonly long _adminUserId;

    public VolunteerManagementWindow(long adminUserId, long associationId, string associationName)
    {
        InitializeComponent();
        _adminUserId = adminUserId;
        _viewModel = new VolunteerManagementViewModel(associationId, associationName);
        DataContext = _viewModel;
    }

    private void ApproveButton_Click(object? sender, RoutedEventArgs e)
    {
        _viewModel.ApproveSelectedVolunteer();
    }

    private void RejectButton_Click(object? sender, RoutedEventArgs e)
    {
        _viewModel.RejectSelectedVolunteer();
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        AssociationAdminWindow window = new(_adminUserId);
        window.Show();

        Close();
    }
}