using Questao5.Domain.Enumerators;

namespace Questao5.Domain.Entities
{
    public class Movement
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public DateTime Date { get; set; }
        public MovementType Type { get; set; }
        public decimal Value { get; set; }
    }
}
