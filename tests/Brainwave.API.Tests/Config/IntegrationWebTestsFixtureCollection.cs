using Bogus.DataSets;
using Bogus;
using Brainwave.ManagementStudents.Domain;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using System.Net.Http.Json;
using System.Text.Json;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Brainwave.ManagementPayment.Application.ValueObjects;
using Brainwave.API.ViewModel;


namespace Brainwave.API.Tests.Config
{
    [CollectionDefinition(nameof(IntegrationApiTestsFixtureCollection))]
    public class IntegrationApiTestsFixtureCollection : ICollectionFixture<IntegrationTestsFixture> { }

    public class IntegrationTestsFixture : IDisposable
    {
        public readonly BrainwaveAppFactory Factory;
        public HttpClient Client;
        public string ConnectionString { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
        public string UserEmail { get; set; }
        public string PasswordConfirmation { get; set; }
        public string Token { get; set; }
        public CreditCard CreditCard { get; set; }
        public Guid CourseId { get; set; }
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public Guid LessonId { get; set; }
        public Guid CertificateId { get; set; }

        public IntegrationTestsFixture()
        {
            var options = new WebApplicationFactoryClientOptions
            {
                BaseAddress = new Uri("http://localhost:5224")
            };
            Factory = new BrainwaveAppFactory();
            Client = Factory.CreateClient(options);
            CreditCard = GenerateCardData();
            var configuration = Factory.Services.GetRequiredService<IConfiguration>();
            ConnectionString = configuration.GetConnectionString("DefaultConnection") ??
                               throw new InvalidOperationException("Connection string 'DefaultConnection' is not configured.");
        }

        public void GenerateUserData()
        {
            var faker = new Faker("pt_BR");
            UserEmail = faker.Internet.Email().ToLower();
            UserName = UserEmail;
            UserPassword = faker.Internet.Password(8, false, "", "@1Ab_");
            PasswordConfirmation = UserPassword;
        }

        public CreditCard GenerateCardData()
        {
            var faker = new Faker("pt_BR");
            var credit = new CreditCard(faker.Finance.CreditCardNumber(CardType.Mastercard), faker.Name.FullName(), faker.Date.Future(1, DateTime.Now), faker.Finance.CreditCardCvv());
            return credit;
        }

        public async Task GetIdsByEnrollmentStatus(EnrollmentStatus status)
        {
            var sql = @"
                select
                    m.Id enrollmentId,
                    c.Id courseId,
                    a.Id studentId,
                    al.Id lessonId
                from
                    Enrollments m
                join EnrollmentStatuses sm on
                    sm.Id = m.StatusId
                join Courses c on
                    c.Id = m.CourseId
                join Students a on
                    a.Id = m.StudentId
                join Lessons al on al.CourseId = c.Id
                where
                    1=1
                and sm.Code = @Status
            ";

            await ExecuteQuery(sql, new { Status = (int)status }, (result) =>
            {
                if (result != null)
                {
                    EnrollmentId = Guid.Parse(result.enrollmentId);
                    CourseId = Guid.Parse(result.courseId);
                    StudentId = Guid.Parse(result.studentId);
                    LessonId = Guid.Parse(result.lessonId);
                }

                return result;
            });
        }

        public async Task GetLessonIdsWithProgress(EnrollmentStatus status)
        {
            var sql = @"
                select
                    m.Id enrollmentId,
                    c.Id courseId,
                    a.Id studentId,
                    al.Id lessonId
                from
                    Enrollments m
                join EnrollmentStatuses sm on
                    sm.Id = m.StatusId
                join Courses c on
                    c.Id = m.CourseId
                join Students a on
                    a.Id = m.StudentId
                join Lessons al on al.CourseId = c.Id
                join LessonProgress lp on lp.LessonId = al.Id and lp.StudentId = a.Id  
                where
                    1=1
                and sm.Code = @Status
            ";

            await ExecuteQuery(sql, new { Status = (int)status }, (result) =>
            {
                if (result != null)
                {
                    EnrollmentId = Guid.Parse(result.enrollmentId);
                    CourseId = Guid.Parse(result.courseId);
                    StudentId = Guid.Parse(result.studentId);
                    LessonId = Guid.Parse(result.lessonId);
                }

                return result;
            });
        }

        public async Task GetLessonIdsWithoutProgress(EnrollmentStatus status)
        {
            var sql = @"
                    select
                        m.Id enrollmentId,
                        c.Id courseId,
                        a.Id studentId,
                        al.Id lessonId,
                        lp.Id
                    from
                        Enrollments m
                    join EnrollmentStatuses sm on 
                        sm.Id = m.StatusId
                    join Courses c on
                        c.Id = m.CourseId
                    join Students a on
                        a.Id = m.StudentId
                    join Lessons al on al.CourseId = c.Id
                    left join LessonProgress lp on lp.LessonId = al.Id and lp.StudentId = a.Id
                    where
                        1=1
                    and sm.Code = @Status
                    and lp.Id is null
               ";

            await ExecuteQuery(sql, new { Status = (int)status }, (result) =>
            {
                if (result != null)
                {
                    EnrollmentId = Guid.Parse(result.enrollmentId);
                    CourseId = Guid.Parse(result.courseId);
                    StudentId = Guid.Parse(result.studentId);
                    LessonId = Guid.Parse(result.lessonId);
                }
                return result;
            });
        }

        public async Task GetCourseIdWithCompletedLessons()
        {
            var sql = @"
                    select
                        distinct c.Id
                    from
                        Courses c
                    join Lessons l on
                        l.CourseId = c.Id
                    join LessonProgress lp on
                        lp.LessonId = l.Id
                    join Students s on
                        s.Id = lp.StudentId
                    join AspNetUsers anu on
                        s.Id = UPPER(anu.Id)
                    where
                        1=1
                        and lp.Status = 2";

            await ExecuteQuery(sql, param: null, (result) =>
            {
                if (result != null)
                {
                    CourseId = Guid.Parse(result.Id);
                }
                return result;
            });
        }

        public void SaveUserToken(string token)
        {
            var response = JsonSerializer.Deserialize<LoginResponse>(token,
                new JsonSerializerOptions() { PropertyNameCaseInsensitive = true }) ?? new LoginResponse();

            Token = response.Data.AccessToken;
            StudentId = Guid.Parse(response.Data.UserToken.Id);
        }

        public class LoginResponse
        {
            public bool Success { get; set; }
            public LoginResponseViewModel Data { get; set; }
        }

        public async Task PerformApiLogin(string? email = null, string? password = null)
        {
            var userData = new LoginUserViewModel()
            {
                Email = email ?? "admin@brainwave.com",
                Password = password ?? "Teste@123"
            };

            var response = await Client.PostAsJsonAsync("/api/account/login", userData);
            response.EnsureSuccessStatusCode();

            SaveUserToken(await response.Content.ReadAsStringAsync());
        }

        public async Task PerformStudentRegistration()
        {
            GenerateUserData();
            var registerUserDto = new RegisterUserViewModel()
            {
                Name = UserName,
                Email = UserEmail,
                Password = UserPassword,
                ConfirmPassword = PasswordConfirmation
            };

            var response = await Client.PostAsJsonAsync("/api/account/registrar/aluno", registerUserDto);
            response.EnsureSuccessStatusCode();

            SaveUserToken(await response.Content.ReadAsStringAsync());
        }


        public async Task<Guid> GetDotNetCourse()
        {
            var sql_enrollment = @"
                    select c.courseid from Enrollments c 
                    where c.status = 1                   
                   ";

            string? courseid = await ExecuteQuery<string>(sql_enrollment, param: null, result => result);

            var sql = @$"
                    select c.Id from courses c 
                    where c.id = '{courseid}'               
                   ";

            string? id = await ExecuteQuery<string>(sql, param: null, result => result);
            return Guid.Parse(id);
        }

        public async Task<Guid> GetFirstCourseId()
        {
            var response = await Client.GetAsync("/api/courses");
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<JsonElement>(data);
            return json.GetProperty("data")[0].GetProperty("id").GetGuid();
        }
        public async Task<Guid> GetSecondCourseId()
        {
            var response = await Client.GetAsync("/api/courses");
            response.EnsureSuccessStatusCode();

            var data = await response.Content.ReadAsStringAsync();

            var json = JsonSerializer.Deserialize<JsonElement>(data);
            return json.GetProperty("data")[1].GetProperty("id").GetGuid();
        }
        public async Task<Guid> GetCourseWithoutLessons()
        {
            var sql = @"
                    select c.Id from courses c 
                    left join lessons a on a.courseid = c.id
                    where a.Id is null 
                   ";

            string? id = await ExecuteQuery<string>(sql, param: null, result => result);
            return Guid.Parse(id);
        }

        public async Task<Guid> GetCourseWithLessons()
        {
            var sql = @"
                    select c.Id from courses c 
                    inner join lessons a on a.courseid = c.id                    
                   ";

            string? id = await ExecuteQuery<string>(sql, param: null, result => result);
            return Guid.Parse(id);
        }

        public async Task<CourseCompletion> GetCourse_UnfinishedLessonsLessons()
        {
            using var connection = new SqliteConnection(ConnectionString);
            await connection.OpenAsync();

            var query = @"
                SELECT sl.UserId, sl.CourseId
                FROM StudentLessons sl
                GROUP BY sl.UserId, sl.CourseId
                HAVING COUNT(DISTINCT sl.LessonId) < (
                    SELECT COUNT(*)
                    FROM Lessons l
                    WHERE l.CourseId = sl.CourseId
                )
            ";

            var completions = await connection.QueryAsync<CourseCompletion>(query);
            return completions.FirstOrDefault();

        }

        public class CourseCompletion
        {
            public string UserId { get; set; }
            public string CourseId { get; set; }
        }

        public async Task GetCertificateId()
        {
            var sql = @"select c.Id from Certificates c";

            await ExecuteQuery(sql, param: null, (result) =>
            {
                if (result != null)
                {
                    CertificateId = Guid.Parse(result.Id);
                }
                return result;
            });
        }

        public JsonElement GetErrors(string result)
        {
            var json = JsonSerializer.Deserialize<JsonElement>(result);
            return json.GetProperty("errors");
        }

        public void Dispose()
        {
            Factory.Dispose();
            Client.Dispose();
        }

        private async Task<TResult?> ExecuteQuery<TResult>(string sql, object? param, Func<dynamic, TResult?> process)
        {
            await using var connection = new SqliteConnection(ConnectionString);
            await connection.OpenAsync();

            var result = await connection.QueryFirstOrDefaultAsync<TResult>(sql, param);

            await connection.CloseAsync();
            return process(result);
        }

    }

}
