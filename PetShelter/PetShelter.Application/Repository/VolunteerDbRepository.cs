using System.Data;
using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Repository;

namespace PetShelter.Infrastructure.Database;

public class VolunteerDbRepository : BaseDbRepository, IVolunteerRepository
{
    private readonly UserDbRepository _userRepository = new();

    public long Insert(Volunteer volunteer)
{
    using IDbConnection connection = CreateConnection();

    IDbCommand userCommand = connection.CreateCommand();

    userCommand.CommandText = """
                              INSERT INTO users
                              (
                                  name,
                                  surname,
                                  gender,
                                  date_of_birth,
                                  phone_number,
                                  email,
                                  password,
                                  role,
                                  address,
                                  is_deleted
                              )
                              VALUES
                              (
                                  @name,
                                  @surname,
                                  @gender,
                                  @date_of_birth,
                                  @phone_number,
                                  @email,
                                  @password,
                                  @role,
                                  @address,
                                  @is_deleted
                              )
                              RETURNING id;
                              """;

    AddParameter(userCommand, "@name", volunteer.Name);
    AddParameter(userCommand, "@surname", volunteer.Surname);
    AddParameter(userCommand, "@gender", (int)volunteer.Gender);
    AddParameter(userCommand, "@date_of_birth", volunteer.DateOfBirth);
    AddParameter(userCommand, "@phone_number", volunteer.PhoneNumber);
    AddParameter(userCommand, "@email", volunteer.EmailAddress);
    AddParameter(userCommand, "@password", volunteer.Password);
    AddParameter(userCommand, "@role", (int)volunteer.Role);
    AddParameter(userCommand, "@address", volunteer.Address);
    AddParameter(userCommand, "@is_deleted", volunteer.IsDeleted);

    long userId = Convert.ToInt64(
        userCommand.ExecuteScalar()
    );

    IDbCommand volunteerCommand = connection.CreateCommand();

    volunteerCommand.CommandText = """
                                   INSERT INTO volunteers
                                   (
                                       user_id,
                                       association_id,
                                       comment,
                                       status
                                   )
                                   VALUES
                                   (
                                       @user_id,
                                       @association_id,
                                       @comment,
                                       @status
                                   );
                                   """;

    AddParameter(volunteerCommand, "@user_id", userId);
    AddParameter(
        volunteerCommand,
        "@association_id",
        volunteer.AssociationId
    );
    AddParameter(
        volunteerCommand,
        "@comment",
        volunteer.VolunteerComment
    );
    AddParameter(
        volunteerCommand,
        "@status",
        (int)volunteer.Status
    );

    volunteerCommand.ExecuteNonQuery();

    return userId;
}

    public int Update(Volunteer volunteer)
    {
        using IDbConnection connection = CreateConnection();

        // Update users table
        IDbCommand userCommand = connection.CreateCommand();

        userCommand.CommandText = """
                                  UPDATE users
                                  SET
                                      name = @name,
                                      surname = @surname,
                                      gender = @gender,
                                      date_of_birth = @date_of_birth,
                                      phone_number = @phone_number,
                                      email = @email,
                                      password = @password,
                                      role = @role,
                                      address = @address,
                                      is_deleted = @is_deleted
                                  WHERE id = @id;
                                  """;

        AddParameter(userCommand, "@id", volunteer.Id);
        AddParameter(userCommand, "@name", volunteer.Name);
        AddParameter(userCommand, "@surname", volunteer.Surname);
        AddParameter(userCommand, "@gender", (int)volunteer.Gender);
        AddParameter(userCommand, "@date_of_birth", volunteer.DateOfBirth);
        AddParameter(userCommand, "@phone_number", volunteer.PhoneNumber);
        AddParameter(userCommand, "@email", volunteer.EmailAddress);
        AddParameter(userCommand, "@password", volunteer.Password);
        AddParameter(userCommand, "@role", (int)volunteer.Role);
        AddParameter(userCommand, "@address", volunteer.Address);
        AddParameter(userCommand, "@is_deleted", volunteer.IsDeleted);

        int userRows = userCommand.ExecuteNonQuery();

        // Update volunteers table
        IDbCommand volunteerCommand = connection.CreateCommand();

        volunteerCommand.CommandText = """
                                       UPDATE volunteers
                                       SET
                                           association_id = @association_id,
                                           comment = @comment,
                                           status = @status
                                       WHERE user_id = @user_id;
                                       """;

        AddParameter(volunteerCommand, "@user_id", volunteer.Id);
        AddParameter(volunteerCommand, "@association_id", volunteer.AssociationId);
        AddParameter(volunteerCommand, "@comment", volunteer.VolunteerComment);
        AddParameter(volunteerCommand, "@status", (int)volunteer.Status);

        int volunteerRows = volunteerCommand.ExecuteNonQuery();

        return volunteerRows;
    }
    
    public bool UpdateStatus(long volunteerId, VolunteerStatus status)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              UPDATE volunteers
                              SET status = @status
                              WHERE user_id = @volunteerId;
                              """;

        AddParameter(
            command,
            "@status",
            (int)status
        );

        AddParameter(
            command,
            "@volunteerId",
            volunteerId
        );

        return command.ExecuteNonQuery() > 0;
    }

    public Volunteer? GetById(long id)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              SELECT
                                  u.id,
                                  u.name,
                                  u.surname,
                                  u.gender,
                                  u.date_of_birth,
                                  u.phone_number,
                                  u.email,
                                  u.password,
                                  u.role,
                                  u.address,
                                  u.is_deleted,
                                  v.association_id,
                                  v.comment,
                                  v.status
                              FROM users u
                              INNER JOIN volunteers v
                                  ON u.id = v.user_id
                              WHERE u.id = @id
                                AND u.is_deleted = FALSE;
                              """;

        AddParameter(command, "@id", id);
        using IDataReader reader = command.ExecuteReader();
        if (!reader.Read())
            return null;

        return MapVolunteer(reader);
    }
    
    public List<Volunteer> GetByAssociationId(long associationId)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              SELECT
                                  u.id,
                                  u.name,
                                  u.surname,
                                  u.gender,
                                  u.date_of_birth,
                                  u.phone_number,
                                  u.email,
                                  u.password,
                                  u.role,
                                  u.address,
                                  u.is_deleted,
                                  v.association_id,
                                  v.comment,
                                  v.status
                              FROM users u
                              INNER JOIN volunteers v
                                  ON u.id = v.user_id
                              WHERE v.association_id = @associationId
                                AND u.is_deleted = FALSE
                              ORDER BY u.id;
                              """;

        AddParameter(
            command,
            "@associationId",
            associationId
        );

        using IDataReader reader = command.ExecuteReader();
        List<Volunteer> volunteers = new();
        while (reader.Read())
        {
            volunteers.Add(MapVolunteer(reader));
        }

        return volunteers;
    }

    public List<Volunteer> GetAll()
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              SELECT
                                  u.id,
                                  u.name,
                                  u.surname,
                                  u.gender,
                                  u.date_of_birth,
                                  u.phone_number,
                                  u.email,
                                  u.address,
                                  u.password,
                                  u.role,
                                  u.is_deleted,
                                  v.association_id,
                                  v.comment,
                                  v.status
                              FROM users u
                              JOIN volunteers v ON u.id = v.user_id
                              WHERE u.is_deleted = FALSE;
                              """;

        using IDataReader reader = command.ExecuteReader();

        List<Volunteer> volunteers = new();

        while (reader.Read())
        {
            volunteers.Add(MapVolunteer(reader));
        }

        return volunteers;
    }

    public bool Delete(long id)
    {
        using IDbConnection connection = CreateConnection();
        IDbCommand command = connection.CreateCommand();

        command.CommandText = """
                              UPDATE users
                              SET is_deleted = TRUE
                              WHERE id = @id
                                AND is_deleted = FALSE;
                              """;

        AddParameter(command, "@id", id);

        return command.ExecuteNonQuery() > 0;
    }

    private static Volunteer MapVolunteer(IDataReader reader)
    {
        return new Volunteer(
            Convert.ToInt64(reader["id"]),
            Convert.ToString(reader["name"])!,
            Convert.ToString(reader["surname"])!,
            (Gender)Convert.ToInt32(reader["gender"]),
            (DateOnly)reader["date_of_birth"],
            Convert.ToString(reader["phone_number"])!,
            Convert.ToString(reader["email"])!,
            Convert.ToString(reader["password"])!,
            (Role)Convert.ToInt32(reader["role"]),
            Convert.ToString(reader["address"])!,
            Convert.ToBoolean(reader["is_deleted"]),
            Convert.ToInt64(reader["association_id"]),
            Convert.ToString(reader["comment"])!,
            (VolunteerStatus)Convert.ToInt32(reader["status"])
        );
    }
}