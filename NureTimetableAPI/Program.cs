using Microsoft.EntityFrameworkCore;
using NureTimetableAPI.Repositories;
using Hangfire;
using NureTimetableAPI.Jobs;
using NureTimetableAPI.Contexts;
using Hangfire.Dashboard.BasicAuthorization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetSection("ConnectionStringProduction").Value);
    config.UseSimpleAssemblyNameTypeSerializer();
    config.UseRecommendedSerializerSettings();
});
builder.Services.AddHangfireServer();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
    options.UseSqlServer(builder.Configuration.GetSection("ConnectionStringProduction").Value);
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
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

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

app.MapGet("/keepalive", () => "I'm alive!");

RecurringJob.AddOrUpdate<CistGroupsStructureFetch>("cist-groups-structure-fetch",
            job => job.Execute(),
            Cron.Weekly(DayOfWeek.Sunday, 1, 30));

RecurringJob.AddOrUpdate<CistTeachersStructureFetch>("cist-teachers-structure-fetch",
            job => job.Execute(),
            Cron.Weekly(DayOfWeek.Sunday, 1, 32));

RecurringJob.AddOrUpdate<CistBuildingsStructureFetch>("cist-buildings-structure-fetch",
            job => job.Execute(),
            Cron.Weekly(DayOfWeek.Sunday, 1, 34));

RecurringJob.AddOrUpdate<KeepAliveJob>("keep-alive-1",
            job => job.Execute(),
            Cron.Hourly(30));

RecurringJob.AddOrUpdate<KeepAliveJob>("keep-alive-2",
            job => job.Execute(),
            Cron.Hourly(0));

app.Run();
