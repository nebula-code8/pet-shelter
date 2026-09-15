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

    public long Insert(Association association)
    {
        Validate(association);

        return _associationRepository.Insert(association);
    }

    public void Update(Association association)
    {
        Validate(association);

        if (_associationRepository.Update(association) == 0)
        {
            throw new Exception("Association was not found.");
        }
    }

    public Association? GetById(long id)
    {
        return _associationRepository.GetById(id);
    }
    
    public Association? GetByAdminId(long adminId)
    {
        return _associationRepository.GetByAdminId(adminId);
    }

    public List<Association> GetAll()
    {
        return _associationRepository.GetAll().Where(a => a.IsDeleted == false).ToList();
    }

    public void Delete(long id)
    {
        if (!_associationRepository.Delete(id))
        {
            throw new Exception("Association was not found.");
        }
    }

    private static void Validate(Association association)
    {
        if (string.IsNullOrWhiteSpace(association.Name) ||
            string.IsNullOrWhiteSpace(association.PhoneNumber) ||
            string.IsNullOrWhiteSpace(association.EmailAddress) ||
            string.IsNullOrWhiteSpace(association.EstablishmentType) ||
            string.IsNullOrWhiteSpace(association.Description) ||
            string.IsNullOrWhiteSpace(association.Address))
        {
            throw new Exception("Please fill in all fields.");
        }
    }
}