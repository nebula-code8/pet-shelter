namespace PetShelter.Application.Domain;

public class Volunteer : User
{
   public string VolunteerComment { get; private set; }
   public VolunteerStatus Status { get; private set; }
   
   public Volunteer (string name, string surname, Gender gender, DateOnly dateOfBirth, string phoneNumber, string emailAddress, string password, Role role, string address, bool isDeleted, string comment, VolunteerStatus status)
      :base(name, surname, gender, dateOfBirth, phoneNumber, emailAddress, password, role, address, isDeleted)
   {
      VolunteerComment = comment;
      Status = status;
   }

   public Volunteer (long id, string name, string surname, Gender gender, DateOnly dateOfBirth, string phoneNumber, string emailAddress, string password, Role role, string address, bool isDeleted, string comment, VolunteerStatus status)
      :base(id, name, surname, gender, dateOfBirth, phoneNumber, emailAddress, password, role, address, isDeleted)   
   {
      VolunteerComment = comment;
      Status = status;
   }
}