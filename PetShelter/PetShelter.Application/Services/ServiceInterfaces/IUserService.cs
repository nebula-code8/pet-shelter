using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IUserService
{
    (long Id, Role Role)? AuthenticateUser(string email, string password);
    long Insert(User user);
    List<User> GetAll();
    User GetById(long id);
    bool ExistsByEmail(string email);
}