using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class ExpenseWindow : Window
{
    private readonly ExpenseViewModel _viewModel;

    public ExpenseWindow(long associationId)
    {
        InitializeComponent();

        _viewModel =
            new ExpenseViewModel(associationId);

        DataContext = _viewModel;
    }

    private void AddExpenseButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        if (_viewModel.AddExpense())
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