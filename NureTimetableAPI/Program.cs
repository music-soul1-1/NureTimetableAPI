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
    config.UseInMemoryStorage();
    config.UseSimpleAssemblyNameTypeSerializer();
    config.UseRecommendedSerializerSettings();
});
builder.Services.AddHangfireServer();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<ICistRepository, CistRepository>();
builder.Services.AddScoped<IPostgreSQLRepository, PostgreSQLRepository>();

builder.Services.AddDbContext<NureTimetableDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

var app = builder.Build();

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

app.UseHangfireDashboard("/hangfire", 
    new DashboardOptions
    {
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
                            Login = builder.Configuration.GetSection("Hangfire:Auth")["Login"],
                            PasswordClear = builder.Configuration.GetSection("Hangfire:Auth")["Password"],
                        }
                    ],
                }
            )
        ]
    }
);

RecurringJob.AddOrUpdate<CistGroupsStructureFetch>("cist-groups-structure-fetch",
            job => job.Execute(),
            Cron.Daily(1, 34));

RecurringJob.AddOrUpdate<CistTeachersStructureFetch>("cist-teachers-structure-fetch",
            job => job.Execute(),
            Cron.Daily(1, 35));

RecurringJob.AddOrUpdate<CistBuildingsStructureFetch>("cist-buildings-structure-fetch",
            job => job.Execute(),
            Cron.Daily(1, 36));

app.Run();
