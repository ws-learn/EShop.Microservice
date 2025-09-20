using BuildingBlocks.Behaviors;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddCarter();
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(config =>
{
    config.RegisterServicesFromAssembly(assembly);
    config.AddOpenBehavior(typeof(ValidationBehavior<,>));
    config.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);

}).UseLightweightSessions();

if (builder.Environment.IsDevelopment())
{
    builder.Services.InitializeMartenWith<CatalogInitialData>();
}

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddValidatorsFromAssembly(assembly);

builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!);

var app = builder.Build();

// Configure the HTTP request pipeline
app.MapCarter();

app.UseExceptionHandler(options => { });

app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

// app.UseExceptionHandler(exeptionHandlerApp =>
// {
//     exeptionHandlerApp.Run(async context =>
//     {
//         var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
//         if (exception is null)
//         {
//             return;
//         }
//         var problemDetails = new ProblemDetails
//         {
//             Title = exception.Message,
//             Status = StatusCodes.Status500InternalServerError,
//             Detail = exception.StackTrace
//         };
//         var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
//         logger.LogError(exception, exception.Message);
//         context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//         context.Response.ContentType = "application/problem+json";
//         await context.Response.WriteAsJsonAsync(problemDetails);
//     });
// });

app.Run();

