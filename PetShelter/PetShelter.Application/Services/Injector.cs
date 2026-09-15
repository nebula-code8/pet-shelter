using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Repository;
using PetShelter.Application.Services.ServiceInterfaces;
using PetShelter.Infrastructure.Database;

namespace PetShelter.Application.Services;

public class Injector
{
    private static readonly Dictionary<Type, Func<object>> _implementations = new()
    {
        { typeof(IAdoptionRepository), () => new AdoptionDbRepository() },
        { typeof(IAnimalRepository), () => new AnimalDbRepository() },
        { typeof(IAssociationRepository), () => new AssociationDbRepository() },
        { typeof(IUserRepository), () => new UserDbRepository() },
        { typeof(IVolunteerRepository), () => new VolunteerDbRepository() },

        { typeof(IAdoptionRequestService), () => new AdoptionRequestService() },
        { typeof(IAnimalService), () => new AnimalService() },
        { typeof(IAssociationService), () => new AssociationService() },
        { typeof(IUserService), () => new UserService() },
        { typeof(IVolunteerService), () => new VolunteerService() }
    };

    public static T CreateInstance<T>()
    {
        Type type = typeof(T);

        if (_implementations.TryGetValue(type, out var factory))
        {
            return (T)factory();
        }

        throw new ArgumentException($"No implementation found for type {type}");
    }
}