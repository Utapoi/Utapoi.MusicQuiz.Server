using System.Text.Json.Serialization;
using Utapoi.MusicQuiz.Application;
using Utapoi.MusicQuiz.Infrastructure;
using Utapoi.MusicQuiz.Server.Hubs.Rooms;
using Constants = Utapoi.MusicQuiz.Server.Common.Constants;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);

builder.Services.AddCors(c =>
{
    c.AddDefaultPolicy(p =>
    {
        p.AllowAnyHeader()
         .AllowAnyMethod()
         .AllowCredentials()
         .WithOrigins("http://localhost:3001");
    });
});

builder.Services
    .AddControllers()
    .AddJsonOptions(c =>
    {
        c.JsonSerializerOptions.PropertyNamingPolicy = null;
        c.JsonSerializerOptions.WriteIndented = false;
        c.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        c.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services
    .AddSignalR(options =>
    {
        options.EnableDetailedErrors = true;
    })
    .AddJsonProtocol(c =>
    {
        c.PayloadSerializerOptions.PropertyNamingPolicy = null;
        c.PayloadSerializerOptions.WriteIndented = false;
        c.PayloadSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        c.PayloadSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.CustomSchemaIds(type => type?.FullName?.Replace("+", ".")); });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseInfrastructure();
app.MapControllers();
app.MapHub<RoomHub>(Constants.RoomsHubPath); // Maybe we'll add more hubs in the future.

app.Run();
