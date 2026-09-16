using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Application.Domain;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class AnimalFormWindow : Window
{
    private readonly AnimalFormViewModel _viewModel;

    public AnimalFormWindow(long associationId)
    {
        InitializeComponent();

        _viewModel =
            new AnimalFormViewModel(associationId);

        DataContext = _viewModel;
    }

    public AnimalFormWindow(
        long associationId,
        Animal animal)
    {
        InitializeComponent();

        _viewModel =
            new AnimalFormViewModel(
                associationId,
                animal
            );

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