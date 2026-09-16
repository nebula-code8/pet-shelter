using System.Data;
using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;

namespace PetShelter.Application.Repository;

public class TemporaryAdoptionDbRepository : BaseDbRepository, ITemporaryAdoptionRepository
{
    public void Insert(TemporaryAdoption tempAdoption)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = @"INSERT INTO temporary_adoptions (user_id, animal_id, start_date, end_date)
VALUES (@user_id, @animal_id, @start_date, @end_date);";

        AddParameter(command, "@user_id", tempAdoption.UserId);
        AddParameter(command, "@animal_id", tempAdoption.AnimalId);
        AddParameter(command, "@start_date", tempAdoption.StartDate);
        AddParameter(command, "@end_date", tempAdoption.EndDate);

        command.ExecuteNonQuery();
    }

    public List<TemporaryAdoption> GetByUserId(long userId)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = @"SELECT user_id, animal_id, start_date, end_date
FROM temporary_adoptions
WHERE user_id = @user_id
ORDER BY start_date DESC;";

        AddParameter(command, "@user_id", userId);

        
using IDataReader reader = command.ExecuteReader();
        List<TemporaryAdoption> list = new();
        while (reader.Read())
        {
            long u = reader.GetInt64(0);
            long a = reader.GetInt64(1);
            DateOnly start = DateOnly.FromDateTime(reader.GetDateTime(2));
            DateOnly? end = reader.IsDBNull(3) ? null : DateOnly.FromDateTime(reader.GetDateTime(3));
            list.Add(new TemporaryAdoption(u, a, start, end));
        }

        return list;
    }

    public List<TemporaryAdoption> GetAllForAssociation(long associationId)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = @"SELECT t.user_id, t.animal_id, t.start_date, end_date
            FROM temporary_adoptions t
            JOIN animals a ON t.animal_id = a.id
            WHERE a.association_id = @assocId
            ORDER BY t.start_date DESC;";

        var param = command.CreateParameter();
        param.ParameterName = "@assocId";
        param.Value = associationId;
        command.Parameters.Add(param);

        using IDataReader reader = command.ExecuteReader();
        List<TemporaryAdoption> list = new();
        while (reader.Read())
        {
            long u = reader.GetInt64(0);
            long a = reader.GetInt64(1);
            DateOnly start = DateOnly.FromDateTime(reader.GetDateTime(2));
            DateOnly? end = reader.IsDBNull(3) ? null : DateOnly.FromDateTime(reader.GetDateTime(3));
            list.Add(new TemporaryAdoption(u, a, start, end));
        }

        return list;
    }

    public List<TemporaryAdoption> GetByAnimalId(long animalId)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = @"SELECT user_id, animal_id, start_date, end_date
FROM temporary_adoptions
WHERE animal_id = @animal_id
ORDER BY start_date DESC;";

        AddParameter(command, "@animal_id", animalId);

        using IDataReader reader = command.ExecuteReader();
        List<TemporaryAdoption> list = new();
        while (reader.Read())
        {
            long u = reader.GetInt64(0);
            long a = reader.GetInt64(1);
            DateOnly start = DateOnly.FromDateTime(reader.GetDateTime(2));
            DateOnly? end = reader.IsDBNull(3) ? null : DateOnly.FromDateTime(reader.GetDateTime(3));
            list.Add(new TemporaryAdoption(u, a, start, end));
        }

        return list;
    }

    public List<TemporaryAdoption> GetAll()
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = @"SELECT user_id, animal_id, start_date, end_date
FROM temporary_adoptions
ORDER BY start_date DESC;";

        using IDataReader reader = command.ExecuteReader();
        List<TemporaryAdoption> list = new();
        while (reader.Read())
        {
            long u = reader.GetInt64(0);
            long a = reader.GetInt64(1);
            DateOnly start = DateOnly.FromDateTime(reader.GetDateTime(2));
            DateOnly? end = reader.IsDBNull(3) ? null : DateOnly.FromDateTime(reader.GetDateTime(3));
            list.Add(new TemporaryAdoption(u, a, start, end));
        }

        return list;
    }

    public bool Delete(long userId, long animalId, DateOnly startDate)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = @"""
            DELETE FROM temporary_adoptions
            WHERE user_id = @user_id AND animal_id = @animal_id AND start_date = @start_date;
            """;

        AddParameter(command, "@user_id", userId);
        AddParameter(command, "@animal_id", animalId);
        AddParameter(command, "@start_date", startDate);

        return command.ExecuteNonQuery() > 0;
    }
}
