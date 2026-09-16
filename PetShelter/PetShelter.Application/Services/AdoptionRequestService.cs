using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services.ServiceInterfaces;
using System.Linq;

namespace PetShelter.Application.Services;

public class AdoptionRequestService : IAdoptionRequestService
{
    private readonly IAdoptionRepository _adoptionRepository;

    public AdoptionRequestService()
    {
        _adoptionRepository = Injector.CreateInstance<IAdoptionRepository>();
    }

    public List<AdoptionRequest> GetByAnimalId(long animalId)
    {
        var adoptionRequests = _adoptionRepository.GetByAnimalId(animalId);
        return (adoptionRequests.Count == 0) ? throw new Exception("No adoption requests found") : adoptionRequests;
    }

    public void SendAddoptionRequest(long userId, long animalId)
    {
        var request = new AdoptionRequest(userId, animalId, AdoptionStatus.Pending, null,
            DateOnly.FromDateTime(DateTime.Now));
        try
        {
            _adoptionRepository.Insert(request);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }

    public List<User> GetAdopters()
    {
        var requests = _adoptionRepository.GetAll();
        var approvedUserIds = requests.Where(r => r.AdoptionStatus == AdoptionStatus.Aproved)
            .Select(r => r.UserId)
            .Distinct();

        var userService = Injector.CreateInstance<IUserService>();

        List<User> adopters = new();
        foreach (var id in approvedUserIds)
        {
            try
            {
                var user = userService.GetById(id);
                adopters.Add(user);
            }
            catch
            {
                // ignore missing users
            }
        }

        return adopters;
    }
}