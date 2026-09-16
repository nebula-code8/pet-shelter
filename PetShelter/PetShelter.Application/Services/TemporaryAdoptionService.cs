using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Application.Services;

public class TemporaryAdoptionService : ITemporaryAdoptionService
{
    private readonly ITemporaryAdoptionRepository _tempRepo;

    public TemporaryAdoptionService()
    {
        _tempRepo = Injector.CreateInstance<ITemporaryAdoptionRepository>();
    }

    public void AddTemporaryAdoption(long userId, long animalId)
    {
        var temp = new TemporaryAdoption(userId, animalId, DateOnly.FromDateTime(DateTime.Now), null);
        _tempRepo.Insert(temp);
    }

    public List<TemporaryAdoption> GetByUserId(long userId)
    {
        return _tempRepo.GetByUserId(userId);
    }

    public List<TemporaryAdoption> GetAll()
    {
        return _tempRepo.GetAll();
    }

    public List<TemporaryAdoption> GetAllForAssociation(long associationId)
    {
        return _tempRepo.GetAllForAssociation(associationId);
    }

    public bool RemoveTemporaryAdoption(long userId, long animalId, DateOnly startDate)
    {
        return _tempRepo.Delete(userId, animalId, startDate);
    }
}
