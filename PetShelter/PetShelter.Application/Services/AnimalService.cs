using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Application.Services;

public class AnimalService: IAnimalService
{
    private readonly IAnimalRepository _animalRepository;
    public  AnimalService()
    {
        _animalRepository = Injector.CreateInstance<IAnimalRepository>();
    }

    public List<Animal> GetByAssociation(long associationId)
    {
        var animals = _animalRepository.GetAll();
        return animals.Where(a => a.AssociationId == associationId && a.IsDeleted == false).ToList();
    }
}