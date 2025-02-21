using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;
using ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;
using ArquiteturaDesafio.Core.Application.Domain;
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Event;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using AutoMapper;
using Bogus.DataSets;
using MediatR;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ArquiteturaDesafio.Tests.Application.Handlers
{
    public class CreateTransactionHandlerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IProducerMessage _producerMessage;
        private readonly CreateTransactionHandler _handler;

        public CreateTransactionHandlerTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _transactionRepository = Substitute.For<ITransactionRepository>();
            _mediator = Substitute.For<IMediator>();
            _mapper = Substitute.For<IMapper>();
            _producerMessage = Substitute.For<IProducerMessage>();
            _handler = new CreateTransactionHandler(_unitOfWork, _transactionRepository, _mediator, _mapper, _producerMessage);
        }

        [Fact]
        public async Task Handle_ShouldCreateTransactionSuccessfully()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                Type = TransactionType.Credit,
                Amount = new Core.Application.UseCases.DTOs.MoneyDTO(100),
                Description = "Test transaction"
            };

            request.UpdateDate(DateTime.Now.Date);

            var transaction = new Transaction(request.Type, new Money(request.Amount.Amount), request.Date, request.Description);
            _mapper.Map<Transaction>(request).Returns(transaction);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(transaction.Id, response.id);
            await _unitOfWork.Received(1).Commit(CancellationToken.None);
            await _mediator.Received(1).Publish(Arg.Any<TransactionCreatedDomainEvent>(), CancellationToken.None);
            await _producerMessage.Received(1).SendMessage(Arg.Any<TransactionCreatedIntegrationEvent>(), "transaction.created");
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationExceptionForInvalidType()
        {
            // Arrange
            var request = new CreateTransactionRequest
            {
                Type = (TransactionType)999, // Invalid Type
                Amount = new Core.Application.UseCases.DTOs.MoneyDTO(100),
                Description = "Test transaction"
            };
            request.UpdateDate(DateTime.Now.Date);

            var transaction = new Transaction(request.Type, new Money(request.Amount.Amount), request.Date, request.Description);
            _mapper.Map<Transaction>(request).Returns(transaction);
            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}