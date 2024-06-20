using Hangfire;
using Microsoft.EntityFrameworkCore;
using StackHub.Demo.Services.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add services to the container.
builder.Services.AddDbContext<DataContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection"));
});

//Hangfire
builder.Services.AddHangfire((sp, config) =>
{
    var connectionString = sp.GetRequiredService<IConfiguration>().GetConnectionString("DatabaseConnection");
    config.UseSqlServerStorage(connectionString);
});

builder.Services.AddHangfireServer();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    //Hangfire https://localhost:7276/hangfire
    app.MapHangfireDashboard();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
