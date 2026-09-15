using System.Collections.ObjectModel;
using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.ViewModels;

public class FinancesViewModel
{
    private readonly IFinanceService _financeService;

    public long AssociationId { get; }
    public string AssociationName { get; }

    public BankAccount? Account { get; private set; }

    public string AccountNumber =>
        Account?.AccountNumber ?? "No bank account";

    public string BalanceText =>
        Account == null
            ? "0.00 RSD"
            : $"{Account.Balance:N2} RSD";

    public ObservableCollection<BankTransaction> Transactions { get; }
        = new();

    public string? ErrorMessage { get; private set; }

    public FinancesViewModel(
        long associationId,
        string associationName)
    {
        AssociationId = associationId;
        AssociationName = associationName;

        _financeService =
            Injector.CreateInstance<IFinanceService>();

        LoadFinances();
    }

    private void LoadFinances()
    {
        try
        {
            Account =
                _financeService.GetAccountByAssociationId(
                    AssociationId
                );

            if (Account == null)
            {
                ErrorMessage =
                    "Bank account was not found for this association.";

                return;
            }

            List<BankTransaction> transactions =
                _financeService
                    .GetTransactionsByAssociationId(
                        AssociationId
                    );

            Transactions.Clear();

            foreach (BankTransaction transaction in transactions)
            {
                Transactions.Add(transaction);
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}