using Hangfire;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using SurveyBasket.Api.Authentication;
using SurveyBasket.Api.Health;
using SurveyBasket.Api.Settings;
using System.Text;
using System.Threading.RateLimiting;

namespace SurveyBasket.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // Add services to the container.
        services.AddControllers();

        // using caching
        services.AddHybridCache();

        services.AddCors(options =>
            options.AddDefaultPolicy(builder =>
                builder
                       .AllowAnyMethod()
                       .AllowAnyHeader()
                       .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
            )
        );

        services
            .AddSwaggerServicesConfiguration()
            .AddMapsterConfiguration()
            .AddFluentValidationConfiguration()
            .AddDbContextConfiguration(configuration)
            .AddAuthenticationConfiguration(configuration);

        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IQuestionService, QuestionService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IResultService, ResultService>();
        services.AddScoped<IEmailSender, EmailService>();
        services.AddScoped<INotificationService, NotificationService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IRoleService, RoleService>();

        // Add Custom Global Exception Handler
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        // using background jobs(hangfire)
        services.AddBackgroundJobsConfiguration(configuration);

        // using of IHttpContextAccessor
        services.AddHttpContextAccessor();

        services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));

        // using Health Check
        services.AddHealthChecks()
            .AddSqlServer(name: "database", connectionString: configuration.GetConnectionString("DefaultConnection")!)
            .AddHangfire(options => { options.MinimumAvailableServers = 1; })
            //.AddUrlGroup(name: "google api", uri: new Uri("https://www.google.com"), tags: ["api"], httpMethod: HttpMethod.Get)
            //.AddUrlGroup(name: "meta api", uri: new Uri("https://www.facebook.com"), tags: ["api"], httpMethod: HttpMethod.Get)
            .AddCheck<MailProviderHealthCheck>(name: "mail service");

        // uisng Rate Limiting
        services.AddRateLimitingConfiguration();

        return services;
    }

    private static IServiceCollection AddSwaggerServicesConfiguration(this IServiceCollection services)
    {
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    private static IServiceCollection AddMapsterConfiguration(this IServiceCollection services)
    {
        // Add Mapster Package
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(Assembly.GetExecutingAssembly());

        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }

    private static IServiceCollection AddFluentValidationConfiguration(this IServiceCollection services)
    {
        // Add Fluent Validation Package
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        return services;
    }

    private static IServiceCollection AddDbContextConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Dbcontext Configurations
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection String 'DefaultConnection' is not found!");

        services.AddDbContext<ApplicationDbContext>(option =>
            option.UseSqlServer(connectionString));

        return services;
    }

    private static IServiceCollection AddAuthenticationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        //services.AddIdentity<ApplicationUser, IdentityRole>()
        services.AddIdentity<ApplicationUser, ApplicationRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddTransient<IAuthorizationHandler, PermissionAuthorizationHandler>();
        services.AddTransient<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();

        services.AddSingleton<IJwtProvider, JwtProvider>();

        // Jwt Authentication Configuration
        //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var jwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(o =>
        {
            o.SaveToken = true;
            o.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!)),
                ValidIssuer = jwtSettings?.Issuer,
                ValidAudience = jwtSettings?.Audience
            };
        });

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequiredLength = 8;

            options.SignIn.RequireConfirmedEmail = true;

            options.User.RequireUniqueEmail = true;

            // Default lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        });

        return services;
    }

    private static IServiceCollection AddBackgroundJobsConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        // Add Hangfire services.
        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(configuration.GetConnectionString("HangfireConnection")));

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        return services;
    }

    private static IServiceCollection AddRateLimitingConfiguration(this IServiceCollection services)
    {
        services.AddRateLimiter(rateLimiterOptions =>
        {
            rateLimiterOptions.RejectionStatusCode = StatusCodes.Status429TooManyRequests; // instead of return 503 (Service UnAvailable) status code

            // IP Address Limiter (Custom Rate Limiting)
            rateLimiterOptions.AddPolicy(RateLimiters.IpLimiter, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.Connection.RemoteIpAddress?.ToString(), // IP Address for User
                    factory: _ => new FixedWindowRateLimiterOptions()
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromSeconds(20)
                    }
                )
            );


            // User Limiter
            rateLimiterOptions.AddPolicy(RateLimiters.UserLimiter, httpContext =>
                RateLimitPartition.GetFixedWindowLimiter(
                    partitionKey: httpContext.User.GetUserId(), // Id for User
                    factory: _ => new FixedWindowRateLimiterOptions()
                    {
                        PermitLimit = 2,
                        Window = TimeSpan.FromSeconds(20)
                    }
                )
            );

            // Concurrency Limiter
            rateLimiterOptions.AddConcurrencyLimiter(RateLimiters.Concurrency, options =>
            {
                options.PermitLimit = 1000; // receive 10 requests in the same time
                options.QueueLimit = 100; // receive 5 requests added in the queue                         
                options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            });


            // Token Bucket Limiter
            //rateLimiterOptions.AddTokenBucketLimiter("token", options =>
            //{
            //    options.TokenLimit = 2; // receive 100 requests in the same time
            //    options.QueueLimit = 1; // receive 5 requests added in the queue                         
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //    options.ReplenishmentPeriod = TimeSpan.FromSeconds(30); // will add 2 token every 30 seconds
            //    options.TokensPerPeriod = 2;
            //    options.AutoReplenishment = true;
            //});


            // Fixed Window Limiter
            //rateLimiterOptions.AddFixedWindowLimiter("fixed", options =>
            //{
            //    options.PermitLimit = 2; // receive 10 requests in the same time
            //    options.Window = TimeSpan.FromSeconds(20);
            //    options.QueueLimit = 1; // receive 5 requests added in the queue                         
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //});


            // Sliding Window Limiter
            //rateLimiterOptions.AddSlidingWindowLimiter("sliding", options =>
            //{
            //    options.PermitLimit = 2; // receive 10 requests in the same time
            //    options.Window = TimeSpan.FromSeconds(20);
            //    options.SegmentsPerWindow = 2;
            //    options.QueueLimit = 1; // receive 5 requests added in the queue                         
            //    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            //});
        });

        return services;
    }
}
