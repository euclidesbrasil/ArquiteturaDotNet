using ArquiteturaDesafio.Core.Application.Domain;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Event;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArquiteturaDesafio.Core.Application.Service
{
    public class TransactionCreatedEventHandler : INotificationHandler<TransactionCreatedDomainEvent>
    {
        private readonly IDailyBalanceRepository _dailyBalanceRepository;
        private readonly IUnitOfWork _unitOfWork;
      
        public TransactionCreatedEventHandler(IUnitOfWork unitOfWork, IDailyBalanceRepository dailyBalanceRepository)
        {
            _unitOfWork = unitOfWork;
            _dailyBalanceRepository = dailyBalanceRepository;
        }

        public async Task Handle(TransactionCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            // Recupera o saldo diário da data da transação
            var dailyBalance = await _dailyBalanceRepository.GetByDateAsync(notification.Date);
            
            // Caso não exista, instancia o objeto para inserir
            dailyBalance = dailyBalance ?? new DailyBalance(notification.Date, new Balance(0));

            //Verifica se é um novo saldo
            var isNewBalance = dailyBalance.TransactionCount == 0;
            
            // Verifica se é um saldo antigo
            var isOlderBalance = dailyBalance.TransactionCount > 0;
            
            // Adicona a Transação
            dailyBalance.AddTransaction(notification.Type, notification.Amount);

            // Se novo inclui
            if (isNewBalance)
            {
                _dailyBalanceRepository.Create(dailyBalance);
            }

            // Se antigo atualiza
            if (isOlderBalance)
            {
                _dailyBalanceRepository.Update(dailyBalance);
            }

            await _unitOfWork.Commit(cancellationToken);
        }
    }

}
