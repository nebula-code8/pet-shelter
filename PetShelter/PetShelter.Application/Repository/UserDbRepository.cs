using System.Data;
using PetShelter.Application.Domain;

namespace PetShelter.Application.Repository;

public class UserDbRepository : BaseDbRepository
{
    public (long Id, Role Role)? AuthenticateUser(string email, string password)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            SELECT id, role
            FROM users
            WHERE email = @email
              AND password = @password
            """;

        AddParameter(command, "@email", email);
        AddParameter(command, "@password", password);

        using IDataReader reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        long id = Convert.ToInt64(reader["id"]);
        Role role = (Role)Convert.ToInt32(reader["role"]);

        return (id, role);
    }

    public long Insert(User user)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            INSERT INTO users
            (
                name,
                surname,
                gender,
                date_of_birth,
                phone_number,
                email,
                password,
                role
            )
            VALUES
            (
                @name,
                @surname,
                @gender,
                @date_of_birth,
                @phoneNumber,
                @email,
                @password,
                @role
            )
            RETURNING id;
            """;

        AddParameter(command, "@name", user.Name);
        AddParameter(command, "@surname", user.Surname);
        AddParameter(command, "@gender", (int)user.Gender);
        AddParameter(command, "@date_of_birth", user.DateOfBirth);
        AddParameter(command, "@phoneNumber", user.PhoneNumber);
        AddParameter(command, "@email", user.EmailAddress);
        AddParameter(command, "@password", user.Password);
        AddParameter(command, "@role", (int)user.Role);

        return Convert.ToInt64(command.ExecuteScalar());
    }

    public int Update(User user)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            UPDATE users
            SET
                name = @name,
                surname = @surname,
                gender = @gender,
                date_of_birth = @date_of_birth,
                phone_number = @phoneNumber,
                email = @email,
                password = @password,
                role = @role
            WHERE id = @id;
            """;

        AddParameter(command, "@name", user.Name);
        AddParameter(command, "@surname", user.Surname);
        AddParameter(command, "@gender", (int)user.Gender);
        AddParameter(command, "@date_of_birth", user.DateOfBirth);
        AddParameter(command, "@phoneNumber", user.PhoneNumber);
        AddParameter(command, "@email", user.EmailAddress);
        AddParameter(command, "@password", user.Password);
        AddParameter(command, "@role", (int)user.Role);
        AddParameter(command, "@id", user.Id);

        return command.ExecuteNonQuery();
    }

    public User? GetById(long id)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                id,
                name,
                surname,
                gender,
                date_of_birth,
                phone_number,
                email,
                password,
                role
            FROM users
            WHERE id = @id;
            """;

        AddParameter(command, "@id", id);

        using IDataReader reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        return MapUser(reader);
    }

    public List<User> GetAll()
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            SELECT
                id,
                name,
                surname,
                gender,
                date_of_birth,
                phone_number,
                email,
                password,
                role
            FROM users
            ORDER BY id;
            """;

        using IDataReader reader = command.ExecuteReader();

        List<User> users = new();

        while (reader.Read())
        {
            users.Add(MapUser(reader));
        }

        return users;
    }

    public int Delete(long id)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
            DELETE FROM users
            WHERE id = @id;
            """;

        AddParameter(command, "@id", id);

        return command.ExecuteNonQuery();
    }

    private static User MapUser(IDataReader reader)
    {
        return new User(
            Convert.ToInt64(reader["id"]),
            Convert.ToString(reader["name"])!,
            Convert.ToString(reader["surname"])!,
            (Gender)Convert.ToInt32(reader["gender"]),
            DateOnly.FromDateTime(Convert.ToDateTime(reader["date_of_birth"])),
            Convert.ToString(reader["phone_number"])!,
            Convert.ToString(reader["email"])!,
            Convert.ToString(reader["password"])!,
            (Role)Convert.ToInt32(reader["role"]),
            Convert.ToString(reader["address"])
        );
    }
}