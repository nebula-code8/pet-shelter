using PetShelter.Application.Domain;

namespace PetShelter.Application.Domain.RepositoryInterfaces;

public interface IBankTransactionRepository
{
    long Insert(BankTransaction transaction);
    List<BankTransaction> GetByBankAccountId(long bankAccountId);
}