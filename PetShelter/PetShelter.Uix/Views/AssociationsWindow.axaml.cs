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

    private void AddButton_Click(object? sender, RoutedEventArgs e)
    {
    }

    private void EditButton_Click(object? sender, RoutedEventArgs e)
    {
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