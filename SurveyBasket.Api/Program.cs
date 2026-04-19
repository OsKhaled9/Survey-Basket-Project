using Serilog;

namespace SurveyBasket.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // using Dependency Injection Class
            builder.Services.AddDependencies(builder.Configuration);

            // using serilog for logging
            builder.Host.UseSerilog((context, configuration) =>
                configuration.ReadFrom.Configuration(context.Configuration)
            );

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(option => option.SwaggerEndpoint("/openapi/v1.json", "v1"));
            }

            // using serilog for logging
            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            // Adding Custom global exception handling middleware
            app.UseExceptionHandler();

            app.Run();
        }
    }
}
