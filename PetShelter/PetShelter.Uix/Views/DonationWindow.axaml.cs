using System.Globalization;
using Avalonia.Controls;
using Avalonia.Interactivity;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.Views;

public partial class DonationWindow : Window
{
    public decimal Amount { get; private set; }
    public string SourceAccount { get; private set; } = "";
    public string Purpose { get; private set; } = "";
    public string? Description { get; private set; }

    private IFinanceService _financeService;
    private long _associationId;
    
    public DonationWindow(long associationId)
    {
        InitializeComponent();
        
        _financeService = Injector.CreateInstance<IFinanceService>();
        _associationId = associationId;
    }

    private async void ConfirmButton_Click(object? sender, RoutedEventArgs e)
    {
        if (!decimal.TryParse(
                AmountTextBox.Text,
                NumberStyles.Number,
                CultureInfo.InvariantCulture,
                out decimal amount))
        {
            return;
        }

        if (string.IsNullOrWhiteSpace(SourceAccountTextBox.Text) ||
            string.IsNullOrWhiteSpace(PurposeTextBox.Text))
        {
            return;
        }

        Amount = amount;
        SourceAccount = SourceAccountTextBox.Text;
        Purpose = PurposeTextBox.Text;

        Description = string.IsNullOrWhiteSpace(DescriptionTextBox.Text)
            ? null
            : DescriptionTextBox.Text;
        
        _financeService.AddDonation(
            _associationId,
            Amount,
            DateOnly.FromDateTime(DateTime.Now),
            SourceAccount,
            Purpose,
            Description);

        await PopupWindow.ShowMessage(this, "Successful Donation", "Your donation has successfully completed");

        Close();
    }

    private void CancelButton_Click(object? sender, RoutedEventArgs e)
    {
        Close();
    }
}