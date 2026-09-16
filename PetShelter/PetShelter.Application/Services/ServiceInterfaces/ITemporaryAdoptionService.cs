using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface ITemporaryAdoptionService
{
    public void AddTemporaryAdoption(long userId, long animalId);
    public List<TemporaryAdoption> GetByUserId(long userId);
    public List<TemporaryAdoption> GetAll();
    List<TemporaryAdoption> GetAllForAssociation(long associationId);
    public bool RemoveTemporaryAdoption(long userId, long animalId, DateOnly startDate);
}
