using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Requests;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Repositories;

namespace Questao5.Application.Handlers
{
    public class AccountBalanceHandler : IRequestHandler<AccountBalanceRequest, AccountBalanceResponse>
    {
        IAccountBalanceRepository _accountBalanceRepository;

        public AccountBalanceHandler(IAccountBalanceRepository accountBalanceRepository)
        {
            _accountBalanceRepository = accountBalanceRepository;
        }

        public async Task<AccountBalanceResponse> Handle(AccountBalanceRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var balance = await _accountBalanceRepository.GetBalance(request.AccountId);
                var account = await _accountBalanceRepository.GetAccount(request.AccountId);

                return new AccountBalanceResponse
                {
                    Balance = balance,  
                    Date = DateTime.Now,
                    Name = account.Nome,
                    Number = account.Numero,
                };
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
