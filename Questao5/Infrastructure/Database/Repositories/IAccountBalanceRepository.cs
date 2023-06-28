using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repositories
{
    public interface IAccountBalanceRepository
    {
        Task<int> AddMovement(Movement movimento);

        Task<decimal> GetBalance(string idContaCorrente);

        Task<Account> GetAccount(string idContaCorrente);
    }
}
