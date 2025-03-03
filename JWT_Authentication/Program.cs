using JWT_Authentication.Extensions;
using JWT_Authentication.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddCustomJwtAuth(builder.Configuration);

// Register TokenService
builder.Services.AddSingleton<TokenService>();
builder.Services.AddSingleton<UserService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// Configure Swagger for JWT Authentication
builder.Services.AddSwaggerGenWithAuth();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();