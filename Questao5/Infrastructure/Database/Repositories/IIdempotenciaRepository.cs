using Questao5.Domain.Entities;

namespace Questao5.Infrastructure.Database.Repositories
{
    public interface IIdempotenciaRepository
    {
        Task<Idempotencia> GetIdempotenciaByKey(string chaveIdempotencia);

        Task<int> AddIdempotencia(Idempotencia idempotencia);
    }
}
