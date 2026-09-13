namespace PetShelter.Application.Domain;

public class Association
{
    public long Id { get; private set; }
    public string Name { get; private set; }
    public DateOnly DateOfEstabishment { get; private set; }
    public string PhoneNumber { get; private set; }
    public string EmailAddress { get; private set; }
    public string Password { get; private set; }
    public string EstablishmentType { get; private set; }
    public string Description { get; private set; }
    public string Address { get; private set; }
    public long AdminId { get; private set; }
    
    public Association(
        string name,
        DateOnly dateOfEstabishment,
        string phoneNumber,
        string emailAddress,
        string password,
        string establishmentType,
        string description,
        string address,
        long adminId)
    {
        Name = name;
        DateOfEstabishment = dateOfEstabishment;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        Password = password;
        EstablishmentType = establishmentType;
        Description = description;
        Address = address;
        AdminId = adminId;
    }

    public Association(
        long id,
        string name,
        DateOnly dateOfEstabishment,
        string phoneNumber,
        string emailAddress,
        string password,
        string establishmentType,
        string description,
        string address,
        long adminId)
    {
        Id = id;
        Name = name;
        DateOfEstabishment = dateOfEstabishment;
        PhoneNumber = phoneNumber;
        EmailAddress = emailAddress;
        Password = password;
        EstablishmentType = establishmentType;
        Description = description;
        Address = address;
        AdminId = adminId;
    }
}