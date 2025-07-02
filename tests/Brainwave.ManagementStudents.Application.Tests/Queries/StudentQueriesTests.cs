using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Brainwave.ManagementStudents.Application.Queries;
using Brainwave.ManagementStudents.Application.Queries.ViewModels;
using Brainwave.ManagementStudents.Domain;
using static Brainwave.ManagementStudents.Domain.Enrollment;
using static Brainwave.ManagementStudents.Domain.StudentLesson;

namespace Brainwave.ManagementStudents.Application.Tests.Queries
{
    public class StudentQueriesTests
    {
        private readonly Mock<IStudentRepository> _repositoryMock;
        private readonly StudentQueries _queries;

        public StudentQueriesTests()
        {
            _repositoryMock = new Mock<IStudentRepository>();
            _queries = new StudentQueries(_repositoryMock.Object);
        }

        [Fact(DisplayName = "Should return enrollment view model when enrollment exists")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetEnrollment_ShouldReturnViewModel_WhenEnrollmentExists()
        {
            var courseId = Guid.NewGuid();
            var studentId = Guid.NewGuid();
            var enrollment = EnrollmentActive.Create(studentId, courseId);

            _repositoryMock.Setup(r => r.GetEnrollmentByCourseIdAndStudentId(courseId, studentId))
                .ReturnsAsync(enrollment);

            var result = await _queries.GetEnrollment(courseId, studentId);

            Assert.NotNull(result);
            Assert.Equal(courseId, result.CourseId);
            Assert.Equal(studentId, result.StudentId);
        }

        [Fact(DisplayName = "Should return null when enrollment not found")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetEnrollment_ShouldReturnNull_WhenNotFound()
        {
            _repositoryMock.Setup(r => r.GetEnrollmentByCourseIdAndStudentId(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Enrollment)null!);

            var result = await _queries.GetEnrollment(Guid.NewGuid(), Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact(DisplayName = "Should return empty list when no pending enrollments")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetPendingPaymentEnrollments_ShouldReturnEmpty_WhenNoneFound()
        {
            _repositoryMock.Setup(r => r.GetPendingPaymentEnrollments(It.IsAny<Guid>()))
                .ReturnsAsync((IEnumerable<Enrollment>)null!);

            var result = await _queries.GetPendingPaymentEnrollments(Guid.NewGuid());

            Assert.Empty(result);
        }

        [Fact(DisplayName = "Should return list of view models when pending enrollments found")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetPendingPaymentEnrollments_ShouldReturnList_WhenFound()
        {
            var studentId = Guid.NewGuid();
            var enrollment = EnrollmentPendingPayment.Create(studentId, Guid.NewGuid());

            _repositoryMock.Setup(r => r.GetPendingPaymentEnrollments(studentId))
                .ReturnsAsync(new[] { enrollment });

            var result = await _queries.GetPendingPaymentEnrollments(studentId);

            Assert.Single(result);
        }

        [Fact(DisplayName = "Should return enrollment list by user id")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetEnrollmentsByUserId_ShouldReturnList_WhenFound()
        {
            var studentId = Guid.NewGuid();
            var enrollment = EnrollmentActive.Create(studentId, Guid.NewGuid());

            _repositoryMock.Setup(r => r.GetEnrollmentsByStudentId(studentId))
                .ReturnsAsync(new[] { enrollment });

            var result = await _queries.GetEnrollmentsByUserId(studentId);

            Assert.Single(result);
        }

        [Fact(DisplayName = "Should return enrollment by id")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetEnrollmentById_ShouldReturnEnrollment_WhenFound()
        {
            var enrollment = EnrollmentActive.Create(Guid.NewGuid(), Guid.NewGuid());

            _repositoryMock.Setup(r => r.GetEnrollmentsById(enrollment.Id))
                .ReturnsAsync(enrollment);

            var result = await _queries.GetEnrollmentById(enrollment.Id);

            Assert.NotNull(result);
            Assert.Equal(enrollment.Id, result.EnrollmentId);
        }

        [Fact(DisplayName = "Should return null when enrollment not found by id")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetEnrollmentById_ShouldReturnNull_WhenNotFound()
        {
            _repositoryMock.Setup(r => r.GetEnrollmentsById(It.IsAny<Guid>()))
                .ReturnsAsync((Enrollment)null!);

            var result = await _queries.GetEnrollmentById(Guid.NewGuid());

            Assert.Null(result);
        }

        [Fact(DisplayName = "Should return student lessons by course")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetStudentLessonsByCourseId_ShouldReturnList_WhenFound()
        {
            var userId = Guid.NewGuid();
            var courseId = Guid.NewGuid();
            var lesson = StudentLessonFactory.Create(userId, courseId, Guid.NewGuid());

            _repositoryMock.Setup(r => r.GetStudentLessonsByCourseId(userId, courseId))
                .ReturnsAsync(new[] { lesson });

            var result = await _queries.GetStudentLessonsByCourseId(userId, courseId);

            Assert.Single(result);
            Assert.Equal(userId, result.First().StudentId);
            Assert.Equal(courseId, result.First().CourseId);
        }

        [Fact(DisplayName = "Should return empty when no student lessons")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetStudentLessonsByCourseId_ShouldReturnEmpty_WhenNotFound()
        {
            _repositoryMock.Setup(r => r.GetStudentLessonsByCourseId(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((IEnumerable<StudentLesson>)null!);

            var result = await _queries.GetStudentLessonsByCourseId(Guid.NewGuid(), Guid.NewGuid());

            Assert.Empty(result);
        }

        [Fact(DisplayName = "Should return student certificates")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetStudentCertificates_ShouldReturnList_WhenFound()
        {
            var studentId = Guid.NewGuid();
            var cert = new Certificate("João", "Test", Guid.NewGuid(), studentId);

            _repositoryMock.Setup(r => r.GetStudentCertificates(studentId))
                .ReturnsAsync(new[] { cert });

            var result = await _queries.GetStudentCertificates(studentId);

            Assert.Single(result);
        }

        [Fact(DisplayName = "Should return empty list when no certificates")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetStudentCertificates_ShouldReturnEmpty_WhenNotFound()
        {
            _repositoryMock.Setup(r => r.GetStudentCertificates(It.IsAny<Guid>()))
                .ReturnsAsync((IEnumerable<Certificate>)null!);

            var result = await _queries.GetStudentCertificates(Guid.NewGuid());

            Assert.Empty(result);
        }

        [Fact(DisplayName = "Should return certificate by enrollment id")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetCertificate_ShouldReturn_WhenFound()
        {
            var studentId = Guid.NewGuid();
            var enrollmentId = Guid.NewGuid();
            var cert = new Certificate("João", "Test", enrollmentId, studentId);

            _repositoryMock.Setup(r => r.GetCertificate(studentId, enrollmentId))
                .ReturnsAsync(cert);

            var result = await _queries.GetCertificate(studentId, enrollmentId);

            Assert.NotNull(result);
            Assert.Equal(studentId, result.StudentId);
            Assert.Equal(enrollmentId, result.EnrollmentId);
        }

        [Fact(DisplayName = "Should return null when certificate not found")]
        [Trait("Queries", "StudentQueries")]
        public async Task GetCertificate_ShouldReturnNull_WhenNotFound()
        {
            _repositoryMock.Setup(r => r.GetCertificate(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .ReturnsAsync((Certificate)null!);

            var result = await _queries.GetCertificate(Guid.NewGuid(), Guid.NewGuid());

            Assert.Null(result);
        }
    }
}
