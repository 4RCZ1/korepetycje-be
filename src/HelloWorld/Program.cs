using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HelloWorld;


var builder = WebApplication.CreateBuilder(args);

// When running in Lambda, this environment variable is set
var isLambda = builder.Configuration["LAMBDA_RUNTIME_DIR"] != null;

if (isLambda)
{
    builder.WebHost.UseLambdaServer(); // 👈 tells it to use Lambda hosting
}

// Add services
builder.Services.AddDbContext<OurDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("InzyniergaCon")));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (!isLambda)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();