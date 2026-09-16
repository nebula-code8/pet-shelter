using System.Data;
using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;

namespace PetShelter.Application.Repository;

public class AnimalDbRepository : BaseDbRepository, IAnimalRepository
{
    public long Insert(Animal animal)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              INSERT INTO animals
                              (
                                  name,
                                  species,
                                  breed,
                                  gender,
                                  date_of_birth,
                                  description,
                                  is_vaccinated,
                                  is_sterilized,
                                  health_status,
                                  date_arrived,
                                  association_id,
                                  is_deleted
                              )
                              VALUES
                              (
                                  @name,
                                  @species,
                                  @breed,
                                  @gender,
                                  @date_of_birth,
                                  @description,
                                  @is_vaccinated,
                                  @is_sterilized,
                                  @health_status,
                                  @date_arrived,
                                  @association_id,
                                  @is_deleted
                              )
                              RETURNING id;
                              """;

        AddParameter(command, "@name", animal.Name);
        AddParameter(command, "@species", animal.Species);
        AddParameter(command, "@breed", animal.Breed);
        AddParameter(command, "@gender", (int)animal.Gender);
        AddParameter(command, "@date_of_birth", animal.DateOfBirth);
        AddParameter(command, "@description", animal.Description);
        AddParameter(command, "@is_vaccinated", animal.IsVaccinated);
        AddParameter(command, "@is_sterilized", animal.IsSterilized);
        AddParameter(command, "@health_status", animal.HealthStatus);
        AddParameter(command, "@date_arrived", animal.DateArrived);
        AddParameter(command, "@association_id", animal.AssociationId);
        AddParameter(command, "@is_deleted", animal.IsDeleted);

        return Convert.ToInt64(command.ExecuteScalar());
    }

    public List<Animal> GetAvailableForAssociation(long associationId)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = @"SELECT
                                  id,
                                  name,
                                  species,
                                  breed,
                                  gender,
                                  date_of_birth,
                                  description,
                                  is_vaccinated,
                                  is_sterilized,
                                  health_status,
                                  date_arrived,
                                  association_id,
                                  is_deleted
                              FROM animals a
                              WHERE a.is_deleted = FALSE
                                AND a.association_id = @assocId
                                AND NOT EXISTS (
                                    SELECT 1 FROM temporary_adoptions t
                                    WHERE t.animal_id = a.id
                                      AND t.end_date IS NULL
                                );";

        AddParameter(command, "@assocId", associationId);

        using IDataReader reader = command.ExecuteReader();
        List<Animal> list = new();
        while (reader.Read())
        {
            list.Add(MapAnimal(reader));
        }

        return list;
    }

    public int Update(Animal animal)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              UPDATE animals
                              SET
                                  name = @name,
                                  species = @species,
                                  breed = @breed,
                                  gender = @gender,
                                  date_of_birth = @date_of_birth,
                                  description = @description,
                                  is_vaccinated = @is_vaccinated,
                                  is_sterilized = @is_sterilized,
                                  health_status = @health_status,
                                  date_arrived = @date_arrived,
                                  association_id = @association_id,
                                  is_deleted = @is_deleted
                              WHERE id = @id;
                              """;

        AddParameter(command, "@id", animal.Id);
        AddParameter(command, "@name", animal.Name);
        AddParameter(command, "@species", animal.Species);
        AddParameter(command, "@breed", animal.Breed);
        AddParameter(command, "@gender", (int)animal.Gender);
        AddParameter(command, "@date_of_birth", animal.DateOfBirth);
        AddParameter(command, "@description", animal.Description);
        AddParameter(command, "@is_vaccinated", animal.IsVaccinated);
        AddParameter(command, "@is_sterilized", animal.IsSterilized);
        AddParameter(command, "@health_status", animal.HealthStatus);
        AddParameter(command, "@date_arrived", animal.DateArrived);
        AddParameter(command, "@association_id", animal.AssociationId);
        AddParameter(command, "@is_deleted", animal.IsDeleted);

        return command.ExecuteNonQuery();
    }

    public Animal? GetById(long id)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              SELECT
                                  id,
                                  name,
                                  species,
                                  breed,
                                  gender,
                                  date_of_birth,
                                  description,
                                  is_vaccinated,
                                  is_sterilized,
                                  health_status,
                                  date_arrived,
                                  association_id,
                                  is_deleted
                              FROM animals
                              WHERE id = @id
                                AND is_deleted = FALSE;
                              """;

        AddParameter(command, "@id", id);

        using IDataReader reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        return MapAnimal(reader);
    }

    public List<Animal> GetAll()
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              SELECT
                                  id,
                                  name,
                                  species,
                                  breed,
                                  gender,
                                  date_of_birth,
                                  description,
                                  is_vaccinated,
                                  is_sterilized,
                                  health_status,
                                  date_arrived,
                                  association_id,
                                  is_deleted
                              FROM animals
                              WHERE is_deleted = FALSE
                              ORDER BY id;
                              """;

        using IDataReader reader = command.ExecuteReader();

        List<Animal> animals = new();

        while (reader.Read())
        {
            animals.Add(MapAnimal(reader));
        }

        return animals;
    }

    public bool Delete(long id)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              UPDATE animals
                              SET is_deleted = TRUE
                              WHERE id = @id
                                AND is_deleted = FALSE;
                              """;

        AddParameter(command, "@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static Animal MapAnimal(IDataReader reader)
    {
        return new Animal(
            Convert.ToInt64(reader["id"]),
            Convert.ToString(reader["name"])!,
            Convert.ToString(reader["species"])!,
            Convert.ToString(reader["breed"])!,
            (Gender)Convert.ToInt32(reader["gender"]),
            (DateOnly)reader["date_of_birth"],
            Convert.ToString(reader["description"])!,
            Convert.ToBoolean(reader["is_vaccinated"]),
            Convert.ToBoolean(reader["is_sterilized"]),
            Convert.ToString(reader["health_status"])!,
            (DateOnly)reader["date_arrived"],
            Convert.ToInt64(reader["association_id"]),
            Convert.ToBoolean(reader["is_deleted"])
        );
    }
}