using System.Text.Json;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.EntityFrameworkCore;
using RinhaBackend.Api.Endpoints;
using RinhaBackend.Api.Extensions;
using RinhaBackend.Infrastructure.Data;

var builder = WebApplication.CreateSlimBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContextPool<RinhaContext>(opt =>
    opt.UseNpgsql(connectionString), 256);

builder.Services.ConfigureHttpJsonOptions(opt =>
{
    opt.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
});

builder.Services.AddRepositories();
builder.Services.AddServices();

builder.Services.AddRequestTimeouts(options => options.DefaultPolicy = 
    new RequestTimeoutPolicy { Timeout = TimeSpan.FromSeconds(60) });

var app = builder.Build();

app.AddClientesEndpoints();

app.Run();