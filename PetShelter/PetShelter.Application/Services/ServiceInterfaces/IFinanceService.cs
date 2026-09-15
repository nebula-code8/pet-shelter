using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IFinanceService
{
    long CreateBankAccount(
        long associationId,
        string accountNumber
    );

    BankAccount? GetAccountByAssociationId(
        long associationId
    );

    List<BankTransaction> GetTransactionsByAssociationId(
        long associationId
    );

    void AddDonation(
        long associationId,
        decimal amount,
        DateOnly date,
        string sourceAccount,
        string purpose,
        string? description
    );

    void AddExpense(
        long associationId,
        decimal amount,
        DateOnly date,
        string destinationAccount,
        string purpose,
        string? description
    );
    
    bool AccountNumberExists(string accountNumber);
}