using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using ArquiteturaDesafio.Infrastructure.Messaging.RabbitMQ.Consumer;
using Newtonsoft.Json.Linq;

namespace ArquiteturaDesafio.Worker
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConsumerMessage _consumer;
        private readonly IDailyBalanceReportRepository _repository;

        public Worker(ILogger<Worker> logger, IConsumerMessage consumer, IDailyBalanceReportRepository repository)
        {
            _logger = logger;
            _consumer = consumer;
            _repository = repository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _consumer.ConsumeQueue("transaction.created", ProcessMessage, stoppingToken);
        }

        private async Task ProcessMessage(string message)
        {
            try
            {
                if (message is null)
                {
                    return;
                }

                // Parse da mensagem para um JObject
                JObject jsonObject = JObject.Parse(message);

                // Extrair a parte "Data" do JSON e converter para a classe TransactionXYZ
                Transaction _transaction = jsonObject["Data"].ToObject<Transaction>();

                // Recupera o saldo diário da data da transação
                var filterDailysBalance = await _repository.Filter(x => x.Date.Date == _transaction.Date.Date, CancellationToken.None);
                var dailyBalance = filterDailysBalance.FirstOrDefault();

                // Caso não exista, instancia o objeto para inserir
                dailyBalance = dailyBalance ?? new Core.Domain.Entities.DailyBalanceReport(_transaction.Date, new Balance(0));

                // Verifica se é um novo saldo
                var isNewBalance = dailyBalance.TransactionCount == 0;

                // Verifica se é um saldo antigo
                var isOlderBalance = dailyBalance.TransactionCount > 0;

                // Adiciona a Transação
                dailyBalance.AddTransaction(_transaction.Type, _transaction.Amount);

                // Se novo inclui
                if (isNewBalance)
                {
                    await _repository.Create(dailyBalance);
                }

                // Se antigo atualiza
                if (isOlderBalance)
                {
                    await _repository.Update(dailyBalance);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao salvar o relatório de saldo diário");
            }
        }
    }
}
