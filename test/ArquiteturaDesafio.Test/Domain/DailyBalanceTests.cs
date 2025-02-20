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
    public class DailyBalanceTests
    {
        [Fact]
        public void ShouldInitializeDailyBalanceCorrectly()
        {
            // Arrange
            var initialBalance = new Money(100);
            var date = DateTime.Now;

            // Act
            var dailyBalance = new DailyBalance(date, initialBalance);

            // Assert
            Assert.Equal(date, dailyBalance.Date);
            Assert.Equal(initialBalance, dailyBalance.InitialBalance);
            Assert.Equal(initialBalance, dailyBalance.FinalBalance);
            Assert.Equal(new Money(0), dailyBalance.TotalCredits);
            Assert.Equal(new Money(0), dailyBalance.TotalDebits);
            Assert.Equal(0, dailyBalance.TransactionCount);
        }

        [Fact]
        public void ShouldAddCreditTransaction()
        {
            // Arrange
            var initialBalance = new Money(100);
            var dailyBalance = new DailyBalance(DateTime.Now, initialBalance);
            var creditAmount = new Money(50);

            // Act
            dailyBalance.AddTransaction(TransactionType.Credit, creditAmount);

            // Assert
            Assert.Equal(initialBalance.Add(creditAmount), dailyBalance.FinalBalance);
            Assert.Equal(creditAmount, dailyBalance.TotalCredits);
            Assert.Equal(new Money(0), dailyBalance.TotalDebits);
            Assert.Equal(1, dailyBalance.TransactionCount);
        }

        [Fact]
        public void ShouldAddDebitTransaction()
        {
            // Arrange
            var initialBalance = new Money(100);
            var dailyBalance = new DailyBalance(DateTime.Now, initialBalance);
            var debitAmount = new Money(30);

            // Act
            dailyBalance.AddTransaction(TransactionType.Debit, debitAmount);

            // Assert
            Assert.Equal(initialBalance.Subtract(debitAmount), dailyBalance.FinalBalance);
            Assert.Equal(new Money(0), dailyBalance.TotalCredits);
            Assert.Equal(debitAmount, dailyBalance.TotalDebits);
            Assert.Equal(1, dailyBalance.TransactionCount);
        }

    }
}
