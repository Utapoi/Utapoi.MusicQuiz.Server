using Utapoi.MusicQuiz.Server.Hubs.Rooms;
using Constants = Utapoi.MusicQuiz.Server.Common.Constants;

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddControllers();
builder.Services.AddSignalR(options =>
{
    options.EnableDetailedErrors = true;
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<RoomHub>(Constants.RoomsHubPath);

app.Run();
