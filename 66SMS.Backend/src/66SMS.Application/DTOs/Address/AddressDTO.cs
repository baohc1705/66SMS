namespace _66SMS.Application.DTOs.Address
{
    public class AddressDTO
    {
        public Guid? Id { get; set; }
        public Guid userId { get; set; }
        public string City { get; set; } = null!;
        public string Ward { get; set; } = null!;
        public string Street { get; set; } = null!;
        public bool IsDefaultShipping { get; set; }
        public bool IsDefaultBilling { get; set; }
    }
}
