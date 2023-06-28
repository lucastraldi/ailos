using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands.Requests
{
    public class AccountMovementRequest : IRequest<AccountMovementResponse>
    {
        public string RequestId { get; set; }
        public string AccountId { get; set; }
        public MovementType Type { get; set; }
        public decimal Value { get; set; }
    }
}
