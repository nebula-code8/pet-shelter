namespace PetShelter.Application.Domain;

public class AdoptionRequest
{
    public long UserId { get; private set; }
    public long AnimalId { get; private set; }
    public AdoptionStatus AdoptionStatus { get; private set; }
    
    public AdoptionRequest(long userId, long animalId, AdoptionStatus adoptionStatus)
    {
        UserId = userId;
        AnimalId = animalId;
        AdoptionStatus = adoptionStatus;
    }
}