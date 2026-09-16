using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IAdoptionRequestService
{
    public List<AdoptionRequest> GetByAnimalId(long animalId);
    public void SendAddoptionRequest(long userId, long animalId);
    public List<User> GetAdopters();
    public List<AdoptionRequest> GetAllRequests();
    public void ApproveRequest(long userId, long animalId);
    public void RejectRequest(long userId, long animalId);
}