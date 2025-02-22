using ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;
using ArquiteturaDesafio.Application.UseCases.Commands.User.UpdateUser;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using NSubstitute;
using System;
using Xunit;

namespace ArquiteturaDesafio.Tests.Application.Handlers
{
    public class DeleteUserHandlerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _tokenService;
        private readonly DeleteUserHandler _handler;

        public DeleteUserHandlerTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _userRepository = Substitute.For<IUserRepository>();
            _mapper = Substitute.For<IMapper>();
            _tokenService = Substitute.For<IJwtTokenService>();
            _handler = new DeleteUserHandler(_unitOfWork, _userRepository, _mapper);
        }

        [Fact]
        public async Task Handle_Should_DeleteUser_When_UserExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var request = new DeleteUserRequest(new Core.Application.UseCases.DTOs.UserDTO() { Id = id });
            var user = new Core.Domain.Entities.User(id);
            _userRepository.Get(id, CancellationToken.None).Returns(Task.FromResult(user));

            // Act
            var response = await _handler.Handle(request, CancellationToken.None);
            _mapper.Map<DeleteUserResponse>(user);

            // Assert
            _userRepository.Received().Delete(user);
            await _unitOfWork.Received().Commit(CancellationToken.None);
            _mapper.Received().Map<DeleteUserResponse>(user);
        }

        [Fact]
        public async Task Handle_Should_ThrowException_When_UserDoesNotExist()
        {
            var id = Guid.NewGuid();
            // Arrange
            var request = new DeleteUserRequest(new Core.Application.UseCases.DTOs.UserDTO() { Id = id });

            _userRepository.Get(id, CancellationToken.None).Returns(Task.FromResult<Core.Domain.Entities.User>(null));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}
