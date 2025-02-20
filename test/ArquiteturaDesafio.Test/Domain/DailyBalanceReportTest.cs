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
    public class DailyBalanceReportTests
    {
        [Fact]
        public void ShouldInitializeDailyBalanceReportCorrectly()
        {
            // Arrange
            var initialBalance = new Money(100);
            var date = DateTime.Now;

            // Act
            var dailyBalanceReport = new DailyBalanceReport(date, initialBalance);

            // Assert
            Assert.Equal(date, dailyBalanceReport.Date);
            Assert.Equal(initialBalance, dailyBalanceReport.InitialBalance);
            Assert.Equal(initialBalance, dailyBalanceReport.FinalBalance);
            Assert.Equal(new Money(0), dailyBalanceReport.TotalCredits);
            Assert.Equal(new Money(0), dailyBalanceReport.TotalDebits);
            Assert.Equal(0, dailyBalanceReport.TransactionCount);
        }

        [Fact]
        public void ShouldAddCreditTransaction()
        {
            // Arrange
            var initialBalance = new Money(100);
            var dailyBalanceReport = new DailyBalanceReport(DateTime.Now, initialBalance);
            var creditAmount = new Money(50);

            // Act
            dailyBalanceReport.AddTransaction(TransactionType.Credit, creditAmount);

            // Assert
            Assert.Equal(initialBalance.Add(creditAmount), dailyBalanceReport.FinalBalance);
            Assert.Equal(creditAmount, dailyBalanceReport.TotalCredits);
            Assert.Equal(new Money(0), dailyBalanceReport.TotalDebits);
            Assert.Equal(1, dailyBalanceReport.TransactionCount);
        }

        [Fact]
        public void ShouldAddDebitTransaction()
        {
            // Arrange
            var initialBalance = new Money(100);
            var dailyBalanceReport = new DailyBalanceReport(DateTime.Now, initialBalance);
            var debitAmount = new Money(30);

            // Act
            dailyBalanceReport.AddTransaction(TransactionType.Debit, debitAmount);

            // Assert
            Assert.Equal(initialBalance.Subtract(debitAmount), dailyBalanceReport.FinalBalance);
            Assert.Equal(new Money(0), dailyBalanceReport.TotalCredits);
            Assert.Equal(debitAmount, dailyBalanceReport.TotalDebits);
            Assert.Equal(1, dailyBalanceReport.TransactionCount);
        }
    }
}