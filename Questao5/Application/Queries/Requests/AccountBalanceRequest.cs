using MediatR;
using Questao5.Application.Queries.Responses;

namespace Questao5.Application.Queries.Requests
{
    public class AccountBalanceRequest : IRequest<AccountBalanceResponse>
    {
        public string AccountId { get; set; }
    }
}
