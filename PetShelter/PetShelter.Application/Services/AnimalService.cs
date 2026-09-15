using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IAdoptionRequestService _adoptionRequestService;

    public AnimalService()
    {
        _animalRepository = Injector.CreateInstance<IAnimalRepository>();
        _adoptionRequestService = Injector.CreateInstance<IAdoptionRequestService>();
    }

    public List<Animal> GetByAssociation(long associationId)
    {
        var animals = _animalRepository.GetAll().Where(a => a.AssociationId == associationId && a.IsDeleted == false).ToList();
        List<Animal> results = new();
        foreach (var animal in animals)
        {
            try
            {
                var adoptionRequests = _adoptionRequestService.GetByAnimalId(animal.Id);
                bool canAdd = true;
                if (adoptionRequests.All(r => r.AdoptionStatus == AdoptionStatus.Rejected) && !(adoptionRequests.Count == 0))
                {
                    //Console.WriteLine(adoptionRequests[0].AdoptionStatus);
                    results.Add(animal);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(animal.Id + " " + ex.Message);
                results.Add(animal);
            }
            
        }
        return results;
    }
}