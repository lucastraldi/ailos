using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class IdempotenciaRepository : IIdempotenciaRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public IdempotenciaRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<Idempotencia> GetIdempotenciaByKey(string chaveIdempotencia)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            string query = "SELECT * FROM idempotencia WHERE chave_idempotencia = @ChaveIdempotencia";
            var parameters = new { ChaveIdempotencia = chaveIdempotencia };
            return await connection.QueryFirstOrDefaultAsync<Idempotencia>(query, parameters);
        }

        public async Task<int> AddIdempotencia(Idempotencia idempotencia)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            string query = "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) " +
                           "VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)";
            var parameters = new
            {
                idempotencia.ChaveIdempotencia,
                idempotencia.Requisicao,
                idempotencia.Resultado
            };
            return await connection.ExecuteAsync(query, parameters);
        }
    }
}