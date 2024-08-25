using PACAdminApp.Models;

namespace PACAdminApp.Models
{
    public class PetAdoptionCenterUser
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; }

        public virtual ICollection<Pet>? Pets { get; set; }
    }
}
