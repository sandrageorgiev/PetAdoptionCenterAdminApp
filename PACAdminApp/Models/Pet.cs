using PACAdminApp.Models;
using PACAdminApp.Models.enums;
using System.Diagnostics.Eventing.Reader;

namespace PACAdminApp.Models
{
    public class Pet
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int? Age { get; set; }
        public PetType? PetType { get; set; }
        public string? Breed { get; set; }
        public string? Sex { get; set; }
        public string? Description { get; set; }
        public bool IsHouseTrained { get; set; }
        public string? FavouriteThings { get; set; }
        public string? HomeRequirements { get; set; }
        public string? PhotoUrl { get; set; }
        public PetStatus? PetStatus { get; set; }
        public string? ShelterId { get; set; }
        public int Price { get; set; } = 20;
        public PetAdoptionCenterUser? Shelter { get; set; }
    }
}
