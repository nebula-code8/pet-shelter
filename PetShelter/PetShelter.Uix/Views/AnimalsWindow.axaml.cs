using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Application.Domain;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class AnimalsWindow : Window
{
    private readonly AnimalsViewModel _viewModel;
    private readonly long _adminUserId;
    private readonly long _associationId;

    public AnimalsWindow(
        long adminUserId,
        long associationId,
        string associationName)
    {
        InitializeComponent();

        _adminUserId = adminUserId;
        _associationId = associationId;

        _viewModel = new AnimalsViewModel(
            associationId,
            associationName
        );

        DataContext = _viewModel;
    }

    private async void AddButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        AnimalFormWindow window =
            new(_associationId);

        bool result =
            await window.ShowDialog<bool>(this);

        if (result)
        {
            _viewModel.LoadAnimals();
        }
    }

    private async void EditButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        Animal? animal =
            _viewModel.GetSelectedAnimalForEdit();

        if (animal == null)
            return;

        AnimalFormWindow window =
            new(_associationId, animal);

        bool result =
            await window.ShowDialog<bool>(this);

        if (result)
        {
            _viewModel.LoadAnimals();
        }
    }

    private void DeleteButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        _viewModel.DeleteSelectedAnimal();
    }

    private void BackButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        AssociationAdminWindow window =
            new(_adminUserId);

        window.Show();

        Close();
    }
}