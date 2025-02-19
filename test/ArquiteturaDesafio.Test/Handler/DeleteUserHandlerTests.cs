using ArquiteturaDesafio.Application.UseCases.Commands.User.DeleteUser;
using ArquiteturaDesafio.Application.UseCases.Commands.User.UpdateUser;
using ArquiteturaDesafio.Core.Domain.Enum;
using ArquiteturaDesafio.Core.Domain.Interfaces;
using AutoMapper;
using NSubstitute;
using System;
using Xunit;

namespace ArquiteturaDesafio.DeveloperEvaluation.Unit.Handler
{
    public class DeleteCartHandlerTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IJwtTokenService _tokenService;
        private readonly DeleteUserHandler _handler;

        public DeleteCartHandlerTests()
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
            var request = new DeleteUserRequest(new Core.Application.UseCases.DTOs.UserDTO() { Id = 1 });

            var user = new Core.Domain.Entities.User(1);

            user.Id = 1;
            _userRepository.Get(request.Id, CancellationToken.None).Returns(Task.FromResult(user));

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
            // Arrange
            var request = new DeleteUserRequest(new Core.Application.UseCases.DTOs.UserDTO() { Id = 1 });

            _userRepository.Get(request.Id, CancellationToken.None).Returns(Task.FromResult<Core.Domain.Entities.User>(null));

            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }
}
