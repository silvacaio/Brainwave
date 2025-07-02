using Brainwave.API.Configurations;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);
builder
    .AddJwt()
    .AddDbContextConfiguration(Brainwave.Core.Enums.EDatabases.SQLite)
    .AddApiConfiguration()
    .RegisterServices()
    .AddSwaggerConfiguration();

var app = builder.Build();

var enableSwagger = builder.Configuration.GetValue<bool>("EnableSwagger");

if (enableSwagger)
{
    app.UseSwaggerSetup();  
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("*");


app.UseAuthorization();

app.MapControllers();

app.UseDbMigrationHelper();

app.Run();
