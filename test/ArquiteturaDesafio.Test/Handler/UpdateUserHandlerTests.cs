using ArquiteturaDesafio.Application.UseCases.Commands.User.CreateUser;
using ArquiteturaDesafio.Application.UseCases.Commands.User.UpdateUser;
using ArquiteturaDesafio.Core.Domain.Common;
using ArquiteturaDesafio.Core.Domain.Entities;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using ArquiteturaDesafio.Core.Domain.ValueObjects;
using AutoMapper;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ArquiteturaDesafio.Tests.Application.Handlers
{
    public class UpdateUserHandlerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _tokenService;
        private readonly UpdateUserHandler _handler;

        public UpdateUserHandlerTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _tokenService = Substitute.For<IJwtTokenService>();
            _handler = new UpdateUserHandler(_unitOfWork, _userRepository, _mapper, _tokenService);
        }

        [Fact]
        public async Task Handle_Should_UpdateUser_When_RequestIsValid()
        {
            // Arrange
            var request = new UpdateUserRequest
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                Firstname = "Test",
                Lastname = "User",
                Address = new Core.Application.UseCases.DTOs.AddressDto() { City = "City", Geolocation = new Core.Application.UseCases.DTOs.GeolocationDto() { Lat = "1.0", Long = "2.0" }, Number = 1, Street = "Street", Zipcode = "606060660" },
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };
            var guid = Guid.NewGuid();
            var responseUser = new UpdateUserResponse
            {
                Id = guid,
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123",
                Firstname = "Test",
                Lastname = "User",
                Address = new Core.Application.UseCases.DTOs.AddressDto() { City = "City", Geolocation = new Core.Application.UseCases.DTOs.GeolocationDto() { Lat = "1.0", Long = "2.0" }, Number = 1, Street = "Street", Zipcode = "606060660" },
                Status = UserStatus.Active,
                Role = UserRole.Admin
            };
            var user = new User(
                "testuser",
                "test@example.com",
                "Password123", "Test", "User",
                new Core.Domain.ValueObjects.Address("City", "Street", 1, "606060660",
                new Core.Domain.ValueObjects.Geolocation("1.0", "2.0")), "11111",
                UserStatus.Active, UserRole.Admin, _tokenService);
            var endereco = new Core.Domain.ValueObjects.Address("City", "Street", 1, "606060660",
                new Core.Domain.ValueObjects.Geolocation("1.0", "2.0"));
            _userRepository.Get(request.Id, CancellationToken.None).Returns(Task.FromResult(user));
            _mapper.Map<User>(request).Returns(user);
            _mapper.Map<Address>(request.Address).Returns(endereco);
            _mapper.Map<UpdateUserResponse>(user).Returns(responseUser);

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);

            // Assert
            _userRepository.Received().Update(user);
            await _unitOfWork.Received().Commit(CancellationToken.None);
            Assert.True(response.Id == guid);
        }
    }
}
