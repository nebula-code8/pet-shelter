using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Repository;
using PetShelter.Infrastructure.Database;

namespace PetShelter.Application.Services;

public class Injector
{
    private static Dictionary<Type, object> _implementations = new Dictionary<Type, object>
    {
        { typeof(IAdoptionRepository), new AdoptionDbRepository() },
        { typeof(IAnimalRepository), new AnimalDbRepository() },
        { typeof(IAssociationRepository), new AssociationDbRepository() },
        { typeof(IUserRepository), new UserDbRepository() },
        { typeof(IVolunteerRepository), new VolunteerDbRepository() },
    };
    
    public static T CreateInstance<T>()
    {
        Type type = typeof(T);

        if (_implementations.ContainsKey(type))
        {
            return (T)_implementations[type];
        }

        throw new ArgumentException($"No implementation found for type {type}");
    }
}