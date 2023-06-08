using IPR_BE.Services;
using IPR_BE.DataAccess;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

//Configuring Serilog to log to Sql Server DB
MSSqlServerSinkOptions sinkOpts = new MSSqlServerSinkOptions();
sinkOpts.TableName = "Log";
ColumnOptions columnOpts = new ColumnOptions();
columnOpts.Store.Remove(StandardColumn.Properties);
columnOpts.Store.Add(StandardColumn.LogEvent);
columnOpts.AdditionalColumns = new List<SqlColumn> {
    new SqlColumn("Host", SqlDbType.NVarChar),
    new SqlColumn("StatusCode", SqlDbType.Int)
};

//Enabling serilog selflog for debugging serilog
Serilog.Debugging.SelfLog.Enable(Console.Error);

//Initializing the logger and have ASP.NET use Serilog to pipe their logs
Log.Logger = new LoggerConfiguration()
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("ReportsDB"),
        sinkOptions: sinkOpts,
        columnOptions: columnOpts
    )
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services to the container.
builder.Services.AddScoped<SMTPService>();
builder.Services.AddScoped<InterviewBotService>();
builder.Services.AddScoped<IMochaService>();
builder.Services.AddScoped<InterviewBotRepo>((ctx) => new InterviewBotRepo(builder.Configuration.GetConnectionString("InterviewBotDB")));
builder.Services.AddDbContext<TestReportDbContext>((options) => options.UseSqlServer(builder.Configuration.GetConnectionString("ReportsDB")));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
                      {
                          policy
                          .AllowAnyHeader()
                          .AllowAnyMethod()
                          .AllowAnyOrigin();
                      });
});

var app = builder.Build();

Log.Logger.Information("Application Starting");
// Configure the HTTP request pipeline.
// Leave Swagger on for prods too, for testing purposes
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
