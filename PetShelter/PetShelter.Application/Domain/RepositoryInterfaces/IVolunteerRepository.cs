namespace PetShelter.Application.Domain.RepositoryInterfaces;

public interface IVolunteerRepository
{
    public long Insert(Volunteer volunteer);
    public int Update(Volunteer volunteer);
    public Volunteer? GetById(long id);
    public List<Volunteer> GetAll();
    public bool Delete(long id);
}