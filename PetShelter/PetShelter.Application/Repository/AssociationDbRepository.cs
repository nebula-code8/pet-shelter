using System.Data;
using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;

namespace PetShelter.Application.Repository;

public class AssociationDbRepository: BaseDbRepository, IAssociationRepository
{
    public long Insert(Association association)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            INSERT INTO associations
            (
                name,
                date_of_establishment,
                phone_number,
                email,
                tip,
                description,
                address,
                admin,
                is_deleted
            )
            VALUES
            (
                @name,
                @date_of_establishment,
                @phone_number,
                @email,
                @tip,
                @description,
                @address,
                @admin,
                @is_deleted
            )
            RETURNING id;
            """;

        AddParameter(command, "@name", association.Name);
        AddParameter(command, "@date_of_establishment", association.DateOfEstabishment);
        AddParameter(command, "@phone_number", association.PhoneNumber);
        AddParameter(command, "@email", association.EmailAddress);
        AddParameter(command, "@tip", association.EstablishmentType);
        AddParameter(command, "@description", association.Description);
        AddParameter(command, "@address", association.Address);
        AddParameter(command, "@admin", association.AdminId);
        AddParameter(command, "@is_deleted", association.IsDeleted);

        return Convert.ToInt64(command.ExecuteScalar());
    }

    public int Update(Association association)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            UPDATE associations
            SET
                name = @name,
                date_of_establishment = @date_of_establishment,
                phone_number = @phone_number,
                email = @email,
                tip = @tip,
                description = @description,
                address = @address,
                admin = @admin,
                is_deleted = @is_deleted
            WHERE id = @id;
            """;

        AddParameter(command, "@id", association.Id);
        AddParameter(command, "@name", association.Name);
        AddParameter(command, "@date_of_establishment", association.DateOfEstabishment);
        AddParameter(command, "@phone_number", association.PhoneNumber);
        AddParameter(command, "@email", association.EmailAddress);
        AddParameter(command, "@tip", association.EstablishmentType);
        AddParameter(command, "@description", association.Description);
        AddParameter(command, "@address", association.Address);
        AddParameter(command, "@admin", association.AdminId);
        AddParameter(command, "@is_deleted", association.IsDeleted);

        return command.ExecuteNonQuery();
    }

    public Association? GetById(long id)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                id,
                name,
                date_of_establishment,
                phone_number,
                email,
                tip,
                description,
                address,
                admin,
                is_deleted
            FROM associations
            WHERE id = @id
              AND is_deleted = FALSE;
            """;

        AddParameter(command, "@id", id);

        using IDataReader reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        return MapAssociation(reader);
    }

    public List<Association> GetAll()
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                id,
                name,
                date_of_establishment,
                phone_number,
                email,
                tip,
                description,
                address,
                admin,
                is_deleted
            FROM associations
            WHERE is_deleted = FALSE
            ORDER BY id;
            """;

        using IDataReader reader = command.ExecuteReader();

        List<Association> associations = new();

        while (reader.Read())
        {
            associations.Add(MapAssociation(reader));
        }

        return associations;
    }

    public bool Delete(long id)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            UPDATE associations
            SET is_deleted = TRUE
            WHERE id = @id
              AND is_deleted = FALSE;
            """;

        AddParameter(command, "@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static Association MapAssociation(IDataReader reader)
    {
        return new Association(
            Convert.ToInt64(reader["id"]),
            Convert.ToString(reader["name"])!,
            DateOnly.FromDateTime(
                Convert.ToDateTime(reader["date_of_establishment"])
            ),
            Convert.ToString(reader["phone_number"])!,
            Convert.ToString(reader["email"])!,
            Convert.ToString(reader["tip"])!,
            Convert.ToString(reader["description"])!,
            Convert.ToString(reader["address"])!,
            Convert.ToInt64(reader["admin"]),
            Convert.ToBoolean(reader["is_deleted"])
        );
    }
}
