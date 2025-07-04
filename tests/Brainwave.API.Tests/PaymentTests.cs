using Brainwave.API.Tests.Config;
using Brainwave.API.ViewModel;
using Brainwave.ManagementStudents.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Brainwave.API.Tests
{
    [Collection(nameof(IntegrationApiTestsFixtureCollection))]
    public class PaymentControllerTests
    {
        private readonly IntegrationTestsFixture _fixture;

        public PaymentControllerTests(IntegrationTestsFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact(DisplayName = "Get Enrollments with Pending Payment")]
        [Trait("Category", "Integration API - Payment")]
        public async Task Get_PendingPaymentEnrollments_ShouldReturnOk()
        {
            // Arrange
            await _fixture.PerformApiLogin("aluno1@brainwave.com", "Teste@123");
            _fixture.Client.AssignToken(_fixture.Token);

            // Act
            var response = await _fixture.Client.GetAsync("/api/payment/pending-payment");

            // Assert
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadAsStringAsync();
            Assert.False(string.IsNullOrEmpty(result));
        }

        [Fact(DisplayName = "Make Payment - Enrollment Not Found")]
        [Trait("Category", "Integration API - Payment")]
        public async Task Post_MakePayment_EnrollmentDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            await _fixture.PerformApiLogin("aluno1@brainwave.com", "Teste@123");
            _fixture.Client.AssignToken(_fixture.Token);


            var cursoId = await _fixture.GetFirstCourseId();

            var payment = new PaymentViewModel
            {
                EnrollmentId = Guid.NewGuid(),
                CardHolderName = "John Doe",
                CardNumber = "4111111111111111",
                ExpirationDate = DateTime.Today.AddDays(1),
                SecurityCode = "123",
                Value = 100
            };

            // Act
            var response = await _fixture.Client.PostAsJsonAsync($"/api/payment/make-payment", payment);

            // Assert
            var erros = _fixture.GetErrors(await response.Content.ReadAsStringAsync());
            Assert.Contains("The specified enrollment does not exist.", erros.ToString());
        }

        [Fact(DisplayName = "Make Payment - Error for Other User")]
        [Trait("Category", "Integration API - Payment")]
        public async Task Post_MakePayment_AnotherUserEnrollment_ShouldReturnError()
        {
            // Arrange            
            await _fixture.PerformApiLogin("aluno1@brainwave.com", "Teste@123");
            _fixture.Client.AssignToken(_fixture.Token);

            var enrollmentId = await _fixture.GetenrollmentStudent2();

            var payment = new PaymentViewModel
            {
                EnrollmentId = enrollmentId,
                CardHolderName = "Aluno Errado",
                CardNumber = "4111111111111111",
                ExpirationDate = DateTime.Today.AddDays(1),
                SecurityCode = "123",
                Value = 100
            };

            // Act
            var response = await _fixture.Client.PostAsJsonAsync($"/api/payment/make-payment", payment);

            // Assert
            var erros = _fixture.GetErrors(await response.Content.ReadAsStringAsync());
            Assert.Contains("You do not have permission to make a payment", erros.ToString());
        }

        [Fact(DisplayName = "Make Payment - Invalid Enrollment Status"), TestPriority(2)]
        [Trait("Category", "Integration API - Payment")]
        public async Task Post_MakePayment_InvalidStatus_ShouldReturnBadRequest()
        {
            // Arrange
            await _fixture.PerformApiLogin("aluno2@brainwave.com", "Teste@123");
            _fixture.Client.AssignToken(_fixture.Token);

            var enrollmentId = await _fixture.GetenrollmentStudent2();

            // simula pagamento anterior (muda status no banco para pago)
            await _fixture.ChangePaymentStatus(enrollmentId, EnrollmentStatus.Done);

            var payment = new PaymentViewModel
            {
                EnrollmentId = enrollmentId,
                CardHolderName = "Aluno",
                CardNumber = "4111111111111111",
                ExpirationDate = DateTime.Today.AddDays(1),
                SecurityCode = "123",
                Value = 100
            };

            // Act
            var response = await _fixture.Client.PostAsJsonAsync($"/api/payment/make-payment", payment);

            // Assert
            var erros = _fixture.GetErrors(await response.Content.ReadAsStringAsync());
            Assert.Contains("The enrollment is not in a pending payment status", erros.ToString());
        }

        [Fact(DisplayName = "Make Payment - Success")]
        [Trait("Category", "Integration API - Payment")]
        public async Task Post_MakePayment_ShouldSucceed()
        {
            // Arrange
            await _fixture.PerformApiLogin("aluno2@brainwave.com", "Teste@123");
            _fixture.Client.AssignToken(_fixture.Token);

            var enrollmentId = await _fixture.GetenrollmentStudent2();

            // simula pagamento anterior (muda status no banco para pago)
            await _fixture.ChangePaymentStatus(enrollmentId, EnrollmentStatus.PendingPayment);

            var payment = new PaymentViewModel
            {
                EnrollmentId = enrollmentId,
                CardHolderName = "Aluno",
                CardNumber = "4111111111111111",
                ExpirationDate = DateTime.Today.AddDays(1),
                SecurityCode = "123",
                Value = 100
            };

            // Act
            var response = await _fixture.Client.PostAsJsonAsync($"/api/payment/make-payment", payment);

            // Assert
            response.EnsureSuccessStatusCode();
        }
    }
}
