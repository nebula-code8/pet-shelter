using PetShelter.Application.Domain;
using PetShelter.Application.Services;
using PetShelter.Application.Services.ServiceInterfaces;

namespace PetShelter.Uix.ViewModels;

public class AssociationAdminViewModel
{
    private readonly IAssociationService _associationService;

    public long UserId { get; }
    public Association? Association { get; }

    public string AssociationName =>
        Association?.Name ?? "Association";

    public string ErrorMessage { get; }

    public AssociationAdminViewModel(long userId)
    {
        UserId = userId;

        _associationService = Injector.CreateInstance<IAssociationService>();

        try
        {
            Association = _associationService.GetByAdminId(userId);

            if (Association == null)
            {
                ErrorMessage =
                    "No association is assigned to this administrator.";
            }
            else
            {
                ErrorMessage = "";
            }
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
        }
    }
}