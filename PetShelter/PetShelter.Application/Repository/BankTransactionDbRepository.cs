using System.Data;
using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;

namespace PetShelter.Application.Repository;

public class BankTransactionDbRepository :
    BaseDbRepository,
    IBankTransactionRepository
{
    public long Insert(BankTransaction transaction)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              INSERT INTO bank_transactions
                              (
                                  bank_account_id,
                                  transaction_type,
                                  amount,
                                  transaction_date,
                                  source_account,
                                  destination_account,
                                  purpose,
                                  description
                              )
                              VALUES
                              (
                                  @bankAccountId,
                                  @type,
                                  @amount,
                                  @date,
                                  @sourceAccount,
                                  @destinationAccount,
                                  @purpose,
                                  @description
                              )
                              RETURNING id;
                              """;

        AddParameter(
            command,
            "@bankAccountId",
            transaction.BankAccountId
        );

        AddParameter(
            command,
            "@type",
            (int)transaction.Type
        );

        AddParameter(
            command,
            "@amount",
            transaction.Amount
        );

        AddParameter(
            command,
            "@date",
            transaction.Date
        );

        AddParameter(
            command,
            "@sourceAccount",
            transaction.SourceAccount
        );

        AddParameter(
            command,
            "@destinationAccount",
            transaction.DestinationAccount
        );

        AddParameter(
            command,
            "@purpose",
            transaction.Purpose
        );

        AddParameter(
            command,
            "@description",
            transaction.Description ?? (object)DBNull.Value
        );

        return Convert.ToInt64(command.ExecuteScalar());
    }

    public List<BankTransaction> GetByBankAccountId(
        long bankAccountId)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              SELECT
                                  id,
                                  bank_account_id,
                                  transaction_type,
                                  amount,
                                  transaction_date,
                                  source_account,
                                  destination_account,
                                  purpose,
                                  description
                              FROM bank_transactions
                              WHERE bank_account_id = @bankAccountId
                              ORDER BY transaction_date DESC, id DESC;
                              """;

        AddParameter(
            command,
            "@bankAccountId",
            bankAccountId
        );

        using IDataReader reader = command.ExecuteReader();

        List<BankTransaction> transactions = new();

        while (reader.Read())
        {
            transactions.Add(MapBankTransaction(reader));
        }

        return transactions;
    }

    private static BankTransaction MapBankTransaction(
        IDataReader reader)
    {
        return new BankTransaction(
            Convert.ToInt64(reader["id"]),
            Convert.ToInt64(reader["bank_account_id"]),
            (TransactionType)Convert.ToInt32(
                reader["transaction_type"]
            ),
            Convert.ToDecimal(reader["amount"]),
            (DateOnly)reader["transaction_date"],
            Convert.ToString(reader["source_account"])!,
            Convert.ToString(reader["destination_account"])!,
            Convert.ToString(reader["purpose"])!,
            reader["description"] == DBNull.Value
                ? null
                : Convert.ToString(reader["description"])
        );
    }
}