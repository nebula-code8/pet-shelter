namespace PetShelter.Application.Domain;

public class Animal
{
    public long Id { get; private set; }

    public string Name { get; private set; }
    public string Species { get; private set; }
    public string Breed { get; private set; }
    public Gender Gender { get; private set; }
    public DateOnly DateOfBirth { get; private set; }

    public string Description { get; private set; }

    public bool IsVaccinated { get; private set; }
    public bool IsSterilized { get; private set; }

    public string HealthStatus { get; private set; }

    public DateOnly DateArrived { get; private set; }

    public long AssociationId { get; private set; }
    public bool IsDeleted { get; private set; }

    public Animal(
        string name,
        string species,
        string breed,
        Gender gender,
        DateOnly dateOfBirth,
        string description,
        bool isVaccinated,
        bool isSterilized,
        string healthStatus,
        DateOnly dateArrived,
        long associationId,
        bool isDeleted)
    {
        Name = name;
        Species = species;
        Breed = breed;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        Description = description;
        IsVaccinated = isVaccinated;
        IsSterilized = isSterilized;
        HealthStatus = healthStatus;
        DateArrived = dateArrived;
        AssociationId = associationId;
        IsDeleted = isDeleted;
    }

    public Animal(
        long id,
        string name,
        string species,
        string breed,
        Gender gender,
        DateOnly dateOfBirth,
        string description,
        bool isVaccinated,
        bool isSterilized,
        string healthStatus,
        DateOnly dateArrived,
        long associationId,
        bool isDeleted)
    {
        Id = id;
        Name = name;
        Species = species;
        Breed = breed;
        Gender = gender;
        DateOfBirth = dateOfBirth;
        Description = description;
        IsVaccinated = isVaccinated;
        IsSterilized = isSterilized;
        HealthStatus = healthStatus;
        DateArrived = dateArrived;
        AssociationId = associationId;
        IsDeleted = isDeleted;
    }

}