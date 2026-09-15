namespace PetShelter.Application.Domain;

public class Volunteer : User
{
    public long AssociationId { get; private set; }
    public string VolunteerComment { get; private set; }
    public VolunteerStatus Status { get; private set; }

    public Volunteer(
        string name,
        string surname,
        Gender gender,
        DateOnly dateOfBirth,
        string phoneNumber,
        string emailAddress,
        string password,
        Role role,
        string address,
        bool isDeleted,
        long associationId,
        string comment,
        VolunteerStatus status)
        : base(
            name,
            surname,
            gender,
            dateOfBirth,
            phoneNumber,
            emailAddress,
            password,
            role,
            address,
            isDeleted)
    {
        AssociationId = associationId;
        VolunteerComment = comment;
        Status = status;
    }

    public Volunteer(
        long id,
        string name,
        string surname,
        Gender gender,
        DateOnly dateOfBirth,
        string phoneNumber,
        string emailAddress,
        string password,
        Role role,
        string address,
        bool isDeleted,
        long associationId,
        string comment,
        VolunteerStatus status)
        : base(
            id,
            name,
            surname,
            gender,
            dateOfBirth,
            phoneNumber,
            emailAddress,
            password,
            role,
            address,
            isDeleted)
    {
        AssociationId = associationId;
        VolunteerComment = comment;
        Status = status;
    }
}