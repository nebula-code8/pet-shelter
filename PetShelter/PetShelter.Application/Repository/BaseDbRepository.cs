using System.Data;

namespace PetShelter.Application.Repository;

public class BaseDbRepository
{
    protected static IDbConnection CreateConnection()
    {
        return PostgresConnection.CreateConnection();
    }

    protected static void AddParameter(IDbCommand command, string name, object? value)
    {
        IDbDataParameter parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value ?? DBNull.Value;
        command.Parameters.Add(parameter);
    }
}