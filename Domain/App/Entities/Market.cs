using SuperMarket.Domain.Abstractions;

namespace SuperMarket.Domain
{
    public class Market : AuditableEntity
    {
        public string Name { get; set; }
    }
}