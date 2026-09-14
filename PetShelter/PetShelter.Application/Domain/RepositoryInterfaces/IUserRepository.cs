namespace PetShelter.Application.Domain.RepositoryInterfaces;

public interface IUserRepository
{
    public (long Id, Role Role)? AuthenticateUser(string email, string password);
    public long Insert(User user);
    public int Update(User user);
    public User? GetById(long id);
    public List<User> GetAll();
}