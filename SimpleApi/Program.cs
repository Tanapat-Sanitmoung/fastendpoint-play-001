using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFastEndpoints();
builder.Services.SwaggerDocument();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseSwaggerGen();

app.UseFastEndpoints();

app.Run();

