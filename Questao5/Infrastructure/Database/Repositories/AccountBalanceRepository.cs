using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Infrastructure.Database.Repositories
{
    public class AccountBalanceRepository : IAccountBalanceRepository
    {
        private readonly DatabaseConfig databaseConfig;

        public AccountBalanceRepository(DatabaseConfig databaseConfig)
        {
            this.databaseConfig = databaseConfig;
        }

        public async Task<int> AddMovement(Movement movimento)
        {
            try
            {
                using var connection = new SqliteConnection(databaseConfig.Name);

                string query = "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                               "VALUES (@IdMovimento, @IdContaCorrente, @Data, @Tipo, @Valor)";
                var parameters = new
                {
                    IdMovimento = movimento.Id.ToString(),
                    IdContaCorrente = movimento.AccountId.ToString(),
                    Data = movimento.Date.ToString(),
                    Tipo = movimento.Type == MovementType.Credit ? "C" : "D",
                    Valor = movimento.Value,
                };
                return await connection.ExecuteAsync(query, parameters);

            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<decimal> GetBalance(string idContaCorrente)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            string query = @"
                SELECT 
                    COALESCE(SUM(CASE WHEN tipomovimento = 'C' THEN valor ELSE 0 END), 0) -
                    COALESCE(SUM(CASE WHEN tipomovimento = 'D' THEN valor ELSE 0 END), 0) AS Saldo
                FROM movimento
                WHERE idcontacorrente = @IdContaCorrente";
            var parameters = new { IdContaCorrente = idContaCorrente };
            return await connection.ExecuteScalarAsync<decimal>(query, parameters);
        }

        public async Task<Account> GetAccount(string idContaCorrente)
        {
            using var connection = new SqliteConnection(databaseConfig.Name);

            string query = "SELECT * FROM contacorrente WHERE idcontacorrente = @IdContaCorrente";
            var parameters = new { IdContaCorrente = idContaCorrente };
            return await connection.QueryFirstOrDefaultAsync<Account>(query, parameters);
        }
    }
}
