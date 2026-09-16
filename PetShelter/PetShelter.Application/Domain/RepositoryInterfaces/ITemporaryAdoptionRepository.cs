using PetShelter.Application.Domain;

namespace PetShelter.Application.Domain.RepositoryInterfaces;

public interface ITemporaryAdoptionRepository
{
    public void Insert(TemporaryAdoption tempAdoption);
    public List<TemporaryAdoption> GetByUserId(long userId);
    public List<TemporaryAdoption> GetByAnimalId(long animalId);
    public List<TemporaryAdoption> GetAll();
    List<TemporaryAdoption> GetAllForAssociation(long associationId);
    public bool Delete(long userId, long animalId, DateOnly startDate);
}
