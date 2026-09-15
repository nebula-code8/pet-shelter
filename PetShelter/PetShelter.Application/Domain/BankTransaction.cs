namespace PetShelter.Application.Domain;

public class BankTransaction
{
    public long Id { get; private set; }
    public long BankAccountId { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public DateOnly Date { get; private set; }
    public string SourceAccount { get; private set; }
    public string DestinationAccount { get; private set; }
    public string Purpose { get; private set; }
    public string? Description { get; private set; }

    public BankTransaction(
        long bankAccountId,
        TransactionType type,
        decimal amount,
        DateOnly date,
        string sourceAccount,
        string destinationAccount,
        string purpose,
        string? description)
    {
        BankAccountId = bankAccountId;
        Type = type;
        Amount = amount;
        Date = date;
        SourceAccount = sourceAccount;
        DestinationAccount = destinationAccount;
        Purpose = purpose;
        Description = description;
    }

    public BankTransaction(
        long id,
        long bankAccountId,
        TransactionType type,
        decimal amount,
        DateOnly date,
        string sourceAccount,
        string destinationAccount,
        string purpose,
        string? description)
        : this(
            bankAccountId,
            type,
            amount,
            date,
            sourceAccount,
            destinationAccount,
            purpose,
            description)
    {
        Id = id;
    }
}