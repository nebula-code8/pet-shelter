using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class AssociationsWindow : Window
{
    private readonly AssociationsViewModel _viewModel;
    private readonly long _userId;

    public AssociationsWindow(long userId)
    {
        InitializeComponent();

        _userId = userId;
        _viewModel = new AssociationsViewModel();

        DataContext = _viewModel;
    }

    private async void AddButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        AssociationFormWindow window = new();

        bool result =
            await window.ShowDialog<bool>(this);

        if (result)
        {
            _viewModel.LoadAssociations();
        }
    }

    private async void EditButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        var association =
            _viewModel.GetSelectedAssociationForEdit();

        if (association == null)
            return;

        AssociationFormWindow window =
            new(association);

        bool result =
            await window.ShowDialog<bool>(this);

        if (result)
        {
            _viewModel.LoadAssociations();
        }
    }

    private void DeleteButton_Click(object? sender, RoutedEventArgs e)
    {
        _viewModel.DeleteSelectedAssociation();
    }

    private void BackButton_Click(object? sender, RoutedEventArgs e)
    {
        AdminWindow adminWindow = new(_userId);
        adminWindow.Show();

        Close();
    }
}