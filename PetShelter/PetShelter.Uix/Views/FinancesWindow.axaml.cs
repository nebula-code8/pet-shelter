using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Uix.ViewModels;

namespace PetShelter.Uix.Views;

public partial class FinancesWindow : Window
{
    public FinancesWindow(
        long associationId,
        string associationName)
    {
        InitializeComponent();

        DataContext = new FinancesViewModel(
            associationId,
            associationName
        );
    }

    private void BackButton_Click(
        object? sender,
        RoutedEventArgs e)
    {
        Close();
    }
}