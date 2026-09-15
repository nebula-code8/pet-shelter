using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Application.Services;

public class FinanceService : IFinanceService
{
    private readonly IBankAccountRepository _bankAccountRepository;
    private readonly IBankTransactionRepository _bankTransactionRepository;

    public FinanceService()
    {
        _bankAccountRepository =
            Injector.CreateInstance<IBankAccountRepository>();

        _bankTransactionRepository =
            Injector.CreateInstance<IBankTransactionRepository>();
    }

    public long CreateBankAccount(
        long associationId,
        string accountNumber)
    {
        if (associationId <= 0)
        {
            throw new Exception(
                "Association is required."
            );
        }

        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            throw new Exception(
                "Account number is required."
            );
        }

        if (_bankAccountRepository.GetByAssociationId(associationId) != null)
        {
            throw new Exception(
                "This association already has a bank account."
            );
        }

        BankAccount bankAccount = new(
            associationId,
            accountNumber.Trim(),
            0
        );

        return _bankAccountRepository.Insert(bankAccount);
    }

    public BankAccount? GetAccountByAssociationId(
        long associationId)
    {
        return _bankAccountRepository.GetByAssociationId(
            associationId
        );
    }

    public List<BankTransaction> GetTransactionsByAssociationId(
        long associationId)
    {
        BankAccount account =
            GetRequiredBankAccount(associationId);

        return _bankTransactionRepository
            .GetByBankAccountId(account.Id);
    }

    public void AddDonation(
        long associationId,
        decimal amount,
        DateOnly date,
        string sourceAccount,
        string purpose,
        string? description)
    {
        ValidateTransaction(
            amount,
            date,
            purpose
        );

        if (string.IsNullOrWhiteSpace(sourceAccount))
        {
            throw new Exception(
                "Source account is required."
            );
        }

        BankAccount account =
            GetRequiredBankAccount(associationId);

        BankTransaction transaction = new(
            account.Id,
            TransactionType.Donation,
            amount,
            date,
            sourceAccount.Trim(),
            account.AccountNumber,
            purpose.Trim(),
            NormalizeOptionalText(description)
        );

        decimal newBalance =
            account.Balance + amount;

        if (!_bankAccountRepository.UpdateBalance(
                account.Id,
                newBalance))
        {
            throw new Exception(
                "Bank account balance could not be updated."
            );
        }

        try
        {
            _bankTransactionRepository.Insert(transaction);
        }
        catch
        {
            _bankAccountRepository.UpdateBalance(
                account.Id,
                account.Balance
            );

            throw;
        }
    }

    public void AddExpense(
        long associationId,
        decimal amount,
        DateOnly date,
        string destinationAccount,
        string purpose,
        string? description)
    {
        ValidateTransaction(
            amount,
            date,
            purpose
        );

        if (string.IsNullOrWhiteSpace(destinationAccount))
        {
            throw new Exception(
                "Destination account is required."
            );
        }

        BankAccount account =
            GetRequiredBankAccount(associationId);

        if (amount > account.Balance)
        {
            throw new Exception(
                "Insufficient funds."
            );
        }

        BankTransaction transaction = new(
            account.Id,
            TransactionType.Expense,
            amount,
            date,
            account.AccountNumber,
            destinationAccount.Trim(),
            purpose.Trim(),
            NormalizeOptionalText(description)
        );

        decimal newBalance =
            account.Balance - amount;

        if (!_bankAccountRepository.UpdateBalance(
                account.Id,
                newBalance))
        {
            throw new Exception(
                "Bank account balance could not be updated."
            );
        }

        try
        {
            _bankTransactionRepository.Insert(transaction);
        }
        catch
        {
            _bankAccountRepository.UpdateBalance(
                account.Id,
                account.Balance
            );

            throw;
        }
    }

    private BankAccount GetRequiredBankAccount(
        long associationId)
    {
        BankAccount? account =
            _bankAccountRepository.GetByAssociationId(
                associationId
            );

        if (account == null)
        {
            throw new Exception(
                "Bank account was not found for this association."
            );
        }

        return account;
    }

    private static void ValidateTransaction(
        decimal amount,
        DateOnly date,
        string purpose)
    {
        if (amount <= 0)
        {
            throw new Exception(
                "Amount must be greater than zero."
            );
        }

        if (string.IsNullOrWhiteSpace(purpose))
        {
            throw new Exception(
                "Purpose is required."
            );
        }

        if (date > DateOnly.FromDateTime(DateTime.Today))
        {
            throw new Exception(
                "Transaction date cannot be in the future."
            );
        }
    }

    private static string? NormalizeOptionalText(
        string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}