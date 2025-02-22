using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;
using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.DeleteTransaction;
using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.UpdateTransaction;
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
    public class DeleteTransactionHandlerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;
        private readonly DeleteTransactionHandler _handler;
        private readonly IProducerMessage _producerMessage;
        private readonly IMediator _mediator;
        public DeleteTransactionHandlerTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _transactionRepository = Substitute.For<ITransactionRepository>();
            _mapper = Substitute.For<IMapper>();
            _mediator = Substitute.For<IMediator>();
            _producerMessage = Substitute.For<IProducerMessage>();
            _handler = new DeleteTransactionHandler(_unitOfWork, _transactionRepository, _mapper, _producerMessage, _mediator);
        }

        [Fact]
        public async Task Handle_ShouldDeleteTransactionSuccessfully()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var request = new DeleteTransactionRequest(transactionId);
            var transaction = new Transaction(TransactionType.Credit, new Money(100), DateTime.Now, "Test transaction");

            _transactionRepository.Get(transactionId, CancellationToken.None).Returns(transaction);
            _mapper.Map<DeleteTransactionResponse>(Arg.Any<string>()).Returns(new DeleteTransactionResponse("Transação excluida com sucesso."));

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(response);
            Assert.Equal("Transação excluida com sucesso.", response.Message);
            _transactionRepository.Received(1).Delete(transaction);
            await _unitOfWork.Received(1).Commit(CancellationToken.None);
        }

        [Fact]
        public async Task Handle_ShouldThrowKeyNotFoundExceptionForNonexistentTransaction()
        {
            // Arrange
            var transactionId = Guid.NewGuid();
            var request = new DeleteTransactionRequest(transactionId);

            _transactionRepository.Get(transactionId, CancellationToken.None).Returns((Transaction)null);

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}