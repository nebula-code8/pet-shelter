using System.Data;
using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;

namespace PetShelter.Application.Repository;

public class BankAccountDbRepository :
    BaseDbRepository,
    IBankAccountRepository
{
    public long Insert(BankAccount bankAccount)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              INSERT INTO bank_accounts
                              (
                                  association_id,
                                  account_number,
                                  balance
                              )
                              VALUES
                              (
                                  @associationId,
                                  @accountNumber,
                                  @balance
                              )
                              RETURNING id;
                              """;

        AddParameter(command, "@associationId", bankAccount.AssociationId);

        AddParameter(command, "@accountNumber", bankAccount.AccountNumber);

        AddParameter(command, "@balance", bankAccount.Balance);

        return Convert.ToInt64(command.ExecuteScalar());
    }

    public BankAccount? GetByAssociationId(long associationId)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              SELECT
                                  id,
                                  association_id,
                                  account_number,
                                  balance
                              FROM bank_accounts
                              WHERE association_id = @associationId;
                              """;

        AddParameter(command, "@associationId", associationId);

        using IDataReader reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        return MapBankAccount(reader);
    }

    public bool UpdateBalance(long bankAccountId, decimal balance)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              UPDATE bank_accounts
                              SET balance = @balance
                              WHERE id = @id;
                              """;

        AddParameter(command, "@balance", balance);
        AddParameter(command, "@id", bankAccountId);

        return command.ExecuteNonQuery() > 0;
    }

    private static BankAccount MapBankAccount(IDataReader reader)
    {
        return new BankAccount(
            Convert.ToInt64(reader["id"]),
            Convert.ToInt64(reader["association_id"]),
            Convert.ToString(reader["account_number"])!,
            Convert.ToDecimal(reader["balance"])
        );
    }
    
    public bool ExistsByAccountNumber(
        string accountNumber)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              SELECT COUNT(*)
                              FROM bank_accounts
                              WHERE account_number = @accountNumber;
                              """;

        AddParameter(
            command,
            "@accountNumber",
            accountNumber
        );

        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }
}