using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Application.Services;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    
    public UserService()
    {
        _userRepository = Injector.CreateInstance<IUserRepository>();
    }
    
    public long Insert(User user)
    {
        if (string.IsNullOrWhiteSpace(user.Name) ||
            string.IsNullOrWhiteSpace(user.Surname) ||
            string.IsNullOrWhiteSpace(user.PhoneNumber) ||
            string.IsNullOrWhiteSpace(user.EmailAddress) ||
            string.IsNullOrWhiteSpace(user.Address) ||
            string.IsNullOrWhiteSpace(user.Password))
        {
            throw new Exception("Please fill in all fields");
        }

        try
        {
            return _userRepository.Insert(user);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);

            throw new Exception(
                "Registration failed. The email may already be in use."
            );
        }
    }

    public (long Id, Role Role)? AuthenticateUser(string email, string password)
    {
        var result = _userRepository.AuthenticateUser(email, password);
        return result;
    }
    
    public List<User> GetAll()
    {
        return _userRepository.GetAll();
    }
    
    public bool ExistsByEmail(string email)
    {
        return _userRepository.ExistsByEmail(email);
    }
}