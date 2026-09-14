using System.Data;
using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;

namespace PetShelter.Application.Repository;

public class AdoptionDbRepository: BaseDbRepository, IAdoptionRepository
{
    public void Insert(AdoptionRequest request)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            INSERT INTO adoption_requests
            (
                user_id,
                animal_id,
                status,
                adoption_date,
                request_date
            )
            VALUES
            (
                @user_id,
                @animal_id,
                @status,
                @adoption_date,
                @request_date
            );
            """;

        AddParameter(command, "@user_id", request.UserId);
        AddParameter(command, "@animal_id", request.AnimalId);
        AddParameter(command, "@status", (int)request.AdoptionStatus);
        AddParameter(command, "@adoption_date", request.AdoptionDate);
        AddParameter(command, "@request_date", request.RequestDate);

        command.ExecuteNonQuery();
    }

    public int Update(AdoptionRequest request)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            UPDATE adoption_requests
            SET
                status = @status,
                adoption_date = @adoption_date,
                request_date = @request_date
            WHERE user_id = @user_id
              AND animal_id = @animal_id;
            """;

        AddParameter(command, "@status", (int)request.AdoptionStatus);
        AddParameter(command, "@adoption_date", request.AdoptionDate);
        AddParameter(command, "@request_date", request.RequestDate);
        AddParameter(command, "@user_id", request.UserId);
        AddParameter(command, "@animal_id", request.AnimalId);

        return command.ExecuteNonQuery();
    }

    public AdoptionRequest? GetById(long userId, long animalId)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                user_id,
                animal_id,
                status,
                adoption_date,
                request_date
            FROM adoption_requests
            WHERE user_id = @user_id
              AND animal_id = @animal_id;
            """;

        AddParameter(command, "@user_id", userId);
        AddParameter(command, "@animal_id", animalId);

        using IDataReader reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        return MapAdoptionRequest(reader);
    }

    public List<AdoptionRequest> GetAll()
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                user_id,
                animal_id,
                status,
                adoption_date,
                request_date
            FROM adoption_requests
            ORDER BY request_date DESC;
            """;

        using IDataReader reader = command.ExecuteReader();

        List<AdoptionRequest> requests = new();

        while (reader.Read())
        {
            requests.Add(MapAdoptionRequest(reader));
        }

        return requests;
    }

    public bool Delete(long userId, long animalId)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            DELETE FROM adoption_requests
            WHERE user_id = @user_id
              AND animal_id = @animal_id;
            """;

        AddParameter(command, "@user_id", userId);
        AddParameter(command, "@animal_id", animalId);

        return command.ExecuteNonQuery() > 0;
    }

    private static AdoptionRequest MapAdoptionRequest(IDataReader reader)
    {
        return new AdoptionRequest(
            Convert.ToInt64(reader["user_id"]),
            Convert.ToInt64(reader["animal_id"]),
            (AdoptionStatus)Convert.ToInt32(reader["status"]),
            DateOnly.FromDateTime(
                Convert.ToDateTime(reader["adoption_date"])
            ),
            DateOnly.FromDateTime(
                Convert.ToDateTime(reader["request_date"])
            )
        );
    }
}