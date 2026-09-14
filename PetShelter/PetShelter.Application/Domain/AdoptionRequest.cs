namespace PetShelter.Application.Domain;

public class AdoptionRequest
{
    public long UserId { get; private set; }
    public long AnimalId { get; private set; }
    public AdoptionStatus AdoptionStatus { get; private set; }
    public DateOnly AdoptionDate { get; private set; }
    public DateOnly RequestDate { get; private set; }
    
    public AdoptionRequest(long userId, long animalId, AdoptionStatus adoptionStatus, DateOnly adoptionDate, DateOnly requestDate)
    {
        UserId = userId;
        AnimalId = animalId;
        AdoptionStatus = adoptionStatus;
        AdoptionDate = adoptionDate;
        RequestDate = requestDate;
    }
}