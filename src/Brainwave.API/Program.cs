using Brainwave.API.Configurations;
var builder = WebApplication.CreateBuilder(args);
builder
    .AddJwt()
    .AddDbContextConfiguration(Brainwave.Core.Enums.EDatabases.SQLite)
    .AddApiConfiguration()
    .RegisterServices()
    .AddSwaggerConfiguration();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var enableSwagger = builder.Configuration.GetValue<bool>("EnableSwagger");

if (enableSwagger)
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
    });
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
