using BusinessLogic;
using DataBaseAccess;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("Live");

builder.Services.AddDbContext<CreditTrackingDbContext>(options => options.UseSqlServer(connectionString));

if (!double.TryParse(builder.Configuration.GetSection("NotificationServiceSettings")["NearFullUsePercentage"],
        out var nearFullUsePercentage) || nearFullUsePercentage < 0 || nearFullUsePercentage > 1)
{
    throw new InvalidOperationException("NearFullUsePercentage of invalid type or value");
}

builder.Services.AddSingleton(_ => new NotificationService(nearFullUsePercentage));
builder.Services.AddSingleton<AccessControlService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
