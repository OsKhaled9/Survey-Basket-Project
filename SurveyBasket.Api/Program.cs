using Hangfire;
using HangfireBasicAuthenticationFilter;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
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

            // using Background Jobs (Hangfire)
            app.UseHangfireDashboard("/jobs", new DashboardOptions()
            {
                Authorization =
                [
                    new HangfireCustomBasicAuthenticationFilter()
                    {
                        User = app.Configuration.GetValue<string>("HangfireSettings:UserName"),
                        Pass = app.Configuration.GetValue<string>("HangfireSettings:Password")
                    }
                ],
                DashboardTitle = "Survey Basket Dashboard",
                //IsReadOnlyFunc = (DashboardContext context) => true
            });

            var scopeFactory = app.Services.GetRequiredService<IServiceScopeFactory>();
            using var scope = scopeFactory.CreateScope();
            var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

            RecurringJob.AddOrUpdate("SendNewPollsNotification", () => notificationService.SendNewPollsNotification(null), Cron.Daily); // everyday on 12 am

            // using CORS
            app.UseCors();

            app.UseAuthorization();

            app.MapControllers();

            // Adding Custom global exception handling middleware
            app.UseExceptionHandler();

            // using Rate Limiting
            app.UseRateLimiter();

            // using health checks
            app.MapHealthChecks("health", new HealthCheckOptions()
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            //app.MapHealthChecks("health-check-api", new HealthCheckOptions()
            //{
            //    Predicate = x => x.Tags.Contains("api"),
            //    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            //});

            app.Run();
        }
    }
}
