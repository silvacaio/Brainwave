using Brainwave.API.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder
    .AddJwt()
    .AddDbContextConfiguration(Brainwave.Core.Enums.EDatabases.SQLite)
    .AddApiConfiguration()
    .RegisterServices()
    .AddSwaggerConfiguration();

var app = builder.Build();

app.UseSwaggerSetup();

app.UseHttpsRedirection();
app.UseCors("*");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseDbMigrationHelper();

app.Run();


public partial class Program { }