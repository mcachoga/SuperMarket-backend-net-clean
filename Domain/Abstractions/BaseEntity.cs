namespace SuperMarket.Domain.Abstractions
{
    public abstract class BaseEntity : IBaseEntity
    {
        public int Id { get; set; }
    }
}