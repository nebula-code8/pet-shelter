using PetShelter.Application.Domain;

namespace PetShelter.Application.Domain.RepositoryInterfaces;

public interface IBankAccountRepository
{
    long Insert(BankAccount bankAccount);
    BankAccount? GetByAssociationId(long associationId);
    bool UpdateBalance(long bankAccountId, decimal balance);
}