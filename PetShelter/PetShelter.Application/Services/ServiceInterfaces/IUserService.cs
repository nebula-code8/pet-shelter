using PetShelter.Application.Domain;

namespace PetShelter.Application.Services.ServiceInterfaces;

public interface IUserService
{
    public (long Id, Role Role)? AuthenticateUser(string email, string password);
    public void Insert(User user);
}