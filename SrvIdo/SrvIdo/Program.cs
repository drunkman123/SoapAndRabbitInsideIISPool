using Microsoft.AspNetCore.Connections;
using SrvIdo;
using SrvIDO.INFRAESTRUCTURE.DbInitialize;
using SrvIDO.DATA.Interfaces;
using SrvIDO.DATA.Service;
using SrvIDO.DATA.Repository;
using SrvIDO.DATA.RabbitMq;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()
                .AllowAnyHeader()
                .SetIsOriginAllowedToAllowWildcardSubdomains();
                      });
});
var config = builder.Configuration;

builder.Services.AddScoped<IQueriesRepository, QueriesRepository>();
builder.Services.AddSingleton<GetErro>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<IRabbitMqClient, RabbitMqClient>();
builder.Services.AddScoped<IIDOService,IDOService>();

builder.Services.AddSingleton<IDbConnectionFactory>(_ =>
    new DbConnectionFactory(config.GetValue<string>("ConnProdOp")!));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseSwagger(x => x.SerializeAsV2 = true);
}

app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next();
});

app.UseCors(MyAllowSpecificOrigins);

app.UseAuthorization();

app.MapControllers();
app.UseMiddleware<ExceptionMiddleware>();

app.Run();
