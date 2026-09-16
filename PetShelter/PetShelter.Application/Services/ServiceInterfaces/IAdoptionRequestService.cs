using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IAdoptionRequestService
{
    public List<AdoptionRequest> GetByAnimalId(long animalId);
    public void SendAddoptionRequest(long userId, long animalId);
}