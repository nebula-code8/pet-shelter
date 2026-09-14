using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Application.Domain;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class AssociationFormWindow : Window
{
    private readonly AssociationFormViewModel _viewModel;

    public AssociationFormWindow()
    {
        InitializeComponent();

        _viewModel = new AssociationFormViewModel();
        DataContext = _viewModel;
    }

    public AssociationFormWindow(Association association)
    {
        InitializeComponent();

        _viewModel = new AssociationFormViewModel(association);
        DataContext = _viewModel;
    }

    private void SaveButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (_viewModel.Save())
        {
            Close(true);
        }
    }

    private void CancelButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        Close(false);
    }
}