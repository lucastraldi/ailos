using MediatR;
using Newtonsoft.Json;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Entities;
using Questao5.Infrastructure.Database.Repositories;

namespace Questao5.Application.Handlers
{
    public class AccountMovementHandler : IRequestHandler<AccountMovementRequest, AccountMovementResponse>
    {
        IIdempotenciaRepository _idempotenciaRepository;
        IAccountBalanceRepository _accountBalanceRepository;

        public AccountMovementHandler(IIdempotenciaRepository idempotenciaRepository, IAccountBalanceRepository accountBalanceRepository)
        {
            _idempotenciaRepository = idempotenciaRepository;
            _accountBalanceRepository = accountBalanceRepository;
        }

        public async Task<AccountMovementResponse> Handle(AccountMovementRequest request, CancellationToken cancellationToken)
        {
            var idempotencia = await _idempotenciaRepository.GetIdempotenciaByKey(request.RequestId);

            if (idempotencia != null)
            {
                return JsonConvert.DeserializeObject<AccountMovementResponse>(idempotencia.Resultado);
            }

            var moviment = new Movement
            {
                Id = Guid.NewGuid().ToString(),
                AccountId = request.AccountId,
                Date = DateTime.Now,
                Type = request.Type,
                Value = request.Value
            };

            var create = await _accountBalanceRepository.AddMovement(moviment);
            {
                try
                {
                    if (create > 0)
                    {
                        var createIdempotencia = await _idempotenciaRepository.AddIdempotencia(
                            new Idempotencia
                            {
                                ChaveIdempotencia = request.RequestId,
                                Requisicao = JsonConvert.SerializeObject(request),
                                Resultado = JsonConvert.SerializeObject(moviment)
                            });

                        if (createIdempotencia > 0)
                        {
                            return new AccountMovementResponse
                            {
                                MovementId = moviment.Id
                            };
                        }

                        throw new Exception("Idempotencia não foi salva!");

                    }

                    throw new Exception("Movimento não foi salvo!");
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}