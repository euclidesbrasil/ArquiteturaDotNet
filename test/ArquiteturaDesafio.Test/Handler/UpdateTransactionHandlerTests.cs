using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;
using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.DeleteTransaction;
using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.UpdateTransaction;
using ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;
using ArquiteturaDesafio.Core.Application.Domain;
using ArquiteturaDesafio.Core.Application.UseCases.DTOs;
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
    public class UpdateTransactionHandlerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly UpdateTransactionHandler _handler;
        private readonly IProducerMessage _producerMessage;
        private readonly IMediator _mediator;
        public UpdateTransactionHandlerTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _transactionRepository = Substitute.For<ITransactionRepository>();
            _mapper = Substitute.For<IMapper>();
            _mediator = Substitute.For<IMediator>();
            _producerMessage = Substitute.For<IProducerMessage>();
            _handler = new UpdateTransactionHandler(_unitOfWork, _transactionRepository, _mapper, _producerMessage, _mediator);
        }

        [Fact]
        public async Task Handle_ShouldUpdateTransactionSuccessfully()
        {
            var idContext = Guid.NewGuid();
            // Arrange
            var request = new UpdateTransactionRequest
            {
                Type = TransactionType.Debit,
                Amount = new MoneyDTO(200),
                Description = "Updated transaction"
            };
            var responseMock = new UpdateTransactionResponse();
            responseMock.UpdateId(idContext);
            var transaction = new Transaction(TransactionType.Credit, new Money(100), DateTime.Now, "Test transaction");
            _transactionRepository.Get(request.Id, CancellationToken.None).Returns(transaction);
            _mapper.Map<Transaction>(request).Returns(transaction);
            _mapper.Map<UpdateTransactionResponse>(transaction).Returns(responseMock);
            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            // Assert
            Assert.NotNull(response);
            await _unitOfWork.Received(1).Commit(CancellationToken.None);
        }

        [Fact]
        public async Task Handle_ShouldThrowInvalidOperationExceptionForNonexistentTransaction()
        {
            var idContext = Guid.NewGuid();
            // Arrange
            var request = new UpdateTransactionRequest
            {
                Type = TransactionType.Debit,
                Amount = new MoneyDTO(200),
                Description = "Updated transaction"
            };
            request.UpdateId(idContext);
            var transaction = new Transaction(TransactionType.Credit, new Money(100), DateTime.Now, "Test transaction");
            _mapper.Map<ArquiteturaDesafio.Core.Domain.Entities.Transaction>(request).Returns(transaction);
            _mapper.Map<Transaction>(request).Returns(transaction);
            _transactionRepository.Get(request.Id, CancellationToken.None).Returns((Transaction)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}