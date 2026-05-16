using _66SMS.Domain.Abstractions.Entities;

namespace _66SMS.Domain.Entities
{
    public class Address : EntityBase<Guid>
    {
        public Guid UserId { get; set; }
        public string City { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public string Street { get; set; } = null!;
        public bool IsDefaultShipping { get; set; }
        public bool IsDefaultBilling { get; set; }
    }
}
