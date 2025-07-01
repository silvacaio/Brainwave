using Brainwave.ManagementStudents.Domain;
using System;
using Xunit;

namespace Brainwave.ManagementStudents.Domain.Tests
{
    public class EnrollmentTests
    {
        [Fact]
        public void Constructor_ShouldSetDefaultStatus_AsPendingPayment()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            // Act
            var enrollment = new Enrollment(studentId, courseId);

            // Assert
            Assert.Equal(studentId, enrollment.StudentId);
            Assert.Equal(courseId, enrollment.CourseId);
            Assert.Equal(EnrollmentStatus.PendingPayment, enrollment.Status);
        }

        [Fact]
        public void Activate_ShouldSetStatus_AsActive()
        {
            // Arrange
            var enrollment = new Enrollment(Guid.NewGuid(), Guid.NewGuid());

            // Act
            enrollment.Activate();

            // Assert
            Assert.Equal(EnrollmentStatus.Active, enrollment.Status);
        }

        [Fact]
        public void Close_ShouldSetStatus_AsDone()
        {
            // Arrange
            var enrollment = new Enrollment(Guid.NewGuid(), Guid.NewGuid());

            // Act
            enrollment.Close();

            // Assert
            Assert.Equal(EnrollmentStatus.Done, enrollment.Status);
        }

        [Fact]
        public void EnrollmentPendingPaymentFactory_ShouldSetStatus_AsPendingPayment()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            // Act
            var enrollment = Enrollment.EnrollmentPendingPayment.Create(studentId, courseId);

            // Assert
            Assert.Equal(studentId, enrollment.StudentId);
            Assert.Equal(courseId, enrollment.CourseId);
            Assert.Equal(EnrollmentStatus.PendingPayment, enrollment.Status);
        }

        [Fact]
        public void EnrollmentActiveFactory_ShouldSetStatus_AsActive()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            // Act
            var enrollment = Enrollment.EnrollmentActive.Create(studentId, courseId);

            // Assert
            Assert.Equal(EnrollmentStatus.Active, enrollment.Status);
        }

        [Fact]
        public void EnrollmentDoneFactory_ShouldSetStatus_AsDone()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            // Act
            var enrollment = Enrollment.EnrollmentDone.Create(studentId, courseId);

            // Assert
            Assert.Equal(EnrollmentStatus.Done, enrollment.Status);
        }

        [Fact]
        public void EnrollmentBlockedFactory_ShouldSetStatus_AsBlocked()
        {
            // Arrange
            var studentId = Guid.NewGuid();
            var courseId = Guid.NewGuid();

            // Act
            var enrollment = Enrollment.EnrollmentBlocked.Create(studentId, courseId);

            // Assert
            Assert.Equal(EnrollmentStatus.Blocked, enrollment.Status);
        }
    }
}

