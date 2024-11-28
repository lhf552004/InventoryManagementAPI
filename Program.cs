using Amazon.Lambda.AspNetCoreServer;

var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); // Logs to the console, which is captured by Docker

// Add services to the DI container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Use the logger
var logger = app.Logger;
logger.LogInformation("Application is starting...");

// Check if running in Lambda
var isLambda = Environment.GetEnvironmentVariable("AWS_EXECUTION_ENV")?.StartsWith("AWS_Lambda") == true;

logger.LogInformation("Is running in Lambda: {IsLambda}", isLambda);

// Configure middleware and HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!isLambda) // Only redirect to HTTPS outside Lambda
{
    app.UseHttpsRedirection();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();

// Entry point for AWS Lambda
public class LambdaEntryPoint : APIGatewayProxyFunction
{
    protected override void Init(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole(); // Ensure logs are sent to CloudWatch in Lambda
        });

        // Configure services and middleware for Lambda runtime
        builder.ConfigureServices(services =>
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
        });

        builder.Configure(app =>
        {
            if (app.ApplicationServices.GetRequiredService<IHostEnvironment>().IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Do not use HTTPS redirection in Lambda
            var isLambda = Environment.GetEnvironmentVariable("AWS_EXECUTION_ENV")?.StartsWith("AWS_Lambda") == true;
            var logger = app.ApplicationServices.GetRequiredService<ILogger<LambdaEntryPoint>>();
            logger.LogInformation("Is running in Lambda: {IsLambda}", isLambda);

            if (!isLambda)
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        });
    }
}
