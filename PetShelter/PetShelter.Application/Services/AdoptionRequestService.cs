using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Application.Services;

public class AdoptionRequestService : IAdoptionRequestService
{
    private readonly IAdoptionRepository _adoptionRepository;
    public AdoptionRequestService()
    {
        _adoptionRepository = Injector.CreateInstance<IAdoptionRepository>();
    }
    
    public List<AdoptionRequest> GetByAnimalId(long animalId)
    {
        var adoptionRequests = _adoptionRepository.GetByAnimalId(animalId);
        return (adoptionRequests.Count == 0) ? throw new Exception("No adoption requests found") : adoptionRequests;
    }
}