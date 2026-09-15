using PetShelter.Application.Domain;
using PetShelter.Application.Domain.RepositoryInterfaces;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Application.Services;

public class AssociationService : IAssociationService
{
    private readonly IAssociationRepository _associationRepository;

    public AssociationService()
    {
        _associationRepository = Injector.CreateInstance<IAssociationRepository>();
    }

    public List<Association> GetAll()
    {
        return _associationRepository.GetAll().Where(a => a.IsDeleted == false).ToList();
    }
}