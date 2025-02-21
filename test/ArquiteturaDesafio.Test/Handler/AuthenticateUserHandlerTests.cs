using ArquiteturaDesafio.Application.UseCases.Commands.Transaction.CreateTransaction;
using ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;
using ArquiteturaDesafio.Core.Application.Domain;
using ArquiteturaDesafio.Core.Application.UseCases.Commands.AuthenticateUser;
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Event;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using AutoMapper;
using Bogus;
using Bogus.DataSets;
using MediatR;
using Microsoft.Extensions.Configuration;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ArquiteturaDesafio.Tests.Application.Handlers
{
    public class AuthenticateUserHandlerTests
    {
        private readonly IUserRepository _repository;
        private readonly IJwtTokenService _tokenService;
        private readonly AuthenticateUserHandler _handler;
        private readonly Faker _faker;

        public AuthenticateUserHandlerTests()
        {
            _repository = Substitute.For<IUserRepository>();
            _tokenService = Substitute.For<IJwtTokenService>();
            _handler = new AuthenticateUserHandler(_repository, Substitute.For<IMapper>(), Substitute.For<IConfiguration>(), _tokenService);
            _faker = new Faker("pt_BR");
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessWhenCredentialsAreValid()
        {
            // Arrange
            var request = new AuthenticateUserRequest
            {
                Username = _faker.Internet.UserName(),
                Password = _faker.Internet.Password()
            };

            var user = new User(_faker.Internet.Email(), request.Username, request.Password, _faker.Name.FullName(), _faker.Address.Country(),
                new Core.Domain.ValueObjects.Address(_faker.Address.City(), _faker.Address.StreetName(), _faker.Random.Number(1, 1000), _faker.Address.ZipCode(), new Geolocation(_faker.Address.Latitude().ToString(), _faker.Address.Longitude().ToString())),
                _faker.Random.Replace("###########"), UserStatus.Active, UserRole.Admin, _tokenService
            );

            _repository.GetByLoginAsync(request.Username).Returns(user);
            _tokenService.VerifyPassword(request.Password, user.PasswordHash).Returns(true);
            _tokenService.GenerateJwtToken(user.Username, user.Role.ToString()).Returns("validToken");

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("validToken", result.Token);
            Assert.Equal("Authenticated", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureWhenUserIsNotFound()
        {
            // Arrange
            var request = new AuthenticateUserRequest
            {
                Username = _faker.Internet.UserName(),
                Password = _faker.Internet.Password()
            };

            _repository.GetByLoginAsync(request.Username).Returns((User)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid credentials", result.Message);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureWhenPasswordIsInvalid()
        {
            /// Arrange
            var request = new AuthenticateUserRequest
            {
                Username = _faker.Internet.UserName(),
                Password = _faker.Internet.Password()
            };

            var user = new User(_faker.Internet.Email(), request.Username, request.Password, _faker.Name.FullName(), _faker.Address.Country(),
                new Core.Domain.ValueObjects.Address(_faker.Address.City(), _faker.Address.StreetName(), _faker.Random.Number(1, 1000), _faker.Address.ZipCode(), new Geolocation(_faker.Address.Latitude().ToString(), _faker.Address.Longitude().ToString())),
                _faker.Random.Replace("###########"), UserStatus.Active, UserRole.Admin, _tokenService
            );

            _repository.GetByLoginAsync(request.Username).Returns(user);
            _tokenService.VerifyPassword(request.Password, user.PasswordHash).Returns(false);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Invalid credentials", result.Message);
        }
    }
}