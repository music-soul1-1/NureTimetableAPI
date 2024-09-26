using Microsoft.EntityFrameworkCore;
using NureTimetableAPI.Repositories;
using Hangfire;
using NureTimetableAPI.Jobs;
using NureTimetableAPI.Contexts;
using Hangfire.Dashboard.BasicAuthorization;
using Asp.Versioning;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetSection("ConnectionStringProduction").Value);
    config.UseSimpleAssemblyNameTypeSerializer();
    config.UseRecommendedSerializerSettings();

    GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute { Attempts = 2, DelaysInSeconds = [240] });
});
builder.Services.AddHangfireServer();

builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(2, 0);
    options.ReportApiVersions = true;
    options.ApiVersionReader = ApiVersionReader.Combine(
        new UrlSegmentApiVersionReader(),
        new QueryStringApiVersionReader("api-ver"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));

}).AddApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Version = "v2",
        Title = "NureTimetableAPI v2",
        Description = "API for NureTimetable",
        License = new OpenApiLicense
        {
            Name = "GNU GPL v3.0",
            Url = new Uri("https://www.gnu.org/licenses/gpl-3.0.html")
        }
    });

    options.DocInclusionPredicate((version, apiDescription) =>
    {
        if (apiDescription.GroupName == null) return true;
        return apiDescription.GroupName == version;
    });
});

builder.Services.AddCors(options => 
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddSingleton<ICistRepository, CistRepository>();
builder.Services.AddScoped<ISQLRepository, SQLRepository>();

builder.Services.AddDbContext<NureTimetableDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionStringProduction").Value, (options) =>
    {
        options.CommandTimeout(180);
    });
});

var app = builder.Build();

// Redirect to docs
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/" || context.Request.Path == "")
    {
        context.Response.Redirect("https://music-soul1-1.github.io/NureTimetableAPI.Docs/", true);
    }
    else
    {
        await next();
    }
});

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "NureTimetableAPI v2");
    options.EnableTryItOutByDefault();
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

//app.UseRouting();

app.UseCors();

app.UseHangfireDashboard("/hangfire", 
    new DashboardOptions
    {
        AppPath = "https://music-soul1-1.github.io/NureTimetableAPI.Docs/",
        DisplayStorageConnectionString = false,
        Authorization =
        [
            new BasicAuthAuthorizationFilter(
                new BasicAuthAuthorizationFilterOptions
                {
                    RequireSsl = false,
                    SslRedirect = false,
                    LoginCaseSensitive = true,
                    Users =
                    [
                        new BasicAuthAuthorizationUser
                        {
                            Login = builder.Configuration.GetSection("HangfireAuthLogin").Value,
                            PasswordClear = builder.Configuration.GetSection("HangfireAuthPassword").Value,
                        }
                    ],
                }
            )
        ]
    }
);

RecurringJob.AddOrUpdate<CistGroupsStructureFetch>("cist-groups-structure-fetch",
            job => job.Execute(),
            Cron.Monthly(1, 1, 30));

RecurringJob.AddOrUpdate<CistTeachersStructureFetch>("cist-teachers-structure-fetch",
            job => job.Execute(),
            Cron.Monthly(1, 1, 32));

RecurringJob.AddOrUpdate<CistBuildingsStructureFetch>("cist-buildings-structure-fetch",
            job => job.Execute(),
            Cron.Monthly(1, 1, 34));

RecurringJob.AddOrUpdate<KeepAliveJob>("keep-alive-1",
            job => job.Execute(),
            Cron.Hourly(30));

RecurringJob.AddOrUpdate<KeepAliveJob>("keep-alive-2",
            job => job.Execute(),
            Cron.Hourly(0));

RecurringJob.AddOrUpdate<KeepAliveJob>("keep-alive-3",
            job => job.Execute(),
            Cron.Hourly(15));

RecurringJob.AddOrUpdate<KeepAliveJob>("keep-alive-4",
            job => job.Execute(),
            Cron.Hourly(45));

app.Run();
