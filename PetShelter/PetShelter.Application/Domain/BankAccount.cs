namespace PetShelter.Application.Domain;

public class BankAccount
{
    public long Id { get; private set; }
    public long AssociationId { get; private set; }
    public string AccountNumber { get; private set; }
    public decimal Balance { get; private set; }

    public BankAccount(
        long associationId,
        string accountNumber,
        decimal balance)
    {
        AssociationId = associationId;
        AccountNumber = accountNumber;
        Balance = balance;
    }

    public BankAccount(
        long id,
        long associationId,
        string accountNumber,
        decimal balance)
    {
        Id = id;
        AssociationId = associationId;
        AccountNumber = accountNumber;
        Balance = balance;
    }
}