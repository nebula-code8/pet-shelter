using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IAnimalService
{
    public List<Animal> GetByAssociation(long associationId);
    public List<Animal> GetByUserId(long userId);
    
    public List<Animal> GetAllForAssociationAdmin(long associationId);
    public Animal? GetById(long id);
    public long Insert(Animal animal);
    public int Update(Animal animal);
    public bool Delete(long id);
}