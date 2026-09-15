using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IAnimalService
{
    public List<Animal> GetByAssociation(long associationId);
    public List<Animal> GetByUserId(long userId);
}