using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Moq.AutoMock;
using MediatR;
using Brainwave.ManagementStudents.Application.Commands.User;
using Brainwave.ManagementStudents.Domain;
using Brainwave.Core.Extensions;

namespace Brainwave.ManagementStudents.Application.Tests.Commands
{
    public class UserCommandHandlerTests
    {
        private readonly AutoMocker _mocker;
        private readonly UserCommandHandler _handler;

        public UserCommandHandlerTests()
        {
            _mocker = new AutoMocker();
            _handler = _mocker.CreateInstance<UserCommandHandler>();
        }

        [Fact(DisplayName = "Should return false when AddStudentCommand is invalid")]
        [Trait("User", "ManagementStudents - UserCommandHandler")]
        public async Task Handle_AddStudent_ShouldReturnFalse_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.NewGuid(), "Student");
            _mocker.GetMock<ICommandValidator>().Setup(v => v.Validate(command)).Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Should add student successfully when command is valid")]
        [Trait("User", "ManagementStudents - UserCommandHandler")]
        public async Task Handle_AddStudent_ShouldReturnTrue_WhenCommandIsValid()
        {
            // Arrange
            var command = new AddStudentCommand(Guid.NewGuid(), "Student");
            _mocker.GetMock<ICommandValidator>().Setup(v => v.Validate(command)).Returns(true);
            _mocker.GetMock<IStudentRepository>().Setup(r => r.Add(It.IsAny<Student>())).Returns(Task.CompletedTask);
            _mocker.GetMock<IStudentRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Should return false when AddAdminCommand is invalid")]
        [Trait("User", "ManagementStudents - UserCommandHandler")]
        public async Task Handle_AddAdmin_ShouldReturnFalse_WhenCommandIsInvalid()
        {
            // Arrange
            var command = new AddAdminCommand(Guid.NewGuid(), "Admin");
            _mocker.GetMock<ICommandValidator>().Setup(v => v.Validate(command)).Returns(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact(DisplayName = "Should add admin successfully when command is valid")]
        [Trait("User", "ManagementStudents - UserCommandHandler")]
        public async Task Handle_AddAdmin_ShouldReturnTrue_WhenCommandIsValid()
        {
            // Arrange
            var command = new AddAdminCommand(Guid.NewGuid(), "Admin");
            _mocker.GetMock<ICommandValidator>().Setup(v => v.Validate(command)).Returns(true);
            _mocker.GetMock<IStudentRepository>().Setup(r => r.Add(It.IsAny<Student>())).Returns(Task.CompletedTask);
            _mocker.GetMock<IStudentRepository>().Setup(r => r.UnitOfWork.Commit()).ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
        }
    }
}
