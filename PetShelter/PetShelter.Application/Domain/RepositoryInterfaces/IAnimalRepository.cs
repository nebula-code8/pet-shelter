namespace PetShelter.Application.Domain.RepositoryInterfaces;

public interface IAnimalRepository
{
    public long Insert(Animal animal);
    public int Update(Animal animal);
    public Animal? GetById(long id);
    public List<Animal> GetAll();
    public bool Delete(long id);
}
