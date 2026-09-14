using System.Data;

namespace PetShelter.Application.Domain.RepositoryInterfaces;

public interface IAdoptionRepository
{
    public void Insert(AdoptionRequest request);
    public int Update(AdoptionRequest request);
    public AdoptionRequest? GetById(long userId, long animalId);
    public List<AdoptionRequest> GetAll();
    public bool Delete(long userId, long animalId);
}