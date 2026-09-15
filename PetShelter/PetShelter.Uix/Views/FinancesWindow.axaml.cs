using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class FinancesWindow : Window
{
    private readonly long _associationId;
    private readonly string _associationName;

    public FinancesWindow(
        long associationId,
        string associationName)
    {
        InitializeComponent();

        _associationId = associationId;
        _associationName = associationName;

        LoadViewModel();
    }

    private void LoadViewModel()
    {
        DataContext = new FinancesViewModel(
            _associationId,
            _associationName
        );
    }

    private async void AddExpenseButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        ExpenseWindow window =
            new(_associationId);

        bool result =
            await window.ShowDialog<bool>(this);

        if (result)
        {
            LoadViewModel();
        }
    }

    private void BackButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        Close();
    }
}