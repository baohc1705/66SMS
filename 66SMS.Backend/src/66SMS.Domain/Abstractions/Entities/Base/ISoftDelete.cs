namespace _66SMS.Domain.Abstractions.Entities.Base
{
    public interface ISoftDelete
    {
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
