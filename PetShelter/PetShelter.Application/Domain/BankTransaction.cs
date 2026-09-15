namespace PetShelter.Application.Domain;

public class BankTransaction
{
    public long Id { get; private set; }
    public long BankAccountId { get; private set; }
    public TransactionType Type { get; private set; }
    public decimal Amount { get; private set; }
    public DateOnly Date { get; private set; }
    public string? CounterpartyAccount { get; private set; }
    public string Purpose { get; private set; }
    public string? Description { get; private set; }

    public BankTransaction(
        long bankAccountId,
        TransactionType type,
        decimal amount,
        DateOnly date,
        string? counterpartyAccount,
        string purpose,
        string? description)
    {
        BankAccountId = bankAccountId;
        Type = type;
        Amount = amount;
        Date = date;
        CounterpartyAccount = counterpartyAccount;
        Purpose = purpose;
        Description = description;
    }

    public BankTransaction(
        long id,
        long bankAccountId,
        TransactionType type,
        decimal amount,
        DateOnly date,
        string? counterpartyAccount,
        string purpose,
        string? description)
        : this(
            bankAccountId,
            type,
            amount,
            date,
            counterpartyAccount,
            purpose,
            description)
    {
        Id = id;
    }
}