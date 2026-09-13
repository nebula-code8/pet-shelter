namespace PetShelter.Application.Domain;

public class User
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public string Surname { get; private set; }
    public Gender Gender { get; private set; }
    public DateOnly DateOfBirth { get; private set; }
    public string PhoneNumber { get; private set; }
    public string EmailAddress { get; private set; }
    public string Password { get; private set; }
    public Role Role { get; private set; }
    public string Address { get; private set; }

    public User(string name, string surname, Gender gender, DateOnly dateOfBirth, string phoneNumber, string emailAddress, string password, Role role, string address)
    {
        Name = name;
        Surname = surname;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        Password = password;
        Role = role;
        Address = address;
    }

    public User(long id, string name, string surname, Gender gender, DateOnly dateOfBirth, string phoneNumber, string emailAddress, string password, Role role, string address)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        Password = password;
        Role = role;
        Address = address;
    }
}