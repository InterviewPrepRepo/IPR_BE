using IPR_BE.Services;
using IPR_BE.DataAccess;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

var sinkOpts = new MSSqlServerSinkOptions();
sinkOpts.TableName = "Log";
var columnOpts = new ColumnOptions();
columnOpts.Store.Remove(StandardColumn.Properties);
columnOpts.Store.Add(StandardColumn.LogEvent);
columnOpts.AdditionalColumns = new List<SqlColumn> {
    new SqlColumn("Host", SqlDbType.NVarChar),
    new SqlColumn("StatusCode", SqlDbType.Int)
};

Serilog.Debugging.SelfLog.Enable(Console.Error);

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

app.UseAuthorization();

app.MapControllers();

app.Run();
