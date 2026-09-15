using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.ViewModels;

public class ExpenseViewModel : INotifyPropertyChanged
{
    private readonly IFinanceService _financeService;
    private readonly long _associationId;

    private string? _errorMessage;

    public string SourceAccount { get; }

    public string Amount { get; set; } = "";
    public DateTimeOffset? Date { get; set; } = DateTimeOffset.Now;
    public string DestinationAccount { get; set; } = "";
    public string Purpose { get; set; } = "";
    public string Description { get; set; } = "";

    public string? ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public ExpenseViewModel(long associationId)
    {
        _associationId = associationId;

        _financeService =
            Injector.CreateInstance<IFinanceService>();

        BankAccount? account =
            _financeService.GetAccountByAssociationId(
                associationId
            );

        if (account == null)
        {
            throw new Exception(
                "Bank account was not found for this association."
            );
        }

        SourceAccount = account.AccountNumber;
    }

    public bool AddExpense()
    {
        try
        {
            ErrorMessage = null;

            if (!decimal.TryParse(
                    Amount,
                    NumberStyles.Number,
                    CultureInfo.InvariantCulture,
                    out decimal amount))
            {
                throw new Exception(
                    "Invalid amount."
                );
            }

            if (Date == null)
            {
                throw new Exception(
                    "Date is required."
                );
            }

            DateOnly date = DateOnly.FromDateTime(
                Date.Value.DateTime
            );

            _financeService.AddExpense(
                _associationId,
                amount,
                date,
                DestinationAccount,
                Purpose,
                string.IsNullOrWhiteSpace(Description)
                    ? null
                    : Description
            );

            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName)
        );
    }
}