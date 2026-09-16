using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Application.Services;

public class AnimalService : IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    private readonly IAdoptionRequestService _adoptionRequestService;
    private readonly IAdoptionRepository _adoptionRequestRepository;

    public AnimalService()
    {
        _animalRepository = Injector.CreateInstance<IAnimalRepository>();
        _adoptionRequestService = Injector.CreateInstance<IAdoptionRequestService>();
        _adoptionRequestRepository = Injector.CreateInstance<IAdoptionRepository>();
    }

    public List<Animal> GetByAssociation(long associationId)
    {
        var animals = _animalRepository.GetAll().Where(a => a.AssociationId == associationId && a.IsDeleted == false)
            .ToList();
        List<Animal> results = new();
        foreach (var animal in animals)
        {
            try
            {
                var adoptionRequests = _adoptionRequestService.GetByAnimalId(animal.Id);
                if (adoptionRequests.All(r => r.AdoptionStatus == AdoptionStatus.Rejected) &&
                    !(adoptionRequests.Count == 0))
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

    public List<Animal> GetByUserId(long userId)
    {
        var adoptionRequests = _adoptionRequestRepository.GetByUserId(userId);
        List<Animal> results = new();
        
        foreach (var request in adoptionRequests)
        {
            if (request.AdoptionStatus == AdoptionStatus.Aproved || request.AdoptionStatus == AdoptionStatus.Pending)
            {
                var animal = _animalRepository.GetById(request.AnimalId);
                if (animal == null) continue;
                results.Add(animal);
            }
        }

        return results;
    }
    
    
    public List<Animal> GetAllForAssociationAdmin(long associationId)
    {
        var animals = _animalRepository
            .GetAll()
            .Where(a => a.AssociationId == associationId)
            .ToList();

        return animals
            .Where(animal =>
                !_adoptionRequestRepository
                    .GetByAnimalId(animal.Id)
                    .Any(r => r.AdoptionStatus == AdoptionStatus.Aproved))
            .ToList();
    }

    public Animal? GetById(long id)
    {
        return _animalRepository.GetById(id);
    }

    public long Insert(Animal animal)
    {
        return _animalRepository.Insert(animal);
    }

    public int Update(Animal animal)
    {
        var requests = _adoptionRequestRepository.GetByAnimalId(animal.Id);

        if (requests.Any(r => r.AdoptionStatus == AdoptionStatus.Pending))
        {
            throw new Exception(
                "Animal cannot be edited while it has a pending adoption request."
            );
        }

        return _animalRepository.Update(animal);
    }

    public bool Delete(long id)
    {
        var requests = _adoptionRequestRepository.GetByAnimalId(id);

        if (requests.Any(r => r.AdoptionStatus == AdoptionStatus.Pending))
        {
            throw new Exception(
                "Animal cannot be deleted while it has a pending adoption request."
            );
        }

        return _animalRepository.Delete(id);
    }
}