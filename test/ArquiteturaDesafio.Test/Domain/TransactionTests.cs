using System;
using System.Collections.Generic;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using NSubstitute;
using Xunit;
namespace ArquiteturaDesafio.Test.Domain
{
    public class TransactionTests
    {
        [Fact]
        public void ShouldInitializeTransactionCorrectly()
        {
            // Arrange
            var type = TransactionType.Credit;
            var amount = new Money(100);
            var date = DateTime.Now;
            var description = "Test transaction";

            // Act
            var transaction = new Transaction(type, amount, date, description);

            // Assert
            Assert.Equal(type, transaction.Type);
            Assert.Equal(amount, transaction.Amount);
            Assert.Equal(date, transaction.Date);
            Assert.Equal(description, transaction.Description);
        }

        [Fact]
        public void ShouldUpdateTransactionCorrectly()
        {
            // Arrange
            var type = TransactionType.Credit;
            var amount = new Money(100);
            var date = DateTime.Now;
            var description = "Test transaction";
            var transaction = new Transaction(type, amount, date, description);

            var newType = TransactionType.Debit;
            var newAmount = new Money(200);
            var newDate = DateTime.Now.AddDays(1);
            var newDescription = "Updated transaction";

            // Act
            transaction.UpdateTransaction(newType, newAmount, newDate, newDescription);

            // Assert
            Assert.Equal(newType, transaction.Type);
            Assert.Equal(newAmount, transaction.Amount);
            Assert.Equal(newDate.ToUniversalTime(), transaction.Date);
            Assert.Equal(newDescription, transaction.Description);
        }

        [Fact]
        public void ShouldGenerateNewId()
        {
            // Arrange
            var type = TransactionType.Credit;
            var amount = new Money(100);
            var date = DateTime.Now;
            var description = "Test transaction";
            var transaction = new Transaction(type, amount, date, description);

            // Act
            transaction.GenerateId();

            // Assert
            Assert.NotEqual(Guid.Empty, transaction.Id);
        }

        [Fact]
        public void ShouldCreateTransactionIntegrateEvent()
        {
            // Arrange
            var type = TransactionType.Credit;
            var amount = new Money(100);
            var date = DateTime.Now;
            var description = "Test transaction";
            var transaction = new Transaction(type, amount, date, description);

            // Act
            var integrationEvent = transaction.CreateTransactionIntegrateEvent();

            // Assert
            Assert.NotNull(integrationEvent);
        }
    }
}
